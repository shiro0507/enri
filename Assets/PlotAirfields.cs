using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class PlotAirfields : MonoBehaviour {

    public TextAsset asset;
    public GameObject prefab;

    // Use this for initialization
    void Start () {
        Load(asset);

        foreach(Coord bin in list) {
            GameObject segment = Instantiate(prefab);
            segment.transform.parent = transform;
            segment.name = bin.name + bin.Latitude + " " + bin.Longitude;
            segment.transform.localPosition = bin.FlatPosition;
        }
    }

    List<Coord> list = new List<Coord>();

    public void Load(TextAsset csv)
    {
        var result = Regex.Split(csv.text, "\r\n|\r|\n");

        for(int i = 1 ; i < result.Length ; i++)
        {
            var line = result[i];
            if (line.StartsWith("#") || line.StartsWith("\n") || line.StartsWith("\r") || line.Length == 0) {
                //ignore comment and emplty lines
            } else {
                Coord coord = ParseCoord(line);
                list.Add(coord);
            }
        }
    }

    public Coord ParseCoord(string line) {
        var coord = new Coord();
        var result = line.Split(new char[] { ',' });
        coord.name = result[2];
        coord.Latitude =  float.Parse(result[5]);

        coord.Longitude =  float.Parse(result[6]);

        return coord;
    }
}
