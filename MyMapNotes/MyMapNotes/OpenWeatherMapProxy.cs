using System;
using System.Collections.Generic;
using System.IO;//MemoryStream
using System.Linq;
using System.Net.Http;//HttpClient
using System.Runtime.Serialization;//DataContract, DataMember
using System.Runtime.Serialization.Json;//DataContractJsonSerializer
using System.Text;
using System.Threading.Tasks;

namespace MyMapNotes
{
    class OpenWeatherMapProxy
    {
        /*   Use a static method to pass the latitude and longitude parameters to the web service url. These parameters will ultimately be read from the device's geolocator data, but are just being hard-coded in this step of development. This method needs to be async as it will wait for the results to come from the web service. As this is an async method it's return must be void, a Task or a Task<T>. By using Task<T> it is not returning a RootObject but the return will be treated as typeof RootObject.    */
        public async static Task<RootObject> GetWeather(double lat, double lon)
        {
            /*   Use HttpClient to contact the web service. Add HttpClient by using NuGet Package Manager (package name = Microsoft.Net.Http   */
            var http = new HttpClient();
            /*   Get a response object of type http ResponseMessage. Append "&units=metric" to the query string to give the result in Centigrade as the default is Kelvin.  */
            /*
             * NOTE: Change the response to show the local data specific to the device location rather than the hard coded location 
             * NOTE: Replace "API_KEY_HERE" with the API key
            var response = await http.GetAsync("http://api.openweathermap.org/data/2.5/weather?lat=54&lon=-10&units=metric&appid=API_KEY_HERE");

            */
            /* NOTE: Replace "API_KEY_HERE" with the API key*/
            var url = String.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=API_KEY_HERE", lat, lon);
            var response = await http.GetAsync(url);


            /*   Read the content of the response from the service as a string.   */
            var result = await response.Content.ReadAsStringAsync();
            /*   Convert the result string into an object graph. This is done by using DataContractJsonSerializer   */
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            /*   Use a MemoryStream object to save the data in memory until it is needed.   */
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
            /*   Retrieve the data from the serializer.   */
            var data = (RootObject)serializer.ReadObject(ms);

            return data;
        }
    }

    /*   Use json2csharp (http://json2csharp.com/) to serialaize the json data from openweathermap (http://openweathermap.org/) into c# classes   */


    /*   The classes and their properties need to be adorned with special attributes to tell DataContractJsonSerializer to treat  the classes (adorned with [DataContract] as classes and the properties (adorned with [DataMember]) as attributes of those classes  */
    [DataContract]
    public class Coord
    {
        [DataMember]
        public double lon { get; set; }

        [DataMember]
        public double lat { get; set; }
    }

    [DataContract]
    public class Weather
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string main { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string icon { get; set; }
    }

    [DataContract]
    public class Main
    {
        [DataMember]
        public double temp { get; set; }

        [DataMember]
        public double pressure { get; set; }

        [DataMember]
        public int humidity { get; set; }

        [DataMember]
        public double temp_min { get; set; }

        [DataMember]
        public double temp_max { get; set; }

        [DataMember]
        public double sea_level { get; set; }

        [DataMember]
        public double grnd_level { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public double speed { get; set; }

        [DataMember]
        public double deg { get; set; }
    }

    [DataContract]
    public class Clouds
    {
        [DataMember]
        public int all { get; set; }
    }

    [DataContract]
    public class Sys
    {
        [DataMember]
        public double message { get; set; }

        [DataMember]
        public string country { get; set; }

        [DataMember]
        public int sunrise { get; set; }

        [DataMember]
        public int sunset { get; set; }
    }

    [DataContract]
    public class RootObject
    {
        [DataMember]
        public Coord coord { get; set; }

        [DataMember]
        public List<Weather> weather { get; set; }

        [DataMember]
        public string @base { get; set; }

        [DataMember]
        public Main main { get; set; }

        [DataMember]
        public Wind wind { get; set; }

        [DataMember]
        public Clouds clouds { get; set; }

        [DataMember]
        public int dt { get; set; }

        [DataMember]
        public Sys sys { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int cod { get; set; }
    }

}
