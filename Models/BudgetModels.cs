using SQLite;
using System;

namespace FinalApp.Models
{
    public class BudgetModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string UserEmail { get; set; }
        public double Income { get; set; }
        public double Rent { get; set; }
        public double Grocery { get; set; }
        public double Transport { get; set; }
        public double Utilities { get; set; }
        public double Debt { get; set; }
        public double Savings { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
