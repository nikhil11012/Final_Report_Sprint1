using DesignPatterns.App.Factory;

namespace DesignPatterns.App.Factory
{
    public class PdfDocument : IDocument
    {
        public string DocumentType => "PDF";

        public void Open()
        {
            Console.WriteLine("Opening PDF document.");
        }
    }

    public class WordDocument : IDocument
    {
        public string DocumentType => "Word";

        public void Open()
        {
            Console.WriteLine("Opening Word document.");
        }
    }

    public class DocumentFactory
    {
        public IDocument CreateDocument(string type)
        {
            return type.ToLower() switch
            {
                "pdf" => new PdfDocument(),
                "word" => new WordDocument(),
                _ => throw new ArgumentException($"Unknown document type: {type}")
            };
        }
    }
}
