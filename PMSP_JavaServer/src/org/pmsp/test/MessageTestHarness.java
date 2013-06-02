package org.pmsp.test;

import java.io.File;

import org.pmsp.MessageParser;
import org.pmsp.ResponseBuilder;


import org.pmsp.domain.Operation;

public class MessageTestHarness {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		
		File testMP3 = new File("res/testSound1.mp3");
		
		ResponseBuilder rb = new ResponseBuilder();
		
		try {
			System.out.println(rb.getChecksum("res/npp.6.3.3.Installer.exe"));
			System.out.println(rb.encodeBase64("res/testSound1.mp3"));
		} catch (Exception e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
		

		//BaseRequest testRequest = testParser.parseFile(testXML1);
		//Operation testRequest = testParser.parseFile(testXML1);
		
		//System.out.println(testRequest.toString());
	}

}
