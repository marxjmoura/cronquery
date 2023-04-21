#!/bin/bash

set -e

basedir="$(dirname $0)/.."
solution_dir="$basedir/src"
api_project_dir="CronQuery.API"
test_project_dir="CronQuery.Tests"

cd $solution_dir/$test_project_dir

dotnet test \
  /p:AltCover="true" \
  /p:AltCoverForce="true" \
  /p:AltCoverThreshold="80" \
  /p:AltCoverOpenCover="true" \
  /p:AltCoverReport="coverage/opencover.xml" \
  /p:AltCoverInputDirectory="$api_project_dir" \
  /p:AltCoverAttributeFilter="ExcludeFromCodeCoverage" \
  /p:AltCoverAssemblyExcludeFilter="^(?!(CronQuery)).*$|CronQuery.Tests"

dotnet reportgenerator \
  "-reports:coverage/opencover.xml" \
  "-reporttypes:Html;HtmlSummary" \
  "-targetdir:coverage/report"
