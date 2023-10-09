namespace Manabu.Infrastructure;

public static class MongoConfig
{
    public static string? ConnectionString => Environment.GetEnvironmentVariable("KitsuneDatabaseConn");
}
