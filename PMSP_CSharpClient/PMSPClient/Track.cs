/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PMSPClient
{
    /// <summary>
    /// This is the Track class used for representing mp3 audio files as objects.
    /// </summary>
    class Track
    {
        //Private fields.
        private string _title;
        private Artist _artist;

        //Public properties.
        public string Title { get { return _title; } }
        public string Artist { get { return _title; } }

        //Main constructor.
        public Track(string title, Artist artist)
        {
            _title = title;
            _artist = artist;
        }

        /// <summary>
        /// Gets a list of tracks from the server.
        /// </summary>
        /// <param name="artist">Optional artist parameter used to specify if only the tracks from a specific artists should be returned.</param>
        /// <returns></returns>
        public static List<Track> GetList(Protocol protocol, Artist artist)
        {
            //Instantiate track list.
            List<Track> tracks = new List<Track>();

            //Insert tracks.
            foreach (XmlNode track in protocol.GetList(ListType.Track).ChildNodes)
            {
                /*
                 * Insert tracks here.
                 */
            }

            /**********TEST DATA ONLY******************/
            tracks.Add(new Track("Intolerance", new Artist("Tool", "")));
            tracks.Add(new Track("Lateralus", new Artist("Tool", "")));
            /******************************************/

            //Return list.
            return tracks;
        }

        /// <summary>
        /// Streams a track using the PMSP protocol.
        /// </summary>
        /// <param name="protocol"></param>
        public void Stream(Protocol protocol)
        {
            /*
             * Stream track here.
             */

            Console.WriteLine("Now streaming " + this.Title + " by " + this._artist.DisplayName + "..." + Environment.NewLine);
        }
    }
}
