﻿using CqrsRadio.Domain.Entities;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.EventStores;
using CqrsRadio.Domain.Handlers;
using CqrsRadio.Domain.Services;
using CqrsRadio.Domain.ValueTypes;

namespace CqrsRadio.Domain.Aggregates
{
    public class User
    {
        private readonly IEventPublisher _publisher;
        private readonly Decision _decision;
        private readonly IDeezerApi _deezerApi;

        public Playlist Playlist { get; private set; }
        public Identity Identity { get; private set; }

        public User(IEventStream stream, IEventPublisher publisher, IDeezerApi deezerApi)
        {
            _publisher = publisher;
            _decision = new Decision(stream);
            _deezerApi = deezerApi;

            Playlist = Playlist.Empty;

            Restore(stream);
        }

        public static User Create(IEventStream stream, IEventPublisher publisher, 
            IDeezerApi deezerApi, string email, string nickname, string userId, string accessToken)
        {
            var identity = Identity.Parse(email, nickname, userId, accessToken);

            var user = new User(stream, publisher, deezerApi)
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
            if (!Playlist.IsEmpty)
                return;

            var playlistId = _deezerApi
                .CreatePlaylist(Identity.AccessToken, Identity.UserId, name);

            Playlist = new Playlist(playlistId, name);

            _publisher.Publish(new PlaylistAdded(Identity.UserId, playlistId, name));
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
                    Playlist = new Playlist(playlistAdded.PlaylistId, playlistAdded.Name);
                }
                else if (domainEvent is SongAdded songAdded)
                {
                    Playlist.AddSong(new Song(songAdded.SongId, songAdded.Title, songAdded.Artist));
                }
                else if (domainEvent is PlaylistDeleted)
                {
                    Playlist = Playlist.Empty;
                }
            }
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