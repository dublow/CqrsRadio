using System.Collections.Generic;
using CqrsRadio.Domain.Configuration;
using Environment = CqrsRadio.Domain.Configuration.Environment;

namespace CqrsRadio.Web.Configuration
{
    class HardCodedMagicPlaylistConfiguration : IMagicPlaylistConfiguration
    {
        public List<Environment> Environments=>new List<Environment>
        {
            new Environment{Name = EnvironmentType.Local, Url = "http://127.0.0.1:1235", AppId = "264622", Channel = "http://127.0.0.1:1235/channel", Size = 50},
            new Environment{Name = EnvironmentType.Production, Url = "http://localhost:1234", AppId = "170341", Channel = "http://magicplaylistgenerator.com/channel", Size = 50}
        };
    }
}
