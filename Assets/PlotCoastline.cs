using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PlotCoastline : MonoBehaviour {

    List<Bin> bins = new List<Bin>();

 

    public class Bin {
        public List<Coord> list = new List<Coord>();
        public int number;
        public int level;
    }

    public TextAsset asset;

	// Use this for initialization
	void Start () {
        Load(asset);

        foreach(Bin bin in bins) {
            GameObject segment = new GameObject();
            segment.transform.parent = transform;
            LineRenderer lineRenderer = CreateFlightLine(segment);
            lineRenderer.numPositions = bin.list.Count;
            for (int i = 0; i < bin.list.Count; i++) {
                lineRenderer.SetPosition(i, bin.list[i].FlatPosition);

             }
        }
	}

    [SerializeField] private Material lineM;
    [SerializeField] float lineWidth = 1f;

    LineRenderer CreateFlightLine(GameObject point){
        LineRenderer lineRenderer = point.gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.material = lineM;
        lineRenderer.useWorldSpace = false;
        return lineRenderer;
    }
	
    public void Load(TextAsset csv)
    {
        var result = Regex.Split(csv.text, "\r\n|\r|\n");

        Bin current = null;
        for(int i = 0 ; i < result.Length ; i++)
        {
            var line = result[i];
            if (line.StartsWith("#") || line.StartsWith("\n") || line.StartsWith("\r") || line.Length == 0) {
                //ignore comment and emplty lines
            } else if (line.StartsWith(">")) {
                current = ParseBin(line);
                bins.Add(current);
            } else {
                Coord coord = ParseCoord(line);
                current.list.Add(coord);
            }
        }
    }

    public Coord ParseCoord(string line) {
        var coord = new Coord();
        var result = line.Split(new char[] { '\t' });
        coord.Longitude = float.Parse(result[0]);
        coord.Latitude = float.Parse(result[1]);

        return coord;
    }

    public Bin ParseBin(string line) {
        var bin = new Bin();
        var result = line.Split(new char[] { ' ', ',' });
        bin.number = int.Parse(result[4]);
        bin.level = int.Parse(result[7]);
        return bin;
    }
}
