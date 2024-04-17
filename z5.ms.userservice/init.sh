#!/bin/bash

echo Copying over configuration.
cp -r /Config/** /app/

EXIT_CODE=$?
if [ $EXIT_CODE -neq 0 ]
then
	echo There was a failure! Container will now exit after a small delay.
	echo Exit code $EXIT_CODE

	# Avoid too crazy of a restart loop if we get stuck in one.
	sleep 30

	exit $EXIT_CODE
fi

echo Starting User Service API.

cd /app
exec dotnet z5.ms.userservice.dll