using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class BaseViewModel : BindableObject, IDisposable
{

    #region TAP LOCKS

    public Dictionary<string, DateTime> TapLocks = new();

    public bool CheckLocked(string uid)
    {
        if (TapLocks.TryGetValue(uid, out DateTime lockTime))
        {
            // If the lock is about to be removed, treat it as unlocked
            if (DateTime.UtcNow >= lockTime)
            {
                TapLocks.Remove(uid);
                return false;
            }
            return true;
        }
        return false;
    }

    public bool CheckLockAndSet([CallerMemberName] string uid = null, int ms = 900)
    {
        if (CheckLocked(uid))
            return true;

        var unlockTime = DateTime.UtcNow.AddMilliseconds(ms);
        TapLocks[uid] = unlockTime;

        _ = Task.Delay(ms).ContinueWith(t =>
        {
            TapLocks.Remove(uid);
        });

        return false;
    }

    #endregion


    public string UID { get; } = Guid.NewGuid().ToString();

    public DateTime BuildTime
    {
        get
        {
            return GetLinkerTime(this.GetType().Assembly);
        }
    }

    public static DateTime GetLinkerTime(Assembly assembly)
    {
        const string BuildVersionMetadataPrefix = "+build";

        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute?.InformationalVersion != null)
        {
            var value = attribute.InformationalVersion;
            var index = value.IndexOf(BuildVersionMetadataPrefix);
            if (index > 0)
            {
                value = value.Right(value.Length - (index + BuildVersionMetadataPrefix.Length));
                return DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ss:fffZ", CultureInfo.InvariantCulture);
            }
        }

        return default;
    }

    public bool IsDebug
    {
        get
        {
#if DEBUG
            return true;
#endif
            return false;
        }
    }


    public string Build
    {
        get
        {
            var ret = VersionTracking.CurrentBuild;
#if DEBUG
            ret += " DEBUG";
#endif
            return ret;
        }
    }

    public string Version
    {
        get
        {
            var ret = VersionTracking.CurrentVersion;
#if DEBUG
            ret += " DEBUG";
#endif
            return ret;
        }
    }

    private string _Title = "Demo";
    public string Title
    {
        get { return _Title; }
        set
        {
            if (_Title != value)
            {
                _Title = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _IsBusy;
    public bool IsBusy
    {
        get { return _IsBusy; }
        set
        {
            if (_IsBusy != value)
            {
                _IsBusy = value;
                OnPropertyChanged();
            }
        }
    }



    //protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    //{
    //    if (EqualityComparer<T>.Default.Equals(storage, value))
    //    {
    //        return false;
    //    }
    //    storage = value;
    //    OnPropertyChanged(propertyName);

    //    return true;
    //}



    //public NavBarViewModel Presentation => App.Instance.Presentation;

    public virtual void OnSubscribing(bool subscribe)
    {

    }

    void Subscribe(bool subscribe = true)
    {
        OnSubscribing(subscribe);



    }


    bool _disposed;
    public void Dispose()
    {
        if (_disposed)
            return;

        Subscribe(false);

        OnDisposing();

        _disposed = true;
    }

    public virtual void OnDisposing()
    {

    }

    public Command CommandSimple => new Command(async () =>
    {
        Console.WriteLine("DEV COMMAND INVOKED");
    });

}