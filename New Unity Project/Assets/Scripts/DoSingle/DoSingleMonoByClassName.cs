using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSingleMonoByClassName<T> : MonoBehaviour where T : DoSingleMonoByClassName<T>
{
    private static T instance;

    protected bool isDontDestory = false;

    protected static GameObject go;


    public static T One
    {



        get
        {
            if (instance == null)
            {

                go = GameObject.Find(typeof(T).Name);
                if (go == null)
                {
                    go = new GameObject();
                    go.name = typeof(T).Name;
                    instance = go.AddComponent<T>();




                }
                else
                {
                    instance = go.GetComponent<T>();
                    if (instance == null)
                    {
                        instance = go.AddComponent<T>();
                    }



                }



            }

            return instance;

        }





    }




    public virtual void Init()
    {





    }

    protected virtual void Start()
    {

        if (isDontDestory)
        {
            DontDestroyOnLoad(go);
        }


    }
    public virtual void Clear()
    {

    }
}
