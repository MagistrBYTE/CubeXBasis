//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineBezierPathEditor.cs
*		Редактор компонента представляющий сплайн BezierPath.
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
/// Редактор компонента представляющий сплайн BezierPath
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSplineBezierPath))]
public class CubeXSplineBezierPathEditor : CubeXSplineBaseEditor<CubeXSplineBezierPath>
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static readonly GUIContent mContentAddCurve = new GUIContent("Добавить кривую");
	protected static readonly GUIContent mContentRemoveCurve = new GUIContent("Удалить последнюю кривую");
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

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Нажатие правой кнопки мыши на узел сплайна для редактирования
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void OnMouseDownRight()
	{

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
			if (GUILayout.Button(mContentAddCurve))
			{
				mSpline.AddCurve();
			}

			GUILayout.Space(2.0f);
			if (GUILayout.Button(mContentRemoveCurve))
			{
				mSpline.RemoveCurve();
				if (mSelectedIndexPoint > mSpline.CountPoints)
				{
					mSelectedIndexPoint = mSpline.CountPoints - 1;
				}
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров редактирования контрольной точки
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected override void DrawEditorPoint()
	{
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup(mContentGroupSelected);

			if (mSelectedIndexPoint != -1)
			{
				GUILayout.Space(2.0f);
				EditorGUILayout.LabelField("Selected point", mSelectedIndexPoint.ToString());

				GUILayout.Space(2.0f);
				GUI.changed = false;
				mSelectedPoint = XEditorInspector.PropertyVector3D("World position", mSpline.GetControlPointWorld(mSelectedIndexPoint));
				if (GUI.changed)
				{
					mSpline.SetControlPointWorld(mSelectedIndexPoint, mSelectedPoint, true);
				}

				if (mSpline.IsHandlePoint(mSelectedIndexPoint))
				{
					GUILayout.Space(2.0f);
					EditorGUI.BeginChangeCheck();
					TBezierHandleMode mode = (TBezierHandleMode)XEditorInspector.PropertyEnum("Mode", mSpline.GetHandleMode(mSelectedIndexPoint));
					if (EditorGUI.EndChangeCheck())
					{
						Undo.RecordObject(mSpline, "Change Point Mode");
						mSpline.SetHandleMode(mSelectedIndexPoint, mode);
						this.serializedObject.Save();
					}
				}
			}
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================