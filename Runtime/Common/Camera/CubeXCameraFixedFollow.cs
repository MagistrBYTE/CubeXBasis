//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема управления камерами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCameraFixedFollow.cs
*		Камера с фиксированной позиций для слежения за объектом.
*		Реализация компонента камеры с фиксированной позиций для слежения за объектом с возможностью изменять
*	расстояние до объекта и режимом орбиты.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
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
		/// Камера с фиксированной позиций для слежения за объектом
		/// </summary>
		/// <remarks>
		/// Реализация компонента камеры с фиксированной позиций для слежения за объектом с возможностью изменять
		/// расстояние до объекта и режимом орбиты.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Camera/FixedFollow-Camera")]
		public class CubeXCameraFixedFollow : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXDisplayName(nameof(Target))]
			internal Transform mTarget;
			[SerializeField]
			[CubeXDisplayName(nameof(IsZoom))]
			internal Boolean mIsZoom = true;
			[SerializeField]
			[CubeXDisplayName(nameof(Distance))]
			internal Vector3 mDistance;
			[SerializeField]
			[CubeXDisplayName(nameof(SpeedXOrbit))]
			internal Single mSpeedXOrbit = 50;
			[SerializeField]
			[CubeXDisplayName(nameof(SpeedYOrbit))]
			internal Single mSpeedYOrbit = 50;
			[NonSerialized]
			internal Boolean mIsOrbit = false;
			[NonSerialized]
			internal Single mCurrentX;
			[NonSerialized]
			internal Single mCurrentY;
			[NonSerialized]
			internal Single mOrbitDistance;
			[NonSerialized]
			internal Boolean mIsControl = true;

			// Кэшированные данные
			[HideInInspector]
			internal Transform mThisTransform;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Целевой объект
			/// </summary>
			public Transform Target
			{
				get { return mTarget; }
				set { mTarget = value; }
			}

			/// <summary>
			/// Возможность изменять дистанцию(наезд) до целевого объекта
			/// </summary>
			public Boolean IsZoom
			{
				get { return mIsZoom; }
				set { mIsZoom = value; }
			}

			/// <summary>
			/// Расстояние до целевого объекта в плоскости XZ
			/// </summary>
			public Vector3 Distance
			{
				get { return mDistance; }
				set { mDistance = value; }
			}

			/// <summary>
			/// Скорость вращения в плоскости X
			/// </summary>
			public Single SpeedXOrbit
			{
				get { return mSpeedXOrbit; }
				set
				{
					mSpeedXOrbit = value;
				}
			}

			/// <summary>
			/// Скорость вращения в плоскости Y
			/// </summary>
			public Single SpeedYOrbit
			{
				get { return mSpeedYOrbit; }
				set { mSpeedYOrbit = value; }
			}

			/// <summary>
			/// Статус управления камерой
			/// </summary>
			public Boolean IsControl
			{
				get { return mIsControl; }
				set { mIsControl = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				mSpeedXOrbit = 50;
				mSpeedYOrbit = 50;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				mThisTransform = this.transform;
				mDistance = mTarget.position - mThisTransform.position;
				mOrbitDistance = mDistance.magnitude;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				if (mIsControl)
				{
					if (Input.GetMouseButtonDown(1))
					{
						mIsOrbit = true;
						mOrbitDistance = mDistance.magnitude;
					}
					if (Input.GetMouseButtonUp(1))
					{
						mIsOrbit = false;
					}
				}
#else

#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр (после Update)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void LateUpdate()
			{
				if (mIsControl)
				{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

					ManagerToDesktop();

#else
					ManagerToMobile();
#endif
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Управление камерой для стандартного ввода
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ManagerToDesktop()
			{
				// Если камера находится в режиме орбиты
				if (mIsOrbit)
				{
					// Приближение камеры
					Single delta_zoom = Input.GetAxis(XCameraInput.AxisOrbitName);
					if (Mathf.Approximately(delta_zoom, 0) == false && mIsZoom)
					{
						mOrbitDistance += delta_zoom;
					}

					// Получаем текущие положение камеры
					mCurrentX = mThisTransform.eulerAngles.y;
					mCurrentY = mThisTransform.eulerAngles.x;

					Single dx = 0;
					Single dy = 0;
					dx = Input.GetAxis(XCameraInput.AxisHorizontalName);
					dy = Input.GetAxis(XCameraInput.AxisVerticalName);

					mCurrentX += dx * mSpeedXOrbit * Time.deltaTime;
					mCurrentY -= dy * mSpeedYOrbit * Time.deltaTime;

					Quaternion rotation = Quaternion.Euler(mCurrentY, mCurrentX, 0);
					Vector3 position = rotation * new Vector3(0.0f, 0.0f, -mOrbitDistance) + mTarget.position;

					mThisTransform.rotation = rotation;
					mThisTransform.position = position;

					mDistance = mTarget.position - mThisTransform.position;
				}
				else
				{
					// Приближение камеры
					Single delta_zoom = Input.GetAxis(XCameraInput.AxisOrbitName);
					if (Mathf.Approximately(delta_zoom, 0) == false && mIsZoom)
					{
						Vector3 dir = mTarget.position - mThisTransform.position;
						mOrbitDistance = dir.magnitude;
						mThisTransform.position += dir * delta_zoom;
						mDistance = mTarget.position - mThisTransform.position;
					}

					mThisTransform.position = mTarget.position - mDistance;
					mThisTransform.LookAt(mTarget);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Управление камерой для мобильного ввода
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ManagerToMobile()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение камеры из текущей позиции в вычисленную
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void MoveToComputePosition()
			{
				mIsControl = false;

				// Получаем текущие положение камеры
				mCurrentX = mThisTransform.eulerAngles.y;
				mCurrentY = mThisTransform.eulerAngles.x;

				Quaternion rotation = Quaternion.Euler(mCurrentY, mCurrentX, 0);
				Vector3 position = rotation * new Vector3(0.0f, 0.0f, -mOrbitDistance) + mTarget.position;

				this.MoveTo(0.5f, position, TEasingType.CubeInOut, ()=>
				{
					mDistance = mTarget.position - mThisTransform.position;
					mIsControl = true;
				});

				this.RotationTo(0.5f, rotation, TEasingType.CubeInOut);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================