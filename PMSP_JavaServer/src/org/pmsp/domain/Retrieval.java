package org.pmsp.domain;

import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Response object for retrieving a list of files
 */
@XStreamAlias("Retrieval")
public class Retrieval {
	private List<? extends MediaFile> mediaFiles;

	public List<? extends MediaFile> getMediaFiles() {
		return mediaFiles;
	}

	public void setMediaFiles(List<? extends MediaFile> mediaFiles) {
		this.mediaFiles = mediaFiles;
	}
}
