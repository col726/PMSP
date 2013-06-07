#!/bin/sh

#calls the test long script 10 times in parallel

for i in {1..10}
do
   ./test-login.sh &
done
