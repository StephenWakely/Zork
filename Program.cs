using System;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Datadog.Logs;

namespace HelloWorld
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new DatadogConfiguration(url: "https://http-intake.logs.datad0g.com", maxRetries: 3);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File("log.txt",
                              rollingInterval: RollingInterval.Day,
                              rollOnFileSizeLimit: true)
                .WriteTo.DatadogLogs("<api key>",
                                     logLevel: LogEventLevel.Verbose,
                                     source: "zogwobblesource",
                                     service: "zogwobbleservice",
                                     host: "zogwobblehost",
                                     tags: new[] { "foo:bar" },
                                     batchSizeLimit: 10,
                                     batchPeriod: new TimeSpan(0,0,5),
                                     configuration: config)
                .CreateLogger();

            try
            {
                // Create an array of 10 famous peoples names
                string[] names = { "Bill Gates", "Steve Jobs", "Elon Musk", "Jeff Bezos", "Warren Buffet", "Mark Zuckerberg", "Larry Page", "Sergey Brin", "Larry Ellison", "Michael Bloomberg" };

                // Create an array of 10 visa card numbers
                string[] visaCardNumbers = { "4111 1111 1111 1111", "4111 1111 1111 1112", "4111 1111 1111 1113", "4111 1111 1111 1114", "4111 1111 1111 1115", "4111 1111 1111 1116", "4111 1111 1111 1120" };

                Random random = new Random();
                for (int i = 0; i < names.Length; i++)
                {
                    Log.Information($"Hello, {names[i]} your card {visaCardNumbers[random.Next(0, visaCardNumbers.Length)]}");
                    Log.Information($"Groovy information {i}");
                    Log.Verbose($"Groovy verbose {i}");
                    Log.Debug($"Funky debug {i}");
                    Log.Warning($"Funky warning {i}");
                    // Log.Information($"card 4009916871755567");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unhandled exception");
            }
            finally
            {
                await Log.CloseAndFlushAsync(); // ensure all logs written before app exits
            }
        }
    }
}

