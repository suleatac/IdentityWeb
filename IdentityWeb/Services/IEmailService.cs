namespace IdentityWeb.Services
{
    public interface IEmailService
    {
         Task SendResetEmail(string resetEmailLink,string ToEmail);
    }
}
