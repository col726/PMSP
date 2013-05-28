/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using NAudio.Wave;
using System.Security.Cryptography;
using TagLib;
using System.Threading;

namespace PMSPClient
{
    /// <summary>
    /// This is the Track class used for representing mp3 audio files as objects.
    /// </summary>
    class Track
    {
        //Private fields.
        private byte[] _mp3;
        private string _mp3TempFileName;
        private string _title;
        private Artist _artist;
        private WaveOut _audio;

        //Public properties.
        public string Title { get { return _title; } }
        public Artist Artist { get { return _artist; } }
        public WaveOut Audio { get { return _audio; } }

        //Main constructor.
        public Track(string mp3)
        {
            //Set fields.
            _mp3 = Convert.FromBase64String(mp3);

            //Populate metadata.
            PopulateMetadata();
        }

        public void PopulateMetadata()
        {
            //Write mp3 temp file.
            _mp3TempFileName = String.Format(@"{0}.mp3", Guid.NewGuid());
            System.IO.File.WriteAllBytes(_mp3TempFileName, _mp3);

            //Get metadata from ID3 tags.
            TagLib.File mp3 = TagLib.File.Create(_mp3TempFileName);
            _artist = new Artist(mp3.Tag.Performers[0]);
            _title = mp3.Tag.Title;

            //Delete temp file.
            System.IO.File.Delete(_mp3TempFileName);
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
            tracks.Add(new Track(System.IO.File.ReadAllText(@"C:\Users\Owner\Downloads\data.txt")));
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
            //Write mp3 temp file.
            _mp3TempFileName = String.Format(@"{0}.mp3", Guid.NewGuid());
            System.IO.File.WriteAllBytes(_mp3TempFileName, _mp3);

            //Spin play off in a new thread so we can interact with it from the console (i.e. Pause/Stop/Resume).
            Thread play = new Thread(Play);
            play.Start();
        }

        /// <summary>
        /// Plays track through NAudio library.
        /// </summary>
        public void Play()
        {
            //Instantiate memory stream for track.
            using (Stream memoryStream = new MemoryStream())
            {
                //Read mp3 file into memory.
                using (Stream stream = System.IO.File.OpenRead(_mp3TempFileName))
                {
                    byte[] buffer = new byte[32768];
                    int read;
                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        memoryStream.Write(buffer, 0, read);
                    }
                }

                //Use NAudio library to play track.
                memoryStream.Position = 0;
                using (WaveStream waveStream = new BlockAlignReductionStream(WaveFormatConversionStream.CreatePcmStream(new Mp3FileReader(memoryStream))))
                {
                    using (_audio = new WaveOut(WaveCallbackInfo.FunctionCallback()))
                    {
                        _audio.Init(waveStream);
                        _audio.Play();
                        while (_audio.PlaybackState == PlaybackState.Playing)
                        {
                            Thread.Sleep(100);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stops playback of a track in the play state.
        /// </summary>
        public void Stop()
        {
            _audio.Stop();
        }
    }
}