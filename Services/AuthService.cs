using System.Net.Http.Json;

namespace TrabajoPracticoProgramacionMovilN6.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para enviar las credenciales a la API
        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginData = new { Email = email, Password = password };

            // Hacemos el POST a tu endpoint
            var response = await _httpClient.PostAsJsonAsync("/api/Usuarios/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                // Mapeamos la respuesta completa según tu JSON
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

                if (result != null && !string.IsNullOrEmpty(result.Token))
                {
                    // Guardamos el token en la bóveda segura del celular
                    await SecureStorage.Default.SetAsync("jwt_token", result.Token);

                    // (Opcional) Podés guardar el nombre y rol si querés usarlos rápido en la UI sin decodificar el token
                    await SecureStorage.Default.SetAsync("user_name", result.Nombre);
                    await SecureStorage.Default.SetAsync("user_role", result.Rol);

                    // Inyectamos el token en el HttpClient para futuras peticiones
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result.Token);

                    return true;
                }
            }
            return false;
        }

        // Método para cerrar sesión
        public void Logout()
        {
            SecureStorage.Default.Remove("jwt_token");
            SecureStorage.Default.Remove("user_name");
            SecureStorage.Default.Remove("user_role");
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }

    // Clase auxiliar actualizada con la estructura exacta de tu API
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
    }
}