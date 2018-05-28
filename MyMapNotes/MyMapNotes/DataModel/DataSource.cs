using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;// Allows us to observe the collection of mapNotes
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MyMapNotes.DataModel
{
    public class MapNote
    {
        /*   Create a data model for the note.  */
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DataSource
    {
        /*   Create a collection of notes that will be managed by the methods in this class e.g. adding new notes, retrieving notes from local storage.    */
        private ObservableCollection<MapNote> _mapNotes;

        /*   Create a constant for saving and retrieving data on disk.   */
        const string fileName = "mapnotes.json";

        public DataSource()
        {
            _mapNotes = new ObservableCollection<MapNote>();
        }

        public async Task<ObservableCollection<MapNote>> GetMapNotes()
        {
            await ensureDataLoaded();
            return _mapNotes;
        }

        private async Task ensureDataLoaded()
        {
            if (_mapNotes.Count == 0)
                await getMapNoteDataAsync();

            return;
        }

        private async Task getMapNoteDataAsync()
        {
            if (_mapNotes.Count != 0)
                return;

            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<MapNote>));

            try
            {
                // Add a using System.IO;
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    _mapNotes = (ObservableCollection<MapNote>)jsonSerializer.ReadObject(stream);
                }
            }
            catch
            {
                _mapNotes = new ObservableCollection<MapNote>();
            }
        }

        public async void AddMapNote(MapNote mapNote)
        {
            _mapNotes.Add(mapNote);
            await saveMapNoteDataAsync();
        }

        public async void DeleteMapNote(MapNote mapNote)
        {
            _mapNotes.Remove(mapNote);
            await saveMapNoteDataAsync();
        }

        private async Task saveMapNoteDataAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<MapNote>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName,
                CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, _mapNotes);
            }
        }
    }
}