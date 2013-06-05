package org.pmsp.domain;

import java.util.List;

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
 * Domain object for a file listing request
 *
 */
@XStreamAlias("FileListRequest")
public class FileListRequest extends RequestType {

	/**
	 * The media type
	 */
	@XStreamAsAttribute
	private String category;
	
	/**
	 * List of criteria that the client was to search for.  Current implementation semantics 
	 * are that these are AND'ed together
	 */
	private List<ListCriteria> criteria;

	public String getCategory() {
		return category;
	}
	public void setCategory(String category) {
		this.category = category;
	}
	
	public List<ListCriteria> getCriteria() {
		return criteria;
	}
	public void setCriteria(List<ListCriteria> criteria) {
		this.criteria = criteria;
	}
	
}
