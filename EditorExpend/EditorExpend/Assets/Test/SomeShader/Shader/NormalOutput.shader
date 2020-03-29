// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/NormalOutput"
{
    Properties
    {
        
    }
    SubShader
    {
        Pass
        {
            Tags { "RenderType"="Opaque" }
            LOD 200

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            struct input{
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
			};

            struct output{
                float4 pos : SV_POSITION;
				float3 color : COLOR0;
			};

            output vert(input i){
                output o;
                o.pos=UnityObjectToClipPos(i.vertex);
                o.color=i.normal;
                return o;
            }

            fixed4 frag(output o) : SV_TARGET{
                return fixed4(o.color,1);
            }

		    ENDCG
        }

    }
    FallBack "Diffuse"
}
