using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        [Display(Name = "Eski Şifre :")]
        [MinLength(6, ErrorMessage = "Şifre En az 6 karakter olabilir.")]
        public string CurrentPassword { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        [Display(Name = "Yeni Şifre :")]
        [MinLength(6, ErrorMessage = "Şifre En az 6 karakter olabilir.")]
        public string NewPassword { get; set; } = default!;


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        [Compare(nameof(NewPassword), ErrorMessage = "Şifreler eşleşmemektedir!")]
        [Display(Name = "Yeni Şifre (Tekrar) :")]
        [MinLength(6,ErrorMessage = "Şifre En az 6 karakter olabilir.")]
        public string ConfirmNewPassword { get; set; } = default!;
    }
}
