//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXExtensionUnityMath.cs
*		Работа с математическими структурами данных в Unity.
*		Реализация дополнительных методов для работы с векторами, кватернионом и другими математическими структурами в Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		//! \addtogroup CoreModuleBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для расширения функциональности типа Vector2
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnityVector2
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация двухмерного вектора из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Двухмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 DeserializeFromString(String data)
			{
				Vector2 vector;
				String[] vector_data = data.Split(';');
				vector.x = XNumbers.ParseSingle(vector_data[0]);
				vector.y = XNumbers.ParseSingle(vector_data[1]);
				return vector;
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в объект вектора из базового объекта
			/// </summary>
			/// <remarks>
			/// Метод анализирует реальный тип объекта и пробует соответственным образом выполнить преобразование
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Вектор</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector2 ToVector(System.Object value)
			{
				if (value is Vector2)
				{
					return ((Vector2)value);
				}
				else
				{
					if (value is String)
					{
						return (DeserializeFromString((String)value));
					}
					else
					{
						if (value is Vector2Int)
						{
							Vector2Int vector = (Vector2Int)value;
							return (new Vector2(vector.x, vector.y));
						}
					}
				}

				return (Vector2.zero);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для расширения функциональности типа Vector2Int
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnityVector2Int
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация двухмерного вектора из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Двухмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2Int DeserializeFromString(String data)
			{
				Vector2Int vector = Vector2Int.zero;
				String[] vector_data = data.Split(';');
				vector.x = XNumbers.ParseInt(vector_data[0]);
				vector.y = XNumbers.ParseInt(vector_data[1]);
				return vector;
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в объект вектора из базового объекта
			/// </summary>
			/// <remarks>
			/// Метод анализирует реальный тип объекта и пробует соответственным образом выполнить преобразование
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Вектор</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector2Int ToVector(System.Object value)
			{
				if (value is Vector2Int)
				{
					return ((Vector2Int)value);
				}
				else
				{
					if (value is String)
					{
						return (DeserializeFromString((String)value));
					}
					else
					{
						if (value is Vector2)
						{
							Vector2 vector = (Vector2)value;
							return (vector.ToInt());
						}
					}
				}

				return (Vector2Int.zero);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для расширения функциональности типа Vector3
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnityVector3
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация трехмерного вектора из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Трехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 DeserializeFromString(String data)
			{
				Vector3 vector;
				String[] vector_data = data.Split(';');
				vector.x = XNumbers.ParseSingle(vector_data[0]);
				vector.y = XNumbers.ParseSingle(vector_data[1]);
				vector.z = XNumbers.ParseSingle(vector_data[2]);
				return vector;
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в объект вектора из базового объекта
			/// </summary>
			/// <remarks>
			/// Метод анализирует реальный тип объекта и пробует соответственным образом выполнить преобразование
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Вектор</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector3 ToVector(System.Object value)
			{
				if (value is Vector3)
				{
					return ((Vector3)value);
				}
				else
				{
					if (value is String)
					{
						return (DeserializeFromString((String)value));
					}
					else
					{
						if (value is Vector3Int)
						{
							Vector3Int vector = (Vector3Int)value;
							return (new Vector3(vector.x, vector.y, vector.z));
						}
					}
				}

				return (Vector3.zero);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для расширения функциональности типа Vector3Int
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnityVector3Int
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация трехмерного вектора из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Трехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3Int DeserializeFromString(String data)
			{
				Vector3Int vector = Vector3Int.zero;
				String[] vector_data = data.Split(';');
				vector.x = XNumbers.ParseInt(vector_data[0]);
				vector.y = XNumbers.ParseInt(vector_data[1]);
				vector.z = XNumbers.ParseInt(vector_data[2]);
				return vector;
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в объект вектора из базового объекта
			/// </summary>
			/// <remarks>
			/// Метод анализирует реальный тип объекта и пробует соответственным образом выполнить преобразование
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Вектор</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector3Int ToVector(System.Object value)
			{
				if (value is Vector3Int)
				{
					return ((Vector3Int)value);
				}
				else
				{
					if (value is String)
					{
						return (DeserializeFromString((String)value));
					}
					else
					{
						if (value is Vector3)
						{
							Vector3 vector = (Vector3)value;
							return (vector.ToInt());
						}
					}
				}

				return (Vector3Int.zero);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для расширения функциональности типа Vector4
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnityVector4
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация четырехмерного вектора из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Четырехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector4 DeserializeFromString(String data)
			{
				Vector4 vector;
				String[] vector_data = data.Split(';');
				vector.x = XNumbers.ParseSingle(vector_data[0]);
				vector.y = XNumbers.ParseSingle(vector_data[1]);
				vector.z = XNumbers.ParseSingle(vector_data[2]);
				vector.w = XNumbers.ParseSingle(vector_data[3]);
				return vector;
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в объект вектора из базового объекта
			/// </summary>
			/// <remarks>
			/// Метод анализирует реальный тип объекта и пробует соответственным образом выполнить преобразование
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Вектор</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Vector4 ToVector(System.Object value)
			{
				if (value is Vector4)
				{
					return ((Vector4)value);
				}
				else
				{
					if (value is String)
					{
						return (DeserializeFromString((String)value));
					}
					else
					{
						if (value is Vector3)
						{
							Vector3 vector = (Vector3)value;
							return (new Vector4(vector.x, vector.y, vector.z));
						}
					}
				}

				return (Vector4.zero);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для расширения функциональности типа Quaternion
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnityQuaternion
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация кватерниона из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Кватернион</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Quaternion DeserializeFromString(String data)
			{
				Quaternion quaternion;
				String[] vector_data = data.Split(';');
				quaternion.x = XNumbers.ParseSingle(vector_data[0]);
				quaternion.y = XNumbers.ParseSingle(vector_data[1]);
				quaternion.z = XNumbers.ParseSingle(vector_data[2]);
				quaternion.w = XNumbers.ParseSingle(vector_data[3]);
				return quaternion;
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в объект кватерниона из базового объекта
			/// </summary>
			/// <remarks>
			/// Метод анализирует реальный тип объекта и пробует соответственным образом выполнить преобразование
			/// </remarks>
			/// <param name="value">Значение</param>
			/// <returns>Кватернион</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Quaternion ToQuaternion(System.Object value)
			{
				if (value is Quaternion)
				{
					return ((Quaternion)value);
				}
				else
				{
					if (value is String)
					{
						return (DeserializeFromString((String)value));
					}
					else
					{
						if (value is Vector4)
						{
							Vector4 vector = (Vector4)value;
							return (new Quaternion(vector.x, vector.y, vector.z, vector.w));
						}
					}
				}

				return (Quaternion.identity);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================