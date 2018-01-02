using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.ApiInfrastructure.ViewModel
{
    public class RestoreSong
    {
        public string UserId { get; set; }
        public string AccessToken { get; set; }
    }
}
