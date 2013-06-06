package org.pmsp;

import static org.pmsp.PMSP_Constants.STATE_WAIT_FOR_LOGIN;
import static org.pmsp.PMSP_Constants.SUPPORTED_VERSIONS_KEY;

import java.io.IOException;
import java.util.Arrays;
import java.util.concurrent.ConcurrentHashMap;

import org.apache.log4j.Logger;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.LogoffRequest;
import org.pmsp.domain.MetadataListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.RequestType;
import org.pmsp.domain.RetrievalRequest;
import org.simpleframework.http.Cookie;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
import org.simpleframework.http.Status;
import org.simpleframework.http.core.Container;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * This is the core of the protocol handler.  Implements one of the Simple framework's interfaces as the underlying 
 * http parsing is handled by the framework. This class controls the flow of the request/response handling and leverages
 * other classes to actually do the work of parsing/checking/building/etc 
 */
public class RequestHandler implements Container {

	private static final Logger logger = Logger.getLogger(RequestHandler.class);
	
	/**
	 * session map, maps session id to user name.  Represents logged in users
	 */
	private static final ConcurrentHashMap<String, String> sessions = new ConcurrentHashMap<String, String>();
	
	private final DfaValidator dfa = new DfaValidator();
	
	/**
	 * This method is called for each message from the client.
	 */
	public void handle(Request request, Response response) {
		try {
			logger.debug(request.getHeader());
			logger.debug(request.getContent());
			
			long time = System.currentTimeMillis();

			//set some generic headers
			response.setValue("Content-Type", "text/xml");
			response.setDate("Date", time);
			
			//check to make sure the version header exists and is a valid format, if not send an http 400
			//version field should be one more more integers, a period, then one more integers
			String versionString = request.getValue(PMSP_Constants.HEADER_VERSION_STRING);
			if (versionString == null || !versionString.trim().matches("^\\d+\\.\\d+$")) {
				response.setStatus(Status.BAD_REQUEST);
				response.setDescription("Missing or invalid header: " + PMSP_Constants.HEADER_VERSION_STRING);
				return;
			}
			//check to see if the version being requested by the client is supported.  If not, send an http 501 
			else if (Arrays.binarySearch(MediaServer.props.getProperty(SUPPORTED_VERSIONS_KEY).split(","), versionString) < 0) {
				response.setStatus(Status.NOT_IMPLEMENTED);
				response.setDescription("Server does not support PMSP version " + versionString);
				return;
			}
			//if the version # is acceptable, set that version # in our response
			else {
				response.setValue("PMSP-Version", versionString);
			}
			
			//parse the message
			MessageParser mp = new MessageParser();
			Operation op = mp.parse(request.getContent());
			
			//get the two cookies we use
			Cookie sessionCookie = request.getCookie(PMSP_Constants.COOKIE_SESSION_KEY);
			Cookie stateCookie = request.getCookie(PMSP_Constants.COOKIE_STATE_KEY);
			
			//the client should either have both cookies (logged in) or neither cookie (not logged in)
			if ((sessionCookie != null && stateCookie == null) || (sessionCookie == null && stateCookie != null)) {
				response.setStatus(Status.BAD_REQUEST);
				response.setDescription("Invalid pmsp cookie pair.  Clear pmsp cookies and try again?");
				return;
			}

			String state = stateCookie == null ? STATE_WAIT_FOR_LOGIN : stateCookie.getValue();
			
			//STATEFUL this is the call to the DFA enforcement logic, we pass in the current state and the type of request
			if (!dfa.checkTransition(state, op.getType())) {
				response.setStatus(Status.BAD_REQUEST);
				response.setDescription("Requested state transition from " + state + " not valid.");
				return;
			}
			
			//If all checks have passed thus far, then build the response
			String user = null;
			RequestType requestType = op.getType();
			ResponseBuilder rb = new ResponseBuilder();
			
			//login request handled differently than the requests for which you must be logged in
			if (op.getType() instanceof LoginRequest) {
				user = rb.login(request, response, op);
				//if the login was a success then store that session id and user in the sessions map
				if (user != null) {
					String sessionId = user;
					sessions.put(sessionId, user);
				}
			}
			//an already authenticated user
			else {
				user = sessions.get(sessionCookie.getValue());
			}
			
			//credentials invalid
			if (user == null) {
				response.setStatus(Status.UNAUTHORIZED);
				//tell the user how to log in
				response.setValue("WWW-Authenticate", "Basic realm=\"PMSP Server\"");
			}
			//build response for the request type
			else if(sessionCookie != null) {
				//update session id cookie to time out after 20 minutes
				sessionCookie = new Cookie(PMSP_Constants.COOKIE_SESSION_KEY, user);
				sessionCookie.setExpiry(1200);
				
				if (requestType instanceof MetadataListRequest){
					rb.listMetadata(request, response, op, user);
				}
				else if (requestType instanceof FileListRequest) {
					rb.listFiles(request, response, op, user);
					
				}
				else if (requestType instanceof RetrievalRequest) {
					rb.retrieval(request, response, op, user);	
				}
				else if (requestType instanceof LogoffRequest) {
					rb.logoff(request, response, op, user);
					//also clear session from sessions map
					sessions.remove(sessionCookie.getValue());
				}
			}


			
		} catch(Throwable t) {
			//generic "something bad happened on the server" error
			try {
				response.setStatus(Status.INTERNAL_SERVER_ERROR);
				logger.error(request.getContent(), t);
			} catch (IOException e1) {}
		}
		finally {
			logger.debug("Response header: " + response.getHeader());
			//always make sure output stream is closed. This wont hurt if close() had already been called
			try {
				response.getPrintStream().close();
			} catch (IOException e) {}
		}
	}
}