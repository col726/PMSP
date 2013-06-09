/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/*=========================Class Description================================
 * Name : Artist.
 * Purpose: This is the Artist class used for audio artists.
 * Version: 1.0
 *==========================================================================*/

/*=======================Directives and Pragmas=============================*/
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

            //Get artists and insert them.
            try
            {
                //Drill down through child nodes to get artist listings.
                XmlNode artistList = protocol.GetMetadataList(ListType.Artist).SelectSingleNode("//MetadataListing");

                //Insert artists into list.
                foreach (XmlNode artist in artistList.ChildNodes)
                {
                    artists.Add(new Artist(artist.InnerText));
                }
            }
            catch (Exception ex) { }
            
            //Return list.
            return artists;
        }
    }
}
