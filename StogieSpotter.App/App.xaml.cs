﻿namespace StogieSpotter.App
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            //AppShell.Current.GoToAsync("//home//details");
        }
    }
}