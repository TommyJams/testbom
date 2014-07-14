using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TommyJams.ViewModel
{
    public class Friend
    {
        public string id { get; set; }

        public string Name { get; set; }

        public Uri PictureUri { get; set; }
    }

    public class FacebookData
    {
        private static ObservableCollection<Friend> friends = new ObservableCollection<Friend>();

        public static ObservableCollection<Friend> Friends
        {
            get
            {
                return friends;
            }
        }
    }
}
