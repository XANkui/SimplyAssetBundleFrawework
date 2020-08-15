/****************************************************
文件：AssetBundleManager.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 16:57:10
功能：所有场景的 Asset Bundle 管理
    1、提取 Manifest 清单文件，缓存本脚本
    2、以场景为单位，管理正给项目中所有的 AssetBundle 包
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{
    public class AssetBundleManager : MonoBehaviour
    {
        // 本类实例
        private static AssetBundleManager _Instance;

        // 场景集合
        private Dictionary<string, MultiABManager> _DicAllScenes = new Dictionary<string, MultiABManager>();

        // AssetBundle (清单文件)系统类
        private AssetBundleManifest _ManifestObj = null;

        private AssetBundleManager() { }

        /// <summary>
        /// 得到实例
        /// </summary>
        /// <returns></returns>
        public static AssetBundleManager GetInstance() {
            if (_Instance == null)
            {
                _Instance = new GameObject("_AssetBundleManager").AddComponent<AssetBundleManager>();
            }

            return _Instance;
        }

        private void Awake()
        {
            // 加载 Manifest 清单文件
            StartCoroutine(ABManifestLoader.GetInstance().LoadManifest());
        }

        /// <summary>
        /// 下载 指定 AssetBundle 包
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="abName"></param>
        /// <param name="loadAllComplete"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundlePack(string sceneName, string abName, DelLoadComplete loadAllComplete) {

            // 参数检查
            if (string.IsNullOrEmpty(sceneName) == true || string.IsNullOrEmpty(abName) == true)
            {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/sceneName or   is null，请检查");

                yield return null;
            }

            // 等待 Manifest 清单文件加载完成
            while (ABManifestLoader.GetInstance().IsLoadFinished == false) {
                yield return null;
            }

            _ManifestObj = ABManifestLoader.GetInstance().GetABManifest();
            if (_ManifestObj == null)
            {
                Debug.LogError(GetType()+ "/LoadAssetBundlePack()/_ManifestObj is null,请先确保加载 Manifest 清单文件");
                yield return null;
            }

            if (_DicAllScenes.ContainsKey(sceneName) == false)
            {
                MultiABManager multiABManagerObj = new MultiABManager(sceneName,abName,loadAllComplete);
                _DicAllScenes.Add(sceneName, multiABManagerObj);

            }

            // 多包管理类
            MultiABManager tmpMultiABMgrObj = _DicAllScenes[sceneName];
            if (tmpMultiABMgrObj ==null)
            {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/tmpMultiABMgrObj is null,请检查");
            }


            yield return tmpMultiABMgrObj.LoadAssetBundle(abName);

            
        }

        /// <summary>
        /// 加载包中指定资源
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AsssetBundle 名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string sceneName,string abName, string assetName,bool isCache) {

            if (_DicAllScenes.ContainsKey(sceneName) == true)
            {
                MultiABManager multiABManager = _DicAllScenes[sceneName];
                return multiABManager.LoadAsset(abName, assetName,isCache);
            }

            Debug.LogError(GetType()+ "/LoadAsset()/找不到场景名称，无法加载AssetBundle资源，请检查！sceneName = " + sceneName);


            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="sceneName"></param>
        public void DisposeAllAsset(string sceneName) {
            if (_DicAllScenes.ContainsKey(sceneName) == true)
            {
                MultiABManager multiABManager = _DicAllScenes[sceneName];
                multiABManager.DisposeAllAsset();
            }
            else {
                Debug.LogError(GetType() + "/DisposeAllAsset()/找不到场景名称，无法释放 AssetBundle 资源，请检查！sceneName = " + sceneName);

            }
        }

    }//class_End
}
