// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.36 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.36;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:4,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:33668,y:32947,varname:node_3138,prsc:2|emission-7021-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:33036,y:32900,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.8068966,c3:0,c4:1;n:type:ShaderForge.SFN_ObjectPosition,id:945,x:31862,y:32869,varname:node_945,prsc:2;n:type:ShaderForge.SFN_FragmentPosition,id:5239,x:31862,y:32729,varname:node_5239,prsc:2;n:type:ShaderForge.SFN_Distance,id:4821,x:32100,y:32817,varname:node_4821,prsc:2|A-5239-XYZ,B-945-XYZ;n:type:ShaderForge.SFN_Cos,id:6509,x:32662,y:33090,varname:node_6509,prsc:2|IN-3464-OUT;n:type:ShaderForge.SFN_Multiply,id:3464,x:32484,y:33090,varname:node_3464,prsc:2|A-1416-OUT,B-584-OUT;n:type:ShaderForge.SFN_Pi,id:584,x:32308,y:33158,varname:node_584,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:1738,x:32855,y:33090,varname:node_1738,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-6509-OUT;n:type:ShaderForge.SFN_Power,id:7191,x:33063,y:33145,varname:node_7191,prsc:2|VAL-1738-OUT,EXP-6176-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8791,x:32766,y:33308,ptovrint:False,ptlb:Power0,ptin:_Power0,varname:node_8791,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Multiply,id:7021,x:33299,y:33038,varname:node_7021,prsc:2|A-7241-RGB,B-7191-OUT;n:type:ShaderForge.SFN_Clamp01,id:1416,x:32501,y:32889,varname:node_1416,prsc:2|IN-873-OUT;n:type:ShaderForge.SFN_ValueProperty,id:6014,x:32057,y:33067,ptovrint:False,ptlb:Distance1,ptin:_Distance1,varname:node_6014,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:3;n:type:ShaderForge.SFN_ValueProperty,id:5796,x:32766,y:33404,ptovrint:False,ptlb:Power1,ptin:_Power1,varname:node_5796,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:3175,x:32547,y:33404,varname:node_3175,prsc:2;n:type:ShaderForge.SFN_Sin,id:5210,x:32934,y:33514,varname:node_5210,prsc:2|IN-5876-OUT;n:type:ShaderForge.SFN_RemapRange,id:49,x:33129,y:33514,varname:node_49,prsc:2,frmn:-1,frmx:1,tomn:0,tomx:1|IN-5210-OUT;n:type:ShaderForge.SFN_Lerp,id:6176,x:33321,y:33373,varname:node_6176,prsc:2|A-8791-OUT,B-5796-OUT,T-49-OUT;n:type:ShaderForge.SFN_Multiply,id:5876,x:32766,y:33514,varname:node_5876,prsc:2|A-3175-T,B-8640-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8640,x:32547,y:33570,ptovrint:False,ptlb:Frequency,ptin:_Frequency,varname:node_8640,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:2235,x:32057,y:32983,ptovrint:False,ptlb:Distance0,ptin:_Distance0,varname:node_2235,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_RemapRangeAdvanced,id:873,x:32322,y:32889,varname:node_873,prsc:2|IN-4821-OUT,IMIN-2235-OUT,IMAX-6014-OUT,OMIN-4204-OUT,OMAX-5010-OUT;n:type:ShaderForge.SFN_Vector1,id:4204,x:32057,y:33158,varname:node_4204,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:5010,x:32057,y:33231,varname:node_5010,prsc:2,v1:1;proporder:7241-2235-6014-8791-5796-8640;pass:END;sub:END;*/

Shader "Shader Forge/TorchLight" {
    Properties {
        _Color ("Color", Color) = (1,0.8068966,0,1)
        _Distance0 ("Distance0", Float ) = 0
        _Distance1 ("Distance1", Float ) = 3
        _Power0 ("Power0", Float ) = 0
        _Power1 ("Power1", Float ) = 0
        _Frequency ("Frequency", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend DstColor Zero
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float4 _Color;
            uniform float _Power0;
            uniform float _Distance1;
            uniform float _Power1;
            uniform float _Frequency;
            uniform float _Distance0;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float4 objPos = mul ( unity_ObjectToWorld, float4(0,0,0,1) );
////// Lighting:
////// Emissive:
                float node_4821 = distance(i.posWorld.rgb,objPos.rgb);
                float node_4204 = 0.0;
                float4 node_3175 = _Time + _TimeEditor;
                float3 emissive = (_Color.rgb*pow((cos((saturate((node_4204 + ( (node_4821 - _Distance0) * (1.0 - node_4204) ) / (_Distance1 - _Distance0)))*3.141592654))*0.5+0.5),lerp(_Power0,_Power1,(sin((node_3175.g*_Frequency))*0.5+0.5))));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
