using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public enum OpenUIAct
{
    //打开
    None,
    Bigger,










    //关闭
    Smaller=11,
}

public enum UIType
{
    None,
    Tip,
    Mid,
    Load,
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
        Transform canvas = GameObject.Find("Canvas").transform;
        resident = canvas.GetOutChild("Resident");
        mid = canvas.GetOutChild("Mid");
        loading = canvas.GetOutChild("Loading");
        tip = canvas.GetOutChild("Tip");

    }

    public void ShowUI(string name, OpenUIAct open = OpenUIAct.None)
    {
        UIBase ui = null;
        if (!UIpanel.ContainsKey(name))
        {
            GameObject go = null;
            AResMgr.One.GetUI(name, (g) =>
            {
                go = g;
                go.name = name;
                if (go != null)
                {
                    go.AddUIInstall();
                    UIpanel.Add(go.name, go);

                    ui = UIpanel[name].GetComponent<UIBase>();
                    ui.transform.SetParent(mid, false);
                    ui.open = open;
                    LoadingComplete(ui, name, open);
                }
                
            }, () =>
            {

                Debug.Log("为空");

            });

        }
        else
        {
            ui = UIpanel[name].GetComponent<UIBase>();
            ui.transform.SetParent(mid, false);
            ui.open = open;
            LoadingComplete(ui, name, open);
        }
       
    }


    public void ShowUI(string name, UIType type, OpenUIAct open = OpenUIAct.None)
    {
        UIBase ui = null;
        if (!UIpanel.ContainsKey(name))
        {
            GameObject go = null;
            AResMgr.One.GetUI(name, (g) =>
            {
                go = g;
                go.name = name;
                if (go != null)
                {
                    go.AddUIInstall();
                }
                UIpanel.Add(go.name, go);

                ui = UIpanel[name].GetComponent<UIBase>();
                ui.transform.SetParent(GetParent(type), false);
                ui.open = open;
                LoadingComplete(ui, name, open);
            }, () =>
            {

                Debug.Log("为空");

            });

        }
        else
        {
            ui = UIpanel[name].GetComponent<UIBase>();
            ui.transform.SetParent(GetParent(type), false);
            ui.open = open;
            LoadingComplete(ui, name, open);
        }
        
    }

    public void CloseUI(string name, bool isDestroy = true)
    {
        UIBase ui = UIpanel[name].GetComponent<UIBase>();
        ui.OnHide();
        CloseUIWay(ui.gameObject, ui.open, () => {

            ui.transform.GetComponent<CanvasGroup>().alpha = 0;
            ui.transform.SetParent(resident, false);
            if (isDestroy)
            {
                UIpanel[name].BreakRelease();
            }

        });
    }

    public void CloseUI(string name, OpenUIAct open, bool isDestroy = true)
    {
        UIBase ui = UIpanel[name].GetComponent<UIBase>();
        ui.OnHide();

        CloseUIWay(ui.gameObject, open, () => {

            ui.transform.GetComponent<CanvasGroup>().alpha = 0;
            ui.transform.SetParent(resident, false);
            if (isDestroy)
            {
                UIpanel[name].BreakRelease();
            }
        });


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
            case OpenUIAct.Bigger:
                {
                    ui.transform.DOScale(Vector3.zero, 0.3f).From().OnComplete(() => {

                        ui.OnShowEnd();

                    });
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
    private void CloseUIWay(GameObject ui, OpenUIAct way, Action endAct)
    {
        switch (way)
        {
            case OpenUIAct.None:
                {

                }
                break; case OpenUIAct.Bigger:
                {
                    ui.transform.DOScale(Vector3.zero, 0.3f).OnComplete(()=> {

                        if(endAct!=null)
                        {
                            endAct();
                        }
                    });
                }
                break;
            default:
                break;
        }
    }


    private Transform GetParent(UIType type)
    {
        Transform parentUI = null;
        switch (type)
        {
            case UIType.None:
                break;
            case UIType.Tip:
                parentUI = tip;
                break;
            case UIType.Mid:
                parentUI = mid;
                break;
            case UIType.Load:
                parentUI = loading;
                break;
            default:
                break;
        }
        return parentUI;

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
