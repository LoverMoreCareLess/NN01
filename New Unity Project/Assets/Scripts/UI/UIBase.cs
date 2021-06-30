using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;


public abstract class UIBase : MonoBehaviour
{
    private OpenUIAct open;
    public OpenUIAct Open
    {
        get
        {
            int p = (int)open + 10;
            return (OpenUIAct)p;

        }
        set
        {
            open = value;
        }
    }

    protected Action showEndact;
  



    public abstract void Init();
    public abstract void Show(params object[] obj);
    public abstract void Hide();



    public virtual void OnInit()
    {
        Init();
        pos = transform.GetComponent<RectTransform>().position;
        rot = transform.GetComponent<RectTransform>().rotation;
        sca = transform.GetComponent<RectTransform>().localScale;
    }

    public virtual void OnShow(params object[] obj)
    {
        Show(obj);
    }

    public void OnShowEnd()
    {
        if (showEndact != null)
        {
            showEndact();
        }
    }

    public virtual void OnHide()
    {
        showEndact = null;
        Hide();

    }



    public void SetShowEnd(Action act)
    {
        showEndact = act;
    }

    Vector3 pos;
    Quaternion rot;
    Vector3 sca;
    public void RestDataRect()
    {
        RectTransform r = transform.GetComponent<RectTransform>();
        r.position = pos;
        r.rotation = rot;
        r.localScale = sca;
    }


    public void ClosePanel()
    {
        UIMgr.One.CloseUI(this.gameObject, Open);
    }
}
