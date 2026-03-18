using DesignPatterns.App.Factory;
using DesignPatterns.App.Observer;
using DesignPatterns.App.Singleton;

namespace DesignPatterns.Tests
{
    public class LoggerTests
    {
        [Fact]
        public void GetInstance_ReturnsSameObject()
        {
            var a = Logger.GetInstance();
            var b = Logger.GetInstance();
            Assert.Same(a, b);
        }
    }

    public class DocumentFactoryTests
    {
        private readonly DocumentFactory _factory = new DocumentFactory();

        [Fact]
        public void CreateDocument_Pdf_ReturnsPdfDocument()
        {
            var doc = _factory.CreateDocument("pdf");
            Assert.Equal("PDF", doc.DocumentType);
        }

        [Fact]
        public void CreateDocument_Word_ReturnsWordDocument()
        {
            var doc = _factory.CreateDocument("word");
            Assert.Equal("Word", doc.DocumentType);
        }

        [Fact]
        public void CreateDocument_Unknown_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _factory.CreateDocument("txt"));
        }
    }

    public class WeatherStationTests
    {
        [Fact]
        public void SetWeatherData_NotifiesRegisteredObserver()
        {
            var station = new WeatherStation();
            var display = new WeatherDisplay("Test Display");
            station.Register(display);
            station.SetWeatherData(30f, 80f);
            Assert.Equal(30f, display.LastTemperature);
            Assert.Equal(80f, display.LastHumidity);
        }

        [Fact]
        public void Unregister_StopsReceivingUpdates()
        {
            var station = new WeatherStation();
            var display = new WeatherDisplay("Test");
            station.Register(display);
            station.SetWeatherData(20f, 50f);
            station.Unregister(display);
            station.SetWeatherData(99f, 99f);
            Assert.Equal(20f, display.LastTemperature);
        }

        [Fact]
        public void MultipleObservers_AllReceiveUpdate()
        {
            var station = new WeatherStation();
            var d1 = new WeatherDisplay("D1");
            var d2 = new WeatherDisplay("D2");
            station.Register(d1);
            station.Register(d2);
            station.SetWeatherData(18f, 60f);
            Assert.Equal(18f, d1.LastTemperature);
            Assert.Equal(18f, d2.LastTemperature);
        }
    }
}
