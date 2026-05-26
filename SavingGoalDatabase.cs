using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using FinalApp.Models; 
using Microsoft.Maui.Storage; // for FileSystem.AppDataDirectory

namespace FinalApp
{
    public static class SavingGoalDatabase
    {
        private static SQLiteAsyncConnection _database;

        // Initialize the database and create table if it doesn't exist
        public static async Task Init()
        {
            if (_database != null)
                return;

            // Get path to the database file
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "SavingGoals.db");

            // Create connection
            _database = new SQLiteAsyncConnection(dbPath);

            // Create table if not exists
            await _database.CreateTableAsync<SavingGoal>();
        }

        // Add a new saving goal
        public static Task<int> AddGoalAsync(SavingGoal goal)
        {
            return _database.InsertAsync(goal);
        }

        // Get all saving goals
        public static Task<List<SavingGoal>> GetGoalsAsync()
        {
            return _database.Table<SavingGoal>().ToListAsync();
        }

        // Optionally add update and delete methods as needed
    }

}
