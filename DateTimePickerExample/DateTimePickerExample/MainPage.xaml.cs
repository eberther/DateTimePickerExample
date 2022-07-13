using DateTimePickerExample.ViewModels;
using Xamarin.Forms;

namespace DateTimePickerExample
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            BindingContext = _viewModel;
        }

        private void StartDateTimePicker_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            _viewModel.UpdateSelectedStartDate();
        }

        private void EndDateTimePicker_OkButtonClicked(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            _viewModel.UpdateSelectedEndDate();
        }
    }
}
