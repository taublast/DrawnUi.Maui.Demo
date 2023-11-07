using AppoMobi.Maui.DrawnUi.Demo.Helpers;
using AppoMobi.Maui.DrawnUi.Demo.Services;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class SomeTabsViewModel : ProjectViewModel
{
	public SomeTabsViewModel(NavigationViewModel navModel) : base(navModel)
	{
		Items = new();

		_mock = new MockDataProvider();

	}

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

