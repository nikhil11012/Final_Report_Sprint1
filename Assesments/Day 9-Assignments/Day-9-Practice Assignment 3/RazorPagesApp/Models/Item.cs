// Models/Item.cs - Simple Item model class
// This class represents an item with a Name and Description

namespace RazorPagesApp.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
