using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Infrastructure.Repositories
{
    public class RadioSongRepository : IRadioSongRepository
    {
        public void Add(SongId songId, string radioName, string title, string artist)
        {
            using (var cnx = new SQLiteConnection("Data Source=cqrsradio.sqlite;Version=3;"))
            {
                var commandText = "insert into radiosong (songid, radioname, title, artist) " +
                                  $"values ({songId.Value}, {radioName}, {title}, {artist})";
                cnx.Open();
                new SQLiteCommand(commandText, cnx)
                    .ExecuteNonQuery();
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
                var reader = new SQLiteCommand(commandText, cnx).ExecuteReader();

                return reader.HasRows;
            }
        }
    }
}
