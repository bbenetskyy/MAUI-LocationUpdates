using Android.Content;
using Android.Locations;
using Android.OS;
using LocationUpdates.Models;
using Location = Android.Locations.Location;
using AndroidApp = Android.App.Application;

namespace LocationUpdates.Services;

public partial class LocationService : Java.Lang.Object, ILocationListener
{
    private LocationManager _androidLocationManager;

    protected void AndroidStop()
    {
        OnStatusChanged($"LocationService->Stop");
        _androidLocationManager?.RemoveUpdates(this);
    }

    protected void AndroidInitialize()
    {
        OnStatusChanged($"LocationService->Initialize");
        _androidLocationManager ??= (LocationManager)AndroidApp.Context.GetSystemService(Context.LocationService);

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                OnStatusChanged("Permission for location is not granted, we can't get location updates");
                return;
            }

            if (!_androidLocationManager.IsLocationEnabled)
            {
                OnStatusChanged("Location is not enabled, we can't get location updates");
                return;
            }

            if (!_androidLocationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                OnStatusChanged("GPS Provider is not enabled, we can't get location updates");
                return;
            }

            _androidLocationManager.RequestLocationUpdates(LocationManager.GpsProvider, 800, 1, this);
        });
    }

    public void OnLocationChanged(Location location)
    {
        if (location != null)
        {
            OnLocationChanged(new LocationModel(location.Latitude, location.Longitude, location.Bearing));
        }
    }

    public void OnProviderDisabled(string provider)
    {
        //inform your services that we stop getting updates
        OnStatusChanged($"{provider} has been disabled");
    }

    public void OnProviderEnabled(string provider)
    {
        //inform your services that we start getting updates
        OnStatusChanged($"{provider} now enabled");
    }

    public void OnStatusChanged(string provider, Availability status, Bundle extras)
    {
        //inform your services that provides status has been changed
        OnStatusChanged($"{provider} change his status and now it's {status}");
    }
}