using RPGF.Core.TextEffecter.Abstractions;
using RPGF.Core.TextWriter.Abstractions;
using RPGF.Domain.TP;
using RPGF.Domain.TP.Abstractions;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace RPGF.Core.TextWriter.Actions
{
    [UseTextAction("^effect$", TextActionType.Scoped)]
    public class TextEffectAction : TextWriterActionBase
    {
        /// <summary>
        /// pending params: 
        ///     type: [name]
        ///     
        ///     names: ["shake", "wooble"]
        /// </summary>
        /// <returns></returns>
        public override IEnumerator Action(TextActionParams @params)
        {
            var effectTypes = GetType().Assembly.GetTypes()
                .Where(type => type.BaseType == typeof(TextEffectBase) && type.GetCustomAttribute<UseTextEffectAttribute>() is not null && !type.IsAbstract)
                .ToDictionary(type => type.GetCustomAttribute<UseTextEffectAttribute>());

            if (@params.TagParams.TryGetValue("type", out var effectTypeName))
            {
                var effectTypeMeta = effectTypes.Keys.FirstOrDefault(key => key.CodeName == effectTypeName);

                if (effectTypeMeta != null && effectTypes.TryGetValue(effectTypeMeta, out var effectType))
                {
                    var effect = Activator.CreateInstance(effectType, new object[]{ TextWriter.TextMeshPro });

                    TextWriter.AddTextEffect(effect as TextEffectBase, @params.StartIndex, @params.EndIndex);
                }
            }

            yield break;
        }
    }
}
