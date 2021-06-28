using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using My = UnityEngine;
using System.Threading;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public static class ExpandMono
{
    public static T Component<T>(this Transform transform, Action<T> act = null) where T : Component
    {
        T t = transform.gameObject.GetOrAddComponent<T>();

        if (act != null)
        {
            act(t);
        }
        return t;
    }
    public static T Component<T>(this GameObject go, Action<T> act = null) where T : Component
    {
        T t = go.GetOrAddComponent<T>();

        if (act != null)
        {
            act(t);
        }
        return t;
    }



    public static Transform GetFatherByName(this Transform p,string name)
    {
        Transform t = null;

        if (p.GetFather()!=null)
        {
            t = p.GetFather();
            if (t.name==name)
            {
                
                return t;
            }
            else
            {
              t=  t.GetFatherByName(name);

                return t;
            }

        }
        else
        {
            return null;
        }


    }

    public static Transform GetFather(this Transform p)
    {
        return p.parent;
    }







    /// <summary>
    /// 通过对象名查找对象
    /// </summary>
    /// <param name="p">父级</param>
    /// <param name="name">对象名</param>
    /// <returns></returns>
    public static Transform FindChildByName(Transform p, string name)
    {
        if (p.name == name)
        {
            return p;
        }
        if (p.childCount <= 0)
        {
            return null;
        }
        for (int i = 0; i < p.childCount; i++)
        {



            Transform tar = FindChildByName(p.GetChild(i), name);
            if (tar != null)
            {
                return tar;
            }


        }

        return null;


    }
    




    public static Transform GetOutChindContainsStr(this Transform p,string key)
    {
        if (p.name.Contains( key))
        {
            return p;
        }
        if (p.childCount <= 0)
        {
            return null;
        }
        for (int i = 0; i < p.childCount; i++)
        {



            Transform tar = GetOutChindContainsStr(p.GetChild(i), key);
            if (tar != null)
            {
                return tar;
            }


        }

        return null;
    }



    /// <summary>
    /// 通过对象名查找对象
    /// </summary>
    /// <param name="p"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform GetOutChild(this Transform p, string name)
    {
        if (p.name == name)
        {
            return p;
        }
        if (p.childCount <= 0)
        {
            return null;
        }
        for (int i = 0; i < p.childCount; i++)
        {



            Transform tar = GetOutChild(p.GetChild(i), name);
            if (tar != null)
            {
                return tar;
            }


        }

        return null;


    }

    public static GameObject CloneGameObj(this GameObject go)
    {

        GameObject o = GameObject.Instantiate(go);
        o.Name(go.name);
        return o;
    }


    public static GameObject Show(this GameObject obj)
    {

        obj.SetActive(true);
        return obj;

    }



    public static GameObject Hide(this GameObject obj)
    {
        obj.SetActive(false);
        return obj;
    }


    public static GameObject SwitchObj(this GameObject o, bool isAct = true)
    {
        switch (isAct)
        {
            case true: o.transform.localScale = UnityEngine.Vector3.one; break;


            case false: o.transform.localScale = UnityEngine.Vector3.zero; break;
        }

        return o;

    }



    public static GameObject Name(this GameObject obj, string name)
    {
        obj.name = name;

        return obj;
    }

    public static Transform Name(this Transform obj, string name)
    {
        obj.name = name;

        return obj;
    }

    public static GameObject SetPlace(this GameObject obj, UnityEngine.Vector3 p = default(UnityEngine.Vector3), UnityEngine.Vector3 r = default(UnityEngine.Vector3))
    {
        if (p != default(UnityEngine.Vector3))
        {
            obj.transform.position = p;
        }
        if (r != default(UnityEngine.Vector3))
        {
            obj.transform.rotation = UnityEngine.Quaternion.LookRotation(r);
        }
        return obj;

    }
    public static GameObject SetFather(this GameObject obj, Transform p, bool isScale = false)
    {
        lock (p)
        {
            obj.transform.SetParent(p, isScale);
        };
        return obj;

    }


    public static GameObject SetFather(this GameObject obj ,Transform p)
    {
        lock (p)
        {

            obj.transform.SetFather(p);

        }
        return obj;

    }




    public static Transform SetFather(this Transform obj, Transform p, bool isScale = false)
    {
        lock (p)
        {
            obj.SetParent(p, isScale);
        };
        return obj;

    }
    public static GameObject Break(this GameObject obj)
    {

        GameObject.Destroy(obj);
        return obj;
    }

    public static Transform Break(this Transform obj)
    {

        GameObject.Destroy(obj.gameObject);
        return obj;
    }
    public static async void BreakRelease(this Transform o)
    {
        GameObject.DestroyImmediate(o.gameObject);
        await Task.Delay(0);
        o = null;
    }
    public static async void BreakRelease(this GameObject o)
    {
        GameObject.DestroyImmediate(o);
        await Task.Delay(0);
        o = null;
    }

    public static Transform Form(this GameObject obj)
    {
        return obj.transform;

    }
    public static GameObject GoObj(this Transform t)
    {
        return t.gameObject;
    }

    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
    }

    public static T DoGet<T>(this GameObject go) where T : UnityEngine.Object
    {

        T t = go.GetComponent<T>();

        return t;

    }

    public static T DoGet<T>(this Transform go) where T : UnityEngine.Object
    {

        T t = go.GetComponent<T>();

        return t;

    }




    /// <summary>
    /// 添加按钮放大缩小效果
    /// </summary>
    /// <param name="b"></param>
    /// <param name="evt"></param>
    /// <returns></returns>
    //public static GameObject DoBtn(this Button b, Action evt)
    //{
    //    if (b.gameObject.GetComponent<ButtonClick>()==null)
    //    {
    //        b.gameObject.AddComponent<ButtonClick>();
    //    }
    //    b.onClick.AddListener(() => { AudioManger.Instance.PlayClickSound(Sound.click_music); evt();  });
    //    return b.gameObject;
    //}
    public static GameObject DoBtnU(this Button b, UnityAction act)
    {
        b.onClick.AddListener(act);
        return b.gameObject;
    }
    public static GameObject DoTxT(this Text t, string tt)
    {
        t.text = tt;

        return t.gameObject;
    }
    public static GameObject DoSp(this Image ima,Sprite s )
    {
        ima.sprite = s;

        return ima.gameObject;
    }





    /// <summary>
    /// 需要本地化的文字调用此接口
    /// </summary>
    /// <param name="t"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    //public static GameObject DoStr(this Text t, string key,Action<string> act=null)
    //{
    //    if (GameMgr.Instance.luangs.ContainsKey(t))
    //    {
    //        GameMgr.Instance.luangs[t] = key;
    //    }
    //    else
    //    {
    //        GameMgr.Instance.luangs.Add(t, key);
    //    }

    //    if(GameMgr.Instance.acts.ContainsKey(t))
    //    {
    //        GameMgr.Instance.acts[t] = act;
    //    }
    //    else
    //    {
    //        GameMgr.Instance.acts.Add(t, act);
    //    }

    //    if (!JsonModel.Instance.GetkeyValuePairs().ContainsKey(key))
    //    {
    //        Debug.Log("没有包含此键");
    //    }
    //    else
    //    {
    //        switch (GameMgr.Instance.luangType)
    //        {
    //            case 1: t.text = JsonModel.Instance.GetkeyValuePairs()[key].CN; break;
    //            case 2: t.text = JsonModel.Instance.GetkeyValuePairs()[key].EN; break;
    //            case 3: t.text = JsonModel.Instance.GetkeyValuePairs()[key].CT; break;
    //            default:
    //                break;
    //        }
    //    }

    //    if (act != null)
    //    {
    //        act(t.text);
    //    }
    //    return t.gameObject;

    //    //t.text = key;
    //    //return t.gameObject;
    //}
    public static Transform DoGetChild(this Transform t, string name)
    {
        if (t.name == name)
        {
            return t;
        }
        if (t.childCount <= 0)
        {
            return null;
        }
        Transform target = null;
        for (int i = 0; i < t.childCount; i++)
        {
            target = DoGetChild(t.GetChild(i), name);
            if (target != null)
            {
                return target;
            }

        }

        return null;

    }
    public static bool isHaveChild(this Transform t)
    {

        return t.childCount <= 0;

    }
    public static bool isHaveChild(this GameObject t)
    {

        return t.transform.childCount <= 0;

    }

    public static GameObject LookAtTrans(this Transform trans, Transform t)
    {

        UnityEngine.Vector3 pos = new UnityEngine.Vector3(t.position.x, trans.position.y, t.position.z);
        trans.LookAt(pos);
        return trans.gameObject;
    }

    public static GameObject LookAtVec3(this Transform tar, UnityEngine.Vector3 v)
    {
        UnityEngine.Vector3 vec = new My.Vector3(v.x, tar.position.y, v.z);
        tar.LookAt(vec);
        return tar.gameObject;
    }



    public static void LookAtDir_2D(this Transform trans, UnityEngine.Vector3 dir)
    {
        var pos = trans.position + dir;
        pos.y = trans.position.y;
        trans.LookAt(pos);
    }
    public static int ToInt(this string str)
    {
        return int.Parse(str);
    }
    /// <summary>
    /// 设置物体X坐标（有参）
    /// </summary>
    /// <param name="tran">当前Transform</param>
    /// <param name="pos_x">X坐标</param>
    public static void SetPosition_X(this Transform tran, float pos_x)
    {
        tran.position = new UnityEngine.Vector3(pos_x, tran.position.y, tran.position.z);
    }
    /// <summary>
    /// 设置物体Y坐标（有参）
    /// </summary>
    /// <param name="tran">当前Transform</param>
    /// <param name="pos_y">Y坐标</param>
    public static void SetPosition_Y(this Transform tran, float pos_y)
    {
        tran.position = new UnityEngine.Vector3(tran.position.x, pos_y, tran.position.z);
    }
    /// <summary>
    /// 设置物体Z坐标（有参）
    /// </summary>
    /// <param name="tran">当前Transform</param>
    /// <param name="pos_z">Z坐标</param>
    public static void SetPosition_Z(this Transform tran, float pos_z)
    {
        tran.position = new UnityEngine.Vector3(tran.position.x, tran.position.y, pos_z);
    }
    /// <summary>
    /// 设置物体坐标
    /// </summary>
    /// <param name="tran">当前Tranfrom</param>
    /// <param name="pos_x">X坐标</param>
    /// <param name="pos_y">Y坐标</param>
    /// <param name="pos_z">Z坐标</param>
    public static void SetPosition_Pos(this Transform tran, float pos_x, float pos_y, float pos_z)
    {
        tran.position = new UnityEngine.Vector3(pos_x, pos_y, pos_z);
    }
  
    public static GameObject SetPosByTran(this Transform trans, Transform tar)
    {
        trans.localPosition = tar.localPosition;
        return trans.gameObject;
    }


    /// <summary>
    /// 设置文本中字体颜色
    /// </summary>
    /// <param name="str">当前字符串</param>
    /// <param name="colorValue">色值</param>
    /// <returns></returns>
    public static string ChangeTxtColor(this string str, string colorValue)
    {
        StringBuilder strB = new StringBuilder();
        return strB.Append("<color=#").Append(colorValue).Append(">").Append(str).Append("</color>").ToString();
    }


    public static void AudioSwitch(this Transform t, bool isAct = true)
    {
        if (t.GetComponent<AudioSource>() != null)
        {
            t.GetComponent<AudioSource>().mute = isAct;
        }
        if (t.childCount <= 0)
        {
            return;
        }
        for (int i = 0; i < t.childCount; i++)
        {

            Transform tr = t.GetChild(i);
            tr.AudioSwitch(isAct);

        }


    }
    public static void SetObjInLayer(this Transform t,string layer)
    {
        
        
            t.gameObject.layer = LayerMask.NameToLayer(layer);
            if (t.childCount <= 0)
            {
                return;
            }
            for (int i = 0; i < t.childCount; i++)
            {
                t.GetChild(i).gameObject.layer = LayerMask.NameToLayer(layer);

                 t.GetChild(i).SetObjInLayer(layer);
            }

        


    }

    public static readonly string no_breaking_space = "\u00A0";

    public static string DoSpace2Str(this string str, int count)
    {

        str = "#" + str;
        string newStr = string.Empty;
        string spaceStr = string.Empty;
        for (int i = 0; i < count; i++)
        {
            spaceStr += no_breaking_space;
        }

        newStr = str.Replace("#", spaceStr);


        return newStr;
    }
    public static bool IsAct(this GameObject go)
    {

        return go.activeInHierarchy;
    }
    public static bool IsAct(this Transform t)
    {

        return t.gameObject.activeInHierarchy;

    }




    public static string ToReadableAgeString(this TimeSpan span)
    {
        return string.Format("{0:0}", span.Days / 365.25);
    }

    public static string ToReadableString(this TimeSpan span)
    {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "s") : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? string.Empty : "s") : string.Empty);

        if (formatted.EndsWith(", ")) formatted = formatted.Substring(0, formatted.Length - 2);

        if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";

        return formatted;
    }
    public static string ToReadableStringHoursAndMin(this TimeSpan span)
    {
        string formatted = string.Format("{0}{1}",
        span.Duration().Hours >= 0 ? string.Format("{0:0}{1}, ", span.Hours, span.Hours == 1 ? string.Empty : "") : string.Empty,
       span.Duration().Minutes >= 0 ? string.Format("{0:0}{1}, ", span.Minutes, span.Minutes == 1 ? string.Empty : "") : string.Empty);

        if (formatted.EndsWith("| ")) formatted = formatted.Substring(0, formatted.Length - 2);

        if (string.IsNullOrEmpty(formatted)) formatted = "0 seconds";
        return formatted;

    }
    public static string ToFormatTime(this long time)
    {


        float h = Mathf.FloorToInt(time / 3600f);
        float m = Mathf.FloorToInt(time / 60f - h * 60f);
        float s = Mathf.FloorToInt(time - m * 60f - h * 3600f);
        return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");



    }
    public static string ToFormatTime(this float time)
    {


        float h = Mathf.FloorToInt(time / 3600f);
        float m = Mathf.FloorToInt(time / 60f - h * 60f);
        float s = Mathf.FloorToInt(time - m * 60f - h * 3600f);
        return h.ToString("00") + ":" + m.ToString("00") + ":" + s.ToString("00");



    }
    public static string ToFormatTimeThisGame(this float time)
    {
        float h = Mathf.FloorToInt(time / 3600f);
        float m = Mathf.FloorToInt(time / 60f - h * 60f);
        float s = Mathf.FloorToInt(time - m * 60f - h * 3600f);
        return m.ToString("00") + ":" + s.ToString("00");


    }
    public static string ToStr(this BigInteger b)
    {
        return b.ToStr();

    }
    //public static string ToFormatMoney(this BigInteger b)
    //{
    //    return BigData.FormatBigInteger(b);

    //}
    public static BigInteger ToBigInteger(this string str)
    {
        try
        {
            return BigInteger.Parse(str);
        }
        catch (Exception ex)
        {

            Debug.Log(ex + "数据结构转换错误");

            return 0;
        }

    }
    public static async void SeeAsyncRot(this Transform t, Transform target, Action onEndCallBack = null, float speed = 2, int haveWaitTime = 0)
    {
        bool isComplete = false;
        while (!isComplete)
        {

            t.rotation = My.Quaternion.Slerp(t.rotation, target.rotation, Time.deltaTime * speed);

            My.Vector3 v1 = t.rotation.eulerAngles;
            My.Vector3 v2 = target.rotation.eulerAngles;
            if (Mathf.Abs(v1.x-v2.x)<=1&&Mathf.Abs(v1.y-v2.y)<=1&&Mathf.Abs(v1.z-v2.z)<=1)
            {
                isComplete = true;
            }
            await Task.Delay(1);


        }
        await Task.Delay(haveWaitTime*1000);
        if (isComplete)
        {
            if (onEndCallBack!=null)
            {
                onEndCallBack();
            }
        }
        
    }
    public static Waiter SeeRot(this Transform t, Transform target, Action onEndCallBack = null, float speed = 2)
    {
        bool isComplete = false;

        
        
        Waiter w = WaitSe.One.AddWaiter(
            () =>
            {
                if (isComplete)
                {
                    return true;
                }
                
                t.rotation = My.Quaternion.Slerp(t.rotation, target.rotation, Time.deltaTime * speed);

                My.Vector3 v1 = t.rotation.eulerAngles;
                My.Vector3 v2 = target.rotation.eulerAngles;
                if (Mathf.Abs(v1.x - v2.x) <= 1 && Mathf.Abs(v1.y - v2.y) <= 1 && Mathf.Abs(v1.z - v2.z) <= 1)
                {
                    isComplete = true;
                    
                }

                return isComplete;

            }, () => {



                if (onEndCallBack != null) {

                    onEndCallBack();

                };
                

                }
            );


        return w;



    }

    public static Waiter SeeRot(this Transform t, UnityEngine.Vector3 target, Action onEndCallBack = null, float speed = 2)
    {
        bool isComplete = false;



        Waiter w = WaitSe.One.AddWaiter(
            () =>
            {
                if (isComplete)
                {
                    return true;
                }
                t.rotation = My.Quaternion.Slerp(t.rotation, UnityEngine.Quaternion.Euler(target), Time.deltaTime * speed);

                My.Vector3 v1 = t.rotation.eulerAngles;
                My.Vector3 v2 = target;
                if (Mathf.Abs(v1.x - v2.x) <= 1 && Mathf.Abs(v1.y - v2.y) <= 1 && Mathf.Abs(v1.z - v2.z) <= 1)
                {
                    isComplete = true;

                }

                return isComplete;

            }, () =>
            {



                if (onEndCallBack != null)
                {

                    onEndCallBack();

                };


            }
            );


        return w;



    }





    public static string SplitStr(this string str,params object [] args)
    {
        str = string.Empty;
        for (int i = 0; i < args.Length; i++)
        {
           
                str += args[i].ToString();
          

        }

        return str;


    }
    public static  Waiter AnimatorFreamAction(this Animator ani,string aniName,Action  playAniEnd,Action doReset=null)
    {
        bool isInAniPlay = false;

        AnimatorStateInfo animatorInfo;
        Waiter w = WaitSe.One.AddWaiter(() =>
        {
           
            if (isInAniPlay == false)
            {
                animatorInfo = ani.GetCurrentAnimatorStateInfo(0);

                //Debug.Log(animatorInfo.normalizedTime.LogStr());
                //Debug.Log(animatorInfo.IsName(aniName).LogStr());
                if ((animatorInfo.normalizedTime >= 1.0f) && (animatorInfo.IsName(aniName)))
                {
                    if (playAniEnd != null)
                    {
                        playAniEnd();
                        playAniEnd = null;
                        isInAniPlay = true;
                       

                    }
                    else
                    {
                        isInAniPlay = true;

                    }

                }
            }
          
            return isInAniPlay;


        },()=> {
            if (doReset!=null)
            {
                
                doReset();
            }

        });

        return w;

    }
    public static Waiter AnimatorPlay2FramWithAction(this Animator ani, int Fram, string aniName,Action playAniEnd, Action doReset = null)
    {
        AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
        AnimationClip aniClip = null;
      //  Debug.LogError(clips.Length);
        foreach (AnimationClip clip in clips)
        {
           // Debug.LogError(clip.name);
            if (clip.name == aniName)
            {
                aniClip = clip; break;
            }
        }
       

        

        var len = aniClip.length;
        float framRate = aniClip.frameRate;
        float totalTime = len / (1 / framRate);
       // Debug.LogError(totalTime);
        Waiter w = WaitSe.One.AddWaiter(() =>
        {

            var curtime = ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
            int curFream = (int)(Mathf.Floor(totalTime * curtime) % totalTime);
           // Debug.Log("111111111111111111111111111");

           
            if (curFream>=Fram)
            {
               // Debug.LogError(curFream);
                if (playAniEnd!=null)
                {
                    playAniEnd();
                }
               
                return true;
            }

            else
            {
                
                return false;
            }


        }, () =>
        {
            if (doReset!=null)
            {
                doReset();
            }

        });

        return w;



    }





    public static Waiter PsPlayEndAction(this Transform t,Action endAct)
    {
       // Debug.LogError("111111111111111111111111111111");
        ParticleSystem[] particleSystems;
        particleSystems = t.GetComponentsInChildren<ParticleSystem>();

        bool allStopped = true;
        List<ParticleSystem> lst = new List<ParticleSystem>();
        lst.AddRange(particleSystems);
        for (int i = lst.Count-1; i > 0; i--)
        {

            ParticleSystem p = lst[i];
            if (p.main.loop==true)
            {
                lst.Remove(p);
               
            }

        }
        //Debug.LogError("111111111111111111111111111111");
        t.gameObject.Show();

        Waiter w = WaitSe.One.AddWaiter(() =>
        {
            allStopped = true;

            foreach (ParticleSystem ps in lst)
            {
                if (!ps.isStopped)
                {
                    allStopped = false;
                }
            }
            if (allStopped)
            {
               // Debug.LogError("111111111111111111111111111111");
                return true;
            }
            else
            {
               // Debug.LogError("111111111111111111111111111111");
                return false;
            }



        }, () =>
        {

            if (endAct != null)
            {
                endAct();
            }


        });

        return w;
    }
    public static List<T> ListRandom<T>(this List<T> myList) where T : UnityEngine.Object
    {

       System. Random ran = new System.Random();
        
        int index = 0;
        T temp = default(T);
        for (int i = 0; i < myList.Count; i++)
        {

            index = ran.Next(0, myList.Count - 1);
            if (index != i)
            {
                temp = myList[i];
                myList[i] = myList[index];
                myList[index] = temp;
            }
        }
        


        return myList;
    }
   
}
