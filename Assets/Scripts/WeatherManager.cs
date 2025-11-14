using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    private string APIKey = "166e0498ec70931f9ce60429eb4fe361";

    private Location[] Locations = 
    {
        new Location("Orlando", "OrlandoCode", "CountryCode"),
        new Location("Los Angeles", "OrlandoCode", "CountryCode"),
        new Location("Tokyo", "OrlandoCode", "CountryCode"),
        new Location("Paris", "OrlandoCode", "CountryCode"),
        new Location("Sydney", "OrlandoCode", "CountryCode")
    };

    private const string OrlandoAPICall = $"http://api.openweathermap.org/geo/1.0/direct?q={Locations[0].cityName},{Locations[0].cityCode},{Locations[0].countryCode}&limit={limit}&appid={APIKey}";
    private const string OrlandoAPICall = $"http://api.openweathermap.org/geo/1.0/direct?q={Locations[0].cityName},{Locations[0].cityCode},{Locations[0].countryCode}&limit={limit}&appid={APIKey}";
    private const string OrlandoAPICall = $"http://api.openweathermap.org/geo/1.0/direct?q={Locations[0].cityName},{Locations[0].cityCode},{Locations[0].countryCode}&limit={limit}&appid={APIKey}";
    private const string OrlandoAPICall = $"http://api.openweathermap.org/geo/1.0/direct?q={Locations[0].cityName},{Locations[0].cityCode},{Locations[0].countryCode}&limit={limit}&appid={APIKey}";
    private const string OrlandoAPICall = $"http://api.openweathermap.org/geo/1.0/direct?q={Locations[0].cityName},{Locations[0].cityCode},{Locations[0].countryCode}&limit={limit}&appid={APIKey}";

    private IEnumerator CallAPI(string url, Action<string> callback) 
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url)) 
        {    
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError) 
            {
                Debug.LogError($"network problem: {request.error}");
            } 
            else if (request.result == UnityWebRequest.Result.ProtocolError) 
            {
                Debug.LogError($"response error: {request.responseCode}");
            } 
            else 
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherJSON(Action<string> callback) 
    {
        return CallAPI(OrlandoAPICall, callback);
    }
}
