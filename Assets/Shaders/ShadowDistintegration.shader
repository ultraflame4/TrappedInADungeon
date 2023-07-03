Shader "Unlit/ShadowDistintegration"
{
   Properties
   {
      // Sprite Texture
      _MainTex ("Texture", 2D) = "white" {}
      _Color ("Shadow Color", Color) = (0,0,0,1)
      // Noise Texture to determine which pixels (that crosses the threshold) are made transparent
      _Noise ("Noise", 2D) = "white" {}
      // Noise Threshold before the pixel is made transparent
      _Threshold ("Distintegration Threshold", Range(0.0, 1.0)) = 0.1
   }
   SubShader
   {
      Tags
      {
         "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane"
      }
      ZWrite Off
      Blend SrcAlpha OneMinusSrcAlpha
      LOD 100

      Pass
      {
         CGPROGRAM
         #pragma vertex vert
         #pragma fragment frag
         // make fog work
         #pragma multi_compile_fog

         #include "UnityCG.cginc"

         struct appdata
         {
            float4 vertex : POSITION;
            float2 uv : TEXCOORD0;
         };

         struct v2f
         {
            float2 uv : TEXCOORD0;
            float4 vertex : SV_POSITION;
         };
         
         sampler2D _MainTex;
         float4 _MainTex_ST;
         fixed4 _Color;
         sampler2D _Noise;
         float _Threshold;

         v2f vert(appdata v)
         {
            v2f o;
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            UNITY_TRANSFER_FOG(o, o.vertex);
            return o;
         }

         fixed4 frag(v2f i) : SV_Target
         {
            // sample the noise & sprite texture
            fixed4 val = tex2D(_Noise, i.uv);
            fixed4 col = tex2D(_MainTex, i.uv).a * _Color;
            UNITY_APPLY_FOG(i.fogCoord, col);
            if (val.r < _Threshold) // If the noise is below the threshold, make the pixel transparent
            {
               return fixed4(1,0,0,0);
            }
            return col;
         }
         ENDCG
      }
   }
}