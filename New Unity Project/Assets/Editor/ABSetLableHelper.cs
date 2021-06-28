using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using System.IO;
using UnityEditor.AddressableAssets;
using System.Data;

public class ABSetLableHelper : EditorWindow
{
    private static AddressableAssetSettings setting;
    public static Dictionary<string, List<string>> addressDic = new Dictionary<string, List<string>>();

    //private static BuildEnvironment environment;

    static string url = "111";








    [MenuItem("Addresables/资源导入")]
    public static void PushIn()
    {
        setting = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>("Assets/AddressableAssetsData/AddressableAssetSettings.asset");
        DoMark(0);
        DoMark(1);
        //setActiveProfileId();
    }




   public static void DoMark(int id=0)
    {
        //string loaclRoot = EdPath.ABLocal;
        //string remotedRoot = EdPath.ABRemoted;


        string strNeedSetLabelRoot = string.Empty;

        DirectoryInfo[] dirScenesDIRArray = null;


        
        //AssetDatabase.RemoveUnusedAssetBundleNames();
       
        strNeedSetLabelRoot = EdPath.GetLoaclOrRemotedPath(id);
       
        DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetLabelRoot);
        dirScenesDIRArray = dirTempInfo.GetDirectories();
        
        foreach (DirectoryInfo currentDIR in dirScenesDIRArray)
        {
           
            string tmpScenesDIR = strNeedSetLabelRoot + "/" + currentDIR.Name;          
                                                                                        
            int tmpIndex = tmpScenesDIR.LastIndexOf("/");
            string tmpScenesName = tmpScenesDIR.Substring(tmpIndex + 1);                
                                                                                        
            JudgeDIRorFileByRecursive(currentDIR, tmpScenesName,id);
        }


        //刷新
        AssetDatabase.Refresh();
        //提示信息，标记包名完成。
        Debug.Log("AssetBundle 本次操作设置标记完成！");



    }

    private static void JudgeDIRorFileByRecursive(FileSystemInfo fileSysInfo, string scenesName,int id=0)
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
                MarkThis(fileinfoObj, scenesName,id);
            }
            //目录类型
            else
            {
                //如果是目录则递归调用
                JudgeDIRorFileByRecursive(fileInfo, scenesName,id);
            }
        }
    }

    private static void MarkThis(FileInfo fileinfoObj, string scenesName,int id)
    {


        string strABName = string.Empty;
        //文件路径（相对路径）
        string strAssetFilePath = string.Empty;


        //参数检查（*.meta 文件不做处理）
        if (fileinfoObj.Extension == ".meta") return;
        //得到AB包名称
        strABName = GetABName(fileinfoObj, scenesName);
        //获取资源文件的相对路径
        int tmpIndex = fileinfoObj.FullName.IndexOf("Assets");
        strAssetFilePath = fileinfoObj.FullName.Substring(tmpIndex);                    //得到文件相对路径
                                                                                        //给资源文件设置AB名称以及后缀
        //AssetImporter tmpImporterObj = AssetImporter.GetAtPath(strAssetFilePath);
        //tmpImporterObj.assetBundleName = strABName + "/" + fileinfoObj.Name.Split('.')[0];//这里的字符串需要替换
        string  path= strABName + "/" + fileinfoObj.Name.Split('.')[0];
        strABName = GetABName(fileinfoObj, scenesName);
        strABName = strABName + "/" + fileinfoObj.Name.Split('.')[0];

        string grop = string.Empty;
        if (id==0)
        {
            grop = "Local";
        }
        else
        {
            grop = "Remoted";
        }
        AutoGroup(grop, strAssetFilePath, strABName);
      

    }
    private static string GetABName(FileInfo fileinfoObj, string scenesName)
    {
        //Debug.Log(fileinfoObj.FullName);//调试
        //返回AB包名称
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
        }
        else
        {
            //定义*.Unity 文件形成的特殊AB包名称
            strABName = scenesName + "/" + scenesName;
        }
        //strABName += "/" + fileinfoObj.FullName;
        return strABName;
    }
    public static string AutoGroup(string groupName, string assetPath,string ABName)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;

        AddressableAssetGroup group = settings.FindGroup(groupName);
        if (group == null)
        {
            group = CreateAssetGroup<SchemaType>(settings, groupName);
            
        }
        var guid = AssetDatabase.AssetPathToGUID(assetPath);
       
        AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);
        // Debug.LogError(assetPath);
        // entry.address = Path.GetFileNameWithoutExtension(assetPath);
        entry.address = ABName;
        return entry.address;
    }
    private static AddressableAssetGroup CreateAssetGroup<SchemaType>(AddressableAssetSettings settings, string groupName)
    {
        
        return settings.CreateGroup(groupName, false, false, false,
            new List<AddressableAssetGroupSchema> { settings.DefaultGroup.Schemas[0], settings.DefaultGroup.Schemas[1] },
            typeof(SchemaType));
    }



    private void OnEnable()
    {
       
        //environment = (BuildEnvironment)System.Enum.Parse(typeof(BuildEnvironment), PlayerPrefs.GetString("BuildEnvironment", BuildEnvironment.Debug.ToString()));

    }

    private static void setActiveProfileId()
    {
        //var names = setting.profileSettings.GetAllProfileNames();
        //if (!names.Contains(environment.ToString()))
        //{
        //    setting.profileSettings.AddProfile(environment.ToString(), setting.activeProfileId);
        //}
        //var id = setting.profileSettings.GetProfileId(environment.ToString());
        //if (setting.activeProfileId != id)
        //    setting.activeProfileId = id;
        //if (environment == BuildEnvironment.Local)
        //{
        //    setting.profileSettings.SetValue(setting.activeProfileId, "RemoteBuildPath", "[UnityEngine.AddressableAssets.Addressables.BuildPath]/[BuildTarget]");
        //    setting.profileSettings.SetValue(setting.activeProfileId, "RemoteLoadPath", "{UnityEngine.AddressableAssets.Addressables.RuntimePath}/[BuildTarget]");
        //    setting.BuildRemoteCatalog = false;
        //}
        //else
        //{
        //    setting.BuildRemoteCatalog = true;
        //    //string[] ver = version.Split('.');
        //    string name = setting.profileSettings.GetProfileName(setting.activeProfileId);
        //    string buildPath = "ServerData" + "/[BuildTarget]/" + "/" + name;
        //    if (setting.profileSettings.GetValueByName(setting.activeProfileId, "RemoteBuildPath") != buildPath)
        //        setting.profileSettings.SetValue(setting.activeProfileId, "RemoteBuildPath", buildPath);
        //    string loadPath = url + "/poetry/" + buildPath;
        //    if (setting.profileSettings.GetValueByName(setting.activeProfileId, "RemoteLoadPath") != loadPath)
        //        setting.profileSettings.SetValue(setting.activeProfileId, "RemoteLoadPath", loadPath);
        //}
    }


}
