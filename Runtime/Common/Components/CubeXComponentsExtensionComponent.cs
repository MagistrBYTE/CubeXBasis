//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема компонентов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXComponentsExtensionComponent.cs
*		Методы расширения функциональности базового компонента Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonComponent
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений функциональности базового компонента Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionComponent
		{
			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Гарантированное получение компонента (который может быть только один на игровом объекте)
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Базовый компонент</param>
			/// <returns>Добавленный или существующий компонент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent EnsureComponent<TComponent>(this Component @this) where TComponent : Component
			{
				TComponent exist_component = @this.gameObject.GetComponent<TComponent>();
				if (exist_component == null)
				{
					exist_component = @this.gameObject.AddComponent<TComponent>();
				}

				return exist_component;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление компонента
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Базовый компонент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveComponent<TComponent>(this Component @this) where TComponent : Component
			{
				TComponent deleting_component = @this.GetComponent<TComponent>();
				if (deleting_component != null)
				{
#if UNITY_EDITOR
					if (UnityEditor.EditorApplication.isPlaying)
					{
						// Удаляем компонент
						GameObject.Destroy(deleting_component);
					}
					else
					{
						// Удаляем компонент
						GameObject.DestroyImmediate(deleting_component);
					}

#else
					// Удаляем компонент
					GameObject.Destroy(deleting_component);
#endif
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ДОЧЕРНИМИ ОБЪЕКТАМИ =======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление компонента к дочернему объекту
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Базовый компонент</param>
			/// <param name="child_name">Имя дочернего игрового объекта</param>
			/// <returns>Добавленный или существующий компонент</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent AddChildComponent<TComponent>(this Component @this, String child_name) where TComponent : Component
			{
				TComponent adding_component = null;
				for (Int32 i = 0; i < @this.transform.childCount; i++)
				{
					Transform transform = @this.transform.GetChild(i);
					if (transform.name == child_name)
					{
						adding_component = transform.GetComponent<TComponent>();
						if (adding_component == null)
						{
							adding_component = transform.gameObject.AddComponent<TComponent>();
						}

						return adding_component;
					}
				}

				GameObject go_child = new GameObject(child_name, typeof(TComponent));
				go_child.transform.SetParent(@this.transform, false);
				go_child.layer = @this.gameObject.layer;
				adding_component = go_child.GetComponent<TComponent>();
				return adding_component;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск компонента определенного типа среди дочерних объектов по имени
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Базовый компонент</param>
			/// <param name="child_name">Имя дочернего игрового объекта</param>
			/// <param name="in_children">Дополнительный поиск среди дочерних объектов второго уровня</param>
			/// <returns>Найденный компонент или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent FindComponentInChildren<TComponent>(this Component @this, String child_name, Boolean in_children = false) where TComponent : Component
			{
				// Поиск среди дочерних объектов
				Transform this_transform = @this.transform;
				for (Int32 i = 0; i < this_transform.childCount; i++)
				{
					TComponent find_component = this_transform.GetChild(i).GetComponent<TComponent>();
					if (find_component != null && find_component.name == child_name)
					{
						return (find_component);
					}
				}

				if (in_children)
				{
					for (Int32 ic = 0; ic < this_transform.childCount; ic++)
					{
						Transform transform_child = this_transform.GetChild(ic).transform;
						if (transform_child != null && transform_child.childCount > 0)
						{
							for (Int32 c = 0; c < transform_child.childCount; c++)
							{
								TComponent find_component = transform_child.GetChild(c).GetComponent<TComponent>();
								if (find_component != null && find_component.name == child_name)
								{
									return (find_component);
								}
							}
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