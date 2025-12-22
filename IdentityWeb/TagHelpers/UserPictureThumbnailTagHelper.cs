using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IdentityWeb.TagHelpers
{
    public class UserPictureThumbnailTagHelper:TagHelper
    {
        public string? PictureUrl { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            var imageUrl = string.IsNullOrEmpty(PictureUrl) ? "/userpictures/default_user_picture.jpeg" : $"/userpictures/{PictureUrl}";
            output.Attributes.SetAttribute("src", imageUrl);
            output.Attributes.SetAttribute("alt", "User Picture");
            output.Attributes.SetAttribute("class", "rounded-circle");
            output.Attributes.SetAttribute("width", "100");
            output.Attributes.SetAttribute("height", "100");
        }
    }
}
