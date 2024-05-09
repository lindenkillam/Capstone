#!/bin/sh

#  XCFramework.sh
#
#  Created by Arkadi Yoskovitz on 9/14/20.
#  
########################################################################################################################
source $SRCROOT/Build-Scripts/Product--Constants.sh

# ±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±
# Main function
# ±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±
function main() {
    # Stop the script if we already have a prepared bundle
    if [[ "true" == ${ALREADY_INVOKED_xcframework:-false} ]]; then
        echo "RECURSION: Detected, stopping"
    else
        export ALREADY_INVOKED_xcframework="true";
        set -e

        echo " # Pre-luanch information dump" ;
        echo " ####################################################################################################### "
        echo " 01: ▸ WORKSPACE_PATH         : ${WORKSPACE_PATH}" ;
        echo " 02: ▸ SCHEME_TO_USE          : ${SCHEME_TO_USE}"  ;
        echo " 03: ▸ PRODUCT_NAME           : ${PRODUCT_NAME}"   ;

        echo " ####################################################################################################### "
        echo " 04: ▸ PRODUCT_CONFIGURATION  : ${PRODUCT_CONFIGURATION}" ;
        echo " 05: ▸ PRODUCT_ORIGIN_DIR     : ${PRODUCT_ORIGIN_DIR}" ;
        echo " 06: ▸ PRODUCT_TARGET_DIR     : ${PRODUCT_TARGET_DIR}" ;

        PRODUCT_ORIGIN_PATH="${PRODUCT_ORIGIN_DIR}/${PRODUCT_FILE_NAME}/" ;
        PRODUCT_TARGET_PATH="${PRODUCT_TARGET_DIR}/${PRODUCT_FILE_NAME}/" ;
        PRODUCT_TARGET_NAME="${PRODUCT_TARGET_DIR}/${PRODUCT_FILE_NAME}"  ;
        echo " ####################################################################################################### "
        echo " 07: ▸ PRODUCT_ORIGIN_PATH    : ${PRODUCT_ORIGIN_PATH}" ;
        echo " 08: ▸ PRODUCT_TARGET_PATH    : ${PRODUCT_TARGET_PATH}" ;
        echo " 09: ▸ PRODUCT_TARGET_NAME    : ${PRODUCT_TARGET_NAME}" ;

        echo " ####################################################################################################### "
        echo " "
        echo " ▸▸ Running script: ${PRODUCT_FILE_NAME} creation and notarization" ;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 01 - Create output && helper directories if needed, the output directory: ${PRODUCT_DIR_PATH}"
        echo " ======================================================================================================= "
        echo " ▸ "
        recreate_directory "${PRODUCT_TARGET_DIR}"  ;
        recreate_directory "${PRODUCT_TARGET_PATH}" ;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 02 - Building mac bundle for:: Product-\"$PRODUCT_NAME\", Scheme-\"$SCHEME_TO_USE\""
        echo " ======================================================================================================= "
        echo " ▸ "
        local SCRIPT_DESTENATION="platform=OS X" ;
        local SCRIPT_SDK="macosx" ;
        perform_build ${WORKSPACE_PATH} ${SCHEME_TO_USE} ${PRODUCT_CONFIGURATION} "${SCRIPT_DESTENATION}" ${SCRIPT_SDK};
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 03 - Prepare files ${PRODUCT_FILE_NAME} for notarization"
        echo " ======================================================================================================= "
        echo " ▸ "
        copy_content  $PRODUCT_ORIGIN_PATH $PRODUCT_TARGET_PATH ;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 04 - Removing resource forks for ${PRODUCT_FILE_NAME}"
        echo " ======================================================================================================= "
        echo " ▸ "
        xattr -cr $PRODUCT_TARGET_PATH ;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 05 - Code-signing ${PRODUCT_FILE_NAME} with ${CODE_SIGNING_CERTIFICATE_NAME}"
        echo " ======================================================================================================= "
        echo " ▸ "
        codesign --deep --verbose --timestamp --options=runtime -s "Developer ID Application: Tetavi LTD (U4LR963H5Y)" -f $PRODUCT_TARGET_PATH ;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 06 - Archive ${PRODUCT_FILE_NAME} for notarization"
        echo " ======================================================================================================= "
        echo " ▸ "
        local FILE_NOTARIAZTION_ORIGIN="${PRODUCT_TARGET_NAME}" ;
        local FILE_NOTARIAZTION_TARGET="${PRODUCT_TARGET_NAME}.notarize.zip" ;
        echo " ▸ FILE_NOTARIAZTION_ORIGIN: ${FILE_NOTARIAZTION_ORIGIN}" ;
        echo " ▸ FILE_NOTARIAZTION_TARGET: ${FILE_NOTARIAZTION_TARGET}" ;
        ditto -c -k --keepParent --rsrc $FILE_NOTARIAZTION_ORIGIN $FILE_NOTARIAZTION_TARGET;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ 06 - Upload Notarization Archive ${PRODUCT_FILE_NAME} for notarization $FILE_NOTARIAZTION_TARGET"
        echo " ======================================================================================================= "
        echo " ▸ "
       # xcrun altool --notarize-app -f $FILE_NOTARIAZTION_TARGET --primary-bundle-id $SCRIPT_BUNDLE_ID --username $SCRIPT_USERNAME --password $SCRIPT_PASSWORD --asc-provider "AQ7MHK25HL"
                
         xcrun altool --notarize-app -f $FILE_NOTARIAZTION_TARGET --primary-bundle-id "com.TetaviLTD.tetCodecMacLib" --username "tetaviltd@gmail.com" --password "ygvf-hked-rkqs-ifmp" --output-format "xml" #--asc-provider "AQ7MHK25HL"
            
        #    xcrun altool --notarization-info --username "tetaviltd@gmail.com" --password "ygvf-hked-rkqs-ifmp" --output-format "xml"
        # SOME INFO NOTES:
        # =================
        # The username is your developer account email.
        # The asc-provider is your ten digit Team ID. If you are only a member in a single team you do not need to provide this.
        # The password uses a special @keychain: keyword that tells altool to get the app-specific password out of a keychain item named Developer-altool. (Remember we created that earlier?)
        
        # NOTE You'll need to activate the scrip above and remove the exit command for this script to work.
       #  exit 9;
        echo " "
        echo " ======================================================================================================= "
        echo " ▸ Finished ${PRODUCT_FILE_NAME} creation and notarization script"
        echo " ======================================================================================================= "
    fi
}
function perform_build     {

    local xcworkspace=${1};
    local scheme_name=${2};

    local configuration=${3};
    
    local destenation=${4};
    
    local sdk_type=${5};
    
    xcodebuild \
        -workspace $xcworkspace \
        -scheme $scheme_name \
        -configuration $configuration \
        -destination "${destenation}" \
        -sdk $sdk_type \
        -UseModernBuildSystem=YES \
        -parallelizeTargets=NO \
        -derivedDataPath ../DerivedData/TetaVIDevelopment \
        OBJROOT="${OBJROOT}/DependentBuilds" \
        BUILD_DIR="${BUILD_DIR}" \
        BUILD_ROOT="${BUILD_ROOT}" \
        MODULE_CACHE_DIR="${MODULE_CACHE_DIR}" \
        ONLY_ACTIVE_ARCH=NO \
        clean \
        build
}
# ±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±
# Global function definitions
# ######################################################################################################################
function recreate_directory() {
    echo " ▸ Re-creating directory: ${1}"
    rm -rf "${1}"
    mkdir -p "${1}"
}
# ======================================================================================================================
function copy_content() {
    echo " ▸ Copy content to: ${2}"
    local SOURCE="${1}"
    local TARGET="${2}"
    cp -r $SOURCE $TARGET
}
# Perform commands
# ######################################################################################################################
function perform_archive     {
    
    local xcworkspace=${1}
    local scheme_name=${2}
    local archive_path=${3}

    local config="Release"

    local destenation_simulator="generic/platform=iOS Simulator"
    local destenation_iosdevice="generic/platform=iOS"

    local sdk_simulator="iphonesimulator"
    local sdk_iosdevice="iphoneos"

    local archive_simulator="$(archive_path_simulator $archive_path $scheme_name)"
    local archive_iosdevice="$(archive_path_iosdevice $archive_path $scheme_name)"

    echo " Simulator "
    echo " ▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸ "
    archive_action $xcworkspace $scheme_name $config "${destenation_simulator}" $sdk_simulator $archive_simulator YES NO

    echo " Device "
    echo " ▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸ "
    archive_action $xcworkspace $scheme_name $config "${destenation_iosdevice}" $sdk_iosdevice $archive_iosdevice YES NO

    echo " "
    echo " ▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸ "
    echo " ▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸ global perform_archive task finished ▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸ "
    echo " ▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸▸ "
    echo " "
}
# Activation
# ±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±±
main
