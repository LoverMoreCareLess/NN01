using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.AI;

public static class ExpandMonoMore
{
    public static int Random(this int z,int[] arrary,int total)
    {
        int r=0;
        r = r.Random(0, total);

        int t = 0;
        for (int i = 0; i < arrary.Length; i++)
        {
            t += arrary[i];
            if (r < t)
            {
                return i;
            }
        }
        return 0;

    }



    public static int Random(this int i, int x, int y)
    {
        i = UnityEngine.Random.Range(x, y + 1);

        return i;
    }
    public static float Random(this float i, float x, float y)
    {
        i = UnityEngine.Random.Range(x, y+1);

        return i;

    }
    public static T Random<T>(this List<T> lst, int length = 0)
    {
        if (length == 0)
        {
            length = lst.Count;
        }
        int index = UnityEngine.Random.Range(0, length);

        if (index <= lst.Count - 1)
        {
            return lst[index];
        }
        else
        {
            return default(T);
        }
    }

    public static T Random<T>(this T[]lst, int length = 0)
    {
        if (length == 0)
        {
            length = lst.Length;
        }
        int index = UnityEngine.Random.Range(0, length);
        if (index <= lst.Length - 1)
        {
            return lst[index];
        }
        else
        {
            return default(T);
        }
    }


    public static string GetName(this GameObject go)
    {
        return go.name;
    }

    public static string GetName(this Transform t)
    {
        return t.name;
    }
    public static int Str2Int(this string s)
    {
        try
        {
            int i = int.Parse(s);
            return i;
        }
        catch (Exception ex)
        {

            Debug.Log(ex.Message);
        }
        return 0;

    }
    public static string[] SpVert(this string str)
    {

        return str.Split('|');

    }
    public static string[] Sphori(this string str)
    {

        return str.Split('_');

    }
   




    public static bool IsEqual<T>(this List<T> lst1, List<T> lst2)
    {
        if (lst1.Count != lst2.Count)
        {
            return false;
        }
        bool equal = true;
        lst1.Sort();
        lst2.Sort();
        for (int i = 0; i < lst1.Count; i++)
        {
            if (lst1[i].Equals(lst2[i]) == false)
            {
                equal = false; break;
            }
        }

        return equal;
    }
    public static bool IsEqual<T>(this T[] a, T[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        bool equal = true;
        Array.Sort(a);
        Array.Sort(b);
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i].Equals(b[i]) == false)
            {
                equal = false; break;
            }
        }

        return equal;
    }
    public static PReTimer GetAWait(this PReTimer t, float time)
    {
        PReTimer timer = new PReTimer();
        timer.totalTime = Time.realtimeSinceStartup * 1000 + time;
        timer.startTime = Time.realtimeSinceStartup * 1000;
        timer.count = 1;
        timer.pingTime = time;
        
        timer.alreadyTime = 0;

        return timer;
    }
    public static T [] ToTArray<T>(this List<T> lst)
    {
        T [] newArray = new T[lst.Count];
        for (int i = 0; i < lst.Count; i++)
        {
            newArray[i] = lst[i];
        }
        return newArray;
    }


    public static PReTimer RegisterAct(this PReTimer timer, Action evt)
    {

        TimeSe.One.Add(out timer, timer, evt);


        return timer;
    }
    
    public static void Stop(this PReTimer timer)
    {
        if (timer==null)
        {
            return;
        }
        timer.callEevent = null;
        if (TimeSe.One==null)
        {
            return;
        }
        timer.isStoped = true;
       // TimeSe.Instance.RemoveTimer(timer.timerId);
        timer = null;
    }

    public static Waiter GetAWait(this Waiter t, Func<bool> wait)
    {
        Waiter w = new Waiter();
        w.IsTrue = wait;
        w.isStop = false;

        return w;
    }
    public static Waiter RegisterAct(this Waiter t, Action evt)
    {

        t = WaitSe.One.Add(t, evt);


        return t;
    }
    public static void Stop(this Waiter t)
    {
        if (t==null)
        {
            return;
        }
        if (WaitSe.One == null)
        {
            return;
        }
        WaitSe.One.Remove(t);
        t = null;
    }



    
    public static float RemainProgress(this PReTimer timer)
    {
        if (timer!=null)
        {
            return timer.leftTime / timer.pingTime;
        }
        return 0;
    }
    public static float BeInProgress(this PReTimer timer)
    {
        if (timer != null)
        {
            return timer.alreadyTime / timer.pingTime;
            
        }
        return 0;
    }
    public static int GetWayLength(this NavMeshAgent nav ,Transform target)
    {
        int len = 0;

        NavMeshPath path = new NavMeshPath();
        bool res= nav.CalculatePath(target.position, path);
        if (res==true)
        {
            len = path.GetCornersNonAlloc(path.corners);
        }
        else
        {
            len = -1;
        }
        //Debug.Log(len);
        return len;

    }
    public static float GetNavmeshWayLength(this NavMeshAgent nav, Transform target)
    {
        float len = 0;

        NavMeshPath path = new NavMeshPath();
        bool res = nav.CalculatePath(target.position, path);
        if (res == true)
        {
            len = path.GetCornersNonAlloc(path.corners);
            Vector3[] poss = path.corners;
            for (int i = 0; i < len; i++)
            {
                if (i+1<=len)
                {
                    len += Vector3.Distance(poss[i], poss[i + 1]);
                }
                
            }



        }
        else
        {
            len = 0;
        }
        //Debug.Log(len);
        return len;

    }
    public static float GetArriveTime(this NavMeshAgent ag,float speed,Transform tar)
    {
       float len=   ag.GetNavmeshWayLength(tar);
        if (len == 0)
        {
            return -1;
        }
        float time = len / speed;
        return time;

    }



    public static T GetNavArriveNear<T>(this List<T> lst,Transform tar)where T:MonoBehaviour
    {
        T go = null;
        int nearLen = int.MaxValue;
        for (int i = 0; i < lst.Count; i++)
        {
            NavMeshAgent nav = lst[i].GetComponent<NavMeshAgent>();
            int len = nav.GetWayLength(tar);
            if (len==-1)
            {
                continue;
            }
            if (len<nearLen)
            {
                nearLen = len;
                go = lst[i];
            }

        }
        return go;



    }
    public static T GetNavArriveNear<T>(this T []lst, Transform tar) where T : MonoBehaviour
    {

        T go = null;
        int nearLen = int.MaxValue;
        for (int i = 0; i < lst.Length; i++)
        {
            NavMeshAgent nav = lst[i].GetComponent<NavMeshAgent>();
            int len = nav.GetWayLength(tar);
            if (len == -1)
            {
                continue;
            }
            if (len < nearLen)
            {
                nearLen = len;
                go = lst[i];
            }

        }
        return go;


    }

    public static T GetNavArrive2Near<T>(this NavMeshAgent nav, T[] lst) where T : UnityEngine.Component
    {

        T go = null;
        int nearLen = int.MaxValue;
        for (int i = 0; i < lst.Length; i++)
        {
            Transform t = lst[i].transform;
            int len = nav.GetWayLength(t);
            if (len == -1)
            {
                continue;
            }
            if (len < nearLen)
            {
                nearLen = len;
                go = lst[i];
            }

        }
        return go;


    }

    public static T GetNavArrive3Near<T>(this Transform tr, T[] lst) where T : UnityEngine.Component
    {
        float distane = Vector3.Distance(tr.position, lst[0].transform.position) ;
        int index = 0;
        for (int i = 0; i <lst.Length; i++)
        {
            
            if (distane>= Vector3.Distance(tr.position, lst[i].transform.position))
            {
                distane = Vector3.Distance(tr.position, lst[i].transform.position);
                index = i;
            }



        }

        return lst[index];


    }
    public static string LogStr(this object o,LogColor logColor=LogColor.Red)
    {
        switch (logColor)
        {
            case LogColor.Red:

               return    string.Format("<color=#EC1111>{0}</color>", o.ToString());
               
            case LogColor.Blue:
                return string.Format("<color=#1155EC>{0}</color>", o.ToString());
            case LogColor.Green:
                return string.Format("<color=#57EC11>{0}</color>", o.ToString());
            case LogColor.Purple:
                return string.Format("<color=#8011EC>{0}</color>", o.ToString());
            case LogColor.Yellow:
                return string.Format("<color=#EBFF07>{0}</color>", o.ToString());

        }

        return null;
    }


}

public enum LogColor{
    Red,
    Blue,
    Green,
    Purple,
    Yellow,







}


