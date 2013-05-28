package org.pmsp.domain;

public abstract class MediaFile {
	String pmspId;
	String checksum;
	String data;
	
	public MediaFile(String pmspId) {
		this.pmspId = pmspId;
	}
	
	public String getPmspId() {
		return pmspId;
	}
	public void setPmspId(String pmspId) {
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
}
