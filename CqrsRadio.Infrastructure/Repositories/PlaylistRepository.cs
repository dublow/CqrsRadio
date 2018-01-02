using System;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly IProvider _provider;
        private readonly IDbParameter _dbParameter;

        public PlaylistRepository(IProvider provider, IDbParameter dbParameter)
        {
            _provider = provider;
            _dbParameter = dbParameter;
        }

        public void Add(UserId userId, PlaylistId playlistId, string name)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = $"insert into playlist (userid, createdAt) values({userId.Value}, '{DateTime.UtcNow:s}')";
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
                var commandText = $"update playlist set createdAt='{DateTime.UtcNow:s}' where userid = {userId.Value}";
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
                var commandText = $"select createdAt as createdAt from playlist where userid = {userId.Value}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dateAsString = reader[0].ToString();
                            var createdAt = DateTime.Parse(dateAsString);
                            return createdAt < interval;

                        }
                        return true;
                    }
                }
            }
        }
    }
}
