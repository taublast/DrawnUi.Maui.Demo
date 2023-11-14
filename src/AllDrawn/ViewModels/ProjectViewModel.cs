using AppoMobi.Maui.DrawnUi.Demo.Resources.Strings;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class ProjectViewModel : BaseViewModel
{

    public ProjectViewModel(NavigationViewModel navModel)
    {
        Presentation = navModel;
    }

    public NavigationViewModel Presentation { get; }

    #region TAP LOCKS

    public ConcurrentDictionary<string, DateTime> TapLocks = new();

    public bool CheckLocked(string uid)
    {
        if (TapLocks.TryGetValue(uid, out DateTime lockTime))
        {
            // If the lock is about to be removed, treat it as unlocked
            if (DateTime.UtcNow >= lockTime)
            {
                TapLocks.TryRemove(uid, out _);
                return false;
            }
            return true;
        }
        return false;
    }

    public bool CheckLockAndSet([CallerMemberName] string uid = null, int ms = 500)
    {
        if (CheckLocked(uid))
            return true;

        var unlockTime = DateTime.UtcNow.AddMilliseconds(ms);
        TapLocks[uid] = unlockTime;

        _ = Task.Delay(ms).ContinueWith(t =>
        {
            TapLocks.TryRemove(uid, out _);
        });

        return false;
    }

    #endregion

    public ICommand CommandShareUrl
    {
        get
        {
            return new Command(async (context) =>
            {

                if (CheckLockAndSet())
                    return;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Uri = context.ToString(),
                        Title = ResStrings.BtnShare
                    });
                });

            });
        }
    }

    public Command CommandCloseModal => new Command(async () =>
    {
        if (CheckLockAndSet())
            return;

        await App.Shell.PopModalAsync(true);
    });

    public Command CommandTest => new Command(async () =>
    {
        if (CheckLockAndSet())
            return;

        Console.WriteLine(" =TEST= ");
    });

    public Command CommandTestVoid => new Command(async () =>
    {

    });



    public Command CommandOpenItemDetails => new Command(async (context) =>
    {
        if (CheckLockAndSet())
            return;

        var request = context as SimpleItemViewModel;
        var route = $"details";
        if (request != null)
        {
            route += $"?id={request.Id}";
        }
        else
        {
            route += $"?id={context}";
        }

        //await Presentation.Shell.GoToAsync(route, true);
    });


}