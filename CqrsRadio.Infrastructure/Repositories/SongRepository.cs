using System;
using System.Collections.Generic;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly IProvider _provider;
        private readonly IDbParameter _dbParameter;

        public SongRepository(IProvider provider, IDbParameter dbParameter)
        {
            _provider = provider;
            _dbParameter = dbParameter;
        }
        public void Add(UserId userId, PlaylistId playlistId, SongId songId, string title, string artist)
        {
            
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
    }
}
