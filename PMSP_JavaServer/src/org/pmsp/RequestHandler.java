package org.pmsp;

import java.io.PrintStream;

import org.pmsp.domain.BaseRequest;
import org.pmsp.domain.LoginRequest;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
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
			MessageParser mp = new MessageParser();
			BaseRequest br = mp.parse(request.getContent());
			String user = "default";
			if (br != null && br instanceof LoginRequest) {
				user = ((LoginRequest)br).getUsername();
			}
			// then need to figure out what to do with the request
			
			
			// then need to build the response, will most likely do that in another class
			body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			body.println("<Response>");
			body.println("Hello " + user);
			body.println("</Response>");

			body.close();
		} catch(Throwable e) {
			e.printStackTrace();
		}
	} 

}