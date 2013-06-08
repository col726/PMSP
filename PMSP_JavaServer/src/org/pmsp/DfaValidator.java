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

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/


/**
 * STATEFUL
 * This defines valid state transitions. Might want to implement this
 * differently in the future, but it supports the protocol's defined DFA. Thought about 
 * representing this via config file or db table, but this is probably
 * not the sort of thing that we would change without corresponding code changes.
 */
public class DfaValidator {

	/**
	 * Map of state to list of allowable messages for that state.  "Messages" are the class name of that message 
	 * the key of the map is the state name in the cookie.
	 */
	private HashMap<String, List<String>> dfa = new HashMap<String, List<String>>();


	/**
	 * Default constructor.  Loads the in memory representation of the DFA states and transitions
	 */
	public DfaValidator() {
		//For each state in our DFA, create a list of the allowable messages while in that state.
		//then add them to the hash that represents the entire DFA structure
		ArrayList<String> transitions = new ArrayList<String>();
		transitions.add(LoginRequest.class.getCanonicalName());
		transitions.add(LogoffRequest.class.getCanonicalName());
		dfa.put(STATE_WAIT_FOR_LOGIN, transitions);

		transitions = new ArrayList<String>();
		transitions.add(MetadataListRequest.class.getCanonicalName());
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

	}
	
	/**
	 * verifies this message type is valid for the source state
	 * @param fromState - this corresponds to the state name in the cookie
	 * @param type - The request object
	 * @return true if it's a valid transition or false if no
	 */
	public boolean checkTransition(String fromState, RequestType type) {
		//if the state is known and the list of allowable messages contains the type specified return true
		return dfa.get(fromState) != null && dfa.get(fromState).contains(type.getClass().getCanonicalName()); 
	}

}
