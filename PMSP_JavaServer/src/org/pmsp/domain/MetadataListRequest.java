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
 * Request object for getting a list of metadata
 */
@XStreamAlias("MetadataListRequest")
public class MetadataListRequest extends RequestType {

	/**
	 * media type
	 */
	@XStreamAsAttribute
	private String category;
	
	/**
	 * What kind of metadata
	 */
	@XStreamAsAttribute
	private String listType;
	
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
	public String getListType() {
		return listType;
	}
	public void setListType(String listType) {
		this.listType = listType;
	}
	
}
