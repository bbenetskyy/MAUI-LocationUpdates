using System;
using CoreLocation;
using LocationUpdates.Models;

namespace LocationUpdates.Services
{
	public partial class LocationService
	{
        private CLLocationManager _iosLocationManager;

        protected void IosStop()
        {
            OnStatusChanged($"LocationService->Stop");
            if (_iosLocationManager is null)
                return;

            _iosLocationManager.StopUpdatingLocation();
            _iosLocationManager.LocationsUpdated -= LocationsUpdated;
        }

        protected void IosInitialize()
        {
            OnStatusChanged($"LocationService->Initialize");
            _iosLocationManager ??= new CLLocationManager()
            {
                DesiredAccuracy = CLLocation.AccuracyBest,
                DistanceFilter = CLLocationDistance.FilterNone,
                PausesLocationUpdatesAutomatically = false
            };

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    OnStatusChanged("Permission for location is not granted, we can't get location updates");
                    return;
                }

                _iosLocationManager.RequestAlwaysAuthorization();
                _iosLocationManager.LocationsUpdated += LocationsUpdated;
                _iosLocationManager.StartUpdatingLocation();
            });
        }

        private void LocationsUpdated(object sender, CLLocationsUpdatedEventArgs e)
        {
            var locations = e.Locations;
            LocationChanged?.Invoke(this, new LocationModel(
                locations[^1].Coordinate.Latitude,
                locations[^1].Coordinate.Longitude,
                (float)locations[^1].Course));
        }
    }
}

