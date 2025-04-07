using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class WeatherManager : MonoBehaviour
{
    public WeatherConfigs weatherConfigs;
    public WaterSurface ocean;
    public VolumeProfile volume;
   // public Shader terrain;

    public void changeWeather(string weather){
        WeatherConfig currentWeather;
        VolumetricClouds clouds;
        if (volume.TryGet<VolumetricClouds>(out clouds))
        {
            if(weatherConfigs.weather.ContainsKey(weather)) {
                currentWeather = weatherConfigs.weather[weather];
                // water configs
                ocean.scatteringColor = weatherConfigs.FromHex(currentWeather.water.scatteringColor);
                ocean.largeWindSpeed = currentWeather.water.distantWindSpeed;

                // clouds configs
               // clouds.densityMultiplier = new ClampedFloatParameter(currentWeather.clouds.densityMultiplier, 0.0f, 1.0f);
                //clouds.shapeFactor = new ClampedFloatParameter(currentWeather.clouds.shapeFactor, 0.0f, 1.0f);
                clouds.cloudPreset = currentWeather.clouds.cloudPreset;
              

                // terrain configs
                //terrain.
                Debug.Log("Weather: " + weather);
            }
        }
    }
}
