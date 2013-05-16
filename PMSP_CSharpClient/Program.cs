/*=========================Group/Course Information+========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/*=========================Program Description================================
 * Name : PMSPClient.
 * Purpose: This program interacts with PMSPServer hosted on another machine
 *          within the same network in order to stream mp3 audio files to the
 *          user, based on selections made from an artist and track listing.
 * Version: 1.0
 * Installation Instructions:
 *==========================================================================*/

/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMSPClient
{
    /// <summary>
    /// This is the main class for the program which serves as the application driver / entry point.
    /// </summary>
    class Program
    {
        /// <summary>
        /// This is the Main function for the program which serves as the program's entry point.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //Instantiate Protocol.
            var protocol = new Protocol();

            //Display welcome message.
            Console.WriteLine("Welcome to the PMSP Streaming Audio Player v1.0!" + Environment.NewLine + Environment.NewLine);

            //Connect to server.
            if (protocol.Connect())
            {
                //Authenticate.
                if (protocol.Authenticate())
                {
                    //Instantiate Menu object with valid options.
                    var menu = new Menu(new List<ConsoleKey> { ConsoleKey.A, ConsoleKey.T, ConsoleKey.Escape });

                    //Run the program.
                    while (1 == 1)
                    {
                        //Initialize variables.
                        List<Artist> artists;
                        List<Track> tracks;

                        //Perform specified action.
                        switch (menu.SelectedOption.ToString())
                        {
                            //List artists.
                            case "A":
                                
                                //Get artists.
                                artists = Artist.GetList(protocol);

                                //List artists.
                                Console.WriteLine(Environment.NewLine + "Here are the available artists:");
                                int artistCount = 1;
                                foreach (Artist artist in artists)
                                {
                                    Console.WriteLine(artistCount.ToString() + "." + "  " + artist.DisplayName);
                                    artistCount++;
                                }

                                //Get selected artist.
                                Console.WriteLine(Environment.NewLine + "Please enter the number of the artist of whom you'd like to listen:");
                                try
                                {
                                    //Get artist from list.
                                    Artist artist = artists[Convert.ToInt32(Console.ReadLine()) - 1];

                                    //Ensure selection is valid before proceeding.
                                    if (artist != null)
                                    {
                                        //Get tracks.
                                        artist.Tracks = Track.GetList(protocol, artist);

                                        //List tracks.
                                        Console.WriteLine(Environment.NewLine + "Here are the available tracks for " + artist.DisplayName + ":");
                                        int trackCount = 1;
                                        foreach (Track track in artist.Tracks)
                                        {
                                            Console.WriteLine(trackCount.ToString() + "." + "  " + track.Title);
                                            trackCount++;
                                        }

                                        //Get selected track.
                                        Console.WriteLine(Environment.NewLine + "Please enter the number of the track of which you'd like to listen:");
                                        try
                                        {
                                            //Get track from list.
                                            Track track = artist.Tracks[Convert.ToInt32(Console.ReadLine()) - 1];

                                            //Ensure selection is valid before proceeding.
                                            if (track != null)
                                            {
                                                //Stream track
                                                track.Stream(protocol);
                                            }
                                            else
                                            {
                                                Console.WriteLine("The track you entered was invalid.  Please try again.");
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("The track you entered was invalid.  Please try again.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("The artist you entered was invalid.  Please try again.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine("The artist you entered was invalid.  Please try again.");
                                }
                                
                                break;

                            //List tracks.
                            case "T":

                                //Get all tracks.
                                tracks = Track.GetList(protocol, null);

                                Console.WriteLine(Environment.NewLine + "Here are the available tracks:" + Environment.NewLine);
                                break;

                            //Exit program.
                            case "Escape":
                                Environment.Exit(0);
                                break;
                        }

                        //set next selected option
                        menu.SetSelectedOption();
                    }
                }
            }
        }
    }
}
