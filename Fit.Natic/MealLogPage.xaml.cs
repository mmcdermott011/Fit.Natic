using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Fit.Natic
{
    public partial class MealLogPage : ContentPage
    {
        public MealLogPage()
        {
            InitializeComponent();
        }

        void Save_Button_Pressed(System.Object sender, System.EventArgs e)
        {
            string name = Meal_Name.Text;
            int cals = int.Parse(Meal_Calories.Text);
            string notes = Meal_Notes.Text;
            App.todaysTarget.logMeal(name, cals, notes);
            Navigation.PopAsync();
        }
    }
}
