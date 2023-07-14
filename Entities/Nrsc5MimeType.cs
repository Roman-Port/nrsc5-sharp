﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nrsc5Sharp.Entities
{
    public enum Nrsc5MimeType : uint
    {
        NRSC5_MIME_PRIMARY_IMAGE = 0xBE4B7536,
        NRSC5_MIME_STATION_LOGO = 0xD9C72536,
        NRSC5_MIME_NAVTEQ = 0x2D42AC3E,
        NRSC5_MIME_HERE_TPEG = 0x82F03DFC,
        NRSC5_MIME_HERE_IMAGE = 0xB7F03DFC,
        NRSC5_MIME_HD_TMC = 0xEECB55B6,
        NRSC5_MIME_HDC = 0x4DC66C5A,
        NRSC5_MIME_TEXT = 0xBB492AAC,
        NRSC5_MIME_JPEG = 0x1E653E9C,
        NRSC5_MIME_PNG = 0x4F328CA0,
        NRSC5_MIME_TTN_TPEG_1 = 0xB39EBEB2,
        NRSC5_MIME_TTN_TPEG_2 = 0x4EB03469,
        NRSC5_MIME_TTN_TPEG_3 = 0x52103469,
        NRSC5_MIME_TTN_STM_TRAFFIC = 0xFF8422D7,
        NRSC5_MIME_TTN_STM_WEATHER = 0xEF042E96
    }
}