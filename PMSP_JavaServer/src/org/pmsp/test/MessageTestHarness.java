package org.pmsp.test;

import java.io.File;

import org.pmsp.MessageParser;

public class MessageTestHarness {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		File testXML1 = new File("res/test//TestMessage1.xml");
		
		MessageParser testParser = new MessageParser();
		//BaseRequest testRequest = testParser.parseFile(testXML1);
		
		//System.out.println(testRequest.toString());
	}

}
