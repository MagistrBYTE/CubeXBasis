//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ввода
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInputDispatcher.cs
*		Центральный диспетчер пользовательского ввода.
*		Реализация центрального диспетчера пользовательского ввода который обобщает и абстрагирует ввод от источника данных,
*	устройств и различных платформ и дополнительно представляет его в виде конкретных событий.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonInput
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер пользовательского ввода
		/// </summary>
		/// <remarks>
		/// <para>
		/// Реализация центрального диспетчера пользовательского ввода который обобщает и абстрагирует ввод от источника данных,
		/// устройств и различных платформ и дополнительно представляет его в виде конкретных событий.
		/// </para>
		/// <para>
		/// Управляется центральным диспетчером <see cref="CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XInputDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Положение указателя в экранных координатах
			/// </summary>
			/// <remarks>
			/// Начало координат верхний левый угол экрана
			/// </remarks>
			public static Vector2 PositionPointer;

			/// <summary>
			/// Положение указателя (второго) в экранных координатах (для мобильных платформ)
			/// </summary>
			/// <remarks>
			/// Начало координат верхний левый угол экрана
			/// </remarks>
			public static Vector2 PositionPointer2;

			/// <summary>
			/// Положение указателя в экранных координатах когда была нажата левая кнопка мыши
			/// </summary>
			public static Vector2 PositionPointerLeftDown;

			/// <summary>
			/// Положение указателя в экранных координатах когда была отпущена левая кнопка мыши
			/// </summary>
			public static Vector2 PositionPointerLeftUp;

			/// <summary>
			/// Положение указателя в экранных координатах когда была нажата правая кнопка мыши
			/// </summary>
			public static Vector2 PositionPointerRightDown;

			/// <summary>
			/// Положение указателя в экранных координатах когда была отпущена правая кнопка мыши
			/// </summary>
			public static Vector2 PositionPointerRightUp;

			/// <summary>
			/// Смещение указателя
			/// </summary>
			public static Vector2 DeltaPointer;

			/// <summary>
			/// Смещение указателя (второго) (для мобильных платформ)
			/// </summary>
			public static Vector2 DeltaPointer2;

			/// <summary>
			/// Список обработчиков событий на событие PointerDown
			/// </summary>
			public static readonly List<ICubeXPointerDown> PointersDown = new List<ICubeXPointerDown>();

			/// <summary>
			/// Список обработчиков событий на событие PointerPress
			/// </summary>
			public static readonly List<ICubeXPointerPress> PointersPress = new List<ICubeXPointerPress>();

			/// <summary>
			/// Список обработчиков событий на событие PointerMove
			/// </summary>
			public static readonly List<ICubeXPointerMove> PointersMove = new List<ICubeXPointerMove>();

			/// <summary>
			/// Список обработчиков событий на событие PointerMove
			/// </summary>
			public static readonly List<ICubeXPointerUp> PointersUp = new List<ICubeXPointerUp>();

			/// <summary>
			/// Список виртуальных кнопок
			/// </summary>
			public static readonly Dictionary<String, ICubeXVirtualButton> Buttons = new Dictionary<String, ICubeXVirtualButton>();

			/// <summary>
			/// Список виртуальных осей
			/// </summary>
			public static readonly Dictionary<String, ICubeXVirtualAxis> Axises = new Dictionary<String, ICubeXVirtualAxis>();

			/// <summary>
			/// Список виртуальных джойстиков
			/// </summary>
			public static readonly Dictionary<String, ICubeXVirtualJoystick> Joysticks = new Dictionary<String, ICubeXVirtualJoystick>();

			// Основная и дополнительная оси управления
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
			private static readonly String AxisFirstMainHorizontal = XAxis.HORIZONTAL;
			private static readonly String AxisFirstMainVertical = XAxis.VERTICAL;
			private static readonly String AxisSecondMainHorizontal = XAxis.MOUSE_X;
			private static readonly String AxisSecondMainVertical = XAxis.MOUSE_Y;
#else
			public static ICubeXVirtualJoystick AxisFirstMainJoystick = null;
			public static ICubeXVirtualJoystick AxisSecondMainJoystick = null;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус смещение указателя
			/// </summary>
			public static Boolean IsDeltaMove
			{
				get { return DeltaPointer.sqrMagnitude > 0.1f; }
			}
			
			/// <summary>
			/// Смещение указателя по X
			/// </summary>
			public static Single DeltaPointerX
			{
				get { return DeltaPointer.x; }
			}

			/// <summary>
			/// Смещение указателя по Y
			/// </summary>
			public static Single DeltaPointerY
			{
				get { return DeltaPointer.y; }
			}
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных центрального диспетчера пользовательского ввода
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск центрального диспетчера пользовательского ввода
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnStart()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление центрального диспетчера каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnUpdate()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				UpdateDesktopInput();

#else
				UpdateMobileInput();
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление центрального диспетчера каждый кадр после (после Update)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnLateUpdate()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				Vector2 mouse_point = Input.mousePosition;
				mouse_point.y = Screen.height - mouse_point.y;
				DeltaPointer = mouse_point - PositionPointer;
				PositionPointer = mouse_point;
#else
#endif
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных стандартного ввода
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateDesktopInput()
			{
				if (Input.GetMouseButtonDown(0))
				{
					Vector2 mouse_point = Input.mousePosition;
					mouse_point.y = Screen.height - mouse_point.y;
					PositionPointerLeftDown = mouse_point;

					for (Int32 i = 0; i < PointersDown.Count; i++)
					{
						PointersDown[i].PointerDown(0);
					}
				}

				if (Input.GetMouseButtonDown(1))
				{
					Vector2 mouse_point = Input.mousePosition;
					mouse_point.y = Screen.height - mouse_point.y;
					PositionPointerRightDown = mouse_point;

					for (Int32 i = 0; i < PointersDown.Count; i++)
					{
						PointersDown[i].PointerDown(1);
					}
				}

				if (Input.GetMouseButtonDown(2))
				{
					for (Int32 i = 0; i < PointersDown.Count; i++)
					{
						PointersDown[i].PointerDown(2);
					}
				}

				if (Input.GetMouseButton(0))
				{
					for (Int32 i = 0; i < PointersPress.Count; i++)
					{
						PointersPress[i].PointerPress(0);
					}
				}

				if (Input.GetMouseButton(1))
				{
					for (Int32 i = 0; i < PointersPress.Count; i++)
					{
						PointersPress[i].PointerPress(1);
					}
				}

				if (Input.GetMouseButton(2))
				{
					for (Int32 i = 0; i < PointersPress.Count; i++)
					{
						PointersPress[i].PointerPress(2);
					}
				}

				if (Input.GetMouseButtonUp(0))
				{
					Vector2 mouse_point = Input.mousePosition;
					mouse_point.y = Screen.height - mouse_point.y;
					PositionPointerLeftUp = mouse_point;

					for (Int32 i = 0; i < PointersUp.Count; i++)
					{
						PointersUp[i].PointerUp(0);
					}
				}

				if (Input.GetMouseButtonUp(1))
				{
					Vector2 mouse_point = Input.mousePosition;
					mouse_point.y = Screen.height - mouse_point.y;
					PositionPointerLeftUp = mouse_point;

					for (Int32 i = 0; i < PointersUp.Count; i++)
					{
						PointersUp[i].PointerUp(1);
					}
				}

				if (Input.GetMouseButtonUp(2))
				{
					for (Int32 i = 0; i < PointersUp.Count; i++)
					{
						PointersUp[i].PointerUp(2);
					}
				}

				if (DeltaPointer.sqrMagnitude > 0.01f)
				{
					Int32 pointer = -1;
					if (Input.GetMouseButton(0))
					{
						pointer = 0;
					}
					if (Input.GetMouseButton(1))
					{
						pointer = 1;
					}
					for (Int32 i = 0; i < PointersMove.Count; i++)
					{
						PointersMove[i].PointerMove(pointer);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных мобильного ввода
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateMobileInput()
			{
				if (Input.touchCount > 0)
				{
					Touch[] touches = Input.touches;
					for (Int32 it = 0; it < touches.Length; it++)
					{
						if (it == 0)
						{
							Vector2 mouse_point = touches[it].position;
							mouse_point.y = Screen.height - mouse_point.y;
							PositionPointer = mouse_point;
							DeltaPointer = touches[it].deltaPosition;
						}
						if (it == 1)
						{
							Vector2 mouse_point = touches[it].position;
							mouse_point.y = Screen.height - mouse_point.y;
							PositionPointer2 = mouse_point;
							DeltaPointer2 = touches[it].deltaPosition;
						}

						if (touches[it].phase == TouchPhase.Began)
						{
							if(it == 0)
							{
								Vector2 mouse_point = touches[it].position;
								mouse_point.y = Screen.height - mouse_point.y;
								PositionPointerLeftDown = mouse_point;
							}

							for (Int32 i = 0; i < PointersDown.Count; i++)
							{
								PointersDown[i].PointerDown(it);
							}
						}

						if (touches[it].phase == TouchPhase.Stationary || touches[it].phase == TouchPhase.Moved)
						{
							for (Int32 i = 0; i < PointersPress.Count; i++)
							{
								PointersPress[i].PointerPress(it);
							}
						}

						if (touches[it].phase == TouchPhase.Moved)
						{
							for (Int32 i = 0; i < PointersMove.Count; i++)
							{
								PointersMove[i].PointerMove(it);
							}
						}

						if (touches[it].phase == TouchPhase.Ended || touches[it].phase == TouchPhase.Canceled)
						{
							if (it == 0)
							{
								Vector2 mouse_point = touches[it].position;
								mouse_point.y = Screen.height - mouse_point.y;
								PositionPointerLeftUp = mouse_point;
							}

							for (Int32 i = 0; i < PointersUp.Count; i++)
							{
								PointersUp[i].PointerUp(it);
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус нажатия указанной кнопки мыши
			/// </summary>
			/// <param name="mouse_button">Кнопка мыши</param>
			/// <returns>Статус нажатия</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsMouseButtonDown(TMouseButtonInput mouse_button)
			{
				if(mouse_button == TMouseButtonInput.Left && Input.GetMouseButtonDown(0))
				{
					return (true);
				}
				else
				{
					if (mouse_button == TMouseButtonInput.Right && Input.GetMouseButtonDown(1))
					{
						return (true);
					}
					else
					{
						if (mouse_button == TMouseButtonInput.Middle && Input.GetMouseButtonDown(2))
						{
							return (true);
						}
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус удержания указанной кнопки мыши
			/// </summary>
			/// <param name="mouse_button">Кнопка мыши</param>
			/// <returns>Статус удержания</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsMouseButton(TMouseButtonInput mouse_button)
			{
				if (mouse_button == TMouseButtonInput.Left && Input.GetMouseButton(0))
				{
					return (true);
				}
				else
				{
					if (mouse_button == TMouseButtonInput.Right && Input.GetMouseButton(1))
					{
						return (true);
					}
					else
					{
						if (mouse_button == TMouseButtonInput.Middle && Input.GetMouseButton(2))
						{
							return (true);
						}
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Статус отпускания указанной кнопки мыши
			/// </summary>
			/// <param name="mouse_button">Кнопка мыши</param>
			/// <returns>Статус отпускания</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsMouseButtonUp(TMouseButtonInput mouse_button)
			{
				if (mouse_button == TMouseButtonInput.Left && Input.GetMouseButtonUp(0))
				{
					return (true);
				}
				else
				{
					if (mouse_button == TMouseButtonInput.Right && Input.GetMouseButtonUp(1))
					{
						return (true);
					}
					else
					{
						if (mouse_button == TMouseButtonInput.Middle && Input.GetMouseButtonUp(2))
						{
							return (true);
						}
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение направление перемещения указателя посредством <see cref="UnityEngine.EventSystems.EventSystem"/>
			/// </summary>
			/// <param name="dx">Дельта смещения по оси X</param>
			/// <param name="dy">Дельта смещения по оси Y</param>
			/// <returns>Направление перемещения указателя</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TDirection2D GetDirectionFromInputEvent(Single dx, Single dy)
			{
				if(Mathf.Abs(dx) > Mathf.Abs(dy))
				{
					if(dx > 0)
					{
						return (TDirection2D.Right);
					}
					else
					{
						return (TDirection2D.Left);
					}
				}
				else
				{
					if (dy > 0)
					{
						return (TDirection2D.Up);
					}
					else
					{
						return (TDirection2D.Down);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение направление перемещения указателя посредством <see cref="UnityEngine.Event"/>
			/// </summary>
			/// <param name="dx">Дельта смещения по оси X</param>
			/// <param name="dy">Дельта смещения по оси Y</param>
			/// <returns>Направление перемещения указателя</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TDirection2D GetDirectionFromInputClassic(Single dx, Single dy)
			{
				if (Mathf.Abs(dx) > Mathf.Abs(dy))
				{
					if (dx > 0)
					{
						return (TDirection2D.Right);
					}
					else
					{
						return (TDirection2D.Left);
					}
				}
				else
				{
					if (dy > 0)
					{
						return (TDirection2D.Down);
					}
					else
					{
						return (TDirection2D.Up);
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ИНТЕРФЕЙСОВ ОПОВЕЩЕНИЯ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация подписки на обработку события нажатия
			/// </summary>
			/// <param name="pointer_down">Интерфейс нажатия</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterPointerDown(ICubeXPointerDown pointer_down)
			{
				PointersDown.Add(pointer_down);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация подписки на обработку события удерживания
			/// </summary>
			/// <param name="pointer_press">Интерфейс удерживания</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterPointerPress(ICubeXPointerPress pointer_press)
			{
				PointersPress.Add(pointer_press);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация подписки на обработку события перемещения
			/// </summary>
			/// <param name="pointer_move">Интерфейс перемещения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterPointerMove(ICubeXPointerMove pointer_move)
			{
				PointersMove.Add(pointer_move);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация подписки на обработку события отпускания
			/// </summary>
			/// <param name="pointer_up">Интерфейс отпускания</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterPointerUp(ICubeXPointerUp pointer_up)
			{
				PointersUp.Add(pointer_up);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации подписки на обработку события нажатия
			/// </summary>
			/// <param name="pointer_down">Интерфейс нажатия</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterPointerDown(ICubeXPointerDown pointer_down)
			{
				PointersDown.Remove(pointer_down);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации подписки на обработку события удерживания
			/// </summary>
			/// <param name="pointer_press">Интерфейс удерживания</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterPointerPress(ICubeXPointerPress pointer_press)
			{
				PointersPress.Remove(pointer_press);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации подписки на обработку события перемещения
			/// </summary>
			/// <param name="pointer_move">Интерфейс перемещения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterPointerMove(ICubeXPointerMove pointer_move)
			{
				PointersMove.Remove(pointer_move);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрации на обработку события отпускания
			/// </summary>
			/// <param name="pointer_up">Интерфейс отпускания</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterPointerUp(ICubeXPointerUp pointer_up)
			{
				PointersUp.Remove(pointer_up);
			}
			#endregion

			#region ======================================= МЕТОДЫ ВИРТУАЛЬНОГО ВВОДА =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение статуса удержания нажатой кнопки
			/// </summary>
			/// <param name="name">Имя кнопки</param>
			/// <returns>Статус удержания нажатой кнопки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GetButton(String name)
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				// Ищем сначала среди зарегистрированных кнопок
				ICubeXVirtualButton button = null;
				if (Buttons.TryGetValue(name, out button))
				{
					return button.IsButtonPressed;
				}
				else
				{
					return Input.GetButton(name);
				}
#else
				// Ищем только среди зарегистрированных кнопок
				ICubeXVirtualButton button = null;
				if(Buttons.TryGetValue(name, out button))
				{
					return (button.IsButtonPressed);
				}
				else
				{
					return (false);
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение статуса нажатия кнопки
			/// </summary>
			/// <param name="name">Имя кнопки</param>
			/// <returns>Статус нажатия кнопки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GetButtonDown(String name)
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				// Ищем сначала среди зарегистрированных кнопок
				ICubeXVirtualButton button = null;
				if (Buttons.TryGetValue(name, out button))
				{
					return button.IsButtonDown;
				}
				else
				{
					return Input.GetButtonDown(name);
				}

#else
				// Ищем только среди зарегистрированных кнопок
				ICubeXVirtualButton button = null;
				if(Buttons.TryGetValue(name, out button))
				{
					return (button.IsButtonDown);
				}
				else
				{
					return (false);
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение статуса отпускания кнопки
			/// </summary>
			/// <param name="name">Имя кнопки</param>
			/// <returns>Статус отпускания кнопки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean GetButtonUp(String name)
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				// Ищем сначала среди зарегистрированных кнопок
				ICubeXVirtualButton button = null;
				if (Buttons.TryGetValue(name, out button))
				{
					return button.IsButtonUp;
				}
				else
				{
					return Input.GetButtonUp(name);
				}
#else
				// Ищем только среди зарегистрированных кнопок
				ICubeXVirtualButton button = null;
				if(Buttons.TryGetValue(name, out button))
				{
					return (button.IsButtonUp);
				}
				else
				{
					return (false);
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по одномерной оси
			/// </summary>
			/// <param name="name">Имя оси</param>
			/// <returns>Смещения/перемещения по одномерной оси в относительных единицах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAxis(String name)
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return Input.GetAxis(name);

#else
				// Ищем только среди зарегистрированных осей
				ICubeXVirtualAxis axis = null;
				if(Axises.TryGetValue(name, out axis))
				{
					return (axis.GetAxis());
				}
				else
				{
					return (0.0f);
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по двумерной оси
			/// </summary>
			/// <param name="name">Имя оси</param>
			/// <returns>Смещения/перемещения по одномерной оси в относительных единицах</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetJoystick(String name)
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return Vector2.zero;
#else
				// Ищем только среди зарегистрированных осей
				ICubeXVirtualJoystick joystick = null;
				if(Joysticks.TryGetValue(name, out joystick))
				{
					return (joystick.GetAxis());
				}
				else
				{
					return (Vector2.zero);
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по первой основной оси
			/// </summary>
			/// <returns>Смещение/перемещение по первой основной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetAxisFirstMain()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return new Vector2(Input.GetAxis(AxisFirstMainHorizontal), Input.GetAxis(AxisFirstMainHorizontal));
#else
				return (AxisFirstMainJoystick.GetAxis());
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по горизонтали первой основной оси
			/// </summary>
			/// <returns>Смещение/перемещение по горизонтали</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAxisFirstMainHorizontal()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return Input.GetAxis(AxisFirstMainHorizontal);

#else
				return (AxisFirstMainJoystick.GetAxisHorizontal());
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по вертикали первой основной оси
			/// </summary>
			/// <returns>Смещение/перемещение по вертикали первой основной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAxisFirstMainVertical()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return Input.GetAxis(AxisFirstMainVertical);

#else
				return (AxisFirstMainJoystick.GetAxisVertical());
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по второй основной оси
			/// </summary>
			/// <returns>Смещение/перемещение по второй основной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 GetAxisSecondMain()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return new Vector2(Input.GetAxis(AxisSecondMainHorizontal), Input.GetAxis(AxisSecondMainHorizontal));
#else
				return (AxisSecondMainJoystick.GetAxis());
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по горизонтали второй основной оси
			/// </summary>
			/// <returns>Смещение/перемещение по горизонтали второй основной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAxisSecondMainHorizontal()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return Input.GetAxis(AxisSecondMainHorizontal);

#else
				return (AxisSecondMainJoystick.GetAxisHorizontal());
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение смещения/перемещения по вертикали второй основной оси
			/// </summary>
			/// <returns>Смещение/перемещение по вертикали второй основной оси</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetAxisSecondMainVertical()
			{
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

				return Input.GetAxis(AxisSecondMainVertical);

#else
				return (AxisSecondMainJoystick.GetAxisVertical());
#endif
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================