using AppoMobi.Maui.DrawnUi.Demo.Interfaces;
using AppoMobi.Maui.DrawnUi.Demo.Services;
using AppoMobi.Maui.DrawnUi.Demo.Views;
using DrawnUi.Maui.Infrastructure;
using System.Threading.Channels;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;



public class ScrollingCellsViewModel : ProjectViewModel, IFullscreenGalleryManager
{
    public ScrollingCellsViewModel(NavigationViewModel navModel) : base(navModel)
    {
        Title = "SkiaScroll + SkiaLayout = CollectionView";

        Items = new();
        ItemsSmall = new();

        _mock = new MockDataProvider();
    }


    /// <summary>
    /// Selected Item
    /// </summary>
    public SimpleItemViewModel Item
    {
        get
        {
            return _item;
        }
        set
        {
            if (_item != value)
            {
                _item = value;
                OnPropertyChanged();
            }
        }
    }
    private SimpleItemViewModel _item;

    public ICommand CommandOpenHGallery
    {
        get
        {
            return new Command(async (context) =>
            {
                if (CheckLockAndSet())
                    return;

                await Task.Run(async () =>
                {
                    SKPoint? cellCenter = null;
                    SelectedGalleryIndex = 0;

                    if (context is SkiaTouchResultContext touchContext)
                    {
                        //calculate data to animate gallery popup towards the center popup
                        var location = touchContext.Control.GetPositionOnCanvas();
                        cellCenter = new SKPoint(
                            location.X + touchContext.Control.MeasuredSize.Pixels.Width / 2f,
                            location.Y + touchContext.Control.MeasuredSize.Pixels.Height / 2f);
                        context = touchContext.Context;

                    }

                    if (context is SimpleItemViewModel item)
                    {
                        Item = item;

                        //todo find index of the passed url
                        var index = ItemsSmall.IndexOf(Item);
                        if (index >= 0)
                            SelectedGalleryIndex = index;
                    }

                    GalleryItems = ItemsSmall.Select(x => x.Banner).ToList();
                    var gallery = new PopupGallerySlider(this);
                    await Presentation.Shell.OpenPopupAsync(gallery.AttachControl, true,
                        true, true, Color.Parse("#EE000000"), cellCenter);

                }).ConfigureAwait(false);


            });
        }
    }


    public ICommand CommandOpenGallery
    {
        get
        {
            return new Command(async (context) =>
            {
                if (CheckLockAndSet())
                    return;

                await Task.Run(async () =>
                {
                    SKPoint? cellCenter = null;
                    SelectedGalleryIndex = 0;

                    if (context is SkiaTouchResultContext touchContext)
                    {
                        //calculate data to animate gallery popup towards the center popup
                        var location = touchContext.Control.GetPositionOnCanvas();
                        cellCenter = new SKPoint(
                            location.X + touchContext.Control.MeasuredSize.Pixels.Width / 2f,
                            location.Y + touchContext.Control.MeasuredSize.Pixels.Height / 2f);
                        context = touchContext.Context;

                    }

                    if (context is SimpleItemViewModel item)
                    {
                        Item = item;

                        //todo find index of the passed url
                        var index = Items.IndexOf(Item);
                        if (index >= 0)
                            SelectedGalleryIndex = index;
                    }

                    GalleryItems = Items.Select(x => x.Banner).ToList();
                    var gallery = new PopupGallerySlider(this);
                    await Presentation.Shell.OpenPopupAsync(gallery.AttachControl, true,
                        true, true, Color.Parse("#EE000000"), cellCenter);

                }).ConfigureAwait(false);


            });
        }
    }


    #region IGalleryManager

    public ICommand CommandCloseGallery
    {
        get
        {
            return new Command(async () =>
            {
                await App.Shell.ClosePopupAsync(true);
            });
        }
    }

    public ICommand CommandPressingImage
    {
        get
        {
            return new Command(async () =>
            {
                try
                {
                    var image = GalleryItems[SelectedGalleryIndex];
                    CommandShareUrl.Execute(image);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }
    }

    public IList<string> GalleryItems { get; protected set; }

    private int _SelectedGalleryIndex;
    public int SelectedGalleryIndex
    {
        get
        {
            return _SelectedGalleryIndex;
        }
        set
        {
            if (_SelectedGalleryIndex != value)
            {
                _SelectedGalleryIndex = value;
                OnPropertyChanged();
            }
        }
    }


    #endregion

    private int _PageSize = 21;
    public int PageSize
    {
        get
        {
            return _PageSize;
        }
        set
        {
            if (_PageSize != value)
            {
                _PageSize = value;
                OnPropertyChanged();
            }
        }
    }

    private readonly MockDataProvider _mock;


    private int _Columns = 1;

    public int Columns
    {
        get
        {
            return _Columns;
        }
        set
        {
            if (_Columns != value)
            {
                _Columns = value;
                OnPropertyChanged();
            }
        }
    }


    public ICommand CommandSetColumns
    {
        get
        {
            return new Command(async (param) =>
            {
                if (param is string parameter)
                {
                    Columns = parameter.ToInteger();
                }
            });
        }
    }

    public ObservableRangeCollection<SimpleItemViewModel> ItemsSmall { get; }


    public ICommand CommandRefreshSmallData
    {
        get
        {
            return new Command(async () =>
            {
                //if (ItemsSmall.Count == 0)
                {
                    _mock.ResetIndexSmall();
                    var data = _mock.GetRandomSmallItems(10);

                    CancelPreloadSmall?.Cancel();
                    await AddItemsToUi(data, ItemsSmall, true);

                    var cancel = new CancellationTokenSource();
                    CancelPreloadSmall = cancel;
                    Tasks.StartDelayed(TimeSpan.FromSeconds(3), () =>
                    {
                        SkiaImageManager.Instance.PreloadBanners(data, cancel);
                    });
                }
            });
        }
    }



    public ICommand CommandRefreshData
    {
        get
        {
            return new Command(async () =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                IsLoading = true;

                try
                {
                    CommandRefreshSmallData.Execute(null);

                    CancelPreload?.Cancel();
                    _mock.ResetIndex();

                    var data = _mock.GetRandomItems(PageSize);
                    await AddItemsToUi(data, Items, true, Items.Count != 0);

                    //preload images in background
                    var cancel = new CancellationTokenSource();
                    CancelPreload = cancel;
                    Tasks.StartDelayed(TimeSpan.FromSeconds(3), () =>
                    {
                        SkiaImageManager.Instance.PreloadBanners(data, cancel); //you can use PreloadImages intead
                    });

                }
                catch (Exception e)
                {
                    Super.Log(e);

                    IsLoading = false;
                    IsBusy = false;
                }

            });
        }
    }

    CancellationTokenSource CancelPreload { get; set; }
    CancellationTokenSource CancelPreloadSmall { get; set; }

    /// <summary>
    /// Will set IsBusy to false after items are added
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    async Task AddItemsToUi(IEnumerable<SimpleItemViewModel> items, ObservableRangeCollection<SimpleItemViewModel> collection, bool reset = false, bool mainThread = true)
    {

        async Task Action()
        {
            try
            {
                if (reset)
                {
                    collection.Clear();
                }
                await Task.Delay(10);

                collection.AddRange(items);
            }
            catch (Exception ex)
            {
                Super.Log(ex);
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        //simulate internet load for demo only!
        await Task.Delay(1500);

        if (mainThread)
            await Dispatcher.DispatchAsync(async () =>
            {


                await Action();
            });
        else
        {
            await Action();
        }

    }

    public ICommand CommandLoadMore
    {
        get
        {
            return new Command(async (object context) =>
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                try
                {
                    var data = _mock.GetRandomItems(PageSize);

                    CancelPreload?.Cancel();
                    await AddItemsToUi(_mock.GetRandomItems(PageSize), Items);

                    //preload images in background
                    var cancel = new CancellationTokenSource();
                    CancelPreload = cancel;
                    Tasks.StartDelayed(TimeSpan.FromSeconds(3), () =>
                    {
                        SkiaImageManager.Instance.PreloadBanners(data, cancel);
                    });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    //IsLoading = false;
                    IsBusy = false;
                }
            });
        }
    }

    public ICommand CommandAbout
    {
        get
        {
            return new Command(async (object context) =>
            {
                if (CheckLockAndSet())
                    return;

                //do not block ui, lets us see the touch effect
                //while we build page to be opened
                await Task.Run(async () =>
                {

                    App.Shell.ShowToast("This demo is loading images from internet in realtime. 🚀🤩😋");

                }).ConfigureAwait(false);
            });
        }
    }



    public ObservableRangeCollection<SimpleItemViewModel> Items { get; }

    /// <summary>
    /// Using in background to preload banners
    /// </summary>
    public List<SimpleItemViewModel> _items { get; } = new();

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


}
