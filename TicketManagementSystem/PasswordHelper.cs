using Microsoft.AspNetCore.Identity;
using TicketManagementSystem.Models;

namespace TicketManagementSystem
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var hasher = new PasswordHasher<Object>();
            return hasher.HashPassword(null, password);
        }

        public static bool VerifyPassword(string hashedPassword, string plainPassword)
        {
            var hasher = new PasswordHasher<Object>();
            var result = hasher.VerifyHashedPassword(null, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
