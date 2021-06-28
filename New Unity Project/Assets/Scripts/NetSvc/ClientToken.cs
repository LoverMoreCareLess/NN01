using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using PENet;
using NetProtol;

public class ClientToken : IOCPToken<GameMsg>
{
    protected override void OnConnected()
    {

        Debug.Log("连接成功");
    }

    protected override void OnDisConnected()
    {
        Debug.Log("断开连接");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        Debug.Log(msg.successCode);

    }
}
