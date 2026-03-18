using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Services
{
    public class ReportGenerator : IReportGenerator
    {
        public string GenerateReport(string title, string content)
        {
            return $"Report: {title}\n{content}";
        }
    }
}
