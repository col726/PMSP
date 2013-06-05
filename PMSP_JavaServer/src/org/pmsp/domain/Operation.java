package org.pmsp.domain;

import com.thoughtworks.xstream.annotations.XStreamAlias;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Enclosing class for all our current request objects.  Done this way to allow flexibility for future functionality 
 */
@XStreamAlias("Operation")
public class Operation {
	private RequestType type;

	public RequestType getType() {
		return type;
	}

	public void setType(RequestType type) {
		this.type = type;
	}
}
