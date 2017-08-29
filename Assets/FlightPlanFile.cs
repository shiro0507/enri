using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FlightPlanFile {

    public Dictionary<string, FlightPlan> plans = new Dictionary<string, FlightPlan>();

    public void Load(string[] paths, PlotWaypoints plotWaypoints)
    {
        foreach (var path in paths)
        {
            var pathMod = path;
            if (path.StartsWith("file:")) {
                pathMod = pathMod.Substring(7);
            }
            using (StreamReader reader = File.OpenText(pathMod))
            {
                string flightId = null;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    if (line.Contains("["))
                    {
                        flightId = line.Substring(1, line.Length - 2);
                    }
                    else
                    {
                        try
                        {
                            var split = line.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                            FlightPlanPoint row = new FlightPlanPoint();
                            Coord coord = Coord.ParseCoord(split[0], split[1], split[2]);
                            row.WaypointID = coord.name;
                            row.Latitude = coord.Latitude;
                            row.Longitude = coord.Longitude;
                            row.firstAltitude = int.Parse(split[3]);
                            row.secondAltitude = int.Parse(split[4]);

                            plotWaypoints.CreateWP(coord, true);
                            plotWaypoints.dynamiclist.Add(coord);

                            FlightPlan flightPlanBlock;
                            if (!plans.TryGetValue(flightId, out flightPlanBlock))
                            {
                                flightPlanBlock = new FlightPlan();
                                plans.Add(flightId, flightPlanBlock);
                            }
                            flightPlanBlock.Destination = row;
                            flightPlanBlock.Waypoints.Add(row);
                        }
                        catch
                        {
                            Debug.Log("Parsing line of txt failed");
                            continue;
                        }
                    }
                }
            }
        }
    }
}
