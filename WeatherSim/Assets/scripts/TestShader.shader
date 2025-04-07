Shader "TestShader" {
    Properties {
        _PeakColor ("PeakColor", Color) = (0.8, 0.9, 0.9, 1)
        _PeakLevel ("PeakLevel", Float) = 300
        _Level3Color ("Level3Color", Color) = (0.75, 0.53, 0, 1)
        _Level3 ("Level3", Float) = 200
        _Level2Color ("Level2Color", Color) = (0.69, 0.63, 0.31, 1)
        _Level2 ("Level2", Float) = 100
        _Level1Color ("Level1Color", Color) = (0.65, 0.86, 0.63, 1)
        _Level1 ("Level1", Float) = 50 // Added missing Level1
        _WaterLevel ("WaterLevel", Float) = 0
        _WaterColor ("WaterColor", Color) = (0.37, 0.78, 0.92, 1)
        _Slope ("Slope Fader", Range(0, 1)) = 0
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        Fog { Mode Off }
        Tags { "LightMode" = "Always" }
        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        struct Input {
            float3 customColor;
            float3 worldPos;
        };
        
        void vert (inout appdata_full v, out Input o) {
            o.customColor = abs(v.normal.y); // This creates a simple gradient from the surface normal
            // Get the world position by transforming the vertex position into world space
            o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Transforms vertex to world space
        }
    
        float _PeakLevel;
        float4 _PeakColor;
        float _Level3;
        float4 _Level3Color;
        float _Level2;
        float4 _Level2Color;
        float _Level1;
        float4 _Level1Color;
        float _Slope;
        float _WaterLevel;
        float4 _WaterColor;
    
        void surf (Input IN, inout SurfaceOutput o) {
            // Initial color assignment
            float4 finalColor = _WaterColor;  // Default color (water color)
    
            // Apply level colors based on world position y value
            if (IN.worldPos.y >= _PeakLevel) {
                finalColor = _PeakColor;
            }
            else if (IN.worldPos.y >= _Level3) {
                finalColor = lerp(_Level3Color, _PeakColor, (IN.worldPos.y - _Level3) / (_PeakLevel - _Level3));
            }
            else if (IN.worldPos.y >= _Level2) {
                finalColor = lerp(_Level2Color, _Level3Color, (IN.worldPos.y - _Level2) / (_Level3 - _Level2));
            }
            else if (IN.worldPos.y >= _Level1) {
                finalColor = lerp(_Level1Color, _Level2Color, (IN.worldPos.y - _Level1) / (_Level2 - _Level1));
            }
    
            // Update the final color based on the slope effect
            finalColor *= float4(saturate(IN.customColor + _Slope), 1.0);
    
            // Assign the final color to Albedo
            o.Albedo = float4(finalColor.rgb, 1.0);
        }
        ENDCG
    }
    Fallback "Diffuse"
    }
    