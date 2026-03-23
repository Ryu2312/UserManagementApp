using System.Text.Json.Serialization;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using UserManagementApp.Modules.Auth.Application.Models;
using UserManagementApp.Modules.Auth.Application.Services.Interfaces;

namespace UserManagementApp.Modules.Auth.Infrastructure.Integrations;

public class DocumentLookupService : IDocumentLookupService
{
    private readonly HttpClient _httpClient;
    private readonly DocumentApiOptions _options;

    public DocumentLookupService(
        HttpClient httpClient,
        IOptions<DocumentApiOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<DocumentLookupResult?> GetPersonAsync(string documentType, string documentNumber)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_options.BaseUrl2) || string.IsNullOrWhiteSpace(_options.Token2))
                return null;

            if (!string.Equals(documentType, "DNI", StringComparison.OrdinalIgnoreCase))
                return null;

            var url =
                $"{_options.BaseUrl2}?document={Uri.EscapeDataString(documentNumber)}&key={Uri.EscapeDataString(_options.Token2)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var apiResponse = await response.Content.ReadFromJsonAsync<DocumentApiResponse>();

            if (apiResponse is null || !apiResponse.Estado || apiResponse.Resultado is null)
                return null;

            var fullLastName = string.Join(" ",
                new[]
                {
                    apiResponse.Resultado.ApellidoPaterno,
                    apiResponse.Resultado.ApellidoMaterno
                }.Where(x => !string.IsNullOrWhiteSpace(x)));

            return new DocumentLookupResult
            {
                DocumentType = documentType,
                DocumentNumber = documentNumber,
                Id = apiResponse.Resultado.Id ?? string.Empty,
                FirstName = apiResponse.Resultado.Nombres ?? string.Empty,
                FatherLastName = apiResponse.Resultado.ApellidoPaterno ?? string.Empty,
                MotherLastName = apiResponse.Resultado.ApellidoMaterno ?? string.Empty,
                LastName = fullLastName,
                FullName = apiResponse.Resultado.NombreCompleto ?? string.Empty,
                Gender = apiResponse.Resultado.Genero ?? string.Empty,
                BirthDate = apiResponse.Resultado.FechaNacimiento ?? string.Empty
            };
        }
        catch
        {
            return null;
        }
    }

    private sealed class DocumentApiResponse
    {
        [JsonPropertyName("estado")]
        public bool Estado { get; set; }

        [JsonPropertyName("mensaje")]
        public string? Mensaje { get; set; }

        [JsonPropertyName("resultado")]
        public ResultadoResponse? Resultado { get; set; }
    }

    private sealed class ResultadoResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("nombres")]
        public string? Nombres { get; set; }

        [JsonPropertyName("apellido_paterno")]
        public string? ApellidoPaterno { get; set; }

        [JsonPropertyName("apellido_materno")]
        public string? ApellidoMaterno { get; set; }

        [JsonPropertyName("nombre_completo")]
        public string? NombreCompleto { get; set; }

        [JsonPropertyName("genero")]
        public string? Genero { get; set; }

        [JsonPropertyName("fecha_nacimiento")]
        public string? FechaNacimiento { get; set; }
    }
}