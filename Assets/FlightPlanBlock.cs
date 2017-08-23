using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPlan
{
    public string FlightID;
    public List<FlightPlanPoint> Waypoints = new List<FlightPlanPoint>();
    public FlightPlanPoint Destination;

    public bool Show = true;
    
}