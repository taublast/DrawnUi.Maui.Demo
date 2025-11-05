
using DrawnUi;
using System.Collections;
using System.Diagnostics;
using System.Windows.Input;

namespace AppoMobi.Maui.DrawnUi.Demo.Views.Controls
{

    public partial class WheelPicker
    {

        public WheelPicker()
        {
            Init();
        }

        public WheelPicker(int visibleItems)
        {
            Init();
            Scroller.VisibleItemCount = visibleItems;
        }

        void Init()
        {
            InitializeComponent();

            SetItemSource();

            Scroller.SelectedIndexChanged += OnIndexChanged;
        }

        public override void OnDisposing()
        {
            base.OnDisposing();

            Scroller.SelectedIndexChanged -= OnIndexChanged;
        }

        private void OnIndexChanged(object sender, int index)
        {
            UpdateIndex(index);

            if (IndexChanged!=null || IndexChangedCommand != null && IndexChangedCommand.CanExecute(index))
            {
                Tasks.StartDelayed(TimeSpan.FromMilliseconds(10), () =>
                {
                    IndexChanged?.Invoke(this, index);
                    IndexChangedCommand?.Execute(index);
                });
            }
        }

        public event EventHandler<int> IndexChanged;

        public static readonly BindableProperty IndexChangedCommandProperty = BindableProperty.Create(
            nameof(IndexChangedCommand),
            typeof(ICommand),
            typeof(WheelPicker),
            null);

        public ICommand IndexChangedCommand
        {
            get { return (ICommand)GetValue(IndexChangedCommandProperty); }
            set { SetValue(IndexChangedCommandProperty, value); }
        }


        public void UpdateIndex(int index)
        {
            SelectedIndex = index;
        }

        public static readonly BindableProperty LinesColorProperty = BindableProperty.Create(
            nameof(LinesColor),
            typeof(Color),
            typeof(WheelPicker),
            defaultValue: Colors.White, propertyChanged: NeedDraw);

        public Color LinesColor
        {
            get { return (Color)GetValue(LinesColorProperty); }
            set { SetValue(LinesColorProperty, value); }
        }

 
        public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create(
            nameof(ItemHeight),
            typeof(double),
            typeof(WheelPicker),
            44.0,
            propertyChanged: NeedInvalidateMeasure);

        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }


        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex),
        typeof(int),
        typeof(WheelPicker),
        -1,
        BindingMode.TwoWay,
        propertyChanged: OnUpdateStartupState);

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly BindableProperty DataSourceProperty = BindableProperty.Create(
            nameof(DataSource),
            typeof(IList),
            typeof(WheelPicker),
            null,
            propertyChanged: DataSourcePropertyChanged);

        public IList DataSource
        {
            get => (IList)GetValue(DataSourceProperty);
            set => SetValue(DataSourceProperty, value);
        }
        private static void DataSourcePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is WheelPicker control)
            {
                control.SetItemSource();
            }
        }

        public void UpdateDebug()
        {
            OnPropertyChanged(nameof(DebugInfo));
        }

        public void SetItemSource()
        {
            Wheel.ItemsSource = DataSource;

            Invalidate(); //do NOT remove this

            SetStartup();

            OnPropertyChanged(nameof(DebugInfo));
        }

        public string DebugInfo
        {
            get
            {
                if (this.DataSource == null || this.DataSource.Count < 1)
                    return "empty";

                //var sub = Wheel.Views[0];
                //var label = sub.Views[0] as SkiaLabel;
                //         return $"y {Scroller.ViewportOffsetY:0.0} items {Wheel.Views.Count}, [{sub.BindingContext}] [{label.BindingContext}] => '{label.Text}' as {label.TextColor.ToArgbHex()}";

                return $"VelocityY: {Scroller.VelocityY:0}";
            }
        }

        public void ApplyStartupIndex()
        {
            Scroller.SelectedIndex = SelectedIndex;
        }

        private static void OnUpdateState(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is WheelPicker control)
            {
                //todo 
            }
        }

        private static void OnUpdateStartupState(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is WheelPicker control)
            {
                control.SetStartup();
            }
        }

        protected void SetStartup()
        {
            ApplyStartupIndex();
        }

        protected override void OnLayoutReady()
        {
            base.OnLayoutReady();

            SetStartup();
        }

 
    }
}
