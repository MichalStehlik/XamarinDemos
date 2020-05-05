using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoRecords.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        private async Task OpenBrowser(Uri uri)
        {
            await Browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
        }

        public void Source_Clicked(object sender, EventArgs e)
        {
            _ = OpenBrowser(new Uri("https://github.com/MichalStehlik/XamarinDemos/tree/master/GeoRecords"));
        }
        public void Xamarin_Clicked(object sender, EventArgs e)
        {
            _ = OpenBrowser(new Uri("https://dotnet.microsoft.com/apps/xamarin"));
        }
        public void School_Clicked(object sender, EventArgs e)
        {
            _ = OpenBrowser(new Uri("https://web.pslib.cz/"));
        }
    }
}