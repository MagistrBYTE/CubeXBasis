//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializationData.cs
*		Определение типов и структур данных которые обеспечивают данные для сериализации конкретных типов.
*		Реализация типов и структур данных которые обеспечивают совокупность данных и параметров необходимых для 
*	сериализации объекта.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		/// Тип данных с точки зрения сериализации
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TSerializeDataType
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

#if UNITY_2017_1_OR_NEWER

			/// <summary>
			/// Ссылка на компонент Unity
			/// </summary>
			UnityComponent = 3,

			/// <summary>
			/// Ссылка на пользовательский скрипт Unity
			/// </summary>
			UnityUserComponent = 4,

			/// <summary>
			/// Ссылка на игровой объект Unity
			/// </summary>
			UnityGameObject = 5,

			/// <summary>
			/// Ссылка на ресурс Unity
			/// </summary>
			UnityResource = 6,

			/// <summary>
			/// Ссылка на пользовательский ресурс Unity
			/// </summary>
			UnityUserResource = 7
#endif
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения членов объектов(полей/свойств) которые подлежат сериализации
		/// </summary>
		/// <remarks>
		/// По умолчанию для каждого типа берется свойства и поля для сериализации в соответствии с правилами и настройками.
		/// Также можно для каждого типа определить объем сериализуемых данных
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CSerializeData
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// ПОЛЬЗОВАТЕЛЬСКИЕ ТИПЫ
			//
			//
			// СТРУКТУРЫ
			//
			/// <summary>
			/// Набор флагов извлечения данных для полей структур
			/// </summary>
			public static BindingFlags BindingFlagsUserStructField = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор флагов извлечения данных для свойств структур
			/// </summary>
			public static BindingFlags BindingFlagsUserStructProperty = BindingFlags.Public | BindingFlags.Instance;

			//
			// КЛАССЫ
			//
			/// <summary>
			/// Набор флагов извлечения данных для полей класса
			/// </summary>
			public static BindingFlags BindingFlagsUserClassField = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор флагов извлечения данных для свойств класса
			/// </summary>
			public static BindingFlags BindingFlagsUserClassProperty = BindingFlags.Public | BindingFlags.Instance;

#if (UNITY_2017_1_OR_NEWER)
			//
			// ТИПЫ Unity
			//
			//
			// СТРУКТУРЫ
			//
			/// <summary>
			/// Набор флагов извлечения данных для полей структур Unity
			/// </summary>
			public static BindingFlags BindingFlagsUnityStructField = BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор флагов извлечения данных для свойств структур Unity
			/// </summary>
			public static BindingFlags BindingFlagsUnityStructProperty = BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор имен полей структур Unity которые будут игнорироваться в любом случае
			/// </summary>
			public static List<String> IgnoreUnityStructFields = new List<String>();

			/// <summary>
			/// Набор имен свойств структур Unity которые будут игнорироваться в любом случае
			/// </summary>
			public static List<String> IgnoreUnityStructProperties = new List<String>();

			//
			// КЛАССЫ
			//
			/// <summary>
			/// Набор флагов извлечения данных для полей класса Unity
			/// </summary>
			public static BindingFlags BindingFlagsUnityClassField = BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор флагов извлечения данных для свойств класса Unity
			/// </summary>
			public static BindingFlags BindingFlagsUnityClassProperty = BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор имен полей класса Unity которые будут игнорироваться в любом случае
			/// </summary>
			public static List<String> IgnoreUnityClassFields = new List<String>();

			/// <summary>
			/// Набор имен свойств класса Unity которые будут игнорироваться в любом случае
			/// </summary>
			public static List<String> IgnoreUnityClassProperties = new List<String>()
			{
#if UNITY_EDITOR
				nameof(UnityEngine.GUIStyleState.scaledBackgrounds)
#endif
			};

			//
			// КОМПОНЕНТЫ
			//
			/// <summary>
			/// Набор флагов извлечения данных для свойств компонентов Unity
			/// </summary>
			public static BindingFlags BindingFlagsUnityComponentProperty = BindingFlags.Public | BindingFlags.Instance;

			/// <summary>
			/// Набор имен свойств компонентов Unity которые будут игнорироваться в любом случае
			/// </summary>
			public static List<String> IgnoreUnityComponentProperties = new List<String>()
			{
				nameof(UnityEngine.Component.name),
				nameof(UnityEngine.Component.tag),
				nameof(UnityEngine.Component.hideFlags),
				nameof(UnityEngine.MeshFilter.mesh),
				nameof(UnityEngine.Renderer.material),
				nameof(UnityEngine.Renderer.materials),
				nameof(UnityEngine.MonoBehaviour.useGUILayout)
			};
#endif
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисления тип данных с точки зрения сериализации
			/// </summary>
			/// <param name="type">Тип</param>
			/// <returns>Тип данных с точки зрения сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TSerializeDataType ComputeSerializeDataType(Type type)
			{
				// Примитивный тип
				if(type.IsPrimitiveType() || XSerializatorPrimitive.IsPrimitiveType(type))
				{
					return (TSerializeDataType.Primitive);
				}

#if UNITY_2017_1_OR_NEWER

				// Компоненты
				if (type.IsUnityComponentType())
				{
					if (type.IsUnityModule())
					{
						return (TSerializeDataType.UnityComponent);
					}
					else
					{
						return (TSerializeDataType.UnityUserComponent);
					}
				}

				// Игровой объект
				if (type.IsUnityGameObjectType())
				{
					return (TSerializeDataType.UnityGameObject);
				}

				// Ресурс
				if (type.IsUnityResourceType())
				{
					if (type.IsUnityModule())
					{
						return (TSerializeDataType.UnityResource);
					}
					else
					{
						return (TSerializeDataType.UnityUserResource);
					}
				}

#endif
				if (type.IsStructType())
				{
					return (TSerializeDataType.Struct);
				}

				if (type.IsClassType())
				{
					return (TSerializeDataType.Class);
				}

				return (TSerializeDataType.Primitive);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание данных для сериализации без членов объекта
			/// </summary>
			/// <remarks>
			/// Применяется для тех типов объекты которых представлены непосредственно значением
			/// </remarks>
			/// <param name="type">Тип</param>
			/// <returns>Данные для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData CreateNoMembers(Type type)
			{
				CSerializeData serialize_data = new CSerializeData(type, true);
				return (serialize_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание данных для сериализации пользовательской структуры по умолчанию
			/// </summary>
			/// <param name="type">Пользовательский тип</param>
			/// <returns>Данные для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData CreateForUserStructType(Type type)
			{
				// Если тип может сам себя записать/прочитать то просто добавляем тип
				if (type.IsSupportInterface<ICubeXSerializeImplementationXML>())
				{
					CSerializeData serialize_data = new CSerializeData(type, true);
					return (serialize_data);
				}
				else
				{
					CSerializeData serialize_data = new CSerializeData(type);

					// Добавляем поля
					serialize_data.AddFields(BindingFlagsUserStructField, TSerializeMemberOption.AnyAttributes);

					// Добавляем свойства
					serialize_data.AddProperties(BindingFlagsUserStructProperty, TSerializeMemberOption.AnyAttributes);

					if (serialize_data.Members.Count != 0)
					{
						// Сортируем
						serialize_data.Members.Sort();
					}
					else
					{
						// У нас нет членов объекта для сериализации - странная ситуация)))
						if (type.IsCubeXPlatformType())
						{
#if (UNITY_2017_1_OR_NEWER)
							//UnityEngine.Debug.LogErrorFormat("For CubeX type <{0}> members = 0", type.Name);
#else
							//XLogger.LogErrorFormatModule(XSerializationDispatcher.MODULE_NAME, "For CubeX type <{0}> members = 0", type.Name);
#endif
							return (null);
						}
					}

					return (serialize_data);
				}
				
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание данных для сериализации пользовательского класса по умолчанию
			/// </summary>
			/// <param name="type">Пользовательский тип</param>
			/// <returns>Данные для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData CreateForUserClassType(Type type)
			{
				// Если тип может сам себя записать/прочитать то ничего то просто добавляем тип
				if (type.IsSupportInterface<ICubeXSerializeImplementationXML>())
				{
					CSerializeData serialize_data = new CSerializeData(type, true);
					return (serialize_data);
				}
				else
				{
					CSerializeData serialize_data = new CSerializeData(type);

					// Добавляем поля
					serialize_data.AddFields(BindingFlagsUserClassField, TSerializeMemberOption.AnyAttributes);

					// Добавляем свойства
					serialize_data.AddProperties(BindingFlagsUserClassProperty, TSerializeMemberOption.AnyAttributes);

					if (serialize_data.Members.Count != 0)
					{
						// Сортируем
						serialize_data.Members.Sort();
					}
					else
					{
						// У нас нет членов объекта для сериализации - странная ситуация)))
						if (type.IsCubeXPlatformType())
						{
#if (UNITY_2017_1_OR_NEWER)
							//UnityEngine.Debug.LogErrorFormat("For CubeX type <{0}> members = 0", type.Name);
#else
							//XLogger.LogErrorFormatModule(XSerializationDispatcher.MODULE_NAME, "For CubeX type <{0}> members = 0", type.Name);
#endif
							return (null);
						}
					}

					return (serialize_data);
				}
			}

#if (UNITY_2017_1_OR_NEWER)
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание данных для сериализации структуры Unity по умолчанию
			/// </summary>
			/// <param name="type">Тип Unity</param>
			/// <returns>Данные для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData CreateForUnityStructType(Type type)
			{
				CSerializeData serialize_data = new CSerializeData(type);

				// Добавляем поля
				serialize_data.AddFields(BindingFlagsUnityStructField, TSerializeMemberOption.None, (FieldInfo field_info) =>
				{
					if (IgnoreUnityStructFields.Contains(field_info.Name)) return false;
					return (true);
				});

				// Добавляем свойства
				serialize_data.AddProperties(BindingFlagsUnityStructProperty, TSerializeMemberOption.None, (PropertyInfo property_info) =>
				{
					if (IgnoreUnityStructProperties.Contains(property_info.Name)) return false;
					return (true);
				});

				if (serialize_data.Members.Count != 0)
				{
					// Сортируем
					serialize_data.Members.Sort();
				}
				else
				{
					// Нету доступных членов объекта для сериализации – такой тип не нужен
					//UnityEngine.Debug.LogWarningFormat("For struct type Unity <{0}> members = 0", type.Name);
					return (null);
				}

				return (serialize_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание данных для сериализации класса Unity по умолчанию
			/// </summary>
			/// <param name="type">Тип Unity</param>
			/// <returns>Данные для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData CreateForUnityClassType(Type type)
			{
				CSerializeData serialize_data = new CSerializeData(type);

				// Добавляем поля
				serialize_data.AddFields(BindingFlagsUnityClassField, TSerializeMemberOption.None, (FieldInfo field_info) =>
				{
					if (IgnoreUnityClassFields.Contains(field_info.Name)) return false;
					return (true);
				});

				// Добавляем свойства
				serialize_data.AddProperties(BindingFlagsUnityClassProperty, TSerializeMemberOption.None, (PropertyInfo property_info) =>
				{
					if (IgnoreUnityClassProperties.Contains(property_info.Name)) return false;
					return (true);
				});

				if (serialize_data.Members.Count != 0)
				{
					// Сортируем
					serialize_data.Members.Sort();
				}
				else
				{
					// Нету доступных членов объекта для сериализации – такой тип не нужен
					//UnityEngine.Debug.LogWarningFormat("For class type Unity <{0}> members = 0", type.Name);
					return (null);
				}

				return (serialize_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание данных для сериализации компонента Unity по умолчанию
			/// </summary>
			/// <param name="type">Тип Unity</param>
			/// <returns>Данные для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData CreateForUnityComponentType(Type type)
			{
				CSerializeData serialize_data = new CSerializeData(type);

				// Добавляем свойства
				serialize_data.AddProperties(BindingFlagsUnityComponentProperty, TSerializeMemberOption.None, (PropertyInfo property_info) =>
				{
					if (IgnoreUnityComponentProperties.Contains(property_info.Name)) return false;
					return (true);
				});

				if (serialize_data.Members.Count != 0)
				{
					// Сортируем
					serialize_data.Members.Sort();
				}
				else
				{
					// Нету доступных членов объекта для сериализации – такой тип не нужен
					//UnityEngine.Debug.LogWarningFormat("For component Unity <{0}> members = 0", type.Name);
					return (null);
				}

				return (serialize_data);
			}
#endif
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на поддержку сериализации указанного поля
			/// </summary>
			/// <param name="field_info">Метаданные поля</param>
			/// <returns>Статус поддержки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsSupportedField(FieldInfo field_info)
			{
				// Не читаем устаревшие поля
				if (field_info.IsObsolete()) return false;

				// Не учитываем делегаты
				if (field_info.IsDelegateType()) return false;

#if UNITY_2017_1_OR_NEWER
				// Типы которые принципиально не возможно сериализовать
				if (field_info.FieldType.IsSubclassOf(typeof(UnityEngine.YieldInstruction))) return (false);
				if (field_info.FieldType.IsSubclassOf(typeof(UnityEngine.Events.UnityEventBase))) return (false);
				if (field_info.FieldType.IsSubclassOf(typeof(UnityEngine.EventSystems.AbstractEventData))) return (false);
#endif
				// Остальные поля читаем
				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на поддержку сериализации указанного свойства
			/// </summary>
			/// <param name="property_info">Метаданные свойства</param>
			/// <returns>Статус поддержки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsSupportedProperty(PropertyInfo property_info)
			{
				// Не читаем свойства которые нельзя записать
				if (property_info.CanWrite == false) return false;

				// Не читаем устаревшие свойства
				if (property_info.IsObsolete()) return false;

				// Не учитываем индексаторы
				if (property_info.IsIndexer()) return false;

				// Не учитываем делегаты
				if (property_info.IsDelegateType()) return false;

#if UNITY_2017_1_OR_NEWER
				// Типы которые принципиально не возможно сериализовать
				if (property_info.PropertyType.IsSubclassOf(typeof(UnityEngine.YieldInstruction))) return (false);
				if (property_info.PropertyType.IsSubclassOf(typeof(UnityEngine.Events.UnityEventBase))) return (false);
				if (property_info.PropertyType.IsSubclassOf(typeof(UnityEngine.EventSystems.AbstractEventData))) return (false);
#endif

				// Остальные свойства читаем
				return true;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			//Основные параметры
			protected internal Type mSerializeType;
			protected internal String mAliasNameType;
			protected internal TSerializeDataType mSerializeDataType;
			protected internal List<TSerializeDataMember> mMembers;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Тип для сериализации
			/// </summary>
			public Type SerializeType
			{
				get { return mSerializeType; }
				set { mSerializeType = value; }
			}

			/// <summary>
			/// Имя типа используемое при сериализации данного типа
			/// </summary>
			public String SerializeNameType
			{
				get
				{ 
					if(mAliasNameType.IsExists())
					{
						return (mAliasNameType); 
					}
					else
					{
						return (mSerializeType.Name);
					}
				}
			}

			/// <summary>
			/// Псевдоним имени типа
			/// </summary>
			public String AliasNameType
			{
				get { return mAliasNameType; }
				set { mAliasNameType = value; }
			}

			/// <summary>
			/// Тип данных с точки зрения сериализации
			/// </summary>
			public TSerializeDataType SerializeDataType
			{
				get { return mSerializeDataType; }
				set { mSerializeDataType = value; }
			}

			/// <summary>
			/// Список членов объекто (полей/свойств) для сериализации
			/// </summary>
			public List<TSerializeDataMember> Members
			{
				get { return mMembers; }
				set { mMembers = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CSerializeData()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект указанным типом
			/// </summary>
			/// <param name="serialize_type">Тип для сериализации данных</param>
			/// <param name="is_no_members">Статус отсутствия членов объекта для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public CSerializeData(Type serialize_type, Boolean is_no_members = false)
			{
				mSerializeType = serialize_type;
				if (!is_no_members)
				{
					mMembers = new List<TSerializeDataMember>();
				}

				CubeXSerializeAliasTypeAttribute alias = serialize_type.GetAttribute<CubeXSerializeAliasTypeAttribute>();
				if(alias != null)
				{
					mAliasNameType = alias.Name;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление члена объекта для сериализации
			/// </summary>
			/// <param name="member_data">Член объект</param>
			/// <param name="name_serialize">Имя члена объекта под которыми оно используется для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddDataMember(MemberInfo member_data, String name_serialize = null)
			{
				TSerializeMemberType serialize_member_type = TSerializeDataMember.ComputeSerializeMemberType(member_data);
				mMembers.Add(new TSerializeDataMember(member_data, serialize_member_type, name_serialize));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление поля для сериализации
			/// </summary>
			/// <param name="field_name">Имя поля для добавления</param>
			/// <param name="name_serialize">Имя поля под которыми оно используется для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddField(String field_name, String name_serialize = null)
			{
				FieldInfo field = mSerializeType.GetField(field_name, BindingFlagsUserClassField);
				AddDataMember(field, name_serialize);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление полей для сериализации
			/// </summary>
			/// <param name="binding_flags">Флаги данных используемые при поиске</param>
			/// <param name="member_option">Опции сериализации поля</param>
			/// <param name="on_support_field">Предикат для дополнительной проверки на поддержку поля</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddFields(BindingFlags binding_flags, TSerializeMemberOption member_option,
				Predicate<FieldInfo> on_support_field = null)
			{
				FieldInfo[] fields = mSerializeType.GetFields(binding_flags);
				for (Int32 i = 0; i < fields.Length; i++)
				{
					FieldInfo field = fields[i];

					// Если поле вообще поддерживает сериализацию
					if (IsSupportedField(field))
					{
						Boolean included = false;

						// Только если есть атрибут
						if (member_option.IsFlagSet(TSerializeMemberOption.SerializationAttribute))
						{
							included = AddFieldByAttribute<CubeXSerializeMemberAttribute>(field, on_support_field);
							if (included)
							{
								continue;
							}
							else
							{
								included = AddFieldByAttribute<XmlAttributeAttribute>(field, on_support_field);
								if (included)
								{
									continue;
								}
								else
								{
#if UNITY_2017_1_OR_NEWER
									included = AddFieldByAttribute<UnityEngine.SerializeField>(field, on_support_field);
									if (included)
									{
										continue;
									}
#endif
								}
							}
						}

						included = AddFieldByAttribute<XmlAttributeAttribute>(field, on_support_field);
						if (included)
						{
							continue;
						}
						else
						{
#if UNITY_2017_1_OR_NEWER
							included = AddFieldByAttribute<UnityEngine.SerializeField>(field, on_support_field);
							if (included)
							{
								continue;
							}
#endif
						}

#if UNITY_2017_1_OR_NEWER
						included = AddFieldByAttribute<UnityEngine.SerializeField>(field, on_support_field);
						if (included)
						{
							continue;
						}
#endif

						if (included == false && member_option == TSerializeMemberOption.None)
						{
							if (on_support_field != null)
							{
								if (on_support_field(field))
								{
									AddDataMember(field, field.Name);
								}
							}
							else
							{
								AddDataMember(field, field.Name);
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление свойства для сериализации
			/// </summary>
			/// <param name="property_name">Имя свойства для добавления</param>
			/// <param name="name_serialize">Имя свойства под которыми оно используется для сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddProperty(String property_name, String name_serialize = null)
			{
				PropertyInfo property = mSerializeType.GetProperty(property_name, BindingFlagsUserClassProperty);
				AddDataMember(property, name_serialize);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление свойств для сериализации
			/// </summary>
			/// <param name="binding_flags">Флаги данных используемые при поиске</param>
			/// <param name="member_option">Опции сериализации свойства</param>
			/// <param name="on_support_property">Предикат для дополнительной проверки на поддержку свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddProperties(BindingFlags binding_flags, TSerializeMemberOption member_option,
				Predicate<PropertyInfo> on_support_property = null)
			{
				PropertyInfo[] properties = mSerializeType.GetProperties(binding_flags);
				for (Int32 i = 0; i < properties.Length; i++)
				{
					PropertyInfo property = properties[i];
					if (IsSupportedProperty(property))
					{
						Boolean included = false;

						// Только если есть атрибут
						if(member_option.IsFlagSet(TSerializeMemberOption.SerializationAttribute))
						{
							included = AddPropertyByAttribute<CubeXSerializeMemberAttribute>(property, on_support_property);
							if(included)
							{
								continue;
							}
							else
							{
								included = AddPropertyByAttribute<XmlAttributeAttribute>(property, on_support_property);
								if (included)
								{
									continue;
								}
							}
						}

						included = AddPropertyByAttribute<XmlAttributeAttribute>(property, on_support_property);
						if (included)
						{
							continue;
						}

						if (included == false && member_option == TSerializeMemberOption.None)
						{
							if (on_support_property != null)
							{
								if (on_support_property(property))
								{
									AddDataMember(property, property.Name);
								}
							}
							else
							{
								AddDataMember(property, property.Name);
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка данных сериализации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ClearMembers()
			{
				if(mMembers != null)
				{
					mMembers.Clear();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ОБРАБОТКИ ДАННЫХ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени для сериализации по параметрам атрибута
			/// </summary>
			/// <param name="attribute">Атрибут связанный c сериализацией члена объекта</param>
			/// <param name="default_name">Имя по умолчанию</param>
			/// <returns>Имя для сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			protected String GetMemberSerializeName(Attribute attribute, String default_name)
			{
				if (attribute is CubeXSerializeMemberAttribute)
				{
					CubeXSerializeMemberAttribute serialization_attribute = attribute as CubeXSerializeMemberAttribute;
					if (serialization_attribute != null && serialization_attribute.Name.IsExists())
					{
						return (serialization_attribute.Name);
					}
				}

				if (attribute is XmlAttributeAttribute)
				{
					XmlAttributeAttribute xml_attribute = attribute as XmlAttributeAttribute;
					if (xml_attribute != null && xml_attribute.AttributeName.IsExists())
					{
						return (xml_attribute.AttributeName);
					}
				}

				if (attribute is XmlElementAttribute)
				{
					XmlElementAttribute xml_attribute = attribute as XmlElementAttribute;
					if (xml_attribute != null && xml_attribute.ElementName.IsExists())
					{
						return (xml_attribute.ElementName);
					}
				}

#if UNITY_2017_1_OR_NEWER

				if (attribute is UnityEngine.Serialization.FormerlySerializedAsAttribute)
				{
					UnityEngine.Serialization.FormerlySerializedAsAttribute unity_attribute =
						attribute as UnityEngine.Serialization.FormerlySerializedAsAttribute;
					if (unity_attribute != null && unity_attribute.oldName.IsExists())
					{
						return (unity_attribute.oldName);
					}
				}
#endif

				return (default_name);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление поля содержащий указанный атрибут к сериализуемым данным
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="field_info">Метаданные поля</param>
			/// <param name="on_support_field">Предикат для дополнительной проверки на поддержку поля</param>
			/// <returns>Статус наличия указанного атрибута</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Boolean AddFieldByAttribute<TAttribute>(FieldInfo field_info,
				Predicate<FieldInfo> on_support_field = null) where TAttribute : Attribute
			{
				TAttribute attribute = field_info.GetAttribute<TAttribute>();
				if (attribute != null)
				{
					if (on_support_field != null)
					{
						if (on_support_field(field_info))
						{
							AddDataMember(field_info, GetMemberSerializeName(attribute, field_info.Name));
						}
						return (true);
					}
					else
					{
						AddDataMember(field_info, GetMemberSerializeName(attribute, field_info.Name));
						return (true);
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление свойства содержащий указанный атрибут к сериализуемым данным
			/// </summary>
			/// <typeparam name="TAttribute">Тип атрибута</typeparam>
			/// <param name="property_info">Метаданные свойства</param>
			/// <param name="on_support_property">Предикат для дополнительной проверки на поддержку свойства</param>
			/// <returns>Статус наличия указанного атрибута</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Boolean AddPropertyByAttribute<TAttribute>(PropertyInfo property_info,
				Predicate<PropertyInfo> on_support_property = null) where TAttribute : Attribute
			{
				TAttribute attribute = property_info.GetAttribute<TAttribute>();
				if (attribute != null)
				{
					if (on_support_property != null)
					{
						if (on_support_property(property_info))
						{
							AddDataMember(property_info, GetMemberSerializeName(attribute, property_info.Name));
						}
						return (true);
					}
					else
					{
						AddDataMember(property_info, GetMemberSerializeName(attribute, property_info.Name));
						return (true);
					}
				}

				return (false);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================