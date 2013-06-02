package org.pmsp;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import org.pmsp.domain.AudioFile;

public class MusicDao {

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
