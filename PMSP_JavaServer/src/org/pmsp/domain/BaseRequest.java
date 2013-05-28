package org.pmsp.domain;



public abstract class BaseRequest {

	public enum MessageType {
		LOGINREQUEST, LISTREQUEST, MEDIAREQUEST
	}
	
	private Number version;
	private String state;
	private String session_id;
	private MessageType messageType;
	
	public Number getVersion() {
		return version;
	}
	public void setVersion(Number version) {
		this.version = version;
	}
	public String getState() {
		return state;
	}
	public void setState(String state) {
		this.state = state;
	}
	public String getSession_id() {
		return session_id;
	}
	public void setSession_id(String session_id) {
		this.session_id = session_id;
	}
}
