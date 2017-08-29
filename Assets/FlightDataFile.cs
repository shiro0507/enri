using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class FlightDataFile
{
    public Dictionary<string, Flight> flights = new Dictionary<string, Flight>();

    public TimeSpan min = TimeSpan.MaxValue;
    public TimeSpan max = TimeSpan.MinValue;


    public void Clear() {
        flights.Clear();
        min = TimeSpan.MaxValue;
        max = TimeSpan.MinValue;
    }
    
	public void Load(string[] paths)
	{
        foreach (var path in paths)
        {
            var pathMod = path;
            if (path.StartsWith("file:")) {
                pathMod = pathMod.Substring(7);
            }
            using (StreamReader reader = File.OpenText(pathMod))
            {
                //skip header
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    try
                    {
                        string[][] grid = CsvParser2.Parse(line);
                        FlightDataPoint row = new FlightDataPoint();
                        row.Time = TimeSpan.Parse(grid[0][0]);

                        Coord coord = Coord.ParseCoord(grid[0][1], grid[0][2], grid[0][3]);
                        row.FlightID = coord.name;
                        row.Latitude = coord.Latitude;
                        row.Longitude = coord.Longitude;

                        row.Altitude = float.Parse(grid[0][4]);
                        row.AircraftType = grid[0][5];
                        if (grid[0].Length >= 10)
                        {
                            row.CAS = grid[0][6];
                            row.TAS = grid[0][7];
                            row.GS = grid[0][8];
                            row.MACH = grid[0][9];
                        }

                        Flight flight;
                        if (!flights.TryGetValue(row.FlightID, out flight))
                        {
                            flight = new Flight();
                            flight.flightId = row.FlightID;
                            flights.Add(row.FlightID, flight);
                            flight.start = row.Time;
                        }
                        flight.end = row.Time;

                        flight.datapoints.Add(row);
                    }
                    catch
                    {
                        Debug.Log("Parsing line of csv failed");
                        continue;
                    }
                }
            }
        }

        foreach(Flight flight in flights.Values) {
            if (flight.start < min) {
                min = flight.start;
            }
            if (flight.end > max) {
                max = flight.end;
            }
        }
	}
}