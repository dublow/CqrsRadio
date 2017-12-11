namespace CqrsRadio.Deezer
{
    public static class Endpoints
    {
        public static string GetPlaylists = "https://api.deezer.com/user/{0}/playlists?access_token={1}&limit=100";
        public static string CreatePlaylist = "https://api.deezer.com/user/{0}/playlists?title={1}&access_token={2}";
        public static string GetSongsByPlaylist = "http://api.deezer.com/playlist/{0}/tracks?access_token={1}&limit=100";
        public static string GetAlbum = "http://api.deezer.com/album/{0}?access_token={1}";
        public static string AddSongsToPlaylist = "http://api.deezer.com/playlist/{0}/tracks?songs={1}&access_token={2}";
    }
}
