/*Database Class
 * This class is meant to handle all database instantiation, singleton design,
 * private functions and API for front end.
 * One object of this class is called when its launched and is interacted with throughout the whole app
 *
 * Database is designed based off tutorial provided by Microsoft at https://docs.microsoft.com/en-us/xamarin/get-started/tutorials/local-database/?tabs=vswin
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
namespace Fit.Natic
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<DailyResults>().Wait();
        }

        public Task<List<DailyResults>> GetDailyTargetsListAsync()
        {
            return _database.Table<DailyResults>().ToListAsync();
        }

        public Task<int> SaveTargetAsync(DailyResults entry)
        {
            return _database.InsertAsync(entry);
        }

        /*Given two DateTime objects, the database will return a list of all the 
         * DailyResults stored in the db that fall within those dates, including
         * the beginning and ending dates
         */
        public async Task<List<DailyResults>> GetDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("------------Trying to get  db---------");
                var query = _database.Table<DailyResults>().Where(s => s.date <= endDate && s.date >=startDate);
                var result = await query.ToListAsync();
                return result;

            }
            catch(Exception f) {
                System.Diagnostics.Debug.WriteLine("------------GetDateRangeAsync db query error----------");
                return null;
            }
        }


        /*returns the number of entries in database
         */
        public int Rowcount()
        {
            return  _database.Table<DailyResults>().CountAsync().Result;
        }


    }


    // Daily Results class is the object that actually gets stored in the database
    // Its information comes from the stored json file
    public class DailyResults
    {
        [PrimaryKey]
        public DateTime date { get; set; }
        public int calorieTarget { get; set; }
        public float sleepTarget { get; set; }
        public int workoutTarget { get; set; }
        public int caloriesLogged { get; set; }
        public float sleepLogged { get; set; }
        public int workoutLogged { get; set; }
        public string notesLogged { get; set; }

    }

}
