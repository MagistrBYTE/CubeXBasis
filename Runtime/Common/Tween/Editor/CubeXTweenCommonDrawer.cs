//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenCommonDrawer.cs
*		Редакторы для рисования параметров хранения и управления анимацией.
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
using CubeX.Common;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Набор флагов для рисования параметров анимации
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[Flags]
public enum TTweenDrawParam
{
	/// <summary>
	/// Начальное и целевое значение
	/// </summary>
	StartAndTarget = 1,

	/// <summary>
	/// Индекс кривой для анимации вперед
	/// </summary>
	CurveForward = 2,

	/// <summary>
	/// Индекс кривой для анимации назад
	/// </summary>
	CurveBackward = 4,

	/// <summary>
	/// Режим проигрывания анимации
	/// </summary>
	WrapMode = 8,

	/// <summary>
	/// Продолжительность анимации
	/// </summary>
	Duration = 16,

	/// <summary>
	/// Все флаги
	/// </summary>
	All = StartAndTarget| CurveForward| CurveBackward | WrapMode | Duration
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Базовый класс редактора для рисования параметров класса анимации
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
public class CubeXTweenBaseDrawer : PropertyDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	protected readonly static GUIContent mContentStart = new GUIContent("Start Value");
	protected readonly static GUIContent mContentTarget = new GUIContent("Target Value");
	protected readonly static GUIContent mContentWrap = new GUIContent("Wrap");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (property.isExpanded)
		{
			return (XEditorInspector.CONTROL_HEIGHT_SPACE * 9);
		}
		else
		{
			return (XEditorInspector.CONTROL_HEIGHT_SPACE);
		}
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
		XEditorInspector.PrepareContent(property);

		property.serializedObject.Update();

		EditorGUI.BeginChangeCheck();
		EditorGUI.BeginProperty(position, XEditorInspector.Content, property);
		{
			SerializedProperty property_name = property.FindPropertyRelative(nameof(CTweenInteger.mName));
			SerializedProperty property_start = property.FindPropertyRelative(nameof(CTweenInteger.mStartValue));
			SerializedProperty property_target = property.FindPropertyRelative(nameof(CTweenInteger.mTargetValue));
			SerializedProperty property_forward = property.FindPropertyRelative(nameof(CTweenInteger.mCurveIndexForward));
			SerializedProperty property_backward = property.FindPropertyRelative(nameof(CTweenInteger.mCurveIndexBackward));
			SerializedProperty property_wrap = property.FindPropertyRelative(nameof(CTweenInteger.mWrapMode));
			SerializedProperty property_ignore = property.FindPropertyRelative(nameof(CTweenInteger.mIgnoreTimeScale));
			SerializedProperty property_duration = property.FindPropertyRelative(nameof(CTweenInteger.mDuration));

			// Определяем высоту для отображения свойств
			position.height = XEditorInspector.CONTROL_HEIGHT;
			if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, XEditorInspector.Content))
			{
				EditorGUI.indentLevel++;

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				property_name.stringValue = EditorGUI.TextField(position, "Name", property_name.stringValue);

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				EditorGUI.PropertyField(position, property_start, mContentStart, false);

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				EditorGUI.PropertyField(position, property_target, mContentTarget, false);

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				Rect rect_selected = position;
				rect_selected.width = position.width * 0.7f;

				Rect rect_curve = position;
				rect_curve.x = rect_selected.xMax;
				rect_curve.width = position.width - rect_selected.width;

				property_forward.intValue = EditorGUI.Popup(rect_selected, "Curve Forward", property_forward.intValue,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (property_forward.intValue > -1 && property_forward.intValue < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUI.CurveField(rect_curve, CubeXTweenDispatcher.CurveStorage.Curves[property_forward.intValue].Curve);
				}

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				rect_selected.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				rect_curve.y += XEditorInspector.CONTROL_HEIGHT_SPACE;

				property_backward.intValue = EditorGUI.Popup(rect_selected, "Curve Backward", property_backward.intValue,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (property_backward.intValue > -1 && property_backward.intValue < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUI.CurveField(rect_curve, CubeXTweenDispatcher.CurveStorage.Curves[property_backward.intValue].Curve);
				}

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				EditorGUI.PropertyField(position, property_wrap, mContentWrap, false);

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				property_ignore.boolValue = EditorGUI.Toggle(position, "IgnoreTimeScale", property_ignore.boolValue);

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				property_duration.floatValue = EditorGUI.FloatField(position, "Duration", property_duration.floatValue);

				EditorGUI.indentLevel--;
			}
		}
		EditorGUI.EndProperty();
		if (EditorGUI.EndChangeCheck())
		{
			property.serializedObject.ApplyModifiedProperties();
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров класса для хранения и управления анимации целым значением
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CTweenInteger))]
public class CubeXTweenIntegerDrawer : CubeXTweenBaseDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации целого значения
	/// </summary>
	/// <param name="tween">Анимация целого значения</param>
	/// <param name="draw_param">Набор флагов для рисования параметров</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void Draw(CTweenInteger tween, TTweenDrawParam draw_param = TTweenDrawParam.All)
	{
		if (draw_param.IsFlagSet(TTweenDrawParam.StartAndTarget))
		{
			GUILayout.Space(2.0f);
			tween.StartValue = XEditorInspector.PropertyInt("Start Value", tween.StartValue);

			GUILayout.Space(2.0f);
			tween.TargetValue = XEditorInspector.PropertyInt("Target Value", tween.TargetValue);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveForward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexForward = EditorGUILayout.Popup("Curve Forward", tween.mCurveIndexForward,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexForward > -1 && tween.mCurveIndexForward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexForward].mCurve);

				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveBackward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexBackward = EditorGUILayout.Popup("Curve Backward", tween.mCurveIndexBackward,
						CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexBackward > -1 && tween.mCurveIndexBackward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexBackward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.WrapMode))
		{
			GUILayout.Space(4.0f);
			tween.WrapMode = (TTweenWrapMode)XEditorInspector.PropertyEnum("WrapMode", tween.WrapMode);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.Duration))
		{
			GUILayout.Space(2.0f);
			tween.IgnoreTimeScale = XEditorInspector.PropertyBoolean("IgnoreTimeScale", tween.IgnoreTimeScale);

			GUILayout.Space(2.0f);
			tween.Duration = XEditorInspector.PropertyFloatSlider("Duration", tween.Duration, 0.1f, 4.0f);
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров класса для хранения и управления анимации вещественным значением
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CTweenSingle))]
public class CubeXTweenSingleDrawer : CubeXTweenBaseDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации вещественного значения
	/// </summary>
	/// <param name="tween">Анимация вещественного значения</param>
	/// <param name="draw_param">Набор флагов для рисования параметров</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void Draw(CTweenSingle tween, TTweenDrawParam draw_param = TTweenDrawParam.All)
	{
		if (draw_param.IsFlagSet(TTweenDrawParam.StartAndTarget))
		{
			GUILayout.Space(2.0f);
			tween.StartValue = XEditorInspector.PropertyFloat("Start Value", tween.StartValue);

			GUILayout.Space(2.0f);
			tween.TargetValue = XEditorInspector.PropertyFloat("Target Value", tween.TargetValue);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveForward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexForward = EditorGUILayout.Popup("Curve Forward", tween.mCurveIndexForward,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexForward > -1 && tween.mCurveIndexForward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexForward].mCurve);

				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveBackward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexBackward = EditorGUILayout.Popup("Curve Backward", tween.mCurveIndexBackward,
						CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexBackward > -1 && tween.mCurveIndexBackward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexBackward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.WrapMode))
		{
			GUILayout.Space(4.0f);
			tween.WrapMode = (TTweenWrapMode)XEditorInspector.PropertyEnum("WrapMode", tween.WrapMode);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.Duration))
		{
			GUILayout.Space(2.0f);
			tween.IgnoreTimeScale = XEditorInspector.PropertyBoolean("IgnoreTimeScale", tween.IgnoreTimeScale);

			GUILayout.Space(2.0f);
			tween.Duration = XEditorInspector.PropertyFloatSlider("Duration", tween.Duration, 0.1f, 4.0f);
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров класса для хранения и управления анимации цветовым значением
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CTweenColor))]
public class CubeXTweenColorDrawer : CubeXTweenBaseDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации цветового значения
	/// </summary>
	/// <param name="tween">Анимация цветового значения</param>
	/// <param name="draw_param">Набор флагов для рисования параметров</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void Draw(CTweenColor tween, TTweenDrawParam draw_param = TTweenDrawParam.All)
	{
		if (draw_param.IsFlagSet(TTweenDrawParam.StartAndTarget))
		{
			GUILayout.Space(2.0f);
			tween.StartValue = XEditorInspector.PropertyColor("Start Value", tween.StartValue);

			GUILayout.Space(2.0f);
			tween.TargetValue = XEditorInspector.PropertyColor("Target Value", tween.TargetValue);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveForward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexForward = EditorGUILayout.Popup("Curve Forward", tween.mCurveIndexForward,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexForward > -1 && tween.mCurveIndexForward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexForward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveBackward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexBackward = EditorGUILayout.Popup("Curve Backward", tween.mCurveIndexBackward,
						CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexBackward > -1 && tween.mCurveIndexBackward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexBackward].mCurve);

				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.WrapMode))
		{
			GUILayout.Space(4.0f);
			tween.WrapMode = (TTweenWrapMode)XEditorInspector.PropertyEnum("WrapMode", tween.WrapMode);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.Duration))
		{
			GUILayout.Space(2.0f);
			tween.IgnoreTimeScale = XEditorInspector.PropertyBoolean("IgnoreTimeScale", tween.IgnoreTimeScale);

			GUILayout.Space(2.0f);
			tween.Duration = XEditorInspector.PropertyFloatSlider("Duration", tween.Duration, 0.1f, 4.0f);
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров класса для хранения и управления анимации 2D вектором
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CTweenVector2D))]
public class CubeXTweenVector2DDrawer : CubeXTweenBaseDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации двухмерного вектора
	/// </summary>
	/// <param name="tween">Анимация двухмерного вектора</param>
	/// <param name="draw_param">Набор флагов для рисования параметров</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void Draw(CTweenVector2D tween, TTweenDrawParam draw_param = TTweenDrawParam.All)
	{
		if (draw_param.IsFlagSet(TTweenDrawParam.StartAndTarget))
		{
			GUILayout.Space(2.0f);
			tween.StartValue = XEditorInspector.PropertyVector2D("Start Value", tween.StartValue);

			GUILayout.Space(2.0f);
			tween.TargetValue = XEditorInspector.PropertyVector2D("Target Value", tween.TargetValue);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveForward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexForward = EditorGUILayout.Popup("Curve Forward", tween.mCurveIndexForward,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexForward > -1 && tween.mCurveIndexForward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexForward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveBackward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexBackward = EditorGUILayout.Popup("Curve Backward", tween.mCurveIndexBackward,
						CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexBackward > -1 && tween.mCurveIndexBackward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexBackward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.WrapMode))
		{
			GUILayout.Space(4.0f);
			tween.WrapMode = (TTweenWrapMode)XEditorInspector.PropertyEnum("WrapMode", tween.WrapMode);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.Duration))
		{
			GUILayout.Space(2.0f);
			tween.IgnoreTimeScale = XEditorInspector.PropertyBoolean("IgnoreTimeScale", tween.IgnoreTimeScale);

			GUILayout.Space(2.0f);
			tween.Duration = XEditorInspector.PropertyFloatSlider("Duration", tween.Duration, 0.1f, 4.0f);
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров класса для хранения и управления анимации 3D вектором
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CTweenVector3D))]
public class CubeXTweenVector3DDrawer : CubeXTweenBaseDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации трехмерного вектора
	/// </summary>
	/// <param name="tween">Анимация трехмерного вектора</param>
	/// <param name="draw_param">Набор флагов для рисования параметров</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void Draw(CTweenVector3D tween, TTweenDrawParam draw_param = TTweenDrawParam.All)
	{
		if (draw_param.IsFlagSet(TTweenDrawParam.StartAndTarget))
		{
			GUILayout.Space(2.0f);
			tween.StartValue = XEditorInspector.PropertyVector3D("Start Value", tween.StartValue);

			GUILayout.Space(2.0f);
			tween.TargetValue = XEditorInspector.PropertyVector3D("Target Value", tween.TargetValue);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveForward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexForward = EditorGUILayout.Popup("Curve Forward", tween.mCurveIndexForward,
					CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexForward > -1 && tween.mCurveIndexForward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexForward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.CurveBackward))
		{
			GUILayout.Space(2.0f);
			GUILayout.BeginHorizontal();
			{
				tween.mCurveIndexBackward = EditorGUILayout.Popup("Curve Backward", tween.mCurveIndexBackward,
						CubeXTweenDispatcher.CurveStorage.ListNames);

				if (tween.mCurveIndexBackward > -1 && tween.mCurveIndexBackward < CubeXTweenDispatcher.CurveStorage.Curves.Count)
				{
					EditorGUILayout.CurveField(CubeXTweenDispatcher.CurveStorage.Curves[tween.mCurveIndexBackward].mCurve);
				}
			}
			GUILayout.EndHorizontal();
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.WrapMode))
		{
			GUILayout.Space(4.0f);
			tween.WrapMode = (TTweenWrapMode)XEditorInspector.PropertyEnum("WrapMode", tween.WrapMode);
		}

		if (draw_param.IsFlagSet(TTweenDrawParam.Duration))
		{
			GUILayout.Space(2.0f);
			tween.IgnoreTimeScale = XEditorInspector.PropertyBoolean("IgnoreTimeScale", tween.IgnoreTimeScale);

			GUILayout.Space(2.0f);
			tween.Duration = XEditorInspector.PropertyFloatSlider("Duration", tween.Duration, 0.1f, 4.0f);
		}
	}
	#endregion
}

//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор для рисования параметров класса для хранения и управления анимации спрайтами
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomPropertyDrawer(typeof(CTweenSprite))]
public class CubeXTweenSpriteDrawer : PropertyDrawer
{
	#region =============================================== ДАННЫЕ ====================================================
	protected readonly static GUIContent mContentWrap = new GUIContent("Wrap");
	protected readonly static GUIContent mContentButtonSet = new GUIContent("Set", "Set duration from sprite data");
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение высоты свойства
	/// </summary>
	/// <param name="property">Сериализируемое свойство</param>
	/// <param name="label">Надпись</param>
	/// <returns>Высота свойства элемента</returns>
	//-----------------------------------------------------------------------------------------------------------------
	public override Single GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (property.isExpanded)
		{
			return (XEditorInspector.CONTROL_HEIGHT_SPACE * 8);
		}
		else
		{
			return (XEditorInspector.CONTROL_HEIGHT_SPACE);
		}
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
		XEditorInspector.PrepareContent(property);

		property.serializedObject.Update();

		EditorGUI.BeginChangeCheck();
		EditorGUI.BeginProperty(position, label, property);
		{
			SerializedProperty property_name = property.FindPropertyRelative(nameof(CTweenSprite.mName));
			SerializedProperty property_index = property.FindPropertyRelative(nameof(CTweenSprite.mStorageSpriteIndex));
			SerializedProperty property_start = property.FindPropertyRelative(nameof(CTweenSprite.mStartFrame));
			SerializedProperty property_target = property.FindPropertyRelative(nameof(CTweenSprite.mTargetFrame));
			SerializedProperty property_wrap = property.FindPropertyRelative(nameof(CTweenSprite.mWrapMode));
			SerializedProperty property_duration = property.FindPropertyRelative(nameof(CTweenSprite.mDuration));

			// Определяем высоту для отображения свойств
			position.height = XEditorInspector.CONTROL_HEIGHT;
			if (property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label))
			{
				EditorGUI.indentLevel++;

				position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
				property_name.stringValue = EditorGUI.TextField(position, "Name", property_name.stringValue);

				if (CubeXTweenDispatcher.SpriteStorage.GroupSprites != null)
				{
					if (CubeXTweenDispatcher.SpriteStorage.GroupCount == 0)
					{
						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						EditorGUI.LabelField(position, "Empty storages", EditorStyles.boldLabel);
					}
					else
					{
						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						property_index.intValue = EditorGUI.Popup(position, "Storage Index", property_index.intValue,
							CubeXTweenDispatcher.SpriteStorage.ListNames);

						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						if (CubeXTweenDispatcher.SpriteStorage[property_index.intValue].mSprites[0] != null)
						{
							EditorGUI.ObjectField(position, CubeXTweenDispatcher.SpriteStorage[property_index.intValue].mSprites[0].texture,
								typeof(Texture), false);
						}

						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						Int32 count_frame = CubeXTweenDispatcher.SpriteStorage[property_index.intValue].Count - 1;
						property_start.intValue = EditorGUI.IntSlider(position, "Start Frame", property_start.intValue, 0, count_frame);

						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						String text_target_frame = "Target Frame (max " + count_frame.ToString() + ")";
						property_target.intValue = EditorGUI.IntSlider(position, text_target_frame, property_target.intValue,
							property_start.intValue, count_frame);

						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						EditorGUI.PropertyField(position, property_wrap, mContentWrap, false);

						position.y += XEditorInspector.CONTROL_HEIGHT_SPACE;
						position.width -= 36;
						property_duration.floatValue = EditorGUI.FloatField(position, "Duration", property_duration.floatValue);

						Rect rect_button = position;
						rect_button.x = position.xMax;
						rect_button.width = 36;

						if (GUI.Button(rect_button, mContentButtonSet, EditorStyles.miniButtonRight))
						{
							property_duration.floatValue = CubeXTweenDispatcher.SpriteStorage[property_index.intValue].TimeAnimation;
						}
					}
				}
				EditorGUI.indentLevel--;
			}
		}
		EditorGUI.EndProperty();
		if (EditorGUI.EndChangeCheck())
		{
			property.serializedObject.ApplyModifiedProperties();
		}
	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров анимации спрайта
	/// </summary>
	/// <param name="tween">Анимация спрайта</param>
	/// <param name="draw_param">Набор флагов для рисования параметров</param>
	//-----------------------------------------------------------------------------------------------------------------
	public static void Draw(CTweenSprite tween, TTweenDrawParam draw_param = TTweenDrawParam.All)
	{
		if (CubeXTweenDispatcher.SpriteStorage.GroupSprites != null && CubeXTweenDispatcher.SpriteStorage.GroupCount != 0)
		{
			GUILayout.Space(2.0f);
			Int32 count_frame = CubeXTweenDispatcher.SpriteStorage[tween.StorageSpriteIndex].Count - 1;
			tween.StartFrame = XEditorInspector.PropertyIntSlider("Start Frame", tween.StartFrame, 0, count_frame);

			GUILayout.Space(2.0f);
			String text_target_frame = "Target Frame (max " + count_frame.ToString() + ")";
			tween.TargetFrame = XEditorInspector.PropertyIntSlider(text_target_frame, tween.TargetFrame, tween.StartFrame, count_frame);
		}

		else
		{
			GUILayout.Space(2.0f);
			tween.StartFrame = XEditorInspector.PropertyInt("Start Frame", tween.StartFrame);

			GUILayout.Space(2.0f);
			tween.TargetFrame = XEditorInspector.PropertyInt("Target Frame", tween.TargetFrame);
		}

		if (CubeXTweenDispatcher.SpriteStorage.GroupSprites != null)
		{
			if (CubeXTweenDispatcher.SpriteStorage.GroupCount == 0)
			{
				XEditorInspector.DrawSection("Empty storages");
			}
			else
			{
				GUILayout.Space(2.0f);
				XEditorInspector.PropertyResource("Sprite", CubeXTweenDispatcher.SpriteStorage[tween.StorageSpriteIndex].mSprites[0].texture);

				GUILayout.Space(2.0f);
				tween.StorageSpriteIndex = EditorGUILayout.Popup("Storage Index", tween.StorageSpriteIndex,
					CubeXTweenDispatcher.SpriteStorage.ListNames);

				GUILayout.Space(2.0f);
				tween.WrapMode = (TTweenWrapMode)XEditorInspector.PropertyEnum("WrapMode", tween.WrapMode);

				GUILayout.Space(2.0f);
				tween.Duration = XEditorInspector.PropertyFloat("Duration", tween.Duration);
			}
		}
	}
	#endregion
}
//=====================================================================================================================
#endif
//=====================================================================================================================