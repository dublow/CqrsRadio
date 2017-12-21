namespace CqrsRadio.Common.StatsD
{
    public interface IStatsDRequest
    {
        void Send(string value);
    }
}
