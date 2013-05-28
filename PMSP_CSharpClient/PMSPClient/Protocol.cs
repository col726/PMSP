/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private string _ip;
        private string _url;
        private const string _port = "31415";
        private bool _isConnected = false;
        private bool _isAuthenticated = false;

        //Public properties.
        //Set url when ip is set.
        public string Ip { get { return _ip; } set { _ip = value; _url = "http://" + value + ":" + _port; } }
        public string Url { get { return _url; } set { _url = value; } }
        public string Port { get { return _port; } }
        public bool IsConnected { get { return _isConnected; } }

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Protocol(){}

        /// <summary>
        /// Initial handshake with server.
        /// </summary>
        public bool Connect()
        {
            //Wrap handshake in try/catch to trap errors.
            try
            {
                //Instantiate http request.

                /**********TEST DATA ONLY******************/
                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://66.175.208.217:31415/");
                /**********TEST DATA ONLY******************/

                //Use this when we go live.
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_url);

                //Construct post data.
                ASCIIEncoding encoding = new ASCIIEncoding();
                string postData = "hello=hello";
                byte[] data = encoding.GetBytes(postData);

                //Specify request parameters.
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                //Get http request stream.
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                //Get http response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                //Read response into variable.
                string responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();

                //If response contains hello message, we're connected.
                if (responseContent.Contains("Hello"))
                {
                    _isConnected = true;
                }
            }
            catch {}

            //Return connection status.
            return _isConnected;
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

            //Authenticate
            if (userName.ToUpper() == "ADAM" && password == "himes")
            {
                _isAuthenticated = true;
            }

            //Return authentication status.
            return _isAuthenticated;
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