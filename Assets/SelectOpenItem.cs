using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectOpenItem : MonoBehaviour {

    string _name;
    
    public Text Text;
    public Toggle Toggle;

    public void Init(string name)
    {
        _name = name;
        Text.text = _name;
        Flight flight;
        if (CreateFlightRoot.flightData.flights.TryGetValue(_name, out flight))
        {
            Toggle.isOn = flight.Show;
        }
        foreach (var waypoint in CreateFlightRoot.flightPlan.plans.Values)
        {
            if (waypoint.Destination.WaypointID == _name)
            {
                Toggle.isOn = waypoint.Show;
            }
        }
    }
    
    public void SetShow(bool show)
    {
        Flight flight;
        if (CreateFlightRoot.flightData.flights.TryGetValue(_name, out flight)) {
            flight.Show = show;
        }
        foreach (var waypoint in CreateFlightRoot.flightPlan.plans.Values)
        {
            if (waypoint.Destination.WaypointID == _name)
            {
                waypoint.Show = show;
            }
        }
    }
}
