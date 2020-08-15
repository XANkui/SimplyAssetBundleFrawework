/****************************************************
文件：ABManifestLoader.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 14:16:47
功能：加载平台的Manifest清单文件
    1、读取 AssetBundle 依赖关系文件（例如：Windows.Manifest）
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{
    public class ABManifestLoader : System.IDisposable
    {
        // 本类实例
        private static ABManifestLoader _Instance;

        // AssetBundle （清单文件）系统类
        private AssetBundleManifest _ManifestObj;

        // AssetBundle 清单文件路径
        private string _StrManifestPath;

        // 读物AB清单文件的 AssetBundle
        private AssetBundle _ABReadManifest;

        // 是否加载Manifest 完成
        private bool _IsLoadFinished;  
        // 只读属性 是否加载Manifest
        public bool IsLoadFinished {
            get {
                return _IsLoadFinished;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        private ABManifestLoader()
        {
            // 确定清单文件 WWW 下载路径
            _StrManifestPath = PathTools.GetWWWAssetBundlePath() + "/" + PathTools.GetPlatformName(); ;
            _ManifestObj = null;
            _ABReadManifest = null;
            _IsLoadFinished = false;
        }

        /// <summary>
        /// 获得本类实例
        /// </summary>
        /// <returns></returns>
        public static ABManifestLoader GetInstance() {
            if (_Instance ==null)
            {
                _Instance = new ABManifestLoader();
            }

            return _Instance;
        }

        /// <summary>
        /// 下载 Manifest 清单文件
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadManifest() {
            using (WWW www = new WWW(_StrManifestPath)) {
                yield return www;
                if (www.progress >=1)
                {
                    // 加载完成，获取 AssetBundle 实例
                    AssetBundle abObj = www.assetBundle;
                    if (abObj != null)
                    {
                        _ABReadManifest = abObj;
                        //  读取清单文件资源（读取到系统类的实例中）
                        _ManifestObj = _ABReadManifest.LoadAsset(ABDefine.ASSETBUNDLE_MANIFEST) as AssetBundleManifest; // 字符串 AssetBundleManifest 是固定常量
                        _IsLoadFinished = true;
                    }
                    else {
                        Debug.LogError(GetType()+ "/ LoadManifest() / WWW 下载出错，请检查下载路径 _StrManifestPath = " + _StrManifestPath +"; 错误信息："+www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 获取 AssetBundleManifest 系统实例
        /// </summary>
        /// <returns></returns>
        public AssetBundleManifest GetABManifest() {
            if (IsLoadFinished == true)
            {
                if (_ManifestObj != null)
                {
                    return _ManifestObj;
                }
                else
                {
                    Debug.LogError(GetType() + "/GetABManifest()/_ManifestObj is null,请检查");
                }
            }
            else {
                Debug.LogError(GetType() + "/GetABManifest()/IsLoadFinished is false，Manifest 没有加载完毕，请检查");

            }

            return null;
        }

        /// <summary>
        /// 获取 AsssetBundleManifest(系统类)指定包名称的所有依赖项
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public string[] RetrivalDependency(string abName) {

            if (_ManifestObj != null && string.IsNullOrEmpty(abName) ==false)
            {
                return _ManifestObj.GetAllDependencies(abName);
            }
            else
            {
                Debug.LogError(GetType() + "/GetABManifest()/_ManifestObj is null, 或者 ABName 有误,请检查");
            }

            return null;
        }

        /// <summary>
        /// 释放本类资源
        /// </summary>
        public void Dispose()
        {
            if (_ABReadManifest != null)
            {
                _ABReadManifest.Unload(true);
            }
        }
    }
}
