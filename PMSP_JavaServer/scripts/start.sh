#!/bin/sh

#Need to change this if you want to be able to run the script from anywhere other than the scripts dir!
PMSP_DIR=..

cd $PMSP_DIR

# Run the app
# Added pmsp.server flag to give us something unique to look for in our grep/kill shutdown process
nohup java -Xmx1024m -classpath "./dist/*:./lib/*" -Dpmsp.server=true  org.pmsp.MediaServer </dev/null >server.out 2>&1 &

sleep 1

pid=`ps -ef | grep "pmsp.server=true" | grep -v grep |  awk '{ print $2 }'`

if [ -n "$pid" ]; then
  echo "Server started "
else
  echo "Server failed to start"
fi


