using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static  class UIMono
{

    public static GameObject AddUIInstall<T>(this GameObject ga) where T : UnityEngine.MonoBehaviour
    {
        if (ga.GetComponent<CanvasGroup>() == null)
        {
            ga.AddComponent<CanvasGroup>();
        }

        ga.AddComByName<T>(ga.name);

        return ga;
    }

    public static void AddComByName<T>(this GameObject go, string codeName) where T : UnityEngine.MonoBehaviour
    {
        T m = Assembly.GetExecutingAssembly().CreateInstance(codeName) as T;

        go.AddComponent<T>();



    }
}
