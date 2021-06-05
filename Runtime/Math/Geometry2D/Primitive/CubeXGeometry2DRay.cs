﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема 2D геометрии
// Группа: Двухмерные геометрические примитивы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXGeometry2DRay.cs
*		Структура для представления луча в двухмерном пространстве.
*		Реализация структуры для представления луча в двухмерном пространстве.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Globalization;
//=====================================================================================================================
namespace CubeX
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup Geometry2D
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Структура луча в двухмерном пространстве
		/// </summary>
		/// <remarks>
		/// Луч представляют собой точку и направление
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct Ray2Df : IEquatable<Ray2Df>, IComparable<Ray2Df>, ICloneable
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Текстовый формат отображения параметров луча
			/// </summary>
			public static String ToStringFormat = "Pos = {0:0.00}, {1:0.00}; Dir = {2:0.00}, {3:0.00}";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Позиция луча
			/// </summary>
			public Vector2Df Position;

			/// <summary>
			/// Направление луча
			/// </summary>
			public Vector2Df Direction;
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует луч указанными параметрами
			/// </summary>
			/// <param name="pos">Позиция луча</param>
			/// <param name="dir">Направление луча</param>
			//---------------------------------------------------------------------------------------------------------
			public Ray2Df(Vector2Df pos, Vector2Df dir)
			{
				Position = pos;
				Direction = dir;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует луч указанным лучом
			/// </summary>
			/// <param name="source">Луч</param>
			//---------------------------------------------------------------------------------------------------------
			public Ray2Df(Ray2Df source)
			{
				Position = source.Position;
				Direction = source.Direction;
			}

#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует луч указанными параметрами
			/// </summary>
			/// <param name="pos">Позиция луча</param>
			/// <param name="dir">Направление луча</param>
			//---------------------------------------------------------------------------------------------------------
			public Ray2Df(UnityEngine.Vector2 pos, UnityEngine.Vector2 dir)
			{
				Position = new Vector2Df(pos.x, pos.y);
				Direction = new Vector2Df(dir.x, dir.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует луч указанным лучом
			/// </summary>
			/// <param name="source">Луч</param>
			//---------------------------------------------------------------------------------------------------------
			public Ray2Df(UnityEngine.Ray2D source)
			{
				Position = new Vector2Df(source.origin.x, source.origin.y);
				Direction = new Vector2Df(source.direction.x, source.direction.y);
			}
#endif
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверяет равен ли текущий объект другому объекту того же типа
			/// </summary>
			/// <param name="obj">Сравниваемый объект</param>
			/// <returns>Статус равенства объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean Equals(Object obj)
			{
				if (obj != null)
				{
					if (typeof(Ray2Df) == obj.GetType())
					{
						Ray2Df ray = (Ray2Df)obj;
						return Equals(ray);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства лучей по значению
			/// </summary>
			/// <param name="other">Сравниваемый луч</param>
			/// <returns>Статус равенства лучей</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Equals(Ray2Df other)
			{
				return this == other;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение лучей для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый луч</param>
			/// <returns>Статус сравнения лучей</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(Ray2Df other)
			{
				if (Position > other.Position)
				{
					return 1;
				}
				else
				{
					if (Position == other.Position && Direction > other.Direction)
					{
						return 1;
					}
					else
					{
						return 0;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода луча
			/// </summary>
			/// <returns>Хеш-код луча</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return Position.GetHashCode() ^ Direction.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование луча
			/// </summary>
			/// <returns>Копия луча</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление луча с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return String.Format(ToStringFormat, Position.X, Position.Y, Direction.X, Direction.Y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения</param>
			/// <returns>Текстовое представление луча с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToString(String format)
			{
				return "Pos = " + Position.ToString(format) + "; Dir = " + Direction.ToString(format);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение лучей на равенство
			/// </summary>
			/// <param name="left">Первый луч</param>
			/// <param name="right">Второй луч</param>
			/// <returns>Статус равенства лучей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator ==(Ray2Df left, Ray2Df right)
			{
				return left.Position == right.Position && left.Direction == right.Direction;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение лучей на неравенство
			/// </summary>
			/// <param name="left">Первый луч</param>
			/// <param name="right">Второй луч</param>
			/// <returns>Статус неравенства лучей</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator !=(Ray2Df left, Ray2Df right)
			{
				return left.Position != right.Position || left.Direction != right.Direction;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обратный луч
			/// </summary>
			/// <param name="ray">Исходный луч</param>
			/// <returns>Обратный луч</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Ray2Df operator -(Ray2Df ray)
			{
				return new Ray2Df(ray.Position, -ray.Direction);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ ПРЕОБРАЗОВАНИЯ ==================================
#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа <see cref="UnityEngine.Ray2D"/>
			/// </summary>
			/// <param name="ray">Луч</param>
			/// <returns>Объект <see cref="UnityEngine.Ray2D"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator UnityEngine.Ray2D(Ray2Df ray)
			{
				return new UnityEngine.Ray2D(ray.Position, ray.Direction);
			}
#endif
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение точки на луче
			/// </summary>
			/// <param name="position">Позиция точки от начала луча</param>
			/// <returns>Точка на луче</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2Df GetPoint(Single position)
			{
				return Position + (Direction * position);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров луча
			/// </summary>
			/// <param name="start_point">Начальная точка</param>
			/// <param name="end_point">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetFromPoint(Vector2Df start_point, Vector2Df end_point)
			{
				Position = start_point;
				Direction = (end_point - start_point).Normalized;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на пересечение с лучом
			/// </summary>
			/// <param name="ray">Луч</param>
			/// <returns>Тип пересечения</returns>
			//---------------------------------------------------------------------------------------------------------
			public TIntersectType2D IntersectRay(Ray2Df ray)
			{
				TIntersectHit2Df raycast_hit = new TIntersectHit2Df();
				return XIntersect2D.RayToRay(ref Position, ref Direction, ref ray.Position, ref ray.Direction, ref raycast_hit);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================