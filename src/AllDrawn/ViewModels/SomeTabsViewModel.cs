using AppoMobi.Maui.DrawnUi.Demo.Helpers;
using AppoMobi.Maui.DrawnUi.Demo.Services;
using System.Windows.Input;
using AppoMobi.Maui.DrawnUi.Demo.Interfaces;
using AppoMobi.Maui.DrawnUi.Demo.Views.Content;
using AppoMobi.Maui.DrawnUi.Infrastructure;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class SomeTabsViewModel : ProjectViewModel, IFullscreenGalleryManager
{
	public SomeTabsViewModel(NavigationViewModel navModel) : base(navModel)
	{
		Items = new();

		_mock = new MockDataProvider();

	}

    #region PICKER

    private int _PickerIndex;
    public int PickerIndex
    {
        get
        {
            return _PickerIndex;
        }
        set
        {
            if (_PickerIndex != value)
            {
                _PickerIndex = value;
                OnPropertyChanged();
            }
        }
    }


    #endregion

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

    #region GALLERY MANAGER

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

                    //this is fro animating popup onening not from screen center
                    //but from the cell middle location
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

                    var gallery = new PopupGallerySlider(this);

                    await Presentation.Shell.OpenPopupAsync(gallery.AttachControl, true,
                        true, cellCenter);

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


    #endregion

	string _title = "WithTabs!!";

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

	private int _SelectedIndex;
	public int SelectedIndex
	{
		get
		{
			return _SelectedIndex;
		}
		set
		{
			if (_SelectedIndex != value)
			{
				_SelectedIndex = value;
				OnPropertyChanged();
			}
		}
	}

	private string _ImageUrl;
	public string ImageUrl
	{
		get
		{
			return _ImageUrl;
		}
		set
		{
			if (_ImageUrl != value)
			{
				_ImageUrl = value;
				OnPropertyChanged();
			}
		}
	}

	private int count = 0;

	void SetImage()
	{
		count++;
		ImageUrl = $"https://picsum.photos/200/200?s={count}";

		//"https://raw.githubusercontent.com/github/explore/80688e429a7d4ef2fca1e82350fe8e3517d3494d/collections/github-pages-examples/github-pages-examples.png";
		//$"https://picsum.photos/400?s={count}";
	}

	public ObservableRangeCollection<SimpleItemViewModel> Items { get; }

	private readonly MockDataProvider _mock;

	public ICommand RefreshCommandData
	{
		get
		{
			return new Command(async () =>
			{
				SetImage();

				if (Items.Count == 0)
				{
					Items.AddRange(_mock.GetRandomSmallItems(25));
				}
			});
		}
	}

    public ICommand RefreshCommandImage
    {
        get
        {
            return new Command(async () =>
            {
                SetImage();

            });
        }
    }




}

