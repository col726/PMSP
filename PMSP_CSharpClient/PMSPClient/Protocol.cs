/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Web;

namespace PMSPClient
{
    /// <summary>
    /// This is the protocol class used for interacting with the PMSP Server.
    /// </summary>
    class Protocol
    {
        //Private fields.
        private Server _server;
        private string _version = "1.0";
        private string _userName;
        private string _password;
        private string _sessionId;
        private bool _isAuthenticated = false;
        private string _exception;

        //Public properties.
        public Server Server { get { return _server; } }
        public bool IsAuthenticated { get { return _isAuthenticated; } }
        public string Exception { get { return _exception; } }

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Protocol()
        {
            //Instantiate server with protocol version.
            _server = new Server(_version);
        }

        /// <summary>
        /// Sets the username and password of the current user.
        /// </summary>
        public void SetCredentials()
        {
            //Get user name.
            Console.WriteLine("Please enter your User Name: ");
            _userName = Console.ReadLine();

            //Get password.
            Console.WriteLine("Please enter your Password: ");
            Char temp;
            _password = "";
            do
            {
                temp = Console.ReadKey().KeyChar;
                if (temp == 13) break;
                _password += temp.ToString();
                Console.Write((char)8);
                Console.Write("*");
            } while (true);
        }

        /// <summary>
        /// STATEFUL: Authenticate user.
        /// </summary>
        public bool Authenticate()
        {
            //Inform user of authentication process.
            Utilities.WriteNewLine();
            Console.Write("Now attempting authentication on server " + _server.Url + "...");

            //Determine whether or not we're authenticated.
            try
            {
                //Create request.
                _server.CreateRequest(_userName, _password);

                //Get http response.
                HttpWebResponse response = _server.GetResponse();

                //If the response is null, there was an error.  Set exception message.
                if (response == null)
                {
                    Console.Write("Failed.");
                    _exception = _server.Exception;
                }

                //Otherwise, set cookie value for subsequent requests.
                else
                {
                    //Inform user.
                    Console.Write("Success!");

                    //Set cookie
                    _sessionId = response.Headers["Set-Cookie"];

                    //Set boolean.
                    _isAuthenticated = true;
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
        /// STATEFUL: Gets a list of artists or tracks based on the input parameter.
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public XmlDocument GetMetadataList(ListType xmllistType)
        {
            //Create new request.
            _server.CreateRequest(_sessionId);

            //Define operation.
            XmlElement operation = _server.RequestData.CreateElement("Operation");
            _server.RequestData.AppendChild(operation);

            //Define type.
            XmlElement type = _server.RequestData.CreateElement("type");
            operation.AppendChild(type);

            //Define criteria.
            XmlElement criteria = _server.RequestData.CreateElement("criteria");
            type.AppendChild(criteria);

            //Get list of tracks or artists.
            switch (xmllistType)
            {
                case ListType.Artist:

                    //Set attributes.
                    type.SetAttribute("class", "ListRequest");
                    type.SetAttribute("category", "Music");
                    type.SetAttribute("listType", "Artist");

                    //Break.
                    break;

                case ListType.Genre:

                    //Set attributes.
                    type.SetAttribute("class", "ListRequest");
                    type.SetAttribute("category", "Music");
                    type.SetAttribute("listType", "Genre");

                    //Break.
                    break;
            }

            //Get response.
            HttpWebResponse response = _server.GetResponse(_server.RequestData);

            //If the response is null, there was an error.  Set exception message.
            if (response == null)
            {
                _exception = _server.Exception;
            }

            //Otherwise, load xml from response.
            else
            {
                //Load xml from response.
                _server.Response.LoadXml(new StreamReader(response.GetResponseStream()).ReadToEnd());
            }

            //Return list.
            return _server.Response;
        }

        /// <summary>
        /// STATEFUL: Gets a list of tracks based on the input parameter.
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public XmlDocument GetMediaFileList(ListType criteriaType, string criteriaValue)
        {
            //Create new request.
            _server.CreateRequest(_sessionId);

            //Define operation.
            XmlElement operation = _server.RequestData.CreateElement("Operation");
            _server.RequestData.AppendChild(operation);

            //Define type.
            XmlElement type = _server.RequestData.CreateElement("type");
            operation.AppendChild(type);

            //Define criteria.
            XmlElement criteria = _server.RequestData.CreateElement("criteria");
            type.AppendChild(criteria);

            //Set attributes.
            type.SetAttribute("class", "ListRequest");
            type.SetAttribute("category", "Music");
            type.SetAttribute("listType", "Track");

            //If we have list criteria, specify it.
            if (!String.IsNullOrEmpty(criteriaValue))
            {
                XmlElement listCriteria = _server.RequestData.CreateElement("ListCriteria");
                listCriteria.SetAttribute("name", criteriaType.ToString());
                listCriteria.SetAttribute("value", criteriaValue);
                criteria.AppendChild(listCriteria);
            }

            //Get response.
            HttpWebResponse response = _server.GetResponse(_server.RequestData);

            //If the response is null, there was an error.  Set exception message.
            if (response == null)
            {
                _exception = _server.Exception;
            }

            //Otherwise, load xml from response.
            else
            {
                //Load xml from response.
                _server.Response.LoadXml(new StreamReader(response.GetResponseStream()).ReadToEnd());
            }

            //Return list.
            return _server.Response;
        }

        /// <summary>
        /// STATEFUL: Retrieves the specified file ID from the server.
        /// </summary>
        /// <param name="fileId">The file ID to retrieve.</param>
        /// <returns></returns>
        public XmlDocument GetFile(string fileId)
        {
            //Create new request.
            _server.CreateRequest(_sessionId);

            //Define retrieval.
            XmlElement operation = _server.RequestData.CreateElement("Operation");
            _server.RequestData.AppendChild(operation);

            //Define type.
            XmlElement type = _server.RequestData.CreateElement("type");
            type.SetAttribute("class", "RetrievalRequest");
            type.SetAttribute("mediaType", "Music");
            operation.AppendChild(type);

            //Define file ID.
            XmlElement id = _server.RequestData.CreateElement("id");
            XmlText idText = _server.RequestData.CreateTextNode(fileId);
            id.AppendChild(idText);
            type.AppendChild(id);

            //Get response.
            HttpWebResponse response = _server.GetResponse(_server.RequestData);

            //If the response is null, there was an error.  Set exception message.
            if (response == null)
            {
                _exception = _server.Exception;
            }

            //Otherwise, load xml from response.
            else
            {
                //Load xml from response.
                _server.Response.LoadXml(new StreamReader(response.GetResponseStream()).ReadToEnd());
            }

            //Return file.
            return _server.Response;
        }
    }
}