﻿using NLog;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var sec = config.GetSection("NLog");
LogManager.Configuration = new NLogLoggingConfiguration(sec);
NLog.Logger log = LogManager.GetCurrentClassLogger();

var cfg = config.GetSection("Monitor");

log.Info($"Starting Serial Monitor on port {cfg["port"]}");

string port;
uint? delayMS = null;

if (cfg == null)
{
    log.Error("Missing configuration");
    return;
}

if (cfg["port"] == null)
{
    log.Error("ComPort is not specified");
    return;
}
else
{
    port = cfg["port"]!;
}

if (cfg["delayMS"] != null)
{
    delayMS = uint.Parse(cfg["delayMS"]!);
}


TelegramParser parser = new();
parser.NewTelegram += (tel) =>
{
    //log.Info(Convert.ToHexString(tel));
    System.Text.StringBuilder hex = new();
    foreach (byte b in tel)
        hex.AppendFormat("{0:X2} ", b);
    log.Info(hex);
};

parser.ParseFile(@"C:\Users\akurzmann\Desktop\suso_trace3.bin");

#if false
SerialMonitor monitor = new(port, delayMS);

// Register CTRL+C
Console.CancelKeyPress += delegate
{
    log.Info("Exiting");
    monitor.Stop();
};


// Start reading monitor
try
{
    monitor.Start();
}
catch (System.IO.IOException ex)
{
    log.Fatal(ex, "Could not start monitor");
    return;
}


while (true) { }
#endif