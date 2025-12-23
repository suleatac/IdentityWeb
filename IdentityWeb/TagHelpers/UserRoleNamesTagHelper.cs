using IdentityWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Text.Encodings.Web;

namespace IdentityWeb.TagHelpers
{
    public class UserRoleNamesTagHelper(UserManager<AppUser> userManager) : TagHelper
    {
        public string UserId { get; set; } = default!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                output.SuppressOutput();
                return;
            }

            var roles = await userManager.GetRolesAsync(user);

            var sb = new StringBuilder();
            foreach (var role in roles)
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }

                sb.Append("<span class='badge bg-secondary'>");
                sb.Append(HtmlEncoder.Default.Encode(role));
                sb.Append("</span>");
            }

            output.TagName = null; // outer tag remains whatever you use in razor
            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
