package org.pmsp;

import java.util.HashMap;

import org.apache.commons.codec.binary.Base64;

public class Authenticator {

	private static HashMap<String, String> userCredentials = new HashMap<String, String>();
	
	public Authenticator() {
		String users = MediaServer.props.getProperty(PMSP_Constants.USERS);
		String[] credentials = users.split(",");
		for (String s : credentials) {
			userCredentials.put(s.substring(0, s.indexOf(":")), s.substring(s.indexOf(":")+1));
		}
	}
	public String authenticate(String authString) {
		String user = null;
		String password = null;
		if (authString != null && authString.indexOf("Basic ") >= -1) {
			String credentials = new String(Base64.decodeBase64(authString.replaceFirst("Basic ", "")));
			user = credentials.substring(0, credentials.indexOf(":"));
			password = credentials.substring(credentials.indexOf(":")+1);
		}
		
		//perform user authentication
		if (!(user != null && password != null && password.equals(userCredentials.get(user)))) {
			user = null;
		}
		
		return user;
	}
}
