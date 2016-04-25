/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="using:WinDev_Emotions.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

/* AFTER GROUPING THE DECLARATIONS INTO THEIR OWN SEPARATE METHODS AND REGIONS,
 * HERE'S WHAT YOU'LL DO EVERY TIME YOU CREATE A NEW VIEWMODEL:
 * - Register at RegisterViewModels
 * - Create a public property at the View model properties region. 
 * - If you want to get serious with MVVM, you may need to add some variables in 
 *      the NavigationService. This item is not required, however.
 *      More info: http://www.mvvmlight.net/doc/nav1.cshtml
 * 
 * Just follow the format of the sample provided :)
 * 
 * 
 */

using System;
using System.Diagnostics.CodeAnalysis;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using WinDev_Emotions.Design;
using WinDev_Emotions.Model; 

namespace WinDev_Emotions.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public const string SecondPageKey = "SecondPage";

        // Creating regions in your code will help you group your declarations and
        // hide them from your view if you want.
        #region View model properties
        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// The name "Photo" is what you will place as the binding path at the DataContext
        /// of the Photos page.
        /// </summary>
        public PhotoViewModel Photo
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PhotoViewModel>();
            }
        }
        #endregion

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // STARTING HERE, MOVE ALL OF THESE LINES TO A NEW PRIVATE STATIC VOID METHOD
            // CALLED RegisterServices. Don't forget to call them later inside this
            // code block too.
            // ========================================================================

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            //}
            //else
            //{
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            //var nav = new NavigationService();
            //nav.Configure(ViewModelLocator.SecondPageKey, typeof(SecondPage));
            //SimpleIoc.Default.Register<INavigationService>(() => nav);

            //SimpleIoc.Default.Register<IDialogService, DialogService>();

            //============ENDS HERE=====================================================
            RegisterServices();

            // MOVE THIS TO ITS OWN STATIC VOID METHOD TOO, CALLED RegisterViewModels.
            // =========================================================================

            //SimpleIoc.Default.Register<MainViewModel>();

            //============ENDS HERE=====================================================
            RegisterViewModels();
        }

        private static void RegisterViewModels()
        {
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PhotoViewModel>();
        }

        private static void RegisterServices()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            var nav = new NavigationService();
            nav.Configure(ViewModelLocator.SecondPageKey, typeof(SecondPage));
            
            SimpleIoc.Default.Register<INavigationService>(() => nav);

            SimpleIoc.Default.Register<IDialogService, DialogService>();
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}