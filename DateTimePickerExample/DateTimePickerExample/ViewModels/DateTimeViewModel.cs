using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace DateTimePickerExample.ViewModels
{
    public class DateTimeViewModel : ReactiveObject
    {

        private DateTime _originDateTime;

        public DateTime OriginDateTime
        {
            get { return _originDateTime; }
            set
            {
                this.RaiseAndSetIfChanged(ref _originDateTime, value);
            }
        }

        private DateTime _selectedDateTime;
        public DateTime SelectedDateTime
        {

            get { return _selectedDateTime; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDateTime, value);
                this.RaisePropertyChanged(nameof(SelectedDate));
            }
        }

        private ObservableCollection<object> _selectedDate;

        public ObservableCollection<object> SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDate, value);
                this.RaisePropertyChanged(nameof(SelectedDateTime));
            }
        }

        public DateTimeViewModel(DateTime dateTime)
        {
            InitializeSelection(dateTime);
            OriginDateTime = dateTime;
        }

        public void InitializeSelection(DateTime dateTime)
        {
            ObservableCollection<object> dateTimeCollection = new ObservableCollection<object>();

            dateTimeCollection.Add(dateTime.Year.ToString());
            dateTimeCollection.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month).Substring(0, 3));
            dateTimeCollection.Add(dateTime.Day < 10 ? $"0{dateTime.Day}" : dateTime.Day.ToString());
            dateTimeCollection.Add(dateTime.Hour < 10 ? $"0{dateTime.Hour}" : dateTime.Hour.ToString());
            dateTimeCollection.Add(dateTime.Minute < 10 ? $"0{dateTime.Minute}" : dateTime.Minute.ToString());

            SelectedDateTime = dateTime;
            SelectedDate = dateTimeCollection;
        }
    }
}
