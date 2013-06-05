#!/bin/sh

#Need to change this if you want to be able to run the script from anywhere other than the bin dir!
PMSP_DIR=..

cd $PMSP_DIR

nohup java -Xmx1024m -classpath "./lib/*" -Dpmsp.server=true  org.pmsp.MediaServer </dev/null >server.out 2>&1 &

sleep 1

pid=`ps -ef | grep "pmsp.server=true" | grep -v grep |  awk '{ print $2 }'`

if [ -n "$pid" ]; then
  echo "Server started "
else
  echo "Server failed to start"
fi


