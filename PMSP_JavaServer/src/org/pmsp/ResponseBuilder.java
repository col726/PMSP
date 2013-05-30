package org.pmsp;

import java.io.IOException;
import java.io.PrintStream;
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
		af.setChecksum("12345");
		af.setData("base64encodeddata");
		mediaFiles.add(af);
		af = new AudioFile("Artist2", "Album2", "Title2", "Genre2", "ID2");
		af.setChecksum("98765");
		af.setData("base64encodeddata2");
		mediaFiles.add(af);
		r.setMediaFiles(mediaFiles);
		body.println(xs.toXML(r));	

		body.close();
	}
}
