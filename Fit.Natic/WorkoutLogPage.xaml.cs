using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Fit.Natic
{
    public partial class WorkoutLogPage : ContentPage
    {
        public WorkoutLogPage()
        {
            InitializeComponent();
        }

        void Save_Button_Pressed(System.Object sender, System.EventArgs e)
        {
            string type = (string) Workout_Type.SelectedItem;
            int length = int.Parse(Workout_Time.Text);
            string notes = Workout_Notes.Text;
            App.todaysTarget.logWorkout(type, length, notes);
            Navigation.PopAsync();

        }
    }
}
