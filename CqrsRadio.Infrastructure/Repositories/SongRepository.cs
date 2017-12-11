using System;
using System.Collections.Generic;
using System.Data.SQLite;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        public void Add(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)
        {
            Console.WriteLine(
                $"UserId:{userId.Value} PlaylistId: {playlistId.Value} SongId: {songId.Value} Title:{title} Artist: {artist}");
        }

        public IEnumerable<Song> GetRandomSongs(int size)
        {
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
            {
                var commandText = $"select songid, title, artist from radiosong order by random() limit {size}";
                cnx.Open();
                using (var command = new SQLiteCommand(commandText, cnx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var songId = SongId.Parse(reader[0].ToString());
                            var title = reader[1].ToString();
                            var artist = reader[2].ToString();
                            yield return new Song(songId, title, artist);
                        }
                    }
                }
            }
        }
    }
}
