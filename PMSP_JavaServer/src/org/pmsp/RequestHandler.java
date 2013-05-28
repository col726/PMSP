package org.pmsp;

import java.io.PrintStream;

import org.pmsp.domain.ListRequest;
import org.pmsp.domain.Operation;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
import org.simpleframework.http.Status;
import org.simpleframework.http.core.Container;

public class RequestHandler implements Container {

	public void handle(Request request, Response response) {
		try {
			PrintStream body = response.getPrintStream();
			long time = System.currentTimeMillis();

			response.setValue("Content-Type", "text/xml");
			response.setDate("Date", time);
			response.setDate("Last-Modified", time);
			response.setValue("PMSP-Version", "1.0");
			

			System.out.println(request.getContent());
			
			Authenticator auth = new Authenticator();
			String user = auth.authenticate(request.getValue("Authorization"));
			
			MessageParser mp = new MessageParser();
			Operation op = mp.parse(request.getContent());

			
			// then need to build the response, will most likely do that in another class
			if (user == null) {
				response.setStatus(Status.UNAUTHORIZED);
			}
			else {
				ResponseBuilder rb = new ResponseBuilder();
				if (op.getType() instanceof ListRequest) {
					rb.list(request, response, op, user);
				}
			}
			
			body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			body.println("<Response>");			

			if (user != null) {
				body.println("Hello " + user);
			}
			else {
				body.println("Authentication failed");
				response.setStatus(Status.UNAUTHORIZED);
			}
			
			body.println("</Response>");

			body.close();
		} catch(Throwable e) {
			e.printStackTrace();
		}
	}
	
	

}