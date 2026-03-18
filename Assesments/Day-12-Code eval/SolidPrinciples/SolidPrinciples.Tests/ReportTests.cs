using SolidPrinciples.App.Formatters;
using SolidPrinciples.App.Models;
using SolidPrinciples.App.Services;

namespace SolidPrinciples.Tests
{
    public class ReportGeneratorTests
    {
        [Fact]
        public void GenerateReport_ReturnsCorrectString()
        {
            var generator = new ReportGenerator();
            var result = generator.GenerateReport("Test", "Body");
            Assert.Contains("Test", result);
            Assert.Contains("Body", result);
        }
    }

    public class ReportSaverTests
    {
        [Fact]
        public void SaveReport_WritesContentToFile()
        {
            var saver = new ReportSaver();
            var path = Path.GetTempFileName();
            saver.SaveReport("hello", path);
            var content = File.ReadAllText(path);
            Assert.Equal("hello", content);
            File.Delete(path);
        }
    }

    public class PdfFormatterTests
    {
        [Fact]
        public void Format_AddsPdfHeader()
        {
            var formatter = new PdfReportFormatter();
            var result = formatter.Format("some content");
            Assert.Contains("[PDF FORMAT]", result);
            Assert.Contains("some content", result);
        }
    }

    public class ExcelFormatterTests
    {
        [Fact]
        public void Format_AddsExcelHeader()
        {
            var formatter = new ExcelReportFormatter();
            var result = formatter.Format("data");
            Assert.Contains("[EXCEL FORMAT]", result);
        }
    }

    public class ReportServiceTests
    {
        [Fact]
        public void ProcessReport_CreatesFormattedFile()
        {
            var generator = new ReportGenerator();
            var formatter = new PdfReportFormatter();
            var saver = new ReportSaver();
            var service = new ReportService(generator, formatter, saver);
            var path = Path.GetTempFileName();
            service.ProcessReport("My Report", "Content here", path);
            var content = File.ReadAllText(path);
            Assert.Contains("[PDF FORMAT]", content);
            Assert.Contains("My Report", content);
            File.Delete(path);
        }
    }

    public class ReportHierarchyTests
    {
        [Fact]
        public void SalesReport_CanBeUsedAsBaseReport()
        {
            Report report = new SalesReport("Sales", "Q1", 5000m);
            Assert.Equal("Sales", report.Title);
        }

        [Fact]
        public void SummaryReport_CanBeUsedAsBaseReport()
        {
            Report report = new SummaryReport("Summary", "March", 100);
            Assert.Equal("Summary", report.Title);
        }

        [Fact]
        public void SalesReport_Export_WritesFile()
        {
            var sr = new SalesReport("S", "C", 100m);
            var path = Path.GetTempFileName();
            sr.Export(path);
            Assert.True(File.Exists(path));
            File.Delete(path);
        }
    }
}
