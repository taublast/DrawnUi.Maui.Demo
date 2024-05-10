

namespace AppoMobi.Maui.DrawnUi.Demo.Views;

public partial class ScreenBrowser
{
    private readonly SimplePageViewModel _vm;


    public ScreenBrowser(string title, string source, bool isUrl = true)
    {
        try
        {
            InitializeComponent();

            _vm = App.Instance.Services.GetService<SimplePageViewModel>();
            BindingContext = _vm;


            LabelTile.Text = title;

            if (isUrl)
            {
                if (string.IsNullOrEmpty(source))
                {
                    source = "about:blank";
                }
                var url = new UrlWebViewSource
                {
                    Url = source
                };
                ControlBrowser.Source = url;
            }
            else
            {
                if (string.IsNullOrEmpty(source))
                {
                    source = "";
                }
                var html = new HtmlWebViewSource
                {
                    Html = source
                };
                ControlBrowser.Source = html;

            }

        }
        catch (Exception e)
        {
            Super.DisplayException(this, e);
        }
    }



}