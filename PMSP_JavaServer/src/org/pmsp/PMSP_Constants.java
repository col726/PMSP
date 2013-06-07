package org.pmsp;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/


/**
 * Constants class. 
 */
public class PMSP_Constants {

	//config settings names
	public static final String PROPERTIES_FILE_KEY = "pmsp.properties";
	public static final String IMPLEMENTATION_KEY = "implementation.class";
	public static final String LISTEN_HOST_KEY = "listen.host";
	public static final String LISTEN_PORT_KEY = "listen.port";
	public static final String USERS_KEY = "users";
	public static final String SUPPORTED_VERSIONS_KEY = "supported.versions";
	public static final String LOGGING_CONFIG_KEY = "logging.config";
	public static final String DATA_DIR_KEY = "data.directory";
	public static final String SESSION_TIMEOUT_DURATION_KEY = "session.timeout";
	
	//cookie names
	public static final String COOKIE_SESSION_KEY =  "pmsp-sessionid";
	public static final String COOKIE_STATE_KEY =  "pmsp-state";
	
	//header field names
	public static final String HEADER_VERSION_STRING = "PMSP-Version";
	
	//state names
	public static final String STATE_WAIT_FOR_LOGIN = "wait.login";
	public static final String STATE_IDLE = "idle";
	public static final String STATE_WAIT_FOR_LIST_CHOICE = "wait.list.choice";
	public static final String STATE_WAIT_FOR_FILE_CHOICE = "wait.file.choice";
	
	//Other constants
	public static final String DB_DRIVER_CLASS = "org.apache.derby.jdbc.EmbeddedDriver";
	public static final int DFA_VIOLATION_RETURN_STATUS= 442;

}
