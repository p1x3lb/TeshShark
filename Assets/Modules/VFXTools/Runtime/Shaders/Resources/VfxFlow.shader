Shader "VFXTools/Flow Effect"
{
	Properties
	{
		[NoScaleOffset]
		_MainTex ("Sprite Texture", 2D) = "white" {}
		[NoScaleOffset]
		_FlowMap ("Flow Map", 2D) = "white" {}
		[NoScaleOffset]
		_Mask ("Mask", 2D) = "white" {}
		_MaskChannel ("Mask Channel", Range(0, 3)) = 0
		_Speed1 ("Speed 1", Float) = 0.25
		_Speed2 ("Speed 2", Float) = 0.125
		_Speed3 ("Speed 3", Float) = 0.5
		_Tiling1 ("Tiling 1", Float) = 4
		_Tiling2 ("Tiling 2", Float) = 2
		_Tiling3 ("Tiling 3", Float) = 8
		_FlowAlpha ("Flow Alpha", Range(0, 1)) = 1
		_FlowTiling ("Flow Tiling", Float) = 1
		_Strength ("Strength", Float) = 1
		_BlendingMode("Blending mode", Range(0, 1)) = 0.25
		_RotateDirection("Rotate direction", Range(-3.141593, 3.141593)) = 0
		_ColorA("Color A", Color) = (0, 0, 0, 0)
		_ColorB("Color B", Color) = (1, 1, 1, 1)
		_AlphaMul("Alpha Multiplier", Range(0, 10)) = 1
		_AlphaPow("Alpha Power", Range(0, 10)) = 1
		_BlinkWeight("Blink Weight", Range(0, 1)) = 1
		_BlinkFadeIn("Blink FadeIn Duration", Range(0, 10)) = 0.5
		_BlinkFadeOut("Blink FadeOut Duration", Range(0, 10)) = 0.5
		_BlinkVisibleTime("Blink Visible Time", Range(0, 10)) = 0
		_BlinkInvisibleTime("Blink Invisible Time", Range(0, 10)) = 0
		_BlinkInvisibleAlpha("Blink Invisible Alpha", Range(0, 1)) = 0
		_BlinkSpeed("Blink Speed", Range(0, 30)) = 1

        // --- Mask support ---
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        // ---
	}

	SubShader
	{
		Tags {
			"Queue"="Transparent"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
		}

        // --- Mask support ---
        Stencil {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

		Cull Off
		Lighting Off
		ZWrite Off
        ZTest Off
        ColorMask [_ColorMask]
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"
            #include "UnityUI.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float4 texcoord  : TEXCOORD0;
				float4 texcoord2  : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
				float4 color : COLOR;
				float blink : TEXCOORD3;
			};

			uniform sampler2D _MainTex;
			uniform sampler2D _FlowMap;
			uniform sampler2D _Mask;
			uniform uint _MaskChannel;
			uniform float _Speed1;
			uniform float _Speed2;
			uniform float _Speed3;
			uniform float _Tiling1;
			uniform float _Tiling2;
			uniform float _Tiling3;
			uniform float _FlowAlpha;
			uniform float _FlowTiling;
			uniform float _Strength;
			uniform float _BlendingMode;
			uniform float _RotateDirection;
			uniform float4 _ColorA;
			uniform float4 _ColorB;
			uniform float _AlphaMul;
			uniform float _AlphaPow;

			uniform float _BlinkWeight;
			uniform float _BlinkFadeIn;
			uniform float _BlinkFadeOut;
			uniform float _BlinkVisibleTime;
			uniform float _BlinkInvisibleTime;
			uniform float _BlinkInvisibleAlpha;
			uniform float _BlinkSpeed;

            uniform bool _UseClipRect;
            uniform float4 _ClipRect;
            uniform bool _UseAlphaClip;

			inline float Blink()
			{
				const float pi = 3.14159265;

				float4 duration;
				duration.x = _BlinkFadeIn;
				duration.y = duration.x + _BlinkVisibleTime;
				duration.z = duration.y + _BlinkFadeOut;
				duration.w = duration.z + _BlinkInvisibleTime;

				float time = (_Time.y * _BlinkSpeed + _BlinkFadeIn) % duration.w;
				float fadeInTime = time / _BlinkFadeIn;
				float fadeOutTime = (time - duration.y) / _BlinkFadeOut;

				float4 fade = cos(float4(fadeInTime + 1, 0, fadeOutTime, 1) * pi) * 0.5 + 0.5;

				float blink = lerp(fade.x, fade.y, step(duration.x, time));
				blink = lerp(blink, fade.z, step(duration.y, time));
				blink = lerp(blink, fade.w, step(duration.z, time));

				return lerp(1, lerp(_BlinkInvisibleAlpha, 1, blink), _BlinkWeight);
			}

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = float4(IN.texcoord, IN.texcoord * _Tiling1);
				OUT.texcoord2 = float4(IN.texcoord * _Tiling2, IN.texcoord * _Tiling3);
                OUT.worldPosition = IN.vertex;
				OUT.color = IN.color;
				OUT.blink = Blink();

				return OUT;
			}

			inline float ContantBias(float x)
			{
				return (x - 0.5) * 2.0;
			}

			inline float2 RotateUv(float2 uv, float2 center, float angle)
			{
				float s, c;
				sincos(angle, s, c);

			    float2x2 rMatrix = float2x2(c, -s, s, c);
			    rMatrix *= 0.5;
			    rMatrix += 0.5;
			    rMatrix = rMatrix * 2 - 1;
			    return mul(uv - center, rMatrix) + center;
			}

			inline fixed4 Sample(float2 rotatedUv, float time, float2 baseUv1, float2 baseUv2, float2 baseUv3)
			{
				float4 colorA = tex2D(_MainTex, frac(rotatedUv * time + baseUv1));
				float4 colorB = tex2D(_MainTex, frac(rotatedUv * time + baseUv2));
				float4 colorC = tex2D(_MainTex, frac(rotatedUv * time + baseUv3));
				return colorA * colorB * colorC * 4;
			}

			inline fixed4 Sample2(float2 rotatedUv, float speed, float2 baseUv, float angleSign)
			{
				float time = _Time.y * speed;
				float time1 = frac(time);
				float time2 = frac(time + 0.5);

				float angleOffset1 = time - time1;
				float angleOffset2 = time + 0.5 - time2;

				fixed4 colorA = tex2D(_MainTex, RotateUv(rotatedUv * time1 + baseUv, 0.5, angleOffset1 * angleSign));
				fixed4 colorB = tex2D(_MainTex, RotateUv(rotatedUv * time2 + baseUv, 0.5, angleOffset2 * angleSign));

				return lerp(colorA, colorB, abs(frac(time) * 2 - 1));
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				float4 flowMap = tex2D(_FlowMap, IN.texcoord.xy);
				float2 flow = flowMap.rg * 2 - 1;
				float len = length(flow);
				float2 dir = flow / len;
				float angle = atan2(dir.y, dir.x);

				float2 rotatedUv = RotateUv(float2(_Strength * len, 0), 0, angle + _RotateDirection);

				float4 c = Sample2(rotatedUv, _Speed1, IN.texcoord.zw, 1);
				c *= Sample2(rotatedUv, _Speed2, IN.texcoord2.xy, -1);
				c *= Sample2(rotatedUv, _Speed3, IN.texcoord2.zw, 1);
				c *= 4;

				c.a = tex2D(_Mask, IN.texcoord.xy)[_MaskChannel] * lerp(1, len, _FlowAlpha);
				c.a = pow(c.a * _AlphaMul, _AlphaPow);
				c.a *= IN.blink;

                if (_UseClipRect)
                {
                    c.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                }

                if (_UseAlphaClip)
                {
                    clip(c.a - 0.001);
                }

				c.rgb = lerp(_ColorA, _ColorB, c.r);
				c *= IN.color;
				c.rgb *= c.a;
				c = saturate(lerp(fixed4(c.rgb, 0), fixed4(c.rgb, c.a), _BlendingMode));

				return c;
			}
		ENDCG
		}
	}
}