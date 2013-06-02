package org.pmsp.domain;

import java.util.ArrayList;
import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;
import com.thoughtworks.xstream.annotations.XStreamImplicit;

@XStreamAlias("RetrievalRequest")
public class RetrievalRequest extends RequestType {
	private String mediaType;
	
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
