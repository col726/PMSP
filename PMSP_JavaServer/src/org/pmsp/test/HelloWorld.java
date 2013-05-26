package org.pmsp.test;

import java.io.PrintStream;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.util.Scanner;

import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
import org.simpleframework.http.core.Container;
import org.simpleframework.http.core.ContainerServer;
import org.simpleframework.transport.Server;
import org.simpleframework.transport.connect.Connection;
import org.simpleframework.transport.connect.SocketConnection;

public class HelloWorld implements Container {

   public void handle(Request request, Response response) {
      try {
         PrintStream body = response.getPrintStream();
         long time = System.currentTimeMillis();
   
         
         response.setValue("Content-Type", "text/plain");
         response.setDate("Date", time);
         response.setDate("Last-Modified", time);
         response.setValue("PMSP-Version", "1.0");
   
//         String inputStreamString = new Scanner(request.getInputStream(),"UTF-8").nextLine();
         System.out.println(request.getContent());
         
         body.println("<Response>");
         body.println("Hello " + request.getContent());
         body.println("</Response>");
         
         body.close();
      } catch(Exception e) {
         e.printStackTrace();
      }
   } 

   public static void main(String[] list) throws Exception {
      Container container = new HelloWorld();
      Server server = new ContainerServer(container);
      Connection connection = new SocketConnection(server);
      SocketAddress address = new InetSocketAddress(31415);

      connection.connect(address);
   }
}