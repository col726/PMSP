#!/bin/sh

pid=`ps -ef | grep "pmsp.server=true" | grep -v grep |  awk '{ print $2 }'`

kill $pid

echo "Server stopped"
