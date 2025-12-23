using System.ComponentModel.DataAnnotations;

namespace IdentityWeb.Areas.Admin.Models
{
    public class RoleAssignViewModel
    {
        [Required]
        public string Id { get; set; }=default!;
        [Required]
        public string Name { get; set; }=default!;
        [Required]
        public bool Exist { get; set; }




    }
}
