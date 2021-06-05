//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема настроек сцены и проекта
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSettingsStorageEditor.cs
*		Редактор базового класса-хранилище пользовательских настроек в формате имя-значение.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Common;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор базового класса-хранилище пользовательских настроек в формате имя-значение
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSettingsStorage))]
public class CubeXSettingsStorageEditor : Editor
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected const String mDescriptionStorage = "Хранилища пользовательских настроек в формате имя-значение";
	protected static readonly GUIContent mContentSettingsAdd = new GUIContent("Добавить");
	protected static readonly GUIContent mContentSettingsRemove = new GUIContent("Удалить");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	// Основные данные
	protected CubeXSettingsStorage mStorage;
	protected SerializedProperty mSPUserSettings;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mStorage = this.target as CubeXSettingsStorage;
		mSPUserSettings = this.serializedObject.FindProperty("mUserSettings.mListSettings");
		mStorage.Flush();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		GUILayout.Space(4.0f);
		EditorGUILayout.HelpBox(mDescriptionStorage, MessageType.Info);

		GUILayout.Space(2.0f);
		DrawUserSettings(mStorage.UserSettings, this.serializedObject, mSPUserSettings);

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование пользовательских настроек
	/// </summary>
	/// <param name="user_settings">Пользовательские настройки</param>
	/// <param name="serialized_object">Объект сериализации</param>
	/// <param name="serialized_property">Свойство сериализации</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void DrawUserSettings(CSettingObject user_settings, 
		SerializedObject serialized_object, SerializedProperty serialized_property)
	{
		user_settings.mExpanded = XEditorInspector.DrawSectionFoldout("Пользовательские настройки", user_settings.mExpanded);
		if (user_settings.mExpanded)
		{
			if (serialized_property != null && serialized_property.arraySize > 0)
			{
				for (Int32 i = 0; i < serialized_property.arraySize; i++)
				{
					SerializedProperty item = serialized_property.GetArrayElementAtIndex(i);
					if (item != null)
					{
						if(EditorGUILayout.PropertyField(item))
						{
							serialized_object.Save();
						}
					}
				}
			}

			GUILayout.Space(2.0f);
			EditorGUILayout.BeginHorizontal();
			{
				if (GUILayout.Button(mContentSettingsAdd, GUILayout.ExpandWidth(true)))
				{
					serialized_object.Update();
					user_settings.Add("Default");
					serialized_object.ApplyModifiedProperties();
				}

				EditorGUI.BeginDisabledGroup(user_settings.Count == 0);
				{
					if (GUILayout.Button(mContentSettingsRemove, GUILayout.ExpandWidth(true)))
					{
						serialized_object.Update();

						// create the menu and add items to it
						GenericMenu menu = new GenericMenu();

						for (Int32 i = 0; i < user_settings.Count; i++)
						{
							GUIContent content = new GUIContent();
							content.text = i.ToString() + "_" + user_settings[i].Name;
							menu.AddItem(content, false, (System.Object user_data) =>
							{
								user_settings.mListSettings.Remove(user_data as CSettings);
							}, user_settings[i]);
						}

						// display the menu
						menu.ShowAsContext();

						serialized_object.ApplyModifiedProperties();
					}
				}
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================