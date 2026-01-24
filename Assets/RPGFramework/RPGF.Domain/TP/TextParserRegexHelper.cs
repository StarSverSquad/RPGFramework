using UnityEngine.UI;

namespace RPGF.Domain.TP
{
    public static class TextParserRegexHelper
    {
        public static class GroupNames
        {
            public const string TagSingle = "tagSingle";
            public const string TagScopedOpen = "tagScopedOpen";
            public const string TagScopedClose = "tagScopedClose";

            public const string TagName = "tagName";
            public const string Params = "params";

            public const string Param = "param";
            public const string ParamName = "paramName";
            public const string ParamValue = "paramValue";
        }

        public const string SINGLE_ACTION_PATTERN =
            @"
            (?<tagSingle>
             <(?<tagName>(?:\w|[|!?*^#@%.:$&\\=""_-])+)
              (?<params>\s(?:\w|\s|[|!?*(){}\]\[^#@%.:$&=""])+)*
             />
            )
            ";

        public const string SCOPED_ACTION_PATTERN_OPEN =
            @"
            (?<tagScopedOpen>
             <(?<tagName>(?:\w|[|!?*^#@%.:$&\\=""_-])+)
              (?<params>\s(?:\w|\s|[|!?*(){}\]\[^#@%.:$&=""])+)*
             >
            )
            ";

        public const string SCOPED_ACTION_PATTERN_CLOSE =
            @"
            (?<tagScopedClose>
             </(?<tagName>(?:\w|[|!?*^#@%.:$&\\=""_-])+)>
            )
            ";

        public const string TAG_PARAM_ACTION_PATTERN =
            @"
            (?<param>
             (?<paramName>(?:\w|[|!?*^#@%.:$&])+)
             =
             ""(?<paramValue>(?:\w|\s|[|!?*(){}\]\[^#@%.:$&=])*)""
            )
            ";
    }
}
