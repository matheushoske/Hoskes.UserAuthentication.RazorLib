using Hoskes.Account.Core;
using Hoskes.UserAuthorization.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Hoskes.UserAuthorization.Services
{
    public class UserService
    {
        private readonly HoskesAuthContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(HoskesAuthContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> RegisterUserAsync(User user)
        {
            if (_context.User.Any(u => u.Email == user.Email))
                throw new BusinessException("Email already registered");
            ValidateStrongPassword(user.Password);
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<User> LoginAsync(string email, string password)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new BusinessException("Invalid login credentials.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            if (result == PasswordVerificationResult.Failed)
                throw new BusinessException("Invalid login credentials.");

            return user;
        }
        public void ValidateStrongPassword(string password)
        {
            if (password.Length < 8)
                throw new BusinessException("Password must be at least 8 characters long.");
            // Check for at least one lowercase letter
            if (!Regex.IsMatch(password, @"[a-z]"))
                throw new BusinessException("Password must contain at least one lowercase letter.");
            // Check for at least one uppercase letter
            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new BusinessException("Password must contain at least one uppercase letter.");
            // Check for at least one number
            if (!Regex.IsMatch(password, @"\d"))
                throw new BusinessException("Password must contain at least one number.");
            // Check for at least one special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+={}\[\]:;,.<>?/-]"))
                throw new BusinessException("Password must contain at least one special character.");
        }
    }
}
