using WinDev_Emotions.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinDev_Emotions.ViewModel;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace WinDev_Emotions.View
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class PhotoPage : Page
    {

        private NavigationHelper navigationHelper;

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Let's have a reference to the view model in case you will need 
        /// some functions from it.
        /// 
        /// It takes the value of the DataContext set in the corresponding XAML file of this
        /// page and converts ("casts") it to become a PhotoViewModel class.
        /// </summary>
        public PhotoViewModel VM
        {
            get
            {
                return (PhotoViewModel)DataContext;
            }
        }

        public PhotoPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="Common.NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="Common.SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="Common.NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        /// <summary>
        /// This fires when the Attach button on the CommandBar is clicked.
        /// 
        /// What we attempt to do here is to allow the user to pick the image from the local
        /// storage via a file picker, then associates it to a corresponding property in the
        /// view model where the Source property of the Image control of this page will bind
        /// to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAttachClick(object sender, RoutedEventArgs e)
        {
            // Create an instance of the file picker and define its properties.
            var picker = new Windows.Storage.Pickers.FileOpenPicker()
            {
                ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail,
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary
            };
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            //Get the only file the user picked.
            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Get the properties of the file and see if it exceeds 4MB.
                // The 4MB limitation is based from the API's limitations.
                var properties = await file.GetBasicPropertiesAsync();
                var size = properties.Size;
                ulong maxSize = Convert.ToUInt64(4096000000); //4MB to bytes

                if (size > maxSize)
                {
                    // Show a message dialog notifying the user of the error, then end
                    // the operation.
                    MessageDialog md = new MessageDialog("The file is too large.");
                    await md.ShowAsync();

                    return;
                }

                // Read the file and get its equivalent value in bytes.
                // More info about streams: http://www.tutorialspoint.com/csharp/csharp_file_io.htm
                Stream bytesStream = await file.OpenStreamForReadAsync();
                byte[] bytes = new byte[(int)bytesStream.Length];
                bytesStream.Read(bytes, 0, (int)bytesStream.Length);

                // Assign 'bytes' to a corresponding property in the view model for
                // later use during uploading.
                //
                // Note the 'byte[]' data type. The view model property must be also
                // of data type 'byte[]'. 
                VM.ImageBytes = bytes;


                // Open the file, convert it to its own stream, and assign it to the
                // SelectedImage property at the view model, where the Source property
                // of the Image control will be data bound.

                // Note the 'BitmapImage' data type. The SelectedImage property at the view
                // model must be also of data type 'BitmapImage'. 
                using (FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read))
                {
                    // The Source property of the Image control accepts the BitmapImage
                    // data type. More info:
                    // https://msdn.microsoft.com/en-us/library/windows/apps/windows.ui.xaml.controls.image.source.aspx

                    BitmapImage img = new BitmapImage();
                    img.SetSource(stream);
                    VM.SelectedImage = img;
                }

            }
        }


        /// <summary>
        /// Calls the clear images method from the view model upon clicking the Clear button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            VM.ResetData();
        }


        private async void OnUploadClick(object sender, RoutedEventArgs e)
        {
            await VM.UploadAndAnalyzeImageAsync();
        }

        private void OnRectFaceTapped(object sender, TappedRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout(sender as Windows.UI.Xaml.Shapes.Path);
        }
    }
}
