cmake_minimum_required(VERSION 3.0.0)
project(TV_TetShared VERSION 0.1.0)


set(SRC_ROOT_DIR ../../../..)   # tetavi_main root dir
set(TV_TetFileCodec_DIR ${SRC_ROOT_DIR}/TV_Infrastructure/Tier1.0/TV_TetFileCodec)
set(PROJ_SRC_DIR ${TV_TetFileCodec_DIR}/TV_TetShared)


set(PROJ_SRCS
    ${PROJ_SRC_DIR}/TV_TetModel.cpp
    ${PROJ_SRC_DIR}/TV_TetTexture.cpp
)

add_library(${PROJECT_NAME} STATIC ${PROJ_SRCS})

set(CPACK_PROJECT_NAME ${PROJECT_NAME})
set(CPACK_PROJECT_VERSION ${PROJECT_VERSION})
include(CPack)

