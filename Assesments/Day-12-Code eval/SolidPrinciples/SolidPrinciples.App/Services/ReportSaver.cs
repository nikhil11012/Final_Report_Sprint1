using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Services
{
    public class ReportSaver : IReportSaver
    {
        public void SaveReport(string report, string filePath)
        {
            File.WriteAllText(filePath, report);
        }
    }
}
