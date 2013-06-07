#!/bin/sh

#this script sends the login message to the server

echo "Starting request for $$"
curl  -s -H "PMSP-Version: 1.0"  -X POST --data-binary @../../test-xml/LoginRequest.xml --user josh:josh  localhost:31415
echo "Ending request for $$"
