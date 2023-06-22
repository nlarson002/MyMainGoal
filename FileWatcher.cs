public sealed class FileWatcherService : IDisposable
{

    static readonly HttpClient client = new HttpClient();
    private readonly ILogger<FileWatcherService> _logger;
    private FileSystemWatcher? watcher = null;
    public FileWatcherService(ILogger<FileWatcherService> logger)
    {
        _logger = logger;
    }

    public void Start()
    {
        watcher = new FileSystemWatcher(@"C:\Users\NickT\Desktop");

        watcher.NotifyFilter = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;

        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;
        watcher.Error += OnError;

        watcher.Filter = "*.txt";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;
    }

    private async void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            var response = await GetResponse();
            _logger.LogWarning($"CREATED FILE:{e.Name}");
            _logger.LogWarning($"CALLED:\r\n{response}");
        }
    }

    private async void OnCreated(object sender, FileSystemEventArgs e)
    {
        var response = await GetResponse();
        _logger.LogWarning($"CREATED FILE:{e.Name}");
        _logger.LogWarning($"CALLED:\r\n{response}");
    }

    private async void OnDeleted(object sender, FileSystemEventArgs e)
    {
        var response = await GetResponse();
        _logger.LogWarning($"CREATED FILE:{e.Name}");
        _logger.LogWarning($"CALLED:\r\n{response}");
    }


    private async void OnRenamed(object sender, RenamedEventArgs e)
    {
        var response = await GetResponse();
        _logger.LogWarning($"CREATED FILE:{e.Name}");
        _logger.LogWarning($"CALLED:\r\n{response}");
    }

    private static async Task<string> GetResponse()
    {
        var response = await client.GetAsync("http://www.google.com");
        var result = await response.Content.ReadAsStringAsync();
        return result;
    }

    private static void OnError(object sender, ErrorEventArgs e) =>
        PrintException(e.GetException());

    private static void PrintException(Exception? ex)
    {
        if (ex != null)
        {
            Console.WriteLine($"Message: {ex.Message}");
            Console.WriteLine("Stacktrace:");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine();
            PrintException(ex.InnerException);
        }
    }

    public void Dispose()
    {
        watcher?.Dispose();
    }
}