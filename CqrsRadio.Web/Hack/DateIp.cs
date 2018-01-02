using System;

namespace CqrsRadio.Web.Hack
{
    public class DateIp
    {
        public readonly string Date;
        public readonly string Ip;
        public readonly bool IsEmpty;

        private DateIp(string date, string ip)
        {
            Date = date;
            Ip = ip;

            IsEmpty = string.IsNullOrEmpty(Date) 
                      && string.IsNullOrEmpty(Ip);
        }

        public static DateIp Create(string date, string ip) => new DateIp(date, ip);
        public static DateIp Empty => new DateIp(string.Empty, String.Empty);
    }
}