using NUnit.Framework;
using Fit.Natic;
using System.Collections.Generic;

namespace BackEndTests
{
    public class UserTest1
    {
        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void saveToandReadFromJsonTest()
        {
            User testUser = new User();
            testUser.name = "MCD";
            testUser.age = 70;
            testUser.gender = "m";
            testUser.height = 62;
            testUser.weight = 180;
            testUser.userTarget = new DailyTarget();
            testUser.userTarget.logMeal("pasta",1000,"was bomb");
            testUser.userTarget.sleepTarget = 2;
            testUser.userTarget.calorieTarget = 10000;
            testUser.userTarget.logWorkout("bench press", 30, "got sweaty");
            testUser.userTarget.logSleep(8, "");
            testUser.userTarget.sleep.notes = "couldnt sleep";

            testUser.saveToJsonAsync();

           
            User testUser2 = User.readFromJson();

            //make sure nothing is null
            Assert.NotNull(testUser2.gender);
            Assert.NotNull(testUser2);
            Assert.NotNull(testUser2.age);
            Assert.NotNull(testUser2.name);
            Assert.NotNull(testUser2.height);
            Assert.NotNull(testUser2.weight);
            Assert.NotNull(testUser2.userTarget);

            //make sure the initialized object variables match what was in the json file

            Assert.AreEqual("MCD", testUser2.name);
            Assert.AreEqual(70, testUser2.age);
            Assert.AreEqual("m", testUser2.gender);
            Assert.AreEqual(180.0, testUser2.weight);
            Assert.AreEqual(62, testUser2.height);
            System.Console.WriteLine(testUser2.getDailyTarget().getNotes());

        }


        [Test]
        public void loadDatabaseTest()
        {
            User testUser = User.readFromJson();
            DailyTarget testTarget = testUser.getDailyTarget();
            DailyResults testDailyResults = new DailyResults {
                date = testTarget.date.Date,
                calorieTarget = testTarget.calorieTarget,
                sleepTarget = testTarget.sleepTarget,
                workoutTarget = testTarget.workoutTarget,
                caloriesLogged = testTarget.actualCalories,
                sleepLogged = testTarget.actualSleep,
                workoutLogged = testTarget.actualWorkout,
                notesLogged = " ",
            };
            App.Database.SaveTargetAsync(testDailyResults);
            Assert.NotNull(App.Database.GetDailyTargetsListAsync());
        }


        [Test]
        public void searchDatabaseDateRangeTest()
        {
            DailyResults result1 = new DailyResults {
                date = System.DateTime.Today.Date,
                calorieTarget = 3000,
                sleepTarget = 8,
                workoutTarget = 60,
                caloriesLogged = 1000,
                sleepLogged = 2,
                workoutLogged = 20,
                notesLogged = "had a good day",

            };

            DailyResults result2 = new DailyResults
            {
                date = System.DateTime.Today.AddDays(-1).Date,
                calorieTarget = 3000,
                sleepTarget = 8,
                workoutTarget = 60,
                caloriesLogged = 2000,
                sleepLogged = 7,
                workoutLogged = 40,
                notesLogged = "had a good day",

            };

            DailyResults result3 = new DailyResults
            {
                date = System.DateTime.Today.AddDays(-2).Date,
                calorieTarget = 3000,
                sleepTarget = 8,
                workoutTarget = 60,
                caloriesLogged = 3000,
                sleepLogged = 8,
                workoutLogged = 60,
                notesLogged = "had a good day",

            };

            App.Database.SaveTargetAsync(result1);
            App.Database.SaveTargetAsync(result2);
            App.Database.SaveTargetAsync(result3);

            var result = App.Database.GetDateRange(System.DateTime.Today.AddDays(-2).Date,System.DateTime.Today.Date ).Result;
            System.Console.WriteLine("number of results from search: " + result.Count);

            Assert.NotNull(App.Database.GetDailyTargetsListAsync());
            Assert.NotNull(result);
        }


        [Test]

        public void calcDailyTest()
        {
            DailyTarget testTarget = User.readFromJson().getDailyTarget();

            Performance.Daily daily = new Performance.Daily(0, 0, 0);

            daily.CalcPerformance(testTarget.calorieTarget,testTarget.workoutTarget,
                testTarget.sleepTarget, testTarget.actualCalories,testTarget.actualWorkout
                , testTarget.actualSleep);

            Assert.AreEqual(daily.CalorieDeficit, 9000);
            Assert.AreEqual(daily.WorkoutDeficit, 30);
            Assert.AreEqual(daily.SleepDeficit, -6);
        }


        [Test]
        public void calculateWeeklyTest()
        {
            Performance.Weekly weekly = new Performance.Weekly(0,0,0);
            DailyResults result1 = new DailyResults
            {
                date = System.DateTime.Today.Date,
                calorieTarget = 3000,
                sleepTarget = 8,
                workoutTarget = 60,
                caloriesLogged = 1000,
                sleepLogged = 2,
                workoutLogged = 20,
                notesLogged = "had a good day",

            };

            DailyResults result3 = new DailyResults
            {
                date = System.DateTime.Today.AddDays(-2).Date,
                calorieTarget = 3000,
                sleepTarget = 8,
                workoutTarget = 60,
                caloriesLogged = 3000,
                sleepLogged = 8,
                workoutLogged = 60,
                notesLogged = "had a good day",

            };

            DailyResults result2 = new DailyResults
            {
                date = System.DateTime.Today.AddDays(-1).Date,
                calorieTarget = 3000,
                sleepTarget = 8,
                workoutTarget = 60,
                caloriesLogged = 2000,
                sleepLogged = 7,
                workoutLogged = 40,
                notesLogged = "had a good day",

            };

           App.Database.SaveTargetAsync(result1);
           App.Database.SaveTargetAsync(result3);
           App.Database.SaveTargetAsync(result2);

           weekly.CalculateWeekly();
           Assert.AreEqual(weekly.CalorieDeficit, -2000);
           Assert.AreEqual(weekly.WorkoutDeficit, -40);
           Assert.AreEqual(weekly.SleepDeficit, -6);

        }


    }
}