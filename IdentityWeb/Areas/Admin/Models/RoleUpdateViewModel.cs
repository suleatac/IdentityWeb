using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.Areas.Admin.Models
{
    public class RoleUpdateViewModel
    {
        [Required]
        public string Id { get; set; } = default!;

        [Required(ErrorMessage = "Rol adı boş geçilemez.")]
        [Display(Name = "Rol Adı")]
        public string Name { get; set; } = default!;
    }
}
