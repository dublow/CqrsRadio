namespace CqrsRadio.ApiInfrastructure.ViewModel
{
    public class AddSong
    {
        public string UserId { get; set; }
        public string PlaylistId { get; set; }
        public string SongId { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
    }
}
