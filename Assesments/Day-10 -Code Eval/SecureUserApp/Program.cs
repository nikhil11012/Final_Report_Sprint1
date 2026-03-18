using System;
using Serilog;
using SecureUserApp.Services;
using SecureUserApp.Helpers;

namespace SecureUserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure Serilog to log to both console and a file
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("=== Secure User Management Application Started ===");

            try
            {
                var userService = new UserService();

                
                Console.WriteLine("\n--- Registering Users ---");

                bool reg1 = userService.Register("alice", "Alice@123", "alice@example.com");
                Console.WriteLine(reg1 ? "Alice registered successfully." : "Alice registration failed.");

                bool reg2 = userService.Register("bob", "Bob@456", "bob@example.com");
                Console.WriteLine(reg2 ? "Bob registered successfully." : "Bob registration failed.");

                
                bool dupReg = userService.Register("alice", "anotherPass");
                Console.WriteLine(dupReg ? "Duplicate register succeeded." : "Duplicate registration blocked (expected).");

                
                Console.WriteLine("\n--- Login Attempts ---");

                bool login1 = userService.Login("alice", "Alice@123");
                Console.WriteLine(login1 ? "Alice logged in successfully." : "Alice login failed.");

                bool loginWrong = userService.Login("alice", "wrongPass");
                Console.WriteLine(loginWrong ? "Login succeeded (unexpected)." : "Login blocked with wrong password (expected).");

                bool loginBob = userService.Login("bob", "Bob@456");
                Console.WriteLine(loginBob ? "Bob logged in successfully." : "Bob login failed.");

                
                Console.WriteLine("\n--- Decrypting User Details ---");

                string aliceDetails = userService.GetDecryptedDetails("alice");
                Console.WriteLine($"Alice's decrypted detail: {aliceDetails}");

                
                Console.WriteLine("\n--- Standalone Encryption Demo ---");
                string originalText  = "SensitiveData_42";
                string encrypted     = EncryptionHelper.EncryptData(originalText);
                string decrypted     = EncryptionHelper.DecryptData(encrypted);

                Console.WriteLine($"Original  : {originalText}");
                Console.WriteLine($"Encrypted : {encrypted}");
                Console.WriteLine($"Decrypted : {decrypted}");
            }
            catch (Exception ex)
            {
                
                Log.Error(ex, "An unexpected error occurred in the application.");
                Console.WriteLine("Something went wrong. Please check the logs.");
            }
            finally
            {
                Log.Information("=== Application Shutting Down ===");
                Log.CloseAndFlush();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
