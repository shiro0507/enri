using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectOpenListItems : MonoBehaviour {

    public Transform _parentTransform;

    List<GameObject> _items = new List<GameObject>();

    public void UpdateItems(int listIndex)
    {
        GameObject prefab = (GameObject)Resources.Load("Prefabs/MenuParts/selectOpenItem");

        if (listIndex == 0)
        {
            foreach (var flight in CreateFlightRoot.flightData.flights.Keys)
            {
                var item = Instantiate(prefab, _parentTransform);
                item.transform.localScale = new Vector3(1, 1, 1);
                item.GetComponent<SelectOpenItem>().Init(flight);
            }
        }

        if (listIndex == 1)
        {
            foreach (var waypoint in CreateFlightRoot.flightPlan.plans.Values)
            {
                var item = Instantiate(prefab, _parentTransform);
                item.transform.localScale = new Vector3(1, 1, 1);
                item.GetComponent<SelectOpenItem>().Init(waypoint.Destination.WaypointID);
            }
        }
    }
}
