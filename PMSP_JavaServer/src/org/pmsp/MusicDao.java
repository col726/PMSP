package org.pmsp;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import org.pmsp.domain.AudioFile;
import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.MetadataListRequest;

public class MusicDao {

	/**
	 * Retrieves the info about a file for the list of id's specified 
	 * @param ids
	 * @return
	 * @throws SQLException
	 */
	public List<AudioFile> findFiles(List<Integer> ids) throws SQLException {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		List <AudioFile> list = new ArrayList<AudioFile>();
		try {
			conn = MediaServer.getDbConnection();
			StringBuilder sb = new StringBuilder("select * from music where id in (");
			
			for( int i = 0 ; i < ids.size(); i++ ) {
			    sb.append("?,");
			}
			sb.deleteCharAt(sb.length()-1).append(")");
			pstmt = conn.prepareStatement(sb.toString());
			for ( int i = 0 ; i < ids.size(); i++ ) {
				pstmt.setInt(i+1, ids.get(i));
			}
			rs = pstmt.executeQuery();
			
			while (rs.next()) {
				AudioFile af = new AudioFile(rs.getString("artist"), rs.getString("album"), rs.getString("title"), 
						rs.getString("genre"), rs.getInt("id"), rs.getString("file_name"));
				list.add(af);
			}
		}
		finally {
			close(rs);
			close(pstmt);
			close(conn);
		}
		return list;
	}
	
	public List<String> findMetadata(MetadataListRequest criteria) throws SQLException {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		List <String> list = new ArrayList<String>();
		try {
			conn = MediaServer.getDbConnection();
			StringBuilder sb = new StringBuilder("select distinct(");
			sb.append(criteria.getListType()).append(") from music ");
			if (criteria.getCriteria().size() > 0) {
				sb.append(" where ");
				for( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				    sb.append(criteria.getCriteria().get(i).getName()).append(" = ? and ");
				}
				sb.delete(sb.length()-4, sb.length());
			}
			
			
			pstmt = conn.prepareStatement(sb.toString());
			for ( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				pstmt.setString(i+1, criteria.getCriteria().get(i).getValue());
			}
			rs = pstmt.executeQuery();
			
			while (rs.next()) {
				list.add(rs.getString(criteria.getListType()));
			}
		}
		finally {
			close(rs);
			close(pstmt);
			close(conn);
		}
		return list;
	}
	
	public List<AudioFile> findTracks(FileListRequest criteria) throws SQLException {
		Connection conn = null;
		PreparedStatement pstmt = null;
		ResultSet rs = null;
		List <AudioFile> list = new ArrayList<AudioFile>();
		try {
			conn = MediaServer.getDbConnection();
			StringBuilder sb = new StringBuilder("select * from music ");
			
			if (criteria.getCriteria().size() > 0) {
				sb.append(" where ");
				for( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				    sb.append(criteria.getCriteria().get(i).getName()).append(" = ? and ");
				}
				sb.delete(sb.length()-4, sb.length());
			}
			
			pstmt = conn.prepareStatement(sb.toString());
			for ( int i = 0 ; i < criteria.getCriteria().size(); i++ ) {
				pstmt.setString(i+1, criteria.getCriteria().get(i).getValue());
			}
			rs = pstmt.executeQuery();
			
			while (rs.next()) {
				AudioFile af = new AudioFile(rs.getString("artist"), rs.getString("album"), rs.getString("title"), 
						rs.getString("genre"), rs.getInt("id"), rs.getString("file_name"));
				list.add(af);
			}
		}
		finally {
			close(rs);
			close(pstmt);
			close(conn);
		}
		return list;
	}
	
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
