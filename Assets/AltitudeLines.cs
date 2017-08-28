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

    public void AddFlight(Flight flight) {
        flights.Add(flight);
    }

    public void RemoveFlight(Flight flight) {
        flights.Remove(flight);
    }

    TimeSpan ts = new TimeSpan(0);
    public int treshold = 10;

    public void Refresh() {
        ts = new TimeSpan(0);
        int counter = 0;
        foreach(Flight f in flights) {
            if (!f.IsShown())
            {
                continue;
            }
            var fts = f.end - f.start;
            if (fts > ts) {
                ts = fts;
            }
            counter++;
            if (counter > treshold) {
                break;
            }
        }
        SetAllDirty();
    }

    public float scale = 40000;

    protected override void OnPopulateMesh( Mesh mesh )
    {
        var sizeX = rectTransform.rect.width;
        var sizeY = rectTransform.rect.height;
        using (var vh = new VertexHelper())
        {
            int j = 0;
//            int v = 0;
            int counter = 0;
            vh.Clear();
        foreach(Flight f in flights) {
                if (!f.IsShown())
                {
                    continue;
                }

                for (int i = 0; i < f.datapoints.Count-1; i++){
                    var curr = f.datapoints[i];
                    var next = f.datapoints[i+1];

                    Vector2 curr_c = new Vector2((float)((ts.TotalMilliseconds - (curr.Time-f.start).TotalMilliseconds)/ts.TotalMilliseconds*sizeX), curr.Altitude/scale*sizeY);
                    Vector2 next_c = new Vector2((float)((ts.TotalMilliseconds - (next.Time-f.start).TotalMilliseconds)/ts.TotalMilliseconds*sizeX), next.Altitude/scale*sizeY);



                    vh.AddVert( (Vector3)(curr_c)+Vector3.up*LineThikness*0.5f, color32, new Vector2(0f, 0f));
                    vh.AddVert( (Vector3)(curr_c)-Vector3.up*LineThikness*0.5f, color32, new Vector2(0f, 1f));
                    vh.AddVert( (Vector3)(next_c)+Vector3.up*LineThikness*0.5f, color32, new Vector2(1f, 1f));
                    vh.AddVert( (Vector3)(next_c)-Vector3.up*LineThikness*0.5f, color32, new Vector2(1f, 0f));

                    vh.AddTriangle(j*4+0,j*4+1,j*4+2);
                    vh.AddTriangle(j*4+2,j*4+3,j*4+0);

                    j++;
//                    v+=4;
//                    if (v+4 >= 65000) {
//                        vh.FillMesh(mesh);
//                        return;
//                    }

                }
                counter++;
                if (counter > treshold) {
                    vh.FillMesh(mesh);
                    return;
                }
            }
            vh.FillMesh(mesh);

        }

    }
}