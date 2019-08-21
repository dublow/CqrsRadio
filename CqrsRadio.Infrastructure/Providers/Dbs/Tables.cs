namespace CqrsRadio.Infrastructure.Providers.Dbs
{
    public static class Tables
    {
        public static string RadioSong => "create table if not exists " +
                                          "radiosong (songid int primary key, radioname varchar(20), title varchar(50), artist varchar(50))";

        public static string Admin => "create table if not exists " +
                                      "admin (login varchar(20) primary key, hash blob)";

        public static string Playlist => "create table if not exists " +
                                         "playlist (userid int primary key, createdAt varchar(20))";
    }
}
