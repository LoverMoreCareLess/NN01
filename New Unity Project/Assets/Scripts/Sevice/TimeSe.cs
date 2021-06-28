using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum TimeUnit
{
    MilliSecond,//毫秒
    Second,//秒
    Minute,//分
    Hour,//小时
    Day,//天




}

public class InPreTimerAaction
{

    public float timer;


    public Action act;

    public  bool isExced = false;

    public InPreTimerAaction(float timer, Action act)
    {
        this.timer = timer;
        this.act = act;
        isExced = false;
    }
}


/// <summary>
/// 时间类
/// </summary>
public class PReTimer
{
    public float totalTime;
    public Action callEevent;
    public int count;
    public float pingTime;
    public int timerId;
    public float alreadyTime;//已经过了的时间
    public float leftTime;//剩余时间
    public float startTime;//开始时间
    public float suspendTime;//暂停时间
    public bool isPaused;//是否暂停
    public bool isStoped;//是否停止计时
    public string Tag = string.Empty;
    public float popTime = 0;
    public float arTime = 0;
    public List<InPreTimerAaction> Timerlsts = new List<InPreTimerAaction>();



    public PReTimer(float totalTime, Action callEevent, int count, float pingTime, int timerId)
    {
        this.totalTime = totalTime;
        this.callEevent = callEevent;
        this.count = count;
        this.pingTime = pingTime;
        this.timerId = timerId;
        isPaused = false;
        isStoped = false;


    }
    public PReTimer()
    {
        isPaused = false;
        isStoped = false;
    }


 

    public void SaveSuspendTime()
    {

        this.suspendTime = 0;





    }

    public void ClassReset()
    {
        this.totalTime = 0;
        this.callEevent = null;
        this.count = 0;
        this.pingTime = 0;
        this.timerId = -1;
        isPaused = false;
        isStoped = false;
    }
}








public class TimeSe : ProvidingServices<TimeSe>
{
    List<PReTimer> _cacheTimerLst = new List<PReTimer>();//缓存区
    List<PReTimer> _workTimerLst = new List<PReTimer>();//实际运行区
    int timerId;//当前记录的timerID;
    static readonly string lockObj = "lock";
    List<int> _timerIdLst = new List<int>();//存放timer 的id;
    List<int> _recIdLst = new List<int>();

  // public ClassPool<PReTimer> _cp;

        

    public override void Init()
    {
        base.Init();
       // _cp = ObjPoolMgr.Instance.GetOrCreateClassObjectPool<PReTimer>(50);

    }


    public void DoPause(PReTimer t)
    {
        t.isPaused = true;
    }

    public void DoGoOn(PReTimer t)
    {

        t.totalTime = Time.realtimeSinceStartup * 1000 + t.leftTime;
        t.startTime = Time.realtimeSinceStartup * 1000 - t.alreadyTime;
        t.isPaused = false;
        

    }
    private void Update()
    {
        WorkTimer();
        RecTimerId();
        
    }
    void WorkTimer()
    {

        for (int i = 0; i < _cacheTimerLst.Count; i++)
        {
            lock (_workTimerLst)
            {
                _workTimerLst.Add(_cacheTimerLst[i]);
            }
        }
        _cacheTimerLst.Clear();


        for (int i = 0; i < _workTimerLst.Count; i++)
        {
            PReTimer timer = _workTimerLst[i];

            if (timer.isPaused == true || timer.isStoped == true)
            {
                continue;
            }
            if (Time.realtimeSinceStartup * 1000 < timer.totalTime)
            {
                timer.leftTime = timer.totalTime - Time.realtimeSinceStartup * 1000;
                timer.alreadyTime = Time.realtimeSinceStartup * 1000 - timer.startTime;

                //Debug.LogError(timer.alreadyTime);
                if (timer.Timerlsts != null)
                {
                    for (int j = 0; j < timer.Timerlsts.Count; j++)
                    {
                        if (timer.Timerlsts[j].timer * 1000 <= timer.alreadyTime)
                        {
                            try
                            {
                                if (timer.Timerlsts[j].isExced==false)
                                {
                                    if (timer.Timerlsts[j].act != null)
                                    {
                                        timer.Timerlsts[j].act();
                                        timer.Timerlsts[j].isExced = true;
                                    }
                                }
                               
                            }
                            catch (Exception ex)
                            {

                                Debug.Log(ex);
                                Debug.Log(ex.Message);
                            }
                        }
                    }
                }




                continue;
            }




            else if (Time.realtimeSinceStartup * 1000 >= timer.totalTime)
            {
                try
                {
                    timer.leftTime = 0;
                    if (timer.callEevent != null)
                    {
                        timer.callEevent();
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("运行回调错误" + ex.Message);
                    Debug.Log(ex);
                }
                if (timer.count == 1)
                {
                    lock (_workTimerLst)
                    {
                        
                        timer.isStoped = true;



                    }

                    lock (_recIdLst)
                    {
                        _recIdLst.Add(timerId);
                    }



                }
                else
                {
                    if (timer.count != 0)
                    {
                        timer.count--;
                    }
                   
                    timer.totalTime += timer.pingTime;
                    timer.startTime = Time.realtimeSinceStartup * 1000;
                    for (int j = 0; j < timer.Timerlsts.Count; j++)
                    {

                        timer.Timerlsts[j].isExced = false;


                    }
                }





            }




        }
        for (int i = 0; i < _workTimerLst.Count; i++)
        {
            PReTimer timer = _workTimerLst[i];
            if (timer.isStoped)
            {
                _workTimerLst.Remove(timer);
                
                //_cp.Set(timer);

            }


        }
        

    }

    public PReTimer AddTimerInPreTimer(Action evt, float pingTime, TimeUnit timeUnit = TimeUnit.MilliSecond, int count = 1,params object[] args)
    {

        pingTime = GetTimeUnit(pingTime, timeUnit);
        int timeId = GetTimerId();
        //float totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        //ReTimer timer = new ReTimer(totalTime, evt, count, pingTime, timeId);
        //PReTimer timer = new PReTimer();
        //PReTimer timer = _cp.Get();
        PReTimer timer = new PReTimer();
        timer.ClassReset();
        timer.totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        timer.startTime = Time.realtimeSinceStartup * 1000;
        timer.callEevent = evt;
        timer.count = count;
        timer.pingTime = pingTime;
        timer.timerId = timerId;
        timer.alreadyTime = 0;



        timer.leftTime = timer.totalTime;
        for (int i = 0; i < args.Length; i++)
        {
            timer.Timerlsts.Add(args[i] as InPreTimerAaction);
        }



        _cacheTimerLst.Add(timer);
        _timerIdLst.Add(timeId);

        return timer;




    }



    #region//TimeSys提供外部调用方法
    /// <summary>
    /// 添加延迟调用时间
    /// </summary>
    /// <param name="evt">事件函数</param>
    /// <param name="pingTime">延迟调用时间</param>
    /// <param name="timeUnit">时间单位</param>
    /// <param name="count">调用次数,若设置为0无限次数调用</param>
    /// <returns></returns>
    public int AddTimer(out PReTimer t, Action evt, float pingTime, TimeUnit timeUnit = TimeUnit.MilliSecond, int count = 1)
    {
        pingTime = GetTimeUnit(pingTime, timeUnit);
        int timeId = GetTimerId();
        //float totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        //ReTimer timer = new ReTimer(totalTime, evt, count, pingTime, timeId);
        // PReTimer timer = new PReTimer();
        // PReTimer timer = _cp.Get();
        PReTimer timer = new PReTimer();
        timer.ClassReset();
        timer.totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        timer.startTime = Time.realtimeSinceStartup * 1000;
        timer.callEevent = evt;
        timer.count = count;
        timer.pingTime = pingTime;
        timer.timerId = timerId;
        timer.alreadyTime = 0;



        timer.leftTime = timer.totalTime;
        _cacheTimerLst.Add(timer);
        _timerIdLst.Add(timeId);
        t = timer;
        return timeId;


    }
    public PReTimer AddTimerReturnTimer(Action evt, float pingTime, TimeUnit timeUnit = TimeUnit.MilliSecond, int count = 1)
    {

        pingTime = GetTimeUnit(pingTime, timeUnit);
        int timeId = GetTimerId();
        //float totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        //ReTimer timer = new ReTimer(totalTime, evt, count, pingTime, timeId);
        //PReTimer timer = new PReTimer();
        //PReTimer timer = _cp.Get();
        PReTimer timer = new PReTimer();
        timer.ClassReset();
        timer.totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        timer.startTime = Time.realtimeSinceStartup * 1000;
        timer.callEevent = evt;
        timer.count = count;
        timer.pingTime = pingTime;
        timer.timerId = timerId;
        timer.alreadyTime = 0;



        timer.leftTime = timer.totalTime;
        _cacheTimerLst.Add(timer);
        _timerIdLst.Add(timeId);

        return timer;



    }
    public int Add(out PReTimer t,PReTimer timer, Action evt)
    {
        int timeId = GetTimerId();
        timer.timerId = timeId;
        timer.callEevent = evt;
        _cacheTimerLst.Add(timer);
        _timerIdLst.Add(timeId);

        t = timer;
        return timerId;
    }




    public int AddTimer(Action evt, float pingTime, TimeUnit timeUnit = TimeUnit.MilliSecond, int count = 1)
    {
        pingTime = GetTimeUnit(pingTime, timeUnit);
        int timeId = GetTimerId();
        //float totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        //ReTimer timer = new ReTimer(totalTime, evt, count, pingTime, timeId);
        //PReTimer timer = new PReTimer();
        //PReTimer timer = _cp.Get();

        PReTimer timer = new PReTimer();
        timer.ClassReset();

        timer.totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        timer.startTime = Time.realtimeSinceStartup * 1000;
        timer.callEevent = evt;
        timer.count = count;
        timer.pingTime = pingTime;
        timer.timerId = timerId;
        timer.alreadyTime = 0;
        timer.isPaused = false;
        timer.isStoped = false;


        timer.leftTime = timer.totalTime;
        _cacheTimerLst.Add(timer);
        _timerIdLst.Add(timeId);

        return timeId;


    }











    /// <summary>
    /// 替换掉已经注册的延迟事件
    /// </summary>
    /// <param name="id">传入timerId</param>
    /// <param name="evt">传入替换的事件</param>
    /// <param name="pingTime">传入延迟事件</param>
    /// <param name="timeUnit">传入时间单位</param>
    /// <param name="count">传入运行次数,为0则无限运行</param>
    /// <returns></returns>
    public bool ReplaceTimerEvent(int id, Action evt, float pingTime, TimeUnit timeUnit = TimeUnit.MilliSecond, int count = 1)
    {
        pingTime = GetTimeUnit(pingTime, timeUnit);
        float totalTime = Time.realtimeSinceStartup * 1000 + pingTime;
        PReTimer timer = new PReTimer(totalTime, evt, count, pingTime, id);
        bool isComplete = false;
        for (int i = 0; i < _workTimerLst.Count; i++)
        {
            if (_workTimerLst[i].timerId == id)
            {
                _workTimerLst[i] = timer;
                isComplete = true;
                break;
            }
        }
        if (!isComplete)
        {
            for (int i = 0; i < _cacheTimerLst.Count; i++)
            {
                if (_cacheTimerLst[i].timerId == id)
                {
                    _cacheTimerLst[i] = timer;
                    isComplete = true;
                    break;
                }

            }
        }



        return isComplete;

    }
    /// <summary>
    /// 通过timerId移除该延迟调用
    /// </summary>
    /// <param name="id">传入timerId</param>
    /// <returns></returns>
    public bool RemoveTimer(int id)
    {
        bool exist = false;

        for (int i = 0; i < _workTimerLst.Count; i++)
        {
            PReTimer timer = _workTimerLst[i];
            if (timer.timerId == id)
            {
                lock (_workTimerLst)
                {
                    //_workTimerLst.RemoveAt(i);
                    timer.isStoped = true;
                    timer.callEevent = null;
                }
                for (int j = 0; j < _timerIdLst.Count; j++)
                {
                    if (_timerIdLst[j] == id)
                    {
                        lock (_timerIdLst) { _timerIdLst.Remove(_timerIdLst[j]); break; }


                    }
                }
                exist = true;
                break;


            }



        }
        if (exist == false)
        {
            for (int i = 0; i < _cacheTimerLst.Count; i++)
            {
                PReTimer timer = _cacheTimerLst[i];
                if (timer.timerId == id)
                {
                    lock (_cacheTimerLst)
                    {
                        _cacheTimerLst.Remove(timer);
                    }

                    for (int j = 0; j < _timerIdLst.Count; j++)
                    {
                        if (_timerIdLst[j] == id)
                        {
                            lock (_timerIdLst)
                            {
                                _timerIdLst.RemoveAt(j); break;
                            }

                        }
                    }
                    exist = true;
                    break;


                }
            }
        }



        return exist;




    }
    public PReTimer GetTimer(int id)
    {
        PReTimer t = null;
        for (int i = 0; i < _workTimerLst.Count; i++)
        {
            PReTimer timer = _workTimerLst[i];
            if (timer.timerId == id)
            {
                t = timer;
                return timer;
            }

        }
        return null;
    }

   


    public void StopAll()
    {
        lock (_cacheTimerLst)
        {
            for (int i = 0; i < _cacheTimerLst.Count; i++)
            {
                if (_cacheTimerLst[i].isPaused==false)
                {
                    DoPause(_cacheTimerLst[i]);
                }
            }


        }
        lock (_workTimerLst)
        {
            for (int i = 0; i < _workTimerLst.Count; i++)
            {
                if (_workTimerLst[i].isPaused==false)
                {
                    DoPause(_workTimerLst[i]);
                }
            }
        }

    }
    public void GoOnAll()
    {

        lock (_cacheTimerLst)
        {
            for (int i = 0; i < _cacheTimerLst.Count; i++)
            {
                if (_cacheTimerLst[i].isPaused == true)
                {
                    DoGoOn(_cacheTimerLst[i]);
                }
            }


        }
        lock (_workTimerLst)
        {
            for (int i = 0; i < _workTimerLst.Count; i++)
            {
                if (_workTimerLst[i].isPaused == true)
                {
                    DoGoOn(_workTimerLst[i]);
                }
            }
        }

    }





    #endregion
    #region//提供相关工具

    void RecTimerId()
    {
        for (int i = 0; i < _recIdLst.Count; i++)
        {
            int tid = _recIdLst[i];
            for (int k = 0; k < _timerIdLst.Count; k++)
            {
                if (_timerIdLst[k] == tid)
                {
                    _timerIdLst.RemoveAt(k); break;
                }
            }


        }
        _recIdLst.Clear();


    }
    private float GetTimeUnit(float pingTime, TimeUnit timeUnit)
    {
        if (timeUnit != TimeUnit.MilliSecond)
        {
            switch (timeUnit)
            {

                case TimeUnit.Second:
                    pingTime *= 1000;
                    break;
                case TimeUnit.Minute:
                    pingTime *= 1000 * 60;
                    break;
                case TimeUnit.Hour:
                    pingTime *= 1000 * 60 * 60;
                    break;
                case TimeUnit.Day:
                    pingTime *= 1000 * 60 * 60 * 24;
                    break;

            }

        }

        return pingTime;
    }














    int GetTimerId()
    {
        lock (lockObj)
        {
            timerId += 1;

        }
        while (true)
        {
            if (timerId == int.MaxValue)
            {
                timerId = 0;
            }
            bool isUsed = false;
            for (int i = 0; i < _timerIdLst.Count; i++)
            {
                if (timerId == _timerIdLst[i])
                {
                    isUsed = true; break;
                }
            }
            if (isUsed)
            {
                timerId += 1;
            }
            else
            {
                break;
            }
        }

        return timerId;

    }
    #endregion



    public override void Clear()
    {

        StopAll();
        _cacheTimerLst.Clear();
        _workTimerLst.Clear(); 
       
       _timerIdLst .Clear();
        _recIdLst.Clear();
    }


}
