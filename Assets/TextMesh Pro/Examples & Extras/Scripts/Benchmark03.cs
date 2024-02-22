﻿using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using UnityEngine.TextCore.LowLevel;


namespace TMPro.Examples
{

    public class Benchmark03 : MonoBehaviour
    {
        public enum BenchmarkType { TmpsdfMobile = 0, TmpsdfMobileSsd = 1, Tmpsdf = 2, TMPBitmapMobile = 3, TextmeshBitmap = 4 }

        [FormerlySerializedAs("NumberOfSamples")] public int numberOfSamples = 100;
        [FormerlySerializedAs("Benchmark")] public BenchmarkType benchmark;

        [FormerlySerializedAs("SourceFont")] public Font sourceFont;


        void Awake()
        {

        }


        void Start()
        {
            TMP_FontAsset fontAsset = null;

            // Create Dynamic Font Asset for the given font file.
            switch (benchmark)
            {
                case BenchmarkType.TmpsdfMobile:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
                    break;
                case BenchmarkType.TmpsdfMobileSsd:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
                    fontAsset.material.shader = Shader.Find("TextMeshPro/Mobile/Distance Field SSD");
                    break;
                case BenchmarkType.Tmpsdf:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont, 90, 9, GlyphRenderMode.SDFAA, 256, 256, AtlasPopulationMode.Dynamic);
                    fontAsset.material.shader = Shader.Find("TextMeshPro/Distance Field");
                    break;
                case BenchmarkType.TMPBitmapMobile:
                    fontAsset = TMP_FontAsset.CreateFontAsset(sourceFont, 90, 9, GlyphRenderMode.SMOOTH, 256, 256, AtlasPopulationMode.Dynamic);
                    break;
            }

            for (int i = 0; i < numberOfSamples; i++)
            {
                switch (benchmark)
                {
                    case BenchmarkType.TmpsdfMobile:
                    case BenchmarkType.TmpsdfMobileSsd:
                    case BenchmarkType.Tmpsdf:
                    case BenchmarkType.TMPBitmapMobile:
                        {
                            GameObject go = new GameObject();
                            go.transform.position = new Vector3(0, 1.2f, 0);

                            TextMeshPro textComponent = go.AddComponent<TextMeshPro>();
                            textComponent.font = fontAsset;
                            textComponent.fontSize = 128;
                            textComponent.text = "@";
                            textComponent.alignment = TextAlignmentOptions.Center;
                            textComponent.color = new Color32(255, 255, 0, 255);

                            if (benchmark == BenchmarkType.TMPBitmapMobile)
                                textComponent.fontSize = 132;

                        }
                        break;
                    case BenchmarkType.TextmeshBitmap:
                        {
                            GameObject go = new GameObject();
                            go.transform.position = new Vector3(0, 1.2f, 0);

                            TextMesh textMesh = go.AddComponent<TextMesh>();
                            textMesh.GetComponent<Renderer>().sharedMaterial = sourceFont.material;
                            textMesh.font = sourceFont;
                            textMesh.anchor = TextAnchor.MiddleCenter;
                            textMesh.fontSize = 130;

                            textMesh.color = new Color32(255, 255, 0, 255);
                            textMesh.text = "@";
                        }
                        break;
                }
            }
        }

    }
}
