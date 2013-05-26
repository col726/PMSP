/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using NAudio.Wave;

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
        private string _url;

        //Public properties.
        public string Title { get { return _title; } }
        public string Artist { get { return _title; } }

        //Main constructor.
        public Track(string title, Artist artist, string url)
        {
            _title = title;
            _artist = artist;
            _url = url;
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
            tracks.Add(new Track("Intolerance", new Artist("Tool", ""), @"C:\Users\Owner\Music\Tool\Undertow\01 Intolerance.mp3"));
            tracks.Add(new Track("Lateralus", new Artist("Tool", ""), @"C:\Users\Owner\Music\Tool\Lateralus\09 Lateralus.mp3"));
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
            //Inform user of current streaming track.
            Console.WriteLine("Now streaming " + this._title + " by " + this._artist.DisplayName + "..." + Environment.NewLine);

            //Instantiate memory stream for track.
            using (Stream memoryStream = new MemoryStream())
            {
                //Get stream from url.
                using (Stream stream = WebRequest.Create(this._url).GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                    }
                }

                //Use NAudio library to stream track.
                memoryStream.Position = 0;
                using (WaveStream blockAlignedStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(memoryStream))))
                {
                    using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        waveOut.Init(blockAlignedStream);
                        waveOut.Play();
                        while (waveOut.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }
            }
        }
    }
}
