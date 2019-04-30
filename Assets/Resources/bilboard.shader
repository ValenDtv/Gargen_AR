// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/billboard"
{
v2f vert(appdata_base v)
        {
            v2f o;
            float4 ori=mul(UNITY_MATRIX_MV,float4(0,0,0,1));
            float4 vt=v.vertex;
            vt.y=vt.z;
            vt.z=0;
            vt.xyz+=ori.xyz;//result is vt.z==ori.z ,so the distance to camera keeped ,and screen size keeped
            o.pos=mul(UNITY_MATRIX_P,vt);
 
            o.texc=v.texcoord;
            return o;
        }
		}
     

 