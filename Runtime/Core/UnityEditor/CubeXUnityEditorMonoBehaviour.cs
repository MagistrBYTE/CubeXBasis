﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема расширения функциональности редактора Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXUnityEditorMonoBehaviour.cs
*		Методы расширений базового компонента логики MonoBehaviour.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
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
		/// Статический класс реализующий методы расширения базового компонента логики MonoBehaviour
		/// </summary>
		/// <remarks>
		/// Только для режима редактора Unity
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionMonoBehaviour
		{
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных компонента в режиме редактора
			/// </summary>
			/// <param name="this">Базовый компонент логики</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SaveInEditor(this UnityEngine.MonoBehaviour @this)
			{
				UnityEditor.EditorUtility.SetDirty(@this);
				if (UnityEditor.EditorApplication.isPlaying == false)
				{
					UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(@this.gameObject.scene);
				}
			}
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================