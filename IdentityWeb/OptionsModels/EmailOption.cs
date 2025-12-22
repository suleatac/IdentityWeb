namespace IdentityWeb.OptionsModels
{
    public class EmailOption
    {
        public string Host { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Email { get; set; } = default!;
        public int Port { get; set; } = default!;
    }
}
