using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public static class EdPath
{

    public static string UnityAssetPath = Application.dataPath;


    public static string CSVLoadPath = UnityAssetPath + "/Resources/";


    public static string CSVCreateClassPath = UnityAssetPath + "/Scripts/CsvClass/";


    public static string StreamAssetPath = Application.streamingAssetsPath;

    public static string ABNameClassOutPath = UnityAssetPath + "/Scripts/ABNameClass/";


    public static string ABResPath = UnityAssetPath + "/MyAddResable/";

    public static string ABLocal = ABResPath + "Local/";


    public static string ABRemoted= ABResPath + "Remoted/";



    public static string GetLoaclOrRemotedPath(int id)
    {
        if (id==0)
        {
            return ABLocal;
        }
        else
        {
            return ABRemoted;
        }
    }

}
