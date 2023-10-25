namespace Manabu.Infrastructure;

public static class MongoConfig
{
    public static string? ConnectionString =>
        Environment.GetEnvironmentVariable("KitsuneDatabaseConn") ?? "mongodb://localhost:271017/";

    public static string? GetDatabaseName(Func<bool> isDevelopment) => 
        isDevelopment() ? "Kitsune_dev" : "Kitsune_prod";
}
