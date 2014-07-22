using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TommyJams;

namespace TommyJams.Model
{
    public class ArtistInfo
    {
        private string _artistName;
        public string ArtistName
        {
            get
            {
                return _artistName;
            }
            set
            {
                if (value != _artistName)
                {
                    _artistName = value;
                }
            }
        }


        private string _artistImage;
        public string ArtistImage
        {
            get
            {
                return _artistImage;
            }
            set
            {
                if (value != _artistImage)
                {
                    _artistImage = value;
                }
            }
        }

        private string _artistDescription;
        public string ArtistDescription
        {
            get
            {
                return _artistDescription;
            }
            set
            {
                if (value != _artistDescription)
                {
                    _artistDescription = value;
                }
            }
        }

        private string _artistCity;
        public string ArtistCity
        {
            get
            {
                return _artistCity;
            }
            set
            {
                if (value != _artistCity)
                {
                    _artistCity = value;
                }
            }
        }

        private string _artistCountry;
        public string ArtistCountry
        {
            get
            {
                return _artistCountry;
            }
            set
            {
                if (value != _artistCountry)
                {
                    _artistCountry = value;
                }
            }
        }

        private string _artistFacebook;
        public string ArtistFacebook
        {
            get
            {
                return _artistFacebook;
            }
            set
            {
                if (value != _artistFacebook)
                {
                    _artistFacebook = value;
                }
            }
        }

        private string _artistSocial;
        public string ArtistSocial
        {
            get
            {
                return _artistSocial;
            }
            set
            {
                if (value != _artistSocial)
                {
                    _artistSocial = value;
                }
            }
        }

    }
}
