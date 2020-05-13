﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема расширения функциональности редактора Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXUnityEditorSerializedObject.cs
*		Методы расширений класса SerializedObject.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleUnityEditor
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения класса SerializedObject
		/// </summary>
		/// <remarks>
		/// Только для режима редактора Unity
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionSerializedObject
		{
#if UNITY_EDITOR
			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение сериализуемого объекта
			/// </summary>
			/// <remarks>
			/// Эта старая методика для информирования об изменении данных - информирование об этом сцены для возможности 
			/// сохранить данные на диск
			/// </remarks>
			/// <param name="serialized_object">Сериализируемый объект</param>
			//---------------------------------------------------------------------------------------------------------
			public static void Save(this UnityEditor.SerializedObject serialized_object)
			{
				UnityEditor.EditorUtility.SetDirty(serialized_object.targetObject);
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					UnityEngine.MonoBehaviour component = serialized_object.targetObject as UnityEngine.MonoBehaviour;
					if (component != null)
					{
						UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
					}
				}
			}
			#endregion

			#region ======================================= РАБОТА С СОХРАНЕНИЕМ/ЗАГРУЗКОЙ ЗНАЧЕНИЯ ===================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени свойства привязанного к конкретному компоненту (игровому объекту)
			/// </summary>
			/// <param name="obj">Сериализируемый объект</param>
			/// <returns>Уникальное имя</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetHashNameInstance(this UnityEditor.SerializedObject obj)
			{
				return (obj.targetObject.GetInstanceID().ToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение имени свойства привязанного к типу
			/// </summary>
			/// <param name="obj">Сериализируемый объект</param>
			/// <returns>Уникальное имя</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetHashNameType(this UnityEditor.SerializedObject obj)
			{
				return (obj.targetObject.GetType().FullName);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение значение по имени свойства привязанного к конкретному компоненту (игровому объекту)
			/// </summary>
			/// <param name="obj">Сериализируемый объект</param>
			/// <param name="prefix">Префикс имени</param>
			/// <param name="value">Значение</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveBoolEditor(this UnityEditor.SerializedObject obj, String prefix, Boolean value)
			{
				UnityEditor.EditorPrefs.SetBool(obj.GetHashNameInstance() + prefix, value);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка значение по имени свойства привязанного к конкретному компоненту (игровому объекту)
			/// </summary>
			/// <param name="obj">Сериализируемый объект</param>
			/// <param name="prefix">Префикс имени</param>
			/// <param name="default_value">Значение по умолчанию</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean LoadEditorBool(this UnityEditor.SerializedObject obj, String prefix, Boolean default_value = true)
			{
				return (UnityEditor.EditorPrefs.GetBool(obj.GetHashNameInstance() + prefix, default_value));
			}
			#endregion
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================