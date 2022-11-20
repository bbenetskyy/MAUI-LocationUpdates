using System;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LocationUpdates
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly Services.LocationService _locationService;

        [ObservableProperty]
        private bool _locationUpdatesEnabled;

        public MainViewModel()
        {
            Updates = new();
            _locationService = new();
        }

        public ObservableCollection<object> Updates { get; }
        
        [RelayCommand]
        public void ChangeLocationUpdates()
        {
            LocationUpdatesEnabled = !LocationUpdatesEnabled;
            if (LocationUpdatesEnabled)
                StartLocationUpdates();
            else
                StopLocationUpdates();
        }

        public void StartLocationUpdates()
        {
            _locationService.LocationChanged += LocationService_LocationChanged;
            _locationService.StatusChanged += LocationService_StatusChanged;
            _locationService.Initialize();
        }

        public void StopLocationUpdates()
        {
            _locationService.Stop();
            _locationService.LocationChanged -= LocationService_LocationChanged;
            _locationService.StatusChanged -= LocationService_StatusChanged;
        }

        private void LocationService_StatusChanged(object sender, string e)
        {
            Updates.Add(e);
        }
            
        private void LocationService_LocationChanged(object sender, Models.LocationModel e)
        {
            Updates.Add(e);
        }
    }
}

