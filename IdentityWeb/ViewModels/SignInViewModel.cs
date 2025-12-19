using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.ViewModels
{
    public class SignInViewModel
    {
        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır")]
        [Required(ErrorMessage = "Email Boş Geçilemez")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        [Display(Name = "Şifre :")]
        public string Password { get; set; } = default!;

        [Display(Name = "Beni Hatırla :")]
        public bool RememberMe { get; set; }
    }
}
