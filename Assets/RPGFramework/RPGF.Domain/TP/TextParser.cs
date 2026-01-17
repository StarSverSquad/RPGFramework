using RPGF.Domain.TP.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPGF.Domain.TP
{
    public class TextParserReturn
    {
        public TextParserTagMetadata[] Actions { get; private set; }

        public string ClearedText { get; private set; }

        public TextParserReturn(string clearedText, params TextParserTagMetadata[] actions)
        {
            ClearedText = clearedText;
            Actions = actions;
        }
    }

    public class TextParserTagMetadata
    {
        public enum TagType
        {
            Single, ScopedOpen, ScopedClose
        }

        public string Tag;

        public int Index;

        public TagType Type;

        public TextActionBase Action;

        public Dictionary<string, string> Params = new();
    }

    public class TextParser
    {
        private readonly Dictionary<TextActionBase, UseTextActionAttribute> _allowedActions;

        public TextParser(Dictionary<TextActionBase, UseTextActionAttribute> allowedActions)
        {
            _allowedActions = allowedActions;
        }

        public TextParserReturn ParseText(string originalText)
        {
            string text = originalText.Clone() as string;

            var single = new Regex(TextParserRegexHelper.SINGLE_ACTION_PATTERN, RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            var scopedOpen = new Regex(TextParserRegexHelper.SCOPED_ACTION_PATTERN_OPEN, RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            var scopedClose = new Regex(TextParserRegexHelper.SCOPED_ACTION_PATTERN_CLOSE, RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            var param = new Regex(TextParserRegexHelper.TAG_PARAM_ACTION_PATTERN, RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            var singleMatches = single.Matches(text);
            var scopedOpenMatches = scopedOpen.Matches(text);
            var scopedCloseMatches = scopedClose.Matches(text);

            var allMatches = new List<Match>();
            allMatches.AddRange(singleMatches);
            allMatches.AddRange(scopedOpenMatches);
            allMatches.AddRange(scopedCloseMatches);
            allMatches.Sort((a, b) => a.Index - b.Index);

            int indexCorrection = 0;

            var actionsMetas = new List<TextParserTagMetadata>();
            foreach (var match in allMatches)
            {
                var matchName = match.Groups[1].Name;

                bool isMajorTag =
                    matchName == TextParserRegexHelper.GroupNames.TagSingle ||
                    matchName == TextParserRegexHelper.GroupNames.TagScopedOpen;

                var tagNameText = match.Groups["tagName"].Value;

                var tagParams = new Dictionary<string, string>();
                if (isMajorTag)
                {
                    var tagParamsText = match.Groups.FirstOrDefault(i => i.Name == "params").Value;
                    if (tagParamsText != null)
                    {
                        var tagParamsMatches = param.Matches(tagParamsText);

                        foreach (Match paramMatch in tagParamsMatches)
                        {
                            var name = paramMatch.Groups["paramName"].Value;
                            var value = paramMatch.Groups["paramValue"].Value;

                            tagParams.Add(name, value);
                        }
                    }
                }

                var action = _allowedActions.FirstOrDefault((pair) => new Regex(pair.Value.TagPattern, RegexOptions.CultureInvariant).IsMatch(tagNameText)).Key;

                if (action is not null)
                {
                    actionsMetas.Add(new()
                    {
                        Action = action,
                        Params = tagParams,
                        Tag = tagNameText,
                        Index = match.Index - indexCorrection,
                        Type = matchName switch
                        {
                            TextParserRegexHelper.GroupNames.TagSingle => TextParserTagMetadata.TagType.Single,
                            TextParserRegexHelper.GroupNames.TagScopedOpen => TextParserTagMetadata.TagType.ScopedOpen,
                            TextParserRegexHelper.GroupNames.TagScopedClose => TextParserTagMetadata.TagType.ScopedClose,
                            _ => TextParserTagMetadata.TagType.Single
                        }
                    });

                    text = text.Remove(match.Index - indexCorrection, match.Length);
                    indexCorrection += match.Length;
                }
            }

            return new TextParserReturn(text, actionsMetas.ToArray());
        }
    }
}