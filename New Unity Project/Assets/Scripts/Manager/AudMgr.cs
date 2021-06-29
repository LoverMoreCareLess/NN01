using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class AudMgr : DoSingleMonoByClassName<AudMgr>
{
    AudioSource bgAs;
    AudioSource clickAs;
    Dictionary<string, AudioClip> audClicpsDic = new Dictionary<string, AudioClip>();

    public override void Init()
    {
        base.Init();
        isDontDestory = true;
       


    }
    private void AudSounceInit()
    {
        GameObject g1 = new GameObject();
        GameObject g2 = new GameObject();
        g1.name = "BG";
        g2.name = "Click";
        g1.AddComponent<AudioSource>().loop = true;
        g2.AddComponent<AudioSource>().loop = false;
        g1.SetFather(this.transform);
        g2.SetFather(this.transform);

    }




    public void PlayBg(string name)
    {
        if (audClicpsDic.ContainsKey(name))
        {
            bgAs.clip = audClicpsDic[name];
            Play(bgAs, true);

        }
        else
        {
           



        }


    }





    public void PlayClick(string name)
    {
        if (audClicpsDic.ContainsKey(name))
        {
            clickAs.clip = audClicpsDic[name];
            Play(clickAs, false);


        }
        else
        {

        }
    }
    public void PlayShot(string name)
    {

    }

    public override void Clear()
    {
        base.Clear();
    }

    private void Play(AudioSource aud,bool isLoop=true)
    {

        
        if (aud.isPlaying)
        {
            aud.Stop();
            aud.loop = isLoop;
            aud.Play();
        }


    }



}
