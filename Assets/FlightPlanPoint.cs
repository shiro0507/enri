using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightPlanPoint
{
    public string WaypointID;
    public float Latitude;
    public float Longitude;

    public Vector3 FlatPosition
    {
        get
        {
            return Constants.Sphere.ToCartesian(new Vector3(Latitude, Longitude, 0));
        }
    }
}
