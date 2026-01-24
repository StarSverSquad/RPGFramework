namespace RPGF.Domain.TP.Models
{
    public class TextParserReturn
    {
        public TextParserTag[] Tags { get; set; }

        public string ClearedText { get; set; }
        public string ClearedTextWithoutShadows { get; set; }

        public TextParserShadowTag[] Shadows { get; set; }
    }
}
