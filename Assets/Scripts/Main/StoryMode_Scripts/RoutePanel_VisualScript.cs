using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoutePanel_VisualScript : MonoBehaviour
{
    [SerializeField] private GameObject RouteTemplate;
    [SerializeField] private Text RouteTitle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region MAIN
    public void LoadRoute(int routeIndex)
    {
        GameObject route_instance = Instantiate(RouteTemplate);
        route_instance.transform.GetChild(routeIndex).gameObject.SetActive(true);
        route_instance.transform.SetParent(transform);
    }

    public void UpdateRouteDetail(int current, int max)
    {
        RouteTitle.text = "Area Stage: " + current + "/" + max;
    }
    #endregion
}
