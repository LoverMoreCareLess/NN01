using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSingleMonoByGameRoot<T> : MonoBehaviour where T : DoSingleMonoByGameRoot<T>
{

    private static T instance;

    public static T One
    {


        get
        {

            if (instance == null)
            {

                GameObject go = GameObject.Find("GameRoot");

                if (go == null)
                {
                    go = new GameObject().Name("GameRoot");
                    T t = go.GetComponent<T>();
                    if (t == null)
                    {
                        instance = go.AddComponent<T>();
                    }
                    else
                    {
                        instance = t;
                        return instance;
                    }

                }
                else
                {
                    T t = go.GetComponent<T>();
                    if (t == null)
                    {
                        instance = go.AddComponent<T>();
                    }
                    else
                    {
                        instance = t;
                        return instance;
                    }



                }

               // DontDestroyOnLoad(go);


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
