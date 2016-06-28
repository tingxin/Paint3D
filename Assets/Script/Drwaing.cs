using UnityEngine;
using System;
using System.Collections;

namespace DrawingTool
{
    public class Drwaing
    {
        public static void DrawLine(Vector3 from, Vector3 to, float brushWidth, Color brushColor, float hardness, Texture2D texture)
        {
            float width = brushWidth * 2;
            float extent = brushWidth;

            int stX = GetInt(Mathf.Clamp(Mathf.Min(from.x, to.x) - extent, 0, texture.width));
            int stY = GetInt(Mathf.Clamp(Mathf.Min(from.y, to.y) - extent, 0, texture.height));

            int endX = GetInt(Mathf.Clamp(Mathf.Max(from.x, to.x) + extent, 0, texture.width));
            int endY = GetInt(Mathf.Clamp(Mathf.Max(from.y, to.y) + extent, 0, texture.height));

            int lengthX = (endX - stX);
            int lengthY = (endY - stY);

            float sqrWidth = brushWidth * brushWidth;
            float sqrWidthNext = (brushWidth + 1) * (brushWidth + 1);

            Color[] pixels = texture.GetPixels(GetInt(stX), GetInt(stY), GetInt(lengthX), GetInt(lengthY), 0);

            Vector3 start = new Vector3(stX, stY, 0);

            Vector3 half = new Vector3(0.5f, 0.5f, 0);

            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    Vector3 positon = new Vector3(x, y, 0) + start;
                    Vector3 center = half + positon;

                    float distance = (center - NearestPontStrict(from, to, center)).sqrMagnitude;

                    if (distance > sqrWidthNext) continue;

                    distance = GaussFalloff(Mathf.Sqrt(distance), brushWidth) * hardness;
                    Color c;
                    int check = GetInt(y * lengthX + x);
                    if (distance > 0)
                    {

                        c = Color.Lerp(pixels[check], brushColor, distance);
                    }
                    else
                    {
                        c = pixels[check];
                    }

                    pixels[check] = c;
                }
            }
            texture.SetPixels((int)start.x, (int)start.y, (int)lengthX, (int)lengthY, pixels, 0);
            texture.Apply();
        }

        static Vector3 NearestPontStrict(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
        {
            Vector3 fullDirection = lineEnd - lineStart;
            Vector3 lineDirection = fullDirection.normalized;
            float closedPoint = Vector3.Dot((point - lineStart), lineDirection) / Vector3.Dot(lineDirection, lineDirection);

            Vector3 nest = lineStart + (Mathf.Clamp(closedPoint, 0.0f, Vector3.Magnitude(fullDirection)) * lineDirection);
            return nest;
        }

        static float GaussFalloff(float distance, float inRadius)
        {
            return Mathf.Clamp01(Mathf.Pow(360.0f, -Mathf.Pow(distance / inRadius, 2.5f) - 0.01f));
        }

        static int GetInt(float number)
        {
            return (int)Mathf.Round(number);
        }
    }
}

