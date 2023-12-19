﻿using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class BattlePatternManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> patternObjects = new List<GameObject>();
    [SerializeField]
    private List<RPGAttackPattern> patterns = new List<RPGAttackPattern>();

    private Coroutine attackCoroutine = null;
    public bool IsAttack => attackCoroutine != null;

    /// <summary>
    /// Добовляет паттерн к списку используемых патернов
    /// </summary>
    /// <param name="pattern">Паттерн</param>
    public void AddPattern(RPGAttackPattern pattern)
    {
        // Устанавливает паттерну его владельца
        GameObject ptrobj = Instantiate(pattern.gameObject, transform, false);
        RPGAttackPattern ptr = ptrobj.GetComponent<RPGAttackPattern>();

        patterns.Add(ptr);
    }

    /// <summary>
    /// Запускает паттерны
    /// </summary>
    /// <param name="tiny">Использовать малые паттерны или нет</param>
    public void Invoke(bool tiny)
    {
        transform.position = (Vector2)Camera.main.transform.position;

        foreach (var item in patterns)
            item.Invoke(tiny);

        attackCoroutine = StartCoroutine(AttackCoroutine());
    }

    /// <summary>
    /// Прерывает работу паттернов
    /// </summary>
    public void Break()
    {
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;

        CleanUp();
    }

    /// <summary>
    /// Создаёт объект паттерна
    /// </summary>
    /// <param name="obj">Объект</param>
    /// <param name="offset">Отступ от центра</param>
    public GameObject CreatePatternObject(GameObject obj, Vector2 offset)
    {
        GameObject o = Instantiate(obj, (Vector2)transform.position + offset, Quaternion.identity, transform);

        patternObjects.Add(o);

        return o;
    }

    /// <summary>
    /// Подчищает этот класс
    /// </summary>
    public void CleanUp()
    {
        foreach (GameObject o in patternObjects)
        {
            if (o != null)
                Destroy(o);
        }            
        patternObjects.Clear();

        foreach (RPGAttackPattern o in patterns)
            Destroy(o.gameObject);
        patterns.Clear();
    }

    /// <summary>
    /// Курутина отсветсвенная за работу паттернов атаки
    /// </summary>
    private IEnumerator AttackCoroutine()
    {
        // Если паттернов больше 0
        if (patterns.Count > 0)
        {
            // Выбор длительности атаки (по самому долгому паттерну)
            float time = patterns.Max(i => i.PatternTime);

            // Ждём время
            yield return new WaitForSeconds(time);

            // Ждём пока некоторые паттерны сигнализируют о завершение (нужно лишь для патеррнов с WaitEnd == true)
            if (patterns.Where(i => i.WaitEnd).Count() > 0)
                yield return new WaitUntil(() => patterns.Where(i => i.WaitEnd).All(i => i.IsWorking == false));
        }
        else
            yield return new WaitForSeconds(2f);

        // Очистка
        CleanUp();

        // Обнуление
        attackCoroutine = null;
    }
}