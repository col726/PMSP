#!/bin/sh

# Runs through all the basic interactions.  Assumes josh is not logged in
curl -v -H "PMSP-Version: 1.0" -X POST  --data-binary @../../test-xml/LoginRequest.xml  --user josh:josh localhost:31415
curl -v -H "PMSP-Version: 1.0"  -b "pmsp-sessionid=josh;pmsp-state=idle"  -X POST --data-binary @../../test-xml/MetadataListRequest-Album.xml localhost:31415
curl -v -H "PMSP-Version: 1.0"  -b "pmsp-sessionid=josh;pmsp-state=wait.list.choice"  -X POST --data-binary @../../test-xml/FileListRequest.xml localhost:31415
curl -v -H "PMSP-Version: 1.0"  -b "pmsp-sessionid=josh;pmsp-state=wait.file.choice" -o output.`echo $$`.xml  -X POST --data-binary @../../test-xml/RetrievalRequest.xml  localhost:31415
curl -v -H "PMSP-Version: 1.0"  -b "pmsp-sessionid=josh;pmsp-state=idle" -X POST  --data-binary @../../test-xml/LogoffRequest.xml  localhost:31415
