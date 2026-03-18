namespace DesignPatterns.App.Factory
{
    public interface IDocument
    {
        void Open();
        string DocumentType { get; }
    }
}
