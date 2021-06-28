using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ProvidingServices <T> : MonoBehaviour where T : ProvidingServices<T>
{
    private static object LockObj = new object();

    private static T instance;

    public static T One
    {


        get
        {
            


                if (instance == null)
                {

                    GameObject go = GameObject.Find("ProvidingServices");

                    if (go == null)
                    {
                        go = new GameObject().Name("ProvidingServices");
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




                }

                return instance;



           


        }


    }

    public virtual void Init()
    {

    }

    public virtual void Clear()
    {

    }



    protected virtual void OnDestroy()
    {
        
    }


}
