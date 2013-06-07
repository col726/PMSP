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

        /*******************BEGIN TEST FUZZ**********************/
        //Set invalid client PMSP Version.
        //private string _version = "1.2";
        /*******************END TEST FUZZ************************/

        private string _version = "1.0";
        private string _userName;
        private string _password;
        private const DfaState _initialDfaState = DfaState.AwaitingLogin;
        private DfaState _currentServerDfaState = _initialDfaState;
        private DfaState _currentClientDfaState = _initialDfaState;
        private DateTime _currentClientDfaStateExpiration;
        private string _sessionId = "pmsp-sessionid=";
        private DateTime _sessionIdExpiration;
        private bool _isAuthenticated = false;
        private string _exception;
        private bool _isSessionExpired = false;

        //Public properties.
        public Server Server { get { return _server; } }
        public bool IsAuthenticated { get { return _isAuthenticated; } }
        public string Exception { get { return _exception; } }
        public bool IsSessionExpired { get { return _isSessionExpired; } }

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
            //Ensure client & server are in the initial DFA state.
            if (_currentServerDfaState == _initialDfaState && _currentClientDfaState == _initialDfaState)
            {
                /*******************BEGIN TEST FUZZ**********************/
                //Set invalid client DFA state.
                //_currentClientDfaState = DfaState.AwaitingListChoice;
                /*******************END TEST FUZZ************************/

                //Inform user of authentication process.
                Utilities.WriteNewLine();
                Console.Write("Now attempting authentication on server " + _server.Url + "...");

                //Determine whether or not we're authenticated.
                try
                {
                    //Create request.
                    _server.CreateRequest(_currentClientDfaState, _sessionId, _userName, _password);

                    //Define operation.
                    XmlElement operation = _server.RequestData.CreateElement("Operation");
                    _server.RequestData.AppendChild(operation);

                    //Define type.
                    XmlElement type = _server.RequestData.CreateElement("type");
                    operation.AppendChild(type);

                    //Set attributes.
                    type.SetAttribute("class", "LoginRequest");

                    //Get http response.
                    HttpWebResponse response = _server.GetResponse(_server.RequestData);

                    //If the response is null, there was an error.  Set exception message.
                    if (response == null)
                    {
                        _exception = _server.Exception;
                    }

                    //Otherwise, set cookie value for subsequent requests.
                    else
                    {
                        //Ensure server is in correct DFA state before proceeding.
                        try
                        {
                            //Set server DFA state from cookie.
                            _currentServerDfaState = Utilities.GetEnumValueFromDescription<DfaState>(Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "pmsp-state", 1));

                            //Set client DFA state to Idle.
                            _currentClientDfaState = _currentServerDfaState;

                            /*******************BEGIN TEST FUZZ**********************/
                            //Set invalid server DFA state.
                            //_currentServerDfaState = DfaState.AwaitingListChoice;
                            /*******************END TEST FUZZ************************/

                            //Ensure server is now in Idle state.
                            if (_currentServerDfaState == DfaState.Idle)
                            {
                                //Set session id.
                                _sessionId = Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "pmsp-sessionid", 1);

                                //Set client session id expiration.
                                _sessionIdExpiration = DateTime.Parse(Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "GMT", 1));

                                //Set client DFA state expiration.
                                _currentClientDfaStateExpiration = DateTime.Parse(Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "GMT", 2));

                                //Set boolean.
                                _isAuthenticated = true;

                                //Inform user of success.
                                Console.Write("Success!");
                            }

                            //If not, set error message.
                            else
                            {
                                _exception = "The server was in an unexpected state.";
                            }
                        }
                        catch (Exception exception)
                        {
                            _exception = exception.Message;
                        }
                    }
                }
                catch (Exception exception)
                {
                    _exception = exception.Message;
                }
            }

            //If the client and server are not in their expected initial state, set error message.
            else
            {
                _exception = "Both the client and server must be in the " + _initialDfaState + " state in order to attempt an authentication request.";
            }

            //If not authenticated, write failed.
            if (!_isAuthenticated)
            {
                Console.Write("Failed.");
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
            //Ensure client & server are in the Idle DFA state.
            if (_currentServerDfaState == DfaState.Idle && _currentClientDfaState == DfaState.Idle)
            {
                /*******************BEGIN TEST FUZZ**********************/
                //Set invalid client DFA state.
                //_currentClientDfaState = DfaState.AwaitingLogin;
                /*******************END TEST FUZZ************************/

                //Only proceed if neither of our cookies have expired.
                if (_currentClientDfaStateExpiration >= DateTime.Now && _sessionIdExpiration >= DateTime.Now)
                {
                    //Create new request with current DFA state & session id.
                    _server.CreateRequest(_currentClientDfaState, _sessionId);

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

                            /*******************BEGIN TEST FUZZ**********************/
                            //Set invalid request.
                            //type.SetAttribute("class", "InvalidRequest");
                            /*******************END TEST FUZZ************************/

                            //Set attributes.
                            type.SetAttribute("class", "MetadataListRequest");
                            type.SetAttribute("category", "Music");
                            type.SetAttribute("listType", "Artist");

                            //Break.
                            break;

                        case ListType.Genre:

                            //Set attributes.
                            type.SetAttribute("class", "MetadataListRequest");
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
                        //Ensure server is in correct DFA state before proceeding.
                        try
                        {
                            //Set server DFA state from cookie.
                            _currentServerDfaState = Utilities.GetEnumValueFromDescription<DfaState>(Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "pmsp-state", 1));

                            /*******************BEGIN TEST FUZZ**********************/
                            //Set invalid server DFA state.
                            //_currentServerDfaState = DfaState.AwaitingLogin;
                            /*******************END TEST FUZZ************************/

                            //Set client DFA state to AwaitingListChoice.
                            _currentClientDfaState = DfaState.AwaitingListChoice;

                            //Ensure server is now in AwaitingListChoice state.
                            if (_currentServerDfaState == DfaState.AwaitingListChoice)
                            {
                                //Load xml from response.
                                _server.Response.LoadXml(new StreamReader(response.GetResponseStream()).ReadToEnd());
                            }
                            //If not, set error message.
                            else
                            {
                                _exception = "The server was in an unexpected state.";
                            }
                        }
                        catch (Exception exception)
                        {
                            _exception = exception.Message;
                        }
                    }
                }

                //If we have an expired cookie, set error.
                else
                {
                    _isSessionExpired = true;
                    _exception = "Your session has expired.";
                }
            }

            //If the client and server are not in their expected initial state, set error message.
            else
            {
                _exception = "Both the client and server must be in the " + DfaState.Idle + " state in order to attempt a metadata list request.";
                return null;
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
            //Ensure client & server are in the AwaitingListChoice DFA state.
            if (_currentServerDfaState == DfaState.AwaitingListChoice && _currentClientDfaState == DfaState.AwaitingListChoice)
            {
                /*******************BEGIN TEST FUZZ**********************/
                //Set invalid client DFA state.
                //_currentClientDfaState = DfaState.AwaitingLogin;
                /*******************END TEST FUZZ************************/

                //Only proceed if neither of our cookies have expired.
                if (_currentClientDfaStateExpiration >= DateTime.Now && _sessionIdExpiration >= DateTime.Now)
                {
                    //Create new request.
                    _server.CreateRequest(_currentClientDfaState, _sessionId);

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
                    type.SetAttribute("class", "FileListRequest");
                    type.SetAttribute("category", "Music");

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
                        //Ensure server is in correct DFA state before proceeding.
                        try
                        {
                            //Set server DFA state from cookie.
                            _currentServerDfaState = Utilities.GetEnumValueFromDescription<DfaState>(Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "pmsp-state", 1));

                            /*******************BEGIN TEST FUZZ**********************/
                            //Set invalid server DFA state.
                            //_currentServerDfaState = DfaState.AwaitingLogin;
                            /*******************END TEST FUZZ************************/

                            //Set client DFA state to AwaitingFileChoice.
                            _currentClientDfaState = DfaState.AwaitingFileChoice;

                            //Ensure server is now in AwaitingFileChoice state.
                            if (_currentServerDfaState == DfaState.AwaitingFileChoice)
                            {
                                //Load xml from response.
                                _server.Response.LoadXml(new StreamReader(response.GetResponseStream()).ReadToEnd());
                            }
                            //If not, set error message.
                            else
                            {
                                _exception = "The server was in an unexpected state.";
                            }
                        }
                        catch (Exception exception)
                        {
                            _exception = exception.Message;
                        }
                    }
                }

                //If we have an expired cookie, set error.
                else
                {
                    _isSessionExpired = true;
                    _exception = "Your session has expired.";
                }
            }

            //If the client and server are not in their expected initial state, set error message.
            else
            {
                _exception = "Both the client and server must be in the " + DfaState.AwaitingListChoice + " state in order to attempt a media file list request.";
                return null;
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
            //Ensure client & server are in the AwaitingFileChoice DFA state.
            if (_currentServerDfaState == DfaState.AwaitingFileChoice && _currentClientDfaState == DfaState.AwaitingFileChoice)
            {
                /*******************BEGIN TEST FUZZ**********************/
                //Set invalid client DFA state.
                //_currentClientDfaState = DfaState.AwaitingLogin;
                /*******************END TEST FUZZ************************/

                //Only proceed if neither of our cookies have expired.
                if (_currentClientDfaStateExpiration >= DateTime.Now && _sessionIdExpiration >= DateTime.Now)
                {

                    //Create new request.
                    _server.CreateRequest(_currentClientDfaState, _sessionId);

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
                        //Ensure server is in correct DFA state before proceeding.
                        try
                        {
                            //Set server DFA state from cookie.
                            _currentServerDfaState = Utilities.GetEnumValueFromDescription<DfaState>(Utilities.GetCookieFromHeader(response.Headers["Set-Cookie"], "pmsp-state", 1));

                            /*******************BEGIN TEST FUZZ**********************/
                            //Set invalid server DFA state.
                            //_currentServerDfaState = DfaState.AwaitingLogin;
                            /*******************END TEST FUZZ************************/

                            //Set client DFA state to Idle.
                            _currentClientDfaState = DfaState.Idle;

                            //Ensure server is now in Idle state.
                            if (_currentServerDfaState == DfaState.Idle)
                            {
                                //Load xml from response.
                                _server.Response.LoadXml(new StreamReader(response.GetResponseStream()).ReadToEnd());
                            }
                            //If not, set error message.
                            else
                            {
                                _exception = "The server was in an unexpected state.";
                            }
                        }
                        catch (Exception exception)
                        {
                            _exception = exception.Message;
                        }
                    }
                }

                //If we have an expired cookie, set error.
                else
                {
                    _isSessionExpired = true;
                    _exception = "Your session has expired.";
                }
            }

            //If the client and server are not in their expected initial state, set error message.
            else
            {
                _exception = "Both the client and server must be in the " + DfaState.AwaitingFileChoice + " state in order to attempt a media file list request.";
                return null;
            }

            //Return file.
            return _server.Response;
        }

        /// <summary>
        /// Logs user out of server.
        /// </summary>
        public void Logout()
        {
            //Only inform user if we're logged in and our session hasn't expired.
            if (_isAuthenticated && !_isSessionExpired)
            {
                //Inform user of logout process.
                Utilities.WriteNewLine();
                Console.WriteLine("You will now be logged out of server " + _server.Url + ".");
                Utilities.WriteNewLine();
                Console.Write("Now logging out of server " + _server.Url + "...");
            }

            //Send logout request.
            try
            {
                //If we're logged in, log out.
                if (_isAuthenticated)
                {
                    //Create request.
                    _server.CreateRequest(_currentClientDfaState, _sessionId);

                    //Define operation.
                    XmlElement operation = _server.RequestData.CreateElement("Operation");
                    _server.RequestData.AppendChild(operation);

                    //Define type.
                    XmlElement type = _server.RequestData.CreateElement("type");
                    operation.AppendChild(type);

                    //Set attributes.
                    type.SetAttribute("class", "LogoffRequest");

                    //Get http response.
                    HttpWebResponse response = _server.GetResponse(_server.RequestData);

                    //If the response is null, there was an error.  Set exception message.
                    if (response == null)
                    {
                        _exception = _server.Exception;
                    }
                }
            }

            catch (Exception exception)
            {
                _exception = exception.Message;
            }

            //Always assume we're logged out after making a logout request.
            finally
            {
                //Only inform user if we're logged in and our session hasn't expired.
                if (_isAuthenticated && !_isSessionExpired)
                {
                    //Always a success.
                    Console.Write("Success!");
                }

                //Set server & client DFA state to AwaitingLogin.
                _currentClientDfaState = DfaState.AwaitingLogin;
                _currentServerDfaState = DfaState.AwaitingLogin;

                //Set booleans.
                _isAuthenticated = false;
                _isSessionExpired = false;
            }
        }
    }
}