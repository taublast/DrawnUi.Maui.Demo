using AppoMobi.Specials;
using DrawnUi.Maui.Controls;

using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class NavigationViewModel : BaseViewModel
{
    /// <summary>
    /// Use this for covering everything in a modal way, precisely tabs
    /// </summary>
    public SkiaLayout RootLayout { get; set; }


    #region MODAL

    public SkiaDrawer Modal { get; protected set; }

    public void CloseModal()
    {
        Modal?.Close();
        Modal = null;
    }

    public async Task PushModal(SkiaLayout content, bool useGestures)
    {
        if (Modal != null)
        {
            Modal.Close();
        }

        var drawer = new SkiaDrawer()
        {
            Tag = "Modal",
            Bounces = true,
            RespondsToGestures = useGestures,
            ZIndex = 9999,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Fill,
            BackgroundColor = Colors.Gainsboro,
            Direction = DrawerDirection.FromBottom,
            HeaderSize = 0,
        };

        drawer.Content = content;

        void LayoutReadyHandler(object sender, EventArgs e)
        {
            if (sender is SkiaDrawer control)
            {
                control.OnViewportReady -= LayoutReadyHandler;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    control.IsOpen = true;
                });
            }
        }

        void DrawerScrolled(object sender, bool inTransition)
        {
            if (!inTransition && sender is SkiaDrawer { IsOpen: false } control)
            {
                control.OnTransitionChanged -= DrawerScrolled;
                control.Dispose(); //will be removed from parent
            }
        }

        drawer.OnViewportReady += LayoutReadyHandler;
        drawer.OnTransitionChanged += DrawerScrolled;

        drawer.SetParent(RootLayout);

        Modal = drawer;
    }

    #endregion

    private bool _HasBlur = true;
    public bool HasBlur
    {
        get
        {
            return _HasBlur;
        }
        set
        {
            if (_HasBlur != value)
            {
                _HasBlur = value;
                OnPropertyChanged();
            }
        }
    }

    public NavigationViewModel()
    {
        UpdateControls();
    }

    public NavigationViewModel(IServiceProvider services)
    {
        _services = services;
        SubscribeToEvents();
        //RefreshUserInfo();
    }

    public void Initialize(string startupRoute)
    {
        this.Shell = new AppShell();

        App.Current.MainPage = this.Shell;

        SubscribeToEvents();

        CommandGoBack = new Command(async () =>
        {
            await Shell.PopAsync();
        });

        Shell.Initialize(startupRoute);
    }

    private void NeedUpdateInsets(object sender, EventArgs e)
    {
        UpdateControls();
    }

    protected void UnsubscribeFromEvents()
    {
        Super.InsetsChanged -= NeedUpdateInsets;

        if (Shell != null)
        {
            Shell.Navigated -= OnShellNavigated;
            Shell.OnRotation -= OnRotation;
            Shell.TabReselected -= OnTabReselected;
        }

        //App.Instance.Notifications.OnReceived -= OnNotificationsReceived;
    }

    void SubscribeToEvents()
    {
        Super.InsetsChanged += NeedUpdateInsets;

        //App.Instance.Notifications.OnReceived += OnNotificationsReceived;

        if (Shell != null)
        {
            Shell.Navigated += OnShellNavigated;
            Shell.OnRotation += OnRotation;
            Shell.TabReselected += OnTabReselected;
        }

        UpdateControls();

        //give time to update ui
        Tasks.StartTimerAsync(TimeSpan.FromMilliseconds(10), async () =>
        {
            StartupFinished = true;
            return false;
        });

    }

    private void OnShellNavigated(object sender, ShellNavigatedEventArgs e)
    {

        if (e.Current.Location.ToString().SafeContains("/tabs"))
        {
            InitFlyout();

            //todo can do one-time tasks if needed
            //Super.StartTimer(TimeSpan.FromSeconds(1.5), () =>
            //{
            //    InsureSignalsConnected();
            //    return false;
            //});
        }
    }

    /// <summary>
    /// We have no menu atm
    /// </summary>
    public void InitFlyout()
    {
        //todo if needed
    }
    public AppShell Shell { get; set; }

    #region Track Loaded Data Invalidation


    #region Request

    public void InvalidateRequestsData()
    {
        foreach (var key in TrackRequestsData.Keys.ToArray())
        {
            SetRequestsDataState(key, false);
        }
    }

    public void SetRequestsDataState(string key, bool state)
    {
        TrackRequestsData.AddOrUpdate(
            key,
            state, (k, b) => state);
    }

    public bool GetRequestsDataState(string key)
    {
        var ok = TrackRequestsData.TryGetValue(key, out bool data);
        if (ok)
            return data;
        return false;
    }

    public ConcurrentDictionary<string, bool> TrackRequestsData = new();

    #endregion


    #region Building

    public void InvalidateBuildingData()
    {
        foreach (var key in TrackBuildingData.Keys.ToArray())
        {
            SetBuildingDataState(key, false);
        }
    }

    public void SetBuildingDataState(string key, bool state)
    {
        TrackBuildingData.AddOrUpdate(
            key,
            state, (k, b) => state);
    }

    public bool GetBuildingDataState(string key)
    {
        var ok = TrackBuildingData.TryGetValue(key, out bool data);
        if (ok)
            return data;
        return false;
    }

    public ConcurrentDictionary<string, bool> TrackBuildingData = new();

    #endregion



    #region RequestedDocument

    public void InvalidateRequestedDocumentData()
    {
        foreach (var key in TrackRequestedDocumentData.Keys.ToArray())
        {
            SetRequestedDocumentDataState(key, false);
        }
    }

    public void SetRequestedDocumentDataState(string key, bool state)
    {
        TrackRequestedDocumentData.AddOrUpdate(
            key,
            state, (k, b) => state);
    }

    public bool GetRequestedDocumentDataState(string key)
    {
        var ok = TrackRequestedDocumentData.TryGetValue(key, out bool data);
        if (ok)
            return data;
        return false;
    }

    public ConcurrentDictionary<string, bool> TrackRequestedDocumentData = new();

    #endregion


    #endregion

    #region CACHE to pass models


    #endregion

    public override void OnDisposing()
    {
        UnsubscribeFromEvents();
    }


    private double _Keyboard;
    public double Keyboard
    {
        get { return _Keyboard; }
        set
        {
            if (_Keyboard != value)
            {
                if (value < 0)
                    value = 0.0;
                _Keyboard = value;
                OnPropertyChanged();
                OnPropertyChanged("BottomSafeInsetsOrKeyboardWIthTabs");
                OnPropertyChanged("BottomSafeInsetsOrKeyboard");
                OnPropertyChanged("PaddingBottomTabsAndSafeInsets");
                OnPropertyChanged("KeyboardOffset");
            }
        }
    }

    public double BottomSafeInsetsOrKeyboardWIthTabs
    {
        get
        {
            var value = BottomTabsUnderPadding;// for translucent ios tabbar
                                               //if (Device.RuntimePlatform == Device.Android)
                                               //var value = 0;

            if (Keyboard > 0)
                return value + KeyboardOffset + Super.Screen.BottomInset;

            if (IsFullscreen) return 0;

            return value + PaddingBottom;
        }
    }


    public double BottomSafeInsetsOrKeyboard
    {
        get
        {
            if (Keyboard > 0)
                return KeyboardOffset;

            if (IsFullscreen) return 0;

            return PaddingBottom;
        }
    }



    private Queue<string> queueHideDicoBtn = new Queue<string>();

    public void HideDicoBtn()
    {
        var myId = Guid.NewGuid().ToString();
        queueHideDicoBtn.Enqueue(myId);
        OnPropertyChanged("DicoBtnVisible");
    }

    public void UnhideDicoBtn()
    {
        try
        {
            queueHideDicoBtn.Dequeue();
        }
        catch (Exception e)
        {
        }
        OnPropertyChanged("DicoBtnVisible");
    }

    public bool DicoBtnVisible
    {
        get
        {
            //todo
            return queueHideDicoBtn.Count == 0;
        }
    }

    private double _DicoBtnMoveY;
    public double DicoBtnMoveY
    {
        get { return _DicoBtnMoveY; }
        set
        {
            if (_DicoBtnMoveY != value)
            {
                _DicoBtnMoveY = value;
                OnPropertyChanged();
                OnPropertyChanged("DicoBtnOffsetY");
            }
        }
    }





    bool StartupFinished { get; set; }

    public void PopToRoot()
    {
        Shell.NavigationLayout.PopTabToRoot();
    }

    private void OnTabReselected(object sender, IndexArgs e)
    {
        //this can be called at startup (xamarin bug) so need to lock it for some time..
        PopToRoot();
    }



    //-------------------------------------------------------------
    // FocusedElement
    //-------------------------------------------------------------
    private const string nameFocusedElement = "FocusedElement";

    public static readonly BindableProperty FocusedElementProperty = BindableProperty.Create(nameFocusedElement,
        typeof(VisualElement), typeof(NavigationViewModel), null, BindingMode.TwoWay);
    public VisualElement FocusedElement
    {
        get { return (VisualElement)GetValue(FocusedElementProperty); }
        set { SetValue(FocusedElementProperty, value); }
    }



    #region NOTIFICATIONS


    public ICommand CommandNotifications
    {
        get
        {
            return new Command(async (object context) =>
            {
                throw new NotImplementedException();
                //var page = new PageListNotifications();
                //App.OpenPage(page);
            });
        }
    }


    private int _NotificationsProfile;
    public int NotificationsProfile
    {
        get { return _NotificationsProfile; }
        set
        {
            if (_NotificationsProfile != value)
            {
                _NotificationsProfile = value;
                OnPropertyChanged();
            }
        }
    }



    private int _NotificationsChat;
    public int NotificationsChat
    {
        get { return _NotificationsChat; }
        set
        {
            if (_NotificationsChat != value)
            {
                _NotificationsChat = value;
                OnPropertyChanged();
            }
        }
    }



    private int _NotificationsSystem;
    public int NotificationsSystem
    {
        get { return _NotificationsSystem; }
        set
        {
            if (_NotificationsSystem != value)
            {
                _NotificationsSystem = value;
                OnPropertyChanged();
            }
        }
    }




    #endregion


    //-------------------------------------------------------------
    // GoBackCheckDenied
    //-------------------------------------------------------------
    private const string nameGoBackCheckDenied = "GoBackCheckDenied";
    public static readonly BindableProperty GoBackCheckDeniedProperty = BindableProperty.Create(nameGoBackCheckDenied, typeof(bool), typeof(NavigationViewModel), false); //, BindingMode.TwoWay
    public bool GoBackCheckDenied
    {
        get { return (bool)GetValue(GoBackCheckDeniedProperty); }
        set { SetValue(GoBackCheckDeniedProperty, value); }
    }

    //-------------------------------------------------------------
    // CommandGoBack
    //-------------------------------------------------------------
    private const string nameCommandGoBack = "CommandGoBack";
    public static readonly BindableProperty CommandGoBackProperty = BindableProperty.Create(nameCommandGoBack, typeof(ICommand), typeof(NavigationViewModel), null); //, BindingMode.TwoWay
    public ICommand CommandGoBack
    {
        get { return (ICommand)GetValue(CommandGoBackProperty); }
        set { SetValue(CommandGoBackProperty, value); }
    }



    public void ShowMenu(bool open = true)
    {

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            // Update the UI
            var mainPage = App.Current.MainPage;
            if (mainPage is FlyoutPage)
            {
                ((FlyoutPage)mainPage).IsPresented = open;
            }
            else
            {
                throw new Exception("NavigationRoot is not MasterDetail");
            }
        });

    }

    public async Task GoBack(bool animate = true)
    {

        //App.Instance.Shell.GoToAsync("..");
        //return;

        if (Shell != null)
        {
            if (Shell.ModalStack.Count > 0)
            {
                await Shell.PopModalAsync(animate);
            }
            else
            {
                await Shell.PopAsync(animate);
            }
        }

    }

    public ICommand CommandNavbarMenu
    {
        get
        {
            return new Command(async (object context) =>
            {
                if (NoMenu)
                    return;

                ShowMenu();
            });
        }
    }



    public ICommand CommandSmartGoBack
    {
        get
        {
            return new Command(async (object context) =>
            {
                await GoBack();
            });
        }
    }

    public ICommand CommandNavbarGoBack
    {
        get
        {
            return new Command(async (object context) =>
            {
                if (GoBackCheckDenied)
                    return;

                await GoBack();

                //else
                //{
                //    if (!NoMenu)
                //        App.ShowMenu();
                //}
            });
        }
    }

    public ICommand CommandTabGoBack
    {
        get
        {
            return new Command(async (object context) =>
            {
                if (GoBackCheckDenied)
                    return;
                throw new NotImplementedException();
                //App.ViewSwitcher.PopTab();
                //   App.ViewSwitcher.PopTab();
            });
        }
    }

    public ICommand CommandNavbarFilter
    {
        get
        {
            return new Command(async (object context) =>
            {
                if (NoMenu)
                    return;


            });
        }
    }

    #region SEARCH

    public static readonly BindableProperty CommandOnSearchProperty = BindableProperty.Create(
        nameof(CommandOnSearch),
        typeof(ICommand),
        typeof(NavigationViewModel),
        defaultValue: null);

    public ICommand CommandOnSearch
    {
        get { return (ICommand)GetValue(CommandOnSearchProperty); }
        set { SetValue(CommandOnSearchProperty, value); }
    }



    public ICommand CommandSearch
    {
        get
        {
            return new Command(async (object context) =>
            {


            });
        }
    }



    #endregion

    #region LAYOUT

    private DeviceRotation _orientationRotation;
    public DeviceRotation Orientation
    {
        get { return _orientationRotation; }
        set
        {
            if (_orientationRotation != value)
            {
                _orientationRotation = value;
                OnPropertyChanged();
            }
        }
    }

    private void OnRotation(object sender, RotationEventArgs e)
    {
        Orientation = e.Orientation;

        UpdateControls();
    }


    public double NavBarPadding
    {
        get
        {
            return StatusBarHeightRequest + NavBarHeightRequest;
        }
    }



    public void UpdateControls()
    {

        //if (Display.IsFrameless)
        //{
        //    StatusOpacity = 0;
        //}
        //else
        //{
        //    StatusOpacity = 1;
        //}

        StatusBarHeightRequest = CalculateStatusBar(Orientation);
        NavBarHeightRequest = CalculateNavBar(Orientation);
        ClippedNavBarHeightRequest = CalculateClippedNavBar(Orientation);
        PaddingHeightRequest = NavBarHeightRequest + StatusBarHeightRequest;
        PaddingClippedHeightRequest = ClippedNavBarHeightRequest + StatusBarHeightRequest;

        BottomTabsHeightRequest = Super.BottomTabsHeight;

        BottomTabsUnderPadding = BottomTabsHeightRequest + PaddingBottom - 2; //for shadow

        NavAndTabsMargin = new Thickness(0, PaddingHeightRequest, 0, 0);

    }

    public double BottomTabsWithInsetsHeightRequest
    {
        get
        {
            return BottomTabsHeightRequest + PaddingBottom;
        }
    }

    private double _StatusOpacity;
    public double StatusOpacity
    {
        get { return _StatusOpacity; }
        set
        {
            if (_StatusOpacity != value)
            {
                _StatusOpacity = value;
                OnPropertyChanged();
            }
        }
    }


    private bool _IsFullscreen;
    public bool IsFullscreen
    {
        get { return _IsFullscreen; }
        set
        {
            if (_IsFullscreen != value)
            {
                _IsFullscreen = value;
                OnPropertyChanged();
                RaiseProps();
            }
        }
    }

    public void RaiseProps()
    {
        OnPropertyChanged("BottomTabsHeightRequest");
        OnPropertyChanged("BottomTabsUnderPadding");
        OnPropertyChanged("PaddingBottom");
        OnPropertyChanged("PaddingBottomTabsAndSafeInsets");
        OnPropertyChanged("BottomSafeInsetsOrKeyboard");

    }

    private double _BottomTabsHeightRequest;
    public double BottomTabsHeightRequest
    {
        get
        {
            if (IsFullscreen) return 0;

            return _BottomTabsHeightRequest;
        }
        set
        {
            if (_BottomTabsHeightRequest != value)
            {
                _BottomTabsHeightRequest = value;
                OnPropertyChanged();
            }
        }
    }

    private double _BottomTabsUnderPadding;
    public double BottomTabsUnderPadding
    {
        get
        {
            if (IsFullscreen) return 0;
            return _BottomTabsUnderPadding;
        }
        set
        {
            if (_BottomTabsUnderPadding != value)
            {
                _BottomTabsUnderPadding = value;
                OnPropertyChanged();
                OnPropertyChanged("PaddingBottomTabsAndSafeInsets");
                OnPropertyChanged("KeyboardOffset");
            }
        }
    }

    private double _PaddingBottom;
    public double PaddingBottom
    {
        get
        {
            if (IsFullscreen) return 0;
            return _PaddingBottom;
        }
        set
        {
            if (_PaddingBottom != value)
            {
                _PaddingBottom = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PaddingBottomTabsAndSafeInsets));
                OnPropertyChanged(nameof(BottomSafeInsetsOrKeyboard));
                OnPropertyChanged(nameof(KeyboardOffset));
                OnPropertyChanged(nameof(BottomTabsWithInsetsHeightRequest));
            }
        }
    }

    public double KeyboardOffset
    {
        get
        {
            var ret = 0.0;

            if (Keyboard > 0)
            {
                ret = Keyboard;
                //    if (Device.RuntimePlatform == Device.iOS)
                ret -= (PaddingBottom + BottomTabsHeightRequest);
            }

            return ret;
        }
    }

    public double PaddingBottomTabsAndSafeInsets
    {
        get
        {
            if (Keyboard > 0)
                return KeyboardOffset + BottomTabsUnderPadding; ;

            if (IsFullscreen)
                return 0;

            return PaddingBottom + BottomTabsUnderPadding;
        }
    }


    private double _PaddingHeightRequest;
    public double PaddingHeightRequest
    {
        get { return _PaddingHeightRequest; }
        set
        {
            if (_PaddingHeightRequest != value)
            {
                _PaddingHeightRequest = value;
                OnPropertyChanged();
            }
        }
    }

    private double _PaddingClippedHeightRequest;
    public double PaddingClippedHeightRequest
    {
        get { return _PaddingClippedHeightRequest; }
        set
        {
            if (_PaddingClippedHeightRequest != value)
            {
                _PaddingClippedHeightRequest = value;
                OnPropertyChanged();
            }
        }
    }

    private double _StatusBarHeightRequest;
    public double StatusBarHeightRequest
    {
        get { return _StatusBarHeightRequest; }
        set
        {
            if (_StatusBarHeightRequest != value)
            {
                _StatusBarHeightRequest = value;
                OnPropertyChanged();
                OnPropertyChanged("NavBarPadding");
            }
        }
    }





    private double _NavBarHeightRequest;
    public double NavBarHeightRequest
    {
        get { return _NavBarHeightRequest; }
        set
        {
            if (_NavBarHeightRequest != value)
            {
                _NavBarHeightRequest = value;
                OnPropertyChanged();
                OnPropertyChanged("NavBarPadding");
            }
        }
    }


    private double _ClippedNavBarHeightRequest;
    public double ClippedNavBarHeightRequest
    {
        get { return _ClippedNavBarHeightRequest; }
        set
        {
            if (_ClippedNavBarHeightRequest != value)
            {
                _ClippedNavBarHeightRequest = value;
                OnPropertyChanged();
            }
        }
    }


    public double CalculateNavBar(DeviceRotation Orientation)
    {
        if (HideNavigation)
            return 0.0;

        double value = Super.NavBarHeight;
        return value;
    }

    private Thickness _NavAndTabsMargin;
    public Thickness NavAndTabsMargin
    {
        get { return _NavAndTabsMargin; }
        set
        {
            if (_NavAndTabsMargin != value)
            {
                _NavAndTabsMargin = value;
                OnPropertyChanged();
            }
        }
    }




    public double CalculateClippedNavBar(DeviceRotation Orientation)
    {
        if (HideNavigation)
            return 0.0;

        double value = 110;
        return value;
    }

    protected double CalculateStatusBar(DeviceRotation Orientation)
    {


        var value = Super.StatusBarHeight;

        //if (Manager.Screen.TopInset > 0 && Display.IsFrameless)
        //{
        //    if (Orientation == PageInsideShell.Rotation.Landscape)
        //    {
        //        value = 0;
        //    }
        //    else
        //    {
        //        value = Manager.UI.StatusBarHeight + Manager.Screen.TopInset;
        //    }
        //}
        //else
        //{
        //    //use standart height
        //}

        if (Orientation == DeviceRotation.Landscape)
        {

        }
        else
        {

            if (Super.Screen.BottomInset > 0)
                PaddingBottom = Super.Screen.BottomInset + 2;
            else
                PaddingBottom = 0;


        }

        return value;
    }

    #endregion




    protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        base.OnPropertyChanged(propertyName);

        switch (propertyName)
        {


        case nameof(NoMenu):

        NoBell = true;
        NoFilter = true;

        break;

        }

    }


    public static readonly BindableProperty HideNavigationProperty = BindableProperty.Create(
        nameof(HideNavigation),
        typeof(bool),
        typeof(NavigationViewModel),
        false);

    public bool HideNavigation
    {
        get { return (bool)GetValue(HideNavigationProperty); }
        set { SetValue(HideNavigationProperty, value); }
    }

    public static readonly BindableProperty NoMenuProperty = BindableProperty.Create(
        nameof(NoMenu),
        typeof(bool),
        typeof(NavigationViewModel),
        false);

    public bool NoMenu
    {
        get { return (bool)GetValue(NoMenuProperty); }
        set { SetValue(NoMenuProperty, value); }
    }

    public static readonly BindableProperty HasNewNotificationsProperty = BindableProperty.Create(
        nameof(HasNewNotifications),
        typeof(bool),
        typeof(NavigationViewModel),
        false);

    public bool HasNewNotifications
    {
        get { return (bool)GetValue(HasNewNotificationsProperty); }
        set { SetValue(HasNewNotificationsProperty, value); }
    }

    public static readonly BindableProperty NewNotificationsCountProperty = BindableProperty.Create(
        nameof(NewNotificationsCount),
        typeof(int),
        typeof(NavigationViewModel),
        0);

    public int NewNotificationsCount
    {
        get { return (int)GetValue(NewNotificationsCountProperty); }
        set { SetValue(NewNotificationsCountProperty, value); }
    }

    public static readonly BindableProperty VisiblePageProperty = BindableProperty.Create(
        nameof(VisiblePage),
        typeof(string),
        typeof(NavigationViewModel),
        null);

    public string VisiblePage
    {
        get { return (string)GetValue(VisiblePageProperty); }
        set { SetValue(VisiblePageProperty, value); }
    }



    #region LEGACY

    //-------------------------------------------------------------
    // NoBell
    //-------------------------------------------------------------
    private const string nameNoBell = "NoBell";
    public static readonly BindableProperty NoBellProperty = BindableProperty.Create(nameNoBell, typeof(bool), typeof(NavigationViewModel), false); //, BindingMode.TwoWay
    public bool NoBell
    {
        get { return (bool)GetValue(NoBellProperty); }
        set { SetValue(NoBellProperty, value); }
    }


    //-------------------------------------------------------------
    // NoFilter
    //-------------------------------------------------------------
    private const string nameNoFilter = "NoFilter";
    public static readonly BindableProperty NoFilterProperty = BindableProperty.Create(nameNoFilter, typeof(bool), typeof(NavigationViewModel), false); //, BindingMode.TwoWay
    public bool NoFilter
    {
        get { return (bool)GetValue(NoFilterProperty); }
        set { SetValue(NoFilterProperty, value); }
    }


    #endregion


    private bool _ShowGoBack;
    public bool ShowGoBack
    {
        get { return _ShowGoBack; }
        set
        {
            if (_ShowGoBack != value)
            {
                _ShowGoBack = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _CanGoBack = true;
    public bool CanGoBack
    {
        get { return _CanGoBack; }
        set
        {
            if (_CanGoBack != value)
            {
                _CanGoBack = value;
                OnPropertyChanged();
            }
        }
    }




    public void SetDrawerMenuIcon()
    {
        //DrawerIconSource = FaPro.Bars;

        DrawerIconSource = "menu.svg";

    }

    private string _DrawerIconSource;
    private readonly IServiceProvider _services;

    public string DrawerIconSource
    {
        get { return _DrawerIconSource; }
        set
        {
            if (_DrawerIconSource != value)
            {
                _DrawerIconSource = value;
                OnPropertyChanged();
            }
        }
    }

    public string BuildDesc
    {
        get
        {
            return $"Build {this.Build} от {this.BuildTime:g}";
        }
    }




}