/****************************************************
文件：AssetLoader.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 09:48:26
功能：AB (AssetBundle)资源加载
    功能：
        1、管理与加载指定AB的资源
        2、加载具有“缓存功能”的资源，带选用参数
        3、卸载、释放资源
        4、查看当前AB资源
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{
    public class AssetLoader : System.IDisposable
    {
        // 当前AssetBundle 
        private AssetBundle _CurrentAssetBundle;

        // 缓存集合
        private Hashtable _Ht;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abObj">给定www加载的AssetBundle实例</param>
        public AssetLoader(AssetBundle abObj) {
            if (abObj != null)
            {
                _CurrentAssetBundle = abObj;
                _Ht = new Hashtable();

            }
            else {
                Debug.Log(GetType()+ " /构造函数 AssetLoader() 参数为空，请检查");
            }
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache=false) {

            return LoadResource<UnityEngine.Object>(assetName,isCache);
        }

        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <param name="asset">资源</param>
        /// <returns></returns>
        public bool UnLoadAsset(UnityEngine.Object asset) {

            if (asset !=null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }

            Debug.LogError(GetType()+ "/UnLoadAsset()/参数 asset is null,请检查");

            return false;
        }

        /// <summary>
        /// 卸载内存镜像资源
        /// </summary>
        public void Dispose()
        {
            _CurrentAssetBundle.Unload(false);
        }

        /// <summary>
        /// 释放当前 AssetBundle 内存镜像资源，且释放内存资源
        /// </summary>
        public void DisposeAll() {
            _CurrentAssetBundle.Unload(true);
        }

        /// <summary>
        /// 查询AB包中所有资源名称
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName() {
            return _CurrentAssetBundle.GetAllAssetNames();
        }


        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        private T LoadResource<T>(string assetName, bool isCache) where T : UnityEngine.Object {

            // 是否缓存集合中已存在
            if (_Ht.Contains(assetName))
            {
                return _Ht[assetName] as T;
            }

            // 没有，则加载
            T tmpTResource = _CurrentAssetBundle.LoadAsset<T>(assetName);

            // 根据要求是否保存到缓存中
            if (tmpTResource != null && isCache == true)
            {
                _Ht.Add(assetName, tmpTResource);
            }
            else if (tmpTResource == null) {
                Debug.LogError(GetType() + "/LoadResource<T>()/ 加载资源为空，请检查 参数 assetName = "+ assetName);
            }

            return tmpTResource;
        }
    }
}
