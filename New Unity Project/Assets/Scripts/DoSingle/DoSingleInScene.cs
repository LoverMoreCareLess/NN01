using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSingleInScene<T> : MonoBehaviour where T : DoSingleInScene<T>
{
    private static T instance;

    public static T One
    {
        get
        {

            if (instance == null)
            {
                GameObject go = GameObject.Find(typeof(T).Name);
                if (go == null)
                {
                    go = new GameObject();
                    go.name = typeof(T).Name;
                    instance = go.AddComponent<T>();
                    return instance;

                }
                else
                {
                    instance = go.GetComponent<T>();
                    if (instance == null)
                    {
                        instance = go.AddComponent<T>();
                        return instance;
                    }
                    else
                    {
                        return instance;
                    }
                }

            }


            return instance;

        }

    }

    public virtual void Init(params object[] args)
    {

    }
    public virtual void Clear()
    {

    }
}
