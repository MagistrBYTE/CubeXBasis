//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseSettingsDrawer.cs
*		Редактор для рисования параметров настройки.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров настройки
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CSettingItem))]
public class CubeXBaseSettingItemDrawer : CubeXBaseVariantDrawer
{
	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты редактируемого свойства элемента
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return (base.GetPropertyHeight(property, label));
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Получаем объект
		CSettingItem setting = property.GetValue<CSettingItem>();

		// Имя настройки 
		Rect rect_name = position;
		rect_name.width = EditorGUIUtility.labelWidth - 1;

		EditorGUI.BeginChangeCheck();
		setting.Name = EditorGUI.TextField(rect_name, setting.Name);
		if(EditorGUI.EndChangeCheck())
		{
			property.Save();
		}

		if(DrawFixedVariant(position, setting, null, true, true))
		{
			property.Save();
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров списка настроек
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CSettings))]
public class CubeXBaseSettingsDrawer : PropertyDrawer
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static readonly GUIContent mContentRemove = new GUIContent("X", "Remove this item");
	protected static readonly GUIContent mContentAdd = new GUIContent("+", "Add item of type");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	protected CReorderableList mReorderableList;
	protected String mNameSetting = "Name setting";
	protected TValueType mValueTypeSetting;
	protected CSettings mListSettings;
	protected Boolean mIsRemoveButton;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты редактируемого свойства элемента
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		// Получаем объект
		mListSettings = property.GetValue<CSettings>();

		// Получаем внутренний список
		SerializedProperty inner_collection = property.FindPropertyRelative(nameof(mListSettings.mSettings));
		mReorderableList = XReorderableList.Get(inner_collection);
		mReorderableList.OnContentHeader = GetHeader;

		// Высота списка
		Single total_height = mReorderableList.GetHeight();

		if (mReorderableList.IsExpanded)
		{
			// Если он открыт прибавляем высоту дополнительны элементов управления
			total_height += GetControlsHeight();
		}

		return (total_height);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		property.serializedObject.Update();

		if(mReorderableList.IsExpanded)
		{
			Rect rect_box = position;
			rect_box.x -= 1;
			rect_box.xMax += 1;
			rect_box.y += XEditorInspector.SPACE;
			GUI.Box(rect_box, "", XEditorStyles.BOX_SELECTION);
		}

		// Рисуем список настроек
		position.y += XEditorInspector.SPACE;
		mReorderableList.Draw(position);

		if (mReorderableList.IsExpanded)
		{
			// Смещаем
			position.y = position.yMax - GetControlsHeight() + XEditorInspector.SPACE;
			position.height = XEditorInspector.CONTROL_HEIGHT;

			//Rect rect_box = position;
			//rect_box.height = GetControlsHeight();
			//GUI.Box(rect_box, "", XEditorSetting.BOX_SELECTION);

			// Имя списка
			mListSettings.Name = EditorGUI.TextField(position.Inflate(-2, 0), "Имя списка", mListSettings.Name);

			// Смещаем
			position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;

			// Добавить новое значение
			Rect rect_label = position;
			rect_label.x += 2;
			rect_label.width = EditorGUIUtility.labelWidth;
			EditorGUI.LabelField(rect_label, "Добавить");

			Rect rect_name = position;
			rect_name.x = rect_label.xMax;
			rect_name.width = position.width - rect_label.width - 100 - XEditorInspector.BUTTON_MINI_WIDTH - 3;
			mNameSetting = EditorGUI.TextField(rect_name, mNameSetting);

			Rect rect_type = position;
			rect_type.x = rect_name.xMax;
			rect_type.width = 100;
			mValueTypeSetting = (TValueType)EditorGUI.EnumPopup(rect_type, mValueTypeSetting, EditorStyles.toolbarDropDown);

			Rect rect_button = position;
			rect_button.x = rect_type.xMax + 1;
			rect_button.width = XEditorInspector.BUTTON_MINI_WIDTH;
			rect_button.height = XEditorInspector.BUTTON_MINI_HEIGHT;
			if (GUI.Button(rect_button, mContentAdd, XEditorStyles.ButtonMiniDefaultGreenRightStyle))
			{
				mListSettings.AddFromValueType(mNameSetting, mValueTypeSetting);
			}
		}

		property.serializedObject.ApplyModifiedProperties();
	}
	#endregion

	#region =============================================== ОБЩИЕ МЕТОДЫ ==============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты управляющих элементов
	/// </summary>
	/// <returns>Высота управляющих элементов</returns>
	//-----------------------------------------------------------------------------------------------------------------
	protected Single GetControlsHeight()
	{
		return (XEditorInspector.CONTROL_HEIGHT_SPACE * 2 + XEditorInspector.SPACE * 2);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение надписи заголовка
	/// </summary>
	/// <param name="status">Статус открытия</param>
	/// <returns>Надпись заголовка</returns>
	//-----------------------------------------------------------------------------------------------------------------
	protected String GetHeader(Boolean status)
	{
		String name = "";
		if (mListSettings.Name.IsExists())
		{
			if (fieldInfo.Name != "mListSettings")
			{
				name = fieldInfo.Name + "[" + mListSettings.Name + "]";
			}
			else
			{
				name = mListSettings.Name;
			}
		}
		else
		{
			name = fieldInfo.Name;
		}

		return (name);
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================