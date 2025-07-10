using API.Data;
using API.Models;
using API.Services;
using System.Collections.Generic;
using System.Text;

namespace API.Functions
{
    public class MyFuncions
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;
        //private readonly LogService ApiLog;
        public MyFuncions(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
        }

        public async Task<string?> CallExternalApiAsync(string endpoint, StringContent content)
        {
            try
            {
                var bearerToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

                if (string.IsNullOrEmpty(bearerToken))
                {
                    Console.WriteLine("El token de autorización no está disponible.");
                    return null;
                }

                // ⚠️ Aceptar certificados autofirmados solo en desarrollo
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };

                using var httpClient = new HttpClient(handler);

                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken.Replace("Bearer ", ""));

                string body = await content.ReadAsStringAsync();
                string curl = $"curl -X POST \"{endpoint}\" \\\n" +
                              $"  -H \"Authorization: Bearer {bearerToken.Replace("Bearer ", "")}\" \\\n" +
                              $"  -H \"Content-Type: {content.Headers.ContentType?.MediaType}\" \\\n" +
                              $"  -d '{body}'";

                Console.WriteLine($"CURL generado para debug:\n{curl}");

                if (string.IsNullOrWhiteSpace(body))
                {
                    Console.WriteLine("El body está vacío. No se enviará contenido.");
                    return null;
                }

                var newContent = new StringContent(body, Encoding.UTF8, content.Headers.ContentType?.MediaType ?? "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(endpoint, newContent);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine($"Error al llamar a la API externa: {response.StatusCode}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excepción al llamar a la API externa: {ex.Message}");
                throw;
            }
        }
    }
}
