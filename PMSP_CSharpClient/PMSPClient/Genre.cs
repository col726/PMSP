using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PMSPClient
{
    /// <summary>
    /// This is the Genre class used for audio genres.
    /// </summary>
    class Genre
    {
        //Private fields.
        string _name;
        List<Track> _tracks;

        //Public properties.
        public string Name { get { return _name; } }
        public List<Track> Tracks { get { return _tracks; } set { _tracks = value; } }

        //Main constructor.
        public Genre(string name)
        {
            _name = name;
        }

        //Get list of genres.
        public static List<Genre> GetList(Protocol protocol)
        {
            //Instantiate genre list.
            List<Genre> genres = new List<Genre>();

            //Get genres and insert them.
            try
            {
                //Drill down through child nodes to get genre listings.
                XmlNode genreList = protocol.GetMetadataList(ListType.Genre).SelectSingleNode("//MetadataListing");

                //Insert genres.
                foreach (XmlNode genre in genreList.ChildNodes)
                {
                    genres.Add(new Genre(genre.InnerText));
                }
            }
            catch (Exception ex) { }

            //Return list.
            return genres;
        }
    }
}
