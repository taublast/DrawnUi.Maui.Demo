using AppoMobi.Maui.DrawnUi.Demo.Helpers;
using AppoMobi.Maui.DrawnUi.Demo.Interfaces;
using AppoMobi.Maui.DrawnUi.Demo.Services;
using AppoMobi.Maui.DrawnUi.Demo.Views.Content;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;


public class ScrollingCellsViewModel : ProjectViewModel, IFullscreenGalleryManager
{
    public ScrollingCellsViewModel(NavigationViewModel navModel) : base(navModel)
    {
        Title = "SkiaScroll + SkiaLayout = CollectionView";

        Items = new();

        _mock = new MockDataProvider();
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

                    App.Shell.ShowToast("A drawn CollectionView 😋👍 alternative built with a ***SkiaScroll*** and a ***SkiaLayout***. Only those visible on screen cells are rendered, refresh view uses ___SkiaLottie___. Can slide cells to reveal a mock edit control.");

                }).ConfigureAwait(false);
            });
        }
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
                    Item = context as SimpleItemViewModel;
                    if (Item != null)
                    {
                        //todo find index of the passed url
                        var index = Items.IndexOf(Item);
                        if (index >= 0)
                            SelectedGalleryIndex = index;
                        else
                            SelectedGalleryIndex = 0;

                        var gallery = new PopupGallerySlider(this);

                        await Presentation.Shell.OpenPopupAsync(gallery.AttachControl, true);
                    }

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

    public IList<string> GalleryItems
    {
        get
        {
            //return all images from Items
            var items = Items.Select(x => x.Banner).ToList();
            return items;
        }
    }

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

    public string Title
    {
        get
        {
            return _title;
        }

        set
        {
            if (_title != value)
            {
                _title = value;
                OnPropertyChanged();
            }
        }
    }
    string _title;
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

                    _mock.ResetIndex();
                    await AddItemsToUi(_mock.GetRandomItems(PageSize), true, Items.Count != 0);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    IsLoading = false;
                    IsBusy = false;
                }

            });
        }
    }

    CancellationTokenSource CancelPreload { get; set; }

    /// <summary>
    /// Will set IsBusy to false after items are added
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    async Task AddItemsToUi(IEnumerable<SimpleItemViewModel> items, bool reset = false, bool mainThread = true)
    {
        //SkiaImageLoadingManager.LogEnabled = true;

        CancelPreload?.Cancel();
        var cancel = new CancellationTokenSource();
        CancelPreload = cancel;

        async Task Action()
        {
            try
            {
                if (reset)
                {
                    Items.Clear();
                }
                //IsLoading = true;
                await Task.Delay(10);
                Items.AddRange(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                IsLoading = false;
            }
        }

        if (mainThread)
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                //simulate internet load for demo only!
                await Task.Delay(2000);

                await Action();
            });
        else
        {
            await Action();
        }

        //todo preload images in background
        if (_items.Count > 0 && !cancel.IsCancellationRequested)
        {
            var index = 0;
            var item = _items[index];
            while (!cancel.IsCancellationRequested)
            {
                if (!item.BannerPreloadOrdered)
                {
                    item.BannerPreloadOrdered = true;
                    await SkiaImageLoadingManager.Instance.Preload(item.Banner, cancel).ConfigureAwait(false);
                }
                index++;
                if (index > _items.Count - 1)
                {
                    break;
                }
                item = _items[index];
            }
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
                    await AddItemsToUi(_mock.GetRandomItems(PageSize));
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
