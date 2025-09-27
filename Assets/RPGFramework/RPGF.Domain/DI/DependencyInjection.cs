using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace RPGF.Domain.DI
{
    public class DependencyInjection : Injectable
    {
        private List<Type> scopedTypes;

        private Dictionary<Type, Type> scopedWithImplimentTypes;

        private List<Injectable> signletons;

        public DependencyInjection()
        {
            scopedTypes = new List<Type>();
            scopedWithImplimentTypes = new Dictionary<Type, Type>();
            signletons = new List<Injectable>();
        }

        public void InjectInto(InjectionTarget target, params Injectable[] injectables)
        {
            var injectionFileds = target.GetType()
                                        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Default)
                                        .Where(f => f.GetCustomAttribute<InjectAttribute>() != null);

            foreach (var field in injectionFileds)
            {
                Injectable resultInject = null;

                resultInject = injectables.FirstOrDefault(inj => inj.GetType() == field.FieldType);

                resultInject ??= signletons.FirstOrDefault(inj => inj.GetType() == field.FieldType);

                if (resultInject == null)
                {
                    Type resultType = null;

                    resultType = scopedTypes.FirstOrDefault(t => t == field.FieldType);

                    resultType = scopedWithImplimentTypes.FirstOrDefault(dt => dt.Key == field.FieldType).Value;

                    if (resultType != null)
                    {
                        resultInject = Activator.CreateInstance(resultType) as Injectable;
                    }
                }

                if (resultInject == null)
                {
                    Debug.LogError($"Зависимость не найдена для поля {field.Name} в типе {target.GetType().Name}");
                    return;
                } 
            }
        }

        public void AddScoped<T>()
            where T : Injectable, new()
        {
            scopedTypes.Add(typeof(T));
        }

        public void AddScoped<I, T>()
            where I : Injectable
            where T : I, new()
        {
            scopedWithImplimentTypes.Add(typeof(I), typeof(T));
        }

        public void AddSignleton<T>(T value)
            where T : Injectable, new()
        {
            value ??= new T();

            signletons.Add(value);
        }
    }
}
