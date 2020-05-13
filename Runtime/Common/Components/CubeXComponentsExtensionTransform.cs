//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема компонентов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXComponentsExtensionTransform.cs
*		Методы расширения функциональности компонента Transform.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonComponent
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений функциональности компонента Transform
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionTransform
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции игрового объекта по X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="x">Позиция по X</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetX(this Transform @this, Single x)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				Vector3 pos = @this.position;
				pos.x = x;
				@this.position = pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции игрового объекта по Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="y">Позиция по Y</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetY(this Transform @this, Single y)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				Vector3 pos = @this.position;
				pos.y = y;
				@this.position = pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции игрового объекта по Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="z">Координата по Z</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetZ(this Transform @this, Single z)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				Vector3 pos = @this.position;
				pos.z = z;
				@this.position = pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Наклон или поворот игрового объекта относительно правого вектора (по оси X)
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="x">Угол поворота в градусах</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetPitch(this Transform @this, Single x)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				Vector3 pos = @this.eulerAngles;
				pos.x = x;
				@this.eulerAngles = pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рыскание или поворот игрового объекта относительно верхнего вектора (по оси Y)
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="y">Угол поворота в градусах</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetYaw(this Transform @this, Single y)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				Vector3 pos = @this.eulerAngles;
				pos.y = y;
				@this.eulerAngles = pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение или поворот игрового объекта относительно вектора направления (по оси Z)
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="z">Угол поворота в градусах</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetRoll(this Transform @this, Single z)
			{
#if UNITY_EDITOR
				if (@this == null)
				{
					return;
				}
#endif
				Vector3 pos = @this.eulerAngles;
				pos.z = z;
				@this.eulerAngles = pos;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Абсолютная глубина компонента в иерархии
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <returns>Абсолютная глубина компонента в иерархии</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 AbsoluteDepth(this Transform @this)
			{
				Int32 depth = 0;
				Transform current_transform;
				if(@this.parent != null)
				{
					depth = 1;
					current_transform = @this.parent;
					if(current_transform.parent != null)
					{
						depth = 2;
						current_transform = @this.parent;
						if (current_transform.parent != null)
						{
							depth = 3;
							current_transform = @this.parent;
							if (current_transform.parent != null)
							{
								depth = 4;
								current_transform = @this.parent;
								if (current_transform.parent != null)
								{
									depth = 5;
									current_transform = @this.parent;
									if (current_transform.parent != null)
									{
										depth = 6;
										current_transform = @this.parent;
										if (current_transform.parent != null)
										{
											depth = 7;
											current_transform = @this.parent;
											if (current_transform.parent != null)
											{
												depth = 8;
											}
										}
									}
								}
							}
						}
					}
				}

				return depth;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время полного поворота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotateX(this Transform @this, Single duration)
			{
				Single rotation = @this.eulerAngles.x;
				rotation += 360 * Time.unscaledDeltaTime / duration;
				@this.eulerAngles = new Vector3(rotation, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время полного поворота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotateY(this Transform @this, Single duration)
			{
				Single rotation = @this.eulerAngles.y;
				rotation += 360 * Time.unscaledDeltaTime / duration;
				@this.eulerAngles = new Vector3(@this.eulerAngles.x, rotation, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вращение по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время полного поворота</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotateZ(this Transform @this, Single duration)
			{
				Single rotation = @this.eulerAngles.z;
				rotation += 360 * Time.unscaledDeltaTime / duration;
				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, rotation);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение компонента указанного типа от непосредственного родителя
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Компонент трансформации</param>
			/// <returns>Компонент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent GetComponentFromParent<TComponent>(this Transform @this) where TComponent : Component
			{
				if (@this.parent != null)
				{
					return (@this.transform.parent.GetComponent<TComponent>());
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение компонента указанного типа от непосредственного родителя или его родителя
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Компонент трансформации</param>
			/// <returns>Компонент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent GetComponentFromParentOrHisParent<TComponent>(this Transform @this) where TComponent : Component
			{
				if (@this.parent != null)
				{
					TComponent сomponent = @this.parent.GetComponent<TComponent>();
					if (сomponent != null)
					{
						return (сomponent);
					}
					else
					{
						if (@this.parent.parent != null)
						{
							return (@this.parent.parent.GetComponent<TComponent>());
						}
					}
				}

				return (null);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================