/****************************************************
文件：MultiABManager.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 15:46:41
功能：一个场景中多个AssetBundle 管理
    1、获取AB包的依赖和引用关系
    2、管理AssetBundle包之间的自动连锁（递归）加载机制
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{
    public class MultiABManager 
    {
        // 引用类：“单个AB包加载的实现”
        private SingleABLoader _CurrentSingleABLoader;

        // AB 包实现类  缓存集合（作用：缓存AB包，放置重复加载，即“AB包缓存集合”）
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;

        // 当前场景（调试使用）名称
        private string _CurrentSceneName;

        // 当前 AssetBundle 名称
        private string _CurrentABName;

        // AB 包与对应依赖关系集合
        private Dictionary<string, ABRelation> _DicABRelation;

        // 委托 所有AB包加载完成
        private DelLoadComplete _LoadAllABPackageCompleteHandler;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CurrentSceneName">场景名称</param>
        /// <param name="CurrentABName">Asset Bundle 名称</param>
        /// <param name="LoadAllABPackageCompleteHandler">委托</param>
        public MultiABManager(string CurrentSceneName, string CurrentABName, DelLoadComplete LoadAllABPackageCompleteHandler)
        {
            _CurrentSceneName = CurrentSceneName;
            _CurrentABName = CurrentABName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelation = new Dictionary<string, ABRelation>();

            _LoadAllABPackageCompleteHandler = LoadAllABPackageCompleteHandler;
        }

        /// <summary>
        /// 完成指定AB包调用
        /// </summary>
        /// <param name="abName">AB包名称</param>
        private void CompleteLoadAB(string abName) {

            Debug.Log(GetType()+ "/CompleteLoadAB()/ 当前完成 AssetBundle 加载的包的包名为 ：" + abName);

            if (abName.Equals(_CurrentABName))
            {
                if (_LoadAllABPackageCompleteHandler != null)
                {
                    _LoadAllABPackageCompleteHandler(abName);
                }
            }
        }


        /// <summary>
        /// 加载 AB 包（包括依赖项）
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle(string abName) {

            // AB 包关系的建立
            if (_DicABRelation.ContainsKey(abName)==false)
            {
                ABRelation abRelationObj = new ABRelation(abName);
                _DicABRelation.Add(abName,abRelationObj);
            }

            ABRelation tmpABRelationObj = _DicABRelation[abName];

            // 得到指定AB所有的依赖关系（查询Manifest清单文件）
            string[] strDependencyArray = ABManifestLoader.GetInstance().RetrivalDependency(abName);
            foreach (string item_Dependency in strDependencyArray)
            {
                // 添加“依赖项”
                tmpABRelationObj.AddDependency(item_Dependency);
                // 添加引用(背以来) （方便递归调用）
                yield return LoadReference(item_Dependency,abName);
            }

            // 真正加载 AB 包
            if (_DicSingleABLoaderCache.ContainsKey(abName) == true)
            {
                yield return _DicSingleABLoaderCache[abName].LoadAssetBundle();
            }
            else {
                _CurrentSingleABLoader = new SingleABLoader(abName, CompleteLoadAB);
                _DicSingleABLoaderCache.Add(abName,_CurrentSingleABLoader);
                yield return _CurrentSingleABLoader.LoadAssetBundle();
            }

         
        }


        /// <summary>
        /// 加载应用 AB 包
        /// </summary>
        /// <param name="abName">AB 包名称</param>
        /// <param name="refABName">被引用AB包名称</param>
        /// <returns></returns>
        private IEnumerator LoadReference(string abName, string refABName) {

            // AB 包已经加载了
            if (_DicABRelation.ContainsKey(abName))
            {
                ABRelation tmpABRelationObj = _DicABRelation[abName];

                // 添加AB包引用关系（被依赖关系）
                tmpABRelationObj.AddReference(refABName);
            }
            else {

                ABRelation tmpABRelationObj = new ABRelation(abName);
                tmpABRelationObj.AddReference(refABName);
                _DicABRelation.Add(abName,tmpABRelationObj);


                // 开始加载依赖的包（一个递归调用）
                yield return LoadAssetBundle(abName);
            }


           
        }

        /// <summary>
        /// 加载AB包资源
        /// </summary>
        /// <param name="abName">Asset Bundle 资源包名</param>
        /// <param name="assetName">包中具体资源名称</param>
        /// <param name="isCache">是否缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache) {

            foreach (string item_abName in _DicSingleABLoaderCache.Keys)
            {
                if (abName == item_abName)
                {
                    return _DicSingleABLoaderCache[item_abName].LoadAsset(assetName, isCache);
                }

            }
            Debug.LogError(GetType() + "/LoadAsset()/找不到AssetBundle包，无法加载资源，请检查！abName = " + abName + ", assetName = " + assetName);

            return null;
        }

        /// <summary>
        /// 释放包中所有加载的资源  (建议场景中切换的时候调用，避免卡顿)
        /// </summary>
        public void DisposeAllAsset() {

            try
            {
                // 逐一释放所有加载过的AssetBundle包中的资源
                foreach (SingleABLoader item_SABLoader in _DicSingleABLoaderCache.Values)
                {
                    item_SABLoader.DisposeAll();
                }
            }
            finally {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;

                // 释放其他对象占用的资源
                _DicABRelation.Clear();
                _DicABRelation = null;
                _CurrentABName = null;
                _CurrentSceneName = null;
                _LoadAllABPackageCompleteHandler = null;

                // 卸载没有使用到的资源
                Resources.UnloadUnusedAssets();
                // 强制垃圾收集
                System.GC.Collect();
            }
            

            
        }

    }//class_End
}
