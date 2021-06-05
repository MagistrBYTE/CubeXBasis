//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема управления камерами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCameraEditor.cs
*		Камера аналогичная камеры в редакторе Unity.
*		Реализация компонента камеры по управлению аналогично камеры в редакторе Unity.
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
		/// Камера аналогичная камеры в редакторе Unity
		/// </summary>
		/// <remarks>
		/// Реализация компонента камеры по управлению аналогично камеры в редакторе Unity
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Camera/Editor-Camera")]
		public class CubeXCameraEditor : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXDisplayName(nameof(KeyMoveForward))]
			internal KeyCode mKeyMoveForward = KeyCode.W;
			[SerializeField]
			[CubeXDisplayName(nameof(KeyMoveBackward))]
			internal KeyCode mKeyMoveBackward = KeyCode.S;
			[SerializeField]
			[CubeXDisplayName(nameof(KeyMoveLeft))]
			internal KeyCode mKeyMoveLeft = KeyCode.A;
			[SerializeField]
			[CubeXDisplayName(nameof(KeyMoveRight))]
			internal KeyCode mKeyMoveRight = KeyCode.D;
			[SerializeField]
			[CubeXDisplayName(nameof(Speed))]
			internal Single mSpeed = 50;
			[NonSerialized]
			internal Vector3 mRotation;

			// Кэшированные данные
			[HideInInspector]
			internal Transform mThisTransform;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Клавиша для перемещения вперед по направлению взгляда
			/// </summary>
			public KeyCode KeyMoveForward
			{
				get { return mKeyMoveForward; }
				set { mKeyMoveForward = value; }
			}

			/// <summary>
			/// Клавиша для перемещения назад по направлению взгляда
			/// </summary>
			public KeyCode KeyMoveBackward
			{
				get { return mKeyMoveBackward; }
				set { mKeyMoveBackward = value; }
			}

			/// <summary>
			/// Клавиша для смещения влево
			/// </summary>
			public KeyCode KeyMoveLeft
			{
				get { return mKeyMoveLeft; }
				set { mKeyMoveLeft = value; }
			}

			/// <summary>
			/// Клавиша для смещения вправо
			/// </summary>
			public KeyCode KeyMoveRight
			{
				get { return mKeyMoveRight; }
				set { mKeyMoveRight = value; }
			}

			/// <summary>
			/// Относительная скорость перемещения
			/// </summary>
			public Single Speed
			{
				get { return mSpeed; }
				set { mSpeed = value; }
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
				// Нажата средняя кнопка мыши
				if (Input.GetMouseButton(2))
				{
					mThisTransform.position += mThisTransform.right * mSpeed * Input.GetAxis(XCameraInput.AxisHorizontalName) * Time.deltaTime;
					mThisTransform.position += Vector3.up * mSpeed * Input.GetAxis(XCameraInput.AxisVerticalName) * Time.deltaTime;
				}
				else
				{
					if (Input.GetMouseButton(1))
					{
						mRotation.y += Input.GetAxis(XCameraInput.AxisHorizontalName);
						mRotation.x += Input.GetAxis(XCameraInput.AxisVerticalName);
						mThisTransform.eulerAngles = mRotation;

						if (Input.GetKey(mKeyMoveForward))
						{
							mThisTransform.position += mThisTransform.forward * Time.deltaTime * mSpeed;
						}
						if (Input.GetKey(mKeyMoveBackward))
						{
							mThisTransform.position += -mThisTransform.forward * Time.deltaTime * mSpeed;
						}
						if (Input.GetKey(mKeyMoveLeft))
						{
							mThisTransform.position += -mThisTransform.right * Time.deltaTime * mSpeed;
						}
						if (Input.GetKey(mKeyMoveRight))
						{
							mThisTransform.position += mThisTransform.right * Time.deltaTime * mSpeed;
						}
					}
				}

				mThisTransform.position += mThisTransform.forward * Input.GetAxis(XCameraInput.AxisOrbitName);
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
				// Получаем текущие положение камеры
				mRotation.y = mThisTransform.eulerAngles.y;
				mRotation.x = mThisTransform.eulerAngles.x;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================