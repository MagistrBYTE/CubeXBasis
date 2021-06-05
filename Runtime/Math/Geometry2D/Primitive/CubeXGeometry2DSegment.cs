﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема 2D геометрии
// Группа: Двухмерные геометрические примитивы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXGeometry2DSegment.cs
*		Структура для представления сегмента в двухмерном пространстве.
*		Реализация структуры для представления сегмента в двухмерном пространстве.
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
		/// Структура сегмента(отрезка) в двухмерном пространстве
		/// </summary>
		/// <remarks>
		/// Сегмент(отрезок) характеризуется начальной и конечной точкой
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[StructLayout(LayoutKind.Sequential)]
		public struct Segment2Df : IEquatable<Segment2Df>, IComparable<Segment2Df>, ICloneable
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Текстовый формат отображения параметров сегмента
			/// </summary>
			public static String ToStringFormat = "Start = {0:0.00}, {1:0.00}; End = {2:0.00}, {3:0.00}";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Начало сегмента
			/// </summary>
			public Vector2Df Start;

			/// <summary>
			/// Окончание сегмента
			/// </summary>
			public Vector2Df End;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Центр сегмента
			/// </summary>
			public Vector2Df Location
			{
				get { return ((Start + End) / 2); }
			}

			/// <summary>
			/// Направление сегмента
			/// </summary>
			public Vector2Df Direction
			{
				get { return ((End - Start)); }
			}

			/// <summary>
			/// Единичное направление сегмента
			/// </summary>
			public Vector2Df DirectionUnit
			{
				get { return ((End - Start).Normalized); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанными параметрами
			/// </summary>
			/// <param name="start">Начало сегмента</param>
			/// <param name="end">Окончание сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment2Df(Vector2Df start, Vector2Df end)
			{
				Start = start;
				End = end;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанной линией
			/// </summary>
			/// <param name="source">Сегмент</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment2Df(Segment2Df source)
			{
				Start = source.Start;
				End = source.End;
			}

#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанными параметрами
			/// </summary>
			/// <param name="start">Начало сегмента</param>
			/// <param name="end">Окончание сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment2Df(UnityEngine.Vector2 start, UnityEngine.Vector2 end)
			{
				Start = new Vector2Df(start.x, start.y);
				End = new Vector2Df(end.x, end.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует сегмент указанным лучом
			/// </summary>
			/// <param name="ray">Луч</param>
			/// <param name="length">Длина сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public Segment2Df(UnityEngine.Ray2D ray, Single length)
			{
				Start = new Vector2Df(ray.origin.x, ray.origin.y);
				End = new Vector2Df(ray.origin.x + ray.direction.x * length, ray.origin.y + ray.direction.y * length);
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
					if (typeof(Segment2Df) == obj.GetType())
					{
						Segment2Df segment = (Segment2Df)obj;
						return Equals(segment);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства сегментов по значению
			/// </summary>
			/// <param name="other">Сравниваемый сегмент</param>
			/// <returns>Статус равенства сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Equals(Segment2Df other)
			{
				return this == other;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение сегментов для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый сегмент</param>
			/// <returns>Статус сравнения сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(Segment2Df other)
			{
				if (Start > other.Start)
				{
					return 1;
				}
				else
				{
					if (Start == other.Start && End > other.End)
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
			/// Получение хеш-кода сегмента
			/// </summary>
			/// <returns>Хеш-код сегмента</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return (Start.GetHashCode() ^ End.GetHashCode());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование сегмента
			/// </summary>
			/// <returns>Копия сегмента</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление сегмента с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return String.Format(ToStringFormat, Start.X, Start.Y, End.X, End.Y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения</param>
			/// <returns>Текстовое представление сегмента с указанием значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToString(String format)
			{
				return "Start = " + Start.ToString(format) + "; End = " + End.ToString(format);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение сегментов на равенство
			/// </summary>
			/// <param name="left">Первый сегмент</param>
			/// <param name="right">Второй сегмент</param>
			/// <returns>Статус равенства сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator ==(Segment2Df left, Segment2Df right)
			{
				return left.Start == right.Start && left.End == right.End;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение сегментов на неравенство
			/// </summary>
			/// <param name="left">Первый сегмент</param>
			/// <param name="right">Второй сегмент</param>
			/// <returns>Статус неравенства сегментов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator !=(Segment2Df left, Segment2Df right)
			{
				return left.Start != right.Start || left.End != right.End;
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ ПРЕОБРАЗОВАНИЯ ==================================
#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа <see cref="UnityEngine.Ray2D"/> 
			/// </summary>
			/// <param name="segment">Сегмент</param>
			/// <returns>Объект <see cref="UnityEngine.Ray2D"/></returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator UnityEngine.Ray2D(Segment2Df segment)
			{
				return new UnityEngine.Ray2D(segment.Start, segment.DirectionUnit);
			}
#endif
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на попадание точки на сегмент
			/// </summary>
			/// <param name="point">Проверяемая точка</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус попадания</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Contains(Vector2Df point, Single epsilon = 0.01f)
			{
				return(XIntersect2D.PointOnSegment(ref Start, ref End, ref point, epsilon));
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================