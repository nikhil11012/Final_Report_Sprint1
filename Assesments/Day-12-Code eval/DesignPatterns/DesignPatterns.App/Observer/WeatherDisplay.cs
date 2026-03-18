namespace DesignPatterns.App.Observer
{
    public class WeatherDisplay : IWeatherObserver
    {
        public string Name { get; }
        public float LastTemperature { get; private set; }
        public float LastHumidity { get; private set; }

        public WeatherDisplay(string name)
        {
            Name = name;
        }

        public void Update(float temperature, float humidity)
        {
            LastTemperature = temperature;
            LastHumidity = humidity;
            Console.WriteLine($"{Name} - Temp: {temperature}°C, Humidity: {humidity}%");
        }
    }
}
