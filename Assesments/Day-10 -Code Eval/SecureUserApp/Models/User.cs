namespace SecureUserApp.Models
{
    
    public class User
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }

        
        public string EncryptedDetails { get; set; }

        public User(string username, string hashedPassword, string encryptedDetails = "")
        {
            Username = username;
            HashedPassword = hashedPassword;
            EncryptedDetails = encryptedDetails;
        }
    }
}
