using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TommyJams.Resources;
using Microsoft.Phone.Controls;

namespace TommyJams.Model
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Priority1Items = new ObservableCollection<EventItem>();
            this.PriorityItems = new ObservableCollection<EventItem>();
            this.Priority2Items = new ObservableCollection<EventItem>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<EventItem> Priority1Items { get; private set; }
        public ObservableCollection<EventItem> PriorityItems { get; private set; }
        public ObservableCollection<EventItem> Priority2Items { get; private set; }


        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Priority1Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aebhasdhsdarosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "../Model/Babel.mp3", Image = "../Model/play.gif" });
            this.Priority1Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Something", Distance = "5 Km", Venue = "10D", SongLink = "../Model/I Will Wait.mp3", Image = "../Model/play.gif" });

            this.PriorityItems.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "../Model/Babel.mp3", Image = "../Model/play.gif" });
            this.PriorityItems.Add(new EventItem() { Genre = "alternative rock", BandName = "Something", Distance = "5 Km", Venue = "10D", SongLink = "../Model/I Will Wait.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "../Model/Babel.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Something", Distance = "5 Km", Venue = "10D", SongLink = "../Model/I Will Wait.mp3", Image = "../Model/play.gif" });
            this.Priority2Items.Add(new EventItem() { Genre = "alternative rock", BandName = "Aerosmith", Distance = "3 Km", Venue = "Hard Rock Cafe", SongLink = "../Model/Babel.mp3", Image = "../Model/play.gif" });
            


            /*this.Items.Add(new ItemViewModel() { ID = "2", LineOne = "runtime three", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { ID = "3", LineOne = "runtime four", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { ID = "4", LineOne = "runtime five", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { ID = "5", LineOne = "runtime six", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { ID = "6", LineOne = "runtime seven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { ID = "7", LineOne = "runtime eight", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });
            this.Items.Add(new ItemViewModel() { ID = "8", LineOne = "runtime nine", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            this.Items.Add(new ItemViewModel() { ID = "9", LineOne = "runtime ten", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            this.Items.Add(new ItemViewModel() { ID = "10", LineOne = "runtime eleven", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            this.Items.Add(new ItemViewModel() { ID = "11", LineOne = "runtime twelve", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            this.Items.Add(new ItemViewModel() { ID = "12", LineOne = "runtime thirteen", LineTwo = "Maecenas praesent accumsan bibendum", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            this.Items.Add(new ItemViewModel() { ID = "13", LineOne = "runtime fourteen", LineTwo = "Dictumst eleifend facilisi faucibus", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            this.Items.Add(new ItemViewModel() { ID = "14", LineOne = "runtime fifteen", LineTwo = "Habitant inceptos interdum lobortis", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            this.Items.Add(new ItemViewModel() { ID = "15", LineOne = "runtime sixteen", LineTwo = "Nascetur pharetra placerat pulvinar", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

             */
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}