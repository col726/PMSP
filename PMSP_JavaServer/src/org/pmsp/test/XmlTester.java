package org.pmsp.test;

import java.util.ArrayList;

import org.pmsp.domain.AudioFile;
import org.pmsp.domain.ListCriteria;
import org.pmsp.domain.ListRequest;
import org.pmsp.domain.Listing;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.Operation;
import org.pmsp.domain.RequestType;
import org.pmsp.domain.Retrieval;
import org.pmsp.domain.RetrievalRequest;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.StaxDriver;

public class XmlTester {

	public static void printListing() {
		XStream xs = new XStream(new StaxDriver());
//		xs.alias("Listing", Listing.class);
//		xs.alias("AudioFile", AudioFile.class);
//		xs.useAttributeFor(AudioFile.class, "pmspId");
//		xs.useAttributeFor(AudioFile.class, "artist");
//		xs.useAttributeFor(AudioFile.class, "album");
//		xs.useAttributeFor(AudioFile.class, "title");
//		xs.useAttributeFor(AudioFile.class, "genre");
		xs.processAnnotations(new Class[] {AudioFile.class, MediaFile.class, Listing.class});
		
		Listing l = new Listing();
		ArrayList<MediaFile> mediaFiles = new ArrayList<MediaFile>();
		mediaFiles.add(new AudioFile("Artist", "Album", "Title", "Genre", 1, "file1"));
		mediaFiles.add(new AudioFile("Artist2", "Album2", "Title2", "Genre2", 2, "file2"));
		l.setMediaFiles(mediaFiles);
		System.out.println(xs.toXML(l));
	}
	
	public static void printRetrieval() {
		XStream xs = new XStream(new StaxDriver());
//		xs.alias("Retrieval", Retrieval.class);
//		xs.alias("AudioFile", AudioFile.class);
//		xs.useAttributeFor(AudioFile.class, "pmspId");
//		xs.useAttributeFor(AudioFile.class, "artist");
//		xs.useAttributeFor(AudioFile.class, "album");
//		xs.useAttributeFor(AudioFile.class, "title");
//		xs.useAttributeFor(AudioFile.class, "genre");
//		xs.useAttributeFor(AudioFile.class, "checksum");
		xs.processAnnotations(new Class[] {AudioFile.class, MediaFile.class, Retrieval.class});
		Retrieval r = new Retrieval();
		ArrayList<MediaFile> mediaFiles = new ArrayList<MediaFile>();
		AudioFile af = new AudioFile("Artist", "Album", "Title", "Genre", 1, "file1");
		af.setChecksum("12345");
		af.setData("base64encodeddata");
		mediaFiles.add(af);
		af = new AudioFile("Artist2", "Album2", "Title2", "Genre2", 2, "file2");
		af.setChecksum("98765");
		af.setData("base64encodeddata2");
		mediaFiles.add(af);
		r.setMediaFiles(mediaFiles);
		System.out.println(xs.toXML(r));
	}
	
	public static void printListRequest() {
		XStream xs = new XStream(new StaxDriver());
		xs.processAnnotations(new Class[] {Operation.class, ListCriteria.class, ListRequest.class});
//		xs.alias("Operation", Operation.class);
//		xs.alias("ListCriteria", ListCriteria.class);
//		xs.alias("ListRequest", ListRequest.class);
		ListRequest lir = new ListRequest();
		lir.setCategory("Music");
		lir.setListType("Track");
		ArrayList<ListCriteria> criteria = new ArrayList<ListCriteria>();
		criteria.add(new ListCriteria("Artist", "Smith"));
		criteria.add(new ListCriteria("Artist", "Green"));
		lir.setCriteria(criteria);
		Operation op = new Operation();
		op.setType(lir);
		System.out.println(xs.toXML(op));
	}
	
	public static void printRetrieveOperation() {
		XStream xs = new XStream(new StaxDriver());
		xs.processAnnotations(new Class[] {Operation.class, RetrievalRequest.class, RequestType.class});
//		xs.alias("Operation", Operation.class);
//		xs.alias("ListCriteria", ListCriteria.class);
//		xs.alias("ListRequest", ListRequest.class);
//		xs.alias("RetrievalRequest", RetrievalRequest.class);

		RetrievalRequest rr = new RetrievalRequest();
		
		rr.getPmspIds().add(new Integer(1));
		rr.getPmspIds().add(new Integer(2));
		rr.setMediaType("Music");
		Operation op = new Operation();
		op.setType(rr);
		System.out.println(xs.toXML(op));
	}
	
	public static void printLoginRequest() {
		XStream xs = new XStream(new StaxDriver());;
		LoginRequest lr = new LoginRequest();
		lr.setUsername("test");
		lr.setPassword("password");
		
		System.out.println(xs.toXML(lr));
	}
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		
		System.out.println("ListRequest:");
		printListRequest();
		System.out.println("\nListResponse:");
		printListing();
		System.out.println("\nRetrieveRequest:");
		printRetrieveOperation();
		System.out.println("\nRetrieveResponse:");
		printRetrieval();
		
		

	}

}
