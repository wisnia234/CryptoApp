using BlazorDownloadFile;
using MudBlazor.Services;

namespace CryptoApp.Client.Extensions;

internal static class Extension
{
    public static bool IsBase64String(this string value)
    {
        Span<byte> buffer = new Span<byte>(new byte[value.Length]);
        return Convert.TryFromBase64String(value, buffer, out int _);
    }

    public static IServiceCollection AddClientServices(this IServiceCollection services)
    {
        services.AddMudServices();
        services.AddBlazorDownloadFile();
        return services;
    }
}
