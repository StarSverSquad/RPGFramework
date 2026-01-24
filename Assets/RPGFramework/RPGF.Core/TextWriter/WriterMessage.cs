using System;
using UnityEngine;

namespace RPGF.Core.TextWriter
{
    [Serializable]
    public class WriterMessage
    {
        [Multiline]
        public string text;
        public float speed;

        public bool clear;
        public bool wait;

        public WriterMessage()
        {
            text = string.Empty;
            speed = 0;

            clear = true;
            wait = true;
        }
    }
}
