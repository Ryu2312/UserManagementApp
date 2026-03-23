using System.ComponentModel.DataAnnotations;

namespace UserManagementApp.Modules.Auth.Application.Models;

public class ActivateAccountInputModel
{
    [Required(ErrorMessage = "El tipo de documento es obligatorio.")]
    public string DocumentType { get; set; } = "DNI";

    [Required(ErrorMessage = "El número de documento es obligatorio.")]
    public string DocumentNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El celular es obligatorio.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Debes confirmar la contraseña.")]
    [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;
}