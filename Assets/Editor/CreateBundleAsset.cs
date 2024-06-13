using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateBundleAsset
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildBundleAssets()
    {
        string assetBundleDirectory = "Assets/StreamingAssets";

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(assetBundleDirectory);

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
    }
}
