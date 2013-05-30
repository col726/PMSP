package org.pmsp.domain;

import java.util.ArrayList;
import java.util.List;

public class RetrievalRequest extends RequestType {
	private List<String> pmspIds = new ArrayList<String>();

	public List<String> getPmspIds() {
		return pmspIds;
	}

	public void setPmspIds(List<String> pmspIds) {
		this.pmspIds = pmspIds;
	}
}
