using Microsoft.Extensions.FileProviders;

namespace Manabu.UI.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseStaticFilesEx(this WebApplication app, string directoryName)
    {
        var currentDir = Directory.GetCurrentDirectory();
        var targetDir = Path.Combine(currentDir, directoryName);
        if (!Directory.Exists(targetDir)) 
            Directory.CreateDirectory(targetDir);

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(targetDir),
            RequestPath = $"/{directoryName}"
        });

        return app;
    }
}
