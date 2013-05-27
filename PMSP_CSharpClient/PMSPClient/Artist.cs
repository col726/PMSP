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
        string _name;
        List<Track> _tracks;

        //Public properties.
        public string Name { get { return _name; } }
        public List<Track> Tracks { get { return _tracks; } set { _tracks = value; } }

        //Main constructor.
        public Artist(string name)
        {
            _name = name;
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
            artists.Add(new Artist("The Milk Carton Kids"));
            /******************************************/

            //Return list.
            return artists;
        }
    }
}
