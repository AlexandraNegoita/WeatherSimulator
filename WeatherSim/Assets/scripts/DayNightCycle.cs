using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Range(0f, 24f)]
    public float currentTime;
    public float timeSpeed = 1f;

    [Header("CurrentTime")]
    public string currentTimeString;
    public bool isDay = true;

    [Header("DaySettings")]
    public Light sunLight;
    public float sunPosition = 1f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve sunLightTemperature;
    

    [Header("NightSettings")]
    public Light moonLight;
    public float moonPosition = 1f;
    public float moonIntensity = 1f;
    public AnimationCurve moonIntensityMultiplier;
    public AnimationCurve moonLightTemperature;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;
        if(currentTime >= 24) {
            currentTime = 0;
        }
        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
    }

    private void OnValidate() {
        UpdateLight();
        CheckShadowStatus();
    }

    void UpdateTimeText() {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ":" + ((currentTime % 1) * 60).ToString("00");
    }
    void UpdateLight() {
        float sunRotation = currentTime / 24f * 360f;
        float moonRotation = currentTime / 24f * 360f + 180f;
       // Debug.Log("sun: " + sunRotation);
        //Debug.Log("moon: " + moonRotation);
        if(sunLight)
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, sunPosition, 0f);

        if(moonLight)
        moonLight.transform.rotation = Quaternion.Euler(moonRotation - 90f, moonPosition, 0f);

        float normalizedTime = currentTime / 24f;
        float sunIntensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);
        float moonIntensityCurve = moonIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData sunlightData = sunLight.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonlightData = moonLight.GetComponent<HDAdditionalLightData>();

        if(sunlightData) {
            sunlightData.intensity = sunIntensityCurve * sunIntensity;
        }

        if(moonlightData) {
            moonlightData.intensity = moonIntensityCurve * moonIntensity;
        }

        float sunTemperatureMultiplier = sunLightTemperature.Evaluate(normalizedTime);
        float moonTemperatureMultiplier = moonLightTemperature.Evaluate(normalizedTime);

        Light sunLightComponent = sunLight.GetComponent<Light>();
        Light moonLightComponent = moonLight.GetComponent<Light>();

        if(sunLightComponent != null) {
            sunLightComponent.colorTemperature = sunTemperatureMultiplier * 10000f;
        }
        if(moonLightComponent != null) {
            moonLightComponent.colorTemperature = moonTemperatureMultiplier * 10000f;
        }
    }
    void CheckShadowStatus() {
        HDAdditionalLightData sunlightData = sunLight.GetComponent<HDAdditionalLightData>();
        HDAdditionalLightData moonlightData = moonLight.GetComponent<HDAdditionalLightData>();

        float currentSunRotation = currentTime;

        if(currentSunRotation >= 6f && currentSunRotation <= 18f) {
            sunlightData.EnableShadows(true);
            moonlightData.EnableShadows(false);
            isDay = true;
        } else {
            sunlightData.EnableShadows(false);
            moonlightData.EnableShadows(true);
            isDay = false;
        }
    }
}
