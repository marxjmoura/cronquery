#!/bin/bash

set -e

basedir="$(dirname $0)/.."
solution_dir="$basedir/src"
api_project_dir="CronQuery.API"
test_project_dir="CronQuery.Tests"

cd $solution_dir/$test_project_dir

echo -n 'Inotify Watchers: '
sudo sysctl -n -w fs.inotify.max_user_instances=1024
echo ''

dotnet test \
  /p:AltCover="true" \
  /p:AltCoverForce="true" \
  /p:AltCoverThreshold="80" \
  /p:AltCoverOpenCover="true" \
  /p:AltCoverXmlReport="coverage/opencover.xml" \
  /p:AltCoverInputDirectory="$api_project_dir" \
  /p:AltCoverAttributeFilter="ExcludeFromCodeCoverage" \
  /p:AltCoverAssemblyExcludeFilter="System(.*)|xunit|$test_project_dir|$api_project_dir.Views"

dotnet reportgenerator \
  "-reports:coverage/opencover.xml" \
  "-reporttypes:Html;HtmlSummary" \
  "-targetdir:coverage/report"
