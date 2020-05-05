using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeoRecords.ViewModels
{
    class LocationViewModel : BaseViewModel
    {
        private Location _location;
        private string _latitude;
        private string _longitude;
        private string _altitude;
        private Placemark _placemark;
        private string _address;
        private string _locality;

        public LocationViewModel()
        {
            GetCoordinatesCommand = new Command(
                async () => 
                {
                    try
                    {
                        IsBusy = true;
                        Location = await Geolocation.GetLastKnownLocationAsync();
                        //var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                        //Location = Geolocation.GetLocationAsync(request).Result;
                        if (Location != null)
                        {
                            Latitude = String.Format("{0:0.00000}°", Location.Latitude);
                            Longitude = String.Format("{0:0.00000}°", Location.Longitude);
                            Altitude = String.Format("{0:0.00} m", Location.Altitude);
                        }
                        else
                        {
                            Latitude = "Unknown";
                            Longitude = "Unknown";
                            Altitude = "Unknown";
                        }
                        IsBusy = false;
                    }
                    catch (FeatureNotSupportedException fnsEx)
                    {
                        Latitude = fnsEx.Message;
                        Longitude = fnsEx.Message;
                        Altitude = fnsEx.Message;
                    }
                    catch (FeatureNotEnabledException fneEx)
                    {
                        Latitude = fneEx.Message;
                        Longitude = fneEx.Message;
                        Altitude = fneEx.Message;
                    }
                    catch (PermissionException pEx)
                    {
                        Latitude = pEx.Message;
                        Longitude = pEx.Message;
                        Altitude = pEx.Message;
                    }
                    catch (Exception ex)
                    {
                        Latitude = ex.Message;
                        Longitude = ex.Message;
                        Altitude = ex.Message;
                    }

                    try
                    {
                        var placemarks = await Geocoding.GetPlacemarksAsync(Location.Latitude, Location.Longitude);
                        _placemark = placemarks?.FirstOrDefault();
                        Address = _placemark.Thoroughfare + " " + _placemark.SubThoroughfare;
                        Locality = _placemark.Locality + ", " + _placemark.CountryName;
                    }
                    catch (Exception ex)
                    {
                        Address = ex.Message;
                        Locality = ex.Message;
                    }
                },
                () => { return !IsBusy; }
            );
            ShowPointOnMapCommand = new Command(
                async () => 
                {
                    if (Location != null)
                    {
                        await Map.OpenAsync(Location, new MapLaunchOptions { Name = _placemark != null ? (_placemark.Thoroughfare + " " + _placemark.SubThoroughfare) : "Place", NavigationMode = NavigationMode.None });
                    }                    
                },
                () =>
                {
                    return Location != null;
                }
            );
            AddToListCommand = new Command(
                () =>
                {
                    IsBusy = true;
                    MessagingCenter.Send(this, "AddLocation", Location);
                    Task.Delay(500);
                    IsBusy = false;
                },
                () =>
                {
                    return (Location != null);
                }
            );
        }

        public Command GetCoordinatesCommand { get; set; }
        public Command ShowPointOnMapCommand { get; set; }
        public Command AddToListCommand { get; set; }
        public Location Location
        {
            get 
            { 
                return _location; 
            }
            set 
            { 
                SetProperty(ref _location, value);
                ShowPointOnMapCommand.ChangeCanExecute();
                AddToListCommand.ChangeCanExecute();
            }
        }

        public string Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value); }
        }

        public string Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value); }
        }

        public string Altitude
        {
            get { return _altitude; }
            set { SetProperty(ref _altitude, value); }
        }
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
        public string Locality
        {
            get { return _locality; }
            set { SetProperty(ref _locality, value); }
        }
    }
}
