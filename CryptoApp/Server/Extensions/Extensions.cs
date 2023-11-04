using CryptoApp.Server.Data;
using CryptoApp.Server.Middleware;
using CryptoApp.Server.Models;
using CryptoApp.Server.Services;
using CryptoApp.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CryptoApp.Server.Extensions;

internal static class Extensions
{
    private static IServiceCollection AddCryptoServices(this IServiceCollection services)
    {
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<ISignatureService, SignatureService>();
        services.AddScoped<ICertificateService, CertificateService>();
        services.AddSingleton<ExceptionMiddleware>();
        services.AddScoped<IHashService, HashService>();
        return services;
    }

    public static IServiceCollection AddServerExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCryptoServices();
        services.AddDbAndIdentity(configuration);

        return services;
    }

    private static IServiceCollection AddDbAndIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();
        services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredLength = 10;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddControllersWithViews();
        services.AddRazorPages();
        return services;
    }
    public static WebApplication AddWebApplication(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();

        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseIdentityServer();
        app.UseAuthorization();


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
