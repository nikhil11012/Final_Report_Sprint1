using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages
{
    // PageModel class for the Add Item page
    // Handles form submission and adding new items to the store
    public class AddItemModel : PageModel
    {
        // BindProperty attribute allows two-way data binding
        // The form input values are automatically bound to these properties on POST
        [BindProperty]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        public string Description { get; set; } = string.Empty;

        // Message property to show feedback to the user after adding an item
        public string Message { get; set; } = string.Empty;

        // OnGet handler - runs when the page is loaded (GET request)
        public void OnGet()
        {
            // Nothing to do on GET, just display the form
        }

        // OnPost handler - runs when the form is submitted (POST request)
        // Creates a new item and adds it to the in-memory store
        public IActionResult OnPost()
        {
            // Check if the form data is valid
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Description))
            {
                Message = "Please fill in all fields.";
                return Page();
            }

            // Create a new item with the form data
            var newItem = new Item
            {
                Id = ItemStore.NextId,
                Name = Name,
                Description = Description
            };

            // Add the item to the store and increment the ID counter
            ItemStore.Items.Add(newItem);
            ItemStore.NextId++;

            // Set success message
            Message = $"Item '{Name}' was added successfully!";

            // Clear the form fields after adding
            Name = string.Empty;
            Description = string.Empty;

            return Page();
        }
    }
}
