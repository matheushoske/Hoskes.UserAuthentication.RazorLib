using Hoskes.Account.Core;
using Hoskes.UserAuthorization.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hoskes.UserAuthorization.Services
{
    public class UserService
    {
        private readonly HoskesGatewayContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(HoskesGatewayContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> RegisterUserAsync(User user)
        {
            if (_context.User.Any(u => u.Email == user.Email))
                throw new BusinessException("Email already registered");
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

            // Password is correct; proceed with login
            // ...

            return user;
        }
    }
}
