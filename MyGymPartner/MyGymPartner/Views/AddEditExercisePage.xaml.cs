using MyGymPartner.Models;
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
    public partial class AddEditExercisePage : ContentPage
    {
        #region Constructor
        public AddEditExercisePage() //Initializing view model for new Exercise
        {
            InitializeComponent();
            BindingContext = new AddEditExerciseViewModel(null); 
        }
        public AddEditExercisePage(ExerciseModel selectedExercise)//Initializing view model for existing Exercise
        {
            InitializeComponent();
            BindingContext = new AddEditExerciseViewModel(selectedExercise);
        }
        #endregion
    }
}