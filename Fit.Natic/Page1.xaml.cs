using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fit.Natic
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
          
            InitializeComponent();
            BindingContext = App.appUser;
            this.BackgroundColor = Color.White;
        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            CalorieValueLabel.Text = String.Format("{0}", value);
        }

        void OnSleepValueChanged(object sender, ValueChangedEventArgs args)
        {
            double value = args.NewValue;
            SleepValueLabel.Text = String.Format("{0}", value);
        }

        void PageAppearing(System.Object sender, System.EventArgs e)
        {

        }
        void ContentPage_Disappearing(System.Object sender, System.EventArgs e)
        {

        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (NameEntry.Text != null && GenderEntry.Text != null && HeightEntry.Text != null &&
                WeightEntry.Text != null && WorkoutTarget.Text != null)
            {
                App.appUser.name = NameEntry.Text;
                App.appUser.gender = GenderEntry.Text;
                App.appUser.height = Convert.ToInt32(HeightEntry.Text);
                App.appUser.weight = Convert.ToInt32(WeightEntry.Text);
                App.todaysTarget.calorieTarget = Convert.ToInt32(CalorieSlider.Value);
                App.todaysTarget.workoutTarget = Convert.ToInt32(WorkoutTarget.Text);
                App.todaysTarget.sleepTarget = Convert.ToSingle(SleepSlider.Value);
                App.appUser.setDailyTarget(App.todaysTarget);

                Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Alert", "You must enter values", "OK");
            }
        }

    }
}