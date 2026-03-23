using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Modules.Auth.Application.Models;

public class LoginInputModel
{
    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public string DocumentType { get; set; } = "DNI";

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}