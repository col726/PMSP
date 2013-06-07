#!/bin/sh
# This script creates 10 concurrent retrieval requests (have to be logged in for this to work)

for i in {1..10}
do
   #./test-retrieve.sh &> /dev/null &
   ./test-retrieve.sh &
done
