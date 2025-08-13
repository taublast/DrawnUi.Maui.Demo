﻿using AppoMobi.Maui.Gestures;
using DrawnUi.Draw;

namespace SomeMauiApp
{

    public class DebugStack : StackLayout
    {

    }

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void HandleLinkTapped(object sender, string e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await App.Current.MainPage.DisplayAlert("A link was tapped!", e, "OK");
            });
        }

        private void OnSpanTapped(object sender, ControlTappedEventArgs controlTappedEventArgs)
        {
            if (sender is TextSpan span)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await App.Current.MainPage.DisplayAlert("A span was tapped!", span.Text, "OK");
                });
            }

        }

    }
}