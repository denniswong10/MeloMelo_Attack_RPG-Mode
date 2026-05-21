using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeloMelo_SQL_Services_GameConfiguration
{
    #region CONFIG (API)
    public static readonly string MeloMelo_Config_VersionControl = "MeloMelo_API_VersionControl.php";
    public static readonly string MeloMelo_Config_ExchangePoint = "MeloMelo_API_ExchangePoint.php";
    public static readonly string MeloMelo_Config_PlayEvent = "MeloMelo_API_PlayEvent.php";
    public static readonly string MeloMelo_Config_MarathonExchange = "MeloMelo_API_MarathonExchange.php";
    public static readonly string MeloMelo_Config_MarathonContent = "MeloMelo_API_MarathonContent.php";
    #endregion
}

#region COMPONENT (ObtainableItems)
public class Cloud_Setup_ExchangePoint_Data
{
    public string Title;
    public string Obtain_Type;
    public string Code;
    public string upToDate_version;
}

public class Cloud_Config_ExchangePoint
{
    public Cloud_Setup_ExchangePoint_Data[] config;
}

public class Cloud_Setup_PlayEvent_Data
{
    public string ItemName;
    public int MaxObatin;
    public int PlayRequire;
    public string upToDate_version;
}

public class Cloud_Config_PlayEvent
{
    public Cloud_Setup_PlayEvent_Data[] config;
}

public class Cloud_Setup_MarathonExchange_Data
{
    public string ItemId;
    public int costInObtain;
    public string upToDate_version;
}

public class Cloud_Config_MarathonExchange
{
    public Cloud_Setup_MarathonExchange_Data[] content;
}
#endregion