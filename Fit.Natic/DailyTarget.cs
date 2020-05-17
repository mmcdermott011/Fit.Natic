using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Schema;
using DateTime = System.DateTime;

namespace Fit.Natic
{
    
    public class DailyTarget
    {
        public DateTime date;

        public int calorieTarget;
        public float sleepTarget;
        public int workoutTarget;

        public Workout workout;
        public Sleep sleep;
        public List<Meal> meals;

        // not sure if these 3 are needed or should be in Performance
        public int actualCalories; 
        public float actualSleep;
        public int actualWorkout;


        public DailyTarget()
        {
            this.calorieTarget = 0;
            this.sleepTarget = 0;
            this.workoutTarget = 0;

            this.meals = new List<Meal>();
            this.workout = new Workout();
            this.sleep = new Sleep();
            this.actualCalories = 0;
            this.actualSleep = 0;
            this.actualWorkout = 0;

        }

        /*Adds a meal to the generic list of meals
         *  Takes the name of the meal as a string, number of calories as integer,
         *  and a string of the meal notes as parameters
         */
        public void logMeal(string name, int cals, string notes)
        {
            this.meals.Add(new Meal { mealName = name, mealCalories = cals, notes = notes , mealTime = DateTime.Now});
            //updating the current days calorie intake
            this.actualCalories += cals;
        }


        /* removes a meal from the generic list of meals
         *  returns true if successful, false if error
         */
        public Boolean removeMeal(Meal meal)
        {
            try
            {
                this.meals.Remove(meal);
                //updating the current days calorie intake
                this.actualCalories -= meal.mealCalories;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /*Not sure if getMeal is going to be needed?
         */
        public List<Meal> getMeals()
        {
            return this.meals;
        }

        /* iterates through the arraylist of meals and 
         * grabs each meals notes and appends it to a string
         */
        public string getMealNotes()
        {
            string allMealNotes = "";
            foreach (Meal meal in meals)
            {
                allMealNotes = allMealNotes + meal.mealName + ": " + meal.notes + "\n ";
            }
            return allMealNotes;
        }

        /* Logs the sleep duration and any notes about the sleep
         */
        public void logSleep(float sleepDur, string notes)
        {
            if(sleepDur >= 0 && actualSleep >=0)
            {
                this.actualSleep = sleepDur;
                this.sleep.notes = notes;
            }
            else
            {
                //print error message saying you cant have negative sleep
            }
        }

        /* just grabs the workout object from DailyTarget
         */
        public Workout getWorkout() {

            return this.workout;
        }

        /* lets you update the workout information
         */
        public bool logWorkout(string type, int duration, string notes) {
            try
            {
                this.actualWorkout += duration;
                this.workout.workoutType = type;
                this.workout.duration = this.actualWorkout;
                this.workout.notes = notes;
                return true;
            }
            catch (Exception s)
            {
                System.Diagnostics.Debug.WriteLine("--------ERROR COULDNT LOG WORKOUT-------------");
                return false;
            }
        }



        /* returns just the notes from meal, sleep and workout
         */
        public string getNotes()
        {
            string allNotes;
            allNotes =  "Meals: \n" + this.getMealNotes() + "\n" +
                    "Workout: " + this.workout.notes + "\n" +  "Sleep: " +
                    this.sleep.notes;
            return allNotes;
        }

        /* resets all logged information for the day to zero
         * actualCalories, actualSleep, actualWorkout, meals, and all notes 
         */
        public void resetLoggedInfo()
        {
            this.actualCalories = 0;
            this.actualSleep = 0;
            this.actualWorkout = 0;
            this.workout = new Workout();
            this.meals = new List<Meal>();
            this.sleep = new Sleep();
        }


        /*returns the whole toString of the object formatted 
         */
        public string toString()
        {
            return "Daily Target - " + DateTime.Now.ToShortDateString() + " \n"
                    + "Calorie Consumption Target: " + calorieTarget + " \t Actual Consumed: " + actualCalories + "\n"
                        + "\t notes: " + this.getMealNotes() + "\n"
                    + "Workout Target: " + workoutTarget + " \t Actual: " + actualWorkout + "\n"
                        + "\t notes: " + this.workout.notes + "\n"
                    + "Sleep Target: " + sleepTarget + " \t Actual: " + actualSleep + "\n"
                        + "\t notes: " + this.sleep.notes + "\n";
        }

    }

    public class Meal
    {
        public string mealName { get; set; }
        public string notes { get; set; }
        public int mealCalories { get; set; }
        public DateTime mealTime { get; set; }
    }


    public class Workout
    {
        public string workoutType { get; set; }
        public int duration { get; set; }
        public string notes { get; set; }

    }

    public class Sleep
    {
        public float duration { get; set; }
        public string notes { get; set; }

    }

    public class Performance
    {
        public float CalorieDeficit;
        public float WorkoutDeficit;
        public float SleepDeficit;

        public Performance() { }

        public Performance(int calorieDeficit, int workoutDeficit, float sleepDeficit)
        {
            this.CalorieDeficit = calorieDeficit;
            this.WorkoutDeficit = workoutDeficit;
            this.SleepDeficit = sleepDeficit;
        }

        public virtual void CalcPerformance(int calorieTarget, int workoutTarget, float sleepTarget, int actualCalories,
            int actualWorkout, float actualSleep)
        {
            this.CalorieDeficit = 0;
            this.WorkoutDeficit = 0;
            this.SleepDeficit = 0;

            if (calorieTarget != 0) { this.CalorieDeficit = (Convert.ToSingle( actualCalories)/ calorieTarget)*100; }
            if (workoutTarget != 0) { this.WorkoutDeficit = (Convert.ToSingle( actualWorkout) / workoutTarget) * 100; }
            if (sleepTarget != 0) { this.SleepDeficit = (actualSleep / sleepTarget) * 100; }
        }

        public class Daily : Performance
        {
            public Daily(int calorie, int workout, float sleep) : base(calorie, workout, sleep)
            { }

            public void CalcPerformance()
            {
                System.Diagnostics.Debug.WriteLine("------------CALC DAILY PERFORMANCE----------");

                //Total daily performance up to current day in month
         

                if (App.todaysTarget.calorieTarget > 0) { this.CalorieDeficit = ((Convert.ToSingle(App.todaysTarget.actualCalories)) / App.todaysTarget.calorieTarget) * 100; }
                if (App.todaysTarget.workoutTarget > 0) { this.WorkoutDeficit = ((Convert.ToSingle(App.todaysTarget.actualWorkout)) / App.todaysTarget.workoutTarget) * 100; }
                if (App.todaysTarget.sleepTarget > 0) { this.SleepDeficit = ((Convert.ToSingle(App.todaysTarget.actualSleep)) / App.todaysTarget.sleepTarget) * 100; }

                System.Diagnostics.Debug.WriteLine("caloriedef " + this.CalorieDeficit.ToString());
                System.Diagnostics.Debug.WriteLine("sleepdef " + this.SleepDeficit.ToString());
                System.Diagnostics.Debug.WriteLine("workdef " + this.WorkoutDeficit.ToString());

            }
        }

        public class Weekly : Performance
        {
            public Weekly(int calorie, int workout, float sleep) : base(calorie, workout, sleep)
            { }

            /*This method gets the current day, figures out what day in the week
             * it is, then gets the date for the start of that week
             * and queries the database for all the DailyResults from the
             * start of that week, up to the current day. It receives the DailyResults
             * objects in list, which it iterates through, calculating the difference
             * between each days targets and actual results, adding them up
             *
             */
            public async Task CalculateWeekly()
            {
                //get which day of the week it is
                DayOfWeek todaysDate = DateTime.Today.DayOfWeek;

                // number of how many days to go back
                int total = 7 - (int)todaysDate;

                // get the date for however many days back
                DateTime otherDate = DateTime.Today.AddDays(-(int)todaysDate).Date;

                //Call Database method to get list of results within date range
                System.Diagnostics.Debug.WriteLine("------------calling getDateRange from calculateWeekly----------");
                List<DailyResults> results = await App.Database.GetDateRange(otherDate, DateTime.Today.Date);

                //Total daily performance up to current day in month
                int tempCalLogged = 0, tempCalTarget = 0, tempWorkLogged = 0, tempWorkTarget = 0;
                float tempSleepLogged = 0, tempSleepTarget = 0;

                foreach (DailyResults day in results)
                {
                    //   int tempDayCalorieDeficit = day.caloriesLogged - day.calorieTarget;
                    //   int tempDayWorkoutDeficit = day.workoutLogged - day.workoutTarget;
                    //   float tempDaySleepDeficit = day.sleepLogged - day.sleepTarget;

                    //   this.CalorieDeficit += tempDayCalorieDeficit;
                    //   this.WorkoutDeficit += tempDayWorkoutDeficit;
                    //   this.SleepDeficit += tempDaySleepDeficit;

                    tempCalLogged += day.caloriesLogged;
                    tempCalTarget += day.calorieTarget;
                    tempWorkLogged += day.workoutLogged;
                    tempWorkTarget += day.workoutTarget;
                    tempSleepLogged += day.sleepLogged;
                    tempSleepTarget += day.sleepTarget;

                }
                tempCalLogged += App.todaysTarget.actualCalories;
                tempCalTarget += App.todaysTarget.calorieTarget;
                tempWorkLogged += App.todaysTarget.actualWorkout;
                tempWorkTarget += App.todaysTarget.workoutTarget;
                tempSleepLogged += App.todaysTarget.actualSleep;
                tempSleepTarget += App.todaysTarget.sleepTarget;

                System.Diagnostics.Debug.WriteLine("------CALC WEEKLY RESULTS----------------");
                System.Diagnostics.Debug.WriteLine("tempCalLogged " + tempCalLogged.ToString() + " ---- " + "Target: " + tempCalTarget.ToString());
                System.Diagnostics.Debug.WriteLine("tempWorkLogged " + tempWorkLogged.ToString() + " ---- " + "Target: " + tempWorkTarget.ToString());
                System.Diagnostics.Debug.WriteLine("tempSleepLogged " + tempSleepLogged.ToString() + " ---- " + "Target: " + tempSleepTarget.ToString());
                

                if (tempCalTarget > 0) { this.CalorieDeficit = ((Convert.ToSingle( tempCalLogged) / tempCalTarget)/(7) * 100) ; }
                if (tempWorkTarget > 0) { this.WorkoutDeficit = ((Convert.ToSingle(tempWorkLogged) / tempWorkTarget)/(7) * 100)  ; }
                if (tempSleepTarget > 0) { this.SleepDeficit = ((tempSleepLogged / tempSleepTarget)/(7) * 100) ; }

                System.Diagnostics.Debug.WriteLine("caloriedef " + this.CalorieDeficit.ToString());
                System.Diagnostics.Debug.WriteLine("sleepdef " + this.SleepDeficit.ToString());
                System.Diagnostics.Debug.WriteLine("workdef " + this.WorkoutDeficit.ToString());
            }

        }


        public class Monthly : Performance
        {
            
            public Monthly(int calorie, int workout, float sleep) : base(calorie, workout, sleep)
            { }
            //ADD FUNCTION TO FIND CURRENT MONTH FROM DATE
            //TOTAL ALL DAILY TARGETS IN MONTH

            public async Task CalculateMonthly()
            {
                int todaysDate = (int) DateTime.Today.Day;
                int month = (int) DateTime.Now.Month;
                int numDays = 0;

                //Possibly wont use
                switch (month)
                {
                    case 1:
                        numDays = 31;
                        break;
                    case 2:
                        numDays = 29;
                        break;
                    case 3:
                        numDays = 31;
                        break;
                    case 4:
                        numDays = 30;
                        break;
                    case 5:
                        numDays = 31;
                        break;
                    case 6:
                        numDays = 30;
                        break;
                    case 7:
                        numDays = 31;
                        break;
                    case 8:
                        numDays = 31;
                        break;
                    case 9:
                        numDays = 30;
                        break;
                    case 10:
                        numDays = 31;
                        break;
                    case 11:
                        numDays = 30;
                        break;
                    case 12:
                        numDays = 31;
                        break;
                }


                //calculate first day
                DateTime otherDate = DateTime.Today.AddDays(-(int)todaysDate).Date;

                //Retrieve daily results from database
                List<DailyResults> results = await App.Database.GetDateRange(otherDate, DateTime.Today.Date);

                //Total daily performance up to current day in month
                int tempCalLogged =0, tempCalTarget=0, tempWorkLogged=0, tempWorkTarget=0;
                float tempSleepLogged=0, tempSleepTarget=0;

                this.CalorieDeficit = 0;
                this.WorkoutDeficit = 0;
                this.SleepDeficit = 0;

                foreach (DailyResults day in results)
                {
                    //   int tempDayCalorieDeficit = day.caloriesLogged - day.calorieTarget;
                    //   int tempDayWorkoutDeficit = day.workoutLogged - day.workoutTarget;
                    //   float tempDaySleepDeficit = day.sleepLogged - day.sleepTarget;

                    //   this.CalorieDeficit += tempDayCalorieDeficit;
                    //   this.WorkoutDeficit += tempDayWorkoutDeficit;
                    //   this.SleepDeficit += tempDaySleepDeficit;

                    tempCalLogged += day.caloriesLogged;
                    tempCalTarget += day.calorieTarget;
                    tempWorkLogged += day.workoutLogged;
                    tempWorkTarget += day.workoutTarget;
                    tempSleepLogged += day.sleepLogged;
                    tempSleepTarget += day.sleepTarget;

                }
                tempCalLogged += App.todaysTarget.actualCalories;
                tempCalTarget += App.todaysTarget.calorieTarget;
                tempWorkLogged += App.todaysTarget.actualWorkout;
                tempWorkTarget += App.todaysTarget.workoutTarget;
                tempSleepLogged += App.todaysTarget.actualSleep;
                tempSleepTarget += App.todaysTarget.sleepTarget;

                System.Diagnostics.Debug.WriteLine("------CALC MONTHLY RESULTS----------------");
                System.Diagnostics.Debug.WriteLine("tempCalLogged" + tempCalLogged.ToString() + "----" + "Target: " + tempCalTarget.ToString());
                System.Diagnostics.Debug.WriteLine("tempWorkLogged" + tempWorkLogged.ToString() + "----" + "Target: " + tempWorkTarget.ToString());
                System.Diagnostics.Debug.WriteLine("tempSleepLogged" + tempSleepLogged.ToString() + "----" + "Target: " + tempSleepTarget.ToString());

                if (tempCalTarget > 0) { this.CalorieDeficit = (Convert.ToSingle( tempCalLogged )/ tempCalTarget)/numDays * 100; }
                if (tempWorkTarget > 0) { this.WorkoutDeficit = (Convert.ToSingle( tempWorkLogged) / tempWorkTarget)/numDays * 100; }
                if (tempSleepTarget > 0) { this.SleepDeficit = (tempSleepLogged / tempSleepTarget)/numDays * 100; }


                System.Diagnostics.Debug.WriteLine("caloriedef" + this.CalorieDeficit.ToString());
                System.Diagnostics.Debug.WriteLine("sleepdef" + this.SleepDeficit.ToString());
                System.Diagnostics.Debug.WriteLine("workdef" + this.WorkoutDeficit.ToString());

            }
        }
    }


}




