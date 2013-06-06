/*=======================Directives and Pragmas=============================*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace PMSPClient
{
    /// <summary>
    /// This class is used for all interactions with the PMSP Server.
    /// </summary>
    public class Server
    {
        //Private fields.
        private string _hostNameOrIpAddress;
        private string _url;
        private const string _port = "31415";
        private string _protocolVersion;
        private HttpWebRequest _request;
        private XmlDocument _requestData;
        private XmlDocument _response = new XmlDocument();
        private string _exception;

        //Public properties.
        public string HostNameOrIpAddress { get { return _hostNameOrIpAddress; } set { _hostNameOrIpAddress = value; _url = "http://" + value + ":" + _port; } }
        public string Url { get { return _url; } set { _url = "http://" + value + ":" + _port; } }
        public string Port { get { return _port; } }
        public HttpWebRequest Request { get { return _request; } }
        public XmlDocument RequestData { get { return _requestData; } }
        public XmlDocument Response { get { return _response; } }
        public string Exception { get { return _exception; } }

        /// <summary>
        /// Main constructor.
        /// </summary>
        public Server(string protocolVersion)
        {
            //Set protocol version.
            _protocolVersion = protocolVersion;
        }

        /// <summary>
        /// Creates a new web request with default parameters.
        /// </summary>
        public void CreateRequest()
        {
            //Instantiate new web request.
            _request = (HttpWebRequest)WebRequest.Create(_url);

            //Specify PMSP Version and append to header.
            _request.Headers["PMSP-Version"] = _protocolVersion;

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
        /// Creates a new request with a cookie.
        /// </summary>
        /// <param name="sessionId"></param>
        public void CreateRequest(DfaState currentClientDfaState, string sessionId)
        {
            //Create request.
            CreateRequest();

            //Specify current dfa state & session id.
            Cookie sessionIdCookie = new Cookie(sessionId.Split('=')[0], sessionId.Split('=')[1]);
            Cookie stateCookie = new Cookie(Utilities.GetEnumDescriptionFromValue(currentClientDfaState).Split('=')[0], Utilities.GetEnumDescriptionFromValue(currentClientDfaState).Split('=')[1]);
            CookieCollection cc = new CookieCollection();
            cc.Add(sessionIdCookie);
            cc.Add(stateCookie);
            _request.CookieContainer = new CookieContainer();
            _request.CookieContainer.Add(new Uri(_url), sessionIdCookie);
            _request.CookieContainer.Add(new Uri(_url), stateCookie);
        }

        /// <summary>
        /// Gets the response XML for the supplied request.
        /// </summary>
        /// <returns></returns>
        public HttpWebResponse GetResponse()
        {
            //Return response.
            try
            {
                return (HttpWebResponse)_request.GetResponse();
            }
            catch (WebException exception)
            {
                HandleException(exception);
                return null;
            }
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

        /// <summary>
        /// Handles web & general exception and sets user-friendly message.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private void HandleException(WebException exception)
        {
            if (exception.Status == WebExceptionStatus.ProtocolError)
            {
                var response = exception.Response as HttpWebResponse;
                if (response != null)
                {
                    _exception = response.StatusDescription;
                }
                else
                {
                    _exception = exception.Message;
                }
            }
            else
            {
                _exception = exception.Message;
            }
        }
    }
}
