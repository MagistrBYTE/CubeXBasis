//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема компонентов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXComponentsDispatcher.cs
*		Центральный диспетчер компонентов.
*		Центральный диспетчер компонентов обеспечивает более удобный и расширенный функционал для работы с компонентами
*	включая эффективный поиск компонентов по различным критериям, кэширования определенных компонентов, получение
*	интерфейсов.
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
		//! \defgroup UnityCommonComponent Подсистема компонентов
		//! Подсистема компонентов содержит функциональность для работы с компонентами. Подсистема компонентов направлена
		//! на достижение более эффективной работы с компонентами, в особенности эффективного поиска и получение компонентов
		//! по различным критериям, кэшированием определенных типов компонент, упрощения типовых действий с компонентами,
		//! а также получение интерфейсов.
		//! Также подсистема содержит методы расширения для стандартных и пользовательских компонентов.
		//! \ingroup UnityCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер компонентов
		/// </summary>
		/// <remarks>
		/// <para>
		/// Центральный диспетчер компонентов обеспечивает более удобный и расширенный функционал для работы с компонентами
		/// включая эффективный поиск компонентов по различным критериям, кэширования определенных компонентов, получение
		/// интерфейсов.
		/// </para>
		/// <para>
		/// Управляется центральным диспетчером <see cref="CubeXSystemDispatcher"/>.
		/// Если центральный диспетчер не используется, то методы нужно вызывать вручную в соответствующих местах
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XComponentDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Список кэшированных компонентов сгруппированных по типу
			/// </summary>
			public static readonly Dictionary<Type, List<Component>> CachedComponents = new Dictionary<Type, List<Component>>();
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			#endregion

			#region ======================================= ОСНОВНЫЕ МЕТОДЫ ДИСПЕТЧЕРА ================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перезапуск данных центрального диспетчера компонентов в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnResetEditor()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных центрального диспетчера компонентов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void OnInit()
			{
				//if (mCachedComponents == null) mCachedComponents = new Dictionary<Type, List<Component>>();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление компонента
			/// </summary>
			/// <remarks>
			/// Обеспечивается корректное удаление компонента в режиме редактора и в режиме игры
			/// </remarks>
			/// <param name="behaviour">Компонент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Destroy(Behaviour behaviour)
			{
#if UNITY_EDITOR
				if (behaviour != null)
				{
					if (UnityEditor.EditorApplication.isPlaying)
					{
						// Удаляем компонент
						behaviour.enabled = false;
						GameObject.Destroy(behaviour);
					}
					else
					{
						// Удаляем компонент
						GameObject.DestroyImmediate(behaviour);
					}
				}
#else
				// Удаляем объект
				GameObject.Destroy(behaviour);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Кэширование компонентов
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			//---------------------------------------------------------------------------------------------------------
			public static void SetCachedComponents<TComponent>() where TComponent : Component
			{
				TComponent[] scripts = Resources.FindObjectsOfTypeAll<TComponent>();
				List<Component> list_components = null;
				if (CachedComponents.ContainsKey(typeof(TComponent)))
				{
					list_components = CachedComponents[typeof(TComponent)];
				}
				else
				{
					list_components = new List<Component>();
					CachedComponents.Add(typeof(TComponent), list_components);
				}

				for (Int32 i = 0; i < scripts.Length; i++)
				{
					if (scripts[i].gameObject.IsPrefab() == false)
					{
						list_components.Add(scripts[i]);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка всех компонент на сцене (включая не активные)
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <returns>Список всех компонентов на сцене</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TComponent> GetAll<TComponent>() where TComponent : Component
			{
				List<TComponent> list = new List<TComponent>();
				TComponent[] scripts = Resources.FindObjectsOfTypeAll<TComponent>();
				
				for (Int32 i = 0; i < scripts.Length; i++)
				{
					if(scripts[i].gameObject.IsPrefab() == false)
					{
						list.Add(scripts[i]);
					}
				}

				return list;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка всех компонент на сцене удовлетворяющих условию предиката (включая не активные)
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="match">Предикат</param>
			/// <returns>Список всех компонентов на сцене</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TComponent> GetAll<TComponent>(Predicate<TComponent> match) where TComponent : Component
			{
				List<TComponent> list = new List<TComponent>();
				TComponent[] scripts = Resources.FindObjectsOfTypeAll<TComponent>();

				for (Int32 i = 0; i < scripts.Length; i++)
				{
					if (scripts[i].gameObject.IsPrefab() == false && match(scripts[i]))
					{
						list.Add(scripts[i]);
					}
				}

				return list;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение массива всех компонент на сцене (включая не активные)
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <returns>Массив всех компонентов на сцене</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent[] GetAllOfArray<TComponent>() where TComponent : Component
			{
				List<TComponent> list = new List<TComponent>();
				TComponent[] scripts = Resources.FindObjectsOfTypeAll<TComponent>();

				for (Int32 i = 0; i < scripts.Length; i++)
				{
					if (scripts[i].gameObject.IsPrefab() == false)
					{
						list.Add(scripts[i]);
					}
				}

				return list.ToArray();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение массива всех компонент на сцене удовлетворяющих условию предиката (включая не активные)
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="match">Предикат</param>
			/// <returns>Массив всех компонентов на сцене</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent[] GetAllOfArray<TComponent>(Predicate<TComponent> match) where TComponent : Component
			{
				List<TComponent> list = new List<TComponent>();
				TComponent[] scripts = Resources.FindObjectsOfTypeAll<TComponent>();

				for (Int32 i = 0; i < scripts.Length; i++)
				{
					if (scripts[i].gameObject.IsPrefab() == false && match(scripts[i]))
					{
						list.Add(scripts[i]);
					}
				}

				return list.ToArray();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка всех компонент в указанном списке
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="list">Список компонент или игровых объектов</param>
			/// <returns>Список всех компонентов в указанном списке</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<TComponent> GetAll<TComponent>(IList list) where TComponent : Component
			{
				List<TComponent> list_result = new List<TComponent>();

				for (Int32 i = 0; i < list.Count; i++)
				{
					TComponent comp = list[i] as TComponent;
					if (comp != null)
					{
						if (list_result.Contains(comp) == false)
						{
							list_result.Add(comp);
						}
					}
					else
					{
						GameObject go = list[i] as GameObject;
						if (go != null)
						{
							comp = go.GetComponent<TComponent>();
							if (comp != null)
							{
								if (list_result.Contains(comp) == false)
								{
									list_result.Add(comp);
								}
							}
						}
					}
				}

				return list_result;
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОИСКА =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента который может быть только один на сцене
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent Find<TComponent>() where TComponent : Component
			{
				TComponent[] scripts = Resources.FindObjectsOfTypeAll<TComponent>();

				if (scripts != null && scripts.Length > 0)
				{
					return scripts[0];
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента по идентификатору ID игрового объекта
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent Find<TComponent>(Int32 id) where TComponent : Component
			{
				GameObject go_element = XGameObjectDispatcher.Find(id);
				if (go_element == null)
				{
					return null;
				}
				else
				{
					return go_element.GetComponent<TComponent>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента по имени игрового объекта
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="name">Имя игрового объекта</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent Find<TComponent>(String name) where TComponent : Component
			{
				GameObject go_element = XGameObjectDispatcher.Find(name);
				if (go_element == null)
				{
					return null;
				}
				else
				{
					return go_element.GetComponent<TComponent>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента по вхождению имени игрового объекта
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="name">Имя игрового объекта</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent FindMatch<TComponent>(String name) where TComponent : Component
			{
				GameObject go_element = XGameObjectDispatcher.FindMatch(name);
				if (go_element == null)
				{
					return null;
				}
				else
				{
					return go_element.GetComponent<TComponent>();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента по имени игрового объекта в указанном списке
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="name">Имя игрового объекта</param>
			/// <param name="list">Список компонент или игровых объектов</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent FindInList<TComponent>(String name, IList list) where TComponent : Component
			{
				for (Int32 i = 0; i < list.Count; i++)
				{
					Component comp = list[i] as Component;
					if (comp != null)
					{
						if (comp.gameObject.name == name)
						{
							return comp.GetComponent<TComponent>();
						}
					}
					else
					{
						GameObject go = list[i] as GameObject;
						if (go != null && go.name == name)
						{
							return go.GetComponent<TComponent>();
						}
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента по идентификатору ID игрового объекта в указанном списке
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="id">Идентификатор игрового объекта</param>
			/// <param name="list">Список компонент или игровых объектов</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent FindInList<TComponent>(Int32 id, IList list) where TComponent : Component
			{
				for (Int32 i = 0; i < list.Count; i++)
				{
					Component comp = list[i] as Component;
					if (comp != null)
					{
						if (comp.gameObject.GetInstanceID() == id)
						{
							return comp.GetComponent<TComponent>();
						}
					}
					else
					{
						GameObject go = list[i] as GameObject;
						if (go != null && go.GetInstanceID() == id)
						{
							return go.GetComponent<TComponent>();
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