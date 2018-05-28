using MyMapNotes.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyMapNotes
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddMapNote : Page
    {
        private bool isViewing = false;
        private MapNote mapNote;

        public AddMapNote()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Replace MAP_SERVICE_TOKEN with token from bing maps dev center
            MyMap.MapServiceToken = "MAP_SERVICE_TOKEN";

            // Use the device's Geolocator to find it's physical location
            Geopoint myPoint;

            if (e.Parameter == null)
            {
                // Add
                isViewing = false;

                var locator = new Geolocator();
                locator.DesiredAccuracyInMeters = 50;

                /*   Add Location capability in the Package.appxmanifest file   */
                var position = await locator.GetGeopositionAsync();
                myPoint = position.Coordinate.Point;

            }
            else
            {
                // View or Delete
                isViewing = true;

                mapNote = (MapNote)e.Parameter;
                titleTextBox.Text = mapNote.Title;
                noteTextBox.Text = mapNote.Note;
                addButton.Content = "Delete";

                var myPosition = new BasicGeoposition();
                myPosition.Latitude = mapNote.Latitude;
                myPosition.Longitude = mapNote.Longitude;

                myPoint = new Geopoint(myPosition);
            }            
            await MyMap.TrySetViewAsync(myPoint, 10D);
        }

        private async void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (isViewing)
            {
                //Delete
                var messageDialog = new MessageDialog("Are you sure?");

                // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
                messageDialog.Commands.Add(new UICommand("Delete", new UICommandInvokedHandler(this.CommandInvokedHandler)));
                messageDialog.Commands.Add(new UICommand("Cancel", new UICommandInvokedHandler(this.CommandInvokedHandler)));

                // Set the command that will be invoked by default
                messageDialog.DefaultCommandIndex = 0;

                // Set the command to be invoked when escape is pressed
                messageDialog.CancelCommandIndex = 1;

                // Show the message dialog
                await messageDialog.ShowAsync();
            }
            else
            {
                /*Create a new instance of the MapNote class, set its properties including the Latitude and Longitude, then call the App.DataModel.AddMapNote() passing in the new instance of MapNote.*/
                // Add
                MapNote newMapNote = new MapNote();
                newMapNote.Title = titleTextBox.Text;
                newMapNote.Note = noteTextBox.Text;
                newMapNote.Created = DateTime.Now;
                newMapNote.Latitude = MyMap.Center.Position.Latitude;
                newMapNote.Longitude = MyMap.Center.Position.Longitude;
                App.DataModel.AddMapNote(newMapNote);
                /*  Navigate back to the MapNotes page.  */
                Frame.Navigate(typeof(MapNotes));
            }
        }//addButton_Click

        /*   Return to the map notes page.   */
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MapNotes));
        }

        /*   Make an "Are you sure?" popup dialog appear when the delete button 
             is pressed to prevent accidental deletion of the note.   */
        private void CommandInvokedHandler(IUICommand command)
        {
            if (command.Label == "Delete")
            {
                App.DataModel.DeleteMapNote(mapNote);
                Frame.Navigate(typeof(MapNotes));
            }
        }//CommandInvokedHandler

    }
}