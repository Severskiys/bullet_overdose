using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace CodeBase._Tools.Helpers
{
	public static class Helper
	{
		public static Vector3 GetRandomPointOnPlane(this Transform target, float radius)
		{
			var insideUnitCircle = Random.insideUnitCircle;
			var direction = new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y);
			return target.position + direction * radius;
		}
		
		public static string MoneyString(this int moneyValue)
		{
			if (moneyValue < 0) return "0";

			if (moneyValue < 10_000) return moneyValue.ToString();

			var moneyString = moneyValue.ToString();
			var thousandCount = (moneyString.Length - 1) / 3;
			switch (thousandCount)
			{
				case 1:
					moneyString = moneyString.Substring(0, moneyString.Length - 3) + "K";
					break;
				case 2:
					moneyString = moneyString.Substring(0, moneyString.Length - 6) + "M";
					break;
				case 3:
					moneyString = moneyString.Substring(0, moneyString.Length - 9) + "B";
					break;
				default:
					moneyString = moneyString.Substring(0, moneyString.Length - 9) + "B";
					break;
			}

			return moneyString;
		}

		private static UnityEngine.Camera _camera;
		public static UnityEngine.Camera Camera
		{
			get
			{
				if (_camera == default) _camera = UnityEngine.Camera.main;

				return _camera;
			}
		}
		
		public static IEnumerator WaitScaledTime(float duration, Action finished)
		{
			yield return new WaitForSeconds(duration);
			finished?.Invoke();
		}

		public static IEnumerator WaitRealTime(float duration, Action finished = null)
		{
			yield return new WaitForSecondsRealtime(duration);
			finished?.Invoke();
		}

		public static IEnumerator WaitFrame(Action finished)
		{
			yield return default;
			finished?.Invoke();
		}

		private static PointerEventData _eventDataCurrentPos;
		private static List<RaycastResult> _result;

		public static bool IsPointerOverUI()
		{
			_eventDataCurrentPos = new PointerEventData(EventSystem.current)
			{
				position = Input.mousePosition
			};
			_result = new List<RaycastResult>();
			EventSystem.current.RaycastAll(_eventDataCurrentPos, _result);
			return _result.Count > 0;
		}

		public static void SetSize(this RectTransform trans, Vector2 newSize)
		{
			Vector2 oldSize = trans.rect.size;
			Vector2 deltaSize = newSize - oldSize;
			Vector2 pivot = trans.pivot;
			trans.offsetMin -= new Vector2(deltaSize.x * pivot.x, deltaSize.y * pivot.y);
			trans.offsetMax += new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - pivot.y));
		}

		public static IEnumerable<T> GetEnumValues<T>() => Enum.GetValues(typeof(T)).Cast<T>();
		
		public static Vector3 CalculateWorldPosition(this Vector3 position)
		{
			//if the point is behind the camera then project it onto the camera plane
			Vector3 camNormal = Camera.transform.forward;
			Vector3 vectorFromCam = position - Camera.transform.position;
			float camNormDot = Vector3.Dot(camNormal, vectorFromCam.normalized);
			if (camNormDot <= 0f)
			{
				//we are beind the camera, project the position on the camera plane
				float camDot = Vector3.Dot(camNormal, vectorFromCam);
				Vector3
					proj = (camNormal * (camDot * 1.01f)); //small epsilon to keep the position infront of the camera
				position = Camera.transform.position + (vectorFromCam - proj);
			}

			return position;
		}
		
		public static float GeometricProgression(float startValue, float coeff, int degree) =>
			startValue * (Mathf.Pow(coeff, degree));

		public static float NiceGeometricProgression(float startValue, float coeff, int degree, float startValueShift) =>
			startValue * (Mathf.Pow(coeff, degree)) + startValueShift - startValue;
		
		public static void DrawBound(Bounds bounds, float timer, Color color)
		{
#if UNITY_EDITOR
			Debug.DrawLine(bounds.min, bounds.max, color, timer);
#endif
		}

		public static void DrawDebug(Vector3 worldPos, float radius, float timer, Color color)
		{
#if UNITY_EDITOR
			Debug.DrawRay(worldPos, radius * Vector3.up, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.down, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.left, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.right, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.forward, color, timer);
			Debug.DrawRay(worldPos, radius * Vector3.back, color, timer);
#endif
		}

		public static T GetRandomWithWeights<T>(List<T> rarityChances) where T : IHasWeight
		{
			List<T> availableChances = new List<T>(rarityChances);
			availableChances.Randomize();
			float totalChance = availableChances.Sum(e => e.Weight);
			float randomValue = Random.Range(0, totalChance);
			int currentIndex = 0;
			while (randomValue >= 0)
			{
				randomValue -= availableChances[currentIndex].Weight;
				currentIndex++;
			}

			currentIndex = Mathf.Clamp(currentIndex - 1, 0, availableChances.Count - 1);
			return availableChances[currentIndex];
		}
		public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
		{
			var rnd = new System.Random();
			return source.OrderBy(item => rnd.Next());
		}
	}
}