package org.pmsp;

import java.io.PrintStream;

import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
import org.simpleframework.http.core.Container;

public class RequestHandler implements Container {

	public void handle(Request request, Response response) {
		try {
			PrintStream body = response.getPrintStream();
			long time = System.currentTimeMillis();


			response.setValue("Content-Type", "text/plain");
			response.setDate("Date", time);
			response.setDate("Last-Modified", time);
			response.setValue("PMSP-Version", "1.0");

			System.out.println(request.getContent());

			body.println("<Response>");
			body.println("Hello " + request.getContent());
			body.println("</Response>");

			body.close();
		} catch(Exception e) {
			e.printStackTrace();
		}
	} 

}