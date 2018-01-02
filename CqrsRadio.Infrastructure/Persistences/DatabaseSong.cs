using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using CqrsRadio.Domain.Repositories;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Infrastructure.Providers;

namespace CqrsRadio.Infrastructure.Persistences
{
    public class DatabaseSong : IDatabase
    {
        private readonly IProvider _provider;
        private readonly IDeezerApi _deezerApi;

        public DatabaseSong(IProvider provider, IDeezerApi deezerApi)
        {
            _provider = provider;
            _deezerApi = deezerApi;
        }

        public void Create()
        {
            if (!File.Exists(_provider.Dbname))
            {
                _provider.CreateFile(_provider.Dbname);
            }

            var radioSongTable = "create table if not exists " +
                                 "radiosong (songid int primary key, radioname varchar(20), title varchar(50), artist varchar(50))";

            ExecuteCommand(radioSongTable);
        }

        public void Restore()
        {
            
        }

        private void ExecuteCommand(string commandText)
        {
            using (var cnx = _provider.Create())
            {
                cnx.Open();
                
                using (var command = cnx.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = commandText;
                    command.ExecuteNonQueryAsync();
                }
            }
        }

        private void ExecuteMultiCommand(List<string> commandTextes)
        {
            using (var cnx = _provider.Create())
            {
                cnx.Open();

                var index = 0;
                var max = commandTextes.Count;
                commandTextes.ForEach(commandText =>
                {
                    Console.Clear();
                    Console.WriteLine($"{index}/{max}");

                    using (var command = cnx.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = commandText;
                        command.ExecuteNonQuery();
                    }

                    index++;
                });
            }
        }
    }
}
