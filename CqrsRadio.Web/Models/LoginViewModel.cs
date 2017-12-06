namespace CqrsRadio.Web.Models
{
    public class LoginViewModel
    {
        public string AccessToken { get; set; }
        public string UserId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string PlaylistName { get; set; }
    }
}
