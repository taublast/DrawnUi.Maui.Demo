using System.Diagnostics;

namespace AppoMobi.Maui.DrawnUi.Demo.ViewModels;


[QueryProperty("Id", "id")]
public class ItemDetailsViewModel : ProjectViewModel
{
    private string _Id;
    public string Id
    {
        get { return _Id; }
        set
        {
            var newValue = Uri.UnescapeDataString(value ?? string.Empty);
            if (_Id != newValue)
            {
                _Id = newValue;
                OnPropertyChanged();
                OnParametersSet(_Id);
            }
        }
    }

    private void OnParametersSet(string id)
    {
        Debug.WriteLine($"[ViewModel] OnParametersSet Id: {id}");

        //if (CommandLoadItem.CanExecute(true))
        //    CommandLoadItem.ExecuteAsync();
    }

    public ItemDetailsViewModel(NavigationViewModel navModel) : base(navModel)
    { }
}