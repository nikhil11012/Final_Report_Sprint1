using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportGenerator _generator;
        private readonly IReportFormatter _formatter;
        private readonly IReportSaver _saver;

        public ReportService(IReportGenerator generator, IReportFormatter formatter, IReportSaver saver)
        {
            _generator = generator;
            _formatter = formatter;
            _saver = saver;
        }

        public void ProcessReport(string title, string content, string outputPath)
        {
            var report = _generator.GenerateReport(title, content);
            var formatted = _formatter.Format(report);
            _saver.SaveReport(formatted, outputPath);
        }
    }
}
