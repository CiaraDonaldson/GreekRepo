using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace darcproducts
{
    public static class Utilities
    {
        public static bool IsInLayerMask(GameObject obj, LayerMask layer) => ((layer.value & (1 << obj.layer)) > 0);

        public static float PLerp(float v0, float v1, float t) => (1 - t) * v0 + t * v1;

        public static float ILerp(float v0, float v1, float t) => v0 + t * (v1 - v0);

        public static double Sigmoid(double value) => 1 / (1 + Mathf.Exp((float)-value));

        public static int IntLerp(int a, int b, float t)
        {
            if (t > 0.9999f)
                return b;
            return a + (int)(((float)b - (float)a) * t);
        }


        public static float Remap(float aValue, float aIn1, float aIn2, float aOut1, float aOut2)
        {
            var t = (aValue - aIn1) / (aIn2 - aIn1);
            if (t > 1f)
                return aOut2;
            if (t < 0f)
                return aOut1;
            return aOut1 + (aOut2 - aOut1) * t;
        }

        public static int Remap(int aValue, int aIn1, int aIn2, int aOut1, int aOut2)
        {
            var t = ((float)aValue - (float)aIn1) / ((float)aIn2 - (float)aIn1);
            if (t > 0.9999f)
                return aOut2;
            if (t < 0f)
                return aOut1;
            return (int)((float)aOut1 + ((float)aOut2 - (float)aOut1) * t);
        }

        public static List<T> Find<T>()
        {
            var l = new List<T>();
            var t = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            foreach (T i in t)
                l.Add(i);
            return l;
        }

        public static HashSet<T> FindAsHashSet<T>()
        {
            var l = new HashSet<T>();
            var t = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            foreach (T i in t)
                l.Add(i);
            return l;
        }

        public static void DrawCircle(LineRenderer line, float radius, float lineWidth)
        {
            if (line == null) return;
            var segments = 360;
            var points = new Vector3[segments];

            line.useWorldSpace = false;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.positionCount = segments;

            for (int i = 0; i < segments; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            }

            line.loop = true;
            line.SetPositions(points);
        }

        public static void DrawEllipse(LineRenderer line, Vector3 origin, float minorAxisWidth, float majorAxisWidth, float lineWidth)
        {
            if (line == null) return;
            var segments = 360;
            var points = new Vector3[segments];

            line.useWorldSpace = false;
            line.startWidth = lineWidth;
            line.endWidth = lineWidth;
            line.positionCount = segments;

            for (int i = 0; i < segments; i++)
            {
                var rad = Mathf.Deg2Rad * (i * 360f / segments);
                points[i] = origin + new Vector3(Mathf.Cos(rad) * minorAxisWidth, 0, Mathf.Sin(rad) * majorAxisWidth);
            }

            line.loop = true;
            line.SetPositions(points);
        }
    }
}
