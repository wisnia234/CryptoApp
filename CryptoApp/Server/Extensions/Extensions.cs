using CryptoApp.Server.Middleware;
using CryptoApp.Server.Services;
using CryptoApp.Server.Services.Interfaces;
using System.Globalization;

namespace CryptoApp.Server.Extensions;

internal static class Extensions
{
    public static IServiceCollection AddCryptoServices(this IServiceCollection services)
    {
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<ISignatureService, SignatureService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddSingleton<ExceptionMiddleware>();
        services.AddScoped<IHashService, HashService>();
        return services;
    }

    public static WebApplication AddWebApplication(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();


        app.MapRazorPages();
        app.MapControllers();
        app.MapFallbackToFile("index.html");
        return app;
    }
    public static string ToEnglishMessage(this Exception ex)
    {
        CultureInfo oldCI = Thread.CurrentThread.CurrentCulture;
        string englishExceptionMessage = ex.Message;
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
        try
        {
            var objectType = Type.GetType(ex.GetType().FullName);
            var instantiatedObject = Activator.CreateInstance(objectType);
            throw (Exception)instantiatedObject;
        }
        catch (Exception e)
        {
            englishExceptionMessage = e.Message;
        }
        Thread.CurrentThread.CurrentCulture = oldCI;
        Thread.CurrentThread.CurrentUICulture = oldCI;
        return englishExceptionMessage;
    }

}
