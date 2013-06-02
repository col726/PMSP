package org.pmsp.domain;

import com.thoughtworks.xstream.annotations.XStreamAlias;

@XStreamAlias("ListCriteria")
public class ListCriteria {

	private String name;
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
