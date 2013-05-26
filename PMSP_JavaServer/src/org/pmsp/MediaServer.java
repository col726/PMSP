package org.pmsp;

import static org.pmsp.PMSP_Constants.IMPLEMENTATION;
import static org.pmsp.PMSP_Constants.LISTEN_PORT;
import static org.pmsp.PMSP_Constants.PROPERTIES_FILE;

import java.io.FileInputStream;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.util.Properties;

import org.simpleframework.http.core.Container;
import org.simpleframework.http.core.ContainerServer;
import org.simpleframework.transport.Server;
import org.simpleframework.transport.connect.Connection;
import org.simpleframework.transport.connect.SocketConnection;

public class MediaServer {

	public static Properties props = new Properties();
	
	public static void main(String[] list) throws Exception {
		String propFile = System.getProperty(PROPERTIES_FILE, PROPERTIES_FILE);
		FileInputStream fis = null;
		try {
			fis = new FileInputStream(propFile);
			props.load(fis);
		}
		finally {
			fis.close();	
		}

		
		
		Container container = (Container) Class.forName(props.getProperty(IMPLEMENTATION)).newInstance();
		Server server = new ContainerServer(container);
		Connection connection = new SocketConnection(server);
		
		SocketAddress address = new InetSocketAddress(Integer.parseInt(props.getProperty(LISTEN_PORT)));

		connection.connect(address);
		
	}
}