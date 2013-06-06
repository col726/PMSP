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
        private string _id;
        private byte[] _mp3;
        private string _mp3TempFileName;
        private string _title;
        private Artist _artist;
        private string _album;
        private string _genre;
        private WaveOut _audio;
        private string _exception;
        private bool _isLoaded;

        //Public properties.
        public string Id { get { return _id; } }
        public string Title { get { return _title; } }
        public Artist Artist { get { return _artist; } }
        public WaveOut Audio { get { return _audio; } }
        public string Exception { get { return _exception; } }
        public bool IsLoaded { get { return _isLoaded; } set { _isLoaded = value; } }

        //Main constructor.
        public Track(string id, Artist artist, string title, string album, string genre)
        {
            //Set fields.
            _id = id;
            _artist = artist;
            _title = title;
            _album = album;
            _genre = genre;

            //Populate metadata.
            //PopulateMetadata();
        }

        /// <summary>
        /// Populates track metadata from ID3 tags.
        /// </summary>
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
        public static List<Track> GetList(Protocol protocol, ListType criteriaType, string criteriaValue)
        {
            //Instantiate track list.
            List<Track> tracks = new List<Track>();

            //If we have tracks, insert them.
            try
            {
                //Drill down through child nodes to get track listings.
                XmlNode trackList = protocol.GetMediaFileList(criteriaType, criteriaValue).SelectSingleNode("//MediaFileListing").ChildNodes[0];

                //Insert tracks.
                foreach (XmlNode track in trackList.ChildNodes)
                {
                    tracks.Add(new Track(track.Attributes["pmspId"].Value,
                                            new Artist(track.Attributes["artist"].Value),
                                            track.Attributes["title"].Value,
                                            track.Attributes["album"].Value,
                                            track.Attributes["genre"].Value));
                }
            }
            catch (Exception ex) { }

            //Return list.
            return tracks;
        }

        /// <summary>
        /// Streams a track using the PMSP protocol.
        /// </summary>
        /// <param name="protocol"></param>
        public void Stream(Protocol protocol)
        {
            //Get audio file.
            XmlNode audioFile = protocol.GetFile(_id).SelectSingleNode("//Retrieval/mediaFiles/AudioFile");

            //If the audio file is valid per checksum comparison, stream track.
            if (IsValid(audioFile))
            {
                //Set mp3 byte array.
                _mp3 = Convert.FromBase64String(audioFile.SelectSingleNode("data/text()").Value);

                //Write mp3 temp file.
                _mp3TempFileName = String.Format(@"{0}.mp3", Guid.NewGuid());
                System.IO.File.WriteAllBytes(_mp3TempFileName, _mp3);

                //Spin play off in a new thread so we can interact with it from the console (i.e. Pause/Stop/Resume).
                Thread play = new Thread(Play);
                play.Start();
            }

            //Otherwise, set error.
            else
            {
                _exception = "The file is corrupted.  Please select another track.";
            }
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
                            _isLoaded = true;
                            Thread.Sleep(100);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Compares the checksums of the server & client to ensure we have a complete file.
        /// </summary>
        /// <param name="audioFile"></param>
        /// <returns></returns>
        private bool IsValid(XmlNode audioFile)
        {
            //Get server-provided checksum.
            string serverCheckSum = audioFile.Attributes["checksum"].Value;

            //Set mp3 string.
            string mp3 = audioFile.SelectSingleNode("//Retrieval/mediaFiles/AudioFile/data/text()").Value;

            //Generate checksum on mp3 string.
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes(mp3);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string clientCheckSum = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
            
            //If client checksum matches server checksum, return true.
            if (serverCheckSum.ToLower() == clientCheckSum.ToLower())
            {
                return true;
            }

            //Otherwise, return false.
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Stops playback of a track in the play state.
        /// </summary>
        public void Stop()
        {
            _audio.Stop();
        }

        /// <summary>
        /// Pauses playback of a track in the play state.
        /// </summary>
        public void Pause()
        {
            _audio.Pause();
        }

        /// <summary>
        /// Resumes playback of a track in the paused state.
        /// </summary>
        public void Resume()
        {
            _audio.Resume();
        }
    }
}