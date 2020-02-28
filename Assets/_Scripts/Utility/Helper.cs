using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public enum JoystickKeys { None, Up, Down, Left, Right, A, B, X, Y, RB, LB, RT, LT, Select, Start }

/// <summary>
/// Class where various helpful functions can be found.
/// </summary>
public static class Helper {
	#region Input
	private static bool hAxisDown = false;
	private static bool vAxisDown = false;

	/// <summary>
	/// Check if the joystick axis was just pressed
	/// </summary>
	/// <param name="axis">The axist to test</param>
	/// <returns>The value of the axis</returns>
	public static float GetAxisDown(string axis) {
		if (axis == "Horizontal") {
			if (Input.GetAxisRaw(axis.ToString()) != 0) {
				if (!hAxisDown) {
					hAxisDown = true;
					return Input.GetAxisRaw(axis);
				}

				return 0;
			}

			hAxisDown = false;
			return 0;
		}

		if (axis == "Vertical") {
			if (Input.GetAxisRaw(axis.ToString()) != 0) {
				if (!vAxisDown) {
					vAxisDown = true;
					return Input.GetAxisRaw(axis);
				}

				return 0;
			}

			vAxisDown = false;
			return 0;
		}

		return 0;
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
	public static Vector3 BezierCurve(Vector3 startPoint, Vector3 endPoint, Vector3 curvePoint, float t) {
		return Mathf.Pow(1 - t, 2) * startPoint + 2 * t * (1 - t) * curvePoint + Mathf.Pow(t, 2) * endPoint;
	}

	/// <summary>
	/// Rotates a 2D vector
	/// </summary>
	/// <param name="vector">The vector to rotate</param>
	/// <param name="degrees">The amount of degrees to rotate</param>
	/// <returns>The resulting rotated vector</returns>
	public static Vector2 RotateVector2D(Vector2 vector, float degrees) {
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
	public static Vector2 GetCameraSizeInUnits(Camera cam) {
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
	public static bool IsVisibleFrom(Renderer renderer, Camera camera) { //The "this" keyword in the first paramenter means that the first parameter isn't actually a paramaneter, but instead the class that we're creating the extension for. Note that we DON'T have to pass this parameter when calling the methods (the second parameter and so on works like normal paramenters)
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
	#endregion

	#region LayerMask
	/// <summary>
	/// Checks if a GameObject is in a LayerMask
	/// </summary>
	/// <param name="mask">LayerMask with all the layers to test against</param>
	/// <param name="gameObject">GameObject to test</param>
	/// <returns>True if the object's layer in the LayerMask</returns>
	public static bool LayerMaskContains(LayerMask mask, GameObject gameObject) {
		return LayerMaskContains(mask, gameObject.layer);
	}

	/// <summary>
	/// Checks if a Layer is in a LayerMask
	/// </summary>
	/// <param name="mask">LayerMask with all the layers to test against</param>
	/// <param name="layer">Layer to test</param>
	/// <returns>True if the layer in the LayerMask</returns>
	public static bool LayerMaskContains(LayerMask mask, int layer) {
		return 0 != (mask.value & 1 << layer);
	}
	#endregion

	#region Random
	/// <summary>
	/// Returns a random float number
	/// </summary>
	/// <returns>A vector2 with the width and height of the camera view</returns>
	private static float RandomNormalDistribution() {
		float v1, v2, s;
		do {
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
	public static float RandomNormalDistribution(float mean, float standard_deviation) {
		return mean + RandomNormalDistribution() * standard_deviation;
	}

	/// <summary>
	/// Uses a Gaussian/Normal distribution to generate a random value
	/// </summary>
	/// <param name="mean">Defines the center of the curve, or the value expected to accur most frequently.</param>
	/// <param name="standard_deviation">Defines how "wide" the curve is, or how big is the offset of the generated values.</param>
	/// <returns>Returns -1 if some parameter is wrong.</returns>
	public static float RandomNormalDistribution(float mean, float standard_deviation, float min, float max) {
		if (min > max || min == max) { //Safety measures
			return -1;
		}

		float x;
		do {
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
	public static string ToCapitalLetter(string text) {
		text = text.ToLower();
		return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(text);
	}
	#endregion

	#region Math
	public static float Remap(float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
	#endregion
}
