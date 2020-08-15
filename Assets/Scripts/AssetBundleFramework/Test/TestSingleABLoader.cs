/****************************************************
文件：TestSingleABLoader.cs
作者：Administrator
邮箱：https://blog.csdn.net/u014361280 
日期：2020/08/15 11:24:10
功能：测试 SingleABLoader
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ABFw
{
    public class TestSingleABLoader : MonoBehaviour
    {

        // 引用类
        private SingleABLoader _LoadObj = null;

        // AB 包名称
        private string _ABName1 = "scene_1/prefabs.ab";


        // AB 包中资源名称
        private string _AssetName1 = "Capsule.prefab";
        private string _AssetName2 = "CubeMaterial.prefab";

        // _AssetName2 依赖的AB 包名
        private string _AssetName2DependencyABName1 = "scene_1/matreials.ab";
        private string _AssetName2DependencyABName2 = "scene_1/testures.ab";

        // Start is called before the first frame update
        void Start()
        {
            // 建议优先加载依赖包 1
            SingleABLoader DependencyABName1LoadObj = new SingleABLoader(_AssetName2DependencyABName1, (abName1) => {
                Debug.Log("依赖包 1 资源加载完毕");
                
                // 然后优先加载依赖包 2
                SingleABLoader DependencyABName2LoadObj = new SingleABLoader(_AssetName2DependencyABName2, (abName2) => {
                    Debug.Log("依赖包 2 资源加载完毕");

                    // 正式加载资源
                    _LoadObj = new SingleABLoader(_ABName1, (abName) => {
                        Debug.Log("资源加载完毕");
                        // 加载AB资源包中的资源，并克隆
                        UnityEngine.Object tmpobj1 = _LoadObj.LoadAsset(_AssetName1, false);
                        Instantiate(tmpobj1);
                        UnityEngine.Object tmpobj2 = _LoadObj.LoadAsset(_AssetName2, false);
                        Instantiate(tmpobj2);

                        // 查询资源包所有资源名称
                        foreach (string str in _LoadObj.RetriveAllAssetName())
                        {
                            Debug.Log(str);
                        }

                    });
                    StartCoroutine(_LoadObj.LoadAssetBundle());

                });
                StartCoroutine(DependencyABName2LoadObj.LoadAssetBundle());
            });
            StartCoroutine(DependencyABName1LoadObj.LoadAssetBundle());

            
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                
                if (_LoadObj != null)
                {
                    Debug.Log("释放镜像资源");
                    _LoadObj.Dispose();
                }
            }

            if (Input.GetKeyDown(KeyCode.B))
            {
               
                if (_LoadObj != null)
                {
                    Debug.Log("释放镜像资源，和内存资源");
                    _LoadObj.DisposeAll();
                }
            }
        }


    }
}
