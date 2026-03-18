using SolidPrinciples.App.Formatters;
using SolidPrinciples.App.Models;
using SolidPrinciples.App.Services;

var generator = new ReportGenerator();
var saver = new ReportSaver();

var pdfFormatter = new PdfReportFormatter();
var excelFormatter = new ExcelReportFormatter();

var pdfService = new ReportService(generator, pdfFormatter, saver);
var excelService = new ReportService(generator, excelFormatter, saver);

pdfService.ProcessReport("Sales Q1", "Revenue increased by 20%.", "pdf_report.txt");
excelService.ProcessReport("Inventory Summary", "1200 units in stock.", "excel_report.txt");

var salesReport = new SalesReport("Q1 Sales", "Sales data for Q1.", 95000m);
var summaryReport = new SummaryReport("Monthly Summary", "Summary for March.", 340);

var allReports = new List<Report> { salesReport, summaryReport };

foreach (var report in allReports)
{
    report.Print();
    Console.WriteLine("---");
}

salesReport.Export("sales_export.txt");

Console.WriteLine("All reports processed successfully.");
