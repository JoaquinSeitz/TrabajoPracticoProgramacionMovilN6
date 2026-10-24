using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TrabajoPracticoProgramacionMovilN6.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Buscamos en la bóveda de MAUI si hay datos de sesión guardados
            var token = await SecureStorage.Default.GetAsync("jwt_token");
            var nombre = await SecureStorage.Default.GetAsync("user_name");
            var rol = await SecureStorage.Default.GetAsync("user_role");

            var identity = new ClaimsIdentity();

            // Si el token existe, construimos la identidad del usuario con su rol real (Admin o Cliente)
            if (!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nombre ?? "Usuario"),
                    new Claim(ClaimTypes.Role, rol ?? "Cliente")
                }, "jwt");
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        // Método que llamaremos desde la pantalla de Login para actualizar la interfaz
        public void MarcarComoAutenticado(string nombre, string rol)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Role, rol)
            }, "jwt");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        // Método que llamaremos al presionar "Cerrar Sesión"
        public void MarcarComoDeslogueado()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}