using Microsoft.AspNetCore.Identity;

namespace IdentityWeb.Localization
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new() {
                Code = "PasswordTooShort",
                Description = $" Şifre en az 6 karakterli olmalıdır. {length} karakterli uzunluk yetersizdir."
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new() {
                Code = "DuplicateUserName",
                Description = $" Bu kullanıcı adı: {userName} daha önce başka bir kullanıcı tarafından alınmıştır."
            };
          
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new() {
                Code = "DuplicateEmail",
                Description = $" Bu email: {email} daha önce başka bir kullanıcı tarafından alınmıştır."
            };

        }
      
    }
}
