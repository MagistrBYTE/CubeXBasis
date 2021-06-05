//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема управления камерами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCameraFree.cs
*		Камера со свободным перемещением.
*		Реализация компонента камеры с абсолютно свободным перемещением в пространстве.
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
		/// Камера со свободным перемещением
		/// </summary>
		/// <remarks>
		/// Реализация компонента камеры с абсолютно свободным перемещением в пространстве
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Camera/Free-Camera")]
		public class CubeXCameraFree : MonoBehaviour
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
			[CubeXDisplayName(nameof(KeyMoveUp))]
			internal KeyCode mKeyMoveUp = KeyCode.Keypad8;
			[SerializeField]
			[CubeXDisplayName(nameof(KeyMoveDown))]
			internal KeyCode mKeyMoveDown = KeyCode.Keypad2;
			[SerializeField]
			[CubeXDisplayName(nameof(Speed))]
			internal Single mSpeed = 50;

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
			/// Клавиша для смещения вверх
			/// </summary>
			public KeyCode KeyMoveUp
			{
				get { return mKeyMoveUp; }
				set { mKeyMoveUp = value; }
			}

			/// <summary>
			/// Клавиша для смещения вниз
			/// </summary>
			public KeyCode KeyMoveDown
			{
				get { return mKeyMoveDown; }
				set { mKeyMoveDown = value; }
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
				if(Input.GetKey(mKeyMoveForward))
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
				if (Input.GetKey(mKeyMoveUp))
				{
					mThisTransform.position += mThisTransform.up * Time.deltaTime * mSpeed;
				}
				if (Input.GetKey(mKeyMoveDown))
				{
					mThisTransform.position += -mThisTransform.up * Time.deltaTime * mSpeed;
				}

				if (Input.GetMouseButton(0))
				{
					mThisTransform.Rotate(Vector3.up, Input.GetAxis(XCameraInput.AxisHorizontalName));
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
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================