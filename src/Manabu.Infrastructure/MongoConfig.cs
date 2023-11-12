namespace Manabu.Infrastructure;

public static class MongoConfig
{
    public static string? ConnectionString =>
        "mongodb+srv://admin:j6Nk%2AS%40R54dG@cluster0.raga6o9.mongodb.net";

    public static string? GetDatabaseName(Func<bool> isDevelopment) => 
        isDevelopment() ? "Kitsune_dev" : "Kitsune_prod";
}
