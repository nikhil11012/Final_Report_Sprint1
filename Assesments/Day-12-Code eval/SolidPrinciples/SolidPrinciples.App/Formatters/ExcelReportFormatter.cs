using SolidPrinciples.App.Interfaces;

namespace SolidPrinciples.App.Formatters
{
    public class ExcelReportFormatter : IReportFormatter
    {
        public string Format(string content)
        {
            return $"[EXCEL FORMAT]\n{content}";
        }
    }
}
