//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializationDispatcher.cs
*		Диспетчер подсистемы сериализации данных для сохранения/загрузки объектов в различных форматах.
*		Диспетчер обеспечивает получение данных сериализации для всех типов, непосредственно осуществляет процесс 
*	сохранения/загрузки данных, связывания данных при непобедимости, а также управляет всеми существующими объектами сериализации.
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
		/// Диспетчер подсистемы сериализации данных для сохранения/загрузки объектов в различных форматах
		/// </summary>
		/// <remarks>
		/// Диспетчер обеспечивает получение данных сериализации для всех типов, непосредственно осуществляет процесс 
		/// сохранения/загрузки данных, связывания данных при их необходимости, а также управляет всеми существующими 
		/// объектами сериализации.
		/// В текущей версии поддерживается сохранение/загрузка только из файла в формат XML.
		/// Управляется центральным диспетчером <see cref="CubeX.Common.CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSerializationDispatcher
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			//
			// ИМЕНА МЕТОДОВ
			//
			/// <summary>
			/// Имя статического метода типа который представляет данные для сериализации
			/// </summary>
			public const String GET_SERIALIZE_DATA_METHOD = "GetSerializeData";

			/// <summary>
			/// Имя статического метода типа, который преобразует объект из строки
			/// </summary>
			public const String DESERIALIZE_FROM_STRING = "DeserializeFromString";

			/// <summary>
			/// Имя метода типа который сериализует объект в строку
			/// </summary>
			public const String SERIALIZE_TO_STRING = "SerializeToString";

			//
			// ИМЕНА ЭЛЕМЕНТОВ И АТРИБУТОВ XML
			//
			/// <summary>
			/// Имя корневого элемента узла XML
			/// </summary>
			public const String XML_NAME_ELEMENT_ROOT = "Serializations";

			/// <summary>
			/// Имя элемента для записи словаря узла XML
			/// </summary>
			public const String XML_NAME_ELEMENT_DICTIONARY = "Dictionary";

			/// <summary>
			/// Имя элемента для записи коллекций узла XML
			/// </summary>
			public const String XML_NAME_ELEMENT_COLLECTION = "Collection";

			/// <summary>
			/// Имя элемента для записи коллекции моделей узла XML
			/// </summary>
			public const String XML_NAME_ELEMENT_COLLECTION_MODEL = "CollectionModel";

			/// <summary>
			/// Имя атрибута для записи данных
			/// </summary>
			public const String XML_NAME_ATTRIBUTE_VALUE = "Value";

			/// <summary>
			/// Имя для записи нулевой ссылки пользовательского класса
			/// </summary>
			public const String XML_NAME_NULL = "NullValue";

			/// <summary>
			/// Имя версии файла XML
			/// </summary>
			public readonly static Version XML_VERSION = new Version(1, 0, 0, 0);

			/// <summary>
			/// Имя модуля
			/// </summary>
			public const String MODULE_NAME = "Serialization";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Словарь всех объектов поддерживающих интерфейс сериализации объекта
			/// </summary>
			public static readonly Dictionary<Int64, ICubeXSerializableObject> SerializableObjects = new Dictionary<Int64, ICubeXSerializableObject>();

			/// <summary>
			/// Глобальный конструктор для создания объекта по имени 
			/// </summary>
			public static Func<String, System.Object> Constructor;

			//
			// БАЗОВЫЕ ПУТИ
			//
#if (UNITY_2017_1_OR_NEWER)
#if UNITY_EDITOR
			/// <summary>
			/// Путь по умолчанию для сохранения/загрузки файлов
			/// </summary>
			public static String DefaultPath = XEditorSettings.AutoSavePath;
#else
			/// <summary>
			/// Путь по умолчанию для сохранения/загрузки  файлов
			/// </summary>
			public static String DefaultPath = UnityEngine.Application.persistentDataPath;
#endif
#else
			/// <summary>
			/// Путь по умолчанию для сохранения/загрузки файлов
			/// </summary>
			public static String DefaultPath = Environment.CurrentDirectory;
#endif
			//
			// БАЗОВЫЕ РАСШИРЕНИЯ
			//
			/// <summary>
			/// Расширение файла по умолчанию
			/// </summary>
			public static String DefaultExt = XFileExtension.XML_D;

			/// <summary>
			/// Словарь данных сериализации по имени типа
			/// </summary>
			private static Dictionary<String, CSerializeData> mSerializeDataByName;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Словарь данных сериализации по имени типа
			/// </summary>
			public static Dictionary<String, CSerializeData> SerializeDataByName
			{
				get
				{
					if (mSerializeDataByName == null)
					{
						OnInitSerializeData();
					}

					return (mSerializeDataByName);
				}
			}
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск диспетчера подсистемы сериализации данных в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
				mSerializeDataByName = null;
				SerializableObjects.Clear();

#if (UNITY_2017_1_OR_NEWER)
				XSerializatorUnity.ClearSerializeReferences();
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация диспетчера подсистемы сериализации данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				OnInitSerializeData();
			}
			#endregion

			#region ======================================= МЕТОДЫ ОБРАБОТКИ ДАННЫХ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных сериализации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void OnInitSerializeData()
			{
				if (mSerializeDataByName == null)
				{
					// 1) Используем профилирование
					System.Diagnostics.Stopwatch profiler = new System.Diagnostics.Stopwatch();
					profiler.Start();

					Int32 count_assemblies = 0;
					Int32 count_types = 0;

					// Создаем словарь по имени типа (не полное имя)
					mSerializeDataByName = new Dictionary<String, CSerializeData>(300);
					// 
					mSerializeDataByName.Add(nameof(Boolean), CSerializeData.CreateNoMembers(typeof(Boolean)));
					mSerializeDataByName.Add(nameof(Byte), CSerializeData.CreateNoMembers(typeof(Byte)));
					mSerializeDataByName.Add(nameof(Char), CSerializeData.CreateNoMembers(typeof(Char)));
					mSerializeDataByName.Add(nameof(Int16), CSerializeData.CreateNoMembers(typeof(Int16)));
					mSerializeDataByName.Add(nameof(UInt16), CSerializeData.CreateNoMembers(typeof(UInt16)));
					mSerializeDataByName.Add(nameof(Int32), CSerializeData.CreateNoMembers(typeof(Int32)));
					mSerializeDataByName.Add(nameof(UInt32), CSerializeData.CreateNoMembers(typeof(UInt32)));
					mSerializeDataByName.Add(nameof(Int64), CSerializeData.CreateNoMembers(typeof(Int64)));
					mSerializeDataByName.Add(nameof(UInt64), CSerializeData.CreateNoMembers(typeof(UInt64)));
					mSerializeDataByName.Add(nameof(Single), CSerializeData.CreateNoMembers(typeof(Single)));
					mSerializeDataByName.Add(nameof(Double), CSerializeData.CreateNoMembers(typeof(Double)));
					mSerializeDataByName.Add(nameof(Decimal), CSerializeData.CreateNoMembers(typeof(Decimal)));
					mSerializeDataByName.Add(nameof(String), CSerializeData.CreateNoMembers(typeof(String)));
					mSerializeDataByName.Add(nameof(DateTime), CSerializeData.CreateNoMembers(typeof(DateTime)));
					mSerializeDataByName.Add(nameof(TimeSpan), CSerializeData.CreateNoMembers(typeof(TimeSpan)));
					mSerializeDataByName.Add(nameof(Version), CSerializeData.CreateNoMembers(typeof(Version)));
					mSerializeDataByName.Add(nameof(Uri), CSerializeData.CreateNoMembers(typeof(Uri)));

					// Получаем все загруженные сборки в домене
					var assemblies = AppDomain.CurrentDomain.GetAssemblies();

					// Проходим по всем сборкам
					for (Int32 ia = 0; ia < assemblies.Length; ia++)
					{
						// Сборка
						var assemble = assemblies[ia];

						// Если она поддерживается
						if (CheckSupportAssembly(assemble))
						{
							count_assemblies++;
							//UnityEngine.Debug.Log("Assembly: " + assemble.FullName);

							// Получаем все типы в сборке
							var types = assemble.GetExportedTypes();

							// Проходим по всем типам
							for (Int32 it = 0; it < types.Length; it++)
							{
								// Получаем тип
								var type = types[it];

								// Только он поддерживается
								if (CheckSupportType(type))
								{
									count_types++;
									//UnityEngine.Debug.Log("Type: " + type.Name);

									// Кэшируем данные
									GetSerializeDataForType(type);

									// Получаем вложенные типы
									var nested_types = type.GetNestedTypes();

									if (nested_types != null && nested_types.Length > 0)
									{
										// Анализируем внутренние типы
										for (Int32 itn = 0; itn < nested_types.Length; itn++)
										{
											var nested_type = nested_types[itn];
											if (nested_type.IsPublic)
											{
												// Кэшируем данные
												GetSerializeDataForType(type);
											}
										}
									}
								}
							}
						}
					}

					profiler.Stop();

#if UNITY_2017_1_OR_NEWER
					UnityEngine.Debug.LogFormat("Assemblies load count: {0}", count_assemblies);
					UnityEngine.Debug.LogFormat("Types load count: {0}", count_types);
					UnityEngine.Debug.LogFormat("Loaded time: {0} ms", profiler.ElapsedMilliseconds);
#else
					XLogger.LogInfoFormatModule(MODULE_NAME, "Assemblies load count: {0}", count_assemblies);
					XLogger.LogInfoFormatModule(MODULE_NAME, "Types load count: {0}", count_types);
					XLogger.LogInfoFormatModule(MODULE_NAME, "Loaded time: {0} ms", profiler.ElapsedMilliseconds);
#endif
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка указанной сборки на целесообразность получать типы для которых потребуются данные сериализации
			/// </summary>
			/// <param name="assembly">Сборка</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			private static Boolean CheckSupportAssembly(Assembly assembly)
			{
				if (assembly.FullName.IndexOf("SyntaxTree") > -1) return (false);
				if (assembly.FullName.IndexOf("NUnit") > -1) return (false);
				if (assembly.FullName.IndexOf("Mono") > -1) return (false);
				if (assembly.FullName.IndexOf("mscorlib") > -1) return (false);
				if (assembly.FullName.IndexOf("System") > -1) return (false);
				if (assembly.FullName.IndexOf("Editor") > -1) return (false);
				if (assembly.FullName.IndexOf("Test") > -1) return (false);

				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка типа на возможность получение оптимальных(нужных) данных сериализации 
			/// </summary>
			/// <param name="type">Тип</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			private static Boolean CheckSupportType(Type type)
			{
				// Небольшое ускорения
				// Перечисления всегда подлежат сериализации
				if (type.IsEnum && type.IsPublic) return (true);
				if (type.IsPrimitive && type.IsPublic) return (true);

				// Общие ограничения
				if (type.IsAbstract) return (false);
				if (type.IsGenericType) return (false);
				if (type.IsPublic == false) return (false);
				if (type.IsStaticType()) return (false);
				if (type.IsSubclassOf(typeof(Exception))) return (false);
				if (type.IsSubclassOf(typeof(Attribute))) return (false);

				//
				// Ограничения платформы CubeX
				//
				// Общие
				if (type.GetAttribute<CubeXSerializeDisableAttribute>() != null) return (false);

				// Внутри платформы
				//if (type.FullName.Contains(XReflection.CUBEX_PREFIX))
				//{

				//}

				//
				// Ограничения Unity
				//
#if UNITY_2017_1_OR_NEWER
				// Общие
				// Типы которые принципиально не возможно сериализовать
				if (type.IsSubclassOf(typeof(UnityEngine.YieldInstruction))) return (false);
				if (type.IsSubclassOf(typeof(UnityEngine.Events.UnityEventBase))) return (false);
				if (type.IsSubclassOf(typeof(UnityEngine.EventSystems.AbstractEventData))) return (false);

				// Внутри платформы
				if (type.IsUnityModule())
				{
					if (type.FullName.Contains("UIElements")) return (false);
					if (type.FullName.IndexOf("Experimental") > -1) return (false);
					if (type.FullName.IndexOf("NUnit") > -1) return (false);
					if (type.FullName.IndexOf("SyntaxTree") > -1) return (false);
				}
				else
				{
					// Типы которые не обозначены для сериализации (за исключения перечислений)
					if (type.GetAttribute<SerializableAttribute>() == null && !type.IsEnum) return (false);
				}
#endif
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение данных сериализации непосредственно от указанного типа
			/// </summary>
			/// <param name="type">Тип</param>
			/// <returns>Данные сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			private static CSerializeData GetSerializeDataFromType(Type type)
			{
				CSerializeData serialize_data = null;

				if (type.GetAttribute<CubeXSerializeDataAttribute>() != null)
				{
					try
					{
						// Есть метод который может дать конкретные данные для сериализации данного типа
						MethodInfo method = type.GetMethod(GET_SERIALIZE_DATA_METHOD, BindingFlags.Static | BindingFlags.Public);
						if (method != null)
						{
							serialize_data = (CSerializeData)method.Invoke(null, null);
						}
						else
						{
#if (UNITY_2017_1_OR_NEWER)
							UnityEngine.Debug.LogErrorFormat("SerializeData attribute, method none of type: <{0}>", type.Name);
#else
							XLogger.LogErrorFormatModule(MODULE_NAME, "SerializeData attribute, method none of type: <{0}>", 
								type.Name + ">");
#endif
						}
					}
					catch (Exception)
					{
					}
				}

				return (serialize_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение данных сериализации для типа
			/// </summary>
			/// <param name="type">Тип</param>
			/// <returns>Данные сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			private static CSerializeData GetSerializeDataForType(Type type)
			{
				CSerializeData serialize_data = null;

				// Вычисляем тип данных
				TSerializeDataType serialize_data_type = CSerializeData.ComputeSerializeDataType(type);

				switch (serialize_data_type)
				{
					case TSerializeDataType.Primitive:
						{
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								serialize_data = CSerializeData.CreateNoMembers(type);
								mSerializeDataByName.Add(type.Name, serialize_data);
							}
							break;
						}
					case TSerializeDataType.Struct:
						{
#if UNITY_2017_1_OR_NEWER
							// Структура Unity
							if (type.IsUnityModule())
							{
								// Только по имени типа
								if (!mSerializeDataByName.ContainsKey(type.Name))
								{
									serialize_data = CSerializeData.CreateForUnityStructType(type);
									if (serialize_data != null)
									{
										mSerializeDataByName.Add(type.Name, serialize_data);
									}
								}
								break;
							}
#endif
							// Структура
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								// Пробуем получить данные непосредственно от типа 
								serialize_data = GetSerializeDataFromType(type);
								if (serialize_data != null)
								{
									mSerializeDataByName.Add(type.Name, serialize_data);
									AddSerializeDataFromAliasName(serialize_data);
								}
								else
								{
									// Вычисляем автоматические 
									serialize_data = CSerializeData.CreateForUserStructType(type);
									if (serialize_data != null)
									{
										mSerializeDataByName.Add(type.Name, serialize_data);
										AddSerializeDataFromAliasName(serialize_data);
									}
								}
							}
							break;
						}
					case TSerializeDataType.Class:
						{
#if UNITY_2017_1_OR_NEWER
							// Класс Unity
							if (type.IsUnityModule())
							{
								// Только по имени типа
								if (!mSerializeDataByName.ContainsKey(type.Name))
								{
									serialize_data = CSerializeData.CreateForUnityClassType(type);
									if (serialize_data != null)
									{
										mSerializeDataByName.Add(type.Name, serialize_data);
									}
								}
								break;
							}
#endif
							// Класс
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								// Пробуем получить данные непосредственно от типа 
								serialize_data = GetSerializeDataFromType(type);
								if (serialize_data != null)
								{
									mSerializeDataByName.Add(type.Name, serialize_data);
									AddSerializeDataFromAliasName(serialize_data);
								}
								else
								{
									// Вычисляем автоматические 
									serialize_data = CSerializeData.CreateForUserClassType(type);
									if (serialize_data != null)
									{
										mSerializeDataByName.Add(type.Name, serialize_data);
										AddSerializeDataFromAliasName(serialize_data);
									}
								}
							}
							break;
						}
#if (UNITY_2017_1_OR_NEWER)
					case TSerializeDataType.UnityComponent:
						{
							// Только по имени типа
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								serialize_data = CSerializeData.CreateForUnityComponentType(type);
								if (serialize_data != null)
								{
									mSerializeDataByName.Add(type.Name, serialize_data);
								}
							}
						}
						break;
					case TSerializeDataType.UnityUserComponent:
						{
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								serialize_data = CSerializeData.CreateForUserClassType(type);
								if (serialize_data != null)
								{
									mSerializeDataByName.Add(type.Name, serialize_data);
								}
							}
						}
						break;
					case TSerializeDataType.UnityGameObject:
						{
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								serialize_data = CSerializeData.CreateNoMembers(type);
								mSerializeDataByName.Add(type.Name, serialize_data);
							}
						}
						break;
					case TSerializeDataType.UnityResource:
					case TSerializeDataType.UnityUserResource:
						{
							if (!mSerializeDataByName.ContainsKey(type.Name))
							{
								serialize_data = CSerializeData.CreateNoMembers(type);
								mSerializeDataByName.Add(type.Name, serialize_data);
							}
						}
						break;
#endif
					default:
						break;
				}

				if (serialize_data != null)
				{
					serialize_data.SerializeDataType = serialize_data_type;
				}

				return (serialize_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление данных для сериализации с учетом псевдонима имени типа
			/// </summary>
			/// <param name="serialize_data">Данные сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			private static void AddSerializeDataFromAliasName(CSerializeData serialize_data)
			{
				// Если еще есть и псевдоним имени типа 
				if (serialize_data.AliasNameType.IsExists())
				{
					// Проверяем его так возможно есть дубликаты
					if (mSerializeDataByName.ContainsKey(serialize_data.AliasNameType))
					{
						CSerializeData sd = mSerializeDataByName[serialize_data.AliasNameType];
#if (UNITY_2017_1_OR_NEWER)
						UnityEngine.Debug.LogErrorFormat("Alias <{0}> type <{1}> already exists in the type <{2}>",
						serialize_data.AliasNameType, serialize_data.SerializeType.Name, sd.SerializeType.Name);
#else
						XLogger.LogErrorFormatModule(MODULE_NAME, "Alias <{0}> type <{1}> already exists in the type <{2}>",
						serialize_data.AliasNameType, serialize_data.SerializeType.Name, sd.SerializeType.Name);
#endif
					}
					else
					{
						// Добавляем
						mSerializeDataByName.Add(serialize_data.AliasNameType, serialize_data);
					}
				}
			}
			#endregion

			#region ======================================= ПОЛУЧЕНИЕ ДАННЫХ СЕРИАЛИЗАЦИИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение данных сериализации для указанного типа
			/// </summary>
			/// <typeparam name="TType">Тип</typeparam>
			/// <returns>Данные сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData GetSerializeData<TType>()
			{
				return GetSerializeData(typeof(TType));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение данных сериализации для указанного типа
			/// </summary>
			/// <param name="type">Тип</param>
			/// <returns>Данные сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData GetSerializeData(Type type)
			{
				if (SerializeDataByName.ContainsKey(type.Name))
				{
					return mSerializeDataByName[type.Name];
				}

				return (GetSerializeDataForType(type));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение данных сериализации для указанного имени типа
			/// </summary>
			/// <param name="type_name">Имя типа</param>
			/// <returns>Данные сериализации</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CSerializeData GetSerializeData(String type_name)
			{
				if (SerializeDataByName.ContainsKey(type_name))
				{
					return mSerializeDataByName[type_name];
				}

#if (UNITY_2017_1_OR_NEWER)
				UnityEngine.Debug.LogErrorFormat("Not serialize data of type <{0}>", type_name);
#else
				XLogger.LogErrorFormatModule(MODULE_NAME, "Not serialize data of type <{0}>", type_name);
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка данных сериализации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void ClearSerializeMembers()
			{
				foreach (var item in SerializeDataByName.Values)
				{
					item.ClearMembers();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ОБЪЕКТОВ СЕРИАЛИЗАЦИИ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объектов поддерживающих интерфейс сериализации после полной загрузки
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateSerializableObjects()
			{
				// Вызываем метод у всех объектов поддерживающих интерфейс сериализации только после полной загрузки объекта 
				if (SerializableObjects.Count > 0)
				{
					foreach (var item in SerializableObjects.Values)
					{
						if (item != null) item.OnAfterLoading();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка словаря объектов поддерживающих интерфейс сериализации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void ClearSerializableObjects()
			{
				SerializableObjects.Clear();
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАПИСИ/ЧТЕНИЯ XML ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись всего объекта в формат XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteDataToXml(XmlWriter writer, System.Object instance)
			{
				// Получаем типа объекта
				Type object_type = instance.GetType();

				if(object_type.IsDictionaryType())
				{
					//
					// Реализовать
					//
				}
				else
				{
					if (object_type.IsClassicCollectionType())
					{
						//
						// Это коллекция
						//
						// Получаем коллекцию
						IList collection_instance = instance as IList;

						// Тип элемента коллекции
						Type element_type = object_type.GetClassicCollectionItemType();

						// Записываем
						XSerializatorCollection.WriteCollectionToXml(writer, element_type, collection_instance, XML_NAME_ELEMENT_COLLECTION);
					}
					else
					{
						if (object_type.IsCollectionModel())
						{
							//
							// Это коллекция
							//

							// Получаем коллекцию
							ICubeXCollectionModel collection_model = (instance as ICubeXCollectionModel);

							// Записываем
							XSerializatorCollectionModel.WriteCollectionModelToXml(writer, collection_model, XML_NAME_ELEMENT_COLLECTION_MODEL);
						}
						else
						{ 
							// Получаем данные сериализации для этого объекта
							CSerializeData serialize_data = GetSerializeData(object_type);

							if (serialize_data == null)
							{
								return;
							}

							switch (serialize_data.SerializeDataType)
							{
								case TSerializeDataType.Primitive:
									{
										// Записываем начало элемента
										writer.WriteStartElement(object_type.Name);
										XSerializatorPrimitive.WriteToAttribute(writer, object_type, instance, XML_NAME_ATTRIBUTE_VALUE);
										writer.WriteEndElement();
									}
									break;
								case TSerializeDataType.Struct:
									{
										XSerializatorObject.WriteInstanceToXml(writer, instance, serialize_data.SerializeNameType);
									}
									break;
								case TSerializeDataType.Class:
									{
										XSerializatorObject.WriteInstanceToXml(writer, instance, serialize_data.SerializeNameType);
									}
									break;
#if UNITY_2017_1_OR_NEWER
							case TSerializeDataType.UnityComponent:
							case TSerializeDataType.UnityUserComponent:
								{
									writer.WriteStartElement(object_type.Name);
									XSerializatorUnity.WriteReferenceComponentToXml(writer, instance as UnityEngine.Component);
									writer.WriteEndElement();
								}
								break;
							case TSerializeDataType.UnityGameObject:
								{
									writer.WriteStartElement(object_type.Name);
									XSerializatorUnity.WriteReferenceGameObjectToXml(writer, instance as UnityEngine.GameObject);
									writer.WriteEndElement();
								}
								break;
							case TSerializeDataType.UnityResource:
							case TSerializeDataType.UnityUserResource:
								{
									writer.WriteStartElement(object_type.Name);
									XSerializatorUnity.WriteReferenceResourceToXml(writer, instance as UnityEngine.Object);
									writer.WriteEndElement();
								}
								break;
#endif
								default:
									break;
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение всех данных объекта из формата XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="index">Индекс при применении индексированных свойств</param>
			/// <returns>Экземпляр объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object ReadDataFromXml(XmlReader reader, System.Object instance, Int32 index = -1)
			{
				System.Object result = null;

				// Это словарь
				if (reader.Name == XML_NAME_ELEMENT_DICTIONARY)
				{
					//
					// Реализовать
					//
				}
				else
				{
					if (reader.Name == XML_NAME_ELEMENT_COLLECTION)
					{
						result = XSerializatorCollection.ReadCollectionFromXml(reader, instance as IList, XML_NAME_ELEMENT_COLLECTION, 0);
					}
					else
					{
						if (reader.Name == XML_NAME_ELEMENT_COLLECTION_MODEL)
						{
							result = XSerializatorCollectionModel.ReadCollectionModelFromXml(reader, instance as ICubeXCollectionModel, 
								XML_NAME_ELEMENT_COLLECTION_MODEL, 0);
						}
						else
						{
							// Получаем данные сериализации для этого типа
							CSerializeData serialize_data = GetSerializeData(reader.Name);

							if (serialize_data == null)
							{
#if (UNITY_2017_1_OR_NEWER)
								UnityEngine.Debug.LogErrorFormat("There is no specified type: <{0}>", reader.Name);
#else
							XLogger.LogErrorFormatModule(MODULE_NAME, "There is no specified type: <{0}>", reader.Name);
#endif
								return (result);
							}

							Type object_type = serialize_data.SerializeType;

							// Если тип может сам себя загрузить
							if (object_type.IsSupportInterface<ICubeXSerializeImplementationXML>())
							{
								if (instance == null)
								{
									result = XReflection.CreateInstance(object_type);
								}
								else
								{
									result = instance;
								}
								ICubeXSerializeImplementationXML serializable_self = result as ICubeXSerializeImplementationXML;
								serializable_self.ReadFromXml(reader);

								return (result);
							}

							switch (serialize_data.SerializeDataType)
							{
								case TSerializeDataType.Primitive:
									{
										result = XSerializatorPrimitive.ReadFromAttribute(reader, object_type, XML_NAME_ATTRIBUTE_VALUE);
									}
									break;
								case TSerializeDataType.Struct:
									{
										result = XSerializatorObject.ReadInstanceFromXml(reader, serialize_data, instance, reader.Name);
									}
									break;
								case TSerializeDataType.Class:
									{
										result = XSerializatorObject.ReadInstanceFromXml(reader, serialize_data, instance, reader.Name);
									}
									break;
#if UNITY_2017_1_OR_NEWER
								case TSerializeDataType.UnityComponent:
								case TSerializeDataType.UnityUserComponent:
									{
										result = XSerializatorUnity.ReadReferenceComponentFromXml(reader, instance, new TSerializeDataMember(reader.Name), index);
									}
									break;
								case TSerializeDataType.UnityGameObject:
									{
										result = XSerializatorUnity.ReadReferenceGameObjectFromXml(reader, instance, new TSerializeDataMember(reader.Name), index);
									}
									break;
								case TSerializeDataType.UnityResource:
								case TSerializeDataType.UnityUserResource:
									{
										result = XSerializatorUnity.ReadReferenceResourceFromXml(reader, instance, new TSerializeDataMember(reader.Name), index);
									}
									break;
#endif
								default:
									break;
							}

						}
					}
				}

				return (result);
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ/ЗАГРУЗКИ/ОБНОВЛЕНИЯ =====================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения объекта в файл
			/// </summary>
			/// <remarks>
			/// Формат записи определяется исходя из расширения файла
			/// </remarks>
			/// <param name="file_name">Имя файла</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveTo(String file_name, System.Object instance)
			{
				String ext = Path.GetExtension(file_name).ToLower();
				switch (ext)
				{
					case XFileExtension.XML_D:
						{
							SaveToXml(file_name, instance);
						}
						break;
					case XFileExtension.BIN_D:
					case XFileExtension.BYTES_D:
						{
							SaveToBinary(file_name, instance);
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка объекта из файла
			/// </summary>
			/// <remarks>
			/// Формат чтения определяется исходя из расширения файла
			/// </remarks>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object LoadFrom(String file_name)
			{
				String ext = Path.GetExtension(file_name).ToLower();
				System.Object result = null;
				switch (ext)
				{
					case XFileExtension.XML_D:
						{
							result = LoadFromXml(file_name);
						}
						break;
					case XFileExtension.BIN_D:
					case XFileExtension.BYTES_D:
						{
							result = LoadFromBinary(file_name);
						}
						break;
					default:
						break;
				}

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объекта из файла
			/// </summary>
			/// <remarks>
			/// Формат чтения определяется исходя из расширения файла
			/// </remarks>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateFrom(System.Object instance, String file_name)
			{
				String ext = Path.GetExtension(file_name).ToLower();
				switch (ext)
				{
					case XFileExtension.XML_D:
						{
							UpdateFromXml(instance, file_name);
						}
						break;
					case XFileExtension.BIN_D:
					case XFileExtension.BYTES_D:
						{
							UpdateFromBinary(instance, file_name);
						}
						break;
					default:
						break;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ XML =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения объекта в файл формата XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(String file_name, System.Object instance)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Создаем поток для записи
				StreamWriter stream_writer = new StreamWriter(path);
				SaveToXml(stream_writer, instance);
				stream_writer.Close();

#if UNITY_EDITOR
				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + path, "OK");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения объекта в строку в формате XML
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Строка в формате XML</returns>
			//---------------------------------------------------------------------------------------------------------
			public static StringBuilder SaveToXml(System.Object instance)
			{
				StringBuilder file_data = new StringBuilder(200);

				// Создаем поток для записи
				StringWriter string_writer = new StringWriter(file_data);

				// Сохраняем данные
				SaveToXml(string_writer, instance);
				string_writer.Close();

				return (file_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения объекта в поток данных в формате XML
			/// </summary>
			/// <param name="text_writer">Средство для записи в поток строковых данных</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(TextWriter text_writer, System.Object instance)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					OnInit();
				}
#endif
				// Открываем файл
				XmlWriterSettings xws = new XmlWriterSettings();
				xws.Indent = true;

				XmlWriter writer = XmlWriter.Create(text_writer, xws);

				// Записываем базовые данные
				writer.WriteStartElement(XML_NAME_ELEMENT_ROOT);
				writer.WriteAttributeString(nameof(Version), XML_VERSION.ToString());

				// Смотрим что за объект
				if (instance is ICubeXSerializeImplementationXML)
				{
					ICubeXSerializeImplementationXML serializable_self = instance as ICubeXSerializeImplementationXML;
					serializable_self.WriteToXml(writer);
				}
				else
				{
					WriteDataToXml(writer, instance);
				}

				// Закрываем поток
				writer.WriteEndElement();
				writer.Close();
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ XML =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка объекта из файла XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object LoadFromXml(String file_name)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Открываем файл
				StringReader string_reader = new StringReader(File.ReadAllText(path));
				return LoadFromXml(string_reader);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка объекта из строки в формате XML
			/// </summary>
			/// <param name="file_data">Строка с данными в формате XML</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object LoadFromStringXml(String file_data)
			{
				// Открываем файл
				StringReader string_reader = new StringReader(file_data);
				return LoadFromXml(string_reader);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка объекта из потока данных
			/// </summary>
			/// <param name="text_reader">Средство для чтения из потока строковых данных</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object LoadFromXml(TextReader text_reader)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					OnInit();
				}
#endif

#if (UNITY_2017_1_OR_NEWER)
				// Очищаем объекты для связи
				XSerializatorUnity.ClearSerializeReferences();
#endif
				// Открываем поток
				XmlReader reader = XmlReader.Create(text_reader);

				// Читаем данные
				System.Object result = null;
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						//
						// Пропускаем корневой объект
						//
						if (reader.Name == XML_NAME_ELEMENT_ROOT)
						{
							if (reader.AttributeCount > 0)
							{
								Version version = new Version(reader.GetAttribute(nameof(Version)));
								if (version > XML_VERSION)
								{
#if (UNITY_2017_1_OR_NEWER)
									UnityEngine.Debug.LogWarningFormat("Warning version: <{0}>", version.ToString());
#else
									XLogger.LogWarningFormatModule(MODULE_NAME, "Warning version: <{0}>", version.ToString());
#endif
								}
							}
							continue;
						}
						else
						{
							result = ReadDataFromXml(reader, null);
							break;
						}
					}
				}


				// Закрываем поток
				reader.Close();
				text_reader.Close();

#if (UNITY_2017_1_OR_NEWER)
				// Связываем данные
				XSerializatorUnity.LinkSerializeReferences();
#endif

				// Вызываем метод у всех объектов поддерживающих интерфейс сериализации только после полной загрузки объекта 
				UpdateSerializableObjects();

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка списка объектов из файла XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Список объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TType> LoadListFromXml<TType>(String file_name)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Открываем файл
				StringReader string_reader = new StringReader(File.ReadAllText(path));
				return (LoadListFromXml<TType>(string_reader));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка списка объектов из строки в формате XML
			/// </summary>
			/// <typeparam name="TType">Тип объекта списка</typeparam>
			/// <param name="file_data">Строка с данными в формате XML</param>
			/// <returns>Список объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TType> LoadListFromStringXml<TType>(String file_data)
			{
				// Открываем файл
				StringReader string_reader = new StringReader(file_data);
				return (LoadListFromXml<TType>(string_reader));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка списка объектов из потока данных
			/// </summary>
			/// <param name="text_reader">Средство для чтения из потока строковых данных</param>
			/// <returns>Список объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TType> LoadListFromXml<TType>(TextReader text_reader)
			{
				ArrayList array_list = LoadFromXml(text_reader) as ArrayList;
				if (array_list != null)
				{
					List<TType> list = new List<TType>(array_list.Count);
					for (Int32 i = 0; i < array_list.Count; i++)
					{
						list.Add((TType)array_list[i]);
					}

					return (list);
				}

				return null;
			}
			#endregion

			#region ======================================= МЕТОДЫ ОБНОВЛЕНИЯ XML =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объекта из файла XML
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateFromXml(System.Object instance, String file_name)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Открываем поток
				StringReader string_reader = new StringReader(File.ReadAllText(path));
				UpdateFromXml(instance, string_reader);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объекта из строки в формате XML
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="file_data">Строка с данными в формате XML</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateFromStringXml(System.Object instance, String file_data)
			{
				// Открываем поток
				StringReader string_reader = new StringReader(file_data);
				UpdateFromXml(instance, string_reader);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объект из потока данных в формате XML
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="text_reader">Средство для чтения из потока строковых данных</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateFromXml(System.Object instance, TextReader text_reader)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					OnInit();
				}
#endif

#if (UNITY_2017_1_OR_NEWER)
				// Очищаем объекты для связи
				XSerializatorUnity.ClearSerializeReferences();
#endif



				// Открываем поток
				XmlReader reader = XmlReader.Create(text_reader);
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						//
						// Пропускаем корневой объект
						//
						if (reader.Name == XML_NAME_ELEMENT_ROOT)
						{
							if (reader.AttributeCount > 0)
							{
								Version version = new Version(reader.GetAttribute(nameof(Version)));
								if (version > XML_VERSION)
								{
#if (UNITY_2017_1_OR_NEWER)
									UnityEngine.Debug.LogWarningFormat("Warning version: <{0}>", version.ToString());
#else
									XLogger.LogWarningFormatModule(MODULE_NAME, "Warning version: <{0}>", version.ToString());
#endif
								}
							}
							continue;
						}
						else
						{
							ReadDataFromXml(reader, instance);
							break;
						}
					}
				}


				// Закрываем поток
				reader.Close();
				text_reader.Close();

#if (UNITY_2017_1_OR_NEWER)
				// Связываем данные
				XSerializatorUnity.LinkSerializeReferences();
#endif


				// Вызываем метод у всех объектов поддерживающих интерфейс сериализации только после полной загрузки объекта 
				UpdateSerializableObjects();
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ БИНАРНЫХ ДАННЫХ =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения объекта в файл бинарных данных
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToBinary(String file_name, System.Object instance)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Создаем поток для записи
				FileStream file_stream = new FileStream(path, FileMode.Create, FileAccess.Write);
				SaveToBinary(file_stream, instance);
				file_stream.Close();

#if UNITY_EDITOR
				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + path, "OK");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения объекта в файловый поток бинарных данных
			/// </summary>
			/// <param name="file_stream">Файловый поток данных</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToBinary(FileStream file_stream, System.Object instance)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					OnInit();
				}
#endif
				// Открываем поток
				BinaryWriter writer = new BinaryWriter(file_stream);

				// Метка записи начала файла
				writer.Write(XExtensionBinaryStream.SUCCESS_LABEL);

				// Смотрим что за объект
				if (instance is ICubeXSerializeImplementationBinary)
				{
					ICubeXSerializeImplementationBinary serializable_self = instance as ICubeXSerializeImplementationBinary;
					serializable_self.WriteToBinary(writer);
				}
				else
				{
					//
					// Пока не поддерживается
					//
				}

				// Метка записи конца файла
				writer.Write(XExtensionBinaryStream.SUCCESS_LABEL);

				// Закрываем поток
				writer.Close();
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ БИНАРНЫХ ДАННЫХ ===========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка объекта из файла бинарных данных
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object LoadFromBinary(String file_name)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Открываем файл
				FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
				return LoadFromBinary(file_stream);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка объекта из файлового потока бинарных данных
			/// </summary>
			/// <param name="file_stream">Файловый поток данных</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object LoadFromBinary(FileStream file_stream)
			{
				System.Object result = null;
				UpdateFromBinary(result, file_stream);
				return (result);
			}
			#endregion

			#region ======================================= МЕТОДЫ ОБНОВЛЕНИЯ ИЗ БИНАРНЫХ ДАННЫХ ======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объекта из файла бинарных данных
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateFromBinary(System.Object instance, String file_name)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(DefaultPath, file_name, DefaultExt);

				// Открываем файл
				FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
				UpdateFromBinary(instance, file_stream);
				file_stream.Close();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление объект из файлового потока бинарных данных
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="file_stream">Файловый поток данных</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateFromBinary(System.Object instance, FileStream file_stream)
			{
				if (instance == null) return;

#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					OnInit();
				}
#endif

#if (UNITY_2017_1_OR_NEWER)
				// Очищаем объекты для связи
				XSerializatorUnity.ClearSerializeReferences();
#endif
				// Открываем поток
				BinaryReader reader = new BinaryReader(file_stream);

				// Читаем метку записи начала файла
				Int32 label_begin = reader.ReadInt32();
				if (label_begin == XExtensionBinaryStream.SUCCESS_LABEL)
				{
					// Смотрим что за объект
					if (instance is ICubeXSerializeImplementationBinary)
					{
						ICubeXSerializeImplementationBinary serializable_self = instance as ICubeXSerializeImplementationBinary;
						serializable_self.ReadFromBinary(reader);

						// Читаем метку записи окончания файла
						Int32 label_end = reader.ReadInt32();
						if (label_end != XExtensionBinaryStream.SUCCESS_LABEL)
						{
#if (UNITY_2017_1_OR_NEWER)
							UnityEngine.Debug.LogWarning("The file is not fully read");
#else
							XLogger.LogWarningModule(MODULE_NAME, "The file is not fully read");
#endif
						}
					}
					else
					{
						//
						// Пока не поддерживается
						//
					}
				}
				else
				{
#if (UNITY_2017_1_OR_NEWER)
					UnityEngine.Debug.LogWarning("Initial label not found");
#else
					XLogger.LogWarningModule(MODULE_NAME, "Initial label not found");
#endif
				}


#if (UNITY_2017_1_OR_NEWER)
				// Связываем данные
				XSerializatorUnity.LinkSerializeReferences();
#endif

				// Вызываем метод у всех объектов поддерживающих интерфейс сериализации только после полной загрузки объекта 
				UpdateSerializableObjects();

				reader.Close();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================