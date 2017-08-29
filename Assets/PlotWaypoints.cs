using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PlotWaypoints : MonoBehaviour
{

    public TextAsset asset;
    public GameObject prefab;
    [SerializeField]
    private Canvas canvas;

    void Start()
    {
        Load(asset);

        foreach (Coord bin in list)
        {

            CreateWP(bin);
        }
    }

    public void CreateWP(Coord bin, bool dynamic = false)
    {
        GameObject segment = Instantiate(prefab);
        segment.transform.parent = transform;
        segment.name = bin.name + bin.Latitude + " " + bin.Longitude + " " + dynamic;
        segment.transform.localPosition = bin.FlatPosition;
        bin.go = segment;


        GameObject newFlightPoint = new GameObject(bin.name);
        Text pointText = newFlightPoint.AddComponent<Text>();
        newFlightPoint.transform.SetParent(canvas.transform);
        newFlightPoint.transform.rotation = new Quaternion(0, 0, 0, 0);
        newFlightPoint.transform.localScale = new Vector3(4, 4, 4);
        newFlightPoint.GetComponent<RectTransform>().pivot = new Vector2(0, 1);

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        pointText.font = ArialFont;
        pointText.material = ArialFont.material;
        pointText.raycastTarget = false;

        ShowPointTag spt = segment.AddComponent<ShowPointTag>();
        spt.canvas = canvas;
        spt.tagText = pointText;
        pointText.text = bin.name;

        segment.SetActive(showall != dynamic);
    }

    bool showall = false;
    public void ToggleAllAndDynamic()
    {
        showall = !showall;
        foreach (Coord bin in list)
        {
            bin.go.SetActive(showall);
        }

        foreach (Coord bin in dynamiclist)
        {
            bin.go.SetActive(!showall);
        }
    }

    public void Clear() {
        foreach (Coord bin in dynamiclist)
        {
            Destroy(bin.go);
            //TODO clear text and showpointtag also
        }
        dynamiclist.Clear();
    }

    List<Coord> list = new List<Coord>();
    public List<Coord> dynamiclist = new List<Coord>();

    public void Load(TextAsset csv)
    {
        var result = Regex.Split(csv.text, "\r\n|\r|\n");

        for (int i = 0; i < result.Length; i++)
        {
            var line = result[i];
            if (line.StartsWith("#") || line.StartsWith("\n") || line.StartsWith("\r") || line.Length == 0)
            {
                //ignore comment and empty lines
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
        return Coord.ParseDecimalCoord(result[0], result[1], result[2]);
    }
}
