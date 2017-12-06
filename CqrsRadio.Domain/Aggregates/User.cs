using System;
using System.Collections.Generic;
using System.Linq;
using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Aggregates
{
    public class User
    {
        private readonly IEventPublisher _publisher;
        private readonly Decision _decision;
        private readonly List<Playlist> _playlists = new List<Playlist>();

        public Identity Identity { get; private set; }

        public User(IEventStream stream, IEventPublisher publisher)
        {
            _publisher = publisher;
            _decision = new Decision(stream);

            Restore(stream);
        }

        public static User Create(IEventStream stream, IEventPublisher publisher, string email, string nickname,
            string userId)
        {
            var identity = Identity.Create(email, nickname, userId);
            var user = new User(stream, publisher)
            {
                Identity = identity
            };

            publisher.Publish(new UserCreated(identity));

            return user;
        }

        public void Delete()
        {
            if(_decision.IsDeleted) return;

            PublishAndApply(new UserDeleted(Identity.UserId));
        }

        public void AddPlaylist(string name)
        {
            if (_decision.IsDeleted)
                return;
            if (HasPlaylist(name))
                return;

            var playlist = new Playlist(Identity.UserId, name);

            _playlists.Add(playlist);

            _publisher.Publish(new PlaylistAdded(Identity.UserId, name));
        }

        public void DeletePlaylist(string name)
        {
            if (_decision.IsDeleted)
                return;
            if (!HasPlaylist(name))
                return;

            _playlists.Remove(new Playlist(Identity.UserId, name));

            _publisher.Publish(new PlaylistDeleted(Identity.UserId, name));
        }

        public void ClearPlaylists()
        {
            if (_decision.IsDeleted)
                return;

            _playlists.Clear();
            _publisher.Publish(new PlaylistsCleared(Identity.UserId));
        }

        public void AddSongToPlaylist(string playlistName, string songId, string genre, string title, string artist)
        {
            if (_decision.IsDeleted)
                return;
            if (!HasPlaylist(playlistName))
                return;

            var playlist = GetPlaylist(playlistName);

            var song = new Song(songId, genre, title, artist);
            if (playlist.Songs.Contains(song))
                return;

            playlist.AddSong(song);

            _publisher.Publish(new SongAdded(Identity.UserId, playlistName, songId, genre, title, artist));
        }

        public Playlist GetPlaylist(string playlistName)
        {
            return _playlists
                .Single(x => x.Name.Equals(playlistName, StringComparison.InvariantCultureIgnoreCase));
        }

        private void PublishAndApply(IDomainEvent evt)
        {
            _publisher.Publish(evt);
            _decision.Apply(evt);
        }

        private void Restore(IEventStream stream)
        {
            foreach (var domainEvent in stream.GetEvents())
            {
                if (domainEvent is UserCreated userCreated)
                {
                    Identity = userCreated.Identity;
                }
                else if (domainEvent is PlaylistAdded playlistAdded)
                {
                    _playlists.Add(new Playlist(playlistAdded.UserId, playlistAdded.Name));
                }
                else if (domainEvent is SongAdded songAdded)
                {
                    var playlist = GetPlaylist(songAdded.PlaylistName);

                    playlist.AddSong(new Song(songAdded.SongId, songAdded.Genre, songAdded.Title, songAdded.Artist));
                }
                else if (domainEvent is PlaylistDeleted playlistDeleted)
                {
                    _playlists.Remove(new Playlist(Identity.UserId, playlistDeleted.Name));
                }
                else if (domainEvent is PlaylistsCleared playlistsCleared)
                {
                    _playlists.Clear();
                }
            }
        }

        private bool HasPlaylist(string name)
        {
            return _playlists
                .Any(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private class Decision
        {
            public bool IsDeleted { get; private set; }

            public Decision(IEventStream stream)
            {
                foreach (var domainEvent in stream.GetEvents())
                {
                    if(domainEvent is UserDeleted userDeleted)
                        Apply(userDeleted);
                }
            }

            public void Apply(IDomainEvent evt)
            {
                IsDeleted = true;
            }
        }
    }
}