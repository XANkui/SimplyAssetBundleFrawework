/****************************************************
文件：DeleteAssetBundle.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/14 19:10:45
功能：Nothing
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ABFw
{


    public class DeleteAssetBundle 
    {
        [MenuItem("AssetBundleTools/DeleteAllAssetBundles")]
        public static void DelAssetBundle() {

            // 删除AB包输出目录
            string strNeedDeleteDir = string.Empty;

            strNeedDeleteDir = PathTools.GetABOutPath();
            if (string.IsNullOrEmpty(strNeedDeleteDir) == false)
            {
                // 注意 ：这里参数 true 表示可以删除非空目录
                Directory.Delete(strNeedDeleteDir,true);

                // 同时删除 meta 文件（去除不必要的警告）
                File.Delete(strNeedDeleteDir + ".meta");

                // 刷新目录
                AssetDatabase.Refresh();
            }

        }
    }//class_End
}

