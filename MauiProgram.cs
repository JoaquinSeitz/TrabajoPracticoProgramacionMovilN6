using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace TrabajoPracticoProgramacionMovilN6
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Inyección obligatoria de MudBlazor
            builder.Services.AddMudServices();

            // --- CONFIGURACIÓN DEL HTTP CLIENT (CON BYPASS SSL E INTERCEPTOR) ---
            string urlApi = "https://localhost:7133";
#if ANDROID
            // Android requiere esta IP especial para salir al localhost de tu PC
            urlApi = "https://10.0.2.2:7133";
#endif

            // 1. Registramos el interceptor global de errores
            builder.Services.AddScoped<Services.GlobalHttpInterceptor>();

            // 2. Configuramos el HttpClient para que use el interceptor y el bypass SSL
            builder.Services.AddScoped(sp =>
            {
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };

                // Enganchamos el interceptor al handler base
                var interceptor = sp.GetRequiredService<Services.GlobalHttpInterceptor>();
                interceptor.InnerHandler = handler;

                return new HttpClient(interceptor)
                {
                    BaseAddress = new Uri(urlApi)
                };
            });

            // Habilitar la seguridad y autorización en Blazor
            builder.Services.AddAuthorizationCore();

            // Inyectar nuestros servicios personalizados
            builder.Services.AddScoped<Services.AuthService>();
            // Lo registramos como Scoped (o Singleton) para que mantenga su estado mientras la app esté abierta
            builder.Services.AddScoped<Services.CarritoService>();
            builder.Services.AddScoped<Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider, Services.CustomAuthStateProvider>();


            return builder.Build();
        }
    }
}