using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using FinalApp.Models;

namespace FinalApp.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection db;

        private async Task Init()
        {
            if (db != null)
                return;

            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "MyAppDB.db3");
                db = new SQLiteAsyncConnection(databasePath);
                await db.CreateTableAsync<Item>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database initialization failed: {ex.Message}");
            }
        }

        // === Item Table ===

        public async Task<List<Item>> GetItemsAsync()
        {
            await Init();
            return await db.Table<Item>().ToListAsync();
        }

        public async Task<Item> GetItemAsync(int id)
        {
            await Init();
            return await db.Table<Item>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> SaveItemAsync(Item item)
        {
            await Init();
            if (item.Id != 0)
                return await db.UpdateAsync(item);
            else
                return await db.InsertAsync(item);
        }

        public async Task<int> DeleteItemAsync(Item item)
        {
            await Init();
            return await db.DeleteAsync(item);
        }

        // === User Table ===

        public async Task InitializeUserTable()
        {
            await Init();
            await db.CreateTableAsync<User>();
        }

        public async Task<bool> AddUser(User user)
        {
            await InitializeUserTable();
            try
            {
                await db.InsertAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            await InitializeUserTable();
            return await db.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ValidateUserCredentials(string email, string password)
        {
            var user = await GetUserByEmail(email);
            return user != null && user.Password == password;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            await InitializeUserTable();
            try
            {
                await db.UpdateAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        // === Budget Table ===

        public async Task InitializeBudgetTable()
        {
            await Init();
            await db.CreateTableAsync<BudgetModel>();
        }

        public async Task<bool> SaveBudgetAsync(BudgetModel budget)
        {
            await InitializeBudgetTable();
            try
            {
                await db.InsertAsync(budget);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving budget: {ex.Message}");
                return false;
            }
        }

        public async Task<List<BudgetModel>> GetAllBudgetsAsync()
        {
            await InitializeBudgetTable();
            return await db.Table<BudgetModel>().OrderByDescending(b => b.CreatedAt).ToListAsync();
        }

        // ✅ Fixed version: Get budgets by user email using BudgetModel
        public async Task<List<BudgetModel>> GetBudgetsForUserAsync(string email)
        {
            await InitializeBudgetTable();
            return await db.Table<BudgetModel>()
                           .Where(b => b.UserEmail == email)
                           .OrderByDescending(b => b.CreatedAt)
                           .ToListAsync();
        }


        // === Saving Goals ===

        public async Task InitializeSavingGoalTable()
        {
            await Init();
            await db.CreateTableAsync<SavingGoal>();
        }

        public async Task<bool> AddSavingGoalAsync(SavingGoal goal)
        {
            await InitializeSavingGoalTable();
            try
            {
                await db.InsertAsync(goal);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding saving goal: {ex.Message}");
                return false;
            }
        }

        public async Task<List<SavingGoal>> GetSavingGoalsAsync()
        {
            await InitializeSavingGoalTable();
            return await db.Table<SavingGoal>().ToListAsync();
        }

        // === Debts ===

        public async Task InitializeDebtTable()
        {
            await Init();
            await db.CreateTableAsync<Debt>();
        }

        public async Task<bool> AddDebtAsync(Debt debt)
        {
            await InitializeDebtTable();
            try
            {
                await db.InsertAsync(debt);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding debt: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Debt>> GetDebtsAsync()
        {
            await InitializeDebtTable();
            return await db.Table<Debt>().ToListAsync();
        }

        public async Task<bool> DeleteDebtAsync(int id)
        {
            await InitializeDebtTable();
            int result = await db.DeleteAsync<Debt>(id);
            return result > 0;
        }


    }
}
