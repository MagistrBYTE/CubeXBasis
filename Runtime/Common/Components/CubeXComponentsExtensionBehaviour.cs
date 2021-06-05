//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема компонентов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXComponentsExtensionBehaviour.cs
*		Методы расширения функциональности компонента Behaviour.
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
		/// Статический класс реализующий методы функциональности компонента Behaviour
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionBehaviour
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Базовый компонент логики</param>
			/// <param name="enabled">Статус включения компонента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void EnabledComponent<TComponent>(this Behaviour @this, Boolean enabled) where TComponent : Behaviour
			{
				TComponent enabling_component = @this.GetComponent<TComponent>();
				if (enabling_component != null)
				{
					enabling_component.enabled = enabled;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Автоматическое добавление/удаление компонента
			/// </summary>
			/// <typeparam name="TComponent">Тип компонента</typeparam>
			/// <param name="this">Базовый компонент логики</param>
			/// <param name="enabled">Статус добавления/удаление компонента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AutoComponent<TComponent>(this Behaviour @this, Boolean enabled) where TComponent : Behaviour
			{
				TComponent component = @this.GetComponent<TComponent>();
				if (component != null)
				{
					if (enabled == false)
					{
#if UNITY_EDITOR
						component.enabled = false;
						if (UnityEditor.EditorApplication.isPlaying)
						{
							// Удаляем компонент
							GameObject.Destroy(component);
						}
						else
						{
							// Удаляем компонент
							GameObject.DestroyImmediate(component);
						}

#else
						// Удаляем компонент
						GameObject.Destroy(component);
#endif
					}
				}
				else
				{
					if (enabled)
					{
						component = @this.gameObject.AddComponent<TComponent>();
					}
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================