namespace FamilyTree.Core
{
    public static class PasswordEncryptionDecryption
    {
        public static string HashPassword(string password)
        {   
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
   
}
