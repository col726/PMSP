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

            //Drill down through child nodes to get artist listings.
            XmlNode artistList = protocol.GetMetadataList(ListType.Artist).SelectSingleNode("//MetadataListing");

            //Insert artists.
            foreach (XmlNode artist in artistList.ChildNodes)
            {
                artists.Add(new Artist(artist.InnerText));
            }

            //Return list.
            return artists;
        }
    }
}
