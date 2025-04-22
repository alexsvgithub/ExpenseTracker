using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Context;
using ExpenseTracker.Data.Interface;
using MongoDB.Driver;

namespace ExpenseTracker.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MongoDbContext _context;

        public UserRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<string> RegisterNewUser(User user)
        {
            try
            {
                if(await IsEmailIdAlreadyPresent(user.emailId))
                {
                    return "Email Id Already Present, Try with other Email id.";
                }

                // Insert the user into the database
                await _context.Users.InsertOneAsync(user);

                // If no exception was thrown, the insertion was successful
                return "User registered successfully.";
            }
            catch (MongoException ex)
            {
                // Log the exception (you can log this to a file or monitoring tool)
                return $"Error during user registration: {ex.Message}";
            }
            catch (Exception ex)
            {
                // Catch any other exceptions (for example, network issues)
                return $"An unexpected error occurred: {ex.Message}";
            }
        }

        private async Task<bool> IsEmailIdAlreadyPresent(string email)
        {
            var filter = Builders<User>.Filter.Eq(e => e.emailId, email);
            var existingUser = _context.Users.Find(filter).FirstOrDefaultAsync();
            return existingUser != null;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.Find(u => u.emailId == email).FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var sort = Builders<User>.Sort.Descending(e => e.CreatedAt);
            return await _context.Users.Find(u => u._id == id).Sort(sort).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var result = await _context.Users.ReplaceOneAsync(u => u._id == user._id, user);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
