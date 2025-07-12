namespace Lab2025.Data;

public class ConnectionStrings : IConnectionStrings
{
    private readonly IConfiguration _config;

    public ConnectionStrings(IConfiguration config)
    {
        _config = config;
    }

    public string? Get(string name)
    {
        return _config[$"ConnectionString:{name}"];
    }
}