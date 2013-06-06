package org.pmsp;
import org.pmsp.domain.Operation;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * This class is responsible for converting the xml into our object.  Because of our object structure and the use of
 * xstream, there's not much here.  But in future version there may be more here.
 *
 */
public class MessageParser {
	
/**
 * Convert the xml to one of our Operation objects
 * @param xml
 * @return
 */
public Operation parse(String xml) {
	Operation br = null;
	
	br = (Operation) MediaServer.getXmlParser().fromXML(xml);
		
	return br;
}

}
