using System;
using CqrsRadio.Domain.ValueTypes;
using NUnit.Framework;

namespace CqrsRadio.Test.ValueTypesTest
{
    [TestFixture]
    public class UserIdShould
    {
        [Test]
        public void ValidWhenCreateUserId()
        {
            UserId.Parse("12345");
            Assert.Pass();
        }

        [Test]
        public void InvalidWhenCreateUserId()
        {
            Assert.Throws<ArgumentException>(() => UserId.Parse("abcde"));
        }

        [Test]
        public void EqualWhenTestSameUserId()
        {
            var userId1 = UserId.Parse("12345");
            var userId2 = UserId.Parse("12345");

            Assert.AreEqual(userId1, userId2);
        }

        [Test]
        public void NotEqualWhenTestDifferentNickname()
        {
            var userId1 = UserId.Parse("12345");
            var userId2 = UserId.Parse("67890");

            Assert.AreNotEqual(userId1, userId2);
        }

        [Test]
        public void EqualOperatorWhenTestSameNickname()
        {
            var userId1 = UserId.Parse("12345");
            var userId2 = UserId.Parse("12345");

            Assert.IsTrue(userId1 == userId2);
        }

        [Test]
        public void NotEqualOperatorWhenTestDifferentNickname()
        {
            var userId1 = UserId.Parse("12345");
            var userId2 = UserId.Parse("67890");

            Assert.IsTrue(userId1 != userId2);
        }
    }
}
