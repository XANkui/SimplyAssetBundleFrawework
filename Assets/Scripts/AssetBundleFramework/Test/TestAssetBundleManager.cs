/****************************************************
文件：TestAssetBundleManager.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 17:35:46
功能：Asset Bundle 整体框架测试脚本
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFw
{
    public class TestAssetBundleManager : MonoBehaviour
    {
        // 场景名称
        private string _SceneName = "scene_1";

        // AB 包名称
        private string _AssetBundleName = "scene_1/prefabs.ab";

        // 资源名称
        private string _AssetName = "CubeMaterial.prefab";


        private void Start()
        {
            Debug.Log("开始 Asset Bundle 框架测试");

            // 调用 AB 包（连锁只能调用依赖资源）
            StartCoroutine(AssetBundleManager.GetInstance().LoadAssetBundlePack(_SceneName, _AssetBundleName, LoadAllABComplete));
        }

        /// <summary>
        /// 所有AB 包加载完毕的事件回调
        /// </summary>
        /// <param name="abName"></param>
        private void LoadAllABComplete(string abName)
        {
            Debug.Log("所有 Asset Bundle 资源加载完毕");

            UnityEngine.Object tmpObj = null;

            // 提取资源
            tmpObj = (UnityEngine.Object)AssetBundleManager.GetInstance().LoadAsset(_SceneName, _AssetBundleName, _AssetName, false);

            if (tmpObj != null)
            {
                Instantiate(tmpObj);
            }
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // 释放当前场景的所有资源
                Debug.Log("释放场景 " +_SceneName +" 的所有资源");
                AssetBundleManager.GetInstance().DisposeAllAsset(_SceneName);
            }
        }
    }
}
