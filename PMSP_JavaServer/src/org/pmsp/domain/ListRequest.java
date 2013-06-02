package org.pmsp.domain;

import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;

@XStreamAlias("ListRequest")
public class ListRequest extends RequestType {

	private String category;
	private String listType;
	private List<ListCriteria> criteria;

	public String getCategory() {
		return category;
	}
	public void setCategory(String category) {
		this.category = category;
	}
	public String getListType() {
		return listType;
	}
	public void setListType(String listType) {
		this.listType = listType;
	}
	public List<ListCriteria> getCriteria() {
		return criteria;
	}
	public void setCriteria(List<ListCriteria> criteria) {
		this.criteria = criteria;
	}
	
}
