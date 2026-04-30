using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }

    public bool RememberMe { get; set; }
}