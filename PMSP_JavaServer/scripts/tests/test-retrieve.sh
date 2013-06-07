#!/bin/sh

# This script uses curl to interact with the server

echo "Starting request for $$"
curl -s -H "PMSP-Version: 1.0"  -b "pmsp-sessionid=josh;pmsp-state=wait.file.choice" -o output.`echo $$` -X POST --data-binary @../../test-xml/RetrievalRequest.xml  localhost:31415
echo "Ending request for $$"
