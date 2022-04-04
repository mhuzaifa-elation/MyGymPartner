using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Essentials;

namespace MyGymPartner.ViewModels
{
    public class NearbyGymViewModel : INotifyPropertyChanged
    {
        #region ClassVariables
        string URL = "";
        public string URLB
        {
            get => URL;
            set
            {
                if (value == URL)
                {
                    return;
                }
                URL = value;
                OnPropertyChanged(nameof(URLB));
            }
        }
        #endregion
        #region Constructor
        //Initializing commands 
        public NearbyGymViewModel()
        {
            ShowNearbyGyms();
        }
        #endregion
        #region Methods 
        
        private async void ShowNearbyGyms()//To Get Device Location and fetch Nearby Gyms
        {
            var result = await Geolocation.GetLocationAsync();
            URLB =string.Format("https://www.google.com/maps/search/gym+nearby/@{0},{1},16z", result.Latitude, result.Longitude);
        }
        #endregion
        #region  INotifyPropertyChanged Methods

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}
