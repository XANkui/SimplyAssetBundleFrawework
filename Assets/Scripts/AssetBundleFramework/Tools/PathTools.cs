/****************************************************
文件：PathTools.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/14 17:11:52
功能：AB框架的 路径工具类
    1、路径常量
    2、路径方法
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{

    public class PathTools 
    {
        /* 路径常量 */
        public const string AB_RESOURCES = "AB_Resources";

        /* 路径方法 */

        /// <summary>
        /// 得到 AB 资源的输入目录
        /// </summary>
        /// <returns></returns>
        public static string GetABResourcesPath() {
            return Application.dataPath + "/" + AB_RESOURCES;
        }

        /// <summary>
        /// 获得 AB 包输出路径
        ///     1\ 平台(PC/移动端等)路径
        ///     2\ 平台名称
        /// </summary>
        /// <returns></returns>
        public static string GetABOutPath() {
            return GetPlatformPath() + "/" + GetPlatformName();
        }

        /// <summary>
        /// 获得平台路径
        /// </summary>
        /// <returns></returns>
        private static string GetPlatformPath() {

            string strReturenPlatformPath = string.Empty;

            switch (Application.platform)
            {
                
                case RuntimePlatform.WindowsPlayer:           
                case RuntimePlatform.WindowsEditor:
                    strReturenPlatformPath = Application.streamingAssetsPath;

                    break;
                case RuntimePlatform.IPhonePlayer:         
                case RuntimePlatform.Android:
                    strReturenPlatformPath = Application.persistentDataPath;
                    
                    break;
              
                default:
                    break;
            }

            return strReturenPlatformPath;
        }

        /// <summary>
        /// 获得平台名称
        /// </summary>
        /// <returns></returns>
        private static string GetPlatformName()
        {
            string strReturenPlatformName = string.Empty;

            switch (Application.platform)
            {

                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    strReturenPlatformName = "Windows";

                    break;
                case RuntimePlatform.IPhonePlayer:
                    strReturenPlatformName = "IPhone";

                    break;
                case RuntimePlatform.Android:
                    strReturenPlatformName = "Android";

                    break;

                default:
                    break;
            }

            return strReturenPlatformName;
        }


    }//Class_End
}
