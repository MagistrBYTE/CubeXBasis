//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineCatmullRomEditor.cs
*		Редактор компонента представляющий сплайн CatmullRom.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор компонента представляющий сплайн CatmullRom
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSplineCatmullRom))]
public class CubeXSplineCatmullRomEditor : CubeXSplineBaseEditor<CubeXSplineCatmullRom>
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static readonly GUIContent mContentButtonFirst = new GUIContent("Добавить точку сначала");
	protected static readonly GUIContent mContentButtonLast = new GUIContent("Добавить точку в конце");
	protected static readonly GUIContent mContentButtonStop = new GUIContent("Стоп");
	protected static readonly GUIContent mContentButtonInsertAfter = new GUIContent("Вставить точку после");
	protected static readonly GUIContent mContentButtonInsertBefore = new GUIContent("Вставить точку до");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		// Основные параметры
		GUILayout.Space(4.0f);
		XEditorInspector.DrawGroup(mContentGroupMain);

		GUILayout.Space(2.0f);
		mSpline.IsClosed = XEditorInspector.PropertyBoolean(nameof(mSpline.IsClosed), mSpline.IsClosed);

		// Рисование параметров контрольных точек
		DrawControlPoint(false);

		// Рисование параметров отображения сплайна
		DrawVisualSpline();

		// Рисование параметров редактирования сплайна
		DrawEditorSpline();

		// Рисование параметров редактирования контрольной точки
		DrawEditorPoint();

		GUILayout.Space(2.0f);
	}
	#endregion

	#region =============================================== ОБЩИЕ МЕТОДЫ ==============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Нажатие левой кнопки мыши на узел сплайна для редактирования
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnMouseDownLeft()
	{
		Event current = Event.current;

		// Если есть режим добавления
		if (mAddPointMode != 0)
		{
			Vector3 result;
			if (GetClickPoint(current.mousePosition, Camera.current, out result))
			{
				AddPoint(result);
			}
		}
		else
		{
			Int32 selected = SelectPoint(current.mousePosition, Camera.current);
			if (selected != -1)
			{
				mSelectedIndexPoint = selected;
				current.Use();
			}
			else
			{
				mSelectedIndexPoint = -1;
			}
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Нажатие правой кнопки мыши на узел сплайна для редактирования
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnMouseDownRight()
	{
		Event current = Event.current;
		Int32 index = SelectPoint(current.mousePosition, Camera.current);
		if (index != -1)
		{
			GenericMenu menu = new GenericMenu();
			menu.AddItem(new GUIContent("Delete Point"), false, () => DeletePoint(index));
			menu.AddItem(new GUIContent("Insert Before"), false, () => InsertBeforePoint(index));
			menu.AddItem(new GUIContent("Insert After"), false, () => InsertAfterPoint(index));
			menu.ShowAsContext();
			current.Use();
		}
	}
	#endregion
	
	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров редактирования сплайна
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void DrawEditorSpline()
	{
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup(mContentGroupEditor);

			GUILayout.Space(2.0f);
			Boolean is_last = GUILayout.Toggle(mAddPointMode == 1, mAddPointMode == 1 ? mContentButtonStop : mContentButtonFirst);
			if (is_last) mAddPointMode = 1;

			GUILayout.Space(2.0f);
			Boolean is_first = GUILayout.Toggle(mAddPointMode == 2, mAddPointMode == 2 ? mContentButtonStop : mContentButtonLast);
			if (is_first) mAddPointMode = 2;

			if (!is_last && !is_first) mAddPointMode = 0;

			GUILayout.Space(2.0f);
			if (GUILayout.Button(mContentButtonInsertAfter))
			{
				InsertAfterPoint(mSelectedIndexPoint);
			}

			GUILayout.Space(2.0f);
			if (GUILayout.Button(mContentButtonInsertBefore))
			{
				InsertBeforePoint(mSelectedIndexPoint);
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.Save();
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================