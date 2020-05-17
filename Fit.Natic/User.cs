using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace Fit.Natic
{
    /* User class is instantiated when app is loaded or when user page is loaded,
     * variables are read in from json, and when edited, saved to json
     *
     * it is important that the data gets written back to json if it has been edited
     *  this may need need to be checked in the PageAppearing/Disappearing methods
     */

    public class User
    {
        public string name { get; set; }
        public int age { get; set; }
        public string gender { get; set; }
        public float weight { get; set; } // in lbs
        public float height { get; set; } // in inches
        public DailyTarget userTarget { get; set; }
        
        /* readFromJson reads in the temporarily stored information
         * on the users info, their daily target, and goals achieved.
         *
         */
        public static User readFromJson()
        {
            var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userInfo.json");
            try
            {
                User user = JsonConvert.DeserializeObject<User>(File.ReadAllText(filePath));
                App.firstTimeLaunched = false;
                App.profileInfoEntered = true;
                App.resourcesPageViewed = true;
                App.statsPageViewed = true;
                return user;

            }
            catch (Exception e)
            {
                //if the file cant be found, the app assumes its being launched for the first time
                App.firstTimeLaunched = true;
                App.profileInfoEntered = false;
                App.resourcesPageViewed = false;
                App.statsPageViewed = false;
                System.Diagnostics.Debug.WriteLine("----------couldnt find user info json file, creating a new user object-----------");
                
                User user = new User();
                user.name = "New User";
                user.age = 0;
                user.gender = "unknown";
                user.height = 0;
                user.weight = 0;
                user.userTarget = new DailyTarget();
                user.userTarget.sleepTarget = 0;
                user.userTarget.calorieTarget = 0;
                user.userTarget.workoutTarget = 0;

                return user;
            }
            
        }


        public async System.Threading.Tasks.Task<bool> saveToJsonAsync()
        {
            string data = JsonConvert.SerializeObject(this);
            try
            {
                var filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "userInfo.json");
                using (var writer = File.CreateText(filePath))
                {
                    await writer.WriteLineAsync(data);
                }
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        /* Would be used by front end if the user makes any changes to their daily target
         * Should take in a value for each of the daily target values and update all of them.
         */
        public void setDailyTarget(DailyTarget newTarget)
        {
            this.userTarget = newTarget;
            this.saveToJsonAsync();
        }


        public DailyTarget getDailyTarget()
        {
            return this.userTarget;
        }

    }
}
