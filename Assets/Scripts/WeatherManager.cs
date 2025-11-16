using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class WeatherRoot
{
    public WeatherMain main;
    public WeatherCondition[] weather;
    public string name;
    public int dt;
    public int timezone;
}

[System.Serializable]
public class WeatherMain
{
    public float temp;
}

[System.Serializable]
public class WeatherCondition
{
    public string main;
    public string description;
}

public class WeatherManager : MonoBehaviour
{
    [HideInInspector] public string currentWeatherConditions;
    [HideInInspector] public string currentTemperature;
    [HideInInspector] public string currentCityName;
    [HideInInspector] public string currentTime; // Time at the location of the most recent API call, formatted as HH:mm:ss, use this one for UI display
    [HideInInspector] public int currentTimeINT; // Time in minutes since midnight at the location of the most recent API call, use this one for sun position in game

    private string APIKey = "166e0498ec70931f9ce60429eb4fe361";

    private Location[] Locations = 
    {
        new Location("Orlando",  "28.5383", "-81.3792"),
        new Location("Los Angeles", "34.0522", "-118.2437"),
        new Location("Tokyo", "35.6895", "139.6917"),
        new Location("Paris", "48.8566", "2.3522"),
        new Location("Sydney", "-33.8688", "151.2093")
    };

    private string[] LocationURLs = new string[5];

    [SerializeField] private Material[] WeatherConditionSkyBoxes = new Material[4];
    [SerializeField] private Material[] TimeOfDaySkyBoxes = new Material[4];

    private void Awake()
    {
        for (int i = 0; i < Locations.Length; i++)
        {
            LocationURLs[i] = $"https://api.openweathermap.org/data/2.5/weather?lat={Locations[i].lat}&lon={Locations[i].lon}&appid={APIKey}";
        }
    }

    private void Start()
    {
        StartCoroutine(GetWeatherJSON(0, SetWeatherInfo));
    }

  private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(GetWeatherJSON(0, SetWeatherInfo));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(GetWeatherJSON(1, SetWeatherInfo));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartCoroutine(GetWeatherJSON(2, SetWeatherInfo));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            StartCoroutine(GetWeatherJSON(3, SetWeatherInfo));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            StartCoroutine(GetWeatherJSON(4, SetWeatherInfo));
        }
    }

    private void SetWeatherInfo(string JSON)
    {
        WeatherRoot weatherData = JsonUtility.FromJson<WeatherRoot>(JSON);

        if (weatherData == null)
        {
            Debug.LogError("Failed to parse weather data.");
            return;
        }

        float tempFahrenheit = (weatherData.main.temp - 273.15f) * 9f / 5f + 32f;
        currentTemperature = $"{tempFahrenheit:F1} °F";

        currentWeatherConditions = weatherData.weather[0].description;
        currentCityName = weatherData.name;

        long localUnixTime = weatherData.dt + weatherData.timezone;

        DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(localUnixTime).DateTime;
        currentTime = dateTime.ToString("HH:mm:ss");

        currentTimeINT = dateTime.Hour * 60 + dateTime.Minute;

        Debug.Log("Weather Data Updated:");
        Debug.Log($"City: {currentCityName}, Temp: {currentTemperature}, Conditions: {currentWeatherConditions}, Time: {currentTime}");
        Debug.Log($"Time INT (minutes since midnight): {currentTimeINT}");
        
        //check for clear sky and time of day
        if(currentWeatherConditions == "clear sky" && dateTime.Hour >= 12 && dateTime.Hour <= 8)
        {
           RenderSettings.skybox = TimeOfDaySkyBoxes[0];
        }
        else if(currentWeatherConditions == "clear sky" && dateTime.Hour >= 9 && dateTime.Hour <= 15)
        {
            RenderSettings.skybox = TimeOfDaySkyBoxes[1];
        }
        else if(currentWeatherConditions == "clear sky" && dateTime.Hour >= 16 && dateTime.Hour <= 18)
        {
            RenderSettings.skybox = TimeOfDaySkyBoxes[2];
        }
        else if(currentWeatherConditions == "clear sky" && dateTime.Hour >= 19 && dateTime.Hour <= 24)
        {
            RenderSettings.skybox = TimeOfDaySkyBoxes[3];
        }
        //check for weather conditions
        else if(currentWeatherConditions == "light rain" || currentWeatherConditions == "moderate rain" || currentWeatherConditions == "heavy intensity rain" || currentWeatherConditions == "very heavy rain")
        {
            RenderSettings.skybox = WeatherConditionSkyBoxes[1];
        }
        else if(currentWeatherConditions == "light snow" || currentWeatherConditions == "snow" || currentWeatherConditions == "heavy snow")
        {
            RenderSettings.skybox = WeatherConditionSkyBoxes[4];
        }
        else if(currentWeatherConditions == "few clouds" || currentWeatherConditions == "scattered clouds" || currentWeatherConditions == "broken clouds" || currentWeatherConditions == "overcast clouds")
        {
            RenderSettings.skybox = WeatherConditionSkyBoxes[3];
        }
       
            
            
        
    }

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

    public IEnumerator GetWeatherJSON(int index, Action<string> callback) 
    {
        return CallAPI(LocationURLs[index], callback);
    }
}
