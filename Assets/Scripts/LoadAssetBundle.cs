using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetBundle
{
    private string directory;
    private string path;

    public LoadAssetBundle(string path, string directory)
    {
        this.path = path;
        this.directory = directory;
    }

    public object LoadAssetFunction(string getData)
    {
        AssetBundle myAsset = AssetBundle.LoadFromFile(path + directory);
        object loadedAsset = null;

        if (myAsset)
        {
            Debug.Log("Found: " + getData);
            loadedAsset = myAsset.LoadAsset(getData);
            myAsset.Unload(false);
            return loadedAsset;
        }

        Debug.Log("Not Found: " + getData);
        return null;
    }
}
