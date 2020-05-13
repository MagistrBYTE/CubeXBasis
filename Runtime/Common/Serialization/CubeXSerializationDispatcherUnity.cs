//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема сериализации Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializationDispatcherUnity.cs
*		Диспетчер подсистемы сериализации данных для Unity.
*		Диспетчер подсистемы сериализации данных для Unity предназначен для управления процессом сериализации данных 
*	на платформе Unity, хранения всех сериализуемых игровых объектов, а также управления вспомогательными процессами
*	связанными с сохранением/загрузкой данных. В текущей версии поддерживается сохранение только в формат XML.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonSerialization
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Диспетчер подсистемы сериализации данных для Unity
		/// </summary>
		/// <remarks>
		/// Диспетчер подсистемы сериализации данных для Unity предназначен для управления процессом сериализации данных
		/// на платформе Unity, хранения всех сериализуемых игровых объектов, а также управления вспомогательными
		/// процессами связанными с сохранением/загрузкой данных.
		/// В текущей версии поддерживается сохранение только в формат XML
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public static class XSerializationDispatcherUnity
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
#if (UNITY_2017_1_OR_NEWER)
#if UNITY_EDITOR
			/// <summary>
			/// Расширение файла по умолчанию
			/// </summary>
			/// <remarks>
			/// В режиме редактора используем расширение .bytes, это позволит работать с текстовым ресурсом, 
			/// но не войдем в проект
			/// </remarks>
			public static readonly String DEFAULT_EXT = ".bytes";
#else
			/// <summary>
			/// Расширение файла по умолчанию
			/// </summary>
			public static readonly String DEFAULT_EXT = ".xml";
#endif
#else
			/// <summary>
			/// Расширение файла по умолчанию
			/// </summary>
			public static readonly String DEFAULT_EXT = ".xml";
#endif
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Список компонентов сериализации
			/// </summary>
			public readonly static List<CubeXSerializationComponent> SerializationComponents = new List<CubeXSerializationComponent>();

			/// <summary>
			/// Набор типов для сохранения
			/// </summary>
			private readonly static TSerializationVolume[] VolumeTypes = new TSerializationVolume[]
			{
				TSerializationVolume.Renderer,
				TSerializationVolume.Light,
				TSerializationVolume.Model,
				TSerializationVolume.Over3D,
				TSerializationVolume.Physics3D,
				TSerializationVolume.Collider3D,
				TSerializationVolume.Joint3D,
				TSerializationVolume.Physics2D,
				TSerializationVolume.Collider2D,
				TSerializationVolume.Joint2D,
				TSerializationVolume.Audio,
				TSerializationVolume.UI
			};

			/// <summary>
			/// Группирование по типам
			/// </summary>
			private static Dictionary<TSerializationVolume, List<Type>> mGroupedComponents;

			// Текущие данные по загрузке
			private static String mCurrentName;
			private static String mCurrentPath;
			private static Int32 mCurrentID;
			private static String mCurrentTag;
			private static String mCurrentLayer;
			private static Boolean mCurrentIsActive;
			private static Int32 mCurrentParentID;
			private static Int64 mCurrentIDSerial;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Группирование по типам
			/// </summary>
			/// <remarks>
			/// Список типов стандартных компонентов сгруппированных по группам.
			/// Каждый компонент принадлежит только одной группе
			/// </remarks>
			public static Dictionary<TSerializationVolume, List<Type>> GroupedComponents
			{
				get
				{
					if (mGroupedComponents == null)
					{
						InitDictionaryVolume();
					}

					return mGroupedComponents;
				}
			}
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск диспетчера подсистемы сериализации данных для Unity в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
				InitDictionaryVolume();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация диспетчера подсистемы сериализации данных для Unity
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				InitDictionaryVolume();
			}
			#endregion

			#region ======================================= РЕГИСТРАЦИЯ КОМПОНЕНТОВ СЕРИАЛИЗАЦИИ ======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Регистрация компонента сериализации на сохранения данных
			/// </summary>
			/// <param name="component">Компонент сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RegisterSerialization(CubeXSerializationComponent component)
			{
				SerializationComponents.Add(component);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена регистрация компонента сериализации на сохранение данных
			/// </summary>
			/// <param name="component">Компонент сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public static void UnRegisterSerialization(CubeXSerializationComponent component)
			{
				SerializationComponents.Remove(component);
			}
			#endregion

			#region ======================================= МЕТОДЫ ГРУППИРОВАНИЯ КОМПОНЕНТОВ ==========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация и распределение стандартных компонентов по группам
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private static void InitDictionaryVolume()
			{
				if (mGroupedComponents == null)
				{
					mGroupedComponents = new Dictionary<TSerializationVolume, List<Type>>();

					// Renderer
					AddTypeToGroup<MeshRenderer>(TSerializationVolume.Renderer);
					AddTypeToGroup<SkinnedMeshRenderer>(TSerializationVolume.Renderer);
					AddTypeToGroup<TrailRenderer>(TSerializationVolume.Renderer);
					AddTypeToGroup<LineRenderer>(TSerializationVolume.Renderer);
					AddTypeToGroup<BillboardRenderer>(TSerializationVolume.Renderer);

					// Light
					AddTypeToGroup<Light>(TSerializationVolume.Light);

					// Model
					AddTypeToGroup<MeshFilter>(TSerializationVolume.Model);

					// Over3D
					AddTypeToGroup<Camera>(TSerializationVolume.Over3D);
					AddTypeToGroup<Animator>(TSerializationVolume.Over3D);

					// Physics3D
					AddTypeToGroup<Rigidbody>(TSerializationVolume.Physics3D);
					AddTypeToGroup<ConstantForce>(TSerializationVolume.Physics3D);

					// Collider3D
					AddTypeToGroup<CharacterController>(TSerializationVolume.Collider3D);
					AddTypeToGroup<BoxCollider>(TSerializationVolume.Collider3D);
					AddTypeToGroup<CapsuleCollider>(TSerializationVolume.Collider3D);
					AddTypeToGroup<MeshCollider>(TSerializationVolume.Collider3D);
					AddTypeToGroup<SphereCollider>(TSerializationVolume.Collider3D);
					AddTypeToGroup<WheelCollider>(TSerializationVolume.Collider3D);

					// Joint3D
					AddTypeToGroup<CharacterJoint>(TSerializationVolume.Joint3D);
					AddTypeToGroup<ConfigurableJoint>(TSerializationVolume.Joint3D);
					AddTypeToGroup<FixedJoint>(TSerializationVolume.Joint3D);
					AddTypeToGroup<HingeJoint>(TSerializationVolume.Joint3D);


					// Physics2D
					AddTypeToGroup<Rigidbody2D>(TSerializationVolume.Physics2D);
					AddTypeToGroup<ConstantForce2D>(TSerializationVolume.Physics2D);
					AddTypeToGroup<AreaEffector2D>(TSerializationVolume.Physics2D);
					AddTypeToGroup<PlatformEffector2D>(TSerializationVolume.Physics2D);
					AddTypeToGroup<PointEffector2D>(TSerializationVolume.Physics2D);
					AddTypeToGroup<SurfaceEffector2D>(TSerializationVolume.Physics2D);

					// Collider2D
					AddTypeToGroup<BoxCollider2D>(TSerializationVolume.Collider2D);
					AddTypeToGroup<CircleCollider2D>(TSerializationVolume.Collider2D);
					AddTypeToGroup<PolygonCollider2D>(TSerializationVolume.Collider2D);
					AddTypeToGroup<EdgeCollider2D>(TSerializationVolume.Collider2D);

					// Joint2D
					AddTypeToGroup<DistanceJoint2D>(TSerializationVolume.Joint2D);
					AddTypeToGroup<HingeJoint2D>(TSerializationVolume.Joint2D);
					AddTypeToGroup<SliderJoint2D>(TSerializationVolume.Joint2D);
					AddTypeToGroup<SpringJoint2D>(TSerializationVolume.Joint2D);
					AddTypeToGroup<WheelJoint2D>(TSerializationVolume.Joint2D);

					// Audio
					AddTypeToGroup<AudioSource>(TSerializationVolume.Audio);
					AddTypeToGroup<AudioListener>(TSerializationVolume.Audio);
					AddTypeToGroup<AudioEchoFilter>(TSerializationVolume.Audio);
					AddTypeToGroup<AudioHighPassFilter>(TSerializationVolume.Audio);
					AddTypeToGroup<AudioLowPassFilter>(TSerializationVolume.Audio);

					// UI
					AddTypeToGroup<UnityEngine.UI.Text>(TSerializationVolume.UI);
					AddTypeToGroup<UnityEngine.UI.Image>(TSerializationVolume.UI);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление типа компонента в определенную группу
			/// </summary>
			/// <typeparam name="TType">Тип</typeparam>
			/// <param name="group">Группа к которой относится данный тип</param>
			//---------------------------------------------------------------------------------------------------------
			private static void AddTypeToGroup<TType>(TSerializationVolume group)
			{
				AddTypeToGroup(typeof(TType), group);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление типа компонента в определенную группу
			/// </summary>
			/// <param name="type">Тип</param>
			/// <param name="group">Группа к которой относится данный тип</param>
			//---------------------------------------------------------------------------------------------------------
			private static void AddTypeToGroup(Type type, TSerializationVolume group)
			{
				if (mGroupedComponents.ContainsKey(group))
				{
					mGroupedComponents[group].Add(type);
				}
				else
				{
					List<Type> types = new List<Type>();
					types.Add(type);
					mGroupedComponents.Add(group, types);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ СЕРИАЛИЗАЦИИ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительное игнорирование чтения/записи некоторых компонентов
			/// </summary>
			/// <param name="type_component">Тип компонента</param>
			/// <returns>Статус игнорирования</returns>
			//---------------------------------------------------------------------------------------------------------
			private static Boolean IgnoreComponentFromType(Type type_component)
			{
				if (type_component.Name == nameof(Transform)) return true;
				if (type_component.Name == nameof(RectTransform)) return true;
				if (type_component.Name == nameof(CubeXSerializationComponent)) return true;
				if (type_component.Name == nameof(CubeXSystemDispatcher)) return true;
				if(type_component.GetAttribute<CubeXSerializeDisableAttribute>() != null) return true;
				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных компонента трансформации в формат аттрибутов XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="game_object">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			private static void WriteGameObjectToXmlAttribute(XmlWriter writer, GameObject game_object)
			{
				writer.WriteAttributeString("Name", game_object.name);
				writer.WriteAttributeString("Path", game_object.GetPathScene());
				writer.WriteAttributeString("ID", game_object.GetInstanceID().ToString());
				writer.WriteAttributeString("Tag", game_object.tag);
				writer.WriteAttributeString("Layer", LayerMask.LayerToName(game_object.layer));
				writer.WriteAttributeString("IsActive", game_object.activeSelf.ToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных компонента трансформации в формат аттрибутов XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="game_object">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			private static void WriteGameObjectTransformToXml(XmlWriter writer, GameObject game_object)
			{
				RectTransform rect_transform = game_object.transform as RectTransform;
				if (rect_transform != null)
				{
					writer.WriteAttributeString("Position2D", rect_transform.anchoredPosition.SerializeToString());
					writer.WriteAttributeString("Position3D", rect_transform.anchoredPosition3D.SerializeToString());
					writer.WriteAttributeString("AnchorMax", rect_transform.anchorMax.SerializeToString());
					writer.WriteAttributeString("AnchorMin", rect_transform.anchorMin.SerializeToString());
					writer.WriteAttributeString("OffsetMax", rect_transform.offsetMax.SerializeToString());
					writer.WriteAttributeString("OffsetMin", rect_transform.offsetMin.SerializeToString());
					writer.WriteAttributeString("Pivot", rect_transform.pivot.SerializeToString());
					writer.WriteAttributeString("SizeDelta", rect_transform.sizeDelta.SerializeToString());
				}
				else
				{
					writer.WriteAttributeString("Position", game_object.transform.localPosition.SerializeToString());
					writer.WriteAttributeString("EulerAngles", game_object.transform.localEulerAngles.SerializeToString());
					writer.WriteAttributeString("LocalScale", game_object.transform.localScale.SerializeToString());
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных и компонентов игрового объекта в формат XML
			/// </summary>
			/// <remarks>
			/// Формируется иерархическая структура XML файла
			/// </remarks>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="game_object">Игровой объект</param>
			/// <param name="depth">Глубина записи</param>
			//---------------------------------------------------------------------------------------------------------
			private static void WriteGameObjectToXml(XmlWriter writer, GameObject game_object, Boolean include_children, Int32 depth)
			{
				// 1) Записываем данные игрового объекта
				writer.WriteStartElement(nameof(GameObject));
				WriteGameObjectToXmlAttribute(writer, game_object);

				// Для объектов верхнего уровня записываем идентификатор родителя.
				// Это необходимо для частных случаем обновления
				if (depth == 0 && game_object.GetPathScene().IsExists())
				{
					if (game_object.transform.parent != null)
					{
						writer.WriteAttributeString("ParentID", game_object.transform.parent.gameObject.GetInstanceID().ToString());
					}
					else
					{
						writer.WriteAttributeString("ParentID", "-1");
					}
				}

				// 2) Записываем компонент трансформации
				WriteGameObjectTransformToXml(writer, game_object);

				// 3) Проверяем на префаб
				//UnityEditor.PrefabType prefab_type = UnityEditor.PrefabUtility.GetPrefabType(game_object);

				// 4) Сохраняем все компоненты
				Component[] components = game_object.GetComponents<Component>();
				for (Int32 i = 0; i < components.Length; i++)
				{
					Type component_type = components[i].GetType();

					// Компоненты которые в любом случае не сохраняются
					if (IgnoreComponentFromType(component_type)) continue;

					XSerializatorUnity.WriteComponentToXml(writer, components[i], component_type);

				}

				// Записываем иерархию объектов
				if (include_children)
				{
					for (Int32 i = 0; i < game_object.transform.childCount; i++)
					{
						GameObject game_object_child = game_object.transform.GetChild(i).gameObject;
						WriteGameObjectToXml(writer, game_object_child, true, depth + 1);
					}
				}

				writer.WriteEndElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных и определённых компонентов игрового объекта в формат XML для режима игры
			/// </summary>
			/// <remarks>
			/// Формируется плоская структура XML файла
			/// </remarks>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="serial">Комопнент сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			private static void WriteGameObjectToXml(XmlWriter writer, CubeXSerializationComponent serial)
			{
				// 1) Получаем игровой объект
				GameObject game_object = serial.gameObject;

				// 2) Записываем данные игрового объекта
				writer.WriteStartElement(nameof(GameObject));
				WriteGameObjectToXmlAttribute(writer, game_object);

				// 3) Записываем данные сериализации
				writer.WriteAttributeString(nameof(serial.IDKeySerial), serial.IDKeySerial.ToString());
				writer.WriteAttributeString(nameof(serial.SerializationVolume), serial.SerializationVolume.ToString());

				// Если это префаб
				if (serial.IsSerializablePrefab)
				{
					// То записываем только трансформацию и статус префаба
					WriteGameObjectTransformToXml(writer, game_object);
				}
				else
				{
					// 4) Если компонент трансформации то записываем его в формат атрибутов
					if (serial.SerializationVolume.IsFlagSet(TSerializationVolume.Transform))
					{
						WriteGameObjectTransformToXml(writer, game_object);
					}

					// 5) Записываем стандартные компоненты по группам
					for (Int32 i = 0; i < VolumeTypes.Length; i++)
					{
						if (serial.SerializationVolume.IsFlagSet(VolumeTypes[i]))
						{
							List<Type> list_types = mGroupedComponents[VolumeTypes[i]];
							for (Int32 ic = 0; ic < list_types.Count; ic++)
							{
								Component component = game_object.GetComponent(list_types[ic]);
								if (component != null)
								{
									XSerializatorUnity.WriteComponentToXml(writer, component, list_types[ic]);
								}
							}
						}
					}

					// 6) Если есть флаг записи пользовательских компонентов
					if (serial.SerializationVolume.IsFlagSet(TSerializationVolume.UserAttribute))
					{
						MonoBehaviour[] components = game_object.GetComponents<MonoBehaviour>();
						for (Int32 i = 0; i < components.Length; i++)
						{
							// Записываем пользовательские компоненты
							Type component_type = components[i].GetType();

							// Компоненты которые в любом случае не сохраняются
							if (IgnoreComponentFromType(component_type)) continue;

							XSerializatorUnity.WriteComponentToXml(writer, components[i], component_type);
						}
					}
				}

				writer.WriteEndElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение параметров игрового объекта из формата атрибутов XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <returns>Игровой объект</returns>
			//---------------------------------------------------------------------------------------------------------
			private static GameObject ReadGameObjectFromXmlAttribute(XmlReader reader)
			{
				GameObject game_object = null;

				// 1) Читаем базовые параметры игрового объекта
				mCurrentName = reader.ReadStringFromAttribute("Name", "no_name");
				mCurrentPath = reader.ReadStringFromAttribute("Path", "no_path");
				mCurrentID = reader.ReadIntegerFromAttribute("ID", -1);
				mCurrentTag = reader.ReadStringFromAttribute("Tag", "Untagged");
				mCurrentLayer = reader.ReadStringFromAttribute("Layer", "Default");
				mCurrentIsActive = reader.ReadBooleanFromAttribute("IsActive", true);
				mCurrentParentID = reader.ReadIntegerFromAttribute("ParentID", -1);
				mCurrentIDSerial = reader.ReadLongFromAttribute("IDKeySerial", -1);

				// 2) Смотрим входит ли объект в подсистему сериализации
				if (mCurrentIDSerial != -1)
				{
					// 3) Ищем в компонентах
					for (Int32 i = 0; i < SerializationComponents.Count; i++)
					{
						if (SerializationComponents[i].IDKeySerial == mCurrentIDSerial)
						{
							// Нашли игровой объект
							game_object = SerializationComponents[i].gameObject;

							// Надо удалить из списка
							SerializationComponents[i].StatusLoad = 1;
						}
					}
				}
				else
				{
					// Ищем
					game_object = XGameObjectDispatcher.Find(mCurrentID);

					// Должно обязательно совпасть имя
					if (game_object == null || game_object.name != mCurrentName)
					{
						// Ищем по имени и тегу
						game_object = XGameObjectDispatcher.Find(mCurrentPath, mCurrentName, mCurrentTag);
					}
				}

				// Если мы никак игровой объект на нашли значит он был уничтожен в runtime
				if (game_object == null)
				{
					Debug.LogFormat("Create object: <{0}>", mCurrentName);

					game_object = new GameObject(mCurrentName);

					// Объект входит в подсистему сериализации
					if (mCurrentIDSerial != -1)
					{
						CubeXSerializationComponent serialization_component = game_object.AddComponent<CubeXSerializationComponent>();
						serialization_component.IDKeySerial = mCurrentIDSerial;

						// Читаем объем сериализации
						serialization_component.SerializationVolume = reader.ReadEnumFromAttribute("SerializationVolume", serialization_component.SerializationVolume);
					}
				}

				// Присваиваем базовые данные
				game_object.name = mCurrentName;
				game_object.tag = mCurrentTag;
				game_object.layer = LayerMask.NameToLayer(mCurrentLayer);
				game_object.SetActive(mCurrentIsActive);

				// Пробуем читать компонент трансформации
				String value;
				if ((value = reader.GetAttribute("Position")) != null)
				{
					game_object.transform.localPosition = XUnityVector3.DeserializeFromString(value);

					if ((value = reader.GetAttribute("EulerAngles")) != null)
					{
						game_object.transform.localEulerAngles = XUnityVector3.DeserializeFromString(value);
					}
					if ((value = reader.GetAttribute("LocalScale")) != null)
					{
						game_object.transform.localScale = XUnityVector3.DeserializeFromString(value);
					}
				}
				else
				{
					// 
					if ((value = reader.GetAttribute("Position2D")) != null)
					{
						RectTransform rect_transform = game_object.transform.EnsureComponent<RectTransform>();
						rect_transform.anchoredPosition = XUnityVector2.DeserializeFromString(value);

						if ((value = reader.GetAttribute("Position3D")) != null)
						{
							rect_transform.anchoredPosition3D = XUnityVector3.DeserializeFromString(value);
						}

						if ((value = reader.GetAttribute("AnchorMax")) != null)
						{
							rect_transform.anchorMax = XUnityVector2.DeserializeFromString(value);
						}

						if ((value = reader.GetAttribute("AnchorMin")) != null)
						{
							rect_transform.anchorMin = XUnityVector2.DeserializeFromString(value);
						}

						if ((value = reader.GetAttribute("OffsetMax")) != null)
						{
							rect_transform.offsetMax = XUnityVector2.DeserializeFromString(value);
						}

						if ((value = reader.GetAttribute("OffsetMin")) != null)
						{
							rect_transform.offsetMin = XUnityVector2.DeserializeFromString(value);
						}

						if ((value = reader.GetAttribute("Pivot")) != null)
						{
							rect_transform.pivot = XUnityVector2.DeserializeFromString(value);
						}

						if ((value = reader.GetAttribute("SizeDelta")) != null)
						{
							rect_transform.sizeDelta = XUnityVector2.DeserializeFromString(value);
						}
					}
				}

				return game_object;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных и компонентов игрового объекта из формата XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="game_object">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			private static void ReadGameObjectFromXml(XmlReader reader, GameObject game_object)
			{
				Int32 depth = reader.Depth;
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element)
					{
						// Если это игровой объект
						if (reader.Name == nameof(GameObject))
						{
							// Если это дочерний игровой объект
							if (depth != reader.Depth)
							{
								// Читаем, находим/создаем игровой объект
								GameObject current_go = ReadGameObjectFromXmlAttribute(reader);

								// Сохраняем иерархические отношения
								current_go.transform.SetParent(game_object.transform, false);

								// Читаем все данные
								XmlReader reader_inner = reader.ReadSubtree();
								ReadGameObjectFromXml(reader_inner, current_go);
								reader_inner.Close();
							}
							else
							{
								continue;
							}
						}
						else
						{
							// Читаем все компоненты
							CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(reader.Name);
							if (serialize_data != null)
							{
								XSerializatorUnity.ReadComponentFromXml(reader, game_object, serialize_data, reader.Name);
							}
							else
							{
								Debug.LogErrorFormat("Unknown type component <{0}>", reader.Name);
							}
						}
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ XML =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения данных и компонентов игрового объекта в файл формата XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <param name="game_object">Игровой объект</param>
			/// <param name="include_children">Статус сохранения дочерних игровых объектов</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(String file_name, GameObject game_object, Boolean include_children)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(XSerializationDispatcher.DefaultPath, file_name, DEFAULT_EXT);

				// Создаем поток для записи
				StreamWriter stream_writer = new StreamWriter(path);
				SaveToXml(stream_writer, game_object, include_children);
				stream_writer.Close();

#if UNITY_EDITOR
				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + path, "OK");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения данных и компонентов игрового объекта в строку в формате XML
			/// </summary>
			/// <param name="game_object">Игровой объект</param>
			/// <param name="include_children">Статус сохранения дочерних игровых объектов</param>
			/// <returns>Строка в формате XML</returns>
			//---------------------------------------------------------------------------------------------------------
			public static StringBuilder SaveToXml(GameObject game_object, Boolean include_children)
			{
				StringBuilder file_data = new StringBuilder(200);

				// Создаем поток для записи
				StringWriter string_writer = new StringWriter(file_data);

				// Сохраняем данные
				SaveToXml(string_writer, game_object, include_children);
				string_writer.Close();

				return (file_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения данных и компонентов игрового объекта в поток данных в формате XML
			/// </summary>
			/// <param name="text_writer">Средство для записи в поток строковых данных</param>
			/// <param name="game_object">Игровой объект</param>
			/// <param name="include_children">Статус сохранения дочерних игровых объектов</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(TextWriter text_writer, GameObject game_object, Boolean include_children)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					// Инициализируем 
					XSerializationDispatcher.OnInit();
					InitDictionaryVolume();
				}
#endif

				// Определяем настройки
				XmlWriterSettings xws = new XmlWriterSettings();
				xws.Indent = true;

				// Открываем средство для записи
				XmlWriter writer = XmlWriter.Create(text_writer, xws);

				// Записываем базовые данные
				writer.WriteStartElement(XSerializationDispatcher.XML_NAME_ELEMENT_ROOT);
				writer.WriteAttributeString(nameof(Version), XSerializationDispatcher.XML_VERSION.ToString());

				// Записываем данные
				WriteGameObjectToXml(writer, game_object, include_children, 0);

				// 4) Закрываем поток
				writer.WriteEndElement();
				writer.Close();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения данных и компонентов списка игровых объектов в файл формата XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <param name="game_objects">Список игровых объектов</param>
			/// <param name="include_children">Статус сохранения дочерних игровых объектов</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(String file_name, IList<GameObject> game_objects, Boolean include_children)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(XSerializationDispatcher.DefaultPath, file_name, DEFAULT_EXT);

				// Создаем поток для записи
				StreamWriter stream_writer = new StreamWriter(path);
				SaveToXml(stream_writer, game_objects, include_children);
				stream_writer.Close();

#if UNITY_EDITOR
				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + path, "OK");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения данных и компонентов списка игровых объектов в строку в формате XML
			/// </summary>
			/// <param name="game_objects">Список игровых объектов</param>
			/// <param name="include_children">Статус сохранения дочерних игровых объектов</param>
			/// <returns>Строка в формате XML</returns>
			//---------------------------------------------------------------------------------------------------------
			public static StringBuilder SaveToXml(IList<GameObject> game_objects, Boolean include_children)
			{
				StringBuilder file_data = new StringBuilder(200);

				// Создаем поток для записи
				StringWriter string_writer = new StringWriter(file_data);

				// Сохраняем данные
				SaveToXml(string_writer, game_objects, include_children);
				string_writer.Close();

				return (file_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения данных и компонентов списка игровых объектов в поток данных в формате XML
			/// </summary>
			/// <param name="text_writer">Средство для записи в поток строковых данных</param>
			/// <param name="game_objects">Список игровых объектов</param>
			/// <param name="include_children">Статус сохранения дочерних игровых объектов</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(TextWriter text_writer, IList<GameObject> game_objects, Boolean include_children)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					// Инициализируем 
					XSerializationDispatcher.OnInit();
					InitDictionaryVolume();
				}
#endif

				// Определяем настройки
				XmlWriterSettings xws = new XmlWriterSettings();
				xws.Indent = true;

				// Открываем средство для записи
				XmlWriter writer = XmlWriter.Create(text_writer, xws);

				// Записываем базовые данные
				writer.WriteStartElement(XSerializationDispatcher.XML_NAME_ELEMENT_ROOT);
				writer.WriteAttributeString(nameof(Version), XSerializationDispatcher.XML_VERSION.ToString());

				// Записываем данные
				for (Int32 i = 0; i < game_objects.Count; i++)
				{
					WriteGameObjectToXml(writer, game_objects[i], include_children, 0);
				}

				// 4) Закрываем поток
				writer.WriteEndElement();
				writer.Close();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения только данных сериалируемых компонентов в файл формата XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(String file_name)
			{
				// Формируем правильный путь
				String path = XFilePath.GetFileName(XSerializationDispatcher.DefaultPath, file_name, DEFAULT_EXT);

				// Создаем поток для записи
				StreamWriter stream_writer = new StreamWriter(path);
				SaveToXml(stream_writer);
				stream_writer.Close();

#if UNITY_EDITOR
				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + path, "OK");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения только данных сериалируемых компонентов в строку в формате XML
			/// </summary>
			/// <returns>Строка в формате XML</returns>
			//---------------------------------------------------------------------------------------------------------
			public static StringBuilder SaveToXml()
			{
				StringBuilder file_data = new StringBuilder(200);

				// Создаем поток для записи
				StringWriter string_writer = new StringWriter(file_data);

				// Сохраняем данные
				SaveToXml(string_writer);
				string_writer.Close();

				return (file_data);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранения только данных сериалируемых компонентов в поток данных в формате XML
			/// </summary>
			/// <param name="text_writer">Средство для записи в поток строковых данных</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveToXml(TextWriter text_writer)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					// Инициализируем 
					XSerializationDispatcher.OnInit();
					InitDictionaryVolume();

					// Получаем все компоненты сериализации
					SerializationComponents.AddRange(XComponentDispatcher.GetAll<CubeXSerializationComponent>());
				}
#endif

				// Определяем настройки
				XmlWriterSettings xws = new XmlWriterSettings();
				xws.Indent = true;

				// Открываем средство для записи
				XmlWriter writer = XmlWriter.Create(text_writer, xws);

				// Записываем базовые данные
				writer.WriteStartElement(XSerializationDispatcher.XML_NAME_ELEMENT_ROOT);
				writer.WriteAttributeString(nameof(Version), XSerializationDispatcher.XML_VERSION.ToString());

				// Записываем данные
				for (Int32 i = 0; i < SerializationComponents.Count; i++)
				{
					WriteGameObjectToXml(writer, SerializationComponents[i]);
				}

				// 4) Закрываем поток
				writer.WriteEndElement();
				writer.Close();
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ XML =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка игровых объектов из файла XML
			/// </summary>
			/// <param name="file_asset">Текстовый ресурс - данные в формате XML</param>
			/// <returns>Список игровых объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<GameObject> LoadFromXml(TextAsset file_asset)
			{
				// 1) Открываем файл
				StringReader string_reader = new StringReader(file_asset.text);
				return(LoadFromXml(string_reader));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка игровых объектов из файла XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			/// <returns>Список игровых объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<GameObject> LoadFromXml(String file_name)
			{
				// 1) Формируем правильный путь
				String path = XFilePath.GetFileName(XSerializationDispatcher.DefaultPath, file_name, DEFAULT_EXT);

				// 2) Открываем файл
				StringReader string_reader = new StringReader(File.ReadAllText(path));
				return (LoadFromXml(string_reader));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка игровых объектов из строки в формате XML
			/// </summary>
			/// <param name="file_data">Строка с данными в формате XML</param>
			/// <returns>Список игровых объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<GameObject> LoadFromStringXml(String file_data)
			{
				// Открываем файл
				StringReader string_reader = new StringReader(file_data);
				return LoadFromXml(string_reader);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка игровых объектов из файла XML
			/// </summary>
			/// <param name="text_reader">Средство для чтения из потока строковых данных</param>
			/// <returns>Список игровых объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<GameObject> LoadFromXml(TextReader text_reader)
			{
#if UNITY_EDITOR
				// Только если в режиме разработки!!!
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					// Инициализируем 
					XSerializationDispatcher.OnInit();
					InitDictionaryVolume();

					// Получаем все компоненты сериализации
					SerializationComponents.AddRange(XComponentDispatcher.GetAll<CubeXSerializationComponent>());
				}
#endif

				// Сбрасываем статус загрузки
				for (Int32 i = 0; i < SerializationComponents.Count; i++)
				{
					SerializationComponents[i].StatusLoad = -1;
				}

				// Очищаем объекты для связи
				XSerializatorUnity.ClearSerializeReferences();

				// Открываем поток
				XmlReader reader = XmlReader.Create(text_reader);

				// Подгружаем данные по игровым объектам
				XGameObjectDispatcher.UpdateCached();

				// Корневой игровой объект
				List<GameObject> root_game_objects = new List<GameObject>();

				// Читаем данные
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name == nameof(GameObject))
					{
						// Читаем, находим/создаем игровой объект
						GameObject current_go = ReadGameObjectFromXmlAttribute(reader);

						// Пробуем искать родителя
						// Только если родительский объект есть
						if (mCurrentPath.IsExists())
						{
							String parent_go_name = mCurrentPath;
							Int32 pos = mCurrentPath.IndexOf('/');
							if (pos != -1)
							{
								parent_go_name = mCurrentPath.Substring(pos + 1);
							}

							// Ищем
							GameObject parent_game_object = XGameObjectDispatcher.Find(mCurrentParentID, parent_go_name);
							if (parent_game_object != null)
							{
								current_go.transform.SetParent(parent_game_object.transform, false);
							}
						}
						else
						{
							// Корневой объект
							root_game_objects.Add(current_go);
						}

						XmlReader reader_inner = reader.ReadSubtree();
						ReadGameObjectFromXml(reader_inner, current_go);
						reader_inner.Close();
					}
				}

				// Подгружаем данные по игровым объектам так как в процесс загрузи мы могли создать новые объекты
				XGameObjectDispatcher.UpdateCached();

				// Закрываем поток
				reader.Close();
				text_reader.Close();

				// Связываем данные
				XSerializatorUnity.LinkSerializeReferences();

				// Вызываем метод у всех объектов поддерживающих интерфейс сериализации только после полной загрузки объекта 
				XSerializationDispatcher.UpdateSerializableObjects();

				// Проверяем
				// -1 - значит объект на загрузился - его надо удалить
				// 0 - объект был создан динамически
				// 1 - существующий объект лишь обновил свои данные
				SerializationComponents.RemoveAll((CubeXSerializationComponent component) =>
				{
					if (component.mStatusLoad == -1)
					{
						XGameObjectDispatcher.Destroy(component.gameObject);
						return true;
					}
					else
					{
						return false;
					}
				});

				return (root_game_objects);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================