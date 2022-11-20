using LocationUpdates.Models;

namespace LocationUpdates.Services;

public partial class LocationService
{
    public event EventHandler<LocationModel> LocationChanged;
    public event EventHandler<string> StatusChanged;
    
    public void Initialize()
    {
#if ANDROID
        AndroidInitialize();
#elif IOS
        IosInitialize();
#endif
    }

    public void Stop()
    {
#if ANDROID
        AndroidStop();
#elif IOS
        IosStop();
#endif
    }

    protected virtual void OnLocationChanged(LocationModel e)
    {
        LocationChanged?.Invoke(this, e);
    }

    protected virtual void OnStatusChanged(string e)
    {
        StatusChanged?.Invoke(this, e);
    }
}