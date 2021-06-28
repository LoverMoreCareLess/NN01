using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class Waiter
{
    public bool isStop;
    public Func<bool> IsTrue;
    public Action onTrueAction;

    public Waiter() { }

    public Waiter( Func<bool> isTrue, Action onTrueAction)
    {
        this.isStop = false;
        IsTrue = isTrue;
        this.onTrueAction = onTrueAction;
    }

    public void ClassReset()
    {
        IsTrue = null;
        isStop = false;
        onTrueAction = null;
    }

    public void DoWait()
    {
        if (isStop==true)
        {
            
            return;
        }

        if (IsTrue()==true)
        {
            if (onTrueAction!=null)
            {
                onTrueAction();
                onTrueAction = null;
                isStop = true;

            }
        }



    }



}



public class WaitSe : ProvidingServices<WaitSe>
{
    private List<Waiter> _cacheLst = new List<Waiter>();
    private List<Waiter> _workLst = new List<Waiter>();
   // ClassPool<Waiter> _cp;

    public override void Init()
    {
        base.Init();
       // _cp = ObjPoolMgr.Instance.GetOrCreateClassObjectPool<Waiter>(20);
    }

    private void Update()
    {
        DoWork();


    }

    private void DoWork()
    {
        for (int i = 0; i < _cacheLst.Count; i++)
        {
            lock (_workLst)
            {
                _workLst.Add(_cacheLst[i]);
            }
        }
        _cacheLst.Clear();
        for (int i = 0; i < _workLst.Count; i++)
        {
            _workLst[i].DoWait();
        }
        for (int i = 0; i < _workLst.Count; i++)
        {
            if (_workLst[i].isStop == true)
            {
                lock (_workLst)
                {
                    Waiter rec = _workLst[i];
                    _workLst.Remove(_workLst[i]);
                    //_cp.Set(rec);
                }
            }
        }
    }
    public Waiter Create(Func<bool> isTrue)
    {
        //Waiter w = _cp.Get();
        Waiter w = new Waiter();
        w.ClassReset();
        w.IsTrue = isTrue;
        return w;
    }


    public  Waiter AddWaiter(Func<bool> isTrue, Action onTrueAction)
    {
        //Waiter w = new Waiter(isTrue, onTrueAction);
        // Waiter w = _cp.Get();
        Waiter w = new Waiter();
        w.ClassReset();
        w.IsTrue = isTrue;
        w.onTrueAction = onTrueAction;
        _cacheLst.Add(w);

        return w;
    }

    public Waiter Add(Waiter w,  Action end)
    {
        w.onTrueAction = end;

        _cacheLst.Add(w);
        return w;
    }

    public void Remove(Waiter w)
    {
        if (_workLst.Contains(w))
        {
           lock(_workLst)
            {
                _workLst.Remove(w);
            }
        }
        else
        {
            if (_cacheLst.Contains(w))
            {
                lock (_cacheLst)
                {
                    _cacheLst.Remove(w);
                }
            }
        }

    }



}
