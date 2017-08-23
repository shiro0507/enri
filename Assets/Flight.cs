using UnityEngine;
using System.Collections.Generic;
using System;

public class Flight {
	public List<FlightDataPoint> datapoints = new List<FlightDataPoint>();
    //cache for populating flightline renderer
	public List<Vector3> positions = new List<Vector3>();

	public TimeSpan start;
	public TimeSpan end;
    public string flightId;

	public Transform flightPathIndicator;
	public Transform planeIndicator;
	public LineRenderer lineRenderer;

    public bool Show = true;

    public bool IsShown()
    {
        FlightPlan block = null;
        CreateFlightRoot.flightPlan.plans.TryGetValue(flightId, out block);

        return Show && (block == null || block.Show);
    }
	int currentDataPoint = 0;

	int firstDisplayed = 0;
	int lastDisplayed = 0;

    public void Reset() {
        currentDataPoint = 0;
        firstDisplayed = 0;
        lastDisplayed = 0;
    }

	public void UpdateIndicators(TimeSpan time) {
		while(datapoints[currentDataPoint].Time < time && currentDataPoint < datapoints.Count) {
			currentDataPoint++;
		}
		planeIndicator.position = datapoints[currentDataPoint].Position;

		var timeFirst = time.Subtract(new TimeSpan(0,5,0));
		while(firstDisplayed < datapoints.Count && datapoints[firstDisplayed].Time < timeFirst) {
			firstDisplayed++;
		}

		var timeLast = time.Add(new TimeSpan(0,5,0));
		while(lastDisplayed < datapoints.Count && datapoints[lastDisplayed].Time < timeLast) {
			lastDisplayed++;
		}

        lineRenderer.numPositions = lastDisplayed - firstDisplayed;
        lineRenderer.SetPositions(positions.GetRange(firstDisplayed, lineRenderer.numPositions).ToArray());
	}
}