using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class AddResInfo
{
    public string lvName;

    public Dictionary<string, AsyncOperationHandle> caches = new Dictionary<string, AsyncOperationHandle>();

    public Dictionary<string, AsyncOperationHandle> prefabsCaches = new Dictionary<string, AsyncOperationHandle>();


    public void LoadAsset<T>(string address, System.Action<T> onComplete, System.Action onFailed = null, bool autoUnload = false) where T : UnityEngine.Object
    {
        if (caches.ContainsKey(address))
        {
            var handle = this.caches[address];
            if (handle.IsDone)
            {
                if (onComplete != null)
                {
                    onComplete(caches[address].Result as T);
                }
            }
            else
            {
                handle.Completed += (result) =>
                {
                    if (result.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                    {
                        var obj = result.Result as T;
                        if (onComplete != null)
                        {
                            onComplete(obj);
                        }
                        if (autoUnload)
                            UnLoadAsset(address);
                    }
                    else
                    {
                        if (onFailed != null)
                        {
                            onFailed();
                        }
                        Debug.LogError("Load " + address + " failed!");
                    }
                };
            }

        }
        else
        {
            
            var handle = Addressables.LoadAssetAsync<T>(address);
            handle.Completed += (result) =>
            {
                if (result.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    var obj = result.Result as T;
                    if (onComplete != null)
                    {
                        onComplete(obj);
                    }
                    if (autoUnload)
                        UnLoadAsset(address);
                }
                else
                {
                    if (onFailed != null)
                    {
                        onFailed();
                    }
                    Debug.LogError("Load " + address + " failed!");
                }
            };
            addCaches(address, handle);
        }
    }
    public void GetPrefab(string address, System.Action<GameObject> onComplete, System.Action onFailed = null, bool autoUnload = false)
    {
       
        
            
            var handle = Addressables.InstantiateAsync(address);
            handle.Completed += (result) =>
            {
                if (result.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
                {
                    var obj = result.Result as GameObject;
                    if (onComplete != null)
                    {
                        onComplete(obj);
                    }
                    if (autoUnload)
                        UnLoadAsset(address);
                }
                else
                {
                    if (onFailed != null)
                    {
                        onFailed();
                    }
                    Debug.LogError("Load " + address + " failed!");
                }
            };
           

      

    }

    



    private void addPreabsCaches(string address, AsyncOperationHandle<GameObject> handle)
    {
        prefabsCaches.Add(address, handle);
    }

    private void addCaches<T>(string address, AsyncOperationHandle<T> handle) where T : UnityEngine.Object
    {
        caches.Add(address, handle);
    }

    private void UnLoadAsset(string address)
    {
        Addressables.Release(caches[address]);
    }





}










public class AResMgr : DoSingle<AResMgr>
{

    Dictionary<string, AddResInfo> MyAddResDic = new Dictionary<string, AddResInfo>();
    
   
    public void CommonGetPrefba(string prefabsName, Action<GameObject> evt, Action fail = null, bool isAutoUnload = false)
    {

        string addres = "Common/Prefabs/" + prefabsName;
        if (MyAddResDic.ContainsKey("Common") == false)
        {
            AddResInfo add = new AddResInfo();
            add.lvName = "Common";
            MyAddResDic.Add(add.lvName, add);

            add.GetPrefab(addres, evt, fail, isAutoUnload);
        }
        else
        {
            AddResInfo add = MyAddResDic["Common"];

            add.GetPrefab(addres, evt, fail, isAutoUnload);
        }

    }
    public void GetUI(string prefabsName, Action<GameObject> evt, Action fail = null, bool isAutoUnload = false)
    {
        string addres = "Common/Ui/" + prefabsName;
        if (MyAddResDic.ContainsKey("Common") == false)
        {
            AddResInfo add = new AddResInfo();
            add.lvName = "Common";
            MyAddResDic.Add(add.lvName, add);

            add.GetPrefab(addres, evt, fail, isAutoUnload);
        }
        else
        {
            AddResInfo add = MyAddResDic["Common"];

            add.GetPrefab(addres, evt, fail, isAutoUnload);
        }
    }
   public void GetAtlas(string prefabsName, Action<SpriteAtlas> evt, Action fail = null, bool isAutoUnload = false)
    {
        string addres = "Common/Atlas/" + prefabsName;
        if (MyAddResDic.ContainsKey("Common") == false)
        {
            AddResInfo add = new AddResInfo();
            add.lvName = "Common";
            MyAddResDic.Add(add.lvName, add);

            add.LoadAsset<SpriteAtlas>(addres, evt, fail, isAutoUnload);
        }
        else
        {
            AddResInfo add = MyAddResDic["Common"];

            add.LoadAsset<SpriteAtlas>(addres, evt, fail, isAutoUnload);
        }
    }

}
