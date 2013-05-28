package org.pmsp.domain;

public class AudioFile extends MediaFile {

	private String artist;
	private String album;
	private String title;
	private String genre;
	
	public AudioFile(String artist, String album, String title, String genre, String pmspId) {
		super(pmspId);
		this.artist = artist;
		this.album = album;
		this.title = title;
		this.genre = genre;
	}
	
	
	public String getArtist() {
		return artist;
	}
	public void setArtist(String artist) {
		this.artist = artist;
	}
	public String getAlbum() {
		return album;
	}
	public void setAlbum(String album) {
		this.album = album;
	}
	public String getTitle() {
		return title;
	}
	public void setTitle(String title) {
		this.title = title;
	}
	public String getGenre() {
		return genre;
	}
	public void setGenre(String genre) {
		this.genre = genre;
	}
	
}
