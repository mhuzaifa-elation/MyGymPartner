using MyGymPartner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyGymPartner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritesPage : ContentPage
    {
        #region Constructor
        public FavoritesPage()
        {
            InitializeComponent();
            BindingContext = new FavoritesViewModel(); //Binding with Viewmodel
        }
        #endregion
    }
}