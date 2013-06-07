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
            Console.WriteLine("Welcome to the PMSP Audio Player v1.0!");

            //Write new line.
            Utilities.WriteNewLine();
            
            //Run.
            while (1 == 1)
            {
                //Instantiate Menu object with valid options.
                var networkMenu = new Menu("Would you like us to search for a PMSP server on the local area network for you?  Enter (y) for yes, (n) for no, or ESC to exit the program:",
                                    new List<ConsoleKey> { ConsoleKey.Y, ConsoleKey.N, ConsoleKey.Escape });

                //Write new line.
                Utilities.WriteNewLine();

                //Instantiate Protocol.
                Protocol protocol = new Protocol();

                //Declare exception variable.
                bool encounteredException = false;

                //Run.
                while (1 == 1)
                {
                    //Perform specified action.
                    switch (networkMenu.SelectedOption.ToString())
                    {
                        //Auto-discover PMSP server.
                        case "Y":

                            //Instantiate Menu object with valid options.
                            var autoDiscoveryMenu = new Menu(String.Concat("PMSP auto-discovery assumes PMSP server(s) have IPv4 addresses, ICMP (ping) enabled, and firewalls configured to allow incoming traffic to port ",
                                protocol.Server.Port, ".  Do you wish to continue?  Enter (y) for yes, (n) for no, or ESC to exit the program:"),
                                new List<ConsoleKey> { ConsoleKey.Y, ConsoleKey.N, ConsoleKey.Escape });

                            //Run.
                            while (1 == 1)
                            {
                                //Perform specified action.
                                switch (autoDiscoveryMenu.SelectedOption.ToString())
                                {
                                    //Auto-discover PMSP server.
                                    case "Y":

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Set user credentials.
                                        protocol.SetCredentials();

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Inform user of progress.
                                        Console.WriteLine("PMSP auto-discovery is now in progress.  This may take a few minutes to complete.  Please wait...");

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Get list of active IPv4 addresses on LAN.
                                        List<string> ips = LAN.GetActiveIp4Addresses();

                                        //For each IP in list, attempt PMSP handshake until successful.
                                        foreach (string ip in ips)
                                        {
                                            //Set server ip.
                                            protocol.Server.HostNameOrIpAddress = ip;

                                            //Attempt handshake
                                            if (protocol.Authenticate())
                                            {
                                                break;
                                            }
                                        }

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Inform user of auto-discovery result.
                                        if (protocol.IsAuthenticated)
                                        {
                                            Console.WriteLine("Congratulations!  You have successfully connected to PMSP Server " + protocol.Server.Url + ".");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Unfortunately, we were unable to connect to a PMSP Server on your network.");
                                        }

                                        //Break.
                                        break;

                                    //Break out of switch.
                                    case "N":

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        break;

                                    //Exit program.
                                    case "Escape":
                                        CleanUp();
                                        Environment.Exit(0);
                                        break;
                                }

                                //Break.
                                break;
                            }

                            //Break;
                            break;

                        //Manually specify PMSP server.
                        case "N":

                            //Prompt user for PMSP URL.
                            Console.WriteLine("Please specify the host name (i.e. MyPMSPServer) or IP address (i.e. 192.168.1.4) of the PMSP Server to which you'd like to connect and press ENTER:");

                            //Set PMSP URL.
                            protocol.Server.Url = Console.ReadLine().Trim();

                            //Write new line.
                            Utilities.WriteNewLine();

                            //Set user credentials.
                            protocol.SetCredentials();

                            //Attempt handshake
                            if (protocol.Authenticate())
                            {
                                //Write new line.
                                Utilities.WriteNewLine();

                                Console.WriteLine("Congratulations!  You have successfully logged in to PMSP Server " + protocol.Server.Url + ".");
                            }
                            else
                            {
                                //Write new line.
                                Utilities.WriteNewLine();

                                //Inform user.
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

                    //If we're authenticated, proceed.
                    if (protocol.IsAuthenticated)
                    {
                        //Instantiate Menu object with valid options.
                        var mainMenu = new Menu("Would you like to browse artists, genres, or tracks?  Please enter (a) for artists, (g) for genres, (l) to logout or ESC to logout and exit the program:", new List<ConsoleKey> { ConsoleKey.A, ConsoleKey.G, ConsoleKey.T, ConsoleKey.L, ConsoleKey.Escape });

                        //Write new line.
                        Utilities.WriteNewLine();

                        //Run the program.
                        while (1 == 1)
                        {
                            //Initialize variables.
                            List<Artist> artists;
                            List<Genre> genres;
                            List<Track> tracks;
                            Track selectedTrack = null;

                            //Perform specified action.
                            switch (mainMenu.SelectedOption.ToString())
                            {
                                //List artists.
                                case "A":

                                    //Write new line.
                                    Utilities.WriteNewLine();

                                    //Get artists.
                                    artists = Artist.GetList(protocol);

                                    //If we're still authenticated and have artists, list them.
                                    if (protocol.IsAuthenticated && artists.Count > 0)
                                    {
                                        //List artists.
                                        Console.WriteLine("Here are the available artists:");
                                        int artistCount = 1;
                                        foreach (Artist artist in artists)
                                        {
                                            Console.WriteLine(artistCount.ToString() + "." + "  " + artist.Name);
                                            artistCount++;
                                        }

                                        //Run.
                                        while (1 == 1)
                                        {
                                            //Write new line.
                                            Utilities.WriteNewLine();

                                            //Get selected artist.
                                            Console.WriteLine("Please enter the number of the artist of whom you'd like to listen:");

                                            try
                                            {
                                                //Get artist from list.
                                                Artist selectedArtist = null;
                                                try
                                                {
                                                    //Set selected artist.
                                                    selectedArtist = artists[Convert.ToInt32(Console.ReadLine()) - 1];

                                                    //Ensure selection is valid before proceeding.
                                                    if (selectedArtist != null)
                                                    {
                                                        //Write new line.
                                                        Utilities.WriteNewLine();

                                                        //Get tracks.
                                                        selectedArtist.Tracks = Track.GetList(protocol, ListType.Artist, selectedArtist.Name);

                                                        //If we're still authenticated and have tracks, list them.
                                                        if (protocol.IsAuthenticated && selectedArtist.Tracks.Count > 0)
                                                        {
                                                            //List tracks.
                                                            Console.WriteLine("Here are the available tracks for " + selectedArtist.Name + ":");
                                                            int trackCount = 1;
                                                            foreach (Track track in selectedArtist.Tracks)
                                                            {
                                                                Console.WriteLine(trackCount.ToString() + "." + "  " + track.Title);
                                                                trackCount++;
                                                            }

                                                            //Write new line.
                                                            Utilities.WriteNewLine();

                                                            //Run.
                                                            while (1 == 1)
                                                            {
                                                                //Write new line.
                                                                Utilities.WriteNewLine();

                                                                //Get selected track.
                                                                Console.WriteLine("Please enter the number of the track of which you'd like to listen:");

                                                                //Get track from list.
                                                                try
                                                                {
                                                                    //Set selected track.
                                                                    selectedTrack = selectedArtist.Tracks[Convert.ToInt32(Console.ReadLine()) - 1];

                                                                    //Break;
                                                                    break;
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Console.WriteLine("The track you entered was invalid.  Please try again.");
                                                                }
                                                            }

                                                        }

                                                        //If we're not authenticated or don't have artists, inform user.
                                                        else
                                                        {
                                                            //We got here because of an exception.
                                                            encounteredException = true;

                                                            //Logout.
                                                            goto case "L";
                                                        }

                                                        //Break out if we have a selected track.
                                                        if (selectedTrack != null)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("The artist you entered was invalid.");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("The artist you entered was invalid.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("The artist you entered was invalid.");
                                            }
                                        }
                                    }

                                    //If we're not authenticated or don't have artists, inform user and log out.
                                    else
                                    {
                                        //We got here because of an exception.
                                        encounteredException = true;

                                        //Logout.
                                        goto case "L";
                                    }

                                    break;

                                //List genres.
                                case "G":

                                    //Write new line.
                                    Utilities.WriteNewLine();

                                    //Get genres.
                                    genres = Genre.GetList(protocol);

                                    //If we're still authenticated and have genres, list them.
                                    if (protocol.IsAuthenticated && genres.Count > 0)
                                    {
                                        //List genres.
                                        Console.WriteLine("Here are the available genres:");
                                        int genreCount = 1;
                                        foreach (Genre genre in genres)
                                        {
                                            Console.WriteLine(genreCount.ToString() + "." + "  " + genre.Name);
                                            genreCount++;
                                        }

                                        //Run.
                                        while (1 == 1)
                                        {
                                            //Write new line.
                                            Utilities.WriteNewLine();

                                            //Get selected genre.
                                            Console.WriteLine("Please enter the number of the genre of which you'd like to listen:");

                                            try
                                            {
                                                //Get genre from list.
                                                Genre selectedGenre = null;
                                                try
                                                {
                                                    //Set selected genre.
                                                    selectedGenre = genres[Convert.ToInt32(Console.ReadLine()) - 1];

                                                    //Ensure selection is valid before proceeding.
                                                    if (selectedGenre != null)
                                                    {
                                                        //Write new line.
                                                        Utilities.WriteNewLine();

                                                        //Get tracks.
                                                        selectedGenre.Tracks = Track.GetList(protocol, ListType.Genre, selectedGenre.Name);

                                                        //If we're still authenticated and have tracks, list them.
                                                        if (protocol.IsAuthenticated && selectedGenre.Tracks.Count > 0)
                                                        {

                                                            //List tracks.
                                                            Console.WriteLine("Here are the available tracks for the " + selectedGenre.Name + " genre:");
                                                            int trackCount = 1;
                                                            foreach (Track track in selectedGenre.Tracks)
                                                            {
                                                                Console.WriteLine(trackCount.ToString() + "." + "  " + track.Title + " by " + track.Artist.Name);
                                                                trackCount++;
                                                            }

                                                            //Write new line.
                                                            Utilities.WriteNewLine();

                                                            //Run.
                                                            while (1 == 1)
                                                            {
                                                                //Write new line.
                                                                Utilities.WriteNewLine();

                                                                //Get selected track.
                                                                Console.WriteLine("Please enter the number of the track of which you'd like to listen:");

                                                                //Get track from list.
                                                                try
                                                                {
                                                                    //Set selected track.
                                                                    selectedTrack = selectedGenre.Tracks[Convert.ToInt32(Console.ReadLine()) - 1];

                                                                    //Break;
                                                                    break;
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Console.WriteLine("The track you entered was invalid.  Please try again.");
                                                                }
                                                            }
                                                        }

                                                        //If we're not authenticated or don't have artists, inform user.
                                                        else
                                                        {
                                                            //We got here because of an exception.
                                                            encounteredException = true;

                                                            //Logout.
                                                            goto case "L";
                                                        }

                                                        //Break out if we have a selected track.
                                                        if (selectedTrack != null)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("The genre you entered was invalid.");
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine("The genre you entered was invalid.");
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine("The genre you entered was invalid.");
                                            }
                                        }
                                    }

                                    //If we're not authenticated or don't have genres, inform user and logout.
                                    else
                                    {
                                        //We got here because of an exception.
                                        encounteredException = true;

                                        //Logout.
                                        goto case "L";
                                    }

                                    break;

                                //Logout.
                                case "L":

                                    //Write new line.
                                    Utilities.WriteNewLine();

                                    //If we got here due to an exception.
                                    if (encounteredException)
                                    {
                                        Console.WriteLine("Unfortunately, the application has encountered the following exception: " + protocol.Exception);

                                        //Write new line.
                                        Utilities.WriteNewLine();
                                    }

                                    //Force logout
                                    protocol.Logout();

                                    //Write new line.
                                    Utilities.WriteNewLine();

                                    //Only inform user if our session hasn't expired.
                                    Console.WriteLine("You are now logged out of server " + protocol.Server.Url + ".");

                                    //Write new line.
                                    Utilities.WriteNewLine();

                                    break;

                                //Exit program.
                                case "Escape":
                                    protocol.Logout();
                                    CleanUp();
                                    Environment.Exit(0);
                                    break;
                            }

                            //If we're still authenticated, proceed.
                            if (protocol.IsAuthenticated)
                            {
                                //If we haven't encountered an error, proceed.
                                if (!encounteredException)
                                {
                                    //Write new line.
                                    Utilities.WriteNewLine();

                                    //Stream selected track.
                                    try
                                    {
                                        //Inform user of track retrieval.
                                        Console.WriteLine("Retrieving track, please wait...");

                                        //Write new line.
                                        Utilities.WriteNewLine();

                                        //Retrieve audio file.
                                        selectedTrack.GetAudioFile(protocol);

                                        //If we're still authenticated and have an audio file, continue.
                                        if (protocol.IsAuthenticated && selectedTrack.AudioFile != null)
                                        {
                                            //Stream track
                                            selectedTrack.Stream(protocol);

                                            //Put thread to sleep until the track is loaded.
                                            while (!selectedTrack.IsLoaded)
                                            {
                                                Thread.Sleep(100);
                                            }

                                            //If the track is playing, inform user.
                                            if (selectedTrack.Audio.PlaybackState == PlaybackState.Playing)
                                            {
                                                //Inform user of title and artist.
                                                Console.WriteLine("Now streaming " + selectedTrack.Title + " by " + selectedTrack.Artist.Name + "...");

                                                //Write new line.
                                                Utilities.WriteNewLine();

                                                //Instantiate Menu object with valid options.
                                                Menu playbackMenu = new Menu("Enter (s) to stop playback or ESC to log out and exit the program:", new List<ConsoleKey> { ConsoleKey.S, ConsoleKey.Escape });

                                                //Run.
                                                while (1 == 1)
                                                {
                                                    //Perform specified action.
                                                    switch (playbackMenu.SelectedOption.ToString())
                                                    {
                                                        //Stop track
                                                        case "S":

                                                            //Stop track
                                                            if (selectedTrack.Audio.PlaybackState == PlaybackState.Playing)
                                                            {
                                                                selectedTrack.Stop();
                                                            }
                                                            break;

                                                        //Exit program.
                                                        case "Escape":

                                                            //Stop track
                                                            if (selectedTrack.Audio.PlaybackState == PlaybackState.Playing)
                                                            {
                                                                selectedTrack.Stop();
                                                            }

                                                            CleanUp();
                                                            Environment.Exit(0);
                                                            break;
                                                    }

                                                    break;
                                                }
                                                //Write new line.
                                                Utilities.WriteNewLine();
                                            }
                                        }

                                        //We got here because of an error, logout.
                                        else
                                        {
                                            //Write new line.
                                            Utilities.WriteNewLine();

                                            //Inform user.
                                            Console.WriteLine("Unfortunately, the application has encountered the following exception: " + protocol.Exception);

                                            //Write new line.
                                            Utilities.WriteNewLine();

                                            //Force logout
                                            protocol.Logout();

                                            //Write new line.
                                            Utilities.WriteNewLine();

                                            //Inform user of logout state.
                                            Console.WriteLine("You are now logged out of server " + protocol.Server.Url + ".");

                                            //Write new line.
                                            Utilities.WriteNewLine();

                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //Inform user and append exception.
                                        Console.WriteLine("Unfortunately, we were unable to stream the track you selected due to the following reason:  " + selectedTrack.Exception);

                                        //Write new line.
                                        Utilities.WriteNewLine();
                                    }
                                }

                                //Write new line.
                                Utilities.WriteNewLine();

                                //set next selected option
                                mainMenu.SetSelectedOption();
                            }

                            //If we're logging out, break.
                            if (!protocol.IsAuthenticated)
                            {
                                break;
                            }
                        }
                    }

                    //Logout.
                    else
                    {
                        //Write new line.
                        Utilities.WriteNewLine();

                        //Logout.
                        protocol.Logout();

                        //Break.
                        break;
                    }

                    //If we're not authenticated here, break.
                    if (!protocol.IsAuthenticated)
                    {
                        break;
                    }
                }
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