package org.pmsp.test;

import java.io.File;

import org.pmsp.MessageParser;
import org.pmsp.domain.BaseRequest;

public class MessageTestHarness {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		File testXML1 = new File("xmlPath/testMessage1.xml");
		
		MessageParser testParser = new MessageParser();
		BaseRequest testRequest = testParser.parseFile(testXML1);
		
		System.out.println(testRequest.toString());
	}

}
