package org.pmsp;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import org.pmsp.domain.AudioFile;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.MediaFile;
import org.pmsp.domain.MetadataListRequest;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Data Access Object for looking up info from the database
 *
 */
public class MediaDao {

	/**
	 * Retrieves the info about the file(s) for the list of id's specified 
	 * @param ids - the id's to retrieve from the db
	 * @param category - media type
	 * @return - the info about the files
	 * @throws SQLException
	 */
	public List<? extends MediaFile> findFiles(List<Integer> ids, String category) throws SQLException {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		List <MediaFile> list = new ArrayList<MediaFile>();
		try {
			conn = MediaServer.getDbConnection();
			//build the sql to match the # of id's passed in
			StringBuilder sb = new StringBuilder("select * from " + category + " where id in (");
			
			for( int i = 0 ; i < ids.size(); i++ ) {
			    sb.append("?,");
			}
			//delete the extra comma we don't need and add ) to complete the sql stmt
			sb.deleteCharAt(sb.length()-1).append(")");
			
			//create prepared stmt and plug in the params
			pstmt = conn.prepareStatement(sb.toString());
			for ( int i = 0 ; i < ids.size(); i++ ) {
				pstmt.setInt(i+1, ids.get(i));
			}
			rs = pstmt.executeQuery();
			
			//iterate over the results and build the resulting MediaFile objects
			while (rs.next()) {
				//for the time being since we only support audio files, this will suffice
				//will need to make this generic when adding support for other media types
				AudioFile af = new AudioFile(rs.getString("artist"), rs.getString("album"), rs.getString("title"), 
						rs.getString("genre"), rs.getInt("id"), rs.getString("file_name"));
				list.add(af);
			}
		}
		//make sure all resources are closed
		finally {
			close(rs);
			close(pstmt);
			close(conn);
		}
		return list;
	}
	
	/**
	 * Retrieve the list of metadata types requested
	 * @param criteria - search criteria
	 * @return list of items matching search criteria
	 * @throws SQLException
	 */
	public List<String> findMetadata(MetadataListRequest criteria) throws SQLException {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		List <String> list = new ArrayList<String>();
		
		try {
			conn = MediaServer.getDbConnection();
			//build sql query
			StringBuilder sb = new StringBuilder("select distinct(");
			sb.append(criteria.getListType()).append(") from " + criteria.getCategory());
			
			//if there are search items and not just a retrieve all request, build the where statement
			if (criteria.getCriteria().size() > 0) {
				sb.append(" where ");
				for( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				    sb.append(criteria.getCriteria().get(i).getName()).append(" = ? and ");
				}
				//delete the trailing and part
				sb.delete(sb.length()-4, sb.length());
			}
			
			//iterate through the search criteria and plug in the values
			pstmt = conn.prepareStatement(sb.toString());
			for ( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				pstmt.setString(i+1, criteria.getCriteria().get(i).getValue());
			}
			rs = pstmt.executeQuery();
			
			//iterate over results and build list of items to return
			while (rs.next()) {
				list.add(rs.getString(criteria.getListType()));
			}
		}
		//make sure all resources are closed
		finally {
			close(rs);
			close(pstmt);
			close(conn);
		}
		return list;
	}
	
	/**
	 * Retrieve a list of files that match the search criteria
	 * @param criteria - the search criteria
	 * @return - a list of files matching the search criteria
	 * @throws SQLException
	 */
	public List<? extends MediaFile> findFiles(FileListRequest criteria) throws SQLException {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		List <MediaFile> list = new ArrayList<MediaFile>();
		
		try {
			//start building the sql statement
			conn = MediaServer.getDbConnection();
			StringBuilder sb = new StringBuilder("select * from " + criteria.getCategory());

			//if search values were provided, build the where clause of the sql statement
			if (criteria.getCriteria().size() > 0) {
				sb.append(" where ");
				for( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				    sb.append(criteria.getCriteria().get(i).getName()).append(" = ? and ");
				}
				//get rid of the trailing and
				sb.delete(sb.length()-4, sb.length());
			}
			
			//iterate over the criteria and plug the value into the pstmt
			pstmt = conn.prepareStatement(sb.toString());
			for ( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				pstmt.setString(i+1, criteria.getCriteria().get(i).getValue());
			}
			rs = pstmt.executeQuery();
			
			while (rs.next()) {
				//for the time being since we only support audio files, this will suffice
				//will need to make this generic when adding support for other media types
				AudioFile af = new AudioFile(rs.getString("artist"), rs.getString("album"), rs.getString("title"), 
						rs.getString("genre"), rs.getInt("id"), rs.getString("file_name"));
				list.add(af);
			}
		}
		//close all the resources
		finally {
			close(rs);
			close(pstmt);
			close(conn);
		}
		return list;
	}
	
	/**
	 * Internal convenience method for closing db related classes and swallowing resulting exceptions
	 * @param obj
	 */
	private void close(Object obj) {
		if (obj instanceof Connection) {
			try {
				((Connection)obj).close();
			}
			catch (Throwable t) {}
		}
		else if (obj instanceof ResultSet) {
			try {
				((ResultSet)obj).close();
			}
			catch (Throwable t) {}
		}
		else if (obj instanceof PreparedStatement) {
			try {
				((PreparedStatement)obj).close();
			}
			catch (Throwable t) {}
		}
	}
}
