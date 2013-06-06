package org.pmsp.domain;

import org.pmsp.MediaServer;
import org.pmsp.PMSP_Constants;

import com.thoughtworks.xstream.annotations.XStreamAlias;
import com.thoughtworks.xstream.annotations.XStreamAsAttribute;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Domain object for audio files.  Can be used with data payload for downloading or without as just a listing
 *
 */
@XStreamAlias("AudioFile")
public class AudioFile extends MediaFile {

	@XStreamAsAttribute
	private String artist;
	@XStreamAsAttribute
	private String album;
	@XStreamAsAttribute
	private String title;
	@XStreamAsAttribute
	private String genre;
	
	public AudioFile(String artist, String album, String title, String genre, Integer pmspId, String fileName) {
		super(pmspId);
		this.artist = artist;
		this.album = album;
		this.title = title;
		this.genre = genre;
		this.fileName = fileName;
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

	/**
	 * Files are stored in a directory under the data dir following this pattern for directory structure
	 */
	@Override
	public String getFullFilePath() {
		StringBuilder sb = new StringBuilder();
		String sep = System.getProperty("file.separator");
		sb.append(MediaServer.props.getProperty(PMSP_Constants.DATA_DIR_KEY)).append(sep).append("files").append(sep);
		sb.append("Music").append(sep).append(artist).append(sep).append(album).append(sep).append(fileName);
		return sb.toString();
	}
	
}
