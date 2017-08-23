using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FlightDataPoint
{
	public TimeSpan Time;		// (hh:mm:ss)
	public string FlightID;
	public float Latitude;		// (deg)
	public float Longitude;		// (deg)
	public float Altitude;		// (feet)
	public string AircraftType;
    public string CAS;            // (knot)
    public string TAS;            // (knot)
    public string GS;            // (knot)
    public string MACH;


    public Vector3 Position {
		get {
			return Constants.Sphere.ToCartesian(new Vector3(Latitude, Longitude, Altitude));
		}
	}

	public Vector3 FlatPosition {
		get {
			return Constants.Sphere.ToCartesian(new Vector3(Latitude, Longitude, 0));
		}
	}
}