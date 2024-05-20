using AppoMobi.Maui.DrawnUi.Demo.Resources.Strings;
using AppoMobi.Maui.DrawnUi.Demo.Views.Controls;
using DrawnUi.Maui.Infrastructure;
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



    public ICommand CommandToggleBlur
    {
        get
        {
            return new Command(async (context) =>
            {

                if (CheckLockAndSet())
                    return;

                Presentation.HasBlur = !Presentation.HasBlur;

            });
        }
    }

    public NavigationViewModel Presentation { get; }

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

    static int helperCount;

    public ICommand CommandOpenPopup
    {
        get
        {
            return new Command(async () =>
            {
                if (CheckLockAndSet())
                    return;

                //do not block ui, lets us see the touch effect
                //while we build page to be opened
                await Task.Run(async () =>
                {

                    var content = new SkiaLayout()
                    {
                        UseCache = SkiaCacheType.Operations,
                        BackgroundColor = Colors.Black,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(64),
                        Padding = new Thickness(24),
                        CreateChildren = () => new()
                        {
                            new SkiaLabel()
                            {
                                TextColor = Colors.White,
                                Text = $"{++helperCount} Popup",
                                FontFamily = "FontText",
                                FontSize = 24
                            }
                        }
                    };

                    var popup = await App.Shell.OpenPopupAsync(content);

                }).ConfigureAwait(false);
            });
        }
    }

    public ICommand CommandOpenPopupT
    {
        get
        {
            return new Command(async () =>
            {
                if (CheckLockAndSet())
                    return;

                //do not block ui, lets us see the touch effect
                //while we build page to be opened
                await Task.Run(async () =>
                {

                    var content = new SkiaLayout()
                    {
                        UseCache = SkiaCacheType.Operations,
                        Type = LayoutType.Wrap,
                        Split = 1,
                        BackgroundColor = Colors.Black,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Margin = new Thickness(64),
                        Padding = new Thickness(24),
                        CreateChildren = () => new()
                        {
                            new SkiaLabel()
                            {
                                TextColor = Colors.White,
                                Text = $"{++helperCount} Popup",
                                FontFamily = "FontText",
                                FontSize = 24
                            },
                            new SmallButton()
                            {
                                Text = "Modal",
                                CommandTapped = this.CommandOpenModal
                            }
                        }
                    };

                    var popup = await App.Shell.OpenPopupAsync(content, true, true, false);

                }).ConfigureAwait(false);
            });
        }
    }

    public ICommand CommandOpenToast
    {
        get
        {
            return new Command(async () =>
            {
                if (CheckLockAndSet())
                    return;

                //do not block ui, lets us see the touch effect
                //while we build page to be opened
                await Task.Run(async () =>
                {

                    App.Shell.ShowToast("Hello World! This is all drawn using **SkiaSharp** 2.88 library. 👍😍🤩");

                }).ConfigureAwait(false);
            });
        }
    }

    public ICommand CommandOpenModal
    {
        get
        {
            return new Command(async () =>
            {
                if (CheckLockAndSet())
                    return;

                await Task.Run(async () =>
                {
                    //var content = new ScreenBrowser("SkiaMauiElement - WebView", "https://dotnet.microsoft.com/en-us/apps/maui");
                    var content = new ModalContent();
                    await App.Shell.PushModalAsync(content, true, true, true);

                    //var page = new ScreenVarious();
                    //await Presentation.Shell.PushDrawnAsync(page, true);

                }).ConfigureAwait(false);
            });
        }
    }

    public ICommand CommandOpenModalT
    {
        get
        {
            return new Command(async () =>
            {
                if (CheckLockAndSet())
                    return;

                await Task.Run(async () =>
                {
                    //var content = new ScreenBrowser("SkiaMauiElement - WebView", "https://dotnet.microsoft.com/en-us/apps/maui");
                    var content = new ModalContent();
                    await App.Shell.PushModalAsync(content, true, true, false);

                    //var page = new ScreenVarious();
                    //await Presentation.Shell.PushDrawnAsync(page, true);

                }).ConfigureAwait(false);
            });
        }
    }

}