package org.pmsp;

import static org.pmsp.PMSP_Constants.SUPPORTED_VERSIONS_KEY;

import java.io.IOException;
import java.util.Arrays;
import java.util.concurrent.ConcurrentHashMap;

import org.apache.log4j.Logger;
import org.pmsp.domain.ListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.RetrievalRequest;
import org.simpleframework.http.Cookie;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
import org.simpleframework.http.Status;
import org.simpleframework.http.core.Container;

public class RequestHandler implements Container {

	private static final Logger logger = Logger.getLogger(RequestHandler.class);
	private static final ConcurrentHashMap<String, String> sessions = new ConcurrentHashMap<String, String>();
	
	public void handle(Request request, Response response) {
		try {
			logger.debug(request.getHeader());
			logger.debug(request.getContent());
			
			long time = System.currentTimeMillis();

			//set some generic headers
			response.setValue("Content-Type", "text/xml");
			response.setDate("Date", time);
			
			String versionString = request.getValue(PMSP_Constants.HEADER_VERSION_STRING);
			if (versionString == null || !versionString.matches("\\d\\.\\d")) {
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
			
			
			
			Cookie sessionid = request.getCookie(PMSP_Constants.COOKIE_SESSION_KEY);
			
			String user = null;
			if (sessionid == null) {
				//do authentication
				Authenticator auth = new Authenticator();
				user = auth.authenticate(request.getValue("Authorization"));
				if (user != null) {
					sessions.put(user, user);
					response.setCookie(PMSP_Constants.COOKIE_SESSION_KEY, user);
				}
			}
			else {
				user = sessions.get(sessionid.getValue());
			}
			
			//credentials invalid
			if (user == null) {
				response.setStatus(Status.UNAUTHORIZED);
			}
			//build response
			else if(sessionid != null) {
				//parse the message
				MessageParser mp = new MessageParser();
				Operation op = mp.parse(request.getContent());

				ResponseBuilder rb = new ResponseBuilder();
				if (op.getType() instanceof ListRequest) {
					rb.list(request, response, op, user);
				}
				else if (op.getType() instanceof RetrievalRequest) {
					rb.retrieval(request, response, op, user);
				}
			}
			
		} catch(Throwable e) {
			
			try {
				response.setStatus(Status.INTERNAL_SERVER_ERROR);
				logger.error(request.getContent(), e);
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