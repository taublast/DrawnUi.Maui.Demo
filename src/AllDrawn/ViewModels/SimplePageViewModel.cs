namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;

public class SimplePageViewModel : ProjectViewModel
{

    public SimplePageViewModel(NavigationViewModel navModel) : base(navModel)
    {

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

    private bool _SomeBoolean;
    public bool SomeBoolean
    {
        get
        {
            return _SomeBoolean;
        }
        set
        {
            if (_SomeBoolean != value)
            {
                _SomeBoolean = value;
                OnPropertyChanged();
            }
        }
    }


    private string _Search;
    public string Search
    {
        get
        {
            return _Search;
        }
        set
        {
            if (_Search != value)
            {
                _Search = value;
                OnPropertyChanged();
            }
        }
    }

}