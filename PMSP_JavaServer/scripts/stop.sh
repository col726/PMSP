#!/bin/sh

# Look for a process with our special string in the process signature
pid=`ps -ef | grep "pmsp.server=true" | grep -v grep |  awk '{ print $2 }'`

if [ -n "$pid" ]; then
    # Kill it. App has a shutdown hook which will run when it gets the kills signal
    kill $pid
    echo "Server stopped"
else
  echo "Server was not running"
fi


