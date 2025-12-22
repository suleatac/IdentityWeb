using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.ViewModels
{
    public class SignUpViewModel
    {

        [Required(ErrorMessage ="Kullanıcı Adı Boş Geçilemez")]
        [Display(Name ="Kullanıcı Adı :")]
        public string UserName { get; set; } = default!;

        [RegularExpression(@"^(?:\+90|0)5\d{9}$", ErrorMessage = "Telefon Formatı Yanlıştır (Örn: 05551234567 veya +905551234567)")]
        [Required(ErrorMessage = "Telefon Boş Geçilemez")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = default!;

        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır")]
        [Required(ErrorMessage = "Email Boş Geçilemez")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = default!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre Boş Geçilemez")]
        [Display(Name = "Şifre :")]
        [MinLength(6, ErrorMessage = "Şifre En az 6 karakter olabilir.")]
        public string Password { get; set; } = default!;

        [Compare(nameof(Password),ErrorMessage ="Şifreler eşleşmemektedir!")]
        [Required(ErrorMessage = "Şifre Tekrarı Boş Geçilemez")]
        [Display(Name = "Şifre Tekrar :")]
        [MinLength(6, ErrorMessage = "Şifre En az 6 karakter olabilir.")]
        public string PasswordConfirm { get; set; } = default!;

    }
}
