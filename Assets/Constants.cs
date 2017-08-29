using UnityEngine;

public class Constants {

	public class WGS84_ellipsoid {
		public const float a = 6378137;			//major radius
		public const float f = 1/298.257223563f;//flattening
		public const float b = a * (1 - f);		//polar axis
	}

	public class Sphere {
		public static Vector3 ToCartesian(Vector3 spherical, float globeRadius = Constants.globeRadius) {
			float latitude = Mathf.Deg2Rad * spherical.x;
			float longtitude = Mathf.Deg2Rad * spherical.y;
			float r = globeRadius * (WGS84_ellipsoid.a + spherical.z)/WGS84_ellipsoid.a;
			float x = -r * Mathf.Sin(longtitude) * Mathf.Cos(latitude);
			float y = r * Mathf.Sin(latitude);
			float z = r * Mathf.Cos(latitude) * Mathf.Cos(longtitude);
			return new Vector3(x, y, z);
		}
	}

	//this should be the same as a radius of the earth 3d model (currently it is set in Radius field of EarthScript)
	public const float globeRadius = 1000f;
}

public class Coord {
    public float Latitude;      // (deg)
    public float Longitude;     // (deg)
    public string name;
    public GameObject go;

    public Vector3 ToVector3() {
        return new Vector2(Latitude, Longitude);
    }

    public Vector3 FlatPosition {
        get {
            return Constants.Sphere.ToCartesian(new Vector3(Latitude, Longitude, 0));
        }
    }

    public static Coord ParseDecimalCoord(string name, string latitude, string longitude)
    {
        var coord = new Coord();
        coord.name = name;
        coord.Latitude = float.Parse(latitude) / 10000;
        var lat = coord.Latitude;
        var tmp = lat % 1;
        coord.Latitude = lat - tmp + tmp / 0.60f;
        coord.Longitude = float.Parse(longitude) / 10000;
        lat = coord.Longitude;
        tmp = lat % 1;
        coord.Longitude = lat - tmp + tmp / 0.60f;
        return coord;
    }

    public static Coord ParseNormalCoord(string name, string latitude, string longitude)
    {
        var coord = new Coord();
        coord.name = name;
        coord.Latitude = float.Parse(latitude);
        coord.Longitude = float.Parse(longitude);
        return coord;
    }

    public static Coord ParseCoord(string name, string latitude, string longitude)
    {
        if (latitude[2] == '.') {
            return Coord.ParseNormalCoord(name, latitude, longitude);
        } else {
            return Coord.ParseDecimalCoord(name, latitude, longitude);
        }
    }
}