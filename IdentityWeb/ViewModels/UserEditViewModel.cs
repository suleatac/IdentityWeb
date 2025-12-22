using IdentityWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.ViewModels
{
    public class UserEditViewModel
    {
      
        [Required(ErrorMessage = "Kullanıcı Adı Boş Geçilemez")]
        [Display(Name = "Kullanıcı Adı :")]
        public string UserName { get; set; } = default!;

        [RegularExpression(@"^(?:\+90|0)5\d{9}$", ErrorMessage = "Telefon Formatı Yanlıştır (Örn: 05551234567 veya +905551234567)")]
        [Required(ErrorMessage = "Telefon Boş Geçilemez")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = default!;

        [EmailAddress(ErrorMessage = "Email Formatı Yanlıştır")]
        [Required(ErrorMessage = "Email Boş Geçilemez")]
        [Display(Name = "Email :")]
        public string Email { get; set; } = default!;
        [DataType(DataType.Date)]
        [Display(Name ="Doğum Tarihi :")]
        public DateTime? BirthDate { get; set; } = default!;


        [Display(Name = "Şehir :")]
        public string? City { get; set; } = default!;

        [Display(Name = "Profil Resmi :")]
        public IFormFile? Picture { get; set; }


        [Display(Name = "Cinsiyet :")]
        public Gender? Gender { get; set; }
    }
}
