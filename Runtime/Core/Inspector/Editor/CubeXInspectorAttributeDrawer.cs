//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorAttributeDrawer.cs
*		Редактор атрибута Inspector.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор атрибута Inspector
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CubeXInspectorAttribute), true)]
public class CubeXInspectorAttributeDrawer : PropertyDrawer
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
		CubeXInspectorAttribute drawer = this.attribute as CubeXInspectorAttribute;

		// Проверка на коллекцию CubeX
		// Мы должны проверить уровень свойства (0 уровень рисует сам редактор инспектора свойств)
		// Также мы должны проверить что это не элемент коллекции (пока нет возможности правильно рисовать стандартные коллекции через атрибут)
		if (property.depth > 0 && property.IsElementCollection() == false)
		{
			if (fieldInfo.GetAttribute<CubeXReorderableAttribute>() != null)
			{
				CReorderableList list = XReorderableList.Get(property);
				drawer.ReorderableList = list;
				return (list.GetHeight());
			}
		}

		drawer.SetSerializedProperty(property, fieldInfo);
		drawer.GetAttributes();
		drawer.SetSerializedObject(property.serializedObject);

		return (drawer.GetMaxHeight(label));
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование сериализуемого свойства
	/// </summary>
	/// <param name="position">Прямоугольник для отображения</param>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		CubeXInspectorAttribute drawer = this.attribute as CubeXInspectorAttribute;

		// Проверяем на список
		if (property.depth > 0 && drawer.ReorderableList != null)
		{
			CReorderableList list = drawer.ReorderableList as CReorderableList;

			// Небольшая корректировка
			position.xMin += property.depth * XEditorInspector.OFFSET_INDENT / 2;
			list.Draw(position);
		}
		else
		{
			// Только если не элемент коллекции - имя там задавать имя бесполезно
			if (property.IsElementCollection() == false)
			{
				// При необходимости задаем имя
				if (drawer.DisplayName.IsExists())
				{
					label.text = drawer.DisplayName;
				}

				// и подсказку
				if (drawer.Tooltip.IsExists())
				{
					label.tooltip = drawer.Tooltip;
				}
			}

			// Рисуем
			drawer.OnDrawElement(position, property, label);
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================