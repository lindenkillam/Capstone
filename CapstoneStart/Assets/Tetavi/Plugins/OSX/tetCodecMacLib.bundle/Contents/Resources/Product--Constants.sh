#!/bin/sh
#  constants.sh
#
#  Created by Arkadi Yoskovitz on 9/14/20.
#  
# ±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±

# Globally defined constants
# ######################################################################################################################
PRODUCT_NAME="TetaVI";
SCHEME_TO_USE="tetCodecMacLib";

WORKSPACE_NAME="tetCodecApple";
WORKSPACE_DIR_="${PROJECT_DIR}/..";
WORKSPACE_PATH="${WORKSPACE_DIR_}/${WORKSPACE_NAME}.xcworkspace";

PRODUCT_DIR_PATH="${PROJECT_DIR}/../Products";
PRODUCT_FILE_TYPE="bundle";
PRODUCT_FILE_NAME="${SCHEME_TO_USE}.${PRODUCT_FILE_TYPE}";

PRODUCT_CONFIGURATION="Release";
PRODUCT_ORIGIN_DIR="${BUILD_DIR}/${PRODUCT_CONFIGURATION}";
PRODUCT_TARGET_DIR="${PRODUCT_DIR_PATH}";

CODE_SIGNING_CERTIFICATE_NAME="Apple Development: tetaviltd@gmail.com (5GD55FH3L4)"

#
########################################################################################################################
SCRIPT_BUNDLE_ID="MyPlugin"
SCRIPT_USERNAME="john.doe@apple.com"
SCRIPT_PASSWORD="@keychain:Software Notarization"
