package org.pmsp.domain;

import com.thoughtworks.xstream.annotations.XStreamAsAttribute;
import com.thoughtworks.xstream.annotations.XStreamOmitField;

public abstract class MediaFile {
	
	@XStreamAsAttribute
	Integer pmspId;
	@XStreamAsAttribute
	String checksum;
	@XStreamOmitField
	String fileName;
	String data;
	
	public MediaFile(Integer pmspId) {
		this.pmspId = pmspId;
	}
	
	public Integer getPmspId() {
		return pmspId;
	}
	public void setPmspId(Integer pmspId) {
		this.pmspId = pmspId;
	}
	public String getChecksum() {
		return checksum;
	}
	public void setChecksum(String checksum) {
		this.checksum = checksum;
	}
	public String getData() {
		return data;
	}
	public void setData(String data) {
		this.data = data;
	}

	public String getFileName() {
		return fileName;
	}

	public void setFileName(String fileName) {
		this.fileName = fileName;
	}
	
	public abstract String getFullFilePath();
}
