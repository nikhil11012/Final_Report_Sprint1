using DesignPatterns.App.Factory;
using DesignPatterns.App.Observer;
using DesignPatterns.App.Singleton;

Console.WriteLine("=== Singleton Pattern ===");
var logger1 = Logger.GetInstance();
var logger2 = Logger.GetInstance();
logger1.Log("Application started.");
logger2.Log("Another log message.");
Console.WriteLine($"Same instance: {ReferenceEquals(logger1, logger2)}");

Console.WriteLine("\n=== Factory Pattern ===");
var factory = new DocumentFactory();
var pdf = factory.CreateDocument("pdf");
var word = factory.CreateDocument("word");
pdf.Open();
word.Open();
Console.WriteLine($"PDF type: {pdf.DocumentType}");
Console.WriteLine($"Word type: {word.DocumentType}");

Console.WriteLine("\n=== Observer Pattern ===");
var station = new WeatherStation();
var display1 = new WeatherDisplay("Living Room Display");
var display2 = new WeatherDisplay("Kitchen Display");

station.Register(display1);
station.Register(display2);
station.SetWeatherData(22.5f, 65f);

station.Unregister(display2);
Console.WriteLine("Kitchen Display unregistered.");
station.SetWeatherData(25f, 70f);
