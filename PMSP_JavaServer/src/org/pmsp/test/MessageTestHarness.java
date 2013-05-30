package org.pmsp.test;

import java.io.File;

import org.pmsp.MessageParser;
<<<<<<< HEAD
=======
import org.pmsp.domain.Operation;
>>>>>>> 72ab3767b36ba53f835e7ddb22ef1447466bc289

public class MessageTestHarness {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		File testXML1 = new File("res/test//TestMessage1.xml");
		
		MessageParser testParser = new MessageParser();
<<<<<<< HEAD
		//BaseRequest testRequest = testParser.parseFile(testXML1);
=======
		Operation testRequest = testParser.parseFile(testXML1);
>>>>>>> 72ab3767b36ba53f835e7ddb22ef1447466bc289
		
		//System.out.println(testRequest.toString());
	}

}
