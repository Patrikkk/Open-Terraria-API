﻿using System;

namespace SteelSeries.GameSense
{
    public class QueueMsgRegisterEvent : QueueMsg
    {
        public override Uri uri { get; }
        public override object data { get; set; }

        public override bool IsCritical() => false;

        public static Uri _uri;
    }
}
