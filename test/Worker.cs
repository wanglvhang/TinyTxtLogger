using System.Diagnostics;

namespace test
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHost _host;
        private readonly HostBuilderContext _ctx;

        public Worker(ILogger<Worker> logger,IHost host, HostBuilderContext ctx)
        {
            _logger = logger;
            _host = host;
            _ctx = ctx;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var sw = new Stopwatch();
                sw.Start();
                testLog();
                sw.Stop();
                Console.WriteLine("worker:" + sw.ElapsedTicks.ToString());

                await Task.Delay(800, stoppingToken);
            }
        }

        private void testLog()
        {

            _logger.LogInformation("infomation log test");
            _logger.LogWarning("警告日志，中文字符测试");
            _logger.LogDebug("debug log test");
            _logger.LogError("error log test {time}", DateTimeOffset.Now);
            _logger.LogCritical("critical running at: {time}", DateTimeOffset.Now);
            _logger.LogTrace("trace log test running at: {time}", DateTimeOffset.Now);

            _logger.LogError(new EventId(111, "testid"), "event id test {time}", DateTimeOffset.Now);
            _logger.LogError(new Exception("logger test exception"),"exception message");
            _logger.LogCritical(new EventId(222), new Exception("critical exception"), "happen at event id test {time}", DateTimeOffset.Now);

        }
    }
}