using System;
using System.IO;

/// <summary>
/// 修改新建脚本中的指定信息
/// </summary>
public class AddScriptInfo : UnityEditor.AssetModificationProcessor
{
    private static string fileDescribe =
        "/****************************************************\n" +
        "文件：#SCRIPTNAME#\n" +
        "作者：#CreateAuthor#\n" +
        "邮箱：https://blog.csdn.net/u014361280 \n" +
        "日期：#CreateTime#\n" +
        "功能：Nothing\n" +
        "*****************************************************/\n\n";

    /// <summary>
    /// 在创建资源的时候执行的函数
    /// </summary>
    /// <param name="path">路径</param>
    private static void OnWillCreateAsset(string path)
    {

        //将.meta后缀屏蔽，避免获取到的是新建脚本时候的 meta 文件
        path = path.Replace(".meta", "");

        //只对.cs脚本作操作
        if (path.EndsWith(".cs") == true)
        {

            // 文件名的分割获取
            string[] iterm = path.Split('/');

            string str = fileDescribe;

            //读取该路径下的.cs文件中的所有文本.
            //注意，此时Unity已经对脚本完成了模版内容的替换，包括#SCRIPTNAME#也已经被替换为文件名了，读取到的是替换后的文本内容
            str += File.ReadAllText(path);

            // 进行关键字的文件名、作者和时间获取，并替换
            str = str.Replace("#SCRIPTNAME#", iterm[iterm.Length - 1]).Replace(
                "#CreateAuthor#", Environment.UserName).Replace(
                "#CreateTime#", string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", DateTime.Now.Year,
                DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute,
                DateTime.Now.Second));

            // 重新写入脚本中，完成数据修改
            File.WriteAllText(path, str);
        }
    }
}