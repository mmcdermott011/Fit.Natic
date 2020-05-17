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
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)  
        {
            double value = args.NewValue;//Hello
            displayLabel.Text = String.Format("", value);
        }
    }
}