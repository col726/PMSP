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
        private string _exception;
        private string _sessionId;

        //Public properties.
        //Set url when ip is set.
        public string Ip { get { return _ip; } set { _ip = value; _url = "http://" + value + ":" + _port; } }
        public string Url { get { return _url; } set { _url = value; } }
        public string Port { get { return _port; } }
        public bool IsConnected { get { return _isConnected; } }
        public bool IsAuthenticated { get { return _isAuthenticated; } }
        public string Exception { get { return _exception; } }

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Protocol(){}

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
            Utilities.WriteNewLine();
            Console.WriteLine("Authenticating, please wait...");
            Utilities.WriteNewLine();

            //Instantiate web request.
            try
            {
                //Instantiate new web request.
                var request = (HttpWebRequest)WebRequest.Create(_url);

                //Specify credentials and append to header.
                string credentials = userName + ":" + password;
                credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                request.Headers["Authorization"] = "Basic " + credentials;

                //Specify PMSP Version and append to header.
                request.Headers["PMSP-Version"] = "1.1";

                //Determine whether or not we're authenticated.
                try
                {
                    //Get http response.
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //Verify PMSP Version.
                    if (response.Headers["PMSP-Version"] == "1.1")
                    {
                        //Set cookie
                        _sessionId = response.Headers["Set-Cookie"];
                        _isAuthenticated = true;
                    }

                    //Invalid PMSP Version.
                    else
                    {
                        _exception = "Invalid PMSP Version.";
                    }
                }
                catch (WebException exception)
                {
                    _exception = exception.Message;
                }
            }
            catch (Exception exception)
            {
                _exception = exception.Message;
            }

            //Return authentication status.
            return _isAuthenticated;
        }

        /// <summary>
        /// Gets a list of artists or tracks based on the input parameter.
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public XmlDocument GetList(ListType xmllistType)
        {
            XmlDocument responseDoc = new XmlDocument();

            //Get list of tracks or artists.
            switch (xmllistType)
            {
                case ListType.Artist:
                    //get artists
                    break;
                case ListType.Track:
                    //get tracks

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

                    responseDoc.LoadXml(this.GetResponse(authenticationRequest));

                    break;
            }

            //Return response xml from server.
            return responseDoc;
        }

        /// <summary>
        /// Retrieves the specified file ID from the server.
        /// </summary>
        /// <param name="fileId">The file ID to retrieve.</param>
        /// <returns></returns>
        public XmlDocument RetrieveFile(string fileId)
        {
            //Specify post data.
            //Instantiate xml document.
            var authenticationRequest = new XmlDocument();

            //Add the XML declaration section.
            XmlDeclaration declaration = authenticationRequest.CreateXmlDeclaration("1.0", null, null);
            declaration.Encoding = "UTF-8";

            // Add the new node to the document.
            XmlElement root = authenticationRequest.DocumentElement;
            authenticationRequest.InsertBefore(declaration, root);

            //Define retrieval.
            XmlElement operation = authenticationRequest.CreateElement("Operation");
            authenticationRequest.AppendChild(operation);

            //Define type.
            XmlElement type = authenticationRequest.CreateElement("type");
            type.SetAttribute("class", "RetrievalRequest");
            operation.AppendChild(type);

            //Define PMSP ID parent.
            XmlElement pmspIds = authenticationRequest.CreateElement("pmspIds");
            type.AppendChild(pmspIds);

            //Define file ID.
            XmlElement id = authenticationRequest.CreateElement("id");
            XmlText idText = authenticationRequest.CreateTextNode(fileId);
            id.AppendChild(idText);
            pmspIds.AppendChild(id);

            XmlDocument responseDoc = new XmlDocument();
            responseDoc.LoadXml(this.GetResponse(authenticationRequest));
            return responseDoc;
        }

        /// <summary>
        /// Creates a base HttpWebRequest object.
        /// </summary>
        /// <returns></returns>
        private HttpWebRequest CreateRequest()
        {
            //Instantiate new web request.
            var request = (HttpWebRequest)WebRequest.Create(_url);

            //Specify session id and add to request cookie container.
            request.Headers["Cookie"] = _sessionId;

            //Specify PMSP Version and append to header.
            request.Headers["PMSP-Version"] = "1.1";

            return request;
        }

        /// <summary>
        /// Gets the response XML for the supplied request XML.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        private string GetResponse(XmlDocument xml)
        {
            var request = this.CreateRequest();

            //Convert xml to byte stream for http post.
            string postData;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xml.WriteTo(xmlTextWriter);
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

            //Get response content.
            return new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }
}