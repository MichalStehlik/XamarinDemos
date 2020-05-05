using Xamarin.Essentials;
using GeoRecords.Models;
using GeoRecords.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace GeoRecords.ViewModels
{
    class ListViewModel : BaseViewModel
    {
        private ObservableCollection<StoredLocation> _locations = new ObservableCollection<StoredLocation>();
        private AppDbContext _db;

        public ListViewModel()
        {
            _db = App.Db;
            LoadCommand = new Command(
                async () => {
                    IsBusy = true;
                    Locations = new ObservableCollection<StoredLocation>(await _db.GetItemsAsync());
                    IsBusy = false;
                }
            );
            RemoveCommand = new Command<int>(
                async (id) => {
                    await _db.DeleteItemAsync(id);
                    LoadCommand.Execute(null);
                }
            );
            MapCommand = new Command<int>(
                async (id) =>
                {
                    var item = await _db.GetItemAsync(id);
                    await Map.OpenAsync(
                        new Location { 
                            Latitude = item.Latitude, 
                            Longitude = item.Longitude, 
                            Altitude = item.Altitude
                        }, 
                        new MapLaunchOptions { 
                            Name = item.Description,
                            NavigationMode = NavigationMode.None 
                        }
                    );
                }              
            );
            ClearCommand = new Command(
                () =>
                {
                    _db.ClearItems();
                    LoadCommand.Execute(null);
                }
            );
            MessagingCenter.Subscribe<LocationViewModel>(this, "UpdateLocations", (sender) =>
            {
                LoadCommand.Execute(null);
            });
            MessagingCenter.Subscribe<LocationViewModel, Location>(this, "AddLocation", async (sender, location) =>
            {
                var result = await _db.AddItemAsync(new StoredLocation
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Altitude = location.Altitude,
                    Date = DateTime.Now
                });
                if (!result)
                {
                    MessagingCenter.Send(this, "ShowAlert", "There was an error.");
                }
                LoadCommand.Execute(null);
            });
        }

        public Command LoadCommand { get; set; }
        public Command<int> RemoveCommand { get; set; }
        public Command<int> MapCommand { get; set; }
        public Command ClearCommand { get; set; }
        public Command MapAllCommand { get; set; }
        public ObservableCollection<StoredLocation> Locations
        {
            get { return _locations; }
            set { SetProperty(ref _locations, value); }
        }
    }
}
