package org.pmsp.test;

import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.ArrayList;

import org.apache.commons.codec.binary.Base64;
import org.apache.commons.io.FileUtils;
import org.pmsp.domain.ListCriteria;
import org.pmsp.domain.ListRequest;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.Operation;

import com.thoughtworks.xstream.XStream;
import com.thoughtworks.xstream.io.xml.DomDriver;


public class Encoder {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		try {
			
			File file = new File(args[0]);
			System.out.println(file.toString());
			String data = Base64.encodeBase64String(FileUtils.readFileToByteArray(file));
			PrintWriter pw = new PrintWriter("data.txt");
			pw.println(data);
			pw.flush(); pw.close();
		
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

}
