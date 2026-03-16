using Microsoft.AspNetCore.SignalR;

namespace Fracto.Api.Hubs;

public class NotificationHub : Hub
{
    // Map UserId to ConnectionId for targeted notifications
    // Note: In a production app, you might use a more robust way 
    // to track user connections, but this works for basic real-time alerts.
    
    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public async Task BroadcastNewBooking(string message)
    {
        // Notify all Admins about a new booking
        await Clients.Group("Admins").SendAsync("ReceiveAdminNotification", message);
    }

    public override async Task OnConnectedAsync()
    {
        var user = Context.User;
        if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
        {
            // Robust check for Admin role (handles "Admin" string or "2" enum value)
            bool isAdmin = user.IsInRole("Admin") || 
                           user.IsInRole("2") || 
                           user.Claims.Any(c => (c.Type == System.Security.Claims.ClaimTypes.Role || c.Type == "role") && (c.Value == "Admin" || c.Value == "2"));

            if (isAdmin)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
                var name = user.Identity?.Name ?? "Administrator";
                await Clients.Caller.SendAsync("ReceiveNotification", $"Welcome, {name}. Monitoring live bookings... 🛡️");
            }
            else
            {
                var name = user.Identity?.Name ?? "User";
                await Clients.Caller.SendAsync("ReceiveNotification", $"Connected as {name}. 🔔");
            }
        }
        await base.OnConnectedAsync();
    }
}
