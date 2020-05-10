using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CameraDemo.ViewModels
{
    class PhotoViewModel :  BaseViewModel
    {
        private string _filePath;
        private ImageSource _image;

        public PhotoViewModel()
        {
            TakePhotoCommand = new Command(
                async () =>
                {
                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Test",
                        SaveToAlbum = true,
                        CompressionQuality = 75,
                        CustomPhotoSize = 50,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 2000,
                        DefaultCamera = CameraDevice.Front
                    });
                    if (file != null)
                    {
                        FilePath = file.Path;
                        Image = new FileImageSource { File = FilePath };
                    }                    
                },
                () => { return (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported); }
            );
            PickPhotoCommand = new Command(
                async () =>
                {
                    var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    });
                    if (file != null)
                    {
                        FilePath = file.Path;
                        Image = new FileImageSource { File = FilePath };
                    }
                },
                () => { return CrossMedia.Current.IsPickPhotoSupported; }
            );
            TakeVideoCommand = new Command(
                async () =>
                {
                    var file = await CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
                    {
                        Name = "video.mp4",
                        Directory = "DefaultVideos",
                    });
                    if (file != null)
                    {
                        FilePath = file.Path;
                        Image = new FileImageSource { File = FilePath };
                    }
                },
                () => { return (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakeVideoSupported); }
            );
            PickVideoCommand = new Command(
                async () =>
                {
                    var file = await CrossMedia.Current.PickVideoAsync();
                    if (file != null)
                    {
                        FilePath = file.Path;
                        Image = new FileImageSource { File = FilePath };
                    }
                },
                () => { return CrossMedia.Current.IsPickVideoSupported; }
            );
        }

        public Command TakePhotoCommand { get; set; }
        public Command PickPhotoCommand { get; set; }
        public Command TakeVideoCommand { get; set; }
        public Command PickVideoCommand { get; set; }
        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }
        public ImageSource Image
        {
            get { return _filePath; }
            set { SetProperty(ref _image, value); }
        }
    }
}
