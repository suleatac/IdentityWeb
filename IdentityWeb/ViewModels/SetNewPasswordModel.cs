using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.ViewModels
{
    public class SetNewPasswordModel
    {
        [Required]
        public string UserId { get; set; } = default!;

        [Required]
        public string Token { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        [Display(Name = "Yeni Şifre :")]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmemektedir!")]
        [Required(ErrorMessage = "Şifre Tekrarı Boş Geçilemez")]
        [Display(Name = "Yeni Şifre Tekrar :")]
        public string PasswordConfirm { get; set; } = default!;
    }
}
