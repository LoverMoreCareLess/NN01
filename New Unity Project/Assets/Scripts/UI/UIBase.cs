using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public abstract class UIBase : MonoBehaviour
{
    public OpenUIAct open
    {
        get;set;
    }

    protected Action showEndact;
    


    public abstract void Init();
    public abstract void Show(params object[] obj);
    public abstract void Hide();



    public virtual void OnInit()
    {
        Init();
    }

    public virtual void OnShow(params object[] obj)
    {
        Show(obj);
    }

    public void OnShowEnd()
    {
        showEndact();
    }

    public virtual void OnHide(params object[] obj)
    {
        showEndact = null;
        Hide();
    }



    public void SetShowEnd(Action act)
    {
        showEndact = act;
    }
}
