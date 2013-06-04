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
import org.pmsp.domain.AudioFile;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.MediaFileListing;
import org.pmsp.domain.MediaMetadataListing;
import org.pmsp.domain.MetadataListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.Retrieval;
import org.pmsp.domain.RetrievalRequest;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;

public class ResponseBuilder {

	
	public void listFiles(Request request, Response response, Operation operation, String user) throws IOException, SQLException {
		PrintStream body = response.getPrintStream();
		response.setCookie(COOKIE_STATE_KEY, STATE_WAIT_FOR_FILE_CHOICE);
		
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		MediaFileListing mfl = new MediaFileListing();
		MusicDao dao = new MusicDao();
		mfl.setMediaFiles(dao.findTracks((FileListRequest)operation.getType()));
		
		
		body.println(MediaServer.getXmlParser().toXML(mfl));			

		body.close();
	}
	
	public void listMetadata(Request request, Response response, Operation operation, String user) throws IOException, 
	SQLException {
		PrintStream body = response.getPrintStream();
		response.setCookie(COOKIE_STATE_KEY, STATE_WAIT_FOR_LIST_CHOICE);
		
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		MediaMetadataListing mml = new MediaMetadataListing();
		MusicDao dao = new MusicDao();
		
		mml.setMetadata(dao.findMetadata((MetadataListRequest)operation.getType()));
		
		body.println(MediaServer.getXmlParser().toXML(mml));		

		
		body.close();
	}
	
	public void retrieval(Request request, Response response, Operation operation, String user) throws IOException, Exception {
		PrintStream body = response.getPrintStream();
		response.setCookie(COOKIE_STATE_KEY, PMSP_Constants.STATE_IDLE);
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		Retrieval r = new Retrieval();
		
		List<AudioFile> files = new MusicDao().findFiles(((RetrievalRequest)operation.getType()).getPmspIds());

		for (AudioFile f : files) {
			String s = encodeBase64(f.getFullFilePath());
			f.setData(s);
			f.setChecksum(DigestUtils.sha1Hex(s));
		}
		r.setMediaFiles(files);
		body.println(MediaServer.getXmlParser().toXML(r));	

		body.close();
	}
	
	public String login(Request request, Response response, Operation operation) throws IOException {

		//do authentication
		Authenticator auth = new Authenticator();
		String user = auth.authenticate(request.getValue("Authorization"));
		if (user != null) {
//			sessions.put(user, user);
			response.setCookie(PMSP_Constants.COOKIE_SESSION_KEY, user);
			response.setCookie(COOKIE_STATE_KEY, STATE_IDLE);
			response.close();
		}	
		return user;
	}
	
	public void logoff(Request request, Response response, Operation operation, String user) throws IOException {

		response.setCookie(PMSP_Constants.COOKIE_SESSION_KEY, "");
		response.setCookie(COOKIE_STATE_KEY, PMSP_Constants.STATE_WAIT_FOR_LOGIN);
		response.close();
		
	}
	
	public byte[] createChecksum(String fileName) throws Exception
	{
		InputStream fis = new FileInputStream(fileName);
		
		byte[] buffer = new byte[1024];
		
		MessageDigest complete = MessageDigest.getInstance("SHA-1");
	     int numRead;
	     do {
	      numRead = fis.read(buffer);
	      if (numRead > 0) {
	        complete.update(buffer, 0, numRead);
	        }
	      } while (numRead != -1);
	     fis.close();
	     return complete.digest();
	}
	
	public String getChecksum(String filename) throws Exception {
	     byte[] b = createChecksum(filename);
	     String result = "";
	     for (int i=0; i < b.length; i++) {
	       result +=
	          Integer.toString( ( b[i] & 0xff ) + 0x100, 16).substring( 1 );
	      }
	     return result;
	   }
	
	public String encodeBase64(String filename) throws Exception
	{
		File file = new File(filename);
		byte[] bytes = loadFile(file);
		byte[] encoded = Base64.encodeBase64(bytes);
		String encodedString = new String(encoded);

		return encodedString;
	}
	

	public static byte[] loadFile(File file) throws IOException {
	    InputStream is = new FileInputStream(file);

	    long length = file.length();
	    if (length > Integer.MAX_VALUE) {
	        // File is too large
	    }
	    byte[] bytes = new byte[(int)length];
	    
	    int offset = 0;
	    int numRead = 0;
	    while (offset < bytes.length
	           && (numRead=is.read(bytes, offset, bytes.length-offset)) >= 0) {
	        offset += numRead;
	    }

	    if (offset < bytes.length) {
	        throw new IOException("Could not completely read file "+file.getName());
	    }

	    is.close();
	    return bytes;
	}
}
