package org.pmsp.domain;

import java.util.ArrayList;
import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;
import com.thoughtworks.xstream.annotations.XStreamAsAttribute;
import com.thoughtworks.xstream.annotations.XStreamImplicit;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Request object for a list of files
 */
@XStreamAlias("RetrievalRequest")
public class RetrievalRequest extends RequestType {
	@XStreamAsAttribute
	private String mediaType;
	
	/**
	 * The list of id's that the client is requesting
	 */
	@XStreamImplicit(itemFieldName="id")
	private List<Integer> pmspIds = new ArrayList<Integer>();

	public List<Integer> getPmspIds() {
		return pmspIds;
	}

	public void setPmspIds(List<Integer> pmspIds) {
		this.pmspIds = pmspIds;
	}

	public String getMediaType() {
		return mediaType;
	}

	public void setMediaType(String mediaType) {
		this.mediaType = mediaType;
	}
}
