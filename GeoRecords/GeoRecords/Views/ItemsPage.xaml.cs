using GeoRecords.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoRecords.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        private ListViewModel _vm;
        public ItemsPage()
        {
            InitializeComponent();
            _vm = (ListViewModel)this.BindingContext;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (_vm.Locations.Count == 0)
            {
                _vm.LoadCommand.Execute(null);
            }
        }
    }
}