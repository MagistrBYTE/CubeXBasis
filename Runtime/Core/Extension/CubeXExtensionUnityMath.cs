//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXExtensionUnityMath.cs
*		Методы расширения функциональности векторных и математических типов Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleExtension
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Vector2"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityVector2
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация вектора в строку
			/// </summary>
			/// <param name="this">Двухмерный вектор</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Vector2 @this)
			{
				return String.Format("{0};{1}", @this.x, @this.y);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений двухмерных векторов
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <param name="other">Сравниваемый вектор</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(this Vector2 @this, Vector2 other, Single epsilon = 0.01f)
			{
				if (Mathf.Abs(@this.x - other.x) < epsilon && Mathf.Abs(@this.y - other.y) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на равенство значений векторов по углу между ними
			/// </summary>
			/// <remarks>
			/// Credits: http://answers.unity3d.com/questions/131624/vector3-comparison.html#answer-131672
			/// </remarks>
			/// <param name="this">Исходный вектор</param>
			/// <param name="other">Сравниваемый вектор</param>
			/// <param name="angle_error">Погрешность угла</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ApproximatelyFromAngle(this Vector2 @this, Vector2 other, Single angle_error = 0.01f)
			{
				//if they aren't the same length, don't bother checking the rest.
				if (!Mathf.Approximately(@this.magnitude, other.magnitude))
				{
					return false;
				}

				var cos_angle_error = Mathf.Cos(angle_error * Mathf.Deg2Rad);

				//A value between -1 and 1 corresponding to the angle.
				//The dot product of normalized Vectors is equal to the cosine of the angle between them.
				//So the closer they are, the closer the value will be to 1. Opposite Vectors will be -1
				//and orthogonal Vectors will be 0.
				var cos_angle = Vector2.Dot(@this.normalized, other.normalized);

				//If angle is greater, that means that the angle between the two vectors is less than the error allowed.
				return cos_angle >= cos_angle_error;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращение перпендикулярного вектора расположенного против часовой стрелки
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <returns>Перпендикулярный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 PerpToCCW(this Vector2 @this)
			{
				return new Vector2(-@this.y, @this.x);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращение перпендикулярного вектора расположенного по часовой стрелки
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <returns>Перпендикулярный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 PerpToCW(this Vector2 @this)
			{
				return new Vector2(@this.y, -@this.x);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращение единичного перпендикулярного вектора расположенного против часовой стрелки
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <returns>Перпендикулярный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 UnitPerpToCCW(this Vector2 @this)
			{
				return new Vector2(-@this.y, @this.x)/ @this.magnitude;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращение единичного перпендикулярного вектора расположенного по часовой стрелки
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <returns>Перпендикулярный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 UnitPerpToCW(this Vector2 @this)
			{
				return new Vector2(@this.y, -@this.x) / @this.magnitude;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращение скалярного произведения с перпендикулярным вектором
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <param name="vector">Вектор</param>
			/// <returns>Скалярное произведение с перпендикулярным вектором</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single DotPerp(this Vector2 @this, Vector2 vector)
			{
				return @this.x * vector.y - @this.y * vector.x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возвращение скалярного произведения с перпендикулярным вектором
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <param name="vector">Вектор</param>
			/// <returns>Скалярное произведение с перпендикулярным вектором</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single DotPerp(this Vector2 @this, ref Vector2 vector)
			{
				return @this.x * vector.y - @this.y * vector.x;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в целочисленный вектор
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <returns>Целочисленный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2Int ToInt(this Vector2 @this)
			{
				return (new Vector2Int((Int32)@this.x, (Int32)@this.y));
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Vector2Int"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityVector2Int
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация вектора в строку
			/// </summary>
			/// <param name="this">Двухмерный вектор</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Vector2Int @this)
			{
				return String.Format("{0};{1}", @this.x, @this.y);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Vector3"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityVector3
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация вектора в строку
			/// </summary>
			/// <param name="this">Трехмерный вектор</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Vector3 @this)
			{
				return String.Format("{0};{1};{2}", @this.x, @this.y, @this.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений векторов
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <param name="other">Сравниваемый вектор</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(this Vector3 @this, Vector3 other, Single epsilon = 0.01f)
			{
				return Math.Abs(@this.x - other.x) < epsilon && Math.Abs(@this.y - other.y) < epsilon
					&& Math.Abs(@this.z - other.z) < epsilon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений векторов в плоскости XZ
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <param name="other">Сравниваемый вектор</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ApproximatelyPlaneXZ(this Vector3 @this, Vector3 other, Single epsilon = 0.01f)
			{
				return Math.Abs(@this.x - other.x) < epsilon && Math.Abs(@this.z - other.z) < epsilon;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на равенство значений векторов по углу между ними
			/// </summary>
			/// <remarks>
			/// Credits: http://answers.unity3d.com/questions/131624/vector3-comparison.html#answer-131672
			/// </remarks>
			/// <param name="this">Исходный вектор</param>
			/// <param name="other">Сравниваемый вектор</param>
			/// <param name="angle_error">Погрешность угла</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ApproximatelyFromAngle(this Vector3 @this, Vector3 other, Single angle_error = 0.01f)
			{
				//if they aren't the same length, don't bother checking the rest.
				if (!Mathf.Approximately(@this.magnitude, other.magnitude))
				{
					return false;
				}

				var cos_angle_error = Mathf.Cos(angle_error * Mathf.Deg2Rad);

				//A value between -1 and 1 corresponding to the angle.
				//The dot product of normalized Vectors is equal to the cosine of the angle between them.
				//So the closer they are, the closer the value will be to 1. Opposite Vectors will be -1
				//and orthogonal Vectors will be 0.
				var cos_angle = Vector3.Dot(@this.normalized, other.normalized);

				//If angle is greater, that means that the angle between the two vectors is less than the error allowed.
				return cos_angle >= cos_angle_error;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в целочисленный вектор
			/// </summary>
			/// <param name="this">Исходный вектор</param>
			/// <returns>Целочисленный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3Int ToInt(this Vector3 @this)
			{
				return (new Vector3Int((Int32)@this.x, (Int32)@this.y, (Int32)@this.z));
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Vector3Int"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityVector3Int
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация вектора в строку
			/// </summary>
			/// <param name="this">Трехмерный вектор</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Vector3Int @this)
			{
				return String.Format("{0};{1};{2}", @this.x, @this.y, @this.z);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Vector4"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityVector4
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация вектора в строку
			/// </summary>
			/// <param name="this">Четырехмерный вектор</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Vector4 @this)
			{
				return String.Format("{0};{1};{2};{3}", @this.x, @this.y, @this.z, @this.w);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Quaternion"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityQuaternion
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация кватерниона в строку
			/// </summary>
			/// <param name="this">Кватернион</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Quaternion @this)
			{
				return String.Format("{0};{1};{2};{3}", @this.x, @this.y, @this.z, @this.w);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================