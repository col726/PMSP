package org.pmsp.domain;

import com.thoughtworks.xstream.annotations.XStreamAlias;

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
