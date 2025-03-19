using DG.Tweening;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class AnimationHelper
{
    #region One constant

    public static IEnumerator MoveToByTime(float from, float to, float time, Action<float> OnChangeCallback, Action OnEndCallback = null)
    {
        float dif = to - from;

        float speed = dif / time;

        float curtime = time;
        float curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos += speed * Time.fixedDeltaTime;

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    public static IEnumerator MoveToBySpeed(float from, float to, float speed, Action<float> OnChangeCallback, Action OnEndCallback = null)
    {
        float dif = to - from;

        float time = dif / speed;

        float curtime = time;
        float curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Mathf.MoveTowards(curpos, to, speed * Time.fixedDeltaTime);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    public static IEnumerator MoveToByTimeLerp(float from, float to, float time, Action<float> OnChangeCallback, Action OnEndCallback = null)
    {
        float dif = to - from;

        float speed = Mathf.Abs(dif / time);

        float curtime = time;
        float curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Mathf.Lerp(curpos, to, speed);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    public static IEnumerator MoveToBySpeedLerp(float from, float to, float speed, Action<float> OnChangeCallback, Action OnEndCallback = null)
    {
        float dif = to - from;

        float time = dif / speed;

        float curtime = time;
        float curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Mathf.Lerp(curpos, to, speed);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    #endregion

    #region Vector2

    public static IEnumerator MoveToByTime2D(Vector2 from, Vector2 to, float time, Action<Vector2> OnChangeCallback, Action OnEndCallback = null)
    {
        Vector2 dif = to - from;

        float speed = dif.magnitude / time;

        float curtime = time;
        Vector2 curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Vector2.MoveTowards(curpos, to, speed * Time.fixedDeltaTime);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    public static IEnumerator MoveToBySpeed2D(Vector2 from, Vector2 to, float speed, Action<Vector2> OnChangeCallback, Action OnEndCallback = null)
    {
        Vector2 dif = to - from;

        float time = dif.magnitude / speed;

        float curtime = time;
        Vector2 curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Vector2.MoveTowards(curpos, to, speed * Time.fixedDeltaTime);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    #endregion

    #region Color

    public static IEnumerator ColorByTime(Color from, Color to, float time, Action<Color> OnChangeCallback, Action OnEndCallback = null)
    {
        Color dif = to - from;

        float speed = ((Vector4)dif).magnitude / time;

        float curtime = time;
        Color curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Vector4.MoveTowards(curpos, to, speed * Time.fixedDeltaTime);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    public static IEnumerator ColorBySpeed(Color from, Color to, float speed, Action<Color> OnChangeCallback, Action OnEndCallback = null)
    {
        Color dif = to - from;

        float time = Mathf.Abs(((Vector4)dif).sqrMagnitude / speed);

        float curtime = time;
        Color curpos = from;

        while (curtime > 0)
        {
            yield return new WaitForFixedUpdate();

            curpos = Vector4.MoveTowards(curpos, to, speed * Time.fixedDeltaTime);

            OnChangeCallback?.Invoke(curpos);

            curtime -= Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    #endregion

    #region Generic

    public static IEnumerator GenericBySpeed<T>(T[] objs, float speed, Action<T> OnChangeCallback, Action OnEndCallback = null)
    {
        float frametime = 1 / speed;

        foreach (var item in objs)
        {
            OnChangeCallback?.Invoke(item);

            yield return new WaitForSeconds(frametime);
        }

        OnEndCallback?.Invoke();
    }

    public static IEnumerator GenericByTime<T>(T[] objs, float time, Action<T> OnChangeCallback, Action OnEndCallback = null)
    {
        float speed = objs.Length / time;

        float frametime = 1 / speed;

        foreach (var item in objs)
        {
            OnChangeCallback?.Invoke(item);

            yield return new WaitForSeconds(frametime);
        }

        OnEndCallback?.Invoke();
    }

    #endregion

    #region Curve

    public static IEnumerator MoveByCurve(AnimationCurve curve, Action<float> OnChangeCallback, Action OnEndCallback = null)
    {
        float time = 0;
        float maxtime = curve.keys.Max(i => i.time);

        while (time <= maxtime)
        {
            OnChangeCallback?.Invoke(curve.Evaluate(time));

            yield return new WaitForFixedUpdate();

            time += Time.fixedDeltaTime;
        }

        OnEndCallback?.Invoke();
    }

    #endregion
}
