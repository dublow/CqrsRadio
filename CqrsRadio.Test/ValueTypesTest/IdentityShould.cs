using System;
using CqrsRadio.Domain.ValueTypes;
using NUnit.Framework;

namespace CqrsRadio.Test.ValueTypesTest
{
    [TestFixture]
    public class IdentityShould
    {
        [Test]
        public void ValidWhenCreateIdentity()
        {
            Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            Assert.Pass();
        }

        [Test]
        public void InvalidWhenCreateIdentityWithInvalidEmail()
        {
            Assert.Throws<ArgumentException>(() => Identity.Parse("nicolas", "nicolas", "12345", "accessToken"));
        }

        [Test]
        public void InvalidWhenCreateIdentityWithInvalidNickname()
        {
            Assert.Throws<ArgumentException>(() => Identity.Parse("nicolas.dfr@gmail.com", string.Empty, "12345", "accessToken"));
        }

        [Test]
        public void InvalidWhenCreateIdentityWithInvalidUserId()
        {
            Assert.Throws<ArgumentException>(() => Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "abc24", "accessToken"));
        }

        [Test]
        public void EqualWhenTestSameIdentity()
        {
            var identity1 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            var identity2 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");

            Assert.AreEqual(identity1, identity2);
        }

        [Test]
        public void NotEqualWhenTestDifferentEmail()
        {
            var identity1 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            var identity2 = Identity.Parse("nicolas.dfr2@gmail.com", "nicolas", "12345", "accessToken");

            Assert.AreNotEqual(identity1, identity2);
        }

        [Test]
        public void NotEqualWhenTestDifferentNickname()
        {
            var identity1 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            var identity2 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas2", "12345", "accessToken");

            Assert.AreNotEqual(identity1, identity2);
        }

        [Test]
        public void NotEqualWhenTestDifferentUserId()
        {
            var identity1 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            var identity2 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12346", "accessToken");

            Assert.AreNotEqual(identity1, identity2);
        }

        [Test]
        public void EqualOperatorWhenTestSameIdentity()
        {
            var identity1 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            var identity2 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");

            Assert.IsTrue(identity1 == identity2);
        }

        [Test]
        public void EqualOperatorWhenTestDifferentIdentity()
        {
            var identity1 = Identity.Parse("nicolas.dfr@gmail.com", "nicolas", "12345", "accessToken");
            var identity2 = Identity.Parse("nicolas.dfr2@gmail.com", "nicolas", "12345", "accessToken");

            Assert.IsTrue(identity1 != identity2);
        }
    }
}
