using AppoMobi.Maui.DrawnUi.Controls;
using AppoMobi.Maui.DrawnUi.Infrastructure.Models;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls
{
    /// <summary>
    ///  For faster rendering we are not using Maui Bindings that act on UI thread
    /// </summary>
    public class FastCellWithBanner : SkiaDynamicDrawnCell
    {
        public override void OnAppearing()
        {
            base.OnAppearing();

            //Debug.WriteLine($"[Cell] OnAppearing {Uid}");

            if (!_appeared)
            {
                _appeared = true;

                Opacity = 0;
                FadeToAsync(1, 750, Easing.Linear);

                SetContent();

                if (ImageBanner != null)
                    ImageBanner.LoadSourceOnFirstDraw = true; //steroids up!
            }

        }

        public override void OnScrolled()
        {
            base.OnScrolled();

            if (Drawer != null && !Drawer.IsOpen)
            {
                var thisY = this.DrawingRect.Top;
                if (Math.Abs(thisY - _lastY) > 50) //pixels
                {
                    Drawer.IsOpen = true;
                }
            }
            else
            {
                _lastY = this.DrawingRect.Top;
            }
        }

        public FastCellWithBanner()
        {

        }

        bool once;
        private double _lastY;
        private bool _appeared;
        protected SkiaDrawer Drawer;
        protected SkiaLabel LabelId;
        protected SkiaLabel LabelTitle;
        protected SkiaLabel LabelDesc;
        protected SkiaImage ImageBanner;
        protected SkiaControl ImagePlaceholder;

        public override void OnDisposing()
        {
            if (ImageBanner != null)
            {
                ImageBanner.OnSuccess -= OnImageLoaded;
                ImageBanner.OnCleared -= OnImageCleared;
            }

            base.OnDisposing();

            Drawer = null;
            LabelTitle = null;
            LabelDesc = null;
            ImageBanner = null;
            ImagePlaceholder = null;
            LabelId = null;
        }

        private void OnImageLoaded(object sender, ContentLoadedEventArgs e)
        {
            if (ImagePlaceholder != null)
                ImagePlaceholder.IsVisible = false;
        }

        private void OnImageCleared(object sender, EventArgs e)
        {
            if (ImagePlaceholder != null)
                ImagePlaceholder.IsVisible = true;
        }

        void FindViews()
        {
            if (Drawer == null)
            {
                Drawer = FindView<SkiaDrawer>("Drawer");
            }
            if (LabelId == null)
            {
                LabelId = FindView<SkiaLabel>("LabelId");
            }
            if (LabelTitle == null)
            {
                LabelTitle = FindView<SkiaLabel>("LabelTitle");
            }
            if (LabelDesc == null)
            {
                LabelDesc = FindView<SkiaLabel>("LabelDesc");
            }
            if (ImageBanner == null)
            {
                ImageBanner = FindView<SkiaImage>("ImageBanner");
                if (ImageBanner != null)
                {
                    ImageBanner.OnSuccess += OnImageLoaded;
                    ImageBanner.OnCleared += OnImageCleared;
                }
            }
            if (ImagePlaceholder == null)
            {
                ImagePlaceholder = FindViewByTag("ImagePlaceholder");
            }
        }

        int contextChanged;

        protected virtual bool ApplyContext()
        {
            _lastY = this.DrawingRect.Top;
            if (Drawer != null && !Drawer.IsOpen)
            {
                Drawer.IsOpen = true;
            }

            if (BindingContext is SimpleItemViewModel item)
            {
                contextChanged++;


                if (LabelId != null)
                {
                    LabelId.Text = item.Id.ToString();
                }
                if (LabelTitle != null)
                {
                    LabelTitle.Text = item.Title;
                }
                if (LabelDesc != null)
                {
                    LabelDesc.Text = item.Description;
                }

                //if (ImagePlaceholder != null)
                //{
                //    ImagePlaceholder.IsVisible = !string.IsNullOrEmpty(item.Banner);
                //}

                if (ImageBanner != null && _appeared)
                {
                    ImageBanner.StopLoading();
                    item.BannerPreloadOrdered = true;
                    ImageBanner.Source = item.Banner;
                }


                return true;
            }

            return false;
        }

        protected override void SetContent()
        {
            if (!once)
            {
                once = true;
                FindViews();
            }

            LockUpdate(true);

            if (ImageBanner != null)
            {
                ImageBanner.StopLoading();
            }

            var set = ApplyContext();

            LockUpdate(false);

            if (set)
            {
                UpdateCell();
            }
        }

        public virtual void UpdateCell()
        {
            if (contextChanged > 1)
                InvalidateChildrenTree(); //order to remeasure inside views for new content

            Update();
        }




    }
}
