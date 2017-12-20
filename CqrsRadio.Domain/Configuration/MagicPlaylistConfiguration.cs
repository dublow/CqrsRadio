using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace CqrsRadio.Domain.Configuration
{
    public interface IMagicPlaylistConfiguration
    {
        List<Environment> Environments { get; }
    }

    public class MagicPlaylistConfiguration: IMagicPlaylistConfiguration
    {
        private static readonly MagicPlaylistSection Section;

        static MagicPlaylistConfiguration()
        {
            Section = (MagicPlaylistSection) ConfigurationManager.GetSection("magicPlaylist");
        }

        public static IMagicPlaylistConfiguration Current => new MagicPlaylistConfiguration();
        public List<Environment> Environments => Section.Environments.ToList();
    }

    public class MagicPlaylistSection : ConfigurationSection
    {
        [ConfigurationProperty("environments", IsRequired = true, IsDefaultCollection = true)]
        public EnvironmentCollection Environments
        {
            get => (EnvironmentCollection) this["environments"];
            set => this["environments"] = value;
        }
    }

    public class EnvironmentCollection : ConfigurationElementCollection, IEnumerable<Environment>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new Environment();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Environment) element).Name;
        }

        IEnumerator<Environment> IEnumerable<Environment>.GetEnumerator()
        {
            foreach (var item in BaseGetAllKeys())
            {
                yield return (Environment)BaseGet(item);
            }
        }
    }

    public class Environment : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public EnvironmentType Name
        {
            get
            {
                var name = this["name"].ToString();
                return name == "Local" ? EnvironmentType.Local : EnvironmentType.Production;
            }
            set => this["name"] = value;
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get => (string)base["url"];
            set => base["url"] = value;
        }

        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get => (string)base["appId"];
            set => base["appId"] = value;
        }

        [ConfigurationProperty("channel", IsRequired = true)]
        public string Channel
        {
            get => (string)base["channel"];
            set => base["channel"] = value;
        }

        [ConfigurationProperty("size", IsRequired = true)]
        public int Size
        {
            get => (int)base["size"];
            set => base["size"] = value;
        }
    }

    public enum EnvironmentType
    {
        Local = 0,
        Production = 1
    }
}
