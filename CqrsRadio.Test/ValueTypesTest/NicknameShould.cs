using System;
using CqrsRadio.Domain.ValueTypes;
using NUnit.Framework;

namespace CqrsRadio.Test.ValueTypesTest
{
    [TestFixture]
    public class NicknameShould
    {
        [Test]
        public void ValidWhenCreateNickname()
        {
            Nickname.Parse("nicolas");
            Assert.Pass();
        }

        [Test]
        public void InvalidWhenCreateNickname()
        {
            Assert.Throws<ArgumentException>(() => Nickname.Parse(string.Empty));
        }

        [Test]
        public void EqualWhenTestSameNickname()
        {
            var nickname1 = Nickname.Parse("nicolas");
            var nickname2 = Nickname.Parse("nicolas");

            Assert.AreEqual(nickname1, nickname2);
        }

        [Test]
        public void EqualWhenTestSameNicknameUpperized()
        {
            var nickname1 = Nickname.Parse("nicolas");
            var nickname2 = Nickname.Parse("NICOLAS");

            Assert.AreEqual(nickname1, nickname2);
        }

        [Test]
        public void NotEqualWhenTestDifferentNickname()
        {
            var nickname1 = Nickname.Parse("nicolas");
            var nickname2 = Nickname.Parse("nicol");

            Assert.AreNotEqual(nickname1, nickname2);
        }

        [Test]
        public void EqualOperatorWhenTestSameNickname()
        {
            var nickname1 = Nickname.Parse("nicolas");
            var nickname2 = Nickname.Parse("nicolas");

            Assert.IsTrue(nickname1 == nickname2);
        }

        [Test]
        public void EqualOperatorWhenTestSameNicknameUpperized()
        {
            var nickname1 = Nickname.Parse("nicolas");
            var nickname2 = Nickname.Parse("NICOLAS");

            Assert.IsTrue(nickname1 == nickname2);
        }

        [Test]
        public void NotEqualOperatorWhenTestDifferentNickname()
        {
            var nickname1 = Nickname.Parse("nicolas");
            var nickname2 = Nickname.Parse("nicol");

            Assert.IsTrue(nickname1 != nickname2);
        }
    }
}
