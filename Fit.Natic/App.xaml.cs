using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fit.Natic
{
    public partial class App : Application
    {
        static Database database = App.Database;
        public static User appUser;
        public static DailyTarget todaysTarget;
        public static bool firstTimeLaunched;
        public static bool profileInfoEntered;
        public static bool statsPageViewed;
        public static bool resourcesPageViewed;
        /*This Database object has a singleton design so that only one exists
         * and runs continuously when the app is running
         */
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DailyResults.db3"));
                }
                return database;
            }
        }

        public App()
        {
            InitializeComponent();
            loadData();
         //   appUser.setDailyTarget(todaysTarget);
            MainPage = new NavPage();
            //if its the first time the app is launched
            if(firstTimeLaunched == true)
            {
                //push a edit profile page to the hierarchy to have them fill out their profile info

                //then change to first time launched = false;
                App.firstTimeLaunched = false;
            }

        }

        /*Loads the user data from the json file, then checks the date 
         * if the information stored in the json file is from the previous day
         * (if todays date > date stored in json file)
         *      read in the information to a user object, create a DailyResults object,
         *      and store the daily results object in the database, then clear
         *      the daily results and increment the date to start a new slate for the day.
         * The new updated User object and daily target object should be written back to json
         */
        public void loadData()
        {
 
            //read user info and daily target stored in json file
            appUser = User.readFromJson();
            todaysTarget = appUser.getDailyTarget();

            System.Diagnostics.Debug.WriteLine("----------launched app.xaml.xs appUser-----------");
            System.Diagnostics.Debug.WriteLine(appUser.ToString());
            System.Diagnostics.Debug.WriteLine("----------launched app.xaml.xs todaysTarget-----------");
            System.Diagnostics.Debug.WriteLine(todaysTarget.toString());

            //check to see if the date stored is from yesterday or not
            if (appUser.getDailyTarget().date.Date < DateTime.Today.Date)
            {
                //create a new DailyResults object from the info saved
                DailyResults resultToSave = new DailyResults {
                    date = todaysTarget.date.Date,
                    calorieTarget = todaysTarget.calorieTarget,
                    sleepTarget = todaysTarget.sleepTarget,
                    workoutTarget = todaysTarget.workoutTarget,
                    caloriesLogged = todaysTarget.actualCalories,
                    sleepLogged = todaysTarget.actualSleep,
                    workoutLogged = todaysTarget.actualWorkout,
                    notesLogged = todaysTarget.getNotes()
                };

                // store daily results in database
                database.SaveTargetAsync(resultToSave);

                //reset the date for the Daily Target
                todaysTarget.date = DateTime.Today;

                //reset all the logged info and notes, but not the target info
                todaysTarget.resetLoggedInfo();

                //assign todays target back to the appUser and this function saves it to json also
                appUser.setDailyTarget(todaysTarget);

                //store the updated info back json -> redundant, setDailyTarget already saves it to json
                //appUser.saveToJsonAsync();

            }
            else if(appUser.getDailyTarget().date.Date == DateTime.Today.Date)
            {

            }
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            //the todaysTarget object could have been modified, which needs to be reassigned back to the user and saved to json
            appUser.setDailyTarget(todaysTarget);
        }

        protected override void OnResume()
        {
        }
    }
}
