using System.Text.RegularExpressions;
using CqrsRadio.Web.Modules;

namespace CqrsRadio.Web.Hack
{
    public static class DateIpParsor
    {
        private static readonly Regex RxDate = new Regex(@"^((?:J[au]n|Feb|Ma[ry]|Apr|Jul|Aug|Sep|Oct|Nov|Dec) \s+ \d+)", RegexOptions.IgnorePatternWhitespace);
        private static readonly Regex RxIp = new Regex(@"((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))[.]((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))[.]((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))[.]((25[0-5])|(2[0-4][0-9])|(1[0-9][0-9])|([1-9][0-9])|([0-9]))", RegexOptions.IgnorePatternWhitespace);

        public static DateIp Line(string line)
        {
            var matchDate = RxDate.Match(line);
            var matchIp = RxIp.Match(line);

            if(matchIp.Success && matchDate.Success)
                return DateIp.Create(matchDate.Value, matchIp.Value);

            return DateIp.Empty;
        }
    }
}