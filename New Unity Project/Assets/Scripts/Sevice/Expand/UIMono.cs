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
}
