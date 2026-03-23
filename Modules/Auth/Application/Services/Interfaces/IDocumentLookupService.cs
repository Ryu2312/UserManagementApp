using UserManagementApp.Modules.Auth.Application.Models;

namespace UserManagementApp.Modules.Auth.Application.Services.Interfaces;

public interface IDocumentLookupService
{
    Task<DocumentLookupResult?> GetPersonAsync(string documentType, string documentNumber);
}