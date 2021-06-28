using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class Mover
{
    public float minDis;
    public Transform target;
    public Vector3 tarVec;
    public GameObject moveObj;
    public Action onCompeleAction;
    public NavMeshAgent nav;
    public float speed;
    public bool isLoop = false;
    public int moveTypeId = 0;
    public bool isPause=false;
    public float waitTime = 0;
    public float totalTime = 0;
    public bool isInitTime = false;
    public Action removeAction;
    public float WaiteTimer = 0;

    public Mover() { }

    public Mover(float minDis, Transform target, GameObject moveObj, Action onCompeleAction, float speed,Action remove,bool isLoop)
    {
        this.minDis = minDis;
        this.target = target;
        this.moveObj = moveObj;
        this.onCompeleAction = onCompeleAction;
        this.speed = speed;
        this.isLoop = isLoop;
        moveTypeId = 0;
        isPause = false;
        isInitTime = false;
        nav = moveObj.GetComponent<NavMeshAgent>();
        this.removeAction = remove;

        

    }

    public Mover(float minDis, Vector3 target, GameObject moveObj, Action onCompeleAction, float speed,Action remove,bool isLoop)
    {
        this.minDis = minDis;
        this.tarVec = target;
        this.moveObj = moveObj;
        this.onCompeleAction = onCompeleAction;
        this.speed = speed;
        this.isLoop = isLoop;
        moveTypeId = 1;
        isPause = false;
        isInitTime = false;
        nav = moveObj.GetComponent<NavMeshAgent>();
        this.removeAction = remove;

    }

    private void DomovePos()
    {
       
        if (isPause)
        {
            if (nav.isStopped==false)
            {
                nav.isStopped = true;
            }
            
            return;
        }

        if (nav==null)
        {
            Debug.Log("寻路组件为空");
        }
        if (nav.isStopped)
        {
            nav.isStopped = false;
        }
        nav.speed = speed;
        nav.SetDestination(tarVec);
        if (Vector3.Distance(tarVec,moveObj.transform.position)<=minDis)
        {
            
            if (isInitTime==false&&waitTime!=0)
            {
                totalTime = waitTime;

                isInitTime = true;
            }
            nav.SetDestination(moveObj.transform.position);
            if (waitTime==0)
            {
                if (isLoop == false)
                {
                  
                    if (removeAction != null)
                    {
                        removeAction();
                        //removeAction = null;
                    }

                }
                
                if (onCompeleAction != null)
                {
                    
                    onCompeleAction();
                    //onCompeleAction = null;
                }
                
               
                
               
                


            }
            else
            {
                WaiteTimer += Time.deltaTime;


                if (WaiteTimer > totalTime)
                {
                    
                    if (isLoop == false)
                    {
                        if (removeAction != null)
                        {
                            removeAction();
                            //removeAction = null;
                        }

                    }
                    if (onCompeleAction != null)
                    {
                        
                        onCompeleAction();
                        //onCompeleAction = null;
                    }

                   
                  
                    
                  
                    

                  
                }
            }


        }

    }
    private void DoMoveTrans()
    {

        if (isPause)
        {
            if (nav.isStopped == false)
            {
                nav.isStopped = true;
            }
            return;
        }

        if (nav == null)
        {
            Debug.Log("寻路组件为空");
        }
        if (nav.isStopped)
        {
            
            nav.isStopped = false;
        }
        nav.speed = speed;
        try
        {
            nav.SetDestination(target.transform.position);
        }
        catch (Exception ex)
        {

            Debug.Log(ex);
            
            Debug.Log(ex.Message);
        }
        
        if (Vector3.Distance(target.transform.position, moveObj.transform.position) <= minDis)
        {
            if (isInitTime == false && waitTime != 0)
            {
                totalTime =  waitTime;
                isInitTime = true;
            }
            nav.SetDestination(moveObj.transform.position);
            if (waitTime == 0)
            {

                if (isLoop == false)
                {
                    if (removeAction != null)
                    {
                        
                        removeAction();
                        //removeAction = null;
                    }

                }
              
                if (onCompeleAction != null)
                {
                    
                    onCompeleAction();
                    //onCompeleAction = null;
                }
                

            }
            else
            {
                WaiteTimer += Time.deltaTime;

                if (WaiteTimer > totalTime)
                {
                    if (isLoop == false)
                    {
                        if (removeAction != null)
                        {
                            removeAction();
                            //removeAction = null;
                        }

                    }

                    if (onCompeleAction != null)
                    {
                        onCompeleAction();
                        //onCompeleAction = null;
                    }
                    

                }
            }


        }


    }
    public void NavMeshAgentWork()
    {
        switch (moveTypeId)
        {
            case 0: DoMoveTrans(); break;
            case 1: DomovePos();break;
        }
    }

    public void ClassReset()
    {
        this.minDis = 0;
        this.tarVec = default(Vector3);
        this.target = null;
        this.moveObj = null;
        this.onCompeleAction = null;
        this.speed = 0;
        this.isLoop = false;
        moveTypeId = 1;
        isPause = false;
        isInitTime = false;
        nav = null;
        this.removeAction = null;
        waitTime = 0;
        totalTime = 0;

    }
}









public class NavSe : ProvidingServices<NavSe>
{
    List<Mover> _cacheLst = new List<Mover>();
    List<Mover> _workLst = new List<Mover>();
    Dictionary<GameObject, Mover> _obj2MoverDic = new Dictionary<GameObject, Mover>();
    //ClassPool<Mover> _cp;
    public bool isStop = false;
    public override void Init()
    {
        base.Init();
        // Debug.Log("11111");
        //_cp = ObjPoolMgr.Instance.GetOrCreateClassObjectPool<Mover>(70);
        isStop = false;
        _cacheLst.Clear();
        _workLst.Clear();
        _obj2MoverDic.Clear();
    }


    private void FixedUpdate()
    {
        if (isStop)
        {
            return;
        }



        for (int i = 0; i < _cacheLst.Count; i++)
        {
            lock (_workLst)
            {
                _workLst.Add(_cacheLst[i]);
            }
        }
        _cacheLst.Clear();
        for (int i = 0; i <_workLst.Count; i++)
        {
            _workLst[i].NavMeshAgentWork();
        }

       


    }

    public void Remove(GameObject go,bool isRec=true)
    {
        Mover m=null;
        _obj2MoverDic.TryGetValue(go, out m);
        
        
        if (m!=null)
        {
            m.isPause = true;
            m.nav.isStopped = true;
            lock (_obj2MoverDic)
            {
                _obj2MoverDic.Remove(go);
            }

            if (_workLst.Contains(m))
            {
                lock (_workLst)
                {
                    _workLst.Remove(m);
                }
            }
            else
            {
                if (_cacheLst.Contains(m))
                {
                    lock (_cacheLst)
                    {
                        _cacheLst.Remove(m);
                    }
                }
            }


        }
        if (isRec)
        {
            //if (m!=null)
            //{
            //    _cp.Set(m);
            //}
            
        }
        

    }

    public void Remove(Mover m)
    {
        m.isPause = true;
        m.nav.isStopped = true;
        if (_workLst.Contains(m))
        {
            lock (_workLst)
            {

                _workLst.Remove(m);


            }
        }
        
        foreach (Mover mover in _obj2MoverDic.Values)
        {
            if (m==mover)
            {
                lock (_obj2MoverDic)
                {
                    _obj2MoverDic.Remove(m.moveObj);
                }
            }
        }
        //if (m!=null)
        //{
        //    _cp.Set(m);
        //}
        

    }

    public void Add(Mover m)
    {
        lock (_obj2MoverDic)
        {
            _obj2MoverDic.Add(m.moveObj, m);
        }
        lock (_cacheLst)
        {
            _cacheLst.Add(m);
        }
        
    }
    public Mover TryAdd(Mover m,Action evt)
    {
        m.onCompeleAction = evt;
        Remove(m.moveObj);
        lock (_cacheLst)
        {
            _cacheLst.Add(m);
        }
        lock (_obj2MoverDic)
        {
            _obj2MoverDic.Add(m.moveObj, m);
        }
        return m;

    }
    public Mover CreateMover(GameObject go, Transform target, float speed = 2f, float dis = 1f, float waitTime = 0,bool l=false)
    {
        
        //Mover m = _cp.Get();
        Mover m = new Mover();
       // m.ClassReset();
        m.moveObj = go;
        m.target = target;
        m.speed = speed;
        m.minDis = dis;
        m.moveTypeId = 0;
        m.nav = go.GetComponent<NavMeshAgent>();
        m.removeAction = () => { Remove(go); };
        m.isLoop = l;
        m.isInitTime = false;
        m.isPause = false;
        m.totalTime = 0;
        m.waitTime = waitTime;

        return m;
    }

    public Mover CreateMover(GameObject go,Vector3 pos,  float speed = 2f, float dis = 1f, float waitTime = 0, bool l = false)
    {
        //if (_cp == null)
        //{
        //    Debug.Log("KONG");
        //    //_cp = ObjPoolMgr.Instance.GetOrCreateClassObjectPool<Mover>(50);

        //}

        // Mover m = _cp.Get();
        //m.ClassReset();
        Mover m = new Mover();
        m.moveObj = go;
        m.tarVec = pos;
        m.speed = speed;
        m.minDis = dis;
        m.moveTypeId = 1;
        m.nav = go.GetComponent<NavMeshAgent>();
        m.removeAction = () => { Remove(go); };
        m.isLoop = l;
        m.isInitTime = false;
        m.isPause = false;
        m.totalTime = 0;
        m.waitTime = waitTime;
        return m;
    }

    public Mover AddMover(GameObject go, Transform target, Action onComplete = null, bool isloop = false, float speed = 2f, float dis = 1f, float waitTime = 0)
    {
        Remove(go);
        Mover m = CreateMover(go, target, speed, dis, waitTime, isloop);
        m.onCompeleAction = onComplete;
        if (onComplete==null)
        {
            Debug.Log("kong");
        }
        
        Add(m);
        return m;
    }

    public Mover AddMover(GameObject go, Vector3 pos, Action onComplete = null, bool isloop = false, float speed = 2f, float dis = 1f, float waitTime = 0)
    {
        Remove(go);
        Mover m = CreateMover(go, pos, speed, dis, waitTime, isloop);
        m.onCompeleAction = onComplete;
        
        Add(m);
        return m;

    }
    public Mover AddRandomMover(GameObject go, Vector3 rangeA, Vector3 rangeB, float Speed = 2, float dis = 1, float waitTime = 0)
    {
        Debug.Log("1次");
        Vector3 v = GetCanMoveRandomPos(go, rangeA, rangeB);

        Mover m = AddMover(go, v, () => { m = AddRandomMover(go, rangeA, rangeB, Speed, dis, waitTime); }, false, Speed, dis, waitTime);

        NavMeshAgent nav = m.nav;
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(v, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            //Debug.Log("有路劲0");


        }
        else
        {
            m = AddRandomMover(go, rangeA, rangeB, Speed, dis, waitTime);
            //Debug.Log("没有路劲");
        }



        return m;
    }
    public Mover AddMoverP2P(List<Transform> poss,Action endAct, GameObject go, float Speed = 2, float dis = 1, float waitTime = 0, int nowIndex = 0)
    {
        int lastIndex = poss.Count-1;

        Mover m = AddMover(go, poss[nowIndex], () =>
        {
            if (nowIndex!=lastIndex)
            {
                nowIndex++;
              m=  AddMoverP2P(poss,endAct, go, Speed, dis, waitTime, nowIndex);
            }
            else
            {
                if (endAct!=null)
                {
                    endAct();
                }
            }



        }, false, Speed, dis,waitTime);

        return m;

    }

    public Mover AddMoverP2P(Transform [] poss, Action endAct, GameObject go, float Speed = 2, float dis = 1, float waitTime = 0, int nowIndex = 0)
    {
        int lastIndex = poss.Length-1;

        Mover m = AddMover(go, poss[nowIndex], () =>
        {
            if (nowIndex != lastIndex)
            {
                nowIndex++;
                m = AddMoverP2P(poss, endAct, go, Speed, dis, waitTime, nowIndex);
            }
            else
            {
                if (endAct != null)
                {
                    endAct();
                }
            }



        }, false, Speed, dis, waitTime);

        return m;

    }


    public Mover AddMoverP2P(Vector3[] poss, Action endAct, GameObject go, float Speed = 2, float dis = 1, float waitTime = 0, int nowIndex = 0)
    {
        int lastIndex = poss.Length - 1;

        Mover m = AddMover(go, poss[nowIndex], () =>
        {
            if (nowIndex != lastIndex)
            {
                nowIndex++;
                m = AddMoverP2P(poss, endAct, go, Speed, dis, waitTime, nowIndex);
            }
            else
            {
                if (endAct != null)
                {
                    endAct();
                }
            }



        }, false, Speed, dis, waitTime);

        return m;

    }
    public Mover AddMoverP2P(List<Vector3> poss, Action endAct, GameObject go, float Speed = 2, float dis = 1, float waitTime = 0, int nowIndex = 0)
    {
        int lastIndex = poss.Count - 1;

        Mover m = AddMover(go, poss[nowIndex], () =>
        {
            if (nowIndex != lastIndex)
            {
                nowIndex++;
                m = AddMoverP2P(poss, endAct, go, Speed, dis, waitTime, nowIndex);
            }
            else
            {
                if (endAct != null)
                {
                    endAct();
                }
            }



        }, false, Speed, dis, waitTime);

        return m;

    }
    public Mover AddMoverReverseMove2Tar(GameObject go, Transform target, Action onComplete = null, bool isloop = false, float speed = 2f, float dis = 1f, float waitTime = 0)
    {
        List<Transform> lst = new List<Transform>();
              
        Transform t = go.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            lst.Add(transform.GetChild(i));
        }
        for (int i = 0; i < lst.Count; i++)
        {
            lst[i].transform.parent = null;
        }
        transform.rotation = Quaternion.LookRotation(-transform.forward);
        for (int i = 0; i < lst.Count; i++)
        {
            lst[i].transform.parent = transform;
        }

        Mover m = AddMover(go, transform, () =>
        {


            if (onComplete != null)
            {
                onComplete();
            }
            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].transform.parent = null;
            }
            transform.rotation = Quaternion.LookRotation(-transform.forward);
            for (int i = 0; i < lst.Count; i++)
            {
                lst[i].transform.parent = transform;
            }


        },false,speed,dis,waitTime);


        return m;



    }







    public Mover AddfollowUpTargetLessRangeWithAction(out Waiter w, Action act,float range, GameObject go, Transform t, float speed = 2, float dis = 1, float followTime = 0)
    {

        Mover m = AddMover(go, t, null, true, speed, dis);
        float timer = 0;
      

       

        w = WaitSe.One.AddWaiter(() =>
        {
            timer += Time.deltaTime;
            if (timer>=followTime)
            {
                if (followTime!=0)
                {
                    act = null;
                    return true;
                }
            }

            if (Vector3.Distance(go.transform.position,t.transform.position)<=range)
            {
                return true;
            }
            return false;

        }, () => {

            if (act!=null)
            {
                act();
            }


        });


        return m;
    }
    public Mover AddfollowUpTargetLessRangeWithActionAndMoreRangeWithAction(out Waiter w, Action actLess,Action actMore , float rangeLess,float rangeMore, GameObject go, Transform t, float speed = 2, float dis = 1, float followTime = 0)
    {

        Mover m = AddMover(go, t, null, true, speed, dis);
        float timer = 0;

        w = WaitSe.One.AddWaiter(() =>
        {
            timer += Time.deltaTime;
            if (timer >= followTime)
            {
                if (followTime != 0)
                {
                    actLess = null;
                    actMore = null;
                    return true;
                }
            }

            if (Vector3.Distance(go.transform.position, t.transform.position) <= rangeLess)
            {
                return true;
            }
            if (Vector3.Distance(go.transform.position, t.transform.position) >= rangeMore)
            {
                actLess = null;
                if (actMore!=null)
                {
                    actMore();
                }
                return true;
            }


            return false;

        }, () =>
        {

            if (actLess != null)
            {
                actLess();
            }


        });


        return m;


    }


    public Mover AddPatrolMoverByPath(GameObject go, Vector3[] pos, Func<float> getSpeedAction, float dis = 1, float waitTime = 0)
    {

        int index = UnityEngine.Random.Range(0, pos.Length);
        Vector3 v = pos[index];
        float speed = getSpeedAction();
        Mover m = AddMover(go, v, () => { m = AddPatrolMoverByPath(go, pos, getSpeedAction, dis, waitTime); }, false, speed, dis, waitTime);

        return m;
    }


    public Mover AddPatrolMoverByPath(GameObject go, Vector3[] pos, float speed = 2, float dis = 1, float waitTime = 0)
    {

        int index = UnityEngine.Random.Range(0, pos.Length);
        Vector3 v = pos[index];

        Mover m = AddMover(go, v, () => { m = AddPatrolMoverByPath(go, pos, speed, dis, waitTime); }, false, speed, dis, waitTime);

        return m;
    }

    public Mover AddPatrolMoverByPath(GameObject go, Transform[] pos, Func<float> getSpeedAction, float dis = 1, float waitTime = 0)
    {

        int index = UnityEngine.Random.Range(0, pos.Length);
        Transform v = pos[index];
        float speed = getSpeedAction();
        Mover m = AddMover(go, v, () => { m = AddPatrolMoverByPath(go, pos, getSpeedAction, dis, waitTime); }, false, speed, dis, waitTime);

        return m;
    }

    public Mover AddPatrolMoverByPath(GameObject go, Transform[] pos, float speed = 2, float dis = 1, float waitTime = 0)
    {

        int index = UnityEngine.Random.Range(0, pos.Length);
        Transform v = pos[index];

        Mover m = AddMover(go, v, () => { m = AddPatrolMoverByPath(go, pos, speed, dis, waitTime); }, false, speed, dis, waitTime);

        return m;
    }
    public Mover AddEnemyInRound(GameObject go, float range, Vector3 v = default(Vector3), float speed = 2, float dis = 1, float waitTime = 0)
    {

       

        Vector3 tar = GetCanMoveRandomPos(go, range, v);
        Mover m = AddMover(go, tar, () => { m= AddEnemyInRound(go, range, v, speed, dis, waitTime);
            

        },false,speed,dis,waitTime);

        NavMeshAgent nav = m.nav;
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(v, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            //Debug.Log("有路劲0");


        }
        else
        {
             m = AddEnemyInRound(go, range, v, speed, dis, waitTime);
            //Debug.Log("没有路劲");
        }


        return m;

    }

    public Mover AddEnemyInRect(GameObject go, Transform center, float height,float weight,float Speed=2,float dis=1,float waitTime = 0)
    {


        Vector3 tar = GetCanMoveRandomPos(go, center.transform.position,height,weight);

        Mover m = AddMover(go, tar, () =>
        {
            m = AddEnemyInRect(go, center, height,weight,Speed,dis,waitTime);


        }, false, Speed, dis, waitTime);

        NavMeshAgent nav = m.nav;
        NavMeshPath path = new NavMeshPath();
        nav.CalculatePath(tar, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            //Debug.Log("有路劲0");


        }
        else
        {
            m = AddEnemyInRect(go, center, height, weight, Speed, dis, waitTime);
            //Debug.Log("没有路劲");
        }



        return null;

    }











    private Vector3 GetCanMoveRandomPos(GameObject go, Vector3 center,float h,float w)
    {


        bool iscanmoveto = false;
        NavMeshAgent nav = go.GetComponent<NavMeshAgent>();
        Vector3 v = default(Vector3);


        Vector3 ranga = default(Vector3);
        Vector3 rangb = default(Vector3);

        w = w / 2;
        h = h / 2;

        ranga = new Vector3(center.x + w, center.y, center.z+h);
        rangb = new Vector3(center.x - w, center.y, center.z - h);



        while (iscanmoveto == false)
        {
            float x = UnityEngine.Random.Range(ranga.x, rangb.x);
            float z = UnityEngine.Random.Range(ranga.z, rangb.z);
            float y = nav.transform.localPosition.y;
            v = new Vector3(x, y, z);
            NavMeshPath path = new NavMeshPath();
            if (nav.CalculatePath(v, path))
            {
                iscanmoveto = true; break;
            }


        }
        return v;



    }












    private Vector3 GetCanMoveRandomPos(GameObject go, float range, Vector3 v = default(Vector3))
    {

        bool iscanMoveto = false;
        NavMeshAgent nav = go.GetComponent<NavMeshAgent>();
        v = default(Vector3);
        if (v == default(Vector3))
        {
            v = go.transform.position;
        }
        float x1 = v.x;
        float z1 = v.z;
        float y1 = v.y;



        Vector3 tar = default(Vector3);
        while (iscanMoveto == false)
        {
            float x = UnityEngine.Random.Range((x1 - range), x1 + range);
            float z = UnityEngine.Random.Range((z1 - range), z1 + range);
            tar = new Vector3(x, y1, z);
            NavMeshPath path = new NavMeshPath();
            if (nav.CalculatePath(tar, path))
            {

                iscanMoveto = true; break;
            }


        }
        return tar;



    }
    private Vector3 GetCanMoveRandomPos(GameObject go, Vector3 a, Vector3 b)
    {


        bool iscanmoveto = false;
        NavMeshAgent nav = go.GetComponent<NavMeshAgent>();
        Vector3 v = default(Vector3);








        while (iscanmoveto == false)
        {
            float x = UnityEngine.Random.Range(a.x, b.x);
            float z = UnityEngine.Random.Range(a.z, b.z);
            float y = nav.transform.localPosition.y;
            v = new Vector3(x, y, z);
            NavMeshPath path = new NavMeshPath();
            if (nav.CalculatePath(v, path))
            {
                iscanmoveto = true; break;
            }


        }
        return v;



    }
    public override void Clear()
    {
        isStop = true;
        _cacheLst.Clear();
        _workLst.Clear();
        _obj2MoverDic.Clear();

    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        _cacheLst.Clear();
        _workLst.Clear();
        _obj2MoverDic.Clear();
    }

}

public static class ExpandNavMesh
{

    public static GameObject SetDestinationMy(this NavMeshAgent nav, Transform tar)
    {

        NavSe.One.Remove(nav.gameObject);
        nav.SetDestination(tar.transform.position);

        return nav.gameObject;
    }
    public static GameObject SetDestinationMy(this NavMeshAgent nav, Vector3 vector)
    {
        
            NavSe.One.Remove(nav.gameObject);
        

        nav.SetDestination(vector);

        return nav.gameObject;
    }

    public static Mover SetNavOn(this NavMeshAgent nav, Vector3 v, bool l = false, float speed = 2f, float dis = 1, Action act = null)
    {
        Mover m;
        m = NavSe.One.AddMover(nav.gameObject, v, act, l, speed, dis);

        return m;
    }
    public static Mover SetNavOn(this Transform t, Vector3 v, bool l = false, float speed = 2f, float dis = 1, Action act = null)
    {

        Mover m;
        m = NavSe.One.AddMover(t.gameObject, v, act, l, speed, dis);

        return m;


    }
    public static Mover SetNavOn(this GameObject t, Vector3 v, bool l = false, float speed = 2f, float dis = 1, Action act = null, float waitTime = 0)
    {

        Mover m;
        m = NavSe.One.AddMover(t, v, act, l, speed, dis, waitTime);

        return m;


    }

    public static Mover SetNavOn(this NavMeshAgent nav, Transform v, bool l = false, float speed = 2f, float dis = 1, Action act = null,float waitime=0)
    {
        Mover m;
        m = NavSe.One.AddMover(nav.gameObject, v, act, l, speed, dis, waitime);

        return m;
    }
    public static Mover SetNavOn(this Transform t, Transform v, bool l = false, float speed = 2f, float dis = 1, Action act = null, float waitTime = 0)
    {

        Mover m;
        m = NavSe.One.AddMover(t.gameObject, v, act, l, speed, dis, waitTime);

        return m;


    }
    public static Mover SetNavOn(this GameObject t, Transform v, bool l = false, float speed = 2f, float dis = 1, Action act = null, float waitTime = 0)
    {

        Mover m;
        m = NavSe.One.AddMover(t, v, act, l, speed, dis, waitTime);

        return m;


    }
    public static Mover GetAMover(this GameObject t, Transform v,  float speed = 2f, float dis = 1,  float waitTime = 0)
    {
        Mover m = NavSe.One.CreateMover(t, v, speed, dis, waitTime);
        return m;
    }
    public static Mover GetAMover(this Transform t, Transform v,  float speed = 2f, float dis = 1, float waitTime = 0)
    {
        Mover m = NavSe.One.CreateMover(t.gameObject, v, speed, dis, waitTime);
        return m;
    }
    public static Mover GetAMover(this NavMeshAgent nav, Transform v,  float speed = 2f, float dis = 1, float waitTime = 0)
    {
        Mover m = NavSe.One.CreateMover(nav.gameObject, v, speed, dis, waitTime);
        return m;
    }


    public static Mover OnComplete(this Mover m, Action evt)
    {
        
        m= NavSe.One.TryAdd(m,evt);

        return m;

    }

    public static void SetNavOff(this GameObject go)
    {

        NavSe.One.Remove(go);


    }

    public static void SetNavOff(this Transform go)
    {

        NavSe.One.Remove(go.gameObject);


    }

    public static void SetNavOff(this Mover m)
    {
        if (m!=null)
        {
            if (NavSe.One == null)
            {
                return;
            }
            NavSe.One.Remove(m.moveObj);
        }

        m = null;

    }
    public static void PauseMover(this Mover m)
    {
        if (m!=null)
        {
            m.isPause = true;
        }
        
    }

    public static void GoOnMover(this Mover m)
    {
        if (m!=null)
        {
            m.isPause = false;
        }
        
    }
    public static void NavSetinLayer(this NavMeshAgent nav, string layer)
    {
        int i = NavMesh.GetAreaFromName(layer);
        int z = 0x1 << i;

       
        if ((nav.areaMask&z)>0)
        {
            return;
        }

        nav.areaMask ^= 0x1 << i;
        
        
    }

    public static void NavSetOffLayer(this NavMeshAgent nav, string layer)
    {
        int i = NavMesh.GetAreaFromName(layer);
        int z = 0x1 << i;

        Debug.Log("取消1");
        if ((nav.areaMask & z) < 0)
        {
            Debug.Log("取消2");
            return;
        }
        Debug.Log("取消3");
        nav.areaMask  &=~(0x1 << i);
    }


    public static Vector3 GetNavMeshNearVector3Pos(this Transform pos, float dis, string layerName)
    {
        int i = NavMesh.GetAreaFromName(layerName);
        int l = 0x1 << i;
        NavMeshHit hit;
        bool res = NavMesh.SamplePosition(pos.position, out hit, dis, i);
        if (res==true)
        {
            return hit.position;
        }
        else
        {
            return pos.position;
        }

    }

    //public static int AgentLayerNameToValue(this string name)
    //{
    //    int idx = NavMesh.GetNavMeshLayerFromName(name);
    //    return 0x1 << idx;
    //}

    //// Nav层名字-->层索引，0、1、2、3、4
    //public static int AgentLayerNameToIndex(this string name)
    //{
    //    return NavMesh.GetNavMeshLayerFromName(name);
    //}

    //// 获取角色当前所在的层值，1、2、4、8、16
    //public static int GetAgentLayer(this NavMeshAgent agent)
    //{
    //    NavMeshHit hit;
    //    // 不要使用agent.raduis为采样范围，因为当该值为0时，函数将返回0
    //    bool reach = NavMesh.SamplePosition(agent.transform.position, out hit, 1f, -1);
    //    return hit.mask;
    //}

    //public static Vector3 SampleNavMeshPosition(this Vector3 logicPosition, out bool reachable)
    //{
    //    NavMeshHit hit;
    //    reachable = NavMesh.SamplePosition(logicPosition, out hit, 1f, -1);
    //    return reachable ? hit.position : logicPosition;
    //}

    //// 开启导航层
    //public static void EnableNavMeshLayer(this NavMeshAgent agent, string layerName)
    //{
    //    if (agent == null)
    //        return;

    //    int layerValue = NavMesh.GetNavMeshLayerFromName(layerName);
    //    if (layerValue == -1)
    //        return;

    //    int mask = agent.walkableMask | 0x1 << layerValue;
    //    WalkArbiter.SetWalkableMask(agent, mask);
    //}

    //// 关闭导航层
    //public static void DisableNavMeshLayer(this NavMeshAgent agent, string layerName)
    //{
    //    if (agent == null)
    //        return;

    //    int layerValue = NavMesh.GetNavMeshLayerFromName(layerName);
    //    if (layerValue == -1)
    //        return;

    //    int mask = agent.walkableMask & ~(0x1 << layerValue);

    //    WalkArbiter.SetWalkableMask(agent, mask);
    //}

    //// 检查某个层是否为开启
    //public static bool IsNavMeshLayerOpen(this NavMeshAgent agent, string layerName)
    //{
    //    int layerValue = NavMesh.GetNavMeshLayerFromName(layerName);
    //    if (layerValue == -1)
    //        return true;

    //    int ret = agent.walkableMask & (0x1 << layerValue);
    //    return ret > 0 ? true : false;
    //}
}