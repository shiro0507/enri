﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlotArea : MonoBehaviour
{

    public TextAsset asset;

    public GameObject prefab;
    public LineRenderer prefabT;

    bool[] visited;

    // Use this for initialization
    void Start () {
        Load(asset);

        for(int i = 0; i < list.Count; i++) {
            var bin = list[i];
            GameObject segment = Instantiate(prefab);
            segment.transform.parent = transform;
            segment.name = "" + i;
            segment.transform.localPosition = bin.FlatPosition;
        }

        using (var vh = new VertexHelper())
        {
            for(int i = 0; i < list.Count; i++) {
                vh.AddVert(list[i].FlatPosition, Color.white, Vector2.zero);
            }

            visited = new bool[list.Count];
            int unvisited = visited.Length;
            int vertCount = list.Count;
            int prev = 0;
            int curr = -1;
            int next = 0;
            while (unvisited >= 3)
            {
                do {
                    curr = (curr+1)%vertCount;
                } while (visited[curr]);
                prev = curr;
                do {
                    prev = (prev+vertCount-1)%vertCount;
                } while (visited[prev]);
                next = curr;
                do {
                    next = (next+vertCount+1)%vertCount;
                } while (visited[next]);
                
                if (isConvex(prev, curr, next)
                    && isEar(prev, curr, next)
                )
                {
                    visited[curr] = true;
                    unvisited--;
                    vh.AddTriangle(prev, curr, next);
                    LineRenderer lr = Instantiate(prefabT);
                    lr.name = ""+prev+ " "+curr + " " + next;
                    lr.numPositions = 4;
                    lr.SetPosition(0, list[prev].FlatPosition);
                    lr.SetPosition(1, list[curr].FlatPosition);
                    lr.SetPosition(2, list[next].FlatPosition);
                    lr.SetPosition(3, list[prev].FlatPosition);
//                    curr = Random.Range(0, vertCount-1);

                }
            }
        }
    }

    float sign (Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
    }

    bool PointInTriangle (Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        bool b1, b2, b3;

        b1 = sign(pt, v1, v2) < 0.0f;
        b2 = sign(pt, v2, v3) < 0.0f;
        b3 = sign(pt, v3, v1) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }

    bool isEar(int prev, int curr, int next)
    {
        var prevC = list[prev].ToVector3();
        var currC = list[curr].ToVector3();
        var nextC = list[next].ToVector3();
        for(int i = 0; i < list.Count; i++) {
            if (!visited[i] && (i != prev) && (i != next) && (i != curr) && PointInTriangle(list[i].ToVector3(), prevC, currC, nextC)) {
                return false;
            }
        }
        return true;
    }

    bool isConvex(int prev, int curr, int next)
    {
        var prevC = list[prev].ToVector3();
        var currC = list[curr].ToVector3();
        var nextC = list[next].ToVector3();

        var toPrev = (prevC - currC).normalized;
        var toCurr = (currC - nextC).normalized;
        var crossC = Vector3.Cross(toPrev, toCurr);
        return crossC.z < 0;
    }

    List<Coord> list = new List<Coord>();

    public void Load(TextAsset csv)
    {
        var result = Regex.Split(csv.text, "\r\n|\r|\n");

        for (int i = 0; i < result.Length; i++)
        {
            var line = result[i];
            if (line.StartsWith("#") || line.StartsWith("\n") || line.StartsWith("\r") || line.Length == 0)
            {
                //ignore comment and emplty lines
            }
            else
            {
                Coord coord = ParseCoord(line);
                list.Add(coord);
            }
        }
    }

    public Coord ParseCoord(string line)
    {
        var result = line.Split(new char[] { ',' });
        
        return Coord.ParseCoord("", result[0], result[1]);
    }
}
