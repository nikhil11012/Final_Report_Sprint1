using System;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using SecureUserApp.Helpers;
using SecureUserApp.Services;

namespace SecureUserApp.Tests
{
    
    [TestFixture]
    public class UserManagementTests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userService = new UserService();
        }


        [Test]
        public void Register_WithValidDetails_ReturnsTrue()
        {
            bool result = _userService.Register("john", "Pass@123");

            ClassicAssert.IsTrue(result, "A valid registration should return true.");
        }

        [Test]
        public void Register_WithDuplicateUsername_ReturnsFalse()
        {
            _userService.Register("john", "Pass@123");

            bool result = _userService.Register("john", "AnotherPass");

            ClassicAssert.IsFalse(result, "Duplicate username registration should be blocked.");
        }

        [Test]
        public void Register_WithEmptyUsername_ReturnsFalse()
        {
            bool result = _userService.Register("", "Pass@123");

            ClassicAssert.IsFalse(result, "An empty username should not be accepted.");
        }

        [Test]
        public void Register_WithEmptyPassword_ReturnsFalse()
        {
            bool result = _userService.Register("john", "");

            ClassicAssert.IsFalse(result, "An empty password should not be accepted.");
        }

        [Test]
        public void Login_WithCorrectCredentials_ReturnsTrue()
        {
            _userService.Register("jane", "Jane@789");

            bool result = _userService.Login("jane", "Jane@789");

            ClassicAssert.IsTrue(result, "Login with correct credentials should succeed.");
        }

        [Test]
        public void Login_WithWrongPassword_ReturnsFalse()
        {
            _userService.Register("jane", "Jane@789");

            bool result = _userService.Login("jane", "WrongPassword");

            ClassicAssert.IsFalse(result, "Login with an incorrect password should fail.");
        }

        [Test]
        public void Login_WithUnknownUser_ReturnsFalse()
        {
            bool result = _userService.Login("nobody", "Pass@123");

            ClassicAssert.IsFalse(result, "Login with a non-existent user should fail.");
        }

        [Test]
        public void HashPassword_SameInputProducesSameHash()
        {
            string hash1 = EncryptionHelper.HashPassword("myPassword");
            string hash2 = EncryptionHelper.HashPassword("myPassword");

            ClassicAssert.AreEqual(hash1, hash2, "The same password should always produce the same hash.");
        }

        [Test]
        public void HashPassword_DifferentInputsProduceDifferentHashes()
        {
            string hash1 = EncryptionHelper.HashPassword("password1");
            string hash2 = EncryptionHelper.HashPassword("password2");

            ClassicAssert.AreNotEqual(hash1, hash2, "Different passwords should not produce the same hash.");
        }

        [Test]
        public void HashPassword_IsNotSameAsPlainText()
        {
            string plain = "secret";
            string hash  = EncryptionHelper.HashPassword(plain);

            ClassicAssert.AreNotEqual(plain, hash, "The hashed value must not equal the original password.");
        }


        [Test]
        public void EncryptData_ReturnsEncryptedString()
        {
            string original  = "sensitiveInfo";
            string encrypted = EncryptionHelper.EncryptData(original);

            ClassicAssert.AreNotEqual(original, encrypted, "Encrypted text should differ from the original.");
        }

        [Test]
        public void DecryptData_ReturnsOriginalString()
        {
            string original  = "sensitiveInfo";
            string encrypted = EncryptionHelper.EncryptData(original);
            string decrypted = EncryptionHelper.DecryptData(encrypted);

            ClassicAssert.AreEqual(original, decrypted, "Decrypted text should match the original.");
        }

        [Test]
        public void EncryptAndDecrypt_WorksForEmptyString()
        {
            string original  = "";
            string encrypted = EncryptionHelper.EncryptData(original);
            string decrypted = EncryptionHelper.DecryptData(encrypted);

            ClassicAssert.AreEqual(original, decrypted, "Encrypting and decrypting an empty string should return an empty string.");
        }

        [Test]
        public void GetDecryptedDetails_AfterRegisterWithDetail_ReturnsCorrectDetail()
        {
            _userService.Register("alice", "AlicePass", "alice@example.com");

            string detail = _userService.GetDecryptedDetails("alice");

            ClassicAssert.AreEqual("alice@example.com", detail, "Decrypted detail should match what was stored during registration.");
        }

        [Test]
        public void DecryptData_WithInvalidInput_ThrowsException()
        {
           
            Assert.Throws<InvalidOperationException>(() =>
            {
                EncryptionHelper.DecryptData("this-is-not-valid-base64!!!");
            });
        }

        [Test]
        public void Login_WithNullUsername_ReturnsFalse()
        {
            bool result = _userService.Login(null, "somePass");

            ClassicAssert.IsFalse(result, "Login with null username should return false, not throw an exception.");
        }

        [Test]
        public void Register_WithNullPassword_ReturnsFalse()
        {
            bool result = _userService.Register("testUser", null);

            ClassicAssert.IsFalse(result, "Registration with null password should return false, not throw an exception.");
        }

        [Test]
        public void Register_ThenLogin_FullFlow_DoesNotThrow()
        {
            Assert.DoesNotThrow(() =>
            {
                _userService.Register("logUser", "Log@Pass1", "loguser@test.com");
                _userService.Login("logUser", "Log@Pass1");
                _userService.GetDecryptedDetails("logUser");
            });
        }
    }
}
