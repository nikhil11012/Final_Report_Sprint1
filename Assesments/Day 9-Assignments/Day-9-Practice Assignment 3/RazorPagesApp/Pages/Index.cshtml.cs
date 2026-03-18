using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesApp.Data;
using RazorPagesApp.Models;

namespace RazorPagesApp.Pages
{
    // PageModel class for the Index page (Items List)
    // This class handles fetching the list of items to display
    public class IndexModel : PageModel
    {
        // Property to hold the list of items for the page
        public List<Item> Items { get; set; } = new List<Item>();

        // OnGet handler - runs when the page is loaded (GET request)
        // Fetches all items from the in-memory store and assigns them to the Items property
        public void OnGet()
        {
            // Load items from the static store
            Items = ItemStore.Items;
        }
    }
}
