#!/bin/sh

# This script uses curl to interact with the server

echo "Starting request for $$"
# Can change -s to -v if you want to see all the data in stdout. The (large) xml file will be writeen to ./output.$$.xml
curl -s -H "PMSP-Version: 1.0"  -b "pmsp-sessionid=josh;pmsp-state=wait.file.choice" -o output.`echo $$`.xml -X POST --data-binary @../../test-xml/RetrievalRequest.xml  localhost:31415
echo "Ending request for $$"
