/****************************************************
文件：BuildAssetBundle.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/14 17:26:18
功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ABFw
{
       
    /// <summary>
    /// AssetBundle 打包工具
    /// </summary>
    public class BuildAssetBundle
    {
        /// <summary>
        /// 打包生成所有的AssetBundles（包）
        /// </summary>
        [MenuItem("AssetBundleTools/BuildAllAssetBundles")]
        public static void BuildAllAB()
        {
            // 打包AB输出路径
            string strABOutPAthDir = string.Empty;

            // 获取“StreamingAssets”文件夹路径（不一定这个文件夹，可自定义）            
            strABOutPAthDir = PathTools.GetABOutPath();

            // 判断文件夹是否存在，不存在则新建
            if (Directory.Exists(strABOutPAthDir) == false)
            {
                Directory.CreateDirectory(strABOutPAthDir);
            }

            // 打包生成AB包 (目标平台根据需要设置即可)
            BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

        }
    }
}
