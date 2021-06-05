//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема управления камерами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCameraTrackball.cs
*		Камера с возможностью вращения вокруг объекта.
*		Реализация компонента камеры с возможностью вращения вокруг объекта.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonCamera
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Камера с возможностью вращения вокруг объекта
		/// </summary>
		/// <remarks>
		/// Реализация компонента камеры с возможностью вращения вокруг объекта
		/// http://wiki.unity3d.com/index.php/TrackballCamera
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Camera/Camera-Trackball")]
		public class CubeXCameraTrackball : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			[SerializeField]
			[CubeXDisplayName(nameof(Target))]
			internal Transform mTarget;
			[SerializeField]
			[CubeXDisplayName(nameof(Distance))]
			internal Single mDistance = 15f;
			[SerializeField]
			[CubeXDisplayName(nameof(VirtualTrackballDistance))]
			internal Single mVirtualTrackballDistance = 0.25f;
			[NonSerialized]
			private Vector3? mLastMousePosition;
			#endregion

			#region ======================================= СВОЙСТВА ===================================================
			/// <summary>
			/// Целевой объект
			/// </summary>
			public Transform Target
			{
				get { return (mTarget); }
				set { mTarget = value; }
			}

			/// <summary>
			/// Расстояние для объекта
			/// </summary>
			public Single Distance
			{
				get { return (mDistance); }
				set { mDistance = value; }
			}

			/// <summary>
			/// Виртуальное расстояние до объекта
			/// </summary>
			public Single VirtualTrackballDistance
			{
				get { return (mVirtualTrackballDistance); }
				set { mVirtualTrackballDistance = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				var start_pos = (this.transform.position - mTarget.transform.position).normalized * mDistance;
				var position = start_pos + mTarget.transform.position;
				transform.position = position;
				transform.LookAt(mTarget.transform.position);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр (после Update)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void LateUpdate()
			{
				var mouse_pos = Input.mousePosition;

				var mouse_btn = Input.GetMouseButton(0);
				if (mouse_btn)
				{
					if (mLastMousePosition.HasValue)
					{
						// We are moving from here.
						var lastPos = this.transform.position;
						var targetPos = mTarget.transform.position;

						// we have traced out this distance on a sphere from lastPos
						/*
						var rotation = TrackBall(
												lastMousePosition.Value.x,
												lastMousePosition.Value.y,
												mousePos.x,
												mousePos.y );
						*/
						var rotation = FigureOutAxisAngleRotation(mLastMousePosition.Value, mouse_pos);

						var vecPos = (targetPos - lastPos).normalized * -mDistance;

						this.transform.position = rotation * vecPos + targetPos;
						this.transform.LookAt(targetPos);

						mLastMousePosition = mouse_pos;
					}
					else
					{
						mLastMousePosition = mouse_pos;
					}
				}
				else
				{
					mLastMousePosition = null;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение кватерниона вращения по разнице позиции курсора
			/// </summary>
			/// <param name="last_mouse_pos">Прошлая позиция курсора</param>
			/// <param name="current_mouse_pos">Текущая позиция курсора</param>
			/// <returns>Кватернион вращения </returns>
			//---------------------------------------------------------------------------------------------------------
			private Quaternion FigureOutAxisAngleRotation(Vector3 last_mouse_pos, Vector3 current_mouse_pos)
			{
				if (last_mouse_pos.x == current_mouse_pos.x && last_mouse_pos.y == current_mouse_pos.y)
				{
					return Quaternion.identity;
				}

				var near = new Vector3(0, 0, Camera.main.nearClipPlane);
				Vector3 p1 = Camera.main.ScreenToWorldPoint(last_mouse_pos + near);
				Vector3 p2 = Camera.main.ScreenToWorldPoint(current_mouse_pos + near);

				//WriteLine("## {0} {1}", p1,p2);
				var axisOfRotation = Vector3.Cross(p2, p1);
				var twist = (p2 - p1).magnitude / (2.0f * mVirtualTrackballDistance);

				if (twist > 1.0f)
				{
					twist = 1.0f;
				}

				if (twist < -1.0f)
				{
					twist = -1.0f;
				}

				var phi = (2.0f * Mathf.Asin(twist)) * 180 / Mathf.PI;

				return Quaternion.AngleAxis(phi, axisOfRotation);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение кватерниона вращения
			/// </summary>
			/// <param name="p1x">Координата первой позиции по X</param>
			/// <param name="p1y">Координата первой позиции по Y</param>
			/// <param name="p2x">Координата второй позиции по X</param>
			/// <param name="p2y">Координата второй позиции по Y</param>
			/// <param name="radius">Радиус вращения</param>
			/// <returns>Кватернион вращения</returns>
			//---------------------------------------------------------------------------------------------------------
			private Quaternion TrackBall(Single p1x, Single p1y, Single p2x, Single p2y, Single radius)
			{
				// if there has been no drag, then return "no rotation"
				if (p1x == p2x && p1y == p2y)
				{
					return Quaternion.identity;
				}

				var p1 = ProjectToSphere(radius, p1x, p1y);
				var p2 = ProjectToSphere(radius, p2x, p2y);
				var a = Vector3.Cross(p2, p1); // axis of rotation
											   // how much to rotate around above axis
				var d = p1 - p2;
				var t = d.magnitude / (2.0f * radius);

				// clamp values to stop things going out of control.
				if (t > 1.0f)
				{
					t = 1.0f;
				}

				if (t < -1.0f)
				{
					t = -1.0f;
				}

				var phi = 2.0f * Mathf.Asin(t);
				phi = phi * 180 / Mathf.PI; // to degrees

				return Quaternion.AngleAxis(phi, a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Projects an x,y pair onto a sphere of radius distance
			/// OR onto a hyperbolic sheet if we are away from the center of the sphere
			/// </summary>
			/// <param name="distance">Distance</param>
			/// <param name="x">Coordinate X</param>
			/// <param name="y">Coordinate Y</param>
			/// <returns>The point on the sphere</returns>
			//---------------------------------------------------------------------------------------------------------
			private Vector3 ProjectToSphere(Single distance, Single x, Single y)
			{
				Single z;
				Single d = Mathf.Sqrt(x * x + y * y);
				if (d < distance * 0.707f)
				{
					// inside sphere
					z = Mathf.Sqrt(distance * distance - d * d);
				}
				else
				{
					// on hyperbola
					var t = distance / 1.4142f;
					z = t * t / d;
				}

				return new Vector3(x, y, z);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================