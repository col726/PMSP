package org.pmsp;

import static org.pmsp.PMSP_Constants.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import org.pmsp.domain.FileListRequest;
import org.pmsp.domain.LoginRequest;
import org.pmsp.domain.LogoffRequest;
import org.pmsp.domain.MetadataListRequest;
import org.pmsp.domain.RequestType;
import org.pmsp.domain.RetrievalRequest;

/**
 * This defines valid state transitions. Might want to implement this
 * differently in the future, but it supports the protocol's defined DFA. Thought about 
 * representing this via config file or db table, but this is probably
 * not the sort of thing that we would change without corresponding code changes.
 * @author jkrupka
 *
 */
public class DfaValidator {

	private HashMap<String, List<String>> dfa = new HashMap<String, List<String>>();


	public DfaValidator() {
		ArrayList<String> transitions = new ArrayList<String>();
		transitions.add(LoginRequest.class.getCanonicalName());
		dfa.put(STATE_WAIT_FOR_LOGIN, transitions);

		transitions = new ArrayList<String>();
		transitions.add(MetadataListRequest.class.getCanonicalName());
		transitions.add(FileListRequest.class.getCanonicalName());
		transitions.add(LogoffRequest.class.getCanonicalName());
		dfa.put(STATE_IDLE, transitions);

		transitions = new ArrayList<String>();
		transitions.add(MetadataListRequest.class.getCanonicalName());
		transitions.add(FileListRequest.class.getCanonicalName());
		transitions.add(LogoffRequest.class.getCanonicalName());
		dfa.put(STATE_WAIT_FOR_LIST_CHOICE, transitions);

		transitions = new ArrayList<String>();
		transitions.add(RetrievalRequest.class.getCanonicalName());
		transitions.add(LogoffRequest.class.getCanonicalName());
		dfa.put(STATE_WAIT_FOR_FILE_CHOICE, transitions);

//		transitions = new ArrayList<String>();
//		transitions.add(LogoffRequest.class.getCanonicalName());
//		dfa.put(STATE_GETTING_FILE, transitions);
	}
	
	
	public boolean checkTransition(String fromState, RequestType type) {
		
		return dfa.get(fromState) != null && dfa.get(fromState).contains(type.getClass().getCanonicalName()); 
	}

}
