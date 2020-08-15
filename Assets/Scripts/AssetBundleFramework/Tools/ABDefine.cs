/****************************************************
文件：ABDefine.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 11:06:00
功能：AB 框架的所有公有定义
    1、本框架项目所有的常量
    2、所有的委托
    3、枚举类型
    4、常量定义
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ABFw
{
    /* 委托定义 */
    public delegate void DelLoadComplete(string assetbundleName);


    public class ABDefine
    {
        /* 框架常量 */
        public const string ASSETBUNDLE_MANIFEST = "AssetBundleManifest";
    }
}
