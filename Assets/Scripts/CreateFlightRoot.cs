using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

// 3Dの飛行経路を作成
public class CreateFlightRoot : MonoBehaviour {
	[SerializeField] private Material lineM;
	[SerializeField] private Material shadowLineM;
	[SerializeField] private Material pointM;
	[SerializeField] private float lineWidth;
	[SerializeField] private float pointRadius;
	[SerializeField] private Canvas canvas;
    public TextAsset dataFileTemp;

	// 通過点から次の通過点までラインを引く
	LineRenderer CreateFlightLine(GameObject point){
		LineRenderer lineRenderer = point.gameObject.AddComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.widthMultiplier = lineWidth;


		point.GetComponent<LineRenderer> ().material = lineM;
		return lineRenderer;
	}

	void CreateShadowLine(GameObject point, Vector3 pos1, Vector3 pos2){
		LineRenderer lineRenderer = point.gameObject.AddComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.widthMultiplier = lineWidth*0.5f;
        lineRenderer.numPositions = 2;
		lineRenderer.SetPosition (0, pos1);
		lineRenderer.SetPosition (1, pos2);

		point.GetComponent<LineRenderer> ().material = shadowLineM;
	}

	// 通過点を作成し情報テキストを表示
	GameObject CreateFlightPoint(Vector3 pos, string flightID){
		GameObject point = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		point.transform.position = pos;
		point.transform.localScale = new Vector3 (pointRadius,pointRadius,pointRadius);
		point.GetComponent<MeshRenderer> ().material = pointM;
		point.transform.parent = this.transform;

		GameObject newFlightPoint = new GameObject(flightID);
		Text pointText = newFlightPoint.AddComponent<Text>();
		newFlightPoint.transform.SetParent(canvas.transform);
		newFlightPoint.transform.rotation = new Quaternion (0, 0, 0, 0);
		newFlightPoint.transform.localScale = new Vector3 (4, 4, 4);
		newFlightPoint.GetComponent<RectTransform> ().pivot = new Vector2 (0, 1);

		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		pointText.font = ArialFont;
		pointText.material = ArialFont.material;
		pointText.raycastTarget = false;

		ShowPointTag spt = point.AddComponent<ShowPointTag>();
		spt.canvas = canvas;
		spt.tagText = pointText;
        pointText.text = flightID;

		return point;
	}

	public int samplingStep = 1;
	Transform ShowFlightLine (Flight flight) {
		Transform flightRoot = new GameObject(flight.datapoints[0].FlightID).transform;
		flightRoot.parent = this.transform;

		var flightPoints = flight.datapoints;

		GameObject point = CreateFlightPoint (flightPoints [0].Position, flight.datapoints[0].FlightID);
		point.transform.parent = flightRoot;
		flight.planeIndicator = point.transform;
	
		LineRenderer lineRenderer = CreateFlightLine (flightRoot.gameObject);
		flight.lineRenderer = lineRenderer;
        lineRenderer.numPositions = flightPoints.Count;
		for (int i = 0; i < flightPoints.Count; i = i+1) {
			flight.positions.Add(flightPoints[i].Position);
			lineRenderer.SetPosition(i, flightPoints[i].Position);

			//shadow line
//			var go = new GameObject();
//			go.transform.parent = point;
//			CreateShadowLine (go, flightPoints [i].Position, flightPoints [i].FlatPosition);
		}
		return flightRoot;
	}

    TimeSpan _current = new TimeSpan(12,00,00);
    TimeSpan Current {
        set {
            _current = value;
            if (OnTimeChange != null) {
                OnTimeChange((_current.TotalMilliseconds - flightData.min.TotalMilliseconds)/(flightData.max.TotalMilliseconds - flightData.min.TotalMilliseconds));
            }
        }
    }
    public AltitudeLines altitudeLines;
    public PlotWaypoints pw;

    public static FlightDataFile flightData = new FlightDataFile();
    public static FlightPlanFile flightPlan = new FlightPlanFile();

    public void UpdateData(string[] paths)
    {
        flightData.Load(paths);
        Current = flightData.min;
    }

    public void UpdatePlan(string[] paths)
    {
        flightPlan.Load(paths, pw);
    }

    Flight flight;

    void FixedUpdate() {
        
        foreach (var flightId in flightData.flights.Keys)
        {
            if (flightData.flights.TryGetValue(flightId, out flight))
            {
                if (_current >= flight.start && _current <= flight.end && flight.flightPathIndicator == null)
                {
                    flight.flightPathIndicator = ShowFlightLine(flight);
                    altitudeLines.AddFlight(flight);
                }
                if (_current > flight.end && flight.flightPathIndicator != null)
                {
                    GameObject.Destroy(flight.flightPathIndicator.gameObject);
                    flight.flightPathIndicator = null;
                    altitudeLines.RemoveFlight(flight);
                }
                if (_current >= flight.start && _current <= flight.end)
                {
                    flight.UpdateIndicators(_current);


                    if (flight.IsShown())
                        {
                            flight.flightPathIndicator.gameObject.SetActive(true);
                        }
                        else
                        {
                            flight.flightPathIndicator.gameObject.SetActive(false);
                        }
                }
            }
        }
        altitudeLines.Refresh();
        if (playing) {
		    Current = _current.Add(new TimeSpan(0,0,1));
        }
	}

    public Action<double>  OnTimeChange;

    public bool playing;
    public void SetTime(float time) {
        foreach (var flight in flightData.flights.Values) {
            flight.Reset();
        }
            
        _current = TimeSpan.FromMilliseconds(flightData.min.TotalMilliseconds + time * (flightData.max.TotalMilliseconds - flightData.min.TotalMilliseconds));
    }
}
