using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PMSPClient
{
    /// <summary>
    /// This is the protocol class used for interacting with the PMSP Server.
    /// </summary>
    class Protocol
    {
        //Private fields.
        private bool _isConnected = false;
        private bool _isAuthenticated = false;
        private Exception _exception;

        //Public properties.
        public Exception Exception { get { return _exception; } }

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Protocol(){}

        /// <summary>
        /// Initial handshake with server.
        /// </summary>
        public bool Connect()
        {
            //Inform user of connection process.
            Console.WriteLine("Connecting to server, please wait...");

            /*
             Connect to server here.
             *
            */

            //Success
            _isConnected = true;
            Console.WriteLine("Connected successfully." + Environment.NewLine);
            return true;
        }

        /// <summary>
        /// Authenticate user.
        /// </summary>
        public bool Authenticate()
        {
            //Get user name.
            Console.WriteLine("Please enter your User Name: ");
            string userName = Console.ReadLine();

            //Get password.
            Console.WriteLine("Please enter your Password: ");
            Char temp;
            String password = "";
            do
            {
                temp = Console.ReadKey().KeyChar;
                if (temp == 13) break;
                password += temp.ToString();
                Console.Write((char)8);
                Console.Write("*");
            } while (true);

            //Inform user of authentication process.
            Console.WriteLine(Environment.NewLine + "Authenticating, please wait...");

            /*
             Authenticate here.
            */

            _isAuthenticated = true;
            Console.WriteLine("Authenticated successfully." + Environment.NewLine);
            return true;
        }

        /// <summary>
        /// Gets a list of artists or tracks based on the input parameter.
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public XmlDocument GetList(ListType listType)
        {
            //Get list of tracks or artists.
            switch(listType)
            {
                case ListType.Artist:
                    //get artists
                    break;
                case ListType.Track:
                    //get tracks
                    break;
            }

            //Return response xml from server.
            return new XmlDocument();
        }
    }
}