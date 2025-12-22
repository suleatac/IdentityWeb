using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.ViewModels
{
    public class ResetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır")]
        [Required(ErrorMessage = "Email Boş Geçilemez")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = default!;
    }
}
