package org.pmsp;

public class PMSP_Constants {

public static final String PROPERTIES_FILE_KEY = "pmsp.properties";
public static final String IMPLEMENTATION_KEY = "implementation.class";
public static final String LISTEN_HOST_KEY = "listen.host";
public static final String LISTEN_PORT_KEY = "listen.port";
public static final String USERS_KEY = "users";
public static final String SUPPORTED_VERSIONS_KEY = "supported.versions";
public static final String LOGGING_CONFIG_KEY = "logging.config";
public static final String DATA_DIR_KEY = "data.directory";

public static final String COOKIE_SESSION_KEY =  "pmsp-sessionid";
public static final String COOKIE_STATE_KEY =  "pmsp-state";


public static final String HEADER_VERSION_STRING = "PMSP-Version";
public static final String DB_DRIVER_CLASS = "org.apache.derby.jdbc.EmbeddedDriver";

public static final String STATE_WAIT_FOR_LOGIN = "wait.login";
public static final String STATE_IDLE = "idle";
public static final String STATE_WAIT_FOR_LIST_CHOICE = "wait.list.coice";
public static final String STATE_WAIT_FOR_FILE_CHOICE = "wait.file.choice";

public static final String MESSAGE_LOGIN = "";
public static final String MESSAGE_LOGOFF = "";
public static final String MESSAGE_GET_METADATA = "";
public static final String MESSAGE_GET_OPTIONS = "";
public static final String MESSAGE_GET_FILE = "";


}
