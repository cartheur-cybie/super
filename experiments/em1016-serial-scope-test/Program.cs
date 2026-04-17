using System.Globalization;
using System.IO.Ports;
using System.Text;

CliOptions options;
try
{
    options = CliOptions.Parse(args);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Argument error: {ex.Message}");
    Console.Error.WriteLine("Use --help for usage.");
    return 2;
}

if (options.ShowHelp)
{
    CliOptions.PrintHelp();
    return 0;
}

if (options.ListPorts)
{
    var names = SerialPort.GetPortNames().OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToArray();
    Console.WriteLine(names.Length == 0 ? "No serial ports found." : "Serial ports:");
    foreach (var name in names)
    {
        Console.WriteLine($"  {name}");
    }

    return 0;
}

if (string.IsNullOrWhiteSpace(options.PortName))
{
    Console.Error.WriteLine("Missing required --port argument. Use --help for usage.");
    return 2;
}

var stopBits = options.StopBits switch
{
    1 => StopBits.One,
    2 => StopBits.Two,
    _ => throw new InvalidOperationException("Unsupported stop bits value.")
};

var parity = options.Parity switch
{
    "none" => Parity.None,
    "even" => Parity.Even,
    "odd" => Parity.Odd,
    "mark" => Parity.Mark,
    "space" => Parity.Space,
    _ => throw new InvalidOperationException("Unsupported parity value.")
};

using var serial = new SerialPort(options.PortName, options.BaudRate, parity, options.DataBits, stopBits)
{
    Handshake = Handshake.None,
    ReadTimeout = 100,
    WriteTimeout = 1000,
    Encoding = Encoding.ASCII
};

try
{
    serial.Open();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Failed to open {options.PortName}: {ex.Message}");
    return 1;
}

Console.WriteLine($"Opened {options.PortName} ({options.BaudRate} {options.DataBits}{(char.ToUpperInvariant(options.Parity[0]))}{options.StopBits})");
Console.WriteLine($"Mode={options.Mode} Interval={options.IntervalMs}ms Repeat={(options.RepeatCount == 0 ? "infinite" : options.RepeatCount.ToString(CultureInfo.InvariantCulture))}");
Console.WriteLine("Press Ctrl+C to stop.");

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

var runStart = DateTimeOffset.UtcNow;
var rxTask = options.Mode is RunMode.TxOnly
    ? Task.CompletedTask
    : ReceiveLoopAsync(serial, runStart, cts.Token);

var txTask = options.Mode is RunMode.RxOnly
    ? Task.CompletedTask
    : TransmitLoopAsync(serial, options, runStart, cts.Token);

try
{
    await Task.WhenAll(txTask, rxTask);
}
catch (OperationCanceledException)
{
    // Expected on Ctrl+C.
}
finally
{
    if (serial.IsOpen)
    {
        serial.Close();
    }
}

Console.WriteLine("Stopped.");
return 0;

static async Task TransmitLoopAsync(SerialPort serial, CliOptions options, DateTimeOffset runStart, CancellationToken ct)
{
    var packetCount = 0;
    while (!ct.IsCancellationRequested)
    {
        packetCount++;
        var payload = options.Pattern switch
        {
            TxPattern.Alt55Aa => BuildAlt55AaPayload(options),
            TxPattern.Text => BuildTextPayload(packetCount),
            TxPattern.Hex => options.HexPayload,
            _ => throw new InvalidOperationException("Unsupported tx pattern.")
        };

        serial.Write(payload, 0, payload.Length);
        WriteTxLog(payload, runStart, packetCount);

        if (options.RepeatCount > 0 && packetCount >= options.RepeatCount)
        {
            break;
        }

        await Task.Delay(options.IntervalMs, ct);
    }
}

static byte[] BuildAlt55AaPayload(CliOptions options)
{
    if (options.BurstLength <= 0)
    {
        return [0x55, 0xAA];
    }

    var bytes = new byte[options.BurstLength];
    for (var i = 0; i < bytes.Length; i++)
    {
        bytes[i] = i % 2 == 0 ? (byte)0x55 : (byte)0xAA;
    }

    return bytes;
}

static byte[] BuildTextPayload(int sequence)
{
    var text = $"BETIX-PING {sequence:D4}\\r\\n";
    return Encoding.ASCII.GetBytes(text);
}

static void WriteTxLog(byte[] payload, DateTimeOffset runStart, int packetCount)
{
    var elapsed = DateTimeOffset.UtcNow - runStart;
    Console.WriteLine($"TX #{packetCount:D4} t+{elapsed.TotalMilliseconds,8:F1}ms {FormatHex(payload)}");
}

static async Task ReceiveLoopAsync(SerialPort serial, DateTimeOffset runStart, CancellationToken ct)
{
    var buffer = new byte[1024];
    while (!ct.IsCancellationRequested)
    {
        int read;
        try
        {
            read = await serial.BaseStream.ReadAsync(buffer.AsMemory(0, buffer.Length), ct);
        }
        catch (TimeoutException)
        {
            continue;
        }

        if (read <= 0)
        {
            continue;
        }

        var chunk = buffer.AsSpan(0, read).ToArray();
        var elapsed = DateTimeOffset.UtcNow - runStart;
        Console.WriteLine($"RX      t+{elapsed.TotalMilliseconds,8:F1}ms {FormatHex(chunk)} |{FormatAscii(chunk)}|");
    }
}

static string FormatHex(ReadOnlySpan<byte> bytes)
{
    return string.Join(' ', bytes.ToArray().Select(b => b.ToString("X2", CultureInfo.InvariantCulture)));
}

static string FormatAscii(ReadOnlySpan<byte> bytes)
{
    var chars = new char[bytes.Length];
    for (var i = 0; i < bytes.Length; i++)
    {
        var b = bytes[i];
        chars[i] = b is >= 32 and <= 126 ? (char)b : '.';
    }

    return new string(chars);
}

enum RunMode
{
    TxRx,
    TxOnly,
    RxOnly
}

enum TxPattern
{
    Alt55Aa,
    Text,
    Hex
}

sealed class CliOptions
{
    public bool ShowHelp { get; private set; }
    public bool ListPorts { get; private set; }
    public string? PortName { get; private set; }
    public int BaudRate { get; private set; } = 115200;
    public int DataBits { get; private set; } = 8;
    public int StopBits { get; private set; } = 1;
    public string Parity { get; private set; } = "none";
    public RunMode Mode { get; private set; } = RunMode.TxRx;
    public TxPattern Pattern { get; private set; } = TxPattern.Alt55Aa;
    public int IntervalMs { get; private set; } = 250;
    public int RepeatCount { get; private set; } = 0;
    public int BurstLength { get; private set; } = 32;
    public byte[] HexPayload { get; private set; } = [0x55, 0xAA, 0x00, 0xFF];

    public static CliOptions Parse(string[] args)
    {
        var options = new CliOptions();

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "-h":
                case "--help":
                    options.ShowHelp = true;
                    break;
                case "--list":
                    options.ListPorts = true;
                    break;
                case "--port":
                    options.PortName = GetValue(args, ref i, arg);
                    break;
                case "--baud":
                    options.BaudRate = ParseInt(GetValue(args, ref i, arg), arg, min: 1200);
                    break;
                case "--data-bits":
                    options.DataBits = ParseInt(GetValue(args, ref i, arg), arg, min: 5, max: 8);
                    break;
                case "--stop-bits":
                    options.StopBits = ParseInt(GetValue(args, ref i, arg), arg, min: 1, max: 2);
                    break;
                case "--parity":
                    options.Parity = GetValue(args, ref i, arg).ToLowerInvariant();
                    ValidateOneOf(options.Parity, arg, ["none", "even", "odd", "mark", "space"]);
                    break;
                case "--mode":
                    var mode = GetValue(args, ref i, arg).ToLowerInvariant();
                    options.Mode = mode switch
                    {
                        "txrx" => RunMode.TxRx,
                        "tx" => RunMode.TxOnly,
                        "rx" => RunMode.RxOnly,
                        _ => throw new ArgumentException($"Invalid {arg} value '{mode}'. Use txrx, tx, or rx.")
                    };
                    break;
                case "--pattern":
                    var pattern = GetValue(args, ref i, arg).ToLowerInvariant();
                    options.Pattern = pattern switch
                    {
                        "alt55aa" => TxPattern.Alt55Aa,
                        "text" => TxPattern.Text,
                        "hex" => TxPattern.Hex,
                        _ => throw new ArgumentException($"Invalid {arg} value '{pattern}'. Use alt55aa, text, or hex.")
                    };
                    break;
                case "--interval-ms":
                    options.IntervalMs = ParseInt(GetValue(args, ref i, arg), arg, min: 1);
                    break;
                case "--repeat":
                    options.RepeatCount = ParseInt(GetValue(args, ref i, arg), arg, min: 0);
                    break;
                case "--burst-length":
                    options.BurstLength = ParseInt(GetValue(args, ref i, arg), arg, min: 2, max: 4096);
                    break;
                case "--hex":
                    options.HexPayload = ParseHexPayload(GetValue(args, ref i, arg));
                    break;
                default:
                    throw new ArgumentException($"Unknown argument: {arg}");
            }
        }

        return options;
    }

    public static void PrintHelp()
    {
        Console.WriteLine("EM1016 serial scope test tool");
        Console.WriteLine();
        Console.WriteLine("Usage:");
        Console.WriteLine("  dotnet run -- --list");
        Console.WriteLine("  dotnet run -- --port /dev/ttyUSB0 [options]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine("  --list                    List serial ports and exit");
        Console.WriteLine("  --port <name>             Serial device path, e.g. /dev/ttyUSB0");
        Console.WriteLine("  --baud <n>                Baud rate (default 115200)");
        Console.WriteLine("  --data-bits <5..8>        Data bits (default 8)");
        Console.WriteLine("  --stop-bits <1|2>         Stop bits (default 1)");
        Console.WriteLine("  --parity <none|even|odd|mark|space>  Parity (default none)");
        Console.WriteLine("  --mode <txrx|tx|rx>       Run mode (default txrx)");
        Console.WriteLine("  --pattern <alt55aa|text|hex> TX pattern in tx/txrx mode");
        Console.WriteLine("  --burst-length <n>        alt55aa pattern payload size (default 32)");
        Console.WriteLine("  --hex \"55 AA 00 FF\"       Hex payload for --pattern hex");
        Console.WriteLine("  --interval-ms <n>         Delay between TX packets (default 250)");
        Console.WriteLine("  --repeat <n>              Number of TX packets; 0 = infinite (default 0)");
        Console.WriteLine();
        Console.WriteLine("Examples:");
        Console.WriteLine("  dotnet run -- --list");
        Console.WriteLine("  dotnet run -- --port /dev/ttyUSB0 --mode tx --pattern alt55aa --repeat 50 --interval-ms 100");
        Console.WriteLine("  dotnet run -- --port /dev/ttyUSB0 --mode txrx --pattern text --baud 57600");
        Console.WriteLine("  dotnet run -- --port COM5 --mode tx --pattern hex --hex \"DE AD BE EF\" --repeat 10");
    }

    private static string GetValue(string[] args, ref int i, string arg)
    {
        if (i + 1 >= args.Length)
        {
            throw new ArgumentException($"Missing value for {arg}");
        }

        i++;
        return args[i];
    }

    private static int ParseInt(string value, string arg, int min, int max = int.MaxValue)
    {
        if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
        {
            throw new ArgumentException($"Invalid integer for {arg}: '{value}'");
        }

        if (parsed < min || parsed > max)
        {
            throw new ArgumentOutOfRangeException(arg, $"Value must be in range {min}..{max}.");
        }

        return parsed;
    }

    private static byte[] ParseHexPayload(string text)
    {
        var tokens = text.Split(new[] { ' ', ',', ':' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0)
        {
            throw new ArgumentException("Hex payload is empty.");
        }

        var bytes = new byte[tokens.Length];
        for (var i = 0; i < tokens.Length; i++)
        {
            var token = tokens[i].StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                ? tokens[i][2..]
                : tokens[i];

            if (!byte.TryParse(token, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var b))
            {
                throw new ArgumentException($"Invalid hex byte '{tokens[i]}'.");
            }

            bytes[i] = b;
        }

        return bytes;
    }

    private static void ValidateOneOf(string value, string arg, string[] valid)
    {
        if (!valid.Contains(value, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Invalid {arg} value '{value}'. Valid: {string.Join(", ", valid)}");
        }
    }
}
