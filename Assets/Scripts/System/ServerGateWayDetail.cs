using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "ServerDetail", menuName = "Registered_Server")]
public class ServerGateWayDetail : ScriptableObject
{
    public string serverTitle;
    public string destinationTitle;
    public bool offlineMode;
}
