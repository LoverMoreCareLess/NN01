﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using PENet;
using NetProtol;

public class GameRoot : MonoBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        NetSvc.One.Init();

        AudMgr.One.Init();

        UIMgr.One.Init();

        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {

            ResReq("22222221", "2222");

        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            UIMgr.One.ShowUI(UIID.MainForm,OpenUIAct.Bigger);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {

            UIMgr.One.CloseUI(UIID.MainForm);

        }
    }
    private void LoginReq(string name,string key)
    {
        GameMsg gm = new GameMsg();
        gm.successCode = SuccessCode.ReqLogin;
        gm.reqLogin = new ReqLogin();
        gm.reqLogin.userName = name;
        gm.reqLogin.key = key;
        NetSvc.One.SendMsg(gm);
    }

    private void ResReq(string name, string key)
    {
        GameMsg gm = new GameMsg();
        gm.successCode = SuccessCode.Reqregister;
        gm.reqregister = new Reqregister ();
        gm.reqregister.userName = name;
        gm.reqregister.key = key;
        NetSvc.One.SendMsg(gm);
    }


}
