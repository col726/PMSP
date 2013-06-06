package org.pmsp.test;

import org.apache.commons.codec.digest.DigestUtils;
import org.pmsp.ResponseBuilder;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Another test class.  Not used in actual application. 
 * Tests the encoding and such
 */

public class MessageTestHarness {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		
		ResponseBuilder rb = new ResponseBuilder();
		
		try {
			System.out.println(DigestUtils.sha1Hex(rb.encodeBase64("res/npp.6.3.3.Installer.exe")));
			System.out.println(rb.encodeBase64("res/testSound1.mp3"));
		} catch (Exception e) {
			e.printStackTrace();
		}
		
	}

}
