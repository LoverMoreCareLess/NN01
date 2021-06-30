using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static  class UIMono
{

    public static GameObject AddUIInstall(this GameObject ga)
    {
        if (ga.GetComponent<CanvasGroup>() == null)
        {
            ga.AddComponent<CanvasGroup>();
        }
        Type type = Type.GetType(ga.name);
        ga.AddComponent(type);

        return ga;

    }

    public static void AddComByName<T>(this GameObject go, string codeName) where T : UnityEngine.MonoBehaviour
    {
        T m = Assembly.GetExecutingAssembly().CreateInstance(codeName) as T;

        go.AddComponent<T>();



    }

    /// <summary>
    /// 转换时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string TimeFormat(long time)
    {
        float h = Mathf.FloorToInt(time / 3600f);
        float m = Mathf.FloorToInt(time / 60f - h * 60f);
        float s = Mathf.FloorToInt(time - m * 60f - h * 3600f);
        return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
    }

    /// <summary>
    /// 转换时间
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static string TimeFormat(this double time)
    {
        float h =Mathf.FloorToInt((float)(time / 3600f));
        float m = Mathf.FloorToInt((float)(time / 60f - h * 60f));
        float s = Mathf.FloorToInt((float)(time - m * 60f - h * 3600f));
        return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");
    }
}
