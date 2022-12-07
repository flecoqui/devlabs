import os
from datetime import datetime
from typing import List

from fastapi import APIRouter, Body, FastAPI
from fastapi.params import Depends
from starlette.requests import Request

from shared_code.configuration_service import ConfigurationService
from shared_code.log_service import LogService

router = APIRouter(prefix="")

app_version = os.getenv("APP_VERSION", "1.0.0.1")

app = FastAPI(
    title="fastapi REST API",
    description="Sample Fastapi REST API.",
    version=app_version,
)


def get_log_service() -> LogService:
    """Getting a single instance of the LogService"""
    return LogService()


def get_configuration_service() -> ConfigurationService:
    """Getting a single instance of the LogService"""
    return ConfigurationService()


@router.get(
    "/version",
    responses={
        200: {"description": "Get version."},
    },
    summary="Returns the current version.",
)
def get_version(
    request: Request,
) -> str:
    """Get Version using GET /version"""
    app_version = os.getenv("APP_VERSION", "1.0.0.1")
    return app_version


@router.get(
    "/time",
    responses={
        200: {"description": "Get current time."},
    },
    summary="Returns the current time.",
)
def get_time(
    request: Request,
) -> str:
    """Get UTC Time using GET /time"""
    now = datetime.utcnow()
    return now.strftime("%Y/%m/%d-%H:%M:%S")



app.include_router(router, prefix="")
