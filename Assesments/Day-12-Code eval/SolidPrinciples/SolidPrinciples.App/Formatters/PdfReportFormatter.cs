using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Formatters
{
    public class PdfReportFormatter : IReportFormatter
    {
        public string Format(string content)
        {
            return $"[PDF FORMAT]\n{content}";
        }
    }
}
