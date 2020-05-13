//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializatorUnity.cs
*		Сериализация компонентов и ссылочных данных Unity в различные форматы.
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
using UnityEngine;
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
		/// Класс хранящий данные для связывания поля/свойства ссылочного объекта Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CSerializeReferenceUnity : CSerializeReference
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			//
			// КОДЫ ОБЪЕКТОВ
			//
			/// <summary>
			/// Код игрового объекта
			/// </summary>
			public const Int32 GAME_OBJECT = 1;

			/// <summary>
			/// Код компонента
			/// </summary>
			public const Int32 COMPONENT = 2;

			/// <summary>
			/// Код ресурса
			/// </summary>
			public const Int32 RESOURCE = 3;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Имя игрового объекта
			/// </summary>
			public String Name;

			/// <summary>
			/// Полный путь игрового объекта от корня сцены
			/// </summary>
			public String Path;

			/// <summary>
			/// Тэг игрового объекта
			/// </summary>
			public String Tag;

			/// <summary>
			/// Статус ссылки на префаб
			/// </summary>
			/// <remarks>
			/// Мы можем иметь ссылку как на объект в сцене так и на объект в ресурсах.
			/// Программно - этот же тип объекта, однако его использование значительно отличается, 
			/// поэтому мы должно знать на какой тип объекта имеется ссылка
			/// </remarks>
			public Boolean IsPrefab;
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и установка объекта ссылки
			/// </summary>
			/// <returns>Статус успешности связывания</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Link()
			{
				Boolean status = false;
				switch (CodeObject)
				{
#if UNITY_2017_1_OR_NEWER
					case GAME_OBJECT:
						{
							status = LinkGameObject();
						}
						break;
					case COMPONENT:
						{
							status = LinkComponent();
						}
						break;
					case RESOURCE:
						{
							status = LinkResource();
						}
						break;
#endif
					default:
						break;
				}

				return (status);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и установка объекта ссылки на игровой объект
			/// </summary>
			/// <returns>Статус успешности связывания</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean LinkGameObject()
			{
				if (IsPrefab)
				{
					return LinkResource();
				}

				// Находим игровой объект
				GameObject game_object = XSerializatorUnity.FindGameObject(ID, Path, Name, Tag);
				if (game_object != null)
				{
					// Мы однозначно нашли объект
					Member.SetMemberValue(Instance, game_object, Index);
					Result = game_object;
					return true;
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и установка объекта ссылки на компонент
			/// </summary>
			/// <returns>Статус успешности связывания</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean LinkComponent()
			{
				if (IsPrefab)
				{
					return LinkResource();
				}

				// Получаем данные сериализации по этому типу
				CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(TypeObject);
				if (serialize_data != null)
				{
					Type type = serialize_data.SerializeType;

					// Находим игровой объект
					GameObject game_object = XSerializatorUnity.FindGameObject(ID, Path, Name, Tag);

					if (game_object != null)
					{
						// Мы однозначно нашли объект
						Component component = XSerializatorUnity.EnsureComponent(game_object, type);
						Member.SetMemberValue(Instance, component, Index);
						Result = component;
						return true;
					}
				}
				else
				{
					Debug.LogErrorFormat("Unknown type <{0}>", TypeObject);
				}

				return false;

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и установка объекта ссылки на ресурс
			/// </summary>
			/// <returns>Статус успешности связывания</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean LinkResource()
			{
				// Получаем данные сериализации по этому типу
				CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(TypeObject);
				if (serialize_data != null)
				{
					Type type = serialize_data.SerializeType;

					// Находим ресурс
					UnityEngine.Object resource = XSerializatorUnity.FindResource(ID, Name, type);
					if (resource != null)
					{
						// Мы однозначно нашли объект
						Member.SetMemberValue(Instance, resource, Index);
						Result = resource;
						return (true);
					}
				}
				else
				{
					Debug.LogErrorFormat("Unknown type <{0}>", TypeObject);
				}

				return false;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий сериализацию компонентов и ссылочных данных Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSerializatorUnity
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Список объект для связывания данных
			/// </summary>
			public static readonly List<CSerializeReferenceUnity> SerializeReferences = new List<CSerializeReferenceUnity>();
			#endregion

			#region ======================================= МЕТОДЫ ДЛЯ СВЯЗЫВАНИЯ ДАННЫХ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Связывание ссылочных данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void LinkSerializeReferences()
			{
				for (Int32 i = 0; i < SerializeReferences.Count; i++)
				{
					SerializeReferences[i].Link();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка ссылочных данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void ClearSerializeReferences()
			{
				SerializeReferences.Clear();
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ ОБЪЕКТОВ UNITY ============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на статус префаба игрового объекта
			/// </summary>
			/// <param name="game_object">Игровой объект</param>
			/// <returns>Статус префаба игрового объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsPrefab(GameObject game_object)
			{
				return (game_object.scene.name == null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на статус префаба игрового объекта
			/// </summary>
			/// <param name="component">Компонент</param>
			/// <returns>Статус префаба игрового объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsPrefab(Component component)
			{
				return (component.gameObject.scene.name == null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного пути игрового объект в иерархии игровых объектов
			/// </summary>
			/// <param name="game_object">Игровой объект</param>
			/// <returns>Полный путь</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetPathScene(GameObject game_object)
			{
				StringBuilder path = new StringBuilder(40);
				while (game_object.transform.parent != null)
				{
					game_object = game_object.transform.parent.gameObject;
					if (game_object.transform.parent != null)
					{
						path.Insert(0, game_object.name);
						path.Insert(0, "/");
					}
					else
					{
						path.Insert(0, game_object.name);
					}
				}
				return (path.ToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение полного пути игрового объект в иерархии игровых объектов
			/// </summary>
			/// <param name="component">Компонент</param>
			/// <returns>Полный путь</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetPathScene(Component component)
			{
				return (GetPathScene(component.gameObject));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по по идентификатору ID, пути, имени и тегу
			/// </summary>
			/// <param name="path">Путь игрового объекта</param>
			/// <param name="name">Имя игрового объекта</param>
			/// <param name="tag">Тэг игрового объекта</param>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject FindGameObject(Int32 id, String path, String name, String tag)
			{
#if UNITY_EDITOR
				// В режиме редактора используем его возможности
				UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(id);
				GameObject game_object = obj as GameObject;

				// Только если сопадает имя
				if (game_object != null && game_object.name == name)
				{
					return game_object;
				}
#endif
				// Ищем стандартным путем среди активных игровых объектов
				GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
				for (Int32 i = 0; i < gos.Length; i++)
				{
					GameObject go = gos[i];

					if (go != null)
					{
						// Игнорируем скрытие и системные объекты
						if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;

						// Если это префаб то пропускаем
						if (go.scene.name == null) continue;

						// Только если совпадает имя
						if (go.name == name)
						{
							// Проверяем по идентификатору
							if (id != -1)
							{
								if (id == go.GetInstanceID())
								{
									return go;
								}
							}

							String go_path = GetPathScene(go);
							if (go_path == path)
							{
								return go;
							}
						}
					}
				}

				// Ищем среди всех игровых объектов
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];

					// Игнорируем скрытие и системные объекты
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;

					// Если это префаб то пропускаем
					if (go.scene.name == null) continue;

					if (go.name == name && go.CompareTag(tag))
					{
						// Проверяем по идентификатору
						if (id != -1)
						{
							if (id == go.GetInstanceID())
							{
								return go;
							}
						}

						String go_path = GetPathScene(go);
						if (go_path == path)
						{
							return go;
						}

					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса по идентификатору ID и имени
			/// </summary>
			/// <param name="id">Идентификатор ресурса</param>
			/// <param name="name">Имя ресурса</param>
			/// <param name="resource_type">Тип ресурса</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEngine.Object FindResource(Int32 id, String name, Type resource_type)
			{
#if UNITY_EDITOR
				// В режиме редактора используем его возможности
				UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(id);
				if (obj != null && obj.name == name)
				{
					return obj;
				}
#endif

				// Ищем только среди загруженных ресурсов
				UnityEngine.Object[] resources = Resources.FindObjectsOfTypeAll(resource_type);
				UnityEngine.Object find_from_name = null;
				for (Int32 i = 0; i < resources.Length; i++)
				{
					if (resources[i].name == name)
					{
						find_from_name = resources[i];

						// Если совпадают идентификаторы, то это точно тот ресурс
						if (resources[i].GetInstanceID() == id)
						{
							return resources[i];
						}
					}
				}

				// Если мы нашли ресурс определённого типа по имени то будет считать это 100% совпадением
				if (find_from_name != null)
				{
					return (find_from_name);
				}

#if UNITY_EDITOR
				if (resource_type == typeof(Sprite))
				{
					// Ищем среди не загруженных ресурсов
					String[] paths = UnityEditor.AssetDatabase.FindAssets("t:texture2D");
					for (Int32 i = 0; i < paths.Length; i++)
					{
						paths[i] = UnityEditor.AssetDatabase.GUIDToAssetPath(paths[i]);
						UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(paths[i]);
						if (all_sprites != null && all_sprites.Length > 0)
						{
							for (Int32 isp = 0; isp < all_sprites.Length; isp++)
							{
								if (all_sprites[isp].name == name)
								{
									Debug.LogFormat("Resource <{0}> find by NAME - load from assets", all_sprites[isp].name);
									return all_sprites[isp] as Sprite;
								}
							}
						}
					}

					// Ищем по вхождению
					for (Int32 i = 0; i < paths.Length; i++)
					{
						UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(paths[i]);
						if (all_sprites != null && all_sprites.Length > 0)
						{
							for (Int32 isp = 0; isp < all_sprites.Length; isp++)
							{
								if (all_sprites[isp].name.Contains(name))
								{
									Debug.LogFormat("Resource <{0}> find by NAME - load from assets", all_sprites[isp].name);
									return all_sprites[isp] as Sprite;
								}
							}
						}
					}
				}
				else
				{
					// Ищем среди не загруженных ресурсов
					String[] paths = UnityEditor.AssetDatabase.FindAssets("t:" + resource_type.Name);
					for (Int32 i = 0; i < paths.Length; i++)
					{
						paths[i] = UnityEditor.AssetDatabase.GUIDToAssetPath(paths[i]);
						String resource_name = Path.GetFileNameWithoutExtension(paths[i]);
						if (resource_name == name)
						{
							UnityEngine.Object resource = UnityEditor.AssetDatabase.LoadAssetAtPath(paths[i], resource_type);

							if (resource != null)
							{
								Debug.LogFormat("Resource <{0}> find by NAME - load from assets", resource.name);
								return resource;
							}
						}
					}
				}
#endif
				return null;

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Гарантирование обеспечение компонента (который может быть только один на игровом объекте)
			/// </summary>
			/// <param name="game_object">Игровой объект</param>
			/// <param name="type_component">Тип компонента</param>
			/// <returns>Добавленный или существующий компонент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Component EnsureComponent(GameObject game_object, Type type_component)
			{
#if UNITY_EDITOR
				if (game_object == null)
				{
					return null;
				}
#endif
				Component component = game_object.GetComponent(type_component);
				if (component == null)
				{
					component = game_object.AddComponent(type_component);
				}

				return component;
			}
			#endregion

			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ ССЫЛОК XML ===========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись члена объекта (поля/свойства) компонента как ссылки в формат атрибута XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="component">Компонент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteReferenceComponentToXml(XmlWriter writer, Component component)
			{
				if (component != null)
				{
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Name), component.name);
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Path), GetPathScene(component));
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.ID), component.gameObject.GetInstanceID().ToString());
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Tag), component.tag);
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.IsPrefab), IsPrefab(component).ToString());
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись члена объекта (поля/свойства) игрового объекта как ссылки в формат атрибута XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="game_object">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteReferenceGameObjectToXml(XmlWriter writer, GameObject game_object)
			{
				if (game_object != null)
				{
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Name), game_object.name);
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Path), GetPathScene(game_object));
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.ID), game_object.GetInstanceID().ToString());
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Tag), game_object.tag);
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.IsPrefab), IsPrefab(game_object).ToString());
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись члена объекта (поля/свойства) ресурса в формат атрибута XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="resource">Ресурса</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteReferenceResourceToXml(XmlWriter writer, UnityEngine.Object resource)
			{
				if (resource != null)
				{
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.Name), resource.name);
					writer.WriteAttributeString(nameof(CSerializeReferenceUnity.ID), resource.GetInstanceID().ToString());
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение члена объекта (поля/свойства) компонента из формата атрибута XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="member">Член данных</param>
			/// <param name="index">Индекс списка</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object ReadReferenceComponentFromXml(XmlReader reader, System.Object instance,
				TSerializeDataMember member, Int32 index = -1)
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name != member.Name)
					{
						// Мы перешли на новый элемент
						break;
					}
				}

				// Если атрибутов нет значит ссылка была нулевая
				if (reader.AttributeCount == 0) return null;

				// Создаем объекта для поиска ссылки
				CSerializeReferenceUnity object_link = new CSerializeReferenceUnity();
				object_link.Instance = instance;
				object_link.Index = index;
				object_link.Member = member.MemberData;
				object_link.Name = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Name), "no_name");
				object_link.Path = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Path), "no_path");
				object_link.TypeObject = reader.Name;
				object_link.ID = reader.ReadIntegerFromAttribute(nameof(CSerializeReferenceUnity.ID), -1);
				object_link.Tag = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Tag), "Untagged");
				object_link.IsPrefab = reader.ReadBooleanFromAttribute(nameof(CSerializeReferenceUnity.IsPrefab), false);
				object_link.CodeObject = CSerializeReferenceUnity.COMPONENT;

				// Пробуем искать
				if (object_link.LinkComponent() == false)
				{
					//Не нашли значит ссылка указывает на объект который еще не создан
					SerializeReferences.Add(object_link);
					return (null);
				}
				else
				{
					return (object_link.Result);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение члена объекта (поля/свойства) игрового объекта из формата атрибута XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="member">Член данных</param>
			/// <param name="index">Индекс списка</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object ReadReferenceGameObjectFromXml(XmlReader reader, System.Object instance,
				TSerializeDataMember member, Int32 index = -1)
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name != member.Name)
					{
						// Мы перешли на новый элемент
						break;
					}
				}

				// Если атрибутов нет значит ссылка была нулевая
				if (reader.AttributeCount == 0) return null;

				// Создаем объекта для поиска ссылки
				CSerializeReferenceUnity object_link = new CSerializeReferenceUnity();
				object_link.Instance = instance;
				object_link.Index = index;
				object_link.Member = member.MemberData;
				object_link.TypeObject = nameof(GameObject);
				object_link.Name = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Name), "no_name");
				object_link.Path = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Path), "no_path");
				object_link.ID = reader.ReadIntegerFromAttribute(nameof(CSerializeReferenceUnity.ID), -1);
				object_link.Tag = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Tag), "Untagged");
				object_link.IsPrefab = reader.ReadBooleanFromAttribute(nameof(CSerializeReferenceUnity.IsPrefab), false);
				object_link.CodeObject = CSerializeReferenceUnity.GAME_OBJECT;

				// Пробуем искать
				if (object_link.LinkGameObject() == false)
				{
					//Не нашли значит ссылка указывает на объект который еще не создан
					SerializeReferences.Add(object_link);
					return (null);
				}
				else
				{
					return (object_link.Result);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение члена объекта (поля/свойства) ресурса из формата атрибута XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="member">Член данных</param>
			/// <param name="index">Индекс списка</param>
			/// <returns>Найденный ресурс или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object ReadReferenceResourceFromXml(XmlReader reader, System.Object instance,
				TSerializeDataMember member, Int32 index = -1)
			{
				while (reader.Read())
				{
					if (reader.NodeType == XmlNodeType.Element && reader.Name != member.Name)
					{
						// Мы перешли на новый элемент
						break;
					}
				}

				// Если атрибутов нет значит ссылка была нулевая
				if (reader.AttributeCount == 0) return null;

				// Создаем объекта для поиска ссылки
				CSerializeReferenceUnity object_link = new CSerializeReferenceUnity();
				object_link.Instance = instance;
				object_link.Index = index;
				object_link.Member = member.MemberData;
				object_link.Name = reader.ReadStringFromAttribute(nameof(CSerializeReferenceUnity.Name), "no_name");
				object_link.TypeObject = reader.Name;
				object_link.ID = reader.ReadIntegerFromAttribute(nameof(CSerializeReferenceUnity.ID), -1);
				object_link.CodeObject = CSerializeReferenceUnity.RESOURCE;

				// Пробуем искать
				if (object_link.LinkResource() == false)
				{
					//Не нашли значит ссылка указывает на объект который еще не создан
					SerializeReferences.Add(object_link);
					return null;
				}
				else
				{
					return (object_link.Result);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ КОМПОНЕНТА XML =======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных компонента в формат элемента XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="component">Компонент</param>
			/// <param name="component_type">Тип компонента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteComponentToXml(XmlWriter writer, UnityEngine.Component component, Type component_type)
			{
				// Получаем данные сериализации для указанного типа
				CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(component_type);

				if (serialize_data == null)
				{
					return;
				}

				// Записываем начало элемента
				writer.WriteStartElement(component_type.Name);

				// Если это стандартный компонент Unity
				if (component_type.IsUnityModule())
				{
					for (Int32 i = 0; i < serialize_data.Members.Count; i++)
					{
						XSerializatorObject.WriteMemberToXml(writer, component, serialize_data.Members[i]);
					}
				}
				else
				{
					// Если он может сам себя записать
					if (component is ICubeXSerializeImplementationXML)
					{
						ICubeXSerializeImplementationXML serializable_self = component as ICubeXSerializeImplementationXML;
						serializable_self.WriteToXml(writer);
					}
					else
					{
						// Смотрим, поддерживает ли объект интерфейс сериализации
						ICubeXSerializableObject serializable = component as ICubeXSerializableObject;
						if (serializable != null)
						{
							// Если поддерживает то записываем атрибут
							writer.WriteAttributeString(nameof(ICubeXSerializableObject.IDKeySerial), serializable.IDKeySerial.ToString());
						}

						if (serialize_data.Members != null)
						{
							for (Int32 i = 0; i < serialize_data.Members.Count; i++)
							{
								XSerializatorObject.WriteMemberToXml(writer, component, serialize_data.Members[i]);
							}
						}
					}
				}

				// Записываем окончание элемента
				writer.WriteEndElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных компонента из формата элемента XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="game_object">Игровой объект</param>
			/// <param name="serialize_data">Данные сериализации</param>
			/// <param name="element_name">Имя элемента</param>
			/// <returns>Экземпляр объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public static void ReadComponentFromXml(XmlReader reader, UnityEngine.GameObject game_object, CSerializeData serialize_data, String element_name)
			{
				// Перемещаемся к элементу
				reader.MoveToElement(element_name);

				// Получаем тип компонента
				Type component_type = serialize_data.SerializeType;

				// Добавляем к игровому объекту или получаем
				UnityEngine.Component component = EnsureComponent(game_object, component_type);
				if (component == null)
				{
					UnityEngine.Debug.LogErrorFormat("Component is type <{0}> == null", component_type.Name);
					return;
				}

				// Если это стандартный компонент Unity
				if (component_type.IsUnityModule())
				{
					// Последовательно читаем данные 
					for (Int32 i = 0; i < serialize_data.Members.Count; i++)
					{
						XSerializatorObject.ReadMemberFromXml(reader, component, serialize_data.Members[i]);
					}
				}
				else
				{
					Boolean is_support_serializable = false;
					Int64 id_key_serial = -1;
					Boolean is_existing_object = false;

					// Смотрим, поддерживает ли тип интерфейс сериализации
					if (component_type.IsSupportInterface<ICubeXSerializableObject>())
					{
						is_support_serializable = true;

						// Читаем код сериализации
						id_key_serial = reader.ReadLongFromAttribute(nameof(ICubeXSerializableObject.IDKeySerial), -1);
						if (id_key_serial != -1)
						{
							// Пробуем найти в словаре объектов
							if (XSerializationDispatcher.SerializableObjects.ContainsKey(id_key_serial))
							{
								// Теперь мы просто обновляем данные объекта
								is_existing_object = true;
							}
						}
					}

					// Если объект может сам себя прочитать
					if (component is ICubeXSerializeImplementationXML)
					{
						ICubeXSerializeImplementationXML serializable_self = component as ICubeXSerializeImplementationXML;
						serializable_self.ReadFromXml(reader);
					}
					else
					{
						if (serialize_data.Members != null)
						{
							// Последовательно читаем данные 
							for (Int32 i = 0; i < serialize_data.Members.Count; i++)
							{
								XSerializatorObject.ReadMemberFromXml(reader, component, serialize_data.Members[i]);
							}
						}
					}

					// Если объект поддерживает сериализацию и является созданным
					if (is_support_serializable && is_existing_object == false)
					{
						ICubeXSerializableObject serializable = component as ICubeXSerializableObject;
						XSerializationDispatcher.SerializableObjects.Add(serializable.IDKeySerial, serializable);
					}
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