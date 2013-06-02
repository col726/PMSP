package org.pmsp.test;

import java.io.File;
import java.io.IOException;
import java.io.PrintWriter;

import org.apache.commons.codec.binary.Base64;
import org.apache.commons.io.FileUtils;


public class Encoder {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		try {
			long start = System.currentTimeMillis();
			File file = new File(args[0]);
			System.out.println(file.toString());
			String data = Base64.encodeBase64String(FileUtils.readFileToByteArray(file));
			PrintWriter pw = new PrintWriter("data.txt");
			pw.println(data);
			pw.flush(); pw.close();
			long end = System.currentTimeMillis();
			System.out.println((end - start) / 1000.0);
		
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

	}

}
