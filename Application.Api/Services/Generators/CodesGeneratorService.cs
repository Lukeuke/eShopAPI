namespace Application.Api.Services.Generators;

public sealed class CodesGeneratorService : IDisposable
{
    public Dictionary<int, string> Codes { get; set; }
    
    private CodesGeneratorService()
    {
        Codes = new Dictionary<int, string>();
    }

    ~CodesGeneratorService()
    {
        ReleaseUnmanagedResources();
    }
    
    private static CodesGeneratorService _instance;
    
    private static readonly object _lock = new ();
    
    public static CodesGeneratorService GetInstance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new CodesGeneratorService();
                }
            }
        }
        return _instance;
    }

    private void ReleaseUnmanagedResources()
    {
        Codes.Clear();
        Codes = null!;
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }
}