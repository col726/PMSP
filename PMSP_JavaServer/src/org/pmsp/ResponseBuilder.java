package org.pmsp;

import static org.pmsp.PMSP_Constants.COOKIE_STATE_KEY;
import static org.pmsp.PMSP_Constants.STATE_IDLE;
import static org.pmsp.PMSP_Constants.STATE_WAIT_FOR_FILE_CHOICE;
import static org.pmsp.PMSP_Constants.STATE_WAIT_FOR_LIST_CHOICE;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintStream;
import java.security.MessageDigest;
import java.sql.SQLException;
import java.util.List;

import org.apache.commons.codec.binary.Base64;
import org.apache.commons.codec.digest.DigestUtils;
import org.apache.derby.iapi.services.io.FileUtil;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.MediaFileListing;
import org.pmsp.domain.MediaMetadataListing;
import org.pmsp.domain.MetadataListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.Retrieval;
import org.pmsp.domain.RetrievalRequest;
import org.simpleframework.http.Cookie;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;
import org.simpleframework.http.Status;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * This class assembles the response message for a given request message 
 */
public class ResponseBuilder {

	/**
	 * Build response message for the file list request
	 * @param request
	 * @param response
	 * @param operation
	 * @param user
	 * @throws IOException
	 * @throws SQLException
	 */
	public void listFiles(Request request, Response response, Operation operation, String user) throws IOException, SQLException {
		PrintStream body = response.getPrintStream();
		response.setCookie(COOKIE_STATE_KEY, STATE_WAIT_FOR_FILE_CHOICE);
		
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		MediaFileListing mfl = new MediaFileListing();
		
		//create dao object and call the appropriate query method
		MediaDao dao = new MediaDao();
		mfl.setMediaFiles(dao.findFiles((FileListRequest)operation.getType()));
		
		//call xstream object to convert our object to xml, write that in http body
		body.println(MediaServer.getXmlParser().toXML(mfl));			

		body.close();
	}
	
	/**
	 * Build response for the metadata list message
	 * @param request
	 * @param response
	 * @param operation
	 * @param user
	 * @throws IOException
	 * @throws SQLException
	 */
	public void listMetadata(Request request, Response response, Operation operation, String user) throws IOException, 
	SQLException {
		PrintStream body = response.getPrintStream();
		response.setCookie(COOKIE_STATE_KEY, STATE_WAIT_FOR_LIST_CHOICE);
		
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		MediaMetadataListing mml = new MediaMetadataListing();
		
		//create dao object and call the appropriate query method
		MediaDao dao = new MediaDao();
		mml.setMetadata(dao.findMetadata((MetadataListRequest)operation.getType()));
		
		//call xstream object to convert our object to xml, write that in http body
		body.println(MediaServer.getXmlParser().toXML(mml));		

		body.close();
	}
	
	/**
	 * 
	 * @param request
	 * @param response
	 * @param operation
	 * @param user
	 * @throws IOException
	 * @throws Exception
	 */
	public void retrieval(Request request, Response response, Operation operation, String user) throws IOException, Exception {
		PrintStream body = response.getPrintStream();
		response.setCookie(COOKIE_STATE_KEY, PMSP_Constants.STATE_IDLE);
		
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		Retrieval r = new Retrieval();
		RetrievalRequest retrieveRequest = (RetrievalRequest)operation.getType();
		
		//create dao object and call the appropriate query method
		List<? extends MediaFile> files = new MediaDao().findFiles(retrieveRequest.getPmspIds(), retrieveRequest.getMediaType());

		//iterate over all the files, do base64 encoding and checksumming
		for (MediaFile f : files) {
			String s = encodeBase64(f.getFullFilePath());
			f.setData(s);
			f.setChecksum(DigestUtils.sha1Hex(s));
		}
		r.setMediaFiles(files);
		
		//call xstream object to convert our object to xml, write that in http body
		body.println(MediaServer.getXmlParser().toXML(r));	

		body.close();
	}
	
	/**
	 * Build response for the login message
	 * @param request
	 * @param response
	 * @param operation
	 * @return
	 * @throws IOException
	 */
	public String login(Request request, Response response, Operation operation) throws IOException {

		//do authentication
		Authenticator auth = new Authenticator();
		String user = auth.authenticate(request.getValue("Authorization"));
		if (user != null) {
			//body of message is empty so use a 204
			response.setStatus(Status.NO_CONTENT);
			//set session id cookie to time out after 20 minutes
			Cookie sessionCookie = new Cookie(PMSP_Constants.COOKIE_SESSION_KEY, user);
			sessionCookie.setExpiry(1200);
			response.setCookie(sessionCookie);
			response.setCookie(COOKIE_STATE_KEY, STATE_IDLE);
			
			response.close();
		}	
		return user;
	}
	
	/**
	 * Build response for the log off message
	 * @param request
	 * @param response
	 * @param operation
	 * @param user
	 * @throws IOException
	 */
	public void logoff(Request request, Response response, Operation operation, String user) throws IOException {
		//body of message is empty so use a 204
		response.setStatus(Status.NO_CONTENT);
		
		//Clear the cookie value and set to expiry to 0 to tell the client to clear
		Cookie sessionCookie = new Cookie(PMSP_Constants.COOKIE_SESSION_KEY, "");
		sessionCookie.setExpiry(0);
		response.setCookie(sessionCookie);
		response.setCookie(COOKIE_STATE_KEY, PMSP_Constants.STATE_WAIT_FOR_LOGIN);
		
		response.close();
		
	}
	/**
	 * Create the base64 representation of the contents of the file
	 * @param filename
	 * @return
	 * @throws Exception
	 */
	public String encodeBase64(String filename) throws Exception
	{
		File file = new File(filename);
		//load the file
		byte[] bytes = loadFile(file);
		
		//encode the file
		byte[] encoded = Base64.encodeBase64(bytes);
		String encodedString = new String(encoded);

		return encodedString;
	}
	

	/**
	 * Create a byte array from the provided file
	 * @param file
	 * @return
	 * @throws IOException
	 */
	public static byte[] loadFile(File file) throws IOException {
	    InputStream is = null;
	    byte[] bytes;
	    
	    try {
		    is = new FileInputStream(file);
	
		    long length = file.length();
		    if (length > Integer.MAX_VALUE) {
		        // File is too large
		    	throw new IOException("File is too large");
		    }
		    bytes = new byte[(int)length];
		    
		    int offset = 0;
		    int numRead = 0;
		    //read in file
		    while (offset < bytes.length
		           && (numRead=is.read(bytes, offset, bytes.length-offset)) >= 0) {
		        offset += numRead;
		    }
	
		    //make sure we got it all
		    if (offset < bytes.length) {
		        throw new IOException("Could not completely read file "+file.getName());
		    }
	    }
	    finally {
	    	if (is != null) {
	    		try {
	    			is.close();
	    		}
	    		catch (Exception e) {}
 	    	}
	    		
	    }
	    
	    return bytes;
	    
	    
	}
}
