package org.pmsp;

import static org.pmsp.PMSP_Constants.DATA_DIR_KEY;
import static org.pmsp.PMSP_Constants.IMPLEMENTATION_KEY;
import static org.pmsp.PMSP_Constants.LISTEN_HOST_KEY;
import static org.pmsp.PMSP_Constants.LISTEN_PORT_KEY;
import static org.pmsp.PMSP_Constants.PROPERTIES_FILE_KEY;

import java.io.FileInputStream;
import java.net.InetSocketAddress;
import java.net.SocketAddress;
import java.sql.DatabaseMetaData;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Properties;

import org.apache.log4j.Logger;
import org.apache.log4j.PropertyConfigurator;
import org.pmsp.domain.AudioFile;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.ListCriteria;
import org.pmsp.domain.Listing;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.LogoffRequest;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.MediaFileListing;
import org.pmsp.domain.MediaMetadataListing;
import org.pmsp.domain.MetadataListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.Retrieval;
import org.pmsp.domain.RetrievalRequest;
import org.simpleframework.http.core.Container;
import org.simpleframework.http.core.ContainerServer;
import org.simpleframework.transport.Server;
import org.simpleframework.transport.connect.Connection;
import org.simpleframework.transport.connect.SocketConnection;

import com.thoughtworks.xstream.XStream;

public class MediaServer {

	public static Properties props = new Properties();
	private static final Logger logger = Logger.getLogger(MediaServer.class);
	private static XStream parser = new XStream();
	protected static Server server;
	protected static Connection connection; 
	
	
	static {
		Class[] classes = new Class[] {Operation.class, ListCriteria.class, FileListRequest.class, RetrievalRequest.class, 
				AudioFile.class, MediaFile.class, Retrieval.class, Listing.class, MediaMetadataListing.class, 
				MediaFileListing.class, MetadataListRequest.class, LoginRequest.class, LogoffRequest.class};
		parser.processAnnotations(classes);
	}
	
	private static void initDatabase() throws ClassNotFoundException, SQLException {
		String driver = PMSP_Constants.DB_DRIVER_CLASS;
		
	    Class.forName(driver);
		java.sql.Connection conn = null;
		Statement s = null;
		ResultSet rs = null;
		try {
			conn = getDbConnection();
			s = conn.createStatement();
			DatabaseMetaData dbmd = conn.getMetaData();
			rs = dbmd.getTables(null, null, "MUSIC", null);
			if(!rs.next()) {
				s.execute("CREATE TABLE MUSIC(" +
						"ID INTEGER NOT NULL GENERATED ALWAYS AS IDENTITY (START WITH 1, INCREMENT BY 1)," +
						"GENRE VARCHAR(20) NOT NULL," +
						"ARTIST VARCHAR(50) NOT NULL," +
						"ALBUM VARCHAR(50) NOT NULL," +
						"TITLE VARCHAR(50) NOT NULL," +
						"FILE_NAME VARCHAR(2000) NOT NULL)");
				logger.debug("MUSIC table created successfully");
			}

		}
		finally {
			try {
				rs.close();
			}
			catch (Throwable t){}
			try {
				s.close();
			}
			catch (Throwable t){}
			try {
				conn.close();
			}
			catch (Throwable t){}
		}
	}
	
	public static java.sql.Connection getDbConnection() throws SQLException {
		String dbName = props.getProperty(DATA_DIR_KEY, ".") + System.getProperty("file.separator") + "pmsp-db";
		
		String connectionURL = "jdbc:derby:" + dbName + ";create=true";
	    return(DriverManager.getConnection(connectionURL));
	}	
	
	public static XStream getXmlParser() {
		return parser;
	}
	
	private static class ShutdownHook {
		public void attachShutDownHook() {
			Runtime.getRuntime().addShutdownHook(new Thread() {
				public void run() {
					//close the socket connection
					if (connection != null) {
						try {
							connection.close();
						} catch (Throwable t) {
						}
					}

					//shut down the http server
					if (server != null) {
						try {
							server.stop();
						} catch (Throwable t) {
						}
					}
					
					//shut down the db server
					try {
						DriverManager.getConnection("jdbc:derby:;shutdown=true");
					} catch (Throwable t) {
					}
				}
			});
		}
	}
	
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
			System.err.println("Error loading properties file, shutting down...");
			System.exit(1);
		}
		
		try {
			initDatabase();
			
		}
		catch (Exception e) {
			System.err.println("Error initializing database, shutting down...");
			e.printStackTrace();
			System.exit(2);
		}
		
		ShutdownHook hook = new ShutdownHook();
		hook.attachShutDownHook();
		
		Container container = (Container) Class.forName(props.getProperty(IMPLEMENTATION_KEY)).newInstance();
		server = new ContainerServer(container);
		connection = new SocketConnection(server);
		String host = props.getProperty(LISTEN_HOST_KEY, "0.0.0.0");
		int port = Integer.parseInt(props.getProperty(LISTEN_PORT_KEY, "31415"));
		SocketAddress address = new InetSocketAddress(host, port);

		connection.connect(address);
		
		
	}
}