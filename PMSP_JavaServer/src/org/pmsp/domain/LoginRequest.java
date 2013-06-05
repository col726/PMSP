package org.pmsp.domain;

import com.thoughtworks.xstream.annotations.XStreamAlias;

/*=========================Group/Course Information=========================
 * Group 1:  Adam Himes, Brian Huber, Colin McKenna, Josh Krupka
 * CS 544
 * Spring 2013
 * Drexel University
 * Final Project
 *==========================================================================*/

/**
 * Class for the login request.  Really only used for the type, at this time the only other attributes 
 * in the log in request are the user and password which are passed in the http header
 */
@XStreamAlias("LoginRequest")
public class LoginRequest extends RequestType {


}
