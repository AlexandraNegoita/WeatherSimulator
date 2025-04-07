using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class WeatherConfigs : MonoBehaviour {
    public Dictionary<string, WeatherConfig> weather = new Dictionary<string, WeatherConfig>() {
        {"CLEAR_SKY", new WeatherConfig(
            new waterConfig(
                "04406B",
                33f
            ),
            new cloudConfig(
                0.529f,
                0.953f,
                VolumetricClouds.CloudPresets.Sparse
            ),
            new terrainConfig(
                0.87f,
                2.6f
            )
        )},
        {"HOT", new WeatherConfig(
            new waterConfig(
                "267584",
                7.0f
            ),
            new cloudConfig(
                0.0f,
                0.953f,
                VolumetricClouds.CloudPresets.Sparse
            ),
            new terrainConfig(
                0.0f,
                2.6f
            )
        )},
        {"CLOUDY", new WeatherConfig(
            new waterConfig(
                "13334B",
                110f
            ),
            new cloudConfig(
                0.656f,
                0.796f,
                VolumetricClouds.CloudPresets.Cloudy
            ),
            new terrainConfig(
                0.95f,
                2.6f
            )
        )},
        {"RAIN", new WeatherConfig(
            new waterConfig(
                "171C26",
                156f
            ),
            new cloudConfig(
                1.0f,
                0.641f,
                VolumetricClouds.CloudPresets.Stormy
            ),
            new terrainConfig(
                1.5f,
                2.6f
            )
        )},
        {"SNOW", new WeatherConfig(
            new waterConfig(

            ),
            new cloudConfig(

            ),
            new terrainConfig(
                
            )
        )},
        {"FREEZE", new WeatherConfig(
            new waterConfig(

            ),
            new cloudConfig(

            ),
            new terrainConfig(
                
            )
        )},
    };
    public Color FromHex(string hex)
    {
        var r = hex.Substring(0, 2);
        var g = hex.Substring(2, 2);
        var b = hex.Substring(4, 2);
        string alpha;
        if (hex.Length >= 8)
            alpha = hex.Substring(6, 2);
        else
            alpha = "FF";

        return new Color((int.Parse(r, NumberStyles.HexNumber) / 255f),
                        (int.Parse(g, NumberStyles.HexNumber) / 255f),
                        (int.Parse(b, NumberStyles.HexNumber) / 255f),
                        (int.Parse(alpha, NumberStyles.HexNumber) / 255f));
    }
}

public struct WeatherConfig
{
    public waterConfig water;
    public cloudConfig clouds;
    public terrainConfig terrain;
    public WeatherConfig(waterConfig waterConfig, cloudConfig cloudConfig, terrainConfig terrainConfig) : this()
    {
        this.water = waterConfig;
        this.clouds = cloudConfig;
        this.terrain = terrainConfig;
    }
}
public struct terrainConfig {
    public float shininess;
    public float detail;

    public terrainConfig(float shininess, float detail) : this()
    {
        this.shininess = shininess;
        this.detail = detail;
    }
} 
public struct waterConfig {
    public string scatteringColor;
    public float distantWindSpeed;

    public waterConfig(string scatteringColor, float distantWindSpeed) : this()
    {
        this.scatteringColor = scatteringColor;
        this.distantWindSpeed = distantWindSpeed;
    }
}

public struct cloudConfig {
    public float densityMultiplier;
    public float shapeFactor;
    public VolumetricClouds.CloudPresets cloudPreset;

    public cloudConfig(float densityMultiplier, float shapeFactor, VolumetricClouds.CloudPresets cloudPreset) : this()
    {
        this.densityMultiplier = densityMultiplier;
        this.shapeFactor = shapeFactor;
        this.cloudPreset = cloudPreset;
    }
}


