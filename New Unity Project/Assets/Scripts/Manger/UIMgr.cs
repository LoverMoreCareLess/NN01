using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.U2D;
using UnityEngine.UI;

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

    public void ShowUI(string name, OpenUIAct open = OpenUIAct.None,params object[] obj)
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
                    ui.Open = open;
                    ui.OnInit();
                    ui.OnShow(obj);
                    LoadingComplete(ui, name, open);
                }
                
            }, 
            () =>
            {

                Debug.Log("为空");

            });

        }
        else
        {
            ui = UIpanel[name].GetComponent<UIBase>();
            ui.transform.SetParent(mid, false);
            ui.Open = open;
            ui.OnShow(obj);
            LoadingComplete(ui, name, open);
        }
       
    }


    public void ShowUI(string name, UIType type, OpenUIAct open = OpenUIAct.None, params object[] obj)
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
                ui.Open = open;
                ui.OnInit();
                ui.OnShow(obj);
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
            ui.Open = open;
            ui.OnShow(obj);
            LoadingComplete(ui, name, open);
        }
        
    }

    public void CloseUI(string name, bool isDestroy = true)
    {
        if(UIpanel.ContainsKey(name))
        {
            UIBase ui = UIpanel[name].GetComponent<UIBase>();
            ui.OnHide();
            CloseUIWay(ui.gameObject, ui.Open, () => {

                ui.transform.GetComponent<CanvasGroup>().alpha = 0;
                ui.transform.SetParent(resident, false);
                ui.RestDataRect();
                if (isDestroy)
                {
                    GameObject g = UIpanel[name];
                    UIpanel.Remove(name);
                    g.BreakRelease();
                }

            });
        }
        else
        {
            Debug.Log("不存在");
        }
        
    }

    public void CloseUI(string name, OpenUIAct open, bool isDestroy = true)
    {

        if(UIpanel.ContainsKey(name))
        {
            UIBase ui = UIpanel[name].GetComponent<UIBase>();
            ui.OnHide();

            CloseUIWay(ui.gameObject, open, () => {

                ui.transform.GetComponent<CanvasGroup>().alpha = 0;
                ui.transform.SetParent(resident, false);
                ui.RestDataRect();
                if (isDestroy)
                {
                    GameObject g = UIpanel[name];
                    UIpanel.Remove(name);
                    g.BreakRelease();
                }
            });
        }
        else
        {
            Debug.Log("不存在");
        }


    }

    public void CloseUI(GameObject go, OpenUIAct open, bool isDestroy = true)
    {
        if (UIpanel.ContainsValue(go))
        {

            UIBase ui = go.GetComponent<UIBase>();
            ui.OnHide();

            CloseUIWay(ui.gameObject, open, () => {

                ui.transform.GetComponent<CanvasGroup>().alpha = 0;
                ui.transform.SetParent(resident, false);
                ui.RestDataRect();
                if (isDestroy)
                {
                    string k = string.Empty;
                    foreach (var item in UIpanel)
                    {
                        if(item.Value==go)
                        {
                            k = item.Key;
                            break;
                        }
                    }
                    UIpanel.Remove(k);
                    go.BreakRelease();
                }
            });
        }
        else
        {
            Debug.Log("不存在");
        }
    }

    /// <summary>
    /// 销毁所有界面
    /// </summary>
    public void DestroyAllUI()
    {
        foreach (var item in UIpanel)
        {
            UIBase u = item.Value.GetComponent<UIBase>();
            u.OnHide();
            u.gameObject.BreakRelease();
        }
        UIpanel.Clear();
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
                    ui.transform.DOScale(Vector3.zero, 0.2f).From().OnComplete(() =>
                    {
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
                break; case OpenUIAct.Smaller:
                {
                    ui.transform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
                    {

                        if (endAct != null)
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

    //图集
    Dictionary<string, SpriteAtlas> spDic = new Dictionary<string, SpriteAtlas>();

    /// <summary>
    /// 获取图集图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public void LoadSprite(string path,string name,Image img)
    {
        if (spDic.ContainsKey(path))
        {
            img.sprite= spDic[path].GetSprite(name);
        }
        else
        {

            AResMgr.One.GetAtlas(name, (sp) =>
            {
                spDic.Add(path, sp);
                img.sprite = sp.GetSprite(name);
            }, 
            () =>
            {
                Debug.Log("加载失败");
            });
        }
    }

}
