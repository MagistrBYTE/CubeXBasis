﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль алгоритмов
// Подраздел: Алгоритмы привязки пространственных данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXAlgorithmSnapPoint2D.cs
*		Общие типы и структуры данных для привязки точки в двухмерном пространстве.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
//=====================================================================================================================
namespace CubeX
{
	namespace Algorithm
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup AlgorithmModuleSnap Алгоритмы привязки пространственных данных
		//! Подсистема алгоритмов для привязки пространственных данных. Алгоритмы привязки пространственных данных 
		//! обеспечивают привязку объекта к определенному узлу, то есть позволяют найти ближайший соответствующий объект 
		//! в зависимости от различных критериев.
		//! \ingroup AlgorithmModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Точка привязки в двухмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public struct TSnapPoint2D : IEquatable<TSnapPoint2D>, IComparable<TSnapPoint2D>, ICloneable
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Текстовый формат отображения параметров точки
			/// </summary>
			public static String ToStringFormat = "X = {0:0.00}; Y = {1:0.00}";

			/// <summary>
			/// Текстовый формат отображения только значений параметров точки
			/// </summary>
			public static String ToStringFormatValue = "{0:0.00}; {1:0.00}";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Опорная точка
			/// </summary>
			public Vector2Df Point;

			/// <summary>
			/// Дистанция до этой точки
			/// </summary>
			public Single Distance;
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует точку привязки указанными параметрами
			/// </summary>
			/// <param name="x">X - координата</param>
			/// <param name="y">Y - координата</param>
			//---------------------------------------------------------------------------------------------------------
			public TSnapPoint2D(Single x, Single y)
			{
				Point.X = x;
				Point.Y = y;
				Distance = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует точку привязки указанными параметрами
			/// </summary>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public TSnapPoint2D(Vector2Df point)
			{
				Point = point;
				Distance = 0;
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует точку привязки указанными параметрами
			/// </summary>
			/// <param name="vector">Вектор</param>
			//---------------------------------------------------------------------------------------------------------
			public TSnapPoint2D(UnityEngine.Vector2 vector)
			{
				Point.X = vector.x;
				Point.Y = vector.y;
				Distance = 0;
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует точку привязки указанными параметрами
			/// </summary>
			/// <param name="point">Точка</param>
			/// <param name="distance">Дистанция до этой точки</param>
			//---------------------------------------------------------------------------------------------------------
			public TSnapPoint2D(Vector2Df point, Single distance)
			{
				Point = point;
				Distance = distance;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверяет равен ли текущий объект другому объекту того же типа
			/// </summary>
			/// <param name="obj">Сравниваемый объект</param>
			/// <returns>Статус равенства объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean Equals(System.Object obj)
			{
				if (obj != null)
				{
					if (typeof(TSnapPoint2D) == obj.GetType())
					{
						TSnapPoint2D snap_point = (TSnapPoint2D)obj;
						return Equals(snap_point);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства точек привязок по значению
			/// </summary>
			/// <param name="other">Сравниваемая точка привязки</param>
			/// <returns>Статус равенства точек привязок</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Equals(TSnapPoint2D other)
			{
				return (Point == other.Point);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение точек привязок для упорядочивания
			/// </summary>
			/// <param name="other">Точка привязки</param>
			/// <returns>Статус сравнения точек привязок</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(TSnapPoint2D other)
			{
				return (Distance.CompareTo(other.Distance));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода точки привязки
			/// </summary>
			/// <returns>Хеш-код точки привязки</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return Point.GetHashCode() ^ Distance.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование точки привязки
			/// </summary>
			/// <returns>Копия точки привязки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление точки привязки с указанием значений координат</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return String.Format(ToStringFormat, Point.X, Point.Y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения</param>
			/// <returns>Текстовое представление точки привязки с указанием значений координат</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToString(String format)
			{
				return ("X = " + Point.X.ToString(format) + "; Y = " + Point.Y.ToString(format));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление точки привязки с указанием значений координат</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToStringValue()
			{
				return String.Format(ToStringFormatValue, Point.X, Point.Y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения компонентов точки привязки</param>
			/// <returns>Текстовое представление точки привязки с указанием значений координат</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToStringValue(String format)
			{
				return String.Format(ToStringFormatValue.Replace("0.00", format), Point.X, Point.Y);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов на равенство
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус равенства</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator ==(TSnapPoint2D left, TSnapPoint2D right)
			{
				return left.Equals(right);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов на неравенство
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус неравенство</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator !=(TSnapPoint2D left, TSnapPoint2D right)
			{
				return !(left == right);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов по операции меньше
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator <(TSnapPoint2D left, TSnapPoint2D right)
			{
				return left.CompareTo(right) < 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов по операции меньше или равно
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator <=(TSnapPoint2D left, TSnapPoint2D right)
			{
				return left.CompareTo(right) <= 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов по операции больше
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator >(TSnapPoint2D left, TSnapPoint2D right)
			{
				return left.CompareTo(right) > 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов по операции больше или равно
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус операции</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean operator >=(TSnapPoint2D left, TSnapPoint2D right)
			{
				return left.CompareTo(right) >= 0;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление дистанции до указанной точки
			/// </summary>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeDistance(ref Vector2Df point)
			{
				Distance = Vector2Df.Distance(ref Point, ref point);
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление дистанции до указанной точки
			/// </summary>
			/// <param name="vector">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeDistance(ref UnityEngine.Vector2 vector)
			{
				Single x = Point.X - vector.x;
				Single y = Point.Y - vector.y;

				Distance = UnityEngine.Mathf.Sqrt(x * x + y * y);
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений точки
			/// </summary>
			/// <param name="point">Точка</param>
			/// <param name="delta_x">Допуск по координате X</param>
			/// <param name="delta_y">Допуск по координате Y</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ApproximatelyPoint(ref Vector2Df point, Single delta_x, Single delta_y)
			{
				if (Math.Abs(Point.X - point.X) < delta_x && Math.Abs(Point.Y - point.Y) < delta_y)
				{
					return true;
				}

				return false;
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений точки
			/// </summary>
			/// <param name="vector">Точка</param>
			/// <param name="delta_x">Допуск по координате X</param>
			/// <param name="delta_y">Допуск по координате Y</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ApproximatelyPoint(ref UnityEngine.Vector2 vector, Single delta_x, Single delta_y)
			{
				if (Math.Abs(Point.X - vector.x) < delta_x && Math.Abs(Point.Y - vector.y) < delta_y)
				{
					return true;
				}

				return false;
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений точки по координате X
			/// </summary>
			/// <param name="x">Координата точки по X</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ApproximatelyPointX(Single x, Single epsilon)
			{
				if (Math.Abs(Point.X - x) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений точки по координате Y
			/// </summary>
			/// <param name="y">Координата точки по Y</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ApproximatelyPointY(Single y, Single epsilon)
			{
				if (Math.Abs(Point.Y - y) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений дистанции
			/// </summary>
			/// <param name="distance">Дистанция</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean ApproximatelyDistance(Single distance, Single epsilon)
			{
				if (Math.Abs(Distance - distance) < epsilon)
				{
					return true;
				}

				return false;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Список точек привязки в двухмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CListSnapPoint2D : ListArray<TSnapPoint2D>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CListSnapPoint2D()
				: base()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="capacity">Начальная максимальная емкость списка</param>
			//---------------------------------------------------------------------------------------------------------
			public CListSnapPoint2D(Int32 capacity)
				: base(capacity)
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ДОБАВЛЕНИЯ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление точки привязки по указанными параметрами
			/// </summary>
			/// <param name="x">X - координата</param>
			/// <param name="y">Y - координата</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(Single x, Single y)
			{
				Add(new TSnapPoint2D(x, y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление точки привязки по указанными параметрами
			/// </summary>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(Vector2Df point)
			{
				Add(new TSnapPoint2D(point));
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует точку привязки указанными параметрами
			/// </summary>
			/// <param name="vector">Вектор</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(UnityEngine.Vector2 vector)
			{
				Add(new TSnapPoint2D(vector));
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление точки привязки по указанными параметрами
			/// </summary>
			/// <param name="point">Точка</param>
			/// <param name="distance">Дистанция до этой точки</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(Vector2Df point, Single distance)
			{
				Add(new TSnapPoint2D(point, distance));
			}
			#endregion

			#region ======================================= МЕТОДЫ ОПРЕДЕЛЕНИЯ ДИСТАНЦИИ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление дистанции до указанной точки
			/// </summary>
			/// <param name="point">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeDistance(Vector2Df point)
			{
				for (Int32 i = 0; i < mCount; i++)
				{
					mArrayOfItems[i].ComputeDistance(ref point);
				}
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление дистанции до указанной точки
			/// </summary>
			/// <param name="vector">Точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ComputeDistance(UnityEngine.Vector2 vector)
			{
				Vector2Df point = new Vector2Df(vector.x, vector.y);
				for (Int32 i = 0; i < mCount; i++)
				{
					mArrayOfItems[i].ComputeDistance(ref point);
				}
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение минимальной дистанции точки привязки
			/// </summary>
			/// <returns>Минимальная дистанция</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMinimumDistance()
			{
				Single minimum = Single.MaxValue;
				for (Int32 i = 0; i < mCount; i++)
				{
					if(mArrayOfItems[i].Distance < minimum)
					{
						minimum = mArrayOfItems[i].Distance;
					}
				}

				return (minimum);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса точки по минимальной дистанции
			/// </summary>
			/// <returns>Индекса точки привязки с минимальной дистанцией</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetMinimumDistanceIndex()
			{
				Single minimum = Single.MaxValue;
				Int32 index = 0;
				for (Int32 i = 0; i < mCount; i++)
				{
					if (mArrayOfItems[i].Distance < minimum)
					{
						minimum = mArrayOfItems[i].Distance;
						index = i;
					}
				}

				return (index);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОИСКА ИНДЕКСА =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса ближайшей точки на основании позиции
			/// </summary>
			/// <param name="point">Точка</param>
			/// <param name="delta_x">Допуск по координате X</param>
			/// <param name="delta_y">Допуск по координате Y</param>
			/// <returns>Найденный индекс или -1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexNearestFromPosition(Vector2Df point, Single delta_x, Single delta_y)
			{
				for (Int32 i = 0; i < mCount; i++)
				{
					if(mArrayOfItems[i].ApproximatelyPoint(ref point, delta_x, delta_y))
					{
						return (i);
					}
				}

				return (-1);
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса ближайшей точки на основании позиции
			/// </summary>
			/// <param name="vector">Точка</param>
			/// <param name="delta_x">Допуск по координате X</param>
			/// <param name="delta_y">Допуск по координате Y</param>
			/// <returns>Найденный индекс или -1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexNearestFromPosition(UnityEngine.Vector2 vector, Single delta_x, Single delta_y)
			{
				for (Int32 i = 0; i < mCount; i++)
				{
					if (mArrayOfItems[i].ApproximatelyPoint(ref vector, delta_x, delta_y))
					{
						return (i);
					}
				}

				return (-1);
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса ближайшей точки на основании позиции по X
			/// </summary>
			/// <param name="x">Координата точки по X</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Найденный индекс или -1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexNearestFromPositionX(Single x, Single epsilon)
			{
				for (Int32 i = 0; i < mCount; i++)
				{
					if (mArrayOfItems[i].ApproximatelyPointX(x, epsilon))
					{
						return (i);
					}
				}

				return (-1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса ближайшей точки на основании позиции по Y
			/// </summary>
			/// <param name="y">Координата точки по Y</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Найденный индекс или -1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexNearestFromPositionY(Single y, Single epsilon)
			{
				for (Int32 i = 0; i < mCount; i++)
				{
					if (mArrayOfItems[i].ApproximatelyPointY(y, epsilon))
					{
						return (i);
					}
				}

				return (-1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса ближайшей точки на основании дистанции
			/// </summary>
			/// <param name="distance">Дистанция</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Найденный индекс или -1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindIndexNearestFromDistance(Single distance, Single epsilon)
			{
				for (Int32 i = 0; i < mCount; i++)
				{
					if (mArrayOfItems[i].ApproximatelyDistance(distance, epsilon))
					{
						return (i);
					}
				}

				return (-1);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================