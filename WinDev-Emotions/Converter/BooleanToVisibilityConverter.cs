using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace WinDev_Emotions.Converter
{
    /// <summary>
    /// This is the converter that will change a boolean value to a Visibility converter.
    /// This is implemented in the project as a way to show or hide progress bars.
    /// 
    /// You can call this by placing this inside the <Page> tag:
    ///     xmlns:conv="Windev_Emotions.Converter"
    /// 
    /// then placing this in the page's resources property, as:
    /// <Page.Resources>
    ///     <conv:BooleanToVisibilityConverter x:Key="BooleanToVisibilityConverter" />
    /// </Page.Resources>
    /// 
    /// And finally, placing it into a value, such as a progress bar:
    /// <ProgressBar Visibility={Binding PropertyOnVMHere, Converter={StaticResource BooleanToVisibilityConverter}} />
    /// 
    /// All converters will implment IValueConverter. Ctrl + period may be necessary.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Get the value from the binded property, convert it to a proper boolean
            // then return as Visibility.Visible if true, Visibility.Collapsed if false.
            if ((bool)value)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
