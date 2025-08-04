using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateConfigPatcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GerenateUpdateConfig();
    }

    #region COMPONENT
    private void GerenateUpdateConfig()
    {
        string[] infoToConfig =
        {
            "APPLICATION_NAME=", // Name of the file zip
            "DOWNLOAD_URL=", // Download link to the zip
            "VERSION_URL=", // Version Control to a drive txt
            "FILE_TO_RUN=", // Name of the folder + executable name
            "DOWNLOAD_VERSION_FILE=true",
            "FORCE_UPDATE=false"
        };

        if (GetFileOnConfig(Application.isEditor))
        {
            int toggleInfo = 0;
            System.IO.StreamWriter config = new System.IO.StreamWriter(Application.isEditor ? "Assets/StreamingAssets/config.txt" : "../../../config.txt");
            foreach (string writeToConfig in infoToConfig)
            {
                config.WriteLine(writeToConfig + GetConfigurationInformation(toggleInfo));
                toggleInfo++;
            }

            Debug.Log("New patch config have been modify...");
            config.Close();
        }

        Debug.Log("Patch config not found...");
    }

    private string GetConfigurationInformation(int id)
    {
        switch (id)
        {
            case 0:
                return "MeloMelo v" + PlayerPrefs.GetString("GameLatest_Update", string.Empty);

            case 1:
                return PlayerPrefs.GetString("Application_Direct_Link", string.Empty);

            case 2:
                return PlayerPrefs.GetString("Application_VersionControl_Log", string.Empty);

            case 3:
                return "MeloMelo v" + PlayerPrefs.GetString("GameLatest_Update", string.Empty) + "/MeloMelo.exe";

            default:
                return string.Empty;
        }
    }

    private bool GetFileOnConfig(bool platformMode)
    {
        if ((!platformMode && System.IO.File.Exists("../../../config.txt")) ||
            (platformMode && System.IO.File.Exists("Assets/StreamingAssets/config.txt")))
            return true;
        else
            return false;
    }
    #endregion
}
