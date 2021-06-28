using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoSingle<T> where T : class, new()
{
    private static T instance;

   

    public static T One
    {
        get
        {
            

                if (instance == null)
                {
                    instance = new T();
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
