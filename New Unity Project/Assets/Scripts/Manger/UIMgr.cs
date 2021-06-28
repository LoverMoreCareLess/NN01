using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum OpenUIAct
{
    None,
}
public class UIMgr : DoSingle<UIMgr>
{




    private Dictionary<string, GameObject> UIpanel;



    private Transform resident;
    private Transform mid;
    private Transform loading;
    private Transform tip;
    public void Init()
    {
        UIpanel = new Dictionary<string, GameObject>();

    }

    public UIBase ShowUI(string name, OpenUIAct open = OpenUIAct.None)
    {
        UIBase ui = null;
        if (!UIpanel.ContainsKey(name))
        {
            GameObject go = null;
            AResMgr.One.GetUI(name, (g) =>
            {
                go = g;
                if (go != null)
                {
                    go.AddUIInstall<UIBase>();
                }
                UIpanel.Add(go.name, go);

                ui = UIpanel[name].GetComponent<UIBase>();
                ui.transform.SetParent(mid, false);
                CanvasGroup cg = ui.transform.GetComponent<CanvasGroup>();
                if (cg != null)
                {
                    cg.alpha = 1;
                }

            }, () =>
            {

                Debug.Log("为空");

            });

        }
        ui.open = open;
        LoadingComplete(ui, name, open);
        return ui;
    }


    public void CloseUI(string name,bool isDestroy = true)
    {
        UIBase ui = UIpanel[name].GetComponent<UIBase>();
        ui.OnHide();
        ui.transform.GetComponent<CanvasGroup>().alpha=0;
        CloseUIWay(ui.gameObject, ui.open);
        if (isDestroy)
        {
            UIpanel[name].BreakRelease();
        }

    }
    public void CloseUI(string name,OpenUIAct open, bool isDestroy = true)
    {
        UIBase ui = UIpanel[name].GetComponent<UIBase>();
        ui.OnHide();
        ui.transform.GetComponent<CanvasGroup>().alpha = 0;
        CloseUIWay(ui.gameObject, open);
        if (isDestroy)
        {
            UIpanel[name].BreakRelease();
        }

    }



    /// <summary>
    /// 加载UI界面
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private void LoadUIPrefab(string name)
    {


    }


    /// <summary>
    /// 加载完成
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="name"></param>
    /// <param name="open"></param>
    private void LoadingComplete(UIBase ui, string name, OpenUIAct open = OpenUIAct.None)
    {
        ui = UIpanel[name].GetComponent<UIBase>();
        ui.transform.SetParent(mid, false);
        CanvasGroup cg = ui.transform.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1;
        }
        OpenUIWay(ui, open);
    }


    /// <summary>
    /// 打开方式
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="way"></param>
    private void OpenUIWay(UIBase ui, OpenUIAct way)
    {
        switch (way)
        {
            case OpenUIAct.None:
                {
                    ui.OnShowEnd();
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 关闭方式
    /// </summary>
    /// <param name="ui"></param>
    /// <param name="way"></param>
    private void CloseUIWay(GameObject ui, OpenUIAct way)
    {
        switch (way)
        {
            case OpenUIAct.None:
                {






                }
                break;
            default:
                break;
        }
    }


    public T GetUIPanel<T>(string name)
    {
        if (UIpanel.ContainsKey(name))
        {
            return UIpanel[name].GetComponent<T>();

        }
        return default;
    }
}
