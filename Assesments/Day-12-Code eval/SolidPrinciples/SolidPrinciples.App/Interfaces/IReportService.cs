namespace SolidPrinciples.App.Interfaces
{
    public interface IReportService
    {
        void ProcessReport(string title, string content, string outputPath);
    }
}
