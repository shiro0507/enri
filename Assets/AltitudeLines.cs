using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AltitudeLines : MaskableGraphic
{
    public float LineThikness = 2;
    public bool UseMargins;
    public Vector2 Margin;
    public Vector2[] Points;
    public Color32 color32;
    List<Flight> flights = new List<Flight>();

    TimeSpan currentTime = new TimeSpan();

    public void SetTime(TimeSpan time)
    {
        currentTime = time;
    }

    public void AddFlight(Flight flight)
    {
        flights.Add(flight);
    }

    public void RemoveFlight(Flight flight)
    {
        flights.Remove(flight);
    }

    TimeSpan ts = new TimeSpan(0);
    public int treshold = 10;

    public void Refresh()
    {
        ts = new TimeSpan(0);
        int counter = 0;
        foreach (Flight f in flights)
        {
            if (!f.IsShown())
            {
                continue;
            }
            var fts = f.end - f.start;
            if (fts > ts)
            {
                ts = fts;
            }
            counter++;
            if (counter > treshold)
            {
                break;
            }
        }
        SetAllDirty();
    }

    public float scale = 40000;

    Vector2 CalculatePosition(TimeSpan time, TimeSpan startTime, float altitude, Vector2 size)
    {
        return new Vector2((float)((ts.TotalMilliseconds - (time - startTime).TotalMilliseconds) / ts.TotalMilliseconds * size.x), altitude / scale * size.y);

    }

    public void Clear()
    {
        foreach (Flight f in flights)
        {
            for (int i = 0; i < f.datapoints.Count; i++)
            {
                var curr = f.datapoints[i];
                if (curr.Waypoint != null)
                {
                    if (curr.Waypoint.indicator != null)
                    {
                        Destroy(curr.Waypoint.indicator.gameObject);
                    }
                    if (curr.Waypoint.firstAltitudeIndicator != null)
                    {
                        Destroy(curr.Waypoint.firstAltitudeIndicator.gameObject);
                    }
                    if (curr.Waypoint.secondAltitudeIndicator != null)
                    {
                        Destroy(curr.Waypoint.secondAltitudeIndicator.gameObject);
                    }
                }
            }
        }
        flights.Clear();
    }

    void Update()
    {
        vh = new VertexHelper();
        var size = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        int j = 0;
        int counter = 0;
        vh.Clear();
        foreach (Flight f in flights)
        {
            if (!f.IsShown())
            {
                continue;
            }

            for (int i = 0; i < f.datapoints.Count; i++)
            {
                var curr = f.datapoints[i];
                Vector2 curr_c = CalculatePosition(curr.Time, f.start, curr.Altitude, size);

                if (curr.Waypoint != null)
                {
                    if (curr.Waypoint.indicator == null)
                    {
                        curr.Waypoint.indicator = Instantiate(waypointIndicatorPrefab, transform);
                        curr.Waypoint.indicator.localScale = Vector3.one;
                    }
                    curr.Waypoint.indicator.localPosition = CalculatePosition(curr.Time, f.start, 0, size);
                    curr.Waypoint.indicator.GetComponentInChildren<Text>().text = curr.Waypoint.WaypointID;

                    if (curr.Waypoint.firstAltitude != 0)
                    {
                        if (curr.Waypoint.firstAltitudeIndicator == null)
                        {
                            curr.Waypoint.firstAltitudeIndicator = Instantiate(firstAltitudeIndicatorPrefab, transform);
                            curr.Waypoint.firstAltitudeIndicator.localScale = Vector3.one;
                        }
                        curr.Waypoint.firstAltitudeIndicator.localPosition = CalculatePosition(curr.Time, f.start, curr.Waypoint.firstAltitude, size);
                    }

                    if (curr.Waypoint.secondAltitude != 0)
                    {
                        if (curr.Waypoint.secondAltitudeIndicator == null)
                        {
                            curr.Waypoint.secondAltitudeIndicator = Instantiate(secondAltitudeIndicatorPrefab, transform);
                            curr.Waypoint.secondAltitudeIndicator.localScale = Vector3.one;
                        }
                        curr.Waypoint.secondAltitudeIndicator.localPosition = CalculatePosition(curr.Time, f.start, curr.Waypoint.secondAltitude * 100, size);
                    }
                }

                if (i < f.datapoints.Count - 1)
                {
                    var next = f.datapoints[i + 1];
                    Vector2 next_c = CalculatePosition(next.Time, f.start, next.Altitude, size);

                    if (curr.Time <= currentTime && currentTime < next.Time)
                    {
                        timeIndicator.localPosition = CalculatePosition(curr.Time, f.start, curr.Altitude, size);
                    }

                    vh.AddVert((Vector3)(curr_c) + Vector3.up * LineThikness * 0.5f, color32, new Vector2(0f, 0f));
                    vh.AddVert((Vector3)(curr_c) - Vector3.up * LineThikness * 0.5f, color32, new Vector2(0f, 1f));
                    vh.AddVert((Vector3)(next_c) + Vector3.up * LineThikness * 0.5f, color32, new Vector2(1f, 1f));
                    vh.AddVert((Vector3)(next_c) - Vector3.up * LineThikness * 0.5f, color32, new Vector2(1f, 0f));

                    vh.AddTriangle(j * 4 + 0, j * 4 + 1, j * 4 + 2);
                    vh.AddTriangle(j * 4 + 2, j * 4 + 3, j * 4 + 0);

                    j++;
                }

            }
            counter++;
            if (counter > treshold)
            {
                break;
            }
        }
        
    }

    public RectTransform timeIndicator;
    public RectTransform waypointIndicatorPrefab;
    public RectTransform firstAltitudeIndicatorPrefab;
    public RectTransform secondAltitudeIndicatorPrefab;

    VertexHelper vh;

    protected override void OnPopulateMesh(Mesh mesh)
    {
        if (vh != null)
        {
            vh.FillMesh(mesh);
            vh.Dispose();
            vh = null;
        }
    }
}