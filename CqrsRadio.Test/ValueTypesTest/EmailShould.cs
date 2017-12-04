using System;
using CqrsRadio.Domain.ValueTypes;
using NUnit.Framework;

namespace CqrsRadio.Test.ValueTypesTest
{
    [TestFixture]
    public class EmailShould
    {
        [Test]
        public void ValidWhenCreateEmail()
        {
            Email.Parse("nicolas.dfr@gmail.com");
            Assert.Pass();
        }

        [Test]
        public void InvalidWhenCreateEmail()
        {
            Assert.Throws<ArgumentException>(() => Email.Parse("nicolas"));
        }

        [Test]
        public void EqualWhenTestSameEmail()
        {
            var email1 = Email.Parse("nicolas.dfr@gmail.com");
            var email2 = Email.Parse("nicolas.dfr@gmail.com");

            Assert.AreEqual(email1, email2);
        }

        [Test]
        public void EqualWhenTestSameEmailUpperized()
        {
            var email1 = Email.Parse("nicolas.dfr@gmail.com");
            var email2 = Email.Parse("NICOLAS.DFR@gmail.com");

            Assert.AreEqual(email1, email2);
        }

        [Test]
        public void NotEqualWhenTestDifferentEmail()
        {
            var email1 = Email.Parse("nicolas.dfr@gmail.com");
            var email2 = Email.Parse("nicolas.dfr2@gmail.com");

            Assert.AreNotEqual(email1, email2);
        }

        [Test]
        public void EqualOperatorWhenTestSameEmail()
        {
            var email1 = Email.Parse("nicolas.dfr@gmail.com");
            var email2 = Email.Parse("nicolas.dfr@gmail.com");

            Assert.IsTrue(email1 == email2);
        }

        [Test]
        public void EqualOperatorWhenTestSameEmailUpperized()
        {
            var email1 = Email.Parse("nicolas.dfr@gmail.com");
            var email2 = Email.Parse("NICOLAS.DFR@gmail.com");

            Assert.IsTrue(email1 == email2);
        }

        [Test]
        public void NotEqualOperatorWhenTestDifferentEmail()
        {
            var email1 = Email.Parse("nicolas.dfr@gmail.com");
            var email2 = Email.Parse("nicolas.dfr2@gmail.com");

            Assert.IsTrue(email1 != email2);
        }
    }
}
