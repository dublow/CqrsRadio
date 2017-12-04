using CqrsRadio.Domain.Utilities;
using NUnit.Framework;

namespace CqrsRadio.Test.UtilitiesTests
{
    [TestFixture]
    public class EmailShould
    {
        [Test]
        public void ValidWhenCheckEmail()
        {
            var email = "nicolas.dfr@gmail.com";

            Assert.IsTrue(StringUtilities.IsValidEmail(email));
        }

        [Test]
        public void ValidWhenCheckInvalidEmail()
        {
            var email1 = "nicolas.dfrgmail.com";
            var email2 = "nicolas.dfr@gmailcom";
            var email3 = "nicolas.dfrgmailcom";

            Assert.IsFalse(StringUtilities.IsValidEmail(email1));
            Assert.IsFalse(StringUtilities.IsValidEmail(email2));
            Assert.IsFalse(StringUtilities.IsValidEmail(email3));
        }
    }
}
