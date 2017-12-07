﻿using System.Linq;
using CqrsRadio.Domain.Events;
using CqrsRadio.Domain.ValueTypes;
using CqrsRadio.Handlers;
using CqrsRadio.Test.Mocks;
using NUnit.Framework;

namespace CqrsRadio.Test.HandlerTests
{
    [TestFixture]
    public class PlaylistHandlerShould
    {
        [Test]
        public void UseRepositoryWhenPlaylistIsAdded()
        {
            // arrange
            var mockedPlaylistRepository = PlaylistRepositoryBuilder.Create();
            var playlistRepository = mockedPlaylistRepository.Build();
            var playlistHandler = new PlaylistHandler(playlistRepository);
            (UserId userId, PlaylistId playlistId, string name) = ("12345", "123", "bestof");
            // act
            playlistHandler.Handle(new PlaylistAdded("12345", "123", "bestof"));
            // assert
            var (actualUserId, actualPlaylisId, actualName) = mockedPlaylistRepository.Playlists.First();
            Assert.AreEqual(userId, actualUserId);
            Assert.AreEqual(playlistId, actualPlaylisId);
            Assert.AreEqual(name, actualName);
        }

        [Test]
        public void UseRepositoryWhenPlaylistIsDeleted()
        {
            // arrange
            var mockedPlaylistRepository = PlaylistRepositoryBuilder.Create();
            mockedPlaylistRepository.Playlists.Add(("12345", "123", "bestof"));
            var playlistRepository = mockedPlaylistRepository.Build();
            var playlistHandler = new PlaylistHandler(playlistRepository);
            // act
            playlistHandler.Handle(new PlaylistDeleted("12345", "123", "bestof"));
            // assert
            Assert.AreEqual(0, mockedPlaylistRepository.Playlists.Count);
        }
    }
}
