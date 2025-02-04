using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

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

        // 获取“StreamingAssets”文件夹路径
        strABOutPAthDir = Application.streamingAssetsPath;

        // 判断文件夹是否存在，不存在则新建
        if (Directory.Exists(strABOutPAthDir) == false)
        {
            Directory.CreateDirectory(strABOutPAthDir);
        }

        // 打包生成AB包
        BuildPipeline.BuildAssetBundles(strABOutPAthDir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);

    }
}