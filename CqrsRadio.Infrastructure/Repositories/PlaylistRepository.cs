using System;
using System.Data.SQLite;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        public void Add(UserId userId, PlaylistId isAny, string name)
        {
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
            {
                var commandText = $"insert into playlist (userid, createdAt) values({userId.Value}, '{DateTime.UtcNow}')";
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
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
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
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
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
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
            {
                // createdAt : 2017/12/23 14:00
                // interval : 2017/12/23 13:50
                // createdAt > interval
                // false

                // createdAt : 2017/12/23 14:00
                // interval : 2017/12/23 14:10
                // createdAt < interval
                // true
                var commandText = $"select cast(createdAt as text) as createdAt from playlist where userid = {userId.Value}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var r = reader.GetValue(0).ToString();
                            var createdAt = DateTime.Parse(r);
                            return createdAt < interval;

                        }
                        return true;
                    }
                }
            }
        }
    }
}
