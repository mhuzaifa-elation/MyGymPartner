using MyGymPartner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGymPartner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NearbyGymPage : ContentPage
    {
       #region Constructor
        public NearbyGymPage()
        {
            InitializeComponent();
            BindingContext = new NearbyGymViewModel(); //Binding with Viewmodel
        }
        #endregion

    }
}