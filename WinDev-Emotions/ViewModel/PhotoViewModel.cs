using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinDev_Emotions.APIs.MSCognitive;
using WinDev_Emotions.Model;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace WinDev_Emotions.ViewModel
{
    /// <summary>
    /// This is the view model for the Photos page.
    /// 
    /// It inherits the MVVMLibrary's ViewModelBase, which contains some defaults 
    /// you may want to use. INotifyPropertyChanged is also part of this, so
    /// all fully-defined properties (via propfull) can now use a built-in Set() method from
    /// MVVMLight to raise every change in the properties automatically without you having
    /// to define it yourself.
    /// </summary>
    public class PhotoViewModel : ViewModelBase
    {

        /// <summary>
        /// The boolean property that enables or disables the upload button. Useful when 
        /// there's no image yet or when the image is being analyzed.
        /// 
        /// It is fully-defined, with the Set() method from MVVMLight implemented, meaning
        /// everything that binds to this will know if this property changes, and will 
        /// update accordingly.
        /// </summary>
        private bool _isUploadButtonEnabled;

        public bool IsUploadButtonEnabled
        {
            get { return _isUploadButtonEnabled; }
            set { Set("IsUploadButtonEnabled", ref _isUploadButtonEnabled, value); }
        }

        /// <summary>
        /// The boolean property that enables or disables the progress bar.
        /// 
        /// It is fully-defined, with the Set() method from MVVMLight implemented, meaning
        /// everything that binds to this will know if this property changes, and will 
        /// update accordingly.
        /// </summary>
        private bool _isProgressBarEnabled;

        public bool IsProgressBarEnabled
        {
            get { return _isProgressBarEnabled; }
            set { Set("IsProgressBarEnabled", ref _isProgressBarEnabled, value); }
        }



        /// <summary>
        /// The BitmapImage property where the Image control will be bound. This property
        /// contains the file selected by the user from the picker.
        /// 
        /// It is fully-defined, with the Set() method from MVVMLight implemented, meaning
        /// everything that binds to this will know if this property changes, and will 
        /// update accordingly.
        /// </summary>
        private BitmapImage _selectedImage;

        public BitmapImage SelectedImage
        {
            get { return _selectedImage; }
            set { Set("SelectedImage", ref _selectedImage, value); }
        }

        /// <summary>
        /// The collection of emotion data from the API where the set of overlay rectangles
        /// will be bound to.
        /// 
        /// It is fully-defined, with the Set() method from MVVMLight implemented, meaning
        /// everything that binds to this will know if this property changes, and will 
        /// update accordingly. Changes in each EmotionData will not be detected unless we
        /// create a new class that derives from it and implements INotifyPropertyChanged.
        /// </summary>
        private ObservableCollection<EmotionData> _detectedFaces;

        public ObservableCollection<EmotionData> DetectedFaces
        {
            get { return _detectedFaces; }
            set { Set("DetectedFaces", ref _detectedFaces, value); }
        }


        /// <summary>
        /// The array of bytes that form the selected image.
        /// 
        /// It is not a fully-defined property because it is not designed to be bound
        /// to an element. It only acts as a storage facility, so it does not need to
        /// broadcast about its changes.
        /// </summary>
        public byte[] ImageBytes { get; set; }

        /// <summary>
        /// This is the constructor of the view model. All of the methods inside
        /// this block will be the first ones that will run when this view model
        /// is created ("instantiated").
        /// </summary>
        public PhotoViewModel()
        {
            IsUploadButtonEnabled = false;
            IsProgressBarEnabled = false;

            // Fires when any of the properties defined above changes its value.
            PropertyChanged += PhotoViewModel_PropertyChanged;
        }

        private void PhotoViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedImage":
                    if (SelectedImage != null)
                    {
                        IsUploadButtonEnabled = true;
                    }
                    else
                    {
                        IsUploadButtonEnabled = false;
                        ResetData();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Uploads the image stored from the properties here and returns a representation
        /// of the object data.
        /// </summary>
        /// <returns></returns>
        public async Task UploadAndAnalyzeImageAsync()
        {
            try
            {
                IsProgressBarEnabled = true;

                //receive the JSON string if successful.
                string rawData = await EmotionApi.PostImageAsync(SelectedImage, ImageBytes);

                // From the derived raw string of JSON from the server, it will be
                // deserialized to the model. See Model/EmotionData.cs for the definition files.
                // 
                // Before implementing this, you have to go to:
                // Tools->Package Manager->Package Manager Console
                // then type (without quotes) "Install-Package Newtonsoft.Json".
                // Ctrl + period when necessary.
                List<EmotionData> people = JsonConvert.DeserializeObject<List<EmotionData>>(rawData);

                // Now let's translate the data from the server to the RectGeometry property
                // of each EmotionData, which represents the list of heads and their emotions,
                // based on the API.
                foreach (var emotion in people)
                {
                    // create a geometry
                    RectangleGeometry geom = new RectangleGeometry();
                    geom.Rect = new Windows.Foundation.Rect(emotion.FaceRectangle.Left, emotion.FaceRectangle.Top, emotion.FaceRectangle.Width, emotion.FaceRectangle.Height);

                    // attach the geometry to the property
                    emotion.RectGeometry = geom;
                }

                IsProgressBarEnabled = false;

                // The emotion data is now being assigned to the observable collection
                // where the overlay of rectangles will be bound to.
                DetectedFaces = new ObservableCollection<EmotionData>(people);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Clears the images. Duh.
        /// </summary>
        public void ResetData()
        {
            SelectedImage = null;
            ImageBytes = null;
            DetectedFaces = null;
        }


    }
}
