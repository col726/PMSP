package org.pmsp;

import java.util.concurrent.ConcurrentHashMap;

import org.apache.commons.codec.binary.Base64;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Authentication class.  Right now it assumes http basic auth is being used and does a simple look up
 * against a string in the prop file to find credentials.  This would obviously need to be changed for a 
 * robust application.
 *
 */
public class Authenticator {

	private static ConcurrentHashMap<String, String> userCredentials = new ConcurrentHashMap<String, String>();
	
	/**
	 * Default Constructor.  Loads valid credentials from property file into memory
	 */
	public Authenticator() {
		String users = MediaServer.props.getProperty(PMSP_Constants.USERS_KEY);
		
		//The "users" string is comma delimted pairs of username:password
		String[] credentials = users.split(",");
		for (String s : credentials) {
			userCredentials.put(s.substring(0, s.indexOf(":")), s.substring(s.indexOf(":")+1));
		}
	}
	
	/**
	 * Perform authentication against users list.  If authentication is successful, return the username 
	 * @param authString
	 * @return username or null if authentication failed
	 */
	public String authenticate(String authString) {
		String user = null;
		String password = null;

		//check for malformed auth string 
		if (authString != null && authString.indexOf("Basic ") >= -1) {
			//HTTP Basic auth says the username password will be a base64 representation of username:password
			String credentials = new String(Base64.decodeBase64(authString.replaceFirst("Basic ", "")));
			user = credentials.substring(0, credentials.indexOf(":"));
			password = credentials.substring(credentials.indexOf(":")+1);
		}
		
		//perform user lookup
		if (!(user != null && password != null && password.equals(userCredentials.get(user)))) {
			user = null;
		}
		
		return user;
	}
}
