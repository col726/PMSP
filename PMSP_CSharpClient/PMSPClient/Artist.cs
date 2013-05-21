using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PMSPClient
{
    /// <summary>
    /// This is the Artist class used for audio artists.
    /// </summary>
    class Artist
    {
        //Private fields.
        string _firstNameOrBandName;
        string _lastName;
        List<Track> _tracks;

        //Public properties.
        public string FirstName { get { return _firstNameOrBandName; } }
        public string LastName { get { return _lastName; } }
        public string DisplayName { get { return (_firstNameOrBandName + " " + _lastName).Trim(); } }
        public List<Track> Tracks { get { return _tracks; } set { _tracks = value; } }

        //Main constructor.
        public Artist(string firstNameOrBandName, string lastName)
        {
            _firstNameOrBandName = firstNameOrBandName;
            _lastName = lastName;
        }

        //Get list of artists.
        public static List<Artist> GetList(Protocol protocol)
        {
            //Instantiate artist list.
            List<Artist> artists = new List<Artist>();

            //Insert artists.
            foreach(XmlNode artist in protocol.GetList(ListType.Artist).ChildNodes)
            {
                /*
                 * Add artists to list here.
                 */
            }

            /**********TEST DATA ONLY******************/
            artists.Add(new Artist("Rob", "Zombie"));
            artists.Add(new Artist("Tool", ""));
            /******************************************/

            //Return list.
            return artists;
        }
    }
}
