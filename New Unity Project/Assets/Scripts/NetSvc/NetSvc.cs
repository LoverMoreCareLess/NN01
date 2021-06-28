using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using PENet;
using NetProtol;

public class NetSvc : DoSingleMonoByGameRoot<NetSvc>
{
    IOCPNet<ClientToken, GameMsg> ioNet;
    Queue<GameMsg> msgQue = new Queue<GameMsg>();
    public void Init()
    {
        ioNet = new IOCPNet<ClientToken, GameMsg>();
        ioNet.StartAsClient(SeverSetting.Ip, SeverSetting.Port);

    }
   

    private void Update()
    {
        while (msgQue.Count>0)
        {
            lock (msgQue)
            {
                GameMsg msg = msgQue.Dequeue();
                HandlerMsg(msg);
            }
           
        }
       
    }

    private void HandlerMsg(GameMsg msg)
    {
        switch (msg.successCode)
        {
            case SuccessCode.none:
                break;
            case SuccessCode.ReqLogin:
                break;
            default:
                break;
        }
    }


    public override void Clear()
    {
        base.Clear();
    }
    public void AddMsgQue(GameMsg msg)
    {
        lock (msgQue)
        {
            msgQue.Enqueue(msg);

        }
    }


    public void SendMsg(GameMsg msg)
    {
        if (msg!=null&&ioNet.token!=null)
        {
            ioNet.token.SendMsg(msg);
        }
        else
        {
            //todo 服务器没有连接
        }


    }




}
