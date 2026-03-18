namespace DesignPatterns.App.Observer
{
    public class WeatherStation
    {
        private readonly List<IWeatherObserver> _observers = new();
        private float _temperature;
        private float _humidity;

        public void Register(IWeatherObserver observer)
        {
            _observers.Add(observer);
        }

        public void Unregister(IWeatherObserver observer)
        {
            _observers.Remove(observer);
        }

        public void SetWeatherData(float temperature, float humidity)
        {
            _temperature = temperature;
            _humidity = humidity;
            NotifyAll();
        }

        private void NotifyAll()
        {
            foreach (var observer in _observers)
            {
                observer.Update(_temperature, _humidity);
            }
        }
    }
}
