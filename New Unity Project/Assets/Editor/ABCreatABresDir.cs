using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.Data;

public class ABCreatABresDir : EditorWindow
{
    [MenuItem("Addresables/生成场景资源文件")]
    static void show()
    {
        EditorWindow.GetWindow<ABCreatABresDir>().Show();
    }
  static  string levelName = null;
    //Object source;
    void OnGUI()
    {
        

        levelName = EditorGUILayout.TextField("场景名称", levelName);

        if (GUILayout.Button("生成场景文件夹"))
        {
            Debug.Log(levelName);
            CreatLevelDir();
        }

    }

    static void CreatLevelDir()
    {
        if (levelName==null)
        {
            return;
        }

        if (Directory.Exists(EdPath.ABRemoted+ levelName)==false)
        {

            CreateOrNot("Atlas");
            CreateOrNot("Material");
            CreateOrNot("Prefabs");
            CreateOrNot("Scene");
            CreateOrNot("Shader");
            CreateOrNot("Sound");
            CreateOrNot("Sprite");
            CreateOrNot("Textrue");
            CreateOrNot("Ui");
        }
        if (Directory.Exists(EdPath.ABLocal + levelName) == false)
        {
            CreateOrNot("Atlas",1);
            CreateOrNot("Material",1);
            CreateOrNot("Prefabs",1);
            CreateOrNot("Scene",1);
            CreateOrNot("Shader",1);
            CreateOrNot("Sound",1);
            CreateOrNot("Sprite",1);
            CreateOrNot("Textrue",1);
            CreateOrNot("Ui",1);
        }






        AssetDatabase.Refresh();

    }

    static void CreateOrNot(string dirName,int id=0)
    {
        string path = string.Empty;
        if (id==0)
        {
           path = EdPath.ABRemoted + levelName + "/" + dirName;
        }
        else
        {
            path = EdPath.ABLocal + levelName + "/" + dirName;
        }

        if (Directory.Exists(path)==false)
        {
            Directory.CreateDirectory(path);
        }


    }
  
}
