//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineBezierCubicEditor.cs
*		Редактор компонента представляющего кубическую кривую Безье.
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
/// Редактор компонента представляющего кубическую кривую Безье
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSplineBezierCubic))]
public class CubeXSplineBezierCubicEditor : CubeXSplineBaseEditor<CubeXSplineBezierCubic>
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		{
			// Основные параметры
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup(mContentGroupMain);

			GUILayout.Space(2.0f);
			mSpline.StartPoint = XEditorInspector.PropertyVector3D(nameof(mSpline.StartPoint), mSpline.StartPoint);

			GUILayout.Space(2.0f);
			mSpline.HandlePoint1 = XEditorInspector.PropertyVector3D(nameof(mSpline.HandlePoint1), mSpline.HandlePoint1);

			GUILayout.Space(2.0f);
			mSpline.HandlePoint2 = XEditorInspector.PropertyVector3D(nameof(mSpline.HandlePoint2), mSpline.HandlePoint2);

			GUILayout.Space(2.0f);
			mSpline.EndPoint = XEditorInspector.PropertyVector3D(nameof(mSpline.EndPoint), mSpline.EndPoint);

			GUILayout.Space(2.0f);
			mSpline.SegmentsSpline = XEditorInspector.PropertyInt(nameof(mSpline.SegmentsSpline), mSpline.SegmentsSpline);

			GUILayout.Space(2.0f);
			mSpline.SnapPlane = (TDimensionPlaneSelect)XEditorInspector.PropertyEnum(nameof(mSpline.SnapPlane), mSpline.SnapPlane);

			GUILayout.Space(2.0f);
			EditorGUILayout.LabelField(nameof(mSpline.Length), mSpline.Length.ToString("F2"));

			GUILayout.Space(2.0f);
			EditorGUILayout.LabelField("CountDraw", mSpline.DrawingPoints.Count.ToString());

			// Рисование параметров отображения сплайна
			DrawVisualSpline();

			// Рисование параметров редактирования контрольной точки
			DrawEditorPoint();

		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}

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
}
//=====================================================================================================================
#endif
//=====================================================================================================================