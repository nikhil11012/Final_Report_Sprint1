using System;
using System.Collections.Generic;
using SecureUserApp.Helpers;
using SecureUserApp.Models;
using Serilog;

namespace SecureUserApp.Services
{
    // Manages user registration and login
    public class UserService
    {
        // Simple in-memory store for demo purposes
        private readonly Dictionary<string, User> _userStore = new Dictionary<string, User>();

        // Registers a new user with a hashed password and encrypted details
        public bool Register(string username, string plainPassword, string additionalDetail = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword))
                {
                    Log.Warning("Registration attempted with empty username or password.");
                    return false;
                }

                if (_userStore.ContainsKey(username))
                {
                    Log.Warning("Registration failed: username '{Username}' already exists.", username);
                    return false;
                }

                string hashedPwd        = EncryptionHelper.HashPassword(plainPassword);
                string encryptedDetail  = string.IsNullOrWhiteSpace(additionalDetail)
                                            ? ""
                                            : EncryptionHelper.EncryptData(additionalDetail);

                var newUser = new User(username, hashedPwd, encryptedDetail);
                _userStore[username] = newUser;

                Log.Information("User '{Username}' registered successfully.", username);
                return true;
            }
            catch (Exception ex)
            {
                // Log the error but don't expose internal details outside
                Log.Error(ex, "An error occurred while registering user '{Username}'.", username);
                return false;
            }
        }

        // Verifies username + password against the stored hashed password
        public bool Login(string username, string plainPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(plainPassword))
                {
                    Log.Warning("Login attempted with empty username or password.");
                    return false;
                }

                if (!_userStore.TryGetValue(username, out User foundUser))
                {
                    Log.Warning("Login failed: user '{Username}' not found.", username);
                    return false;
                }

                string hashedInput = EncryptionHelper.HashPassword(plainPassword);

                if (hashedInput == foundUser.HashedPassword)
                {
                    Log.Information("User '{Username}' logged in successfully.", username);
                    return true;
                }
                else
                {
                    Log.Warning("Login failed for user '{Username}': incorrect password.", username);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during login for user '{Username}'.", username);
                return false;
            }
        }

        // Returns decrypted details of a registered user (only for authorized use)
        public string GetDecryptedDetails(string username)
        {
            if (!_userStore.TryGetValue(username, out User foundUser))
            {
                Log.Warning("Details requested for unknown user '{Username}'.", username);
                return string.Empty;
            }

            if (string.IsNullOrWhiteSpace(foundUser.EncryptedDetails))
                return string.Empty;

            return EncryptionHelper.DecryptData(foundUser.EncryptedDetails);
        }

        // Retrieves user by username (returns null if not found)
        public User GetUser(string username)
        {
            _userStore.TryGetValue(username, out User user);
            return user;
        }
    }
}
