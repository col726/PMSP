package org.pmsp;
import java.io.File;

import org.pmsp.domain.ListCriteria;
import org.pmsp.domain.ListRequest;
import org.pmsp.domain.Operation;
import org.pmsp.domain.RetrievalRequest;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;

public class MessageParser {
	

public Operation parse(String xml) {
	XStream xs = new XStream(new DomDriver());;
	xs.alias("Operation", Operation.class);
	xs.alias("ListCriteria", ListCriteria.class);
	xs.alias("ListRequest", ListRequest.class);
	xs.alias("RetrievalRequest", RetrievalRequest.class);
	xs.alias("id", String.class);
	
	Operation br = null;
	try {
	br = (Operation) xs.fromXML(xml);
	}
	catch (Throwable t) {
		t.printStackTrace();
	}
	
	
	//will probably want to figure out what this really is and return the non-abstract version
	
	return br;
}

public Operation parseFile(File xml) {
	XStream xs = new XStream(new DomDriver());;
	
	Operation br = null;
	try {
	br = (Operation) xs.fromXML(xml);
	}
	catch (Throwable t) {
		t.printStackTrace();
	}
	
	
	//will probably want to figure out what this really is and return the non-abstract version
	
	return br;
}

}
