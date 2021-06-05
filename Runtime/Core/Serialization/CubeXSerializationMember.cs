//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializationMember.cs
*		Определение типов и структур данных для сериализации члена данных объекта.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
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
		/// Дополнительные параметры члена объекта при его сериализации
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Flags]
		public enum TSerializeMemberOption
		{
			/// <summary>
			/// Дополнительных параметров нет
			/// </summary>
			None = 0,

			/// <summary>
			/// Только если имеются атрибут сериализации
			/// </summary>
			SerializationAttribute = 1,

			/// <summary>
			/// Только если имеются атрибут сериализации Xml
			/// </summary>
			XmlAttribute = 2,

#if UNITY_2017_1_OR_NEWER

			/// <summary>
			/// Только если имеется атрибут сериализации поля для Unity
			/// </summary>
			SerializeField = 4,
#endif

#if UNITY_2017_1_OR_NEWER
			/// <summary>
			/// Любой из атрибутов
			/// </summary>
			AnyAttributes = SerializationAttribute | XmlAttribute | SerializeField
#else
			/// <summary>
			/// Любой из атрибутов
			/// </summary>
			AnyAttributes = SerializationAttribute | XmlAttribute
#endif

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип члена объекта с точки зрения сериализации
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TSerializeMemberType
		{
			/// <summary>
			/// Примитивный тип
			/// </summary>
			/// <remarks>
			/// Примитивный тип это самый простой тип с точки зрения сериализации
			/// </remarks>
			Primitive = 0,

			/// <summary>
			/// Структура
			/// </summary>
			/// <remarks>
			/// Структура это составной тип который передаётся через значение поэтому его всегда надо устанавливать
			/// </remarks>
			Struct = 1,

			/// <summary>
			/// Класс
			/// </summary>
			/// <remarks>
			/// Класс это составной тип который передаётся через ссылку, поэтому его можно попробовать
			/// получить через ссылку и уже обновлять существующий объект
			/// </remarks>
			Class = 2,

			/// <summary>
			/// Список или массив
			/// </summary>
			/// <remarks>
			/// Список или массив это составной объект которые записывает свои элементы
			/// </remarks>
			List = 3,

			/// <summary>
			/// Специальный тип коллекции для хранения моделей
			/// </summary>
			/// <remarks>
			/// Список или массив это составной объект которые записывает свои элементы
			/// </remarks>
			ListModels = 4,

			/// <summary>
			/// Словарь
			/// </summary>
			/// <remarks>
			/// Словарь это составной объект которые записывает свои элементы
			/// </remarks>
			Dictionary = 5,

			/// <summary>
			/// Ссылка
			/// </summary>
			/// <remarks>
			/// Ссылка это определение того надо записывать не сам объект и лишь его ссылочные данные, 
			/// она допустима только для классов и других ссылочных типов.
			/// Для определения ссылки используется атрибут <see cref="CubeXSerializeMemberAsReferenceAttribute"/>
			/// </remarks>
			Reference = 6,

#if UNITY_2017_1_OR_NEWER

			/// <summary>
			/// Ссылка на компонент Unity
			/// </summary>
			UnityComponent = 10,

			/// <summary>
			/// Ссылка на пользовательский скрипт Unity
			/// </summary>
			UnityUserComponent = 11,

			/// <summary>
			/// Ссылка на игровой объект Unity
			/// </summary>
			UnityGameObject = 12,

			/// <summary>
			/// Ссылка на ресурс Unity
			/// </summary>
			UnityResource = 13,

			/// <summary>
			/// Ссылка на пользовательский ресурс Unity
			/// </summary>
			UnityUserResource = 14
#endif
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Данные для сериализации члена объекта
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public struct TSerializeDataMember : IEquatable<TSerializeDataMember>, IComparable<TSerializeDataMember>
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Пустые данные для сериализации (используется как заглушка)
			/// </summary>
			public static readonly TSerializeDataMember Empty = new TSerializeDataMember();

			/// <summary>
			/// Текстовый формат отображения параметров для сериализации члена объекта
			/// </summary>
			public static String ToStringFormat = "{0}={1}";
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисления тип члена объекта с точки зрения сериализации
			/// </summary>
			/// <param name="member_info">Член данных</param>
			/// <returns>Тип члена объекта с точки зрения сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TSerializeMemberType ComputeSerializeMemberType(MemberInfo member_info)
			{
				Type member_type = member_info.GetMemberType();

				// Примитивный тип
				if (member_type.IsPrimitiveType() || XSerializatorPrimitive.IsPrimitiveType(member_type))
				{
					return (TSerializeMemberType.Primitive);
				}

#if UNITY_2017_1_OR_NEWER

				// Компоненты
				if (member_type.IsUnityComponentType())
				{
					if (member_type.IsUnityModule())
					{
						return (TSerializeMemberType.UnityComponent);
					}
					else
					{
						return (TSerializeMemberType.UnityUserComponent);
					}
					
				}

				// Игровой объект
				if (member_type.IsUnityGameObjectType())
				{
					return (TSerializeMemberType.UnityGameObject);
				}

				// Ресурс
				if (member_type.IsUnityResourceType())
				{
					if (member_type.IsUnityModule())
					{
						return (TSerializeMemberType.UnityResource);
					}
					else
					{
						return (TSerializeMemberType.UnityUserResource);
					}
				}

#endif
				if (member_type.IsStructType())
				{
					return (TSerializeMemberType.Struct);
				}

				if (member_type.IsClassType())
				{
					return (TSerializeMemberType.Class);
				}

				if (member_type.IsClassicCollectionType())
				{
					return (TSerializeMemberType.List);
				}

				if (member_type.IsCollectionModel())
				{
					return (TSerializeMemberType.ListModels);
				}

				if (member_type.IsDictionaryType())
				{
					return (TSerializeMemberType.Dictionary);
				}

				if (member_info.GetAttribute<CubeXSerializeMemberAsReferenceAttribute>() != null)
				{
					return (TSerializeMemberType.Reference);
				}

				return (TSerializeMemberType.Primitive);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Имя члена объекта под которыми оно используется для сериализации
			/// </summary>
			public String NameSerialize;

			/// <summary>
			/// Метаданные член объекта
			/// </summary>
			public MemberInfo MemberData;

			/// <summary>
			/// Тип члена объекта с точки зрения сериализации
			/// </summary>
			public TSerializeMemberType SerializeType;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя данных для сериализации
			/// </summary>
			/// <remarks>
			/// Если имя специально имя не установлено то используется имя члена объекта
			/// </remarks>
			public String Name
			{
				get
				{
					if (String.IsNullOrEmpty(NameSerialize))
					{
						return (MemberData.Name);
					}
					else
					{
						return (NameSerialize);
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект указанными значениями
			/// </summary>
			/// <param name="name_serialize">Имя члена объекта под которыми оно используется для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public TSerializeDataMember(String name_serialize = null)
			{
				MemberData = null;
				NameSerialize = name_serialize;
				SerializeType = TSerializeMemberType.Primitive;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект указанными значениями
			/// </summary>
			/// <param name="member_data">Метаданные члена объекта</param>
			/// <param name="name_serialize">Имя члена объекта под которыми оно используется для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public TSerializeDataMember(MemberInfo member_data, String name_serialize = null)
			{
				MemberData = member_data;
				NameSerialize = name_serialize;
				SerializeType = TSerializeMemberType.Primitive;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект указанными значениями
			/// </summary>
			/// <param name="member_data">Метаданные члена объекта</param>
			/// <param name="serialize_type">Тип члена объекта с точки зрения сериализации </param>
			/// <param name="name_serialize">Имя члена объекта под которыми оно используется для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public TSerializeDataMember(MemberInfo member_data, TSerializeMemberType serialize_type, String name_serialize = null)
			{
				MemberData = member_data;
				SerializeType = serialize_type;
				NameSerialize = name_serialize;
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
					if (typeof(TSerializeDataMember) == obj.GetType())
					{
						TSerializeDataMember value = (TSerializeDataMember)obj;
						return Equals(value);
					}
				}
				return base.Equals(obj);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка равенства объектов по значению
			/// </summary>
			/// <param name="other">Сравниваемый объект</param>
			/// <returns>Статус равенства объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Equals(TSerializeDataMember other)
			{
				return MemberData == other.MemberData;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение данных для упорядочивания элементов
			/// </summary>
			/// <param name="other">Данные для сериализации члена объекта</param>
			/// <returns>Статус сравнения данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(TSerializeDataMember other)
			{
				Int32 order_1 = (Int32)(SerializeType);
				Int32 order_2 = (Int32)(other.SerializeType);
				if (order_1 > order_2)
				{
					return 1;
				}
				else
				{
					if (order_1 < order_2)
					{
						return -1;
					}
					else
					{
						return 0;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода объекта
			/// </summary>
			/// <returns>Хеш-код объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return MemberData.GetHashCode() ^ base.GetHashCode();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление данных для сериализации члена объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return String.Format(ToStringFormat, MemberData.Name, GetMemberType());
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
			public static Boolean operator ==(TSerializeDataMember left, TSerializeDataMember right)
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
			public static Boolean operator !=(TSerializeDataMember left, TSerializeDataMember right)
			{
				return !(left == right);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение типа члена объекта (в контексте получения данных)
			/// </summary>
			/// <returns>Соответствующий тип члена объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public Type GetMemberType()
			{
				Type result = null;

				switch (MemberData.MemberType)
				{
					case MemberTypes.All:
						break;
					case MemberTypes.Constructor:
						break;
					case MemberTypes.Custom:
						break;
					case MemberTypes.Event:
						break;
					case MemberTypes.Field:
						{
							result = ((FieldInfo)MemberData).FieldType;
						}
						break;
					case MemberTypes.Method:
						{

						}
						break;
					case MemberTypes.NestedType:
						{

						}
						break;
					case MemberTypes.Property:
						{
							result = ((PropertyInfo)MemberData).PropertyType;
						}
						break;
					case MemberTypes.TypeInfo:
						{

						}
						break;
					default:
						{

						}
						break;
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения члена объекта (в контексте получения данных от свойства/поля)
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object GetMemberValue(System.Object instance)
			{
				System.Object result = null;

				switch (MemberData.MemberType)
				{
					case MemberTypes.All:
						break;
					case MemberTypes.Constructor:
						break;
					case MemberTypes.Custom:
						break;
					case MemberTypes.Event:
						break;
					case MemberTypes.Field:
						{
							result = ((FieldInfo)MemberData).GetValue(instance);
						}
						break;
					case MemberTypes.Method:
						{

						}
						break;
					case MemberTypes.NestedType:
						break;
					case MemberTypes.Property:
						{
							result = ((PropertyInfo)MemberData).GetValue(instance, null);
						}
						break;
					case MemberTypes.TypeInfo:
						break;
					default:
						{

						}
						break;
				}

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка данных члена объекта (в контексте установки данных)
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="value">Значение</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetMemberValue(System.Object instance, System.Object value)
			{
				if (instance == null)
				{
					String error = String.Format("Instance = NULL, Field name: [{0}] = Value: [{1}]", MemberData.Name, value.ToString());
#if UNITY_2017_1_OR_NEWER
					UnityEngine.Debug.LogError(error);
#else
					XLogger.LogErrorModule(nameof(XSerializationDispatcher), error);
#endif
					return;
				}

				switch (MemberData.MemberType)
				{
					case MemberTypes.All:
						break;
					case MemberTypes.Constructor:
						break;
					case MemberTypes.Custom:
						break;
					case MemberTypes.Event:
						break;
					case MemberTypes.Field:
						{
							((FieldInfo)MemberData).SetValue(instance, value);
						}
						break;
					case MemberTypes.Method:
						{

						}
						break;
					case MemberTypes.NestedType:
						break;
					case MemberTypes.Property:
						{
							((PropertyInfo)MemberData).SetValue(instance, value, null);
						}
						break;
					case MemberTypes.TypeInfo:
						break;
					default:
						{

						}
						break;
				}
			}

#if UNITY_2017_1_OR_NEWER
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на принадлежность типа к типу игрового объекта Unity
			/// </summary>
			/// <returns>Статус принадлежности</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsUnityGameObject()
			{
				return (MemberData.GetMemberType().IsUnityGameObjectType());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на принадлежность типа к типу компонента Unity
			/// </summary>
			/// <returns>Статус принадлежности</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsUnityComponent()
			{
				return (MemberData.GetMemberType().IsUnityComponentType());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на принадлежность типа к типу ресурса Unity
			/// </summary>
			/// <returns>Статус принадлежности</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsUnityResource()
			{
				return (MemberData.GetMemberType().IsUnityResourceType());
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================