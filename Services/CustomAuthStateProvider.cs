using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TrabajoPracticoProgramacionMovilN6.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Buscamos TODOS los datos de sesión guardados
            var token = await SecureStorage.Default.GetAsync("jwt_token");
            var nombre = await SecureStorage.Default.GetAsync("user_name");
            var rol = await SecureStorage.Default.GetAsync("user_role");
            var id = await SecureStorage.Default.GetAsync("user_id");       // Faltaba
            var email = await SecureStorage.Default.GetAsync("user_email"); // Faltaba

            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(token))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, nombre ?? "Usuario"),
                    new Claim(ClaimTypes.Role, rol ?? "Cliente"),
                    // AHORA SÍ CARGAMOS EL ID Y EL EMAIL A LA IDENTIDAD:
                    new Claim(ClaimTypes.NameIdentifier, id ?? "1"),
                    new Claim(ClaimTypes.Email, email ?? "")
                }, "jwt");
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        // Le pusimos valores por defecto (id="" y email="") para que no se te rompa tu Login.razor actual, 
        // pero lo ideal será que luego le pases todos los datos.
        public void MarcarComoAutenticado(string nombre, string rol, string id = "", string email = "")
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, nombre),
                new Claim(ClaimTypes.Role, rol),
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Email, email)
            }, "jwt");

            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarcarComoDeslogueado()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}