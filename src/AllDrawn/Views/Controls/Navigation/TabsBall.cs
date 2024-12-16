namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls.Navigation
{
    /// <summary>
	/// A ball that moves between tabs to indicate which one is selected
	/// </summary>
	public class TabsBall : SkiaLayout
    {
        protected bool IsAnimationEnabled = true;

        public bool CanAnimate
        {
            get
            {
                return LayoutReady && IsAnimationEnabled;
            }
        }

        public TabsBall()
        {
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Fill;
        }


        public virtual SkiaShape CreateBall()
        {
            return new SkiaShape
            {
                Type = ShapeType.Circle,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
            };
        }

        public virtual void UpdateBall()
        {
            if (Ball != null)
            {
                Ball.HeightRequest = CircleRadius * 2;
                Ball.WidthRequest = CircleRadius * 2;
                Ball.BackgroundColor = Color;
                Ball.TranslationY = VerticalOffset;
            }
        }

        public SkiaShape Ball { get; set; }

        protected int LastSelectedIndex { get; set; } = -1;

        protected void OnSelectedIndexChanged()
        {
            if (LastSelectedIndex >= 0)
            {
                //Super.Log($"[BALL] {SelectedIndex}");
                Animate();
            }
            LastSelectedIndex = SelectedIndex;
        }

        CancellationTokenSource _cts;

        protected virtual void Animate()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            double toX = CalculateTargetHorizontalOffset(SelectedIndex);
            Ball.TranslateToAsync(toX, VerticalOffset, (uint)AnimationSpeedMs, Easing.CubicOut, _cts);
        }

        protected float CalculateTargetHorizontalOffset(int selectedTabIndex)
        {
            var availableWidth = Width;
            var tabWidth = availableWidth / TabsCount;

            var index = 0;
            if (selectedTabIndex > 0)
                index = selectedTabIndex;

            var targetHorizontalOffset = (tabWidth * index) + (tabWidth / 2) - CircleRadius;

            return (float)targetHorizontalOffset;
        }

        bool _firstLayout = true;

        /// <summary>
        /// Sets the initial ball position after layout was changed
        /// </summary>
        protected void UpdateBallPosition()
        {
            if (Width < 1)
                return;

            if (Ball == null)
                AddBall();

            Ball.TranslationX = CalculateTargetHorizontalOffset(SelectedIndex);
        }

        protected override void OnLayoutReady()
        {
            base.OnLayoutReady();

            AddBall();
        }

        /// <summary>
        /// We got the data to compute the initial ball position
        /// </summary>
        protected override void OnLayoutChanged()
        {
            base.OnLayoutChanged();

            UpdateBallPosition();
        }

        public override ScaledSize Measure(float widthConstraint, float heightConstraint, float scale)
        {
            return base.Measure(widthConstraint, heightConstraint, scale);
        }

        protected virtual void AddBall()
        {
            Ball = CreateBall();

            AddSubView(Ball);

            UpdateBall();
        }

        public static readonly BindableProperty AnimationSpeedMsProperty = BindableProperty.Create(
            nameof(AnimationSpeedMs),
            typeof(int),
            typeof(TabsBall),
            150);

        public int AnimationSpeedMs
        {
            get { return (int)GetValue(AnimationSpeedMsProperty); }
            set { SetValue(AnimationSpeedMsProperty, value); }
        }


        public static readonly BindableProperty CircleRadiusProperty = BindableProperty.Create(
            nameof(CircleRadius),
            typeof(float),
            typeof(TabsBall),
            8f, propertyChanged: OnNeedUpdateBall);

        public float CircleRadius
        {
            get { return (float)GetValue(CircleRadiusProperty); }
            set { SetValue(CircleRadiusProperty, value); }
        }

        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(
            nameof(SelectedIndex),
            typeof(int),
            typeof(TabsBall),
            -1, propertyChanged: SelectedIndexWasChanged);

        private static void SelectedIndexWasChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is TabsBall control)
            {
                control.OnSelectedIndexChanged();
            }
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly BindableProperty TabsCountProperty = BindableProperty.Create(
            nameof(TabsCount),
            typeof(int),
            typeof(TabsBall),
            1, propertyChanged: OnTabsLayoutChanged);

        private static void OnTabsLayoutChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is TabsBall control)
            {
                control.UpdateBallPosition();
            }
        }

        public int TabsCount
        {
            get { return (int)GetValue(TabsCountProperty); }
            set { SetValue(TabsCountProperty, value); }
        }

        public static readonly BindableProperty ColorProperty = BindableProperty.Create(
            nameof(Color),
            typeof(Color),
            typeof(TabsBall),
            Colors.Red, propertyChanged: OnNeedUpdateBall);

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly BindableProperty VerticalOffsetProperty = BindableProperty.Create(
            nameof(VerticalOffset),
            typeof(float),
            typeof(TabsBall),
            0.0f, propertyChanged: OnNeedUpdateBall);

        private static void OnNeedUpdateBall(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is TabsBall control)
            {
                control.UpdateBall();
            }
        }

        public float VerticalOffset
        {
            get { return (float)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }


    }
}
