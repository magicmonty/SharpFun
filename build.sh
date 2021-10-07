#!/usr/bin/env bash

bash --version 2>&1 | head -n 1

set -eo pipefail
SCRIPT_DIR=$(cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd)

###########################################################################
# CONFIGURATION
###########################################################################

BUILD_PROJECT_FILE="$SCRIPT_DIR/build/_build.csproj"
TEMP_DIRECTORY="$SCRIPT_DIR//.nuke/temp"

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_MULTILEVEL_LOOKUP=0

###########################################################################
# EXECUTION
###########################################################################

function FirstJsonValue {
    perl -nle 'print $1 if m{"'"$1"'": "([^"]+)",?}' <<< "${@:2}"
}

dotnet build "$BUILD_PROJECT_FILE" /nodeReuse:false /p:UseSharedCompilation=false -nologo -clp:NoSummary --verbosity quiet
dotnet run --project "$BUILD_PROJECT_FILE" --no-build -- "$@"
