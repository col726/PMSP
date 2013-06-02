package org.pmsp;
import java.io.File;

import org.pmsp.domain.Operation;

import com.thoughtworks.xstream.XStream;

public class MessageParser {
	

public Operation parse(String xml) {
//	XStream xs = new XStream(new StaxDriver());;
//	xs.alias("Operation", Operation.class);
//	xs.alias("ListCriteria", ListCriteria.class);
//	xs.alias("ListRequest", ListRequest.class);
//	xs.alias("RetrievalRequest", RetrievalRequest.class);
//	xs.alias("id", String.class);
//	xs.processAnnotations(new Class[] {Operation.class, ListCriteria.class, ListRequest.class, RetrievalRequest.class});
	Operation br = null;
	try {
	br = (Operation) MediaServer.getXmlParser().fromXML(xml);
	}
	catch (Throwable t) {
		t.printStackTrace();
	}
	
	
	//will probably want to figure out what this really is and return the non-abstract version
	
	return br;
}

public Operation parseFile(File xml) {
	XStream xs = MediaServer.getXmlParser();
	
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
