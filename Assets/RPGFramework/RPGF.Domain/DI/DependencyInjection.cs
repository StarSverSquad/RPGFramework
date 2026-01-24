using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RPGF.Domain.DI
{
    public class DependencyInjection : Injectable
    {
        private List<Type> scopedTypes;
        private Dictionary<Type, Type> scopedWithImplimentTypes;

        private List<Injectable> signletons;

        private List<DependencyInjection> subInjectors;

        public DependencyInjection()
        {
            scopedTypes = new List<Type>();
            scopedWithImplimentTypes = new Dictionary<Type, Type>();
            signletons = new List<Injectable>();
            subInjectors = new List<DependencyInjection>();
        }

        public void InjectInto(InjectionTarget target, params Injectable[] injectables)
        {
            var injectionFileds = target.GetType()
                                        .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Default)
                                        .Where(f => f.GetCustomAttribute<InjectAttribute>() != null
                                                    && f.IsPrivate && f.IsInitOnly);

            foreach (var field in injectionFileds)
            {
                if (field.GetValue(target) is not null)
                    continue;

                var resultInject = ResolveDependency(field, injectables);

                if (resultInject is null)
                {
                    foreach (var injector in subInjectors)
                    {
                        resultInject = injector.ResolveDependency(field);

                        if (resultInject is not null)
                            break;
                    }

                    if (resultInject is null)
                        throw new ApplicationException($"Зависимость не найдена для поля {field.Name} в типе {target.GetType().Name}");
                }

                if (resultInject is InjectionTarget subTarget)
                {
                    InjectInto(subTarget);
                }

                field.SetValue(target, resultInject);
            }
        }
        public Injectable ResolveDependency(FieldInfo target, params Injectable[] injectables)
        {
            Injectable resultInject = null;

            resultInject = injectables.FirstOrDefault(inj => inj.GetType() == target.FieldType);

            resultInject ??= signletons.FirstOrDefault(inj => inj.GetType() == target.FieldType);

            if (resultInject is null)
            {
                Type resultType = null;

                resultType = scopedTypes.FirstOrDefault(t => t == target.FieldType);

                resultType ??= scopedWithImplimentTypes.FirstOrDefault(dt => dt.Key == target.FieldType).Value;

                if (resultType is not null)
                    resultInject = Activator.CreateInstance(resultType) as Injectable;
            }

            return resultInject;
        }

        public void AddSubInjector(DependencyInjection injector)
        {
            subInjectors.Add(injector);
        }

        public void AddScoped<T>()
            where T : class, Injectable
        {
            scopedTypes.Add(typeof(T));
        }

        public void AddScoped<I, T>()
            where I : Injectable
            where T : class, I
        {
            scopedWithImplimentTypes.Add(typeof(I), typeof(T));
        }

        public void AddSignleton<T>(T value)
            where T : class, Injectable
        {
            if (value is InjectionTarget target)
                InjectInto(target);

            signletons.Add(value);
        }

        public void RemoveSingleton<T>(T value)
            where T : class, Injectable
        {
            if (signletons.Contains(value))
                signletons.Remove(value);
        }

        public T CreateSingleton<T>()
            where T : class, Injectable
        {
            T value = Activator.CreateInstance<T>();

            if (value is InjectionTarget target)
                InjectInto(target);

            AddSignleton(value);

            return value;
        }
    }
}
