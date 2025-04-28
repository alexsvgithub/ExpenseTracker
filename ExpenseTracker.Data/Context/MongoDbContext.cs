using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using ExpenseTracker.Core.Models;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Data.Context
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        
        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration["MongoSettings:ConnectionString"]);
            _database = client.GetDatabase(configuration["MongoSettings:Database"]);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("Transactions");

        public IMongoCollection<Categories> Categories => _database.GetCollection<Categories>("Categories");

    }
}
