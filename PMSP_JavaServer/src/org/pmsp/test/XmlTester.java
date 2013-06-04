package org.pmsp.test;

import java.util.ArrayList;

import org.pmsp.domain.AudioFile;
import org.pmsp.domain.ListCriteria;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.Listing;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.MediaFileListing;
import org.pmsp.domain.MediaMetadataListing;
import org.pmsp.domain.MetadataListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.RequestType;
import org.pmsp.domain.Retrieval;
import org.pmsp.domain.RetrievalRequest;

import com.thoughtworks.xstream.XStream;

public class XmlTester {

	public static void printLoginRequest() {
		XStream xs = new XStream();
		xs.processAnnotations(new Class[] {Operation.class, LoginRequest.class});
		
		Operation op = new Operation();
		op.setType(new LoginRequest());
		
		System.out.println(xs.toXML(op));
	}
	public static void printFileListing() {
		XStream xs = new XStream();
		xs.processAnnotations(new Class[] {AudioFile.class, MediaFile.class, Listing.class, MediaFileListing.class});
		
		MediaFileListing l = new MediaFileListing();
		ArrayList<MediaFile> mediaFiles = new ArrayList<MediaFile>();
		mediaFiles.add(new AudioFile("Artist", "Album", "Title", "Genre", 1, "file1"));
		mediaFiles.add(new AudioFile("Artist2", "Album2", "Title2", "Genre2", 2, "file2"));
		l.setMediaFiles(mediaFiles);
		System.out.println(xs.toXML(l));
	}

	
	public static void printMetadataListing() {
		XStream xs = new XStream();
		xs.processAnnotations(new Class[] {AudioFile.class, MediaFile.class, Listing.class, MediaMetadataListing.class});
		
		MediaMetadataListing mml = new MediaMetadataListing();
		ArrayList<String> metadata = new ArrayList<String>();
		metadata.add("Folk");
		metadata.add("Rap");
		metadata.add("Alternative");
		mml.setMetadata(metadata);
		System.out.println(xs.toXML(mml));
	}

	
	public static void printRetrieval() {
		XStream xs = new XStream();
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
	
	public static void printFileListRequest() {
		XStream xs = new XStream();
		xs.processAnnotations(new Class[] {Operation.class, ListCriteria.class, FileListRequest.class});
//		xs.alias("Operation", Operation.class);
//		xs.alias("ListCriteria", ListCriteria.class);
//		xs.alias("ListRequest", ListRequest.class);
		FileListRequest lir = new FileListRequest();
		lir.setCategory("Music");
		ArrayList<ListCriteria> criteria = new ArrayList<ListCriteria>();
		criteria.add(new ListCriteria("Artist", "Smith"));
		criteria.add(new ListCriteria("Artist", "Green"));
		lir.setCriteria(criteria);
		Operation op = new Operation();
		op.setType(lir);
		System.out.println(xs.toXML(op));
	}
	
	public static void printMetadataListRequest() {
		XStream xs = new XStream();
		xs.processAnnotations(new Class[] {Operation.class, ListCriteria.class, MetadataListRequest.class});
//		xs.alias("Operation", Operation.class);
//		xs.alias("ListCriteria", ListCriteria.class);
//		xs.alias("ListRequest", ListRequest.class);
		MetadataListRequest mlr = new MetadataListRequest();
		mlr.setCategory("Music");
		mlr.setListType("Album");
		ArrayList<ListCriteria> criteria = new ArrayList<ListCriteria>();
		criteria.add(new ListCriteria("Artist", "Smith"));
		criteria.add(new ListCriteria("Artist", "Green"));
		mlr.setCriteria(criteria);
		Operation op = new Operation();
		op.setType(mlr);
		System.out.println(xs.toXML(op));
	}
	
	public static void printRetrieveOperation() {
		XStream xs = new XStream();
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
	
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		
		System.out.println("FileListRequest:");
		printFileListRequest();
		System.out.println("\nFileListResponse:");
		printFileListing();
		
		System.out.println("\nMetaDataListRequest:");
		printMetadataListRequest();
		System.out.println("\nMetaDataListResponse:");
		printMetadataListing();
		
		System.out.println("\nRetrieveRequest:");
		printRetrieveOperation();
		System.out.println("\nRetrieveResponse:");
		printRetrieval();
		System.out.println("\nLoginRequest:");
		printLoginRequest();
		
		

	}

}
