//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorAttributesHeadersDrawer.cs
*		Редакторы атрибутов отрисовки заголовков секций и групп.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор атрибута HeaderSection
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CubeXHeaderSectionAttribute), true)]
public class CubeXHeaderSectionAttributeDrawer : DecoratorDrawer
{
	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты декоративного элемента
	/// </summary>
	/// <returns>Высота декоративного элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetHeight()
	{
		return (base.GetHeight() + EditorGUIUtility.standardVerticalSpacing * 2);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров декоративного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position)
	{
		CubeXHeaderSectionAttribute section_attribute = attribute as CubeXHeaderSectionAttribute;

		GUIStyle style = EditorStyles.boldLabel;

		// Старые параметры
		Color old_color = style.normal.textColor;
		TextAnchor old_anchor = style.alignment;
		style.fontSize++;

		if (section_attribute.TextColor.Approximately(XUnityColor.Zero, 0.01f))
		{
			section_attribute.TextColor = XEditorStyles.ColorHeaderSection;
		}

		// Новые параметры
		style.normal.textColor = section_attribute.TextColor;
		style.alignment = section_attribute.TextAlignment;

		// Рисуем
		GUI.Label(position, section_attribute.Name, style);

		// Восстанавливаем старые параметры
		style.normal.textColor = old_color;
		style.alignment = old_anchor;
		style.fontSize--;
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор атрибута HeaderSectionBox
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CubeXHeaderSectionBoxAttribute), true)]
public class CubeXHeaderSectionBoxAttributeDrawer : DecoratorDrawer
{
	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты декоративного элемента
	/// </summary>
	/// <returns>Высота декоративного элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetHeight()
	{
		return (base.GetHeight() + (EditorStyles.helpBox.border.top + 
			EditorStyles.helpBox.border.bottom + EditorGUIUtility.standardVerticalSpacing));
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров декоративного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position)
	{
		CubeXHeaderSectionBoxAttribute section_attribute = attribute as CubeXHeaderSectionBoxAttribute;

		GUIStyle style = EditorStyles.boldLabel;

		// Старые параметры
		Color old_color = style.normal.textColor;
		TextAnchor old_anchor = style.alignment;
		style.fontSize++;

		if (section_attribute.TextColor.Approximately(XUnityColor.Zero, 0.01f))
		{
			section_attribute.TextColor = XEditorStyles.ColorHeaderSection;
		}

		// Новые параметры
		style.normal.textColor = section_attribute.TextColor;
		style.alignment = section_attribute.TextAlignment;

		// Рисуем
		position.height -= EditorGUIUtility.standardVerticalSpacing;
		GUI.Box(position, "", EditorStyles.helpBox);
		if (section_attribute.TextAlignment == TextAnchor.UpperLeft ||
			section_attribute.TextAlignment == TextAnchor.MiddleLeft ||
			section_attribute.TextAlignment == TextAnchor.LowerLeft)
		{
			position.x += EditorGUIUtility.standardVerticalSpacing * 2;
		}
		GUI.Label(position, section_attribute.Name, style);

		// Восстанавливаем старые параметры
		style.normal.textColor = old_color;
		style.alignment = old_anchor;
		style.fontSize--;
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор атрибута HeaderGroup
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CubeXHeaderGroupAttribute), true)]
public class CubeXHeaderGroupAttributeDrawer : DecoratorDrawer
{
	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты декоративного элемента
	/// </summary>
	/// <returns>Высота декоративного элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetHeight()
	{
		return (base.GetHeight() + EditorGUIUtility.standardVerticalSpacing * 2);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров декоративного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position)
	{
		CubeXHeaderGroupAttribute group_attribute = attribute as CubeXHeaderGroupAttribute;

		GUIStyle style = EditorStyles.boldLabel;

		// Старые параметры
		Color old_color = style.normal.textColor;
		TextAnchor old_anchor = style.alignment;

		if (group_attribute.TextColor.Approximately(XUnityColor.Zero, 0.01f))
		{
			group_attribute.TextColor = XEditorStyles.ColorHeaderGroup;
		}

		// Новые параметры
		style.normal.textColor = group_attribute.TextColor;
		style.alignment = group_attribute.TextAlignment;

		// Рисуем
		position.x += (group_attribute.Indent * XEditorInspector.OFFSET_INDENT);
		position.width -= (group_attribute.Indent * XEditorInspector.OFFSET_INDENT);
		GUI.Label(position, group_attribute.Name, style);

		// Восстанавливаем старые параметры
		style.normal.textColor = old_color;
		style.alignment = old_anchor;
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор атрибута HeaderGroupBox
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CubeXHeaderGroupBoxAttribute), true)]
public class CubeXHeaderGroupBoxAttributeDrawer : DecoratorDrawer
{
	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты декоративного элемента
	/// </summary>
	/// <returns>Высота декоративного элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetHeight()
	{
		return (base.GetHeight() + (EditorStyles.helpBox.border.top +
			EditorStyles.helpBox.border.bottom));
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров декоративного элемента
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position)
	{
		CubeXHeaderGroupBoxAttribute group_attribute = attribute as CubeXHeaderGroupBoxAttribute;

		GUIStyle style = EditorStyles.boldLabel;

		// Старые параметры
		Color old_color = style.normal.textColor;
		TextAnchor old_anchor = style.alignment;

		if (group_attribute.TextColor.Approximately(XUnityColor.Zero, 0.01f))
		{
			group_attribute.TextColor = XEditorStyles.ColorHeaderGroup;
		}

		// Новые параметры
		style.normal.textColor = group_attribute.TextColor;
		style.alignment = group_attribute.TextAlignment;

		// Рисуем
		position.x += (group_attribute.Indent * XEditorInspector.OFFSET_INDENT);
		position.width -= (group_attribute.Indent * XEditorInspector.OFFSET_INDENT);
		position.height -= EditorGUIUtility.standardVerticalSpacing;
		GUI.Box(position, "", EditorStyles.helpBox);
		if (group_attribute.TextAlignment == TextAnchor.UpperLeft ||
			group_attribute.TextAlignment == TextAnchor.MiddleLeft ||
			group_attribute.TextAlignment == TextAnchor.LowerLeft)
		{
			position.x += EditorGUIUtility.standardVerticalSpacing * 2;
		}
		GUI.Label(position, group_attribute.Name, style);

		// Восстанавливаем старые параметры
		style.normal.textColor = old_color;
		style.alignment = old_anchor;
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================