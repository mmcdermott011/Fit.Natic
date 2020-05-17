using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Fit.Natic
{
    public partial class SleepLogPage : ContentPage
    {
        public SleepLogPage()
        {
            InitializeComponent();
            BindingContext = App.todaysTarget.sleep;
        }

        void Save_Button_Pressed(System.Object sender, System.EventArgs e)
        {
            float length = Convert.ToSingle(sleepLength.Text);
            string notes = sleepnotes.Text;
            App.todaysTarget.logSleep(length,notes);
            Navigation.PopAsync();
        }

        void ContentPage_Appearing(System.Object sender, System.EventArgs e)
        {
            BindingContext = App.todaysTarget.sleep;

        }

        void ContentPage_Disappearing(System.Object sender, System.EventArgs e)
        {
        }
    }
}
