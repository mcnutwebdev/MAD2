using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MyMapNotes
{
    public class LocationManager
    {
        /**/
        public async static Task<Geoposition> GetPosition()
        {
            var acccessStatus = await Geolocator.RequestAccessAsync();

            if (acccessStatus != GeolocationAccessStatus.Allowed) throw new Exception();

            var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };

            var position = await geolocator.GetGeopositionAsync();

            return position;
        }
    }
}
