package org.pmsp;

import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.PrintStream;
import java.security.MessageDigest;
import java.util.ArrayList;

import org.pmsp.domain.AudioFile;
import org.pmsp.domain.Listing;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.Operation;
import org.pmsp.domain.Retrieval;
import org.simpleframework.http.Request;
import org.simpleframework.http.Response;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;
import org.apache.commons.codec.binary.Base64;

public class ResponseBuilder {

	public void login(Request request, Response response, Operation operation, String user) {
		
	}
	
	public void list(Request request, Response response, Operation operation, String user) throws IOException {
		PrintStream body = response.getPrintStream();
		
		body.println("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
		
		XStream xs = new XStream(new DomDriver());
		xs.alias("Listing", Listing.class);
		xs.alias("AudioFile", AudioFile.class);
		xs.useAttributeFor(AudioFile.class, "pmspId");
		xs.useAttributeFor(AudioFile.class, "artist");
		xs.useAttributeFor(AudioFile.class, "album");
		xs.useAttributeFor(AudioFile.class, "title");
		xs.useAttributeFor(AudioFile.class, "genre");
		Listing l = new Listing();
		ArrayList<MediaFile> mediaFiles = new ArrayList<MediaFile>();
		mediaFiles.add(new AudioFile("Artist", "Album", "Title", "Genre", "ID"));
		mediaFiles.add(new AudioFile("Artist2", "Album2", "Title2", "Genre2", "ID2"));
		l.setMediaFiles(mediaFiles);
		
		
		body.println(xs.toXML(l));			

		body.close();
	}
	
	public void retrieval(Request request, Response response, Operation operation, String user) throws IOException {
		PrintStream body = response.getPrintStream();
		
		String testFile = "res/testSound1.mp3";
		
		XStream xs = new XStream(new DomDriver());
		xs.alias("Retrieval", Retrieval.class);
		xs.alias("AudioFile", AudioFile.class);
		xs.useAttributeFor(AudioFile.class, "pmspId");
		xs.useAttributeFor(AudioFile.class, "artist");
		xs.useAttributeFor(AudioFile.class, "album");
		xs.useAttributeFor(AudioFile.class, "title");
		xs.useAttributeFor(AudioFile.class, "genre");
		xs.useAttributeFor(AudioFile.class, "checksum");
		Retrieval r = new Retrieval();
		
		//TODO need to actually fetch the data that was asked for, generate the checksum, the Base64 encoded data, etc
		ArrayList<MediaFile> mediaFiles = new ArrayList<MediaFile>();
		AudioFile af = new AudioFile("Artist", "Album", "Title", "Genre", "ID");
		try {
			af.setChecksum(getChecksum(testFile));
			af.setData(this.encodeBase64(testFile));//encodeBase64
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}//getCheckSum
		
		mediaFiles.add(af);
		af = new AudioFile("Artist2", "Album2", "Title2", "Genre2", "ID2");
		af.setChecksum("98765");
		af.setData("base64encodeddata2");
		mediaFiles.add(af);
		r.setMediaFiles(mediaFiles);
		body.println(xs.toXML(r));	

		body.close();
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
