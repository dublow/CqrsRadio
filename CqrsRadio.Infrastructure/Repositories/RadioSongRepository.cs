using System;
using System.Data.SQLite;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class RadioSongRepository : IRadioSongRepository
    {
        public void Add(SongId songId, string radioName, string title, string artist)
        {
            if (title.Contains("'"))
                title = title.Replace("'", " ");

            if (artist.Contains("'"))
                artist = artist.Replace("'", " ");
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
            {
                var commandText = "insert into radiosong (songid, radioname, title, artist) " +
                                  $"values ({songId.Value}, '{radioName}', '{title}', '{artist}')";
                cnx.Open();
                using (var command = new SQLiteCommand(commandText, cnx))
                {
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

            
        }

        public bool SongExists(SongId songId)
        {
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
            {
                var commandText = $"select songid from radiosong where songid = {songId.Value}";
                cnx.Open();
                using (var command = new SQLiteCommand(commandText, cnx))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                } 
            }
        }
    }
}
