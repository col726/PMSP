package org.pmsp.domain;

import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;
import com.thoughtworks.xstream.annotations.XStreamImplicit;

@XStreamAlias("MetadataListing")
public class MediaMetadataListing extends Listing {

	@XStreamImplicit(itemFieldName="item")
	private List<String> metadata;

	public List<String> getMetadata() {
		return metadata;
	}

	public void setMetadata(List<String> metadata) {
		this.metadata = metadata;
	}

}
