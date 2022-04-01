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
        public AddEditExercisePage()
        {
            InitializeComponent();
            BindingContext = new AddEditExerciseViewModel(null);
        }

        public AddEditExercisePage(ExcerciseModel selectedExercise)
        {
            InitializeComponent();
            BindingContext = new AddEditExerciseViewModel(selectedExercise);
        }
    }
}