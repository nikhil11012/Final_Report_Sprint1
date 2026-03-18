// Data/ItemStore.cs - Static in-memory data store for items
// This class holds a shared list of items that all pages can access

using RazorPagesApp.Models;

namespace RazorPagesApp.Data
{
    // Static class to store items in memory
    // In a real application, this would be replaced with a database
    public static class ItemStore
    {
        // Static list to hold all items, shared across pages
        public static List<Item> Items { get; set; } = new List<Item>
        {
            // Adding some default items so the list is not empty at start
            new Item { Id = 1, Name = "Notebook", Description = "A spiral-bound notebook for notes" },
            new Item { Id = 2, Name = "Pen", Description = "A blue ballpoint pen" },
            new Item { Id = 3, Name = "Eraser", Description = "A white rubber eraser" }
        };

        // Counter to generate unique IDs for new items
        public static int NextId { get; set; } = 4;
    }
}
