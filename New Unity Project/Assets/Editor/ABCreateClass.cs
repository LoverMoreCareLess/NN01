using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEditor;

public class ABCreateClass : Editor
{
    [MenuItem("AB/生成所有资源名字Class")]

    public static void CreateABClassWork()
    {

        CreateUIClass(0);
        CreateUIClass(1);


        WriteClass(uiStr, "UIID");
        WriteClass(prefabsStr, "PrefabID");
        WriteClass(atlasStr, "AtlasID");
        WriteClass(sound, "SoundID");

    }











    public static void CreateUIClass(int id)
    {
        uiStr.Clear();
        string strNeedSetLabelRoot = string.Empty;
        strNeedSetLabelRoot = EdPath.GetLoaclOrRemotedPath(id);
        DirectoryInfo[] dirScenesDIRArray = null;
        DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetLabelRoot);

        dirScenesDIRArray = dirTempInfo.GetDirectories();
        foreach (DirectoryInfo currentDIR in dirScenesDIRArray)
        {
            string tmpScenesDIR = strNeedSetLabelRoot + "/" + currentDIR.Name;          //全路径
                                                                                        //DirectoryInfo tmpScenesDIRInfo = new DirectoryInfo(tmpScenesDIR);
            int tmpIndex = tmpScenesDIR.LastIndexOf("/");
            string tmpScenesName = tmpScenesDIR.Substring(tmpIndex + 1);

            Debug.Log(tmpScenesName);
            GetClassDic(currentDIR, tmpScenesName);
        }

        //Debug.Log(prefabsStr.Count);
       

    }

    private static void WriteClass(List<string> lst, string ClassName)
    {


        StringBuilder fieldDefs = new StringBuilder();
        for (int i = 0; i < lst.Count; i++)
        {

            string s = string.Format("public const string {0} = ", lst[i]);

            string fieldName = string.Format("\"{0}\";", lst[i]);
            s += fieldName;
            fieldDefs.AppendLine(s + "\r\n");
        }



        StringBuilder classImpl = new StringBuilder(UItypeClass);
        classImpl.Replace("{Field1}", ClassName);
        classImpl.Replace("{Field2}", fieldDefs.ToString());
        string script = classImpl.ToString();

        string className = ClassName;
        string fname = EdPath.ABNameClassOutPath + $"{className}.cs";
        if (File.Exists(fname))
        {
            File.Delete(fname);
        }
        

        //else
        //{
        //    File.Create(fname);
        //}
        if (lst.Count <= 0)
        {
                       return;
        }

        File.WriteAllText(fname, script);

    }

    public static List<string> uiStr = new List<string>();
    public static List<string> prefabsStr = new List<string>();
    public static List<string> atlasStr = new List<string>();
    public static List<string> sound = new List<string>();

    private static void GetClassDic(FileSystemInfo fileSysInfo, string scenesName)
    {
        //参数检查
        if (!fileSysInfo.Exists)
        {
            Debug.LogError("文件或者目录名称： " + fileSysInfo + " 不存在，请检查");
            return;
        }

        //得到当前目录下一级的文件信息集合
        DirectoryInfo dirInfoObj = fileSysInfo as DirectoryInfo;                         //文件信息转换为目录信息
        FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();
        foreach (FileSystemInfo fileInfo in fileSysArray)
        {
            FileInfo fileinfoObj = fileInfo as FileInfo;
            //文件类型
            if (fileinfoObj != null)
            {
                //修改此文件的AssetBundle标签
                //SetFileABLabel(fileinfoObj, scenesName);
                if (fileinfoObj.Extension != ".meta")
                {
                    Debug.Log(fileInfo.Name);

                    string strABName = string.Empty;


                    //Win路径
                    string tmpWinPath = fileinfoObj.FullName;                                       //文件信息的全路径（Win格式）
                                                                                                    //Unity路径
                    string tmpUnityPath = tmpWinPath.Replace("\\", "/");                             //替换为Unity字符串分割符
                                                                                                     //定位“场景名称”后面字符位置
                    int tmpSceneNamePostion = tmpUnityPath.IndexOf(scenesName) + scenesName.Length;
                    //AB包中“类型名称”所在区域
                    string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePostion + 1);
                    //测试
                    //Debug.Log("@@@strABFileNameArea:  "+ strABFileNameArea);
                    if (strABFileNameArea.Contains("/"))
                    {
                        string[] tempStrArray = strABFileNameArea.Split('/');
                        //AB包名称正式形成
                        //Debug.Log("###tempStrArray[0]:  " + tempStrArray[0]);
                        strABName = scenesName + "/" + tempStrArray[0];

                        Debug.Log(tempStrArray[0]);
                        if (tempStrArray[0] == "Ui")
                        {
                            uiStr.Add(fileInfo.Name.Split('.')[0]);

                        }
                        else if (tempStrArray[0] == "Prefabs")
                        {
                            prefabsStr.Add(fileInfo.Name.Split('.')[0]);
                        }
                        else if (tempStrArray[0] == "Atlas")
                        {
                            atlasStr.Add(fileInfo.Name.Split('.')[0]);
                        }
                        else if (tempStrArray[0] == "Sound")
                        {
                            sound.Add(fileInfo.Name.Split('.')[0]);
                        }






                    }

                }


            }
            //目录类型
            else
            {
                //如果是目录则递归调用
                //JudgeDIRorFileByRecursive(fileInfo, scenesName);
                GetClassDic(fileInfo, scenesName);
            }
        }
    }

    const string UItypeClass = @"
using System;
using System.Collections.Generic;
using UnityEngine;


public  class {Field1}
{
{Field2}



	
}";


}
