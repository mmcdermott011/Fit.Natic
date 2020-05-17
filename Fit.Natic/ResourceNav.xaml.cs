using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Fit.Natic
{
    public partial class ResourceNav : ContentPage
    {
        public ResourceNav()
        {
            InitializeComponent();
        }

        void Resources_Appearing(System.Object sender, System.EventArgs e)
        {
            if (App.firstTimeLaunched == true || App.resourcesPageViewed == false)
            {
                 //DisplayAlert("Resources", "Use this page to find recipes, workouts, and calorie calculators", "OK");
                App.resourcesPageViewed = true;
            }
        }

        void Resources_Disappearing(System.Object sender, System.EventArgs e)
        {
        }

        private void Button_OnClicked_Workouts(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://www.muscleandstrength.com/workout-routines");
        }

        private void Button_OnClicked_Meals(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://www.active.com/nutrition/articles/your-7-day-meal-plan-882313");
        }

        private void Button_OnClicked_Calorie(object sender, EventArgs e)
        {
            Launcher.OpenAsync("https://www.calculator.net/calorie-calculator.html");
        }
    }
}
