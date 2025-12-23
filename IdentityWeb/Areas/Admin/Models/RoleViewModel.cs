using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.Areas.Admin.Models
{
    public class RoleViewModel
    {
        [Required]
        public string Id { get; set; } = default!;
        [Required(ErrorMessage = "Rol Adı Boş Geçilemez")]
        [Display(Name = "Rol Adı :")]
        public string? Name { get; set; }
    }
}
