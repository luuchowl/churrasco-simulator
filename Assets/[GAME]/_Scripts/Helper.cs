using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using System.Globalization;

using Random = UnityEngine.Random;

public static class Helper {
	#region Utilities
	/// <summary>
	/// Resets all fields of a given object to their default values.
	/// </summary>
	/// <param name="obj">the object to be cleared</param>
	public static void ClearFields(object obj)
	{
		FieldInfo[] fieldInfos = obj.GetType().GetFields();

		for (int i = 0; i < fieldInfos.Length; i++)
		{
			if (fieldInfos[i].FieldType == typeof(bool))
			{
				fieldInfos[i].SetValue(obj, false);
			}

			if (fieldInfos[i].FieldType == typeof(string))
			{
				fieldInfos[i].SetValue(obj, string.Empty);
			}

			if (fieldInfos[i].FieldType == typeof(int))
			{
				fieldInfos[i].SetValue(obj, 0);
			}

			if (fieldInfos[i].FieldType == typeof(Sprite))
			{
				fieldInfos[i].SetValue(obj, null);
			}
		}
	}

	/// <summary>
	/// Finds the closest object from the reference object.
	/// </summary>
	/// <param name="reference">The object used as the "center". All other objects will be compared to this one.</param>
	/// <param name="maxDistance">The max valid distance. Use -1 to not have a limit.</param>
	/// <param name="objsToCompare">The list of valid objects to compare the distances.</param>
	/// <returns>The closest object from the refence. Returns null if no valid objects were found.</returns>
	public static Transform GetClosestObject(Transform reference, float maxDistance = -1, params Transform[] objsToCompare)
	{
		if (objsToCompare.Length == 0) return null;

		float minDistance = maxDistance == -1 ? float.MaxValue : maxDistance;
		int closestID = -1;

		for (int i = 0; i < objsToCompare.Length; i++)
		{
			float dst = Vector3.Distance(reference.position, objsToCompare[i].position);

			if (dst < minDistance)
			{
				closestID = i;
				minDistance = dst;
			}
		}

		return closestID != -1 ? objsToCompare[closestID] : null;
	}
	#endregion

	#region Float
	/// <summary>
	/// Remap a value from one range to another range of values (e.g. 50 in range [0-100] becomes 0.5 in range [0-1])
	/// </summary>
	/// <param name="value">The value to be remaped</param>
	/// <param name="from1">The minimun value from the first range</param>
	/// <param name="to1">The maximum value from the first range</param>
	/// <param name="from2">The minimun value from the second range</param>
	/// <param name="to2">The maximum value from the second range</param>
	/// <returns></returns>
	public static float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public static float ParseInvariantFloat(string s)
	{
		return float.Parse(s, CultureInfo.InvariantCulture.NumberFormat);
	}
	#endregion

	#region Vector
	/// <summary>
	/// Calculates a point in a bezier curve
	/// </summary>
	/// <param name="startPoint">Where the curve starts</param>
	/// <param name="endPoint">Where the curve ends</param>
	/// <param name="curvePoint">The point that defines the curve direction</param>
	/// <param name="t">The percentage of the curve, ranging from 0.0 to 1.0</param>
	/// <returns>The point in 3 dimensional space</returns>
	public static Vector3 BezierCurve(Vector3 startPoint, Vector3 endPoint, Vector3 curvePoint, float t)
	{
		return Mathf.Pow(1 - t, 2) * startPoint + 2 * t * (1 - t) * curvePoint + Mathf.Pow(t, 2) * endPoint;
	}

	/// <summary>
	/// Rotates a 2D vector
	/// </summary>
	/// <param name="vector">The vector to rotate</param>
	/// <param name="degrees">The amount of degrees to rotate</param>
	/// <returns>The resulting rotated vector</returns>
	public static Vector2 RotateVector2D(Vector2 vector, float degrees)
	{
		Vector2 result = new Vector2();
		result.x = vector.x * Mathf.Cos(degrees * Mathf.Deg2Rad) - vector.y * Mathf.Sin(degrees * Mathf.Deg2Rad);
		result.y = vector.x * Mathf.Sin(degrees * Mathf.Deg2Rad) + vector.y * Mathf.Cos(degrees * Mathf.Deg2Rad);

		return result;
	}
	#endregion

	#region Camera
	/// <summary>
	/// Calculates the size of the ortographic camera in units
	/// </summary>
	/// <param name="cam">The camera to get the size from</param>
	/// <returns>A vector2 with the width and height of the camera view</returns>
	public static Vector2 GetCameraSizeInUnits(Camera cam)
	{
		Vector2 camSize = new Vector2();
		camSize.y = 2 * cam.orthographicSize;
		camSize.x = camSize.y * cam.aspect;

		return camSize;
	}

	/// <summary>
	/// Check if object is inside the defined camera frustum.
	/// </summary>
	/// <param name="camera">The camera used to check</param>
	/// <returns>Returns true if is inside the frustum.</returns>
	public static bool IsVisibleFrom(Renderer renderer, Camera camera)
	{ //The "this" keyword in the first paramenter means that the first parameter isn't actually a paramaneter, but instead the class that we're creating the extension for. Note that we DON'T have to pass this parameter when calling the methods (the second parameter and so on works like normal paramenters)
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
	#endregion

	#region Layer
	/// <summary>
	/// Checks if a GameObject is in a LayerMask
	/// </summary>
	/// <param name="mask">LayerMask with all the layers to test against</param>
	/// <param name="gameObject">GameObject to test</param>
	/// <returns>True if the object's layer in the LayerMask</returns>
	public static bool LayerMaskContains(LayerMask mask, GameObject gameObject)
	{
		if (gameObject == null)
			return false;

		return LayerMaskContains(mask, gameObject.layer);
	}

	/// <summary>
	/// Checks if a Layer is in a LayerMask
	/// </summary>
	/// <param name="mask">LayerMask with all the layers to test against</param>
	/// <param name="layer">Layer to test</param>
	/// <returns>True if the layer in the LayerMask</returns>
	public static bool LayerMaskContains(LayerMask mask, int layer)
	{
		return 0 != (mask.value & 1 << layer);
	}

	public static void ChangeLayerRecursively(Transform parent, int newLayer)
	{
		parent.gameObject.layer = newLayer;

		for (int i = 0; i < parent.childCount; i++)
		{
			Transform child = parent.GetChild(i);
			child.gameObject.layer = newLayer;

			if (child.childCount > 0)
			{
				ChangeLayerRecursively(child, newLayer);
			}
		}
	}
	#endregion

	#region Random
	/// <summary>
	/// Returns a random float number
	/// </summary>
	/// <returns>A vector2 with the width and height of the camera view</returns>
	private static float RandomNormalDistribution()
	{
		float v1, v2, s;
		do
		{
			v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
			v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
			s = v1 * v1 + v2 * v2;
		} while (s >= 1.0f || s == 0f);

		s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

		return v1 * s;
	}

	/// <summary>
	/// Uses a Gaussian/Normal distribution to generate a random value
	/// </summary>
	/// <param name="mean">Defines the center of the curve, or the value expected to accur most frequently.</param>
	/// <param name="standard_deviation">Defines how "wide" the curve is, or how big is the offset of the generated values.</param>
	/// <returns>Returns -1 if some parameter is wrong.</returns>
	public static float RandomNormalDistribution(float mean, float standard_deviation)
	{
		return mean + RandomNormalDistribution() * standard_deviation;
	}

	/// <summary>
	/// Uses a Gaussian/Normal distribution to generate a random value
	/// </summary>
	/// <param name="mean">Defines the center of the curve, or the value expected to accur most frequently.</param>
	/// <param name="standard_deviation">Defines how "wide" the curve is, or how big is the offset of the generated values.</param>
	/// <returns>Returns -1 if some parameter is wrong.</returns>
	public static float RandomNormalDistribution(float mean, float standard_deviation, float min, float max)
	{
		if (min > max || min == max)
		{ //Safety measures
			return -1;
		}

		float x;
		do
		{
			x = RandomNormalDistribution(mean, standard_deviation);
		} while (x < min || x > max);
		return x;
	}
	#endregion

	#region String
	/// <summary>
	/// Returns the string with the first letter of each word capitalized.
	/// </summary>
	/// <param name="text">The string to change.</param>
	public static string ToFirstCapitalLetter(string text)
	{
		text = text.ToLower();
		return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
	}

	/// <summary>
	/// Compare 2 strings in a case insensitive way.
	/// </summary>
	/// <param name="s1">The first string</param>
	/// <param name="s2">The second string</param>
	/// <returns>True if they are the same string, false if not</returns>
	public static bool CaseInsensitiveEquals(string s1, string s2)
	{
		return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
	}
	
	/// <summary>
	/// Formats the text so it separates words by camel case conventions.
	/// </summary>
	/// <param name="text">The original text</param>
	/// <returns>The formatted text</returns>
	public static string ToCamelCaseText(string text)
	{
		string finalText = string.Empty;

		for (int i = 0; i < text.Length; i++)
		{
			char currentChar = text[i];

			if (i == 0)
			{
				finalText += currentChar.ToString().ToUpper();
			}
			else if ((char.IsUpper(currentChar) || char.IsDigit(currentChar)) && !char.IsUpper(text[i - 1]) && !char.IsDigit(text[i - 1]))
			{
				finalText += $" {currentChar}";
			}
			else
			{
				finalText += currentChar;
			}
		}

		return finalText;
	}
	#endregion

	#region DateTime
	/// <summary>
	/// Converts an Unix time stamp to DateTime
	/// </summary>
	/// <param name="unixTimeStamp">The Unix time stamp</param>
	public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
	{
		// Unix timestamp is seconds past January 1st 1970 in UTC
		DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
		return dtDateTime;
	}
	#endregion
}
