package org.pmsp;

import static org.pmsp.PMSP_Constants.IMPLEMENTATION_KEY;
import static org.pmsp.PMSP_Constants.LISTEN_PORT_KEY;
import static org.pmsp.PMSP_Constants.PROPERTIES_FILE_KEY;

import java.io.FileInputStream;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.util.Properties;

import org.apache.log4j.PropertyConfigurator;
import org.simpleframework.http.core.Container;
import org.simpleframework.http.core.ContainerServer;
import org.simpleframework.transport.Server;
import org.simpleframework.transport.connect.Connection;
import org.simpleframework.transport.connect.SocketConnection;

public class MediaServer {

	public static Properties props = new Properties();
	
	public static void main(String[] list) throws Exception {
		
		String propFile = System.getProperty(PROPERTIES_FILE_KEY, PROPERTIES_FILE_KEY);
		
		//configure the logger, default to "logger.properties" if no log config file is specified
		PropertyConfigurator.configureAndWatch(props.getProperty(PROPERTIES_FILE_KEY,"logger.properties"));
		FileInputStream fis = null;
		try {
			fis = new FileInputStream(propFile);
			props.load(fis);
		}
		finally {
			fis.close();	
		}
		if (props == null) {
			System.err.println("Error loading properties file, shutting down..");
			System.exit(1);
		}
		
		
		Container container = (Container) Class.forName(props.getProperty(IMPLEMENTATION_KEY)).newInstance();
		Server server = new ContainerServer(container);
		Connection connection = new SocketConnection(server);
		
		SocketAddress address = new InetSocketAddress(Integer.parseInt(props.getProperty(LISTEN_PORT_KEY)));

		connection.connect(address);
		
	}
}