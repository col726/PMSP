package org.pmsp.domain;

import com.thoughtworks.xstream.annotations.XStreamAlias;
import com.thoughtworks.xstream.annotations.XStreamAsAttribute;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * A set of criteria for looking up information in the pmsp database
 *
 */
@XStreamAlias("ListCriteria")
public class ListCriteria {

	@XStreamAsAttribute
	private String name;
	@XStreamAsAttribute
	private String value;

	public ListCriteria(String name, String value) {
		super();
		this.name = name;
		this.value = value;
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public String getValue() {
		return value;
	}

	public void setValue(String value) {
		this.value = value;
	}
	
}
