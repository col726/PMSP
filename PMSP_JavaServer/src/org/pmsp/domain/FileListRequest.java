package org.pmsp.domain;

import java.util.List;

import com.thoughtworks.xstream.annotations.XStreamAlias;
import com.thoughtworks.xstream.annotations.XStreamAsAttribute;

@XStreamAlias("FileListRequest")
public class FileListRequest extends RequestType {

	@XStreamAsAttribute
	private String category;
	
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
	
}
