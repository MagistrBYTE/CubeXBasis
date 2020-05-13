//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема управления камерами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCameraMouseEllipseOrbit.cs
*		Камера с возможностью вращения вокруг объекта по эллипсу.
*		Реализация компонента камеры с возможностью вращения вокруг объекта по эллипсу, перемещением, скроллинга и 
*	плавной остановки вращения.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		/// Камера с возможностью вращения вокруг объекта по эллипсу
		/// </summary>
		/// <remarks>
		/// <para>
		/// Реализация компонента камеры с возможностью вращения вокруг объекта по эллипсу, перемещением, скроллинга
		/// и плавной остановки вращения
		/// </para>
		/// <para>
		/// http://unity3d.ru/distribution/viewtopic.php?f=12&t=7816
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Camera/MouseEllipseOrbit")]
		public class CubeXCameraMouseEllipseOrbit : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[Header("Main setting")]
			[SerializeField]
			[CubeXDisplayName(nameof(Target))]
			internal Transform mTarget;
			[SerializeField]
			[CubeXDisplayName(nameof(ButtonOrbit))]
			internal TMouseButtonInput mButtonOrbit = TMouseButtonInput.Right;

			// Вращение
			[Header("Rotation")]
			[SerializeField]
			[CubeXDisplayName(nameof(RadiusEllipseA))]
			internal Single mRadiusEllipseA = 10;
			[SerializeField]
			[CubeXDisplayName(nameof(RadiusEllipseB))]
			internal Single mRadiusEllipseB = 10;
			[SerializeField]
			[CubeXDisplayName(nameof(RotationSpeedX))]
			internal Single mRotationSpeedX = 250;
			[SerializeField]
			[CubeXDisplayName(nameof(RotationSpeedY))]
			internal Single mRotationSpeedY = 120;
			[SerializeField]
			[CubeXDisplayName(nameof(MinLimitY))]
			internal Single mMinLimitY = -20;
			[SerializeField]
			[CubeXDisplayName(nameof(MaxLimitY))]
			internal Single mMaxLimitY = 80;

			// Паннинг
			[Header("Panning")]
			[SerializeField]
			[CubeXDisplayName(nameof(IsPanning))]
			internal Boolean mIsPanning;
			[SerializeField]
			[CubeXDisplayName(nameof(ButtonPan))]
			internal TMouseButtonInput mButtonPan = TMouseButtonInput.Middle;
			[SerializeField]
			[CubeXDisplayName(nameof(PanningSpeed))]
			internal Single mPanningSpeed = 5;
			[SerializeField]
			[CubeXDisplayName(nameof(PanningLayer))]
			internal LayerMask mPanningLayer;

			// Скроллинг
			[Header("Scrolling")]
			[SerializeField]
			[CubeXDisplayName(nameof(IsScrolling))]
			internal Boolean mIsScrolling = true;
			[SerializeField]
			[CubeXDisplayName(nameof(ScrollSpeed))]
			internal Single mScrollSpeed = 1;
			[SerializeField]
			[CubeXDisplayName(nameof(ScrollMinDistance))]
			internal Single mScrollMinDistance = 1;
			[SerializeField]
			[CubeXDisplayName(nameof(ScrollMaxDistance))]
			internal Single mScrollMaxDistance = 10;

			// Кэшированные данные
			[HideInInspector]
			internal Transform mThisTransform;
			[HideInInspector]
			internal Camera mCurrentCamera;

			// Служебные данные
			private Single mAngleX = 0;
			private Single mAngleY = 0;
			private Single mDistance = 10;
			private Single mAngle;
			private Vector3 mNewTargetPosition;
			private Vector3 mAngles;
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
			/// Кнопка мыши для активации вращения
			/// </summary>
			public TMouseButtonInput ButtonOrbit
			{
				get { return mButtonOrbit; }
				set { mButtonOrbit = value; }
			}

			//
			// ВРАЩЕНИЕ
			//
			/// <summary>
			/// Размер полуоси эллипса
			/// </summary>
			public Single RadiusEllipseA
			{
				get { return mRadiusEllipseA; }
				set { mRadiusEllipseA = value; }
			}

			/// <summary>
			/// Размер полуоси эллипса
			/// </summary>
			public Single RadiusEllipseB
			{
				get { return mRadiusEllipseB; }
				set { mRadiusEllipseB = value; }
			}

			/// <summary>
			/// Скорость вращения по X
			/// </summary>
			public Single RotationSpeedX
			{
				get { return mRotationSpeedX; }
				set { mRotationSpeedX = value; }
			}

			/// <summary>
			/// скорость вращения по Y
			/// </summary>
			public Single RotationSpeedY
			{
				get { return mRotationSpeedY; }
				set { mRotationSpeedY = value; }
			}

			/// <summary>
			/// Минимальный предел просмотра по оси Y
			/// </summary>
			public Single MinLimitY
			{
				get { return mMinLimitY; }
				set { mMinLimitY = value; }
			}

			/// <summary>
			/// Максимальный предел просмотра по оси Y
			/// </summary>
			public Single MaxLimitY
			{
				get { return mMaxLimitY; }
				set { mMaxLimitY = value; }
			}

			//
			// ПЕРЕМЕЩЕНИЕ
			//
			/// <summary>
			/// Статус перемещения
			/// </summary>
			public Boolean IsPanning
			{
				get { return mIsPanning; }
				set { mIsPanning = value; }
			}

			/// <summary>
			/// Кнопка мыши для активации перемещения
			/// </summary>
			public TMouseButtonInput ButtonPan
			{
				get { return mButtonPan; }
				set { mButtonPan = value; }
			}

			/// <summary>
			/// Скорость перемещения
			/// </summary>
			public Single PanningSpeed
			{
				get { return mPanningSpeed; }
				set { mPanningSpeed = value; }
			}

			/// <summary>
			/// Слой, в котором находится плоскость перемещения
			/// </summary>
			public Int32 PanningLayer
			{
				get { return mPanningLayer; }
				set { mPanningLayer = value; }
			}

			//
			// СКРОЛЛИНГ
			//
			/// <summary>
			/// Скроллинг
			/// </summary>
			public Boolean IsScrolling
			{
				get { return mIsScrolling; }
				set { mIsScrolling = value; }
			}

			/// <summary>
			/// Скорость скроллинга
			/// </summary>
			public Single ScrollSpeed
			{
				get { return mScrollSpeed; }
				set { mScrollSpeed = value; }
			}

			/// <summary>
			/// Минимальная дистанция скроллинга
			/// </summary>
			public Single ScrollMinDistance
			{
				get { return mScrollMinDistance; }
				set { mScrollMinDistance = value; }
			}

			/// <summary>
			/// Максимальная дистанция скроллинга
			/// </summary>
			public Single ScrollMaxDistance
			{
				get { return mScrollMaxDistance; }
				set { mScrollMaxDistance = value; }
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

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				mThisTransform = this.transform;
				mCurrentCamera = this.GetComponent<Camera>();
				mAngles = mThisTransform.eulerAngles;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				mNewTargetPosition = mTarget.position;

				mAngleX = mAngles.y;
				mAngleY = mAngles.x;

				// Make the rigid body not change rotation
				Orbit();
				Scroll();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр (после Update)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void LateUpdate()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				ManagerToDesktop();

#else
				ManagerToMobile();
#endif

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
				// вращение
				if (XInputDispatcher.IsMouseButton(mButtonOrbit))
				{
					Orbit();
				}

				if (mIsPanning)
				{
					Panning();
				}
				if (mIsScrolling)
				{
					Scroll();
				}

				transform.localPosition = (transform.localRotation) * new Vector3(0, 0, -mDistance);
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

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращения камеры
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void Orbit()
			{
				if (mTarget)
				{
					mAngleX += Input.GetAxis(XCameraInput.AxisHorizontalName) * mRotationSpeedX * 0.02f;
					mAngleY -= Input.GetAxis(XCameraInput.AxisVerticalName) * mRotationSpeedY * 0.02f;

					Single y = XMathAngle.Clamp(mAngleY, mMinLimitY, mMaxLimitY);
					Quaternion rotation = Quaternion.Euler(y, mAngleX, 0);
					transform.rotation = rotation;

					Vector3 target_dir = transform.position - mTarget.position;
					Single angle = Vector3.Angle(target_dir, mTarget.forward);
					Single numerator = Mathf.Pow(mRadiusEllipseA, 2) * Mathf.Pow(mRadiusEllipseB, 2);

					Single denominator = Mathf.Pow(mRadiusEllipseA, 2) * Mathf.Pow(Mathf.Sin(angle * Mathf.Deg2Rad), 2) +
					Mathf.Pow(mRadiusEllipseB, 2) * Mathf.Pow(Mathf.Cos(angle * Mathf.Deg2Rad), 2);

					mDistance = Mathf.Sqrt(numerator / denominator);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение камеры
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void Panning()
			{
				// передвижение мыши
				if (XInputDispatcher.IsMouseButton(mButtonPan))
				{
					Snap();
				}

				mTarget.position = Vector3.Lerp(mTarget.position, mNewTargetPosition, Time.deltaTime * mPanningSpeed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Привязка камеры к новой позиции
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void Snap()
			{
				Ray ray = mCurrentCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit = new RaycastHit();
				if (Physics.Raycast(ray, out hit, 4000, ~mPanningLayer))
				{
					mNewTargetPosition = hit.point;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Скроллинг
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void Scroll()
			{
				// приблизить
				if ((Input.GetAxis(XCameraInput.AxisOrbitName)) > 0)
				{
					if (mDistance >= mScrollMinDistance)
					{
						Single dist1 = mDistance;
						mDistance += -(Input.GetAxis(XCameraInput.AxisOrbitName)) * mScrollSpeed;
						Single k = dist1 / mDistance;
						mRadiusEllipseA = mRadiusEllipseA / k;
						mRadiusEllipseB = mRadiusEllipseB / k;
					}
				}
				//отдалить
				if ((Input.GetAxis(XCameraInput.AxisOrbitName)) < 0)
				{
					if (mDistance <= mScrollMaxDistance)
					{
						Single dist1 = mDistance;
						mDistance += -(Input.GetAxis(XCameraInput.AxisOrbitName)) * mScrollSpeed;
						Single k = dist1 / mDistance;
						mRadiusEllipseA = mRadiusEllipseA / k;
						mRadiusEllipseB = mRadiusEllipseB / k;
					}
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================