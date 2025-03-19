using System.ComponentModel.DataAnnotations;

namespace tehnologiinet.Models;

public class StudentDto
{
    [Required(ErrorMessage = "Prenumele este obligatoriu")]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Numele este obligatoriu")]
    public string LastName { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Emailul nu este valid!")]
    public string Email { get; set; }
    [Required]
    [Range(1, 4, ErrorMessage = "Anul de studii trebuie sa fie intre 1 si 4")]
    public int Year { get; set; }
}