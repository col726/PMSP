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

            //Instantiate web request.
            var request = (HttpWebRequest)WebRequest.Create(_url);

            //Specify credentials and append to header.
            string credentials = userName + ":" + password;
            credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            request.Headers["Authorization"] = "Basic " + credentials;
            
            //Specify post data.
            //Instantiate xml document.
            var authenticationRequest = new XmlDocument();
            
            //Add the XML declaration section.
            XmlDeclaration declaration = authenticationRequest.CreateXmlDeclaration("1.0", null, null);
            declaration.Encoding = "UTF-8";

            // Add the new node to the document.
            XmlElement root = authenticationRequest.DocumentElement;
            authenticationRequest.InsertBefore(declaration, root);
            
            //Define operation.
            XmlElement operation = authenticationRequest.CreateElement("Operation");
            authenticationRequest.AppendChild(operation);
            
            //Define type.
            XmlElement type = authenticationRequest.CreateElement("type");
            type.SetAttribute("class", "ListRequest");
            operation.AppendChild(type);

            //Define category.
            XmlElement category = authenticationRequest.CreateElement("category");
            XmlText categoryText = authenticationRequest.CreateTextNode("Music");
            category.AppendChild(categoryText);
            type.AppendChild(category);

            //Define list type.
            XmlElement listType = authenticationRequest.CreateElement("listType");
            XmlText listTypeText = authenticationRequest.CreateTextNode("Track");
            listType.AppendChild(listTypeText);
            type.AppendChild(listType);

            //Define criteria.
            XmlElement criteria = authenticationRequest.CreateElement("criteria");
            type.AppendChild(criteria);

            //Define criteria 1st ListCriteria
            XmlElement listCriteria = authenticationRequest.CreateElement("ListCriteria");
            criteria.AppendChild(listCriteria);

            //Define list criteria type.
            XmlElement listCriteriaType = authenticationRequest.CreateElement("type");
            XmlText listCriteriaTypeText = authenticationRequest.CreateTextNode("Music");
            listCriteriaType.AppendChild(listCriteriaTypeText);
            listCriteria.AppendChild(listCriteriaType);

            //Define list criteria type.
            XmlElement listCriteriaName = authenticationRequest.CreateElement("name");
            XmlText listCriteriaNameText = authenticationRequest.CreateTextNode("Artist");
            listCriteriaName.AppendChild(listCriteriaNameText);
            listCriteria.AppendChild(listCriteriaName);

            //Define list criteria type.
            XmlElement listCriteriaValue = authenticationRequest.CreateElement("value");
            XmlText listCriteriaValueText = authenticationRequest.CreateTextNode("Smith");
            listCriteriaValue.AppendChild(listCriteriaValueText);
            listCriteria.AppendChild(listCriteriaValue);

            //Define criteria 2nd ListCriteria
            listCriteria = authenticationRequest.CreateElement("ListCriteria");
            criteria.AppendChild(listCriteria);

            //Define list criteria type.
            listCriteriaType = authenticationRequest.CreateElement("type");
            listCriteriaTypeText = authenticationRequest.CreateTextNode("Music");
            listCriteriaType.AppendChild(listCriteriaTypeText);
            listCriteria.AppendChild(listCriteriaType);

            //Define list criteria type.
            listCriteriaName = authenticationRequest.CreateElement("name");
            listCriteriaNameText = authenticationRequest.CreateTextNode("Artist");
            listCriteriaName.AppendChild(listCriteriaNameText);
            listCriteria.AppendChild(listCriteriaName);

            //Define list criteria type.
            listCriteriaValue = authenticationRequest.CreateElement("value");
            listCriteriaValueText = authenticationRequest.CreateTextNode("Green");
            listCriteriaValue.AppendChild(listCriteriaValueText);
            listCriteria.AppendChild(listCriteriaValue);

            //authenticationRequest.Save("test.xml");

            //Convert xml to byte stream for http post.
            string postData;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                authenticationRequest.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                postData = stringWriter.GetStringBuilder().ToString();
            }

            byte[] data = Encoding.UTF8.GetBytes(postData);

            //Specify request parameters.
            request.Method = "POST";
            request.ContentType = "application/xml";
            request.Accept = "application/xml";

            //Get http request stream.
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            //Get http response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //Read response into variable.
            string responseContent = new StreamReader(response.GetResponseStream()).ReadToEnd();

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