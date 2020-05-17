using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;
namespace Fit.Natic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityPage : ContentPage
    {
        Performance.Daily daily_stats;
        Performance.Weekly weekly_stats;
        Performance.Monthly monthly_stats;

        public ActivityPage()
        {
            if (App.firstTimeLaunched == true)
            {
                Navigation.PushAsync(new Page1());
                DisplayAlert("Hey!", "Welcome to Fit.Natic, your own personal fitness calculator. \nFill out your profile and get logging! " +
                    "\nOur Activity page will show you how you're doing.", "Hi!");
            }

            daily_stats = new Performance.Daily(0, 0, 0);
            weekly_stats = new Performance.Weekly(0, 0, 0);
            monthly_stats = new Performance.Monthly(0, 0, 0);
            InitializeComponent();
            loadCharts();
        }

        public async Task loadCharts()
        {
            daily_stats.CalcPerformance();
            await weekly_stats.CalculateWeekly();
            await monthly_stats.CalculateMonthly();

            var dailyEntries = new[]
            {
                new Entry(daily_stats.SleepDeficit)
                {
                    Label = "Sleep (hrs)",
                    ValueLabel = App.todaysTarget.actualSleep.ToString(),
                    Color = SKColor.Parse("#FFAB2D")

                },
                

                new Entry(daily_stats.CalorieDeficit)
                {
                    Label = "Calories",
                    ValueLabel = App.todaysTarget.actualCalories.ToString(),
                    Color = SKColor.Parse("#93DBFF")
                },

                new Entry(daily_stats.WorkoutDeficit)
                {
                    Label = "Workout (min)",
                    ValueLabel =  App.todaysTarget.actualWorkout.ToString(),
                    Color = SKColor.Parse("#D97D54")
                }
            };

            var weeklyEntries = new[]
            {
                new Entry(weekly_stats.SleepDeficit)
                {

                  Color = SKColor.Parse("#FFAB2D")
                },


                new Entry(weekly_stats.CalorieDeficit)
                {
                    Color = SKColor.Parse("#93DBFF")
                },

                new Entry(weekly_stats.WorkoutDeficit)
                {
                    Color = SKColor.Parse("#D97D54")
                }
            };

            var monthlyEntries = new[]
            {
                new Entry(monthly_stats.SleepDeficit)
                {

                    Color = SKColor.Parse("#FFAB2D")
                },


                new Entry(monthly_stats.CalorieDeficit)
                {
                    Color = SKColor.Parse("#93DBFF")
                },

                new Entry(monthly_stats.WorkoutDeficit)
                {
                    Color = SKColor.Parse("#D97D54")
                }
            };

            TodayChart.Chart = new RadialGaugeChart { Entries = dailyEntries, LabelTextSize = 40, MaxValue = 100};
            WeekChart.Chart = new RadialGaugeChart { Entries = weeklyEntries, MaxValue = 100 };
            MonthChart.Chart = new RadialGaugeChart { Entries = monthlyEntries, MaxValue = 100 };

        }

        public async void LogSleep (System.Object sender, EventArgs e)
        {

            await Navigation.PushAsync(new SleepLogPage());

        }


        public async void LogWorkout(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new WorkoutLogPage());
        }

        public async void LogMeal(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MealLogPage());
        }

        void ContentPage_Appearing(System.Object sender, System.EventArgs e)
        {
            //App.appUser.setDailyTarget(App.todaysTarget);

            //check if its first time launching the app
            if (App.firstTimeLaunched == true || App.statsPageViewed == false )
            {
        //        DisplayAlert("Activity", "This page has buttons where you can log your meals, calories, and sleep. Below there are charts where you can track your progress!", "OK");
                App.statsPageViewed = true;
            }
            //want to reload charts in case anything has changed since leaving / coming back to the page
            Task.Run(async () => { await loadCharts(); });
        }

        void ContentPage_Disappearing(System.Object sender, System.EventArgs e)
        {

        }
    }
}
