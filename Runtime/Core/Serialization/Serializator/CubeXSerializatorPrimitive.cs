//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializatorPrimitive.cs
*		Сериализация примитивных данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleSerialization
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий сериализацию примитивных данных в различные форматы
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSerializatorPrimitive
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			//
			// ИМЕНА МЕТОДОВ ДЛЯ ПРИМИТИВНЫХ ДАННЫХ
			//
			/// <summary>
			/// Имя статического метода типа который создает объект из переданной строки
			/// </summary>
			public const String DESERIALIZE_METHOD_STR = "DeserializeFromString";

			/// <summary>
			/// Имя метода типа который сериализует объект в строку и возвращает её
			/// </summary>
			public const String SERIALIZE_METHOD_STR = "SerializeToString";

			//
			// СТАНДАРТНЫЕ ПРИМИТИВНЫЕ ДАННЫЕ
			//
			/// <summary>
			/// Имена примитивных типов которые можно записать в одну строку
			/// </summary>
			public static readonly String[] PrimitiveTypeNames = new String[]
			{
				// Типы System
				nameof(Boolean),
				nameof(Byte),
				nameof(Char),
				nameof(Int16),
				nameof(UInt16),
				nameof(Int32),
				nameof(UInt32),
				nameof(Int64),
				nameof(UInt64),
				nameof(Single),
				nameof(Double),
				nameof(Decimal),
				nameof(String),
				nameof(DateTime),
				nameof(TimeSpan),
				nameof(Version),
				nameof(Uri),

				// Типы CubeX
				nameof(TColor),
				nameof(CVariant),
				nameof(CSettingItem),

				// Типы Unity
#if (UNITY_2017_1_OR_NEWER)
				nameof(UnityEngine.Vector2),
				nameof(UnityEngine.Vector3),
				nameof(UnityEngine.Vector4),
				nameof(UnityEngine.Vector2Int),
				nameof(UnityEngine.Vector3Int),
				nameof(UnityEngine.Quaternion),
				nameof(UnityEngine.Color),
				nameof(UnityEngine.Color32),
				nameof(UnityEngine.Rect),
				nameof(UnityEngine.RectInt),
				nameof(UnityEngine.Bounds),
				nameof(UnityEngine.BoundsInt),
#endif
			};
			#endregion

			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ XML ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на примитивный тип
			/// </summary>
			/// <param name="type">Проверяемый тип</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsPrimitiveType(Type type)
			{
				return (type.IsEnum || PrimitiveTypeNames.Contains(type.Name) || type.GetAttribute<CubeXSerializeAsPrimitiveAttribute>() != null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись объекта примитивного типа в формат атрибута XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="type">Тип объекта</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="attribute_name">Имя атрибута</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteToAttribute(XmlWriter writer, Type type, System.Object instance, String attribute_name)
			{
				switch (type.Name)
				{
					//
					//
					//
					case nameof(Boolean):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Boolean)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Byte):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Byte)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Char):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Char)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Int16):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Int16)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UInt16):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((UInt16)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Int32):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Int32)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UInt32):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((UInt32)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Int64):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Int64)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UInt64):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Int64)(UInt64)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Single):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Single)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Double):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Double)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Decimal):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((Decimal)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(String):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((String)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(DateTime):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue((DateTime)instance);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(TimeSpan):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue(((TimeSpan)instance).Ticks);
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Version):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue(((Version)instance).ToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(Uri):
						{
							writer.WriteStartAttribute(attribute_name);
							writer.WriteValue(((Uri)instance).ToString());
							writer.WriteEndAttribute();
						}
						break;
					//
					//
					//
					case nameof(TColor):
						{
							writer.WriteStartAttribute(attribute_name);
							TColor color = (TColor)instance;
							writer.WriteValue(color.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(CVariant):
						{
							writer.WriteStartAttribute(attribute_name);
							CVariant variant = (CVariant)instance;
							writer.WriteValue(variant.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(CSettingItem):
						{
							writer.WriteStartAttribute(attribute_name);
							CSettingItem setting = (CSettingItem)instance;
							writer.WriteValue(setting.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					//
					//
					//
#if (UNITY_2017_1_OR_NEWER)
					case nameof(UnityEngine.Vector2):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Vector2 vector = (UnityEngine.Vector2)instance;
							writer.WriteValue(vector.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Vector3):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Vector3 vector = (UnityEngine.Vector3)instance;
							writer.WriteValue(vector.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Vector4):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Vector4 vector = (UnityEngine.Vector4)instance;
							writer.WriteValue(vector.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Vector2Int):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Vector2Int vector = (UnityEngine.Vector2Int)instance;
							writer.WriteValue(vector.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Vector3Int):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Vector3Int vector = (UnityEngine.Vector3Int)instance;
							writer.WriteValue(vector.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Quaternion):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Quaternion quaternion = (UnityEngine.Quaternion)instance;
							writer.WriteValue(quaternion.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Color):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Color color = (UnityEngine.Color)instance;
							writer.WriteValue(color.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Color32):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Color32 color = (UnityEngine.Color32)instance;
							writer.WriteValue(color.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Rect):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Rect rect = (UnityEngine.Rect)instance;
							writer.WriteValue(rect.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.RectInt):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.RectInt rect = (UnityEngine.RectInt)instance;
							writer.WriteValue(rect.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.Bounds):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.Bounds bounds = (UnityEngine.Bounds)instance;
							writer.WriteValue(bounds.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
					case nameof(UnityEngine.BoundsInt):
						{
							writer.WriteStartAttribute(attribute_name);
							UnityEngine.BoundsInt bounds = (UnityEngine.BoundsInt)instance;
							writer.WriteValue(bounds.SerializeToString());
							writer.WriteEndAttribute();
						}
						break;
#endif
					default:
						{
							// Проверка на перечисление
							if (type.IsEnum)
							{
								writer.WriteStartAttribute(attribute_name);
								Enum enum_obj = (Enum)instance;

								writer.WriteValue(enum_obj.ToString());
								writer.WriteEndAttribute();
								break;
							}

							// Проверка на примитивный тип
							if(type.GetAttribute<CubeXSerializeAsPrimitiveAttribute>() != null)
							{
								MethodInfo method_info = type.GetMethod(SERIALIZE_METHOD_STR, BindingFlags.Public | BindingFlags.Instance);
								if(method_info != null)
								{
									String data = method_info.Invoke(instance, null).ToString();
									writer.WriteStartAttribute(attribute_name);
									writer.WriteValue(data);
									writer.WriteEndAttribute();
								}
							}
						}
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение объекта примитивного типа из формата атрибута XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="type">Тип объекта</param>
			/// <param name="attribute_name">Имя атрибута</param>
			/// <returns>Экземпляр объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object ReadFromAttribute(XmlReader reader, Type type, String attribute_name)
			{
				String value;
				switch (type.Name)
				{
					//
					//
					//
					case nameof(Boolean):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return (XBoolean.Parse(value));
							}
						}
						break;
					case nameof(Byte):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return Byte.Parse(value);
							}
						}
						break;
					case nameof(Char):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return Char.Parse(value);
							}
						}
						break;
					case nameof(Int16):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return Int16.Parse(value);
							}
						}
						break;
					case nameof(UInt16):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return UInt16.Parse(value);
							}
						}
						break;
					case nameof(Int32):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return Int32.Parse(value);
							}
						}
						break;
					case nameof(UInt32):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return UInt32.Parse(value);
							}
						}
						break;
					case nameof(Int64):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return Int64.Parse(value);
							}
						}
						break;
					case nameof(UInt64):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return UInt64.Parse(value);
							}
						}
						break;
					case nameof(Single):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XNumbers.ParseSingle(value);
							}
						}
						break;
					case nameof(Double):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XNumbers.ParseDouble(value);
							}
						}
						break;
					case nameof(Decimal):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XNumbers.ParseDecimal(value);
							}
						}
						break;
					case nameof(String):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return value;
							}
						}
						break;
					case nameof(DateTime):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return DateTime.Parse(value);
							}
						}
						break;
					case nameof(TimeSpan):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return TimeSpan.Parse(value);
							}
						}
						break;
					case nameof(Version):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return new Version(value);
							}
						}
						break;
					case nameof(Uri):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return new Uri(value);
							}
						}
						break;
					//
					// 
					//
					case nameof(TColor):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return TColor.DeserializeFromString(value);
							}
						}
						break;
					case nameof(CVariant):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return CVariant.DeserializeFromString(value);
							}
						}
						break;
					case nameof(CSettingItem):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return CSettingItem.DeserializeFromString(value);
							}
						}
						break;
					//
					//
					//
#if (UNITY_2017_1_OR_NEWER)
					case nameof(UnityEngine.Vector2):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityVector2.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Vector3):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityVector3.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Vector4):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityVector4.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Vector2Int):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityVector2Int.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Vector3Int):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityVector3Int.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Quaternion):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityQuaternion.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Color):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityColor.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Color32):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityColor32.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Rect):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityRect.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.RectInt):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityRectInt.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.Bounds):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityBounds.DeserializeFromString(value);
							}
						}
						break;
					case nameof(UnityEngine.BoundsInt):
						{
							if ((value = reader.GetAttribute(attribute_name)) != null)
							{
								return XUnityBoundsInt.DeserializeFromString(value);
							}
						}
						break;
#endif
					default:
						{
							// Проверка на перечисление
							if (type.IsEnum)
							{
								if ((value = reader.GetAttribute(attribute_name)) != null)
								{
									return Enum.Parse(type, value);
								}
							}

							// Проверка на примитивный тип
							if (type.GetAttribute<CubeXSerializeAsPrimitiveAttribute>() != null)
							{
								if ((value = reader.GetAttribute(attribute_name)) != null)
								{
									MethodInfo method_info = type.GetMethod(DESERIALIZE_METHOD_STR, BindingFlags.Public | BindingFlags.Static);
									if (method_info != null)
									{
										CReflectedType.ArgList1[0] = value;
										return (method_info.Invoke(null, CReflectedType.ArgList1));
									}
								}
							}
						}
						break;
				}

				return null;
			}
			#endregion

			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ Binary ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись объекта примитивного типа в бинарный поток
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="type">Тип объекта</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteToBinary(BinaryWriter writer, Type type, System.Object instance)
			{
				switch (type.Name)
				{
					//
					//
					//
					case nameof(Boolean):
						{
							writer.Write((Boolean)instance);
						}
						break;
					case nameof(Byte):
						{
							writer.Write((Byte)instance);
						}
						break;
					case nameof(Char):
						{
							writer.Write((Char)instance);
						}
						break;
					case nameof(Int16):
						{
							writer.Write((Int16)instance);
						}
						break;
					case nameof(UInt16):
						{
							writer.Write((UInt16)instance);
						}
						break;
					case nameof(Int32):
						{
							writer.Write((Int32)instance);
						}
						break;
					case nameof(UInt32):
						{
							writer.Write((UInt32)instance);
						}
						break;
					case nameof(Int64):
						{
							writer.Write((Int64)instance);
						}
						break;
					case nameof(UInt64):
						{
							writer.Write((Int64)(UInt64)instance);
						}
						break;
					case nameof(Single):
						{
							writer.Write((Single)instance);
						}
						break;
					case nameof(Double):
						{
							writer.Write((Double)instance);
						}
						break;
					case nameof(Decimal):
						{
							writer.Write((Decimal)instance);
						}
						break;
					case nameof(String):
						{
							writer.Write((String)instance);
						}
						break;
					case nameof(DateTime):
						{
							writer.Write((DateTime)instance);
						}
						break;
					case nameof(TimeSpan):
						{
							writer.Write(((TimeSpan)instance).Ticks);
						}
						break;
					case nameof(Version):
						{
							writer.Write(((Version)instance).ToString());
						}
						break;
					case nameof(Uri):
						{
							writer.Write(((Uri)instance).ToString());
						}
						break;
					//
					//
					//
					case nameof(TColor):
						{
							TColor color = (TColor)instance;
							writer.Write(color.ToRGBA());
						}
						break;
					case nameof(CVariant):
						{
							CVariant variant = (CVariant)instance;
							writer.Write(variant.SerializeToString());
						}
						break;
					case nameof(CSettingItem):
						{
							CSettingItem setting = (CSettingItem)instance;
							writer.Write(setting.SerializeToString());
						}
						break;
					//
					//
					//
#if (UNITY_2017_1_OR_NEWER)
					case nameof(UnityEngine.Vector2):
						{
							UnityEngine.Vector2 vector = (UnityEngine.Vector2)instance;
							writer.Write(vector.x);
							writer.Write(vector.y);
						}
						break;
					case nameof(UnityEngine.Vector3):
						{
							UnityEngine.Vector3 vector = (UnityEngine.Vector3)instance;
							writer.Write(vector.x);
							writer.Write(vector.y);
							writer.Write(vector.z);
						}
						break;
					case nameof(UnityEngine.Vector4):
						{
							UnityEngine.Vector4 vector = (UnityEngine.Vector4)instance;
							writer.Write(vector.x);
							writer.Write(vector.y);
							writer.Write(vector.z);
							writer.Write(vector.w);
						}
						break;
					case nameof(UnityEngine.Vector2Int):
						{
							UnityEngine.Vector2Int vector = (UnityEngine.Vector2Int)instance;
							writer.Write(vector.x);
							writer.Write(vector.y);
						}
						break;
					case nameof(UnityEngine.Vector3Int):
						{
							UnityEngine.Vector3Int vector = (UnityEngine.Vector3Int)instance;
							writer.Write(vector.x);
							writer.Write(vector.y);
							writer.Write(vector.z);
						}
						break;
					case nameof(UnityEngine.Quaternion):
						{
							UnityEngine.Quaternion quaternion = (UnityEngine.Quaternion)instance;
							writer.Write(quaternion.x);
							writer.Write(quaternion.y);
							writer.Write(quaternion.z);
							writer.Write(quaternion.w);
						}
						break;
					case nameof(UnityEngine.Color):
						{
							UnityEngine.Color color = (UnityEngine.Color)instance;
							writer.Write(color.ToRGBA());
						}
						break;
					case nameof(UnityEngine.Color32):
						{
							UnityEngine.Color32 color = (UnityEngine.Color32)instance;
							writer.Write(color.ToRGBA());
						}
						break;
					case nameof(UnityEngine.Rect):
						{
							UnityEngine.Rect rect = (UnityEngine.Rect)instance;
							writer.Write(rect.x);
							writer.Write(rect.y);
							writer.Write(rect.width);
							writer.Write(rect.height);
						}
						break;
					case nameof(UnityEngine.RectInt):
						{
							UnityEngine.RectInt rect = (UnityEngine.RectInt)instance;
							writer.Write(rect.x);
							writer.Write(rect.y);
							writer.Write(rect.width);
							writer.Write(rect.height);
						}
						break;
					case nameof(UnityEngine.Bounds):
						{
							UnityEngine.Bounds bounds = (UnityEngine.Bounds)instance;
							writer.Write(bounds.min.x);
							writer.Write(bounds.min.y);
							writer.Write(bounds.min.z);
							writer.Write(bounds.max.x);
							writer.Write(bounds.max.y);
							writer.Write(bounds.max.z);
						}
						break;
					case nameof(UnityEngine.BoundsInt):
						{
							UnityEngine.BoundsInt bounds = (UnityEngine.BoundsInt)instance;
							writer.Write(bounds.min.x);
							writer.Write(bounds.min.y);
							writer.Write(bounds.min.z);
							writer.Write(bounds.max.x);
							writer.Write(bounds.max.y);
							writer.Write(bounds.max.z);
						}
						break;
#endif
					default:
						{
							// Проверка на перечисление
							if (type.IsEnum)
							{
								writer.Write((Int32)instance);
								break;
							}

							// Проверка на примитивный тип
							if (type.GetAttribute<CubeXSerializeAsPrimitiveAttribute>() != null)
							{
								MethodInfo method_info = type.GetMethod(SERIALIZE_METHOD_STR, BindingFlags.Public | BindingFlags.Instance);
								if (method_info != null)
								{
									String data = method_info.Invoke(instance, null).ToString();
									writer.Write(data);
								}
							}
						}
						break;
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================