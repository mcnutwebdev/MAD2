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
using Windows.UI.Xaml.Media.Imaging;//BitmapImage
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyMapNotes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherPage : Page
    {
        public WeatherPage()
        {
            this.InitializeComponent();
        }

        private async void WeatherButton_Click(object sender, RoutedEventArgs e)
        {
            var position = await LocationManager.GetPosition();

            RootObject myWeather = await OpenWeatherMapProxy.GetWeather(position.Coordinate.Point.Position.Latitude, position.Coordinate.Point.Position.Longitude);
            /*   Use the icons from openweathermap.org using the url of whichever icon is required    */           
            string icon = String.Format("http://openweathermap.org/img/w/{0}.png", myWeather.weather[0].icon);
            
            /*   When using a uri for resources in the project use the prefix "ms-appx:///" in place of "http://" i.e.:
            string icon = String.Format("ms-appx:///Assets/Weather/{0}.png", myWeather.weather[0].icon);    */
            /*   Display the icon on the screen by creating a new BitmapImage and giving it a new Uri  */
            ResultImage.Source = new BitmapImage(new Uri(icon, UriKind.Absolute));
            /*   Print the location name, the temperature and weather conditions to the screen. Cast the temperature to an int and toString to remove the decimal values.   */
            CityTextBlock.Text = myWeather.name;
            TempTextBlock.Text = ((int)myWeather.main.temp).ToString();
            DescriptionTextBlock.Text = myWeather.weather[0].description;            
            LatitudeTextBlock.Text = "Latitude: " + position.Coordinate.Point.Position.Latitude.ToString();
            LongitudeTextBlock.Text = " Longitude: " + position.Coordinate.Point.Position.Longitude.ToString();
            /*  Hide the button */
            ButtonBox.Visibility = Visibility.Collapsed;
        }
    }
}
