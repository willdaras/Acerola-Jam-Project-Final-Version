// --------------------
// Hey if anyone looks at this, this was a shadergraph shader I modified because im still not very confident in writing my own shaders
// thats why the names are so weird (my modifications were the for loops in `SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN))`
// --------------------

Shader "Custom/CRT Blur Effect"
    {
        Properties
        {
            _BlurStrength("BlurStrength", Range(1, 100)) = 2
            [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
            [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
            [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
        }
        SubShader
        {
            Tags
            {
                "RenderPipeline"="UniversalPipeline"
                // RenderType: <None>
                // Queue: <None>
                // DisableBatching: <None>
                "ShaderGraphShader"="true"
                "ShaderGraphTargetId"="UniversalFullscreenSubTarget"
            }
            Pass
            {
                Name "DrawProcedural"
            
            // Render State
            Cull Off
                Blend Off
                ZTest Off
                ZWrite Off
            
            // Debug
            // <None>
            
            // --------------------------------------------------
            // Pass
            
            HLSLPROGRAM
            
            // Pragmas
            #pragma target 3.0
                #pragma vertex vert
                #pragma fragment frag
            // #pragma enable_d3d11_debug_symbols
            
            /* WARNING: $splice Could not find named fragment 'DotsInstancingOptions' */
            /* WARNING: $splice Could not find named fragment 'HybridV1InjectedBuiltinProperties' */
            
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            #define FULLSCREEN_SHADERGRAPH
            
            // Defines
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_VERTEXID
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            
            // Force depth texture because we need it for almost every nodes
            // TODO: dependency system that triggers this define from position or view direction usage
            #define REQUIRE_DEPTH_TEXTURE
            #define REQUIRE_NORMAL_TEXTURE
            
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_DRAWPROCEDURAL
            
            // custom interpolator pre-include
            /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
            
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            
            // --------------------------------------------------
            // Structs and Packing
            
            // custom interpolators pre packing
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
            
            struct Attributes
                {
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                     uint vertexID : VERTEXID_SEMANTIC;
                };
                struct SurfaceDescriptionInputs
                {
                     float3 WorldSpacePosition;
                     float4 ScreenPosition;
                     float2 NDCPosition;
                     float2 PixelPosition;
                     float4 uv0;
                };
                struct Varyings
                {
                     float4 positionCS : SV_POSITION;
                     float4 texCoord0;
                     float4 texCoord1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                };
                struct VertexDescriptionInputs
                {
                };
                struct PackedVaryings
                {
                     float4 positionCS : SV_POSITION;
                     float4 texCoord0 : INTERP0;
                     float4 texCoord1 : INTERP1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                };
            
            PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    ZERO_INITIALIZE(PackedVaryings, output);
                    output.positionCS = input.positionCS;
                    output.texCoord0.xyzw = input.texCoord0;
                    output.texCoord1.xyzw = input.texCoord1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    return output;
                }
                
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.texCoord0.xyzw;
                    output.texCoord1 = input.texCoord1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    return output;
                }
                
            
            // --------------------------------------------------
            // Graph
            
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
                float _BlurStrength;
                CBUFFER_END
                
                
                // Object and Global properties
                float _FlipY;
            
            // Graph Includes
            // GraphIncludes: <None>
            
            // Graph Functions
            
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Multiply_float_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }
                
                void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                {
                    Out = A + B;
                }
                
                TEXTURE2D_X(_BlitTexture);
                float4 Unity_Universal_SampleBuffer_BlitSource_float(float2 uv)
                {
                    uint2 pixelCoords = uint2(uv * _ScreenSize.xy);
                    return LOAD_TEXTURE2D_X_LOD(_BlitTexture, pixelCoords, 0);
                }
                
                void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A + B;
                }
                
                void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A / B;
                }
            
            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
            
            // Graph Vertex
            // GraphVertex: <None>
            
            // Custom interpolators, pre surface
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
            
            // Graph Pixel
            struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _UV_261c486febf44a83979fce2c9cfcd3c3_Out_0_Vector4 = IN.uv0;
                    float _Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float; // out value
                    Unity_Divide_float(1, _ScreenParams.x, _Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float); // (a, b, out)
                    float4 _Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4;
                    for (int i = 0; i < _BlurStrength; i++)
                    {
                        float _Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float;
                        Unity_Multiply_float_float(_Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float, i, _Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float); // multiply screen position by i
                        float2 _Vector2_97c1993a733149a3ad410e519ae63d61_Out_0_Vector2 = float2(_Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float, 0); // creates Vector2
                        float2 _Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2;
                        Unity_Add_float2((_UV_261c486febf44a83979fce2c9cfcd3c3_Out_0_Vector4.xy), _Vector2_97c1993a733149a3ad410e519ae63d61_Out_0_Vector2, _Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2); // adds to original UV
                        float4 _URPSampleBuffer_54ba6ce09ede4319ac51af5689d90428_Output_2_Vector4 = Unity_Universal_SampleBuffer_BlitSource_float((float4(_Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2, 0.0, 1.0)).xy);
                        Unity_Add_float4(_Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4, _URPSampleBuffer_54ba6ce09ede4319ac51af5689d90428_Output_2_Vector4, _Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4);
                    }
                    for (int i = 0; i < _BlurStrength; i++)
                    {
                        float _Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float;
                        Unity_Multiply_float_float(_Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float, i, _Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float); // multiply screen position by i
                        float2 _Vector2_97c1993a733149a3ad410e519ae63d61_Out_0_Vector2 = float2(_Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float, 0); // creates Vector2
                        float2 _Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2;
                        Unity_Add_float2((_UV_261c486febf44a83979fce2c9cfcd3c3_Out_0_Vector4.xy), -_Vector2_97c1993a733149a3ad410e519ae63d61_Out_0_Vector2, _Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2); // adds to original UV
                        float4 _URPSampleBuffer_54ba6ce09ede4319ac51af5689d90428_Output_2_Vector4 = Unity_Universal_SampleBuffer_BlitSource_float((float4(_Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2, 0.0, 1.0)).xy);
                        Unity_Add_float4(_Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4, _URPSampleBuffer_54ba6ce09ede4319ac51af5689d90428_Output_2_Vector4, _Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4);
                    }
                    float _Property_0ef12379a78343f79d1e161f93a57710_Out_0_Float = _BlurStrength * 2;
                    float4 _Divide_e192fcf541e44cba9803060dc5f9bcb2_Out_2_Vector4;
                    Unity_Divide_float4(_Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4, (_Property_0ef12379a78343f79d1e161f93a57710_Out_0_Float.xxxx), _Divide_e192fcf541e44cba9803060dc5f9bcb2_Out_2_Vector4);
                    surface.BaseColor = (_Divide_e192fcf541e44cba9803060dc5f9bcb2_Out_2_Vector4.xyz);
                    surface.Alpha = 1;
                    return surface;
                }
            
            // --------------------------------------------------
            // Build Graph Inputs
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                    float3 normalWS = SHADERGRAPH_SAMPLE_SCENE_NORMAL(input.texCoord0.xy);
                    float4 tangentWS = float4(0, 1, 0, 0); // We can't access the tangent in screen space
                
                
                
                
                    float3 viewDirWS = normalize(input.texCoord1.xyz);
                    float linearDepth = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(input.texCoord0.xy), _ZBufferParams);
                    float3 cameraForward = -UNITY_MATRIX_V[2].xyz;
                    float camearDistance = linearDepth / dot(viewDirWS, cameraForward);
                    float3 positionWS = viewDirWS * camearDistance + GetCameraPositionWS();
                
                
                    output.WorldSpacePosition = positionWS;
                    output.ScreenPosition = float4(input.texCoord0.xy, 0, 1);
                    output.uv0 = input.texCoord0;
                    output.NDCPosition = input.texCoord0.xy;
                
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                        return output;
                }
                
            
            // --------------------------------------------------
            // Main
            
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenCommon.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenDrawProcedural.hlsl"
            
            ENDHLSL
            }
            Pass
            {
                Name "Blit"
            
            // Render State
            Cull Off
                Blend Off
                ZTest Off
                ZWrite Off
            
            // Debug
            // <None>
            
            // --------------------------------------------------
            // Pass
            
            HLSLPROGRAM
            
            // Pragmas
            #pragma target 3.0
                #pragma vertex vert
                #pragma fragment frag
            // #pragma enable_d3d11_debug_symbols
            
            /* WARNING: $splice Could not find named fragment 'DotsInstancingOptions' */
            /* WARNING: $splice Could not find named fragment 'HybridV1InjectedBuiltinProperties' */
            
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            #define FULLSCREEN_SHADERGRAPH
            
            // Defines
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_VERTEXID
            #define VARYINGS_NEED_POSITION_WS
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            
            // Force depth texture because we need it for almost every nodes
            // TODO: dependency system that triggers this define from position or view direction usage
            #define REQUIRE_DEPTH_TEXTURE
            #define REQUIRE_NORMAL_TEXTURE
            
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_BLIT
            
            // custom interpolator pre-include
            /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
            
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenShaderPass.cs.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/SpaceTransforms.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/Functions.hlsl"
            
            // --------------------------------------------------
            // Structs and Packing
            
            // custom interpolators pre packing
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
            
            struct Attributes
                {
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                     uint vertexID : VERTEXID_SEMANTIC;
                     float3 positionOS : POSITION;
                };
                struct SurfaceDescriptionInputs
                {
                     float3 WorldSpacePosition;
                     float4 ScreenPosition;
                     float2 NDCPosition;
                     float2 PixelPosition;
                     float4 uv0;
                };
                struct Varyings
                {
                     float4 positionCS : SV_POSITION;
                     float4 texCoord0;
                     float4 texCoord1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                };
                struct VertexDescriptionInputs
                {
                };
                struct PackedVaryings
                {
                     float4 positionCS : SV_POSITION;
                     float4 texCoord0 : INTERP0;
                     float4 texCoord1 : INTERP1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                     uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                     uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                     uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                };
            
            PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    ZERO_INITIALIZE(PackedVaryings, output);
                    output.positionCS = input.positionCS;
                    output.texCoord0.xyzw = input.texCoord0;
                    output.texCoord1.xyzw = input.texCoord1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    return output;
                }
                
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.texCoord0 = input.texCoord0.xyzw;
                    output.texCoord1 = input.texCoord1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    return output;
                }
                
            
            // --------------------------------------------------
            // Graph
            
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
                float _BlurStrength;
                CBUFFER_END
                
                
                // Object and Global properties
                float _FlipY;
            
            // Graph Includes
            // GraphIncludes: <None>
            
            // Graph Functions
            
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Multiply_float_float(float A, float B, out float Out)
                {
                    Out = A * B;
                }
                
                void Unity_Add_float2(float2 A, float2 B, out float2 Out)
                {
                    Out = A + B;
                }
                
                TEXTURE2D_X(_BlitTexture);
                float4 Unity_Universal_SampleBuffer_BlitSource_float(float2 uv)
                {
                    uint2 pixelCoords = uint2(uv * _ScreenSize.xy);
                    return LOAD_TEXTURE2D_X_LOD(_BlitTexture, pixelCoords, 0);
                }
                
                void Unity_Add_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A + B;
                }
                
                void Unity_Divide_float4(float4 A, float4 B, out float4 Out)
                {
                    Out = A / B;
                }
            
            // Custom interpolators pre vertex
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
            
            // Graph Vertex
            // GraphVertex: <None>
            
            // Custom interpolators, pre surface
            /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreSurface' */
            
            // Graph Pixel
            struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _UV_261c486febf44a83979fce2c9cfcd3c3_Out_0_Vector4 = IN.uv0;
                    float _Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float;
                    Unity_Divide_float(1, _ScreenParams.x, _Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float);
                    float _Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float;
                    Unity_Multiply_float_float(_Divide_fea60c9b13f44f04ba88e54733e6758b_Out_2_Float, 1, _Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float);
                    float2 _Vector2_97c1993a733149a3ad410e519ae63d61_Out_0_Vector2 = float2(_Multiply_ab86998ce97e49b2ba925fd2168da85d_Out_2_Float, 0);
                    float2 _Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2;
                    Unity_Add_float2((_UV_261c486febf44a83979fce2c9cfcd3c3_Out_0_Vector4.xy), _Vector2_97c1993a733149a3ad410e519ae63d61_Out_0_Vector2, _Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2);
                    float4 _URPSampleBuffer_54ba6ce09ede4319ac51af5689d90428_Output_2_Vector4 = Unity_Universal_SampleBuffer_BlitSource_float((float4(_Add_8a8cef1368af4a639460f4f91b6b6404_Out_2_Vector2, 0.0, 1.0)).xy);
                    float4 _Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4;
                    Unity_Add_float4(float4(1, 2, 0, 1), _URPSampleBuffer_54ba6ce09ede4319ac51af5689d90428_Output_2_Vector4, _Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4);
                    float _Property_0ef12379a78343f79d1e161f93a57710_Out_0_Float = _BlurStrength;
                    float4 _Divide_e192fcf541e44cba9803060dc5f9bcb2_Out_2_Vector4;
                    Unity_Divide_float4(_Add_77566d26f05240f79e9f4a56674134b1_Out_2_Vector4, (_Property_0ef12379a78343f79d1e161f93a57710_Out_0_Float.xxxx), _Divide_e192fcf541e44cba9803060dc5f9bcb2_Out_2_Vector4);
                    surface.BaseColor = (_Divide_e192fcf541e44cba9803060dc5f9bcb2_Out_2_Vector4.xyz);
                    surface.Alpha = 1;
                    return surface;
                }
            
            // --------------------------------------------------
            // Build Graph Inputs
            
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                    float3 normalWS = SHADERGRAPH_SAMPLE_SCENE_NORMAL(input.texCoord0.xy);
                    float4 tangentWS = float4(0, 1, 0, 0); // We can't access the tangent in screen space
                
                
                
                
                    float3 viewDirWS = normalize(input.texCoord1.xyz);
                    float linearDepth = LinearEyeDepth(SHADERGRAPH_SAMPLE_SCENE_DEPTH(input.texCoord0.xy), _ZBufferParams);
                    float3 cameraForward = -UNITY_MATRIX_V[2].xyz;
                    float camearDistance = linearDepth / dot(viewDirWS, cameraForward);
                    float3 positionWS = viewDirWS * camearDistance + GetCameraPositionWS();
                
                
                    output.WorldSpacePosition = positionWS;
                    output.ScreenPosition = float4(input.texCoord0.xy, 0, 1);
                    output.uv0 = input.texCoord0;
                    output.NDCPosition = input.texCoord0.xy;
                
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                        return output;
                }
                
            
            // --------------------------------------------------
            // Main
            
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenCommon.hlsl"
            #include "Packages/com.unity.shadergraph/Editor/Generation/Targets/Fullscreen/Includes/FullscreenBlit.hlsl"
            
            ENDHLSL
            }
        }
        CustomEditor "UnityEditor.Rendering.Fullscreen.ShaderGraph.FullscreenShaderGUI"
        FallBack "Hidden/Shader Graph/FallbackError"
    }