using GeoRecords.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeoRecords.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<ListViewModel, string>(this, "ShowAlert", (sender, msg) => { DisplayAlert("Info", msg, "Ok"); });
            MessagingCenter.Subscribe<LocationViewModel, string>(this, "ShowAlert", (sender, msg) => { DisplayAlert("Info", msg, "Ok"); });
        }
    }
}
