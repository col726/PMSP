package org.pmsp;
import org.pmsp.domain.Operation;
import java.io.File;

import org.pmsp.domain.BaseRequest;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;

public class MessageParser {
	

public Operation parse(String xml) {
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

public BaseRequest parseFile(File xml) {
	XStream xs = new XStream(new DomDriver());;
	
	BaseRequest br = null;
	try {
	br = (BaseRequest) xs.fromXML(xml);
	}
	catch (Throwable t) {
		t.printStackTrace();
	}
	
	
	//will probably want to figure out what this really is and return the non-abstract version
	
	return br;
}

}
