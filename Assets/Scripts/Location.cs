using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Location
{
    public string cityName;
    public string lat;
    public string lon;

    public Location(string cityName, string lat, string lon)
    {
        this.cityName = cityName;
        this.lat = lat;
        this.lon = lon;
    }
}
