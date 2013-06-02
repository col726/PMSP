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
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using NAudio.Wave;

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
            //Display welcome message.
            Console.WriteLine("Welcome to the PMSP Streaming Audio Player v1.0!");

            //Write new line.
            Utilities.WriteNewLine();

            //Instantiate Menu object with valid options.
            var menu = new Menu("Would you like us to search for a PMSP server on the local area network for you?  Enter (y) for yes, (n) for no, or ESC to exit the program:",
                                new List<ConsoleKey> { ConsoleKey.Y, ConsoleKey.N, ConsoleKey.Escape });

            //Write new line.
            Utilities.WriteNewLine();

            //Instantiate Protocol.
            Protocol protocol = new Protocol();

            //Run.
            while (1 == 1)
            {
                //Perform specified action.
                switch (menu.SelectedOption.ToString())
                {
                    //Auto-discover PMSP server.
                    case "Y":

                        //Set user credentials.
                        protocol.SetCredentials();

                        //Inform user of time required for auto-discovery, and assumptions.
                        Console.WriteLine(String.Concat("PMSP auto-discovery is now in progress.  This may take a few minutes to complete.  PMSP auto-discovery assumes PMSP server(s) have ping enabled and firewalls configured to allow incoming traffic to port ",
                                          protocol.Server.Port,
                                          ".  Please wait..."));

                        //Write new line.
                        Utilities.WriteNewLine();

                        //Get list of active IPv4 addresses on LAN.
                        List<string> ips = LAN.GetActiveIp4Addresses();

                        //For each IP in list, attempt PMSP handshake until successful.
                        foreach (string ip in ips)
                        {
                            //Set server ip.
                            protocol.Server.Ip = ip;

                            //Attempt handshake
                            if (protocol.Authenticate())
                            {
                                break;
                            }
                        }
                        
                        //Inform user of auto-discovery result.
                        if (protocol.IsConnected)
                        {
                            Console.WriteLine("Congratulations!  You have successfully connected to PMSP Server " + protocol.Server.Url + ".");
                        }
                        else
                        {
                            Console.WriteLine("Unfortunately, we were unable to connect to a PMSP Server on your network.");
                        }

                        break;

                    //Manually specify PMSP server.
                    case "N":

                        //Prompt user for PMSP URL.
                        Console.WriteLine("Please specify the full URL, including port number, of the PMSP Server to which you'd like to connect and press ENTER:");

                        //Set PMSP URL.
                        protocol.Server.Url = Console.ReadLine().Trim();

                        //Write new line.
                        Utilities.WriteNewLine();

                        //Set user credentials.
                        protocol.SetCredentials();

                        //Attempt handshake
                        if (protocol.Authenticate())
                        {
                            Console.WriteLine("Congratulations!  You have successfully logged in to PMSP Server " + protocol.Server.Url + ".");
                        }
                        else
                        {
                            Console.WriteLine("Unfortunately, we were unable to log you in to PMSP Server " + protocol.Server.Url + ".  " + protocol.Exception);
                        }

                        //Write new line.
                        Utilities.WriteNewLine();
                        
                        break;

                    //Exit program.
                    case "Escape":
                        CleanUp();
                        Environment.Exit(0);
                        break;
                }

                //If we're connected, proceed.
                if (protocol.IsAuthenticated)
                {
                    //Instantiate Menu object with valid options.
                    menu = new Menu("Would you like to browse artists or tracks?  Please enter (a) for artists, (t) for tracks, or ESC to exit the program:", new List<ConsoleKey> { ConsoleKey.A, ConsoleKey.T, ConsoleKey.Escape, ConsoleKey.R });

                    //Write new line.
                    Utilities.WriteNewLine();

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

                                //Write new line.
                                Utilities.WriteNewLine();

                                //Get artists.
                                artists = Artist.GetList(protocol);

                                //List artists.
                                Console.WriteLine("Here are the available artists:");
                                int artistCount = 1;
                                foreach (Artist artist in artists)
                                {
                                    Console.WriteLine(artistCount.ToString() + "." + "  " + artist.Name);
                                    artistCount++;
                                }

                                //Write new line.
                                Utilities.WriteNewLine();

                                //Get selected artist.
                                Console.WriteLine("Please enter the number of the artist of whom you'd like to listen:");
                                try
                                {
                                    //Get artist from list.
                                    Artist artist = artists[Convert.ToInt32(Console.ReadLine()) - 1];

                                    //Ensure selection is valid before proceeding.
                                    if (artist != null)
                                    {
                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Get tracks.
                                        artist.Tracks = Track.GetList(protocol, artist);

                                        //List tracks.
                                        Console.WriteLine("Here are the available tracks for " + artist.Name + ":");
                                        int trackCount = 1;
                                        foreach (Track track in artist.Tracks)
                                        {
                                            Console.WriteLine(trackCount.ToString() + "." + "  " + track.Title);
                                            trackCount++;
                                        }

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Get selected track.
                                        Console.WriteLine("Please enter the number of the track of which you'd like to listen:");
                                        try
                                        {
                                            //Get track from list.
                                            Track track = artist.Tracks[Convert.ToInt32(Console.ReadLine()) - 1];

                                            //Ensure selection is valid before proceeding.
                                            if (track != null)
                                            {
                                                //Inform user of track retrieval.
                                                Console.WriteLine("Retrieving track, please wait...");

                                                //Write new line.
                                                Utilities.WriteNewLine();

                                                //Stream track
                                                track.Stream(protocol);

                                                //Put thread to sleep until the track is loaded.
                                                while (!track.IsLoaded)
                                                {
                                                    Thread.Sleep(100);
                                                }

                                                //If the track is playing, inform user.
                                                if (track.Audio.PlaybackState == PlaybackState.Playing)
                                                {
                                                    //Inform user of title and artist.
                                                    Console.WriteLine("Now streaming " + track.Title + " by " + track.Artist.Name + "...");

                                                    //Write new line.
                                                    Utilities.WriteNewLine();

                                                    //Instantiate Menu object with valid options.
                                                    Menu playbackMenu = new Menu("Enter (s) to stop playback or ESC to exit the program:", new List<ConsoleKey> { ConsoleKey.S, ConsoleKey.Escape });

                                                    //Run.
                                                    while (1 == 1)
                                                    {
                                                        //Perform specified action.
                                                        switch (playbackMenu.SelectedOption.ToString())
                                                        {
                                                            //Stop track
                                                            case "S":

                                                                //Stop track
                                                                if (track.Audio.PlaybackState == PlaybackState.Playing)
                                                                {
                                                                    track.Stop();
                                                                }
                                                                break;

                                                            //Exit program.
                                                            case "Escape":

                                                                //Stop track
                                                                if (track.Audio.PlaybackState == PlaybackState.Playing)
                                                                {
                                                                    track.Stop();
                                                                }

                                                                CleanUp();
                                                                Environment.Exit(0);
                                                                break;
                                                        }

                                                        break;
                                                    }
                                                }

                                                //If the track isn't playing, inform user.
                                                else
                                                {
                                                    //Inform user and append exception.
                                                    Console.WriteLine("Unfortunately, we were unable to stream the track you selected due to the following reason: " + track.Exception);

                                                    //Write new line.
                                                    Utilities.WriteNewLine();
                                                }
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

                                //New line
                                Utilities.WriteNewLine();

                                //Inform user of available tracks.
                                Console.WriteLine("Here are the available tracks:");

                                //Write new line.
                                Utilities.WriteNewLine();

                                break;
                            
                            //Retrieve file. **TEMP**
                            case "R":

                                System.Xml.XmlDocument xml = protocol.GetFile("1");

                                break;

                            //Exit program.
                            case "Escape":
                                CleanUp();
                                Environment.Exit(0);
                                break;
                        }

                        //Write new line.
                        Utilities.WriteNewLine();

                        //set next selected option
                        menu.SetSelectedOption();
                    }
                }

                //Invalid login.
                else
                {
                    //Prompt user for next action.
                    menu.SetSelectedOption();
                }

                //Write new line.
                Utilities.WriteNewLine();
            }
        }

        /// <summary>
        /// Delete temp files.
        /// </summary>
        private static void CleanUp()
        {
            foreach (string file in Directory.GetFiles(Environment.CurrentDirectory, "*.mp3"))
            {
                File.Delete(file);
            }
        }
    }
}