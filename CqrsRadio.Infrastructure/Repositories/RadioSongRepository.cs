using System;
using System.Data.SQLite;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class RadioSongRepository : IRadioSongRepository
    {
        private readonly IProvider _provider;
        public RadioSongRepository(IProvider provider)
        {
            _provider = provider;
        }
        public void Add(SongId songId, string radioName, string title, string artist)
        {
            if (title.Contains("'"))
                title = title.Replace("'", " ");

            if (artist.Contains("'"))
                artist = artist.Replace("'", " ");
            using (var cnx = _provider.Create())
            {
                var commandText = "insert into radiosong (songid, radioname, title, artist) " +
                                  $"values ({songId.Value}, '{radioName}', '{title}', '{artist}')";
                cnx.Open();

                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddToDuplicate(string radioName, string title, string artist)
        {
            throw new NotImplementedException();
        }

        public void AddToError(string radioName, string error)
        {
            throw new NotImplementedException();
        }

        public bool SongExists(SongId songId)
        {
            using (var cnx = _provider.Create())
            {
                var commandText = $"select songid from radiosong where songid = {songId.Value}";
                cnx.Open();
                using (var command = cnx.CreateCommand())
                {
                    command.CommandText = commandText;
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                } 
            }
        }
    }
}
