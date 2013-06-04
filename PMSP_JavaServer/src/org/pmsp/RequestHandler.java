package org.pmsp;

import static org.pmsp.PMSP_Constants.COOKIE_STATE_KEY;
import static org.pmsp.PMSP_Constants.STATE_IDLE;
import static org.pmsp.PMSP_Constants.*;
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

public class RequestHandler implements Container {

	private static final Logger logger = Logger.getLogger(RequestHandler.class);
	private static final ConcurrentHashMap<String, String> sessions = new ConcurrentHashMap<String, String>();
	private static final DfaValidator dfa = new DfaValidator();
	
	public void handle(Request request, Response response) {
		try {
			logger.debug(request.getHeader());
			logger.debug(request.getContent());
			
			long time = System.currentTimeMillis();

			//set some generic headers
			response.setValue("Content-Type", "text/xml");
			response.setDate("Date", time);
			
			String versionString = request.getValue(PMSP_Constants.HEADER_VERSION_STRING);
			if (versionString == null || !versionString.trim().matches("^\\d+\\.\\d+$")) {
				response.setStatus(Status.BAD_REQUEST);
				response.setDescription("Missing or invalid header " + PMSP_Constants.HEADER_VERSION_STRING);
				return;
			}
			else if (Arrays.binarySearch(MediaServer.props.getProperty(SUPPORTED_VERSIONS_KEY).split(","), versionString) < 0) {
				response.setStatus(Status.NOT_IMPLEMENTED);
				response.setDescription("Server does not support PMSP version " + versionString);
				return;
			}
			else {
				response.setValue("PMSP-Version", versionString);
			}
			
			//parse the message
			MessageParser mp = new MessageParser();
			Operation op = mp.parse(request.getContent());
			
			
			Cookie sessionCookie = request.getCookie(PMSP_Constants.COOKIE_SESSION_KEY);
			Cookie stateCookie = request.getCookie(PMSP_Constants.COOKIE_STATE_KEY);
			
			
			if ((sessionCookie != null && stateCookie == null) || (sessionCookie == null && stateCookie != null)) {
				response.setStatus(Status.BAD_REQUEST);
				response.setDescription("Invalid pmsp cookie pair");
				return;
			}

			String state = stateCookie == null ? STATE_WAIT_FOR_LOGIN : stateCookie.getValue();
			
			if (!dfa.checkTransition(state, op.getType())) {
				response.setStatus(Status.BAD_REQUEST);
				response.setDescription("Requested state transition from " + state + " not valid");
				return;
			}
			
			String user = null;
			RequestType requestType = op.getType();
			ResponseBuilder rb = new ResponseBuilder();
			
			if (op.getType() instanceof LoginRequest) {
				user = rb.login(request, response, op);
				if (user != null) {
					String sessionId = user;
					sessions.put(sessionId, user);
				}
			}
			else {
				user = sessions.get(sessionCookie.getValue());
			}
			
			//credentials invalid
			if (user == null) {
				response.setStatus(Status.UNAUTHORIZED);
			}
			//build response
			else if(sessionCookie != null) {

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
					sessions.remove(sessionCookie.getValue());
				}
			}			


			
		} catch(Throwable t) {
			
			try {
				response.setStatus(Status.INTERNAL_SERVER_ERROR);
				logger.error(request.getContent(), t);
			} catch (IOException e1) {}
		}
		finally {
			logger.debug("Response header: " + response.getHeader());
			try {
				response.getPrintStream().close();
			} catch (IOException e) {}
		}
	}
	
	
	

}