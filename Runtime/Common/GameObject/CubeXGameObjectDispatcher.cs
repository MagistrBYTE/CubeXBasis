//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема игровых объектов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXGameObjectDispatcher.cs
*		Центральный диспетчер игровых объектов.
*		Центральный диспетчер игровых объектов обеспечивает унифицированный поиск и предоставление всех доступных
*	игровых объектов, включая неактивные объекты которые находятся на сцене.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonGameObject
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер игровых объектов
		/// </summary>
		/// <remarks>
		/// <para>
		/// Центральный диспетчер игровых объектов обеспечивает унифицированный поиск и предоставление всех доступных
		/// игровых объектов, включая неактивные объекты которые находятся на сцене.
		/// </para>
		/// <para>
		/// Управляется центральным диспетчером <see cref="CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XGameObjectDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Список кэшированных игровых объектов на сцене сгруппированных по тегу
			/// </summary>
			public static readonly Dictionary<String, List<GameObject>> CachedGameObjects = new Dictionary<String, List<GameObject>>(10);
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера игровых объектов в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация центрального диспетчера игровых объектов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				UpdateCached();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление игрового объекта в текущую сцену
			/// </summary>
			/// <param name="go">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			public static void OnGameObjectAdded(GameObject go)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление игрового объекта из текущей сцены
			/// </summary>
			/// <param name="go">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			public static void OnGameObjectRemoved(GameObject go)
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Уничтожение игрового объекта
			/// </summary>
			/// <remarks>
			/// Обеспечивается корректное удаление игрового объекта в режиме редактора и в режиме игры
			/// </remarks>
			/// <param name="go">Игровой объект</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Destroy(GameObject go)
			{
#if UNITY_EDITOR
				if (go != null)
				{
					if (UnityEditor.EditorApplication.isPlaying)
					{
						// Удаляем объект
						GameObject.Destroy(go);
					}
					else
					{
						// Удаляем объект
						GameObject.DestroyImmediate(go);
					}
				}
#else
				// Удаляем объект
				GameObject.Destroy(go);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Построение иерархии объектов по указанному списку данных для хранения иерархической связи
			/// </summary>
			/// <param name="hierarchy">Список объектов</param>
			//---------------------------------------------------------------------------------------------------------
			public static void BuildHierarchy(List<TGameObjectHierarchy> hierarchy)
			{
				for (Int32 id = 0; id < hierarchy.Count; id++)
				{
					if(hierarchy[id].ParentID != -1)
					{
						for (Int32 i = 0; i < hierarchy.Count; i++)
						{
							if(hierarchy[id].ParentID == hierarchy[i].ID)
							{
								// Сопоставляем
								if(hierarchy[id].Object != null && hierarchy[i].Object)
								{
									hierarchy[id].Object.SetParent(hierarchy[i].Object, false);
								}
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных о кэшированных игровых объектов на сцене
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void UpdateCached()
			{
				CachedGameObjects.Clear();
				GameObject[] gos = Resources.FindObjectsOfTypeAll<GameObject>();
				for (Int32 i = 0; i < gos.Length; i++)
				{
					GameObject go = gos[i];

					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;

					if (go.IsPrefab()) continue;

					if (CachedGameObjects.ContainsKey(gos[i].tag))
					{
						CachedGameObjects[gos[i].tag].Add(gos[i]);
					}
					else
					{
						List<GameObject> list = new List<GameObject>();
						list.Add(gos[i]);
						CachedGameObjects.Add(gos[i].tag, list);
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОИСКА =============================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по идентификатору ID
			/// </summary>
			/// <remarks>
			/// Только в режиме редактора
			/// </remarks>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			internal static GameObject FindFromID(Int32 id)
			{
				// В режиме редактора используем его возможности
				UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(id);
				return (obj as GameObject);
			}
#endif

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по идентификатору ID
			/// </summary>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject Find(Int32 id)
			{
#if UNITY_EDITOR
				// В режиме редактора используем его возможности
				UnityEngine.Object obj = UnityEditor.EditorUtility.InstanceIDToObject(id);
				GameObject game_object = obj as GameObject;
				if(game_object != null)
				{
					return game_object;
				}
#endif
				// Ищем стандартным путем
				foreach (var item in CachedGameObjects.Values)
				{
					for (Int32 i = 0; i < item.Count; i++)
					{
						GameObject go = item[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							Int32 id_current = go.GetInstanceID();
							if (id_current == id)
							{
								return go;
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.GetInstanceID() == id)
					{
						Debug.LogFormat("GameObject <{0}> find by ID in editor mode", go.name);
						return go;
					}
				}
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по идентификатору ID и его имени
			/// </summary>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <param name="name">Имя игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject Find(Int32 id, String name)
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
				// Ищем стандартным путем
				foreach (var item in CachedGameObjects.Values)
				{
					for (Int32 i = 0; i < item.Count; i++)
					{
						GameObject go = item[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							Int32 id_current = go.GetInstanceID();

							// Только если сопадает имя
							if (id_current == id && go.name == name)
							{
								return go;
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.GetInstanceID() == id && go.name == name)
					{
						Debug.LogFormat("GameObject <{0}> find by ID and NAME in editor mode", go.name);
						return go;

					}
				}
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по имени
			/// </summary>
			/// <param name="name">Имя игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject Find(String name)
			{
				// Ищем стандартным путем
				GameObject game_object = GameObject.Find(name);
				if(game_object != null)
				{
					return (game_object);
				}

				// Ищем в кэше
				foreach (var item in CachedGameObjects.Values)
				{
					for (Int32 i = 0; i < item.Count; i++)
					{
						GameObject go = item[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							if (go.name == name)
							{
								return go;
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.name == name)
					{
						Debug.LogFormat("GameObject <{0}> find by NAME in editor mode", go.name);
						return go;
					}
				}
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по имени и тегу
			/// </summary>
			/// <param name="name">Имя игрового объекта</param>
			/// <param name="tag">Тэг игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject Find(String name, String tag)
			{
				// Ищем стандартным путем
				List<GameObject> list_tag;
				if(CachedGameObjects.TryGetValue(tag, out list_tag))
				{
					for (Int32 i = 0; i < list_tag.Count; i++)
					{
						GameObject go = list_tag[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							if (go.name == name)
							{
								return go;
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.name == name && go.CompareTag(tag))
					{
						Debug.LogFormat("GameObject <{0}> find by TAG and NAME in editor mode", go.name);
						return go;
					}
				}
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по пути, имени и тегу
			/// </summary>
			/// <param name="path">Путь игрового объекта</param>
			/// <param name="name">Имя игрового объекта</param>
			/// <param name="tag">Тэг игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject Find(String path, String name, String tag)
			{
				// Ищем стандартным путем
				List<GameObject> list_tag;
				if (CachedGameObjects.TryGetValue(tag, out list_tag))
				{
					for (Int32 i = 0; i < list_tag.Count; i++)
					{
						GameObject go = list_tag[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							if (go.name == name)
							{
								String go_path = go.GetPathScene();
								if (go_path == path)
								{
									return go;
								}
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.name == name && go.CompareTag(tag))
					{
						String go_path = go.GetPathScene();
						if (go_path == path)
						{
							Debug.LogFormat("GameObject <{0}> find by PATH and NAME in editor mode", go.name);
							return go;
						}

					}
				}
#endif

				return null;
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
			public static GameObject Find(Int32 id, String path, String name, String tag)
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
				// Ищем стандартным путем
				List<GameObject> list_tag;
				if (CachedGameObjects.TryGetValue(tag, out list_tag))
				{
					for (Int32 i = 0; i < list_tag.Count; i++)
					{
						GameObject go = list_tag[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							if (go.name == name)
							{
								// Проверяем по идентификатору
								if (id != -1)
								{
									if(id == go.GetInstanceID())
									{
										return go;
									}
								}

								String go_path = go.GetPathScene();
								if (go_path == path)
								{
									return go;
								}
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];
					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.name == name && go.CompareTag(tag))
					{
						// Проверяем по идентификатору
						if (id != -1)
						{
							if (id == go.GetInstanceID())
							{
								Debug.LogFormat("GameObject <{0}> find by TAG and NAME in editor mode", go.name);
								return go;
							}
						}

						String go_path = go.GetPathScene();
						if (go_path == path)
						{
							Debug.LogFormat("GameObject <{0}> find by PATH and NAME in editor mode", go.name);
							return go;
						}

					}
				}
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по вхождению имени
			/// </summary>
			/// <param name="name">Часть имени игрового объекта</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject FindMatch(String name)
			{
				// Ищем стандартным путем
				foreach (var item in CachedGameObjects.Values)
				{
					for (Int32 i = 0; i < item.Count; i++)
					{
						GameObject go = item[i];
						if (go != null)
						{
							if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
							if (go.IsPrefab()) continue;

							if (go.name.IndexOf(name) > -1)
							{
								return go;
							}
						}
					}
				}

#if UNITY_EDITOR

				// В режиме редактора ищем дополнительно и сообщаем об этом
				GameObject[] game_objects = Resources.FindObjectsOfTypeAll<GameObject>();

				for (Int32 i = 0; i < game_objects.Length; i++)
				{
					GameObject go = game_objects[i];

					if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave) continue;
					if (go.IsPrefab()) continue;

					if (go.name.IndexOf(name) > -1)
					{
						Debug.LogFormat("GameObject <{0}> find match by NAME in editor mode", go.name);
						return go;

					}
				}
#endif

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по идентификатору ID в указанном списке
			/// </summary>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <param name="list">Список игровых объектов</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject FindInList(Int32 id, IList list)
			{
				for (Int32 i = 0; i < list.Count; i++)
				{
					GameObject go = list[i] as GameObject;
					if (go != null)
					{
						if (go.GetInstanceID() == id)
						{
							return go;
						}
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по имени в указанном списке
			/// </summary>
			/// <param name="name">Имя игрового объекта</param>
			/// <param name="list">Список игровых объектов</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject FindInList(String name, IList list)
			{
				for (Int32 i = 0; i < list.Count; i++)
				{
					GameObject go = list[i] as GameObject;
					if (go != null)
					{
						if (go.name == name)
						{
							return go;
						}
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск игрового объекта по вхождению имени в указанном списке
			/// </summary>
			/// <param name="name">Часть имени игрового объекта</param>
			/// <param name="list">Список игровых объектов</param>
			/// <returns>Найденный игровой объект или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject FindInListMatch(String name, IList list)
			{
				for (Int32 i = 0; i < list.Count; i++)
				{
					GameObject go = list[i] as GameObject;
					if (go != null)
					{
						if (go.name.IndexOf(name) > -1)
						{
							return go;
						}
					}
				}

				return null;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================