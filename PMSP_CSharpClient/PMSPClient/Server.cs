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
    /// This class is used for all interactions with the PMSP Server.
    /// </summary>
    public class Server
    {
        //Private fields.
        private string _ip;
        private string _url;
        private const string _port = "31415";
        private HttpWebRequest _request;
        private XmlDocument _requestData;
        private XmlDocument _response = new XmlDocument();

        //Public properties.
        public string Ip { get { return _ip; } set { _ip = value; _url = "http://" + value + ":" + _port; } }
        public string Url { get { return _url; } set { _url = value; } }
        public string Port { get { return _port; } }
        public HttpWebRequest Request { get { return _request; } }
        public XmlDocument RequestData { get { return _requestData; } }
        public XmlDocument Response { get { return _response; } }

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Server(){}

        /// <summary>
        /// Creates a new web request with default parameters.
        /// </summary>
        public void CreateRequest()
        {
            //Instantiate new web request.
            _request = (HttpWebRequest)WebRequest.Create(_url);

            //Specify PMSP Version and append to header.
            _request.Headers["PMSP-Version"] = "1.1";

            //Specify request parameters.
            _request.Method = "POST";
            _request.ContentType = "application/xml";
            _request.Accept = "application/xml";

            //New data request.
            _requestData = new XmlDocument();

            //Add the XML declaration section to the request.
            XmlDeclaration declaration = _requestData.CreateXmlDeclaration("1.0", null, null);
            declaration.Encoding = "UTF-8";

            // Add the root node to the request.
            XmlElement root = _requestData.DocumentElement;
            _requestData.InsertBefore(declaration, root);
        }

        /// <summary>
        /// Creates a new web request with credentials.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void CreateRequest(string userName, string password)
        {
            //Create request.
            CreateRequest();

            //Specify credentials
            string credentials = userName + ":" + password;
            credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            _request.Headers["Authorization"] = "Basic " + credentials;
        }

        /// <summary>
        /// Creates a new request with a session id cookie.
        /// </summary>
        /// <param name="sessionId"></param>
        public void CreateRequest(string sessionId)
        {
            //Create request.
            CreateRequest();

            //Specify session id.
            _request.Headers["Cookie"] = sessionId;
        }

        /// <summary>
        /// Gets the response XML for the supplied request.
        /// </summary>
        /// <returns></returns>
        public HttpWebResponse GetResponse()
        {
            //Return response.
            return (HttpWebResponse)_request.GetResponse();
        }

        /// <summary>
        /// Overloaded method for getting the response XML for the supplied request containing post data.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public HttpWebResponse GetResponse(XmlDocument xml)
        {
            //Convert xml to byte stream for http post.
            string postData;
            using (var stringWriter = new StringWriter())
            using (var xmlTextWriter = XmlWriter.Create(stringWriter))
            {
                xml.WriteTo(xmlTextWriter);
                xmlTextWriter.Flush();
                postData = stringWriter.GetStringBuilder().ToString();
            }

            //Encode post data to byte stream.
            byte[] data = Encoding.UTF8.GetBytes(postData);

            //Get http request stream.
            using (Stream stream = _request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            //Return response
            return GetResponse();
        }
    }
}
