using CqrsRadio.Common.StatsD;
using Moq;

namespace CqrsRadio.Test.Mocks
{
    public class MetricBuilder
    {
        private readonly Mock<IMetric> _mock;

        private MetricBuilder()
        {
            _mock = new Mock<IMetric>();
        }

        public static MetricBuilder Create()
        {
            return new MetricBuilder();
        }

        public IMetric Build()
        {
            _mock.Setup(x => x.Count(It.IsAny<string>())).Callback(() => { });
            _mock.Setup(x => x.Gauge(It.IsAny<string>(), It.IsAny<int>())).Callback(() => { });
            _mock.Setup(x => x.Timer(It.IsAny<string>()));

            return _mock.Object;
        }
    }
}
