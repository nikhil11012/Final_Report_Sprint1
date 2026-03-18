namespace DesignPatterns.App.Observer
{
    public interface IWeatherObserver
    {
        void Update(float temperature, float humidity);
    }
}
