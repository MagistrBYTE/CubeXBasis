//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема управления камерами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCameraFixedFollow.cs
*		Камера с плавным слежением за объектом.
*		Реализация компонента камеры с плавным слежением за объектом с возможностью изменять расстояние до объекта и
*	режимом орбиты.
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
		/// Камера с плавным слежением за объектом
		/// </summary>
		/// <remarks>
		/// Реализация компонента камеры с плавным слежением за объектом с возможностью изменять расстояние до объекта и
		/// режимом орбиты.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Camera/SmoothFollow-Camera")]
		public class CubeXCameraSmoothFollow : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			[Header("Main setting")]
			[SerializeField]
			[CubeXDisplayName(nameof(Target))]
			internal Transform mTarget;
			[SerializeField]
			[CubeXDisplayName(nameof(IsZoom))]
			internal Boolean mIsZoom = true;
			[SerializeField]
			[CubeXDisplayName(nameof(Distance))]
			internal Single mDistance = 4;
			[SerializeField]
			[CubeXDisplayName(nameof(Height))]
			internal Single mHeight = 2;
			[SerializeField]
			[CubeXDisplayName(nameof(RotateDamping))]
			internal Single mRotateDamping = 5;
			[SerializeField]
			[CubeXDisplayName(nameof(HeightDamping))]
			internal Single mHeightDamping = 2;

			// Вращение вокруг объекта
			[Header("Setting orbit")]
			[SerializeField]
			[CubeXDisplayName(nameof(IsSupportOrbit))]
			internal Boolean mIsSupportOrbit = true;
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

			// Дополнительное управление
			[Header("Manager height")]
			[SerializeField]
			[CubeXDisplayName(nameof(KeyHeightUp))]
			internal KeyCode mKeyHeightUp = KeyCode.Keypad8;
			[SerializeField]
			[CubeXDisplayName(nameof(KeyHeightDown))]
			internal KeyCode mKeyHeightDown = KeyCode.Keypad2;

			// Кэшированные данные
			[NonSerialized]
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
			public Single Distance
			{
				get { return mDistance; }
				set { mDistance = value; }
			}

			/// <summary>
			/// Расстояние до целевого объекта в плоскости Y
			/// </summary>
			public Single Height
			{
				get { return mHeight; }
				set { mHeight = value; }
			}

			/// <summary>
			/// Скорость вращения камеры
			/// </summary>
			public Single RotateDamping
			{
				get { return mRotateDamping; }
				set { mRotateDamping = value; }
			}

			/// <summary>
			/// Скорость снижения камеры
			/// </summary>
			public Single HeightDamping
			{
				get { return mHeightDamping; }
				set { mHeightDamping = value; }
			}

			/// <summary>
			/// Возможность вращение камеры вокруг целевого объекта
			/// </summary>
			public Boolean IsSupportOrbit
			{
				get { return mIsSupportOrbit; }
				set { mIsSupportOrbit = value; }
			}

			/// <summary>
			/// Скорость вращения в плоскости X
			/// </summary>
			public Single SpeedXOrbit
			{
				get { return mSpeedXOrbit; }
				set { mSpeedXOrbit = value; }
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
			/// Клавиша для увеличения высоты
			/// </summary>
			public KeyCode KeyHeightUp
			{
				get { return mKeyHeightUp; }
				set { mKeyHeightUp = value; }
			}

			/// <summary>
			/// Клавиша для уменьшения высоты
			/// </summary>
			public KeyCode KeyHeightDown
			{
				get { return mKeyHeightDown; }
				set { mKeyHeightDown = value; }
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
				mDistance = 4;
				mHeight = 2;
				mRotateDamping = 5;
				mHeightDamping = 2;

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
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				if (mIsControl)
				{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

					if (mIsSupportOrbit)
					{
						if (Input.GetMouseButtonDown(1))
						{
							mIsOrbit = true;
						}
						if (Input.GetMouseButtonUp(1))
						{
							mIsOrbit = false;
						}
					}
#else

#endif
				}
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
					// Получаем текущие положение камеры
					mCurrentX = mThisTransform.eulerAngles.y;
					mCurrentY = mThisTransform.eulerAngles.x;

					mCurrentX += Input.GetAxis(XCameraInput.AxisHorizontalName) * mSpeedXOrbit * Time.deltaTime;
					mCurrentY -= Input.GetAxis(XCameraInput.AxisVerticalName) * mSpeedYOrbit * Time.deltaTime;

					Quaternion rotation = Quaternion.Euler(mCurrentY, mCurrentX, 0);
					Vector3 position = rotation * new Vector3(0.0f, 0.0f, -mOrbitDistance) + mTarget.position;

					mThisTransform.rotation = rotation;
					mThisTransform.position = position;

					// Приближение камеры
					if (mIsZoom)
					{
						mOrbitDistance += Input.GetAxis(XCameraInput.AxisOrbitName);
					}

					ComputeDistance();
				}
				else
				{
					this.SmoothFollowLook();

					// Приближение камеры
					if (mIsZoom)
					{
						Vector3 dir = mTarget.position - mThisTransform.position;
						mOrbitDistance = dir.magnitude;
						mThisTransform.position += dir * Input.GetAxis(XCameraInput.AxisOrbitName);
					}

					ComputeDistance();
				}

				// Смещения высоты камеры
				if (Input.GetKey(mKeyHeightUp))
				{
					mHeight += 0.05f;
				}


				if (Input.GetKey(mKeyHeightDown))
				{
					mHeight -= 0.05f;
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
			/// Вычисление дистанции
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeDistance()
			{
				// Вычисление расстояния
				mDistance = Vector2.Distance(new Vector2(mThisTransform.position.x, mThisTransform.position.z),
					new Vector2(mTarget.position.x, mTarget.position.z));

				// Вычисление высоты
				mHeight = mThisTransform.position.y - mTarget.position.y;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Плавный возврат камеры в направление объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SmoothFollowLook()
			{
				// Calculate the current rotation angles
				Single wanted_rotation_angle = Target.eulerAngles.y;
				Single wanted_height = Target.position.y + mHeight;

				Single current_rotation_angle = transform.eulerAngles.y;
				Single current_height = transform.position.y;

				// Damp the rotation around the y-axis
				current_rotation_angle = Mathf.LerpAngle(current_rotation_angle, wanted_rotation_angle, mRotateDamping * Time.deltaTime);

				// Damp the height
				current_height = Mathf.Lerp(current_height, wanted_height, mHeightDamping * Time.deltaTime);

				// Convert the angle into a rotation
				Quaternion current_rotation = Quaternion.Euler(0, current_rotation_angle, 0);

				// Set the position of the camera on the x-z plane to:
				// distance meters behind the target
				transform.position = mTarget.position;
				transform.position -= current_rotation * Vector3.forward * mDistance;

				// Set the height of the camera
				transform.position = new Vector3(transform.position.x, current_height, transform.position.z);

				//Always look at the target
				transform.LookAt(Target);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение камеры из текущей позиции в вычисленную
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void MoveToComputePosition()
			{
				mIsControl = false;

				// Calculate the current rotation angles
				Single wanted_rotation_angle = Target.eulerAngles.y;
				Single wanted_height = Target.position.y + mHeight;

				Single current_rotation_angle = transform.eulerAngles.y;
				Single current_height = transform.position.y;

				// Damp the rotation around the y-axis
				current_rotation_angle = Mathf.LerpAngle(current_rotation_angle, wanted_rotation_angle, mRotateDamping * Time.deltaTime);

				// Damp the height
				current_height = Mathf.Lerp(current_height, wanted_height, mHeightDamping * Time.deltaTime);

				// Convert the angle into a rotation
				Quaternion current_rotation = Quaternion.Euler(0, current_rotation_angle, 0);

				// Set the position of the camera on the x-z plane to:
				// distance meters behind the target
				Vector3 position = mTarget.position;
				position -= current_rotation * Vector3.forward * mDistance;

				// Set the height of the camera
				Vector3 target_position = new Vector3(position.x, current_height, position.z);

				Quaternion look_target = Quaternion.LookRotation((mTarget.position - target_position).normalized);

				this.MoveTo(0.5f, target_position, TEasingType.CubeInOut, () =>
				{
					mIsControl = true;
				});

				this.RotationTo(0.5f, look_target, TEasingType.SineInOut);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================