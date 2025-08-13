using DrawnUi.Camera;
using Reversi.Views.Partials;
using System.Windows.Input;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels
{

    public class MainPageViewModel : ProjectViewModel
    {

        public string LastSavedPhoto 
        { 
            get => lastSavedPhoto;
            set
            {
                if (lastSavedPhoto != value)
                {
                    lastSavedPhoto = value;
                    OnPropertyChanged();
                }
            }
        }


        private LoadedImageSource _displayPreview;
        public LoadedImageSource DisplayPreview
        {
            get
            {
                return _displayPreview;
            }
            set
            {
                if (_displayPreview != value)
                {
                    _displayPreview = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShowPhoto));
                }
            }
        }

        public bool ShowPhoto
        {
            get
            {
                return DisplayPreview != null;
            }
            set
            {
            }
        }

        #region COMMANDS

        public ICommand CommandCamera
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet("cam", 3500) || App.Shell.HasTopmostModalBindingContext<TakePictureViewModel>())
                        return;

                    SkiaCamera.CheckPermissions(async (presented) =>
                        {
                            var vm = App.Instance.Services.GetService<TakePictureViewModel>();
                            //todo set callback
                            var content = new ScreenCameraPhoto(vm);
                            await App.Shell.PushModalAsync(content, false, true, true, new Dictionary<string, object>
                            {
                                {"callback", new Command(async (context) =>
                                {
                                    if (context is CapturedImage item)
                                    {
                                        DisplayPreview = new LoadedImageSource(item.Image);
                                        LastSavedPhoto = item.Path;

                                        /*
                                        using var bitmap = SKBitmap.FromImage(item.Image);
                                        var data = bitmap.Encode(SKEncodedImageFormat.Jpeg, 90);
                                        using var stream = data.AsStream();
            
                                        using var memoryStream = new MemoryStream();
                                        await stream.CopyToAsync(memoryStream);
                                        memoryStream.Position = 0;
                                        byte[] bytes = memoryStream.ToArray();
            
                                        //save to tmp..
                                        var tmpFilename = await SaveTmpFIle(bytes, "jpg");
            
                                        //save to app folder
                                        var storedFIlename = await CopyToAppDataDirectory(tmpFilename);                        
                                        App.Instance.User.Avatar = storedFIlename;
                                        App.Instance.SaveUser();
                                        Presentation.UpdateUser();
                                        */

                                        CommandCloseModal.Execute(null);

                                        App.Shell.ShowToast("Saved to camera roll");
                                    }
                                })}
                            });

                        },
                        (presented) =>
                        {
                            //no permissions no camera screen bye bye
                            App.Shell.ShowToast("No permissions");
                        });

                });
            }
        }




        public ICommand CommandTabReselected
        {
            get
            {
                return new Command(async (context) =>
                {
                    Presentation.PopToRoot();
                });
            }
        }

        public ICommand RefreshCommandData
        {
            get
            {
                return new Command(async () =>
                {
                    //await Task.Delay(2500);
                    LoadData(true);
                });
            }
        }



        public ICommand CommandPushModal
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
                        var content = new ModalContent();
                        await Presentation.Shell.PushModalAsync(content, true, true, true);

                    }).ConfigureAwait(false);

                });
            }
        }




        private bool _DrawerOpen;
        public bool DrawerOpen
        {
            get
            {
                return _DrawerOpen;
            }
            set
            {
                if (_DrawerOpen != value)
                {
                    _DrawerOpen = value;
                    OnPropertyChanged();
                }
            }
        }


        public ICommand CommandPushDrawer
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    DrawerOpen = true;
                });
            }
        }

        //public ICommand CommandPushDrawer
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            //do not block ui, lets us see the touch effect
        //            //while we build page to be opened
        //            await Task.Run(async () =>
        //            {
        //                var drawer = new SkiaDrawer()
        //                {
        //                    Tag = "Drawer",
        //                    Bounces = false,
        //                    ZIndex = 9999,
        //                    HeightRequest = 450,
        //                    VerticalOptions = LayoutOptions.End,
        //                    Direction = DrawerDirection.FromBottom,
        //                    HeaderSize = 0,
        //                };

        //                var content = new ModalContent();
        //                drawer.SetContent(content);

        //                void LayoutReadyHandler(object sender, EventArgs e)
        //                {
        //                    if (sender is SkiaDrawer control)
        //                    {
        //                        control.OnViewportReady -= LayoutReadyHandler;
        //                        MainThread.BeginInvokeOnMainThread(() =>
        //                        {
        //                            control.IsOpen = true;
        //                        });
        //                    }
        //                }

        //                void DrawerScrolled(object sender, bool inTransition)
        //                {
        //                    if (!inTransition && sender is SkiaDrawer { IsOpen: false } control)
        //                    {
        //                        control.OnTransitionChanged -= DrawerScrolled;
        //                        control.Dispose(); //will be removed from parent
        //                    }
        //                }

        //                drawer.OnViewportReady += LayoutReadyHandler;
        //                drawer.OnTransitionChanged += DrawerScrolled;

        //                drawer.SetParent(Presentation.Shell.ShellLayout);

        //            }).ConfigureAwait(false);


        //        });
        //    }
        //}


        public ICommand CommandPushFun
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
                        var page = new ScreenFun(new LakeViewModel());
                        await Presentation.Shell.PushAsync(page, true);

                    }).ConfigureAwait(false);

                });
            }
        }

        public ICommand CommandDebugAction
        {
            get
            {
                return new Command(async () =>
                {
                    Presentation.Shell.NavigationLayout.DebugAction();
                });
            }
        }

        public ICommand CommandPushCamera
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    if (DeviceInfo.Platform == DevicePlatform.Android)
                    {
                        await Task.Run(async () =>
                        {
                            //todo


                        }).ConfigureAwait(false);

                    }
                    else
                    {
                        App.Shell.ShowToast("Android only at the moment");
                    }

                });
            }
        }

        public ICommand CommandPushLottieAndFriends
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    await Task.Run(async () =>
                    {

                        await App.Shell.PushAsync(new ScreenAnims());

                    }).ConfigureAwait(false);

                });
            }
        }

        public ICommand CommandPushVarious
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    await Task.Run(async () =>
                    {

                        await App.Shell.PushAsync(new ScreenVarious());

                    }).ConfigureAwait(false);

                });
            }
        }

        public ICommand CommandPushMaui
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
                        var content = new ScreenBrowser("SkiaMauiElement - WebView", "https://dotnet.microsoft.com/en-us/apps/maui");
                        await App.Shell.PushModalAsync(content, true, true, true);

                        //var page = new ScreenVarious();
                        //await Presentation.Shell.PushDrawnAsync(page, true);

                    }).ConfigureAwait(false);

                });
            }
        }

        public ICommand CommandPushLabels
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
                        var page = new ScreenLabels();
                        await Presentation.Shell.PushAsync(page, true);

                    }).ConfigureAwait(false);
                });
            }
        }

        //public ICommand CommandPushPdf
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            if (CheckLockAndSet())
        //                return;

        //            await Task.Run(async () =>
        //            {
        //                await App.Shell.GoToAsync(AppRoutes.Xaml2Pdf.Route, true);
        //            }).ConfigureAwait(false);
        //        });
        //    }
        //}

        public ICommand CommandPushCarousel
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    await Task.Run(async () =>
                    {
                        await App.Shell.GoToAsync(AppRoutes.Carousel.Route, true);
                        //var page = new ScreenCarousel();
                        //await Presentation.Shell.PushDrawnAsync(page, true);

                    }).ConfigureAwait(false);
                });
            }
        }

        public ICommand CommandPushControls
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    await Task.Run(async () =>
                    {
                        await App.Shell.GoToAsync(AppRoutes.Controls.Route, true);

                    }).ConfigureAwait(false);
                });
            }
        }


        public ICommand CommandPushTransforms
        {
            get
            {
                return new Command(async () =>
                {
                    if (CheckLockAndSet())
                        return;

                    await Task.Run(async () =>
                    {
                        var page = new ScreenTransforms();
                        await Presentation.Shell.PushAsync(page, true);

                    }).ConfigureAwait(false);
                });
            }
        }

        #endregion




        //Presentation.BottomTabsHeightRequest
        public double Test
        {
            get
            {
                return 50;
            }
        }


        private int _SelectedIndex;
        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set
            {
                if (_SelectedIndex != value)
                {
                    _SelectedIndex = value;
                    OnPropertyChanged();
                }
            }
        }




        public ObservableRangeCollection<object> Items { get; } = new();



        public void LoadData(bool forceReload = false)
        {
            if (!IsBusy)
            {
                IsBusy = true;


                try
                {

                    //var items = ParallelEnumerable.Range(1, 1_000)
                    //	.AsParallel()
                    //	.Select(i => new OptionItem(i.ToString(), $"Option {i}", i % 3 == 0))
                    //	.ToList();


                    //Task.Run(() => MainThread.BeginInvokeOnMainThread(async () =>
                    //{
                    //	IsLoading = false;
                    //	Items.Clear();
                    //	await Task.Delay(10);
                    //	Items.AddRange(items);
                    //	HasData = true;
                    //	IsBusy = false;

                    //})).ConfigureAwait(false);


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    IsBusy = false;
                    IsLoading = false;
                }

            }
            ;
        }

        private bool _IsLoading;
        public bool IsLoading
        {
            get
            {
                return _IsLoading;
            }
            set
            {
                if (_IsLoading != value)
                {
                    _IsLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _HasData;
        private string lastSavedPhoto;

        public bool HasData
        {
            get
            {
                return _HasData;
            }
            set
            {
                if (_HasData != value)
                {
                    _HasData = value;
                    OnPropertyChanged();
                }
            }
        }

        public MainPageViewModel(NavigationViewModel navModel) : base(navModel)
        {

        }
    }
}
