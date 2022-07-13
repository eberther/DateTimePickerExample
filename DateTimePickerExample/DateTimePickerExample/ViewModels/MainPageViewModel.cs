using DateTimePickerExample.Extensions;
using ReactiveUI;
using System;
using System.Globalization;
using System.Reactive;

namespace DateTimePickerExample.ViewModels
{
    public class MainPageViewModel : ReactiveObject
    {
        public ReactiveCommand<Unit, Unit> ChangeStartDateTimeCommand { get; }
        public ReactiveCommand<Unit, Unit> ChangeEndDateTimeCommand { get; }
        public bool IsChangeStartDateTimeOpen
        {
            get => _isChangeStartDateTimeOpen;
            set => this.RaiseAndSetIfChanged(ref _isChangeStartDateTimeOpen, value);
        }
        public bool IsChangeEndDateTimeOpen
        {
            get => _isChangeEndDateTimeOpen;
            set => this.RaiseAndSetIfChanged(ref _isChangeEndDateTimeOpen, value);
        }
        public DateTimeViewModel StartDateTimeViewModel
        {
            get => _startDateTimeViewModel;
            set => this.RaiseAndSetIfChanged(ref _startDateTimeViewModel, value);
        }

        public DateTimeViewModel EndDateTimeViewModel
        {
            get => _endDateTimeViewModel;
            set => this.RaiseAndSetIfChanged(ref _endDateTimeViewModel, value);
        }
        public string StartDate
        {
            get => _startDate.ToString("g", CultureInfo.CurrentUICulture);
        }

        public string EndDate
        {
            get => _endDate.ToString("g", CultureInfo.CurrentUICulture);
        }


        private bool _isChangeStartDateTimeOpen = false;
        private bool _isChangeEndDateTimeOpen = false;
        private DateTimeViewModel _startDateTimeViewModel;
        private DateTimeViewModel _endDateTimeViewModel;
        private DateTime _startDate;
        private DateTime _endDate;

        public MainPageViewModel()
        {
            _startDate = new DateTime(2022, 1, 10, 10, 5, 0);
            _endDate = new DateTime(2022, 1, 15, 17, 41, 0);
            StartDateTimeViewModel = new DateTimeViewModel(_startDate);
            EndDateTimeViewModel = new DateTimeViewModel(_endDate);
            ChangeStartDateTimeCommand = ReactiveCommand.Create(OnChangeStartDateTime);
            ChangeEndDateTimeCommand = ReactiveCommand.Create(OnChangeEndDateTime);
        }
        private void OnChangeStartDateTime()
        {
            StartDateTimeViewModel.InitializeSelection(_endDate);
            IsChangeStartDateTimeOpen = true;
        }
        public void UpdateSelectedStartDate()
        {
            StartDateTimeViewModel.SelectedDateTime = StartDateTimeViewModel.SelectedDate.ToDateTime();
            _startDate = StartDateTimeViewModel.SelectedDateTime;
            this.RaisePropertyChanged(nameof(StartDate));
            //this.RaisePropertyChanged(nameof(StartDateTimeViewModel));
        }
        private void OnChangeEndDateTime()
        {
            EndDateTimeViewModel.InitializeSelection(_endDate);
            IsChangeEndDateTimeOpen = true;
        }
        public void UpdateSelectedEndDate()
        {
            EndDateTimeViewModel.SelectedDateTime = EndDateTimeViewModel.SelectedDate.ToDateTime();
            _endDate = EndDateTimeViewModel.SelectedDateTime;
            this.RaisePropertyChanged(nameof(EndDate));
            //this.RaisePropertyChanged(nameof(EndDateTimeViewModel));
        }
    }
}
