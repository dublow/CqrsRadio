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
            Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            Assert.Pass();
        }

        [Test]
        public void InvalidWhenCreateIdentityWithInvalidEmail()
        {
            Assert.Throws<ArgumentException>(() => Identity.Create("nicolas", "nicolas", "12345"));
        }

        [Test]
        public void InvalidWhenCreateIdentityWithInvalidNickname()
        {
            Assert.Throws<ArgumentException>(() => Identity.Create("nicolas.dfr@gmail.com", string.Empty, "12345"));
        }

        [Test]
        public void InvalidWhenCreateIdentityWithInvalidUserId()
        {
            Assert.Throws<ArgumentException>(() => Identity.Create("nicolas.dfr@gmail.com", "nicolas", "abc24"));
        }

        [Test]
        public void EqualWhenTestSameIdentity()
        {
            var identity1 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            var identity2 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");

            Assert.AreEqual(identity1, identity2);
        }

        [Test]
        public void NotEqualWhenTestDifferentEmail()
        {
            var identity1 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            var identity2 = Identity.Create("nicolas.dfr2@gmail.com", "nicolas", "12345");

            Assert.AreNotEqual(identity1, identity2);
        }

        [Test]
        public void NotEqualWhenTestDifferentNickname()
        {
            var identity1 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            var identity2 = Identity.Create("nicolas.dfr@gmail.com", "nicolas2", "12345");

            Assert.AreNotEqual(identity1, identity2);
        }

        [Test]
        public void NotEqualWhenTestDifferentUserId()
        {
            var identity1 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            var identity2 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12346");

            Assert.AreNotEqual(identity1, identity2);
        }

        [Test]
        public void EqualOperatorWhenTestSameIdentity()
        {
            var identity1 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            var identity2 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");

            Assert.IsTrue(identity1 == identity2);
        }

        [Test]
        public void EqualOperatorWhenTestDifferentIdentity()
        {
            var identity1 = Identity.Create("nicolas.dfr@gmail.com", "nicolas", "12345");
            var identity2 = Identity.Create("nicolas.dfr2@gmail.com", "nicolas", "12345");

            Assert.IsTrue(identity1 != identity2);
        }
    }
}
