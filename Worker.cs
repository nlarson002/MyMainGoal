namespace App.WindowsService;

public sealed class Worker : BackgroundService
{
    private readonly FileWatcherService _fileWatcher;
    private readonly ILogger<Worker> _logger;

    static readonly HttpClient client = new HttpClient();

    public Worker(
        FileWatcherService fileWatcher,
         ILogger<Worker> logger) =>
        (_fileWatcher, _logger) = (fileWatcher, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogWarning("File Watcher Started");
        _fileWatcher.Start();
        while (!stoppingToken.IsCancellationRequested)
        {
            //     using HttpResponseMessage response = await client.GetAsync("https://www.google.com");
            //     response.EnsureSuccessStatusCode();
            //     string responseBody = await response.Content.ReadAsStringAsync();
            //     _logger.LogWarning(responseBody);
            //     await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
        _fileWatcher.Dispose();
    }
}
