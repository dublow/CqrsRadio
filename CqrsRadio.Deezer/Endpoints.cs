namespace CqrsRadio.Deezer
{
    public static class Endpoints
    {
        public static string GetPlaylists = "https://api.deezer.com/user/{0}/playlists?access_token={1}&limit=100";
        public static string CreatePlaylist = "https://api.deezer.com/user/{0}/playlists?title={1}&access_token={2}";
    }
}
