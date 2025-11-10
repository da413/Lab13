using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Location
{
    string cityName, cityCode, countryCode;

    public Location(string cityName, string cityCode, string countryCode)
    {
        this.cityName = cityName;
        this.cityCode = cityCode;
        this.countryCode = countryCode;
    }
}
