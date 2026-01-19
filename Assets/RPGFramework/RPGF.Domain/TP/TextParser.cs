using RPGF.Domain.TP.Abstractions;
using RPGF.Domain.TP.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RPGF.Domain.TP
{
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

            var tags = new List<TextParserTag>();
            var shadows = new List<TextParserShadowTag>();
            foreach (var match in allMatches)
            {
                var matchName = match.Groups[1].Name;

                bool isMajorTag =
                    matchName == TextParserRegexHelper.GroupNames.TagSingle ||
                    matchName == TextParserRegexHelper.GroupNames.TagScopedOpen;

                var tagNameText = match.Groups[TextParserRegexHelper.GroupNames.TagName].Value;

                var tagParams = new Dictionary<string, string>();
                if (isMajorTag)
                {
                    var tagParamsText = match.Groups.FirstOrDefault(i => i.Name == TextParserRegexHelper.GroupNames.Params).Value;
                    if (tagParamsText != null)
                    {
                        var tagParamsMatches = param.Matches(tagParamsText);

                        foreach (Match paramMatch in tagParamsMatches)
                        {
                            var name = paramMatch.Groups[TextParserRegexHelper.GroupNames.ParamName].Value;
                            var value = paramMatch.Groups[TextParserRegexHelper.GroupNames.ParamValue].Value;

                            tagParams.Add(name, value);
                        }
                    }
                }

                var action = _allowedActions.FirstOrDefault((pair) => new Regex(pair.Value.TagPattern, RegexOptions.CultureInvariant).IsMatch(tagNameText)).Key;

                if (action is not null)
                {
                    tags.Add(new()
                    {
                        Action = action,
                        Params = tagParams,
                        Tag = tagNameText,
                        Index = match.Index - indexCorrection,
                        Type = matchName switch
                        {
                            TextParserRegexHelper.GroupNames.TagSingle => TextParserTag.TagType.Single,
                            TextParserRegexHelper.GroupNames.TagScopedOpen => TextParserTag.TagType.ScopedOpen,
                            TextParserRegexHelper.GroupNames.TagScopedClose => TextParserTag.TagType.ScopedClose,
                            _ => TextParserTag.TagType.Single
                        }
                    });

                    text = text.Remove(match.Index - indexCorrection, match.Length);
                    indexCorrection += match.Length;
                }
                else
                {
                    shadows.Add(new()
                    {
                        Index = match.Index - indexCorrection,
                        Tag = tagNameText,
                        Lenght = match.Length
                    });
                }
            }

            foreach (var tag in tags)
            {
                var preShadows = shadows.Where(s => s.Index < tag.Index);

                tag.RealIndex = tag.Index;
                foreach (var shadow in preShadows)
                {
                    tag.RealIndex -= shadow.Lenght;
                }
            }

            indexCorrection = 0;
            string textWitoutShadows = text.Clone() as string;
            foreach (var shadow in shadows)
            {
                textWitoutShadows = textWitoutShadows.Remove(shadow.Index - indexCorrection, shadow.Lenght);
                indexCorrection += shadow.Lenght;
            }

            return new TextParserReturn() 
            { 
                ClearedText = text,
                ClearedTextWithoutShadows = textWitoutShadows,
                Tags = tags.ToArray(),
                Shadows = shadows.ToArray()
            };
        }
    }
}