using DateTimePickerExample.Extensions;
using Syncfusion.SfPicker.XForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DateTimePickerExample.Controls
{
    public class DateTimePicker : SfPicker
    {
        #region Properties

        public static readonly BindableProperty MinimumDateTimeProperty = BindableProperty.Create(nameof(MinimumDateTime), typeof(DateTime), typeof(DateTimePicker), null);
        public DateTime MinimumDateTime
        {
            get { return (DateTime)GetValue(MinimumDateTimeProperty); }
            set { SetValue(MinimumDateTimeProperty, value); }
        }

        public static readonly BindableProperty MaximumDateTimeProperty = BindableProperty.Create(nameof(MaximumDateTime), typeof(DateTime), typeof(DateTimePicker), null);
        public DateTime MaximumDateTime
        {
            get { return (DateTime)GetValue(MaximumDateTimeProperty); }
            set { SetValue(MaximumDateTimeProperty, value); }
        }


        public Dictionary<string, string> Months { get; set; }

        public ObservableCollection<object> Date { get; set; }

        public ObservableCollection<string> Day { get; set; }

        public ObservableCollection<string> Month { get; set; }

        public ObservableCollection<string> Year { get; set; }

        public ObservableCollection<string> Hour { get; set; }

        public ObservableCollection<string> Minute { get; set; }

        public ObservableCollection<string> Headers { get; set; }


        #endregion

        public DateTimePicker()
        {
            Months = new Dictionary<string, string>();
            for (int i = 1; i <= 12; i++)
            {
                Months.Add(CultureInfo.CurrentUICulture.DateTimeFormat.GetAbbreviatedMonthName(i), CultureInfo.CurrentUICulture.DateTimeFormat.GetMonthName(i));
            }
            CreateEmptyCollections();

            Headers = new ObservableCollection<string>();
            Headers.Add("Year");
            Headers.Add("Month");
            Headers.Add("Day");
            Headers.Add("Hour");
            Headers.Add("Minute");

            ColumnHeaderText = Headers;
            ShowFooter = true;
            ShowHeader = true;
            ShowColumnHeader = true;

            PickerWidth = 380;
            PickerHeight = 400;

            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            if (PickerWidth > screenWidth)
            {
                // Especially in case of Zoom on a Iphone, this setting is relevant to make sure the DateTimePicker is fully visible
                PickerWidth = screenWidth - 10;
            }
            OnColumnLoaded += DateTimePicker_OnColumnLoaded;
            Opened += DateTimePicker_Opened;
            Closed += DateTimePicker_Closed;
        }

        private void DateTimePicker_OnColumnLoaded(object sender, ColumnLoadedEventArgs e)
        {
        }

        private void CreateEmptyCollections()
        {
            Date = new ObservableCollection<object>();

            Day = new ObservableCollection<string>();
            Month = new ObservableCollection<string>();
            Year = new ObservableCollection<string>();
            Hour = new ObservableCollection<string>();
            Minute = new ObservableCollection<string>();
        }

        private void DateTimePicker_Closed(object sender, EventArgs e)
        {
            SelectionChanged -= CustomDatePicker_SelectionChanged;
        }

        private void DateTimePicker_Opened(object sender, EventArgs e)
        {
            PopulateDateCollection();
            SelectionChanged += CustomDatePicker_SelectionChanged;
        }

        private void PopulateDateCollection()
        {
            CreateEmptyCollections();
            var selectedDateTime = (SelectedItem as IEnumerable<object>).ToDateTime();
            var selectedYear = selectedDateTime.Year;
            var selectedMonth = selectedDateTime.Month;
            var selectedDay = selectedDateTime.Day;
            var selectedHour = selectedDateTime.Hour;

            Year = PopulateYears();
            Month = PopulateMonths(selectedYear);
            Day = PopulateDays(selectedYear, selectedMonth);
            Hour = PopulateHours(selectedYear, selectedMonth, selectedDay);
            Minute = PopulateMinutes(selectedYear, selectedMonth, selectedDay, selectedHour);

            Date?.Clear();
            Date.Add(Year);
            Date.Add(Month);
            Date.Add(Day);
            Date.Add(Hour);
            Date.Add(Minute);
            ItemsSource = Date;
        }

        private void CustomDatePicker_SelectionChanged(object sender, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (Date.Count == 5)
                {
                    if (e.OldValue != e.NewValue && e.OldValue is IList && (e.OldValue as IList).Count > 0)
                    {
                        var oldValues = e.OldValue as ObservableCollection<object>;
                        var newValues = e.NewValue as ObservableCollection<object>;
                        if (Equals(oldValues[0], newValues[0]) == false)
                        {
                            //Year changed
                            UpdateMonths(Date, e);
                            UpdateDays(Date, e);
                            UpdateHours(Date, e);
                            UpdateMinutes(Date, e);
                        }
                        else if (Equals(oldValues[1], newValues[1]) == false)
                        {
                            //Month changed
                            UpdateDays(Date, e);
                            UpdateHours(Date, e);
                            UpdateMinutes(Date, e);
                        }
                        else if (Equals(oldValues[2], newValues[2]) == false)
                        {
                            //Day changed
                            UpdateHours(Date, e);
                            UpdateMinutes(Date, e);
                        }
                        else if (Equals(oldValues[3], newValues[3]) == false)
                        {
                            //Hour changed
                            UpdateMinutes(Date, e);
                        }
                    }
                }
            });
        }

        private void UpdateMinutes(ObservableCollection<object> date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            int selectedYear = int.Parse((e.NewValue as IList)[0].ToString());
            int selectedMonth = Months.Keys.ToList().IndexOf((e.NewValue as IList)[1].ToString()) + 1;
            int selectedDay = int.Parse((e.NewValue as IList)[2].ToString());
            int selectedHour = int.Parse((e.NewValue as IList)[3].ToString());
            var minutes = PopulateMinutes(selectedYear, selectedMonth, selectedDay, selectedHour);
            var indexOfHours = 4;
            UpdateSelectedItems(date, e, minutes, indexOfHours);
        }

        private void UpdateHours(ObservableCollection<object> date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            int selectedYear = int.Parse((e.NewValue as IList)[0].ToString());
            int selectedMonth = Months.Keys.ToList().IndexOf((e.NewValue as IList)[1].ToString()) + 1;
            int selectedDay = int.Parse((e.NewValue as IList)[2].ToString());
            var days = PopulateHours(selectedYear, selectedMonth, selectedDay);
            var indexOfHours = 3;
            UpdateSelectedItems(date, e, days, indexOfHours);
        }

        private void UpdateDays(ObservableCollection<object> date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            int selectedYear = int.Parse((e.NewValue as IList)[0].ToString());
            int selectedMonth = Months.Keys.ToList().IndexOf((e.NewValue as IList)[1].ToString()) + 1;
            var days = PopulateDays(selectedYear, selectedMonth);
            var indexOfDays = 2;
            UpdateSelectedItems(date, e, days, indexOfDays);
        }

        private void UpdateMonths(ObservableCollection<object> date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e)
        {
            int selectedYear = int.Parse((e.NewValue as IList)[0].ToString());
            var months = PopulateMonths(selectedYear);
            var indexOfMonths = 1;
            UpdateSelectedItems(date, e, months, indexOfMonths);
        }


        private void UpdateSelectedItems(ObservableCollection<object> date, Syncfusion.SfPicker.XForms.SelectionChangedEventArgs e, ObservableCollection<string> updatedValueColumn, int indexOfUpdatedValues)
        {
            ObservableCollection<object> updatedValues = new ObservableCollection<object>();
            foreach (var item in e.NewValue as IList)
            {
                updatedValues.Add(item);
            }
            var currentValueColumn = date[indexOfUpdatedValues] as IList;
            if (updatedValueColumn.Count > 0)
            {
                date[indexOfUpdatedValues] = updatedValueColumn;
            }
            if (currentValueColumn.Contains(updatedValues[indexOfUpdatedValues]))
            {
                SelectedItem = updatedValues;
            }
            else
            {
                updatedValues[indexOfUpdatedValues] = currentValueColumn[currentValueColumn.Count - 1];
                SelectedItem = updatedValues;
            }
        }
        private ObservableCollection<string> PopulateYears()
        {
            ObservableCollection<string> years = new ObservableCollection<string>();
            for (int i = MinimumDateTime.Year; i <= MaximumDateTime.Year; i++)
            {
                years.Add(i.ToString());
            }
            return years;
        }

        private ObservableCollection<string> PopulateMonths(int selectedYear)
        {
            ObservableCollection<string> months = new ObservableCollection<string>();

            if (MinimumDateTime.Year == MaximumDateTime.Year)
            {
                PopulateMonthsFromStartAndEnd(months, MinimumDateTime.Month, MaximumDateTime.Month);
            }
            else
            {
                if (selectedYear == MaximumDateTime.Year)
                {
                    PopulateMonthsFromStartAndEnd(months, 1, MaximumDateTime.Month);
                }
                else if (selectedYear == MinimumDateTime.Year)
                {
                    PopulateMonthsFromStartAndEnd(months, MinimumDateTime.Month, 12);
                }
                else
                {
                    PopulateMonthsFromStartAndEnd(months, 1, 12);
                }
            }
            return months;
        }

        private void PopulateMonthsFromStartAndEnd(ObservableCollection<string> months, int startMonth, int endMonth)
        {
            for (int i = startMonth; i <= endMonth; i++)
            {
                months.Add(CultureInfo.CurrentUICulture.DateTimeFormat.GetAbbreviatedMonthName(i));
            }
        }

        private ObservableCollection<string> PopulateDays(int selectedYear, int selectedMonth)
        {
            ObservableCollection<string> days = new ObservableCollection<string>();

            if (selectedYear == MaximumDateTime.Year && selectedMonth == MaximumDateTime.Month)
            {
                if (MinimumDateTime.Year == MaximumDateTime.Year && MinimumDateTime.Month == MaximumDateTime.Month)
                {
                    PopulateDaysFromStartAndEnd(days, MinimumDateTime.Day, MaximumDateTime.Day);
                }
                else
                {
                    PopulateDaysFromStartAndEnd(days, 1, MaximumDateTime.Day);
                }
            }
            else if (selectedYear == MinimumDateTime.Year && selectedMonth == MinimumDateTime.Month)
            {
                PopulateDaysFromStartAndEnd(days, MinimumDateTime.Day, DateTime.DaysInMonth(selectedYear, selectedMonth));
            }
            else
            {
                PopulateDaysFromStartAndEnd(days, 1, DateTime.DaysInMonth(selectedYear, selectedMonth));
            }
            return days;
        }

        private void PopulateDaysFromStartAndEnd(ObservableCollection<string> days, int startDay, int endDay)
        {
            for (int i = startDay; i <= endDay; i++)
            {
                if (i < 10)
                {
                    days.Add("0" + i);
                }
                else
                {
                    days.Add(i.ToString());
                }
            }
        }

        private ObservableCollection<string> PopulateHours(int selectedYear, int selectedMonth, int selectedDay)
        {
            ObservableCollection<string> hours = new ObservableCollection<string>();

            if (selectedYear == MaximumDateTime.Year && selectedMonth == MaximumDateTime.Month && MinimumDateTime.Day == MaximumDateTime.Day)
            {
                PopulateHoursFromStartAndEnd(hours, MinimumDateTime.Hour, MaximumDateTime.Hour);
            }
            else if (selectedYear == MaximumDateTime.Year && selectedMonth == MaximumDateTime.Month && selectedDay == MaximumDateTime.Day)
            {
                PopulateHoursFromStartAndEnd(hours, 0, MaximumDateTime.Hour);

            }
            else if (selectedYear == MinimumDateTime.Year && selectedMonth == MinimumDateTime.Month && selectedDay == MinimumDateTime.Day)
            {
                PopulateHoursFromStartAndEnd(hours, MinimumDateTime.Hour, 23);
            }
            else
            {
                PopulateHoursFromStartAndEnd(hours, 0, 23);
            }

            return hours;
        }

        private void PopulateHoursFromStartAndEnd(ObservableCollection<string> hours, int startHour, int endHour)
        {
            for (int i = startHour; i <= endHour; i++)
            {
                if (i < 10)
                {
                    hours.Add("0" + i.ToString());
                }
                else
                {
                    hours.Add(i.ToString());
                }
            }
        }

        private ObservableCollection<string> PopulateMinutes(int selectedYear, int selectedMonth, int selectedDay, int selectedHour)
        {
            ObservableCollection<string> minutes = new ObservableCollection<string>();

            if (selectedYear == MaximumDateTime.Year && selectedMonth == MaximumDateTime.Month && selectedDay == MaximumDateTime.Day && MinimumDateTime.Hour == MaximumDateTime.Hour)
            {
                PopulateMinutesFromStartAndEnd(minutes, MinimumDateTime.Minute, MaximumDateTime.Minute);
            }
            else if (selectedYear == MaximumDateTime.Year && selectedMonth == MaximumDateTime.Month && selectedDay == MaximumDateTime.Day && selectedHour == MaximumDateTime.Hour)
            {
                PopulateMinutesFromStartAndEnd(minutes, 0, MaximumDateTime.Minute);
            }
            else if (selectedYear == MinimumDateTime.Year && selectedMonth == MinimumDateTime.Month && selectedDay == MinimumDateTime.Day && selectedHour == MinimumDateTime.Hour)
            {
                PopulateMinutesFromStartAndEnd(minutes, MinimumDateTime.Minute, 59);
            }
            else
            {
                PopulateMinutesFromStartAndEnd(minutes, 0, 59);
            }
            return minutes;
        }

        private void PopulateMinutesFromStartAndEnd(ObservableCollection<string> minutes, int startMinute, int endMinute)
        {
            for (int j = startMinute; j <= endMinute; j++)
            {
                if (j < 10)
                {
                    minutes.Add("0" + j);
                }
                else
                {
                    minutes.Add(j.ToString());
                }
            }
        }



    }
}