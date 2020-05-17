using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Fit.Natic
{
    public partial class StaticProfPage : ContentPage
    {
       
        public StaticProfPage()
        {
            this.BackgroundColor = Color.LightGray;
            InitializeComponent();
            BindingContext = App.appUser;
            WorkoutGoal.Text = App.todaysTarget.workoutTarget.ToString();
            calorieSlider.Value = App.todaysTarget.calorieTarget;
            sleepSlider.Value = App.todaysTarget.sleepTarget;
        }

        void ContentPage_Appearing(System.Object sender, System.EventArgs e)
        {
            resetBinding();
        }

        void ContentPage_Disappearing(System.Object sender, System.EventArgs e)
        {

        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ProfPage());
        }

        void resetBinding()
        {
            DailyTarget target = App.todaysTarget;
            this.BindingContext = null;
            this.BindingContext = App.appUser;
            WorkoutGoal.Text = target.workoutTarget.ToString();
            calorieSlider.Value = target.calorieTarget;
            sleepSlider.Value = target.sleepTarget;

        }

    }
}
