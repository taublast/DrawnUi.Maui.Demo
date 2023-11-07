namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenItemDetails
{
    private readonly ItemDetailsViewModel _vm;

    public ScreenItemDetails(ItemDetailsViewModel vm)
    {
        try
        {
            _vm = vm;

            BindingContext = _vm;

            InitializeComponent();
        }
        catch (Exception e)
        {
            Super.DisplayException(this, e);
        }
    }


    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        var check = this.BindingContext;

    }
}