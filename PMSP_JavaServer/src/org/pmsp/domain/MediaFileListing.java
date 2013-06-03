package org.pmsp.domain;

import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;

@XStreamAlias("MediaFileListing")
public class MediaFileListing extends Listing {
	private List<? extends MediaFile> mediaFiles;

	public List<? extends MediaFile> getMediaFiles() {
		return mediaFiles;
	}

	public void setMediaFiles(List<? extends MediaFile> mediaFiles) {
		this.mediaFiles = mediaFiles;
	}
}
