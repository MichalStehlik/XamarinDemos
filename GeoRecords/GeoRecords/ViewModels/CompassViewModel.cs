using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeoRecords.ViewModels
{
    class CompassViewModel : BaseViewModel
    {
        private string _magneticNorth;
        public Command ToggleCompassCommand { get; set; }
        public CompassViewModel()
        {
            Compass.ReadingChanged += Compass_ReadingChanged;
            ToggleCompassCommand = new Command(
                () =>
                {
                    if (Compass.IsMonitoring)
                        Compass.Stop();
                    else
                        Compass.Start(SensorSpeed.Default);
                }
            );
        }

        void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var data = e.Reading;
            MagneticNorth = String.Format("{0:0.00}°", data.HeadingMagneticNorth);
        }
        public string MagneticNorth
        {
            get { return _magneticNorth; }
            set { SetProperty(ref _magneticNorth, value); }
        }
    }
}
