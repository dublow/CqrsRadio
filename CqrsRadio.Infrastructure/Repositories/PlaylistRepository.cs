using System;
using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly IProvider _provider;
        public PlaylistRepository(IProvider provider)
        {
            _provider = provider;
        }
        public void Add(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)
        {
            Console.WriteLine(
                $"UserId:{userId.Value} PlaylistId: {playlistId.Value} SongId: {songId.Value} Title:{title} Artist: {artist}");
        }

        public IEnumerable<Song> GetRandomSongs(int size)
        {
            using (var cnx = _provider.Create())
            {
                
                var commandText = $"select songid, title, artist from radiosong order by random() limit {size}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
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

        public void Add(UserId userId, PlaylistId isAny, string name)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = $"insert into playlist (userid, createdAt) values({userId.Value}, {DateTime.UtcNow})";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(UserId userId)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = $"update playlist set createdAt={DateTime.UtcNow} where userid = {userId.Value}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(UserId userId, PlaylistId isAny, string name)
        {
            throw new NotImplementedException();
        }

        public bool Exists(UserId userId)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = $"select userid from playlist where userid = {userId.Value}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }

        public bool CanCreatePlaylist(UserId userId, DateTime interval)
        {
            using (var cnx = _provider.Create())
            {
                // createdAt : 2017/12/23 14:00
                // interval : 2017/12/23 13:50
                // createdAt > interval
                // false

                // createdAt : 2017/12/23 14:00
                // interval : 2017/12/23 14:10
                // createdAt < interval
                // true
                var commandText = $"select createdAt from playlist where userid = {userId.Value}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var createdAt = DateTime.Parse(reader[0].ToString());
                            return createdAt < interval;

                        }
                        return true;
                    }
                }
            }
        }
    }
}
