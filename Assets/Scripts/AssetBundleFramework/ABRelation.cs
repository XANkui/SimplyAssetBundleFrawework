/****************************************************
文件：ABRelation.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 15:28:33
功能：Asset Bundle 关系
    1、依赖项关系
    2、引用项关系
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{
    public class ABRelation 
    {
        // 当前 AssetBundle 名称
        private string _ABName;

        // 本包所有的依赖包集合
        private List<string> _ListAllDependencyAB;

        // 本包所有的引用包集合
        private List<string> _ListAllReferenceAB;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ABName"></param>
        public ABRelation(string ABName)
        {
            if (string.IsNullOrEmpty(ABName) == false)
            {
                _ABName = ABName;
            }

            _ListAllDependencyAB = new List<string>();
            _ListAllReferenceAB = new List<string>();
        }

        /// <summary>
        /// 增加依赖关系
        /// </summary>
        /// <param name="abName"> Asset Bundle 的包名（其他的）</param>
        public void AddDependency(string  abName) {
            if (_ListAllDependencyAB.Contains(abName) == false)
            {
                _ListAllDependencyAB.Add(abName);
            }
        }

        /// <summary>
        /// 移除依赖
        /// </summary>
        /// <param name="abName">Asset Bundle 的包名（其他的）</param>
        /// <returns>
        /// true: 此 AssetBundle 没有依赖项
        /// false: 此 AssetBundle 还有其他依赖项
        /// </returns>
        public bool RemoveDependency(string abName) {
            if (_ListAllDependencyAB.Contains(abName) == true)
            {
                _ListAllDependencyAB.Remove(abName);
            }

            if (_ListAllDependencyAB.Count > 0)
            {
                return false;
            }
            else {

                return true;
            }


        }

        /// <summary>
        /// 获得所有该包的所有依赖项
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllDependency() {
            return _ListAllDependencyAB;
        }


        /// <summary>
        /// 增加引用（被依赖）关系
        /// </summary>
        /// <param name="abName"> Asset Bundle 的包名（其他的）</param>
        public void AddReference(string abName)
        {
            if (_ListAllReferenceAB.Contains(abName) == false)
            {
                _ListAllReferenceAB.Add(abName);
            }
        }

        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// <param name="abName">Asset Bundle 的包名（其他的）</param>
        /// <returns>
        /// true: 此 AssetBundle 没有引用项
        /// false: 此 AssetBundle 还有其他引用项
        /// </returns>
        public bool RemoveReference(string abName)
        {
            if (_ListAllReferenceAB.Contains(abName) == true)
            {
                _ListAllReferenceAB.Remove(abName);
            }

            if (_ListAllReferenceAB.Count > 0)
            {
                return false;
            }
            else
            {

                return true;
            }


        }

        /// <summary>
        /// 获得所有该包的所有引用项
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllReference()
        {
            return _ListAllReferenceAB;
        }
    }
}
