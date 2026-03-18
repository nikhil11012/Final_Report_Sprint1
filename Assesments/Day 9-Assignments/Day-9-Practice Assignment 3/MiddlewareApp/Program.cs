// Program.cs - Main entry point for the Middleware Application
// This file configures the middleware pipeline for request logging,
// error handling, static file serving, and basic security.

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Middleware 1: Enforce HTTPS redirection for all requests
// This ensures that any HTTP request is redirected to HTTPS
app.UseHttpsRedirection();

// Middleware 2: Custom request/response logging middleware
// Logs details about each incoming request and the response status code
app.Use(async (context, next) =>
{
    // Log the incoming request details
    Console.WriteLine($"[Request] {DateTime.Now} | Method: {context.Request.Method} | Path: {context.Request.Path}");

    // Call the next middleware in the pipeline
    await next();

    // Log the response status code after the request is processed
    Console.WriteLine($"[Response] {DateTime.Now} | Status Code: {context.Response.StatusCode}");
});

// Middleware 3: Custom error handling middleware
// Catches unhandled exceptions and returns a custom error page
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        // Log the error details to the console
        Console.WriteLine($"[Error] {DateTime.Now} | Exception: {ex.Message}");

        // Return a custom error page with a 500 status code
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/html";
        await context.Response.WriteAsync(@"
            <!DOCTYPE html>
            <html>
            <head><title>Error</title></head>
            <body>
                <h1>Something went wrong!</h1>
                <p>An unexpected error occurred. Please try again later.</p>
            </body>
            </html>
        ");
    }
});

// Middleware 4: Content Security Policy header
// Adds a CSP header to mitigate XSS attacks on static files
app.Use(async (context, next) =>
{
    // Add Content-Security-Policy header to every response
    // This restricts where scripts, styles, and other resources can be loaded from
    context.Response.Headers.Append("Content-Security-Policy",
        "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self'");

    await next();
});

// Middleware 5: Serve static files from the wwwroot folder
// This allows the app to serve HTML, CSS, and JavaScript files
app.UseStaticFiles();

// Default route - serves a welcome message at the root URL
app.MapGet("/", () => "Welcome to the Middleware Application! Visit /index.html for the static page.");

// Test route to demonstrate error handling middleware
app.MapGet("/error", () =>
{
    throw new Exception("This is a test exception to demonstrate error handling.");
});

app.Run();
