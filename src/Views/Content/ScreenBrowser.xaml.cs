namespace AppoMobi.Maui.DrawnUi.Demo.Views.Content;

public partial class ScreenBrowser
{
    private readonly SimplePageViewModel _vm;

    /// <summary>
    /// Can pass url or html, if html is passed isUrl must be false
    /// </summary>
    /// <param name="vm"></param>
    /// <param name="title"></param>
    /// <param name="source"></param>
    /// <param name="isUrl"></param>
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