package org.pmsp.test;

import java.util.ArrayList;

import org.pmsp.domain.AudioFile;
import org.pmsp.domain.ListCriteria;
import org.pmsp.domain.ListRequest;
import org.pmsp.domain.Listing;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.Operation;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;

public class XmlTester {

	public static void printListing() {
		XStream xs = new XStream(new DomDriver());
		
		Listing l = new Listing();
		ArrayList<MediaFile> mediaFiles = new ArrayList<MediaFile>();
		mediaFiles.add(new AudioFile("Artist", "Album", "Title", "Genre", "ID"));
		mediaFiles.add(new AudioFile("Artist2", "Album2", "Title2", "Genre2", "ID2"));
		l.setMediaFiles(mediaFiles);
		System.out.println(xs.toXML(l));
	}
	
	public static void printOperation() {
		XStream xs = new XStream(new DomDriver());
		xs.alias("Operation", Operation.class);
		xs.alias("ListCriteria", ListCriteria.class);
		xs.alias("ListRequest", ListRequest.class);
		ListRequest lir = new ListRequest();
		lir.setCategory("Music");
		lir.setListType("Track");
		ArrayList<ListCriteria> criteria = new ArrayList<ListCriteria>();
		criteria.add(new ListCriteria("Music", "Artist", "Smith"));
		criteria.add(new ListCriteria("Music", "Artist", "Green"));
		lir.setCriteria(criteria);
		Operation op = new Operation();
		op.setType(lir);
		System.out.println(xs.toXML(op));
	}
	
	public static void printLoginRequest() {
		XStream xs = new XStream(new DomDriver());;
		LoginRequest lr = new LoginRequest();
		lr.setUsername("test");
		lr.setPassword("password");
		
		System.out.println(xs.toXML(lr));
	}
	/**
	 * @param args
	 */
	public static void main(String[] args) {
		printListing();
		
		

	}

}
