using RPGF.Domain.TP.Abstractions;
using System.Collections.Generic;

namespace RPGF.Domain.TP.Models
{
    public class TextParserTag
    {
        public enum TagType
        {
            Single, ScopedOpen, ScopedClose
        }

        public string Tag;

        public int Index;
        public int RealIndex;

        public TagType Type;

        public TextActionBase Action;

        public Dictionary<string, string> Params = new();
    }
}
