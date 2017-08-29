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

            for (int i = 0; i < f.datapoints.Count - 1; i++)
            {
                var curr = f.datapoints[i];
                var next = f.datapoints[i + 1];

                Vector2 curr_c = CalculatePosition(curr.Time, f.start, curr.Altitude, size);
                Vector2 next_c = CalculatePosition(next.Time, f.start, next.Altitude, size);

                if (curr.Time <= currentTime && currentTime < next.Time)
                {
                    timeIndicator.localPosition = CalculatePosition(curr.Time, f.start, curr.Altitude, size);
                }

                if (curr.Waypoint != null)
                {
                    if (curr.Waypoint.indicator == null)
                    {
                        curr.Waypoint.indicator = Instantiate(waypointIndicatorPrefab, transform);
                    }
                    curr.Waypoint.indicator.localPosition = CalculatePosition(curr.Time, f.start, 0, size);
                }

                vh.AddVert((Vector3)(curr_c) + Vector3.up * LineThikness * 0.5f, color32, new Vector2(0f, 0f));
                vh.AddVert((Vector3)(curr_c) - Vector3.up * LineThikness * 0.5f, color32, new Vector2(0f, 1f));
                vh.AddVert((Vector3)(next_c) + Vector3.up * LineThikness * 0.5f, color32, new Vector2(1f, 1f));
                vh.AddVert((Vector3)(next_c) - Vector3.up * LineThikness * 0.5f, color32, new Vector2(1f, 0f));

                vh.AddTriangle(j * 4 + 0, j * 4 + 1, j * 4 + 2);
                vh.AddTriangle(j * 4 + 2, j * 4 + 3, j * 4 + 0);

                j++;

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

    VertexHelper vh;

    protected override void OnPopulateMesh(Mesh mesh)
    {
        if (vh != null)
        {
            vh.FillMesh(mesh);
        }
        vh.Dispose();
        vh = null;
    }
}