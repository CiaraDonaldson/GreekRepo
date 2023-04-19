using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace darcproducts
{
    public static class Utilities
    {
        /// <summary>
        /// Copies over field data from a Base to a Copy
        /// </summary>
        /// <typeparam name="T">Type of Copy and Base</typeparam>
        /// <param name="Base">Base Type to copy fields from</param>
        /// <param name="Copy">Copy Type to copy field to</param>
        public static void CopyValues<T>(T Base, T Copy)
        {
            Type type = Base.GetType();
            foreach (FieldInfo field in type.GetFields())
                field.SetValue(Copy, field.GetValue(Base));
        }

        /// <summary>
        /// Checks to see if a GameObject is in a particular LayerMask
        /// </summary>
        /// <param name="obj">GameObject to check</param>
        /// <param name="layer">LayerMask to check</param>
        /// <returns>Boolean: True or False</returns>
        public static bool IsInLayerMask(GameObject obj, LayerMask layer) => ((layer.value & (1 << obj.layer)) > 0);

        /// <summary>
        /// A precise linear interpolation
        /// </summary>
        /// <param name="v0">In value 1</param>
        /// <param name="v1">In value 2</param>
        /// <param name="t">Time value t</param>
        /// <returns>Float: a Value between Value 1 and Value 2 based on t</returns>
        public static float PreciseLerp(float v0, float v1, float t) => (1 - t) * v0 + t * v1;

        /// <summary>
        /// A imprecise linear interpolation
        /// </summary>
        /// <param name="v0">In value 1</param>
        /// <param name="v1">In value 2</param>
        /// <param name="t">Time value t</param>
        /// <returns>Float: a Value between Value 1 and Value 2 based on t</returns>
        public static float ImpreciseLerp(float v0, float v1, float t) => v0 + t * (v1 - v0);

        /// <summary>
        /// Integer linear interpolation
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns>Int: an Integer value between Value 1 and Value 2 based on t</returns>
        public static int IntLerp(int a, int b, float t)
        {
            if (t > 0.9999f)
                return b;
            return a + (int)(((float)b - a) * t);
        }

        /// <summary>
        /// Sqaushes a value to a size between 0 and 1
        /// </summary>
        /// <param name="value">Value to squash</param>
        /// <returns>Double: a Value between 0 and 1</returns>
        public static double Sigmoid(double value) => 1 / (1 + Mathf.Exp((float)-value));

        /// <summary>
        /// remaps a value with a min and max to a new min and max
        /// </summary>
        /// <param name="aValue">Value to remap</param>
        /// <param name="aIn1">Value min</param>
        /// <param name="aIn2">Value max</param>
        /// <param name="aOut1">new min Value</param>
        /// <param name="aOut2">new max Value</param>
        /// <returns>Float: a new Float Value between new min and max based on old min and max</returns>
        public static float Remap(float aValue, float aIn1, float aIn2, float aOut1, float aOut2)
        {
            var t = (aValue - aIn1) / (aIn2 - aIn1);
            if (t > 1f)
                return aOut2;
            if (t < 0f)
                return aOut1;
            return aOut1 + (aOut2 - aOut1) * t;
        }

        /// <summary>
        /// remaps a value with a min and max to a new min and max
        /// </summary>
        /// <param name="aValue">Value to remap</param>
        /// <param name="aIn1">Value min</param>
        /// <param name="aIn2">Value max</param>
        /// <param name="aOut1">new min Value</param>
        /// <param name="aOut2">new max Value</param>
        /// <returns>Int: a new Integer Value between new min and max based on old min and max</returns>
        public static int Remap(int aValue, int aIn1, int aIn2, int aOut1, int aOut2)
        {
            var t = (aValue - (float)aIn1) / ((float)aIn2 - aIn1);
            if (t > 0.9999f)
                return aOut2;
            if (t < 0f)
                return aOut1;
            return (int)(aOut1 + ((float)aOut2 - aOut1) * t);
        }

        /// <summary>
        /// find's and returns a List of a particular Type
        /// </summary>
        /// <typeparam name="T">Type looking for</typeparam>
        /// <returns>List: a List of objects of that Type</returns>
        public static List<T> Find<T>()
        {
            var l = new List<T>();
            var t = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            foreach (T i in t)
                l.Add(i);
            return l;
        }

        /// <summary>
        ///  find's and returns a HashSet of a particular Type
        /// </summary>
        /// <typeparam name="T">Type looking for</typeparam>
        /// <returns>HashSet: a HashSet of that Type</returns>
        public static HashSet<T> FindAsHashSet<T>()
        {
            var l = new HashSet<T>();
            var t = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>().OfType<T>();
            foreach (T i in t)
                l.Add(i);
            return l;
        }

        /// <summary>
        /// draws a circle using a LineRenderer
        /// </summary>
        /// <param name="line">LineRender to create circle with</param>
        /// <param name="radius">the radius of the circle</param>
        /// <param name="lineWidth">the width of the line</param>
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

        /// <summary>
        /// draws an elipse using a LineRenderer
        /// </summary>
        /// <param name="line">LineRenderer to draw elipse with</param>
        /// <param name="origin">the center of the elipse</param>
        /// <param name="minorAxisWidth">the shortest width of the elipse</param>
        /// <param name="majorAxisWidth">the longest width of the elipse</param>
        /// <param name="lineWidth">the width of the line</param>
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
