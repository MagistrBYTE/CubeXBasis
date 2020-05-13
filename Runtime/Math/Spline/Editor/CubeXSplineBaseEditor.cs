//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineBaseEditor.cs
*		Редактор базового компонента представляющего сплайн.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
using CubeX.Maths;
using CubeX.Editor;
//=====================================================================================================================
//---------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Редактор базового компонента представляющего сплайн
/// </summary>
//---------------------------------------------------------------------------------------------------------------------
[CustomEditor(typeof(CubeXSplineBase))]
public class CubeXSplineBaseEditor<TTypeSpline> : Editor where TTypeSpline : CubeXSplineBase
{
	#region =============================================== СТАТИЧЕСКИЕ ДАННЫЕ ========================================
	protected static readonly GUIContent mContentGroupMain = new GUIContent("Main setting");
	protected static readonly GUIContent mContentGroupDraw = new GUIContent("Draw spline");
	protected static readonly GUIContent mContentGroupEditor = new GUIContent("Editor spline");
	protected static readonly GUIContent mContentGroupSelected = new GUIContent("Selected point");
	#endregion

	#region =============================================== ДАННЫЕ ====================================================
	// Основные данные
	protected TTypeSpline mSpline;
	protected Int32 mSelectedIndexPoint;
	protected Vector3 mSelectedPoint;
	protected Int32 mAddPointMode;
	protected Vector2 mScrollPosition;
	#endregion

	#region =============================================== СОБЫТИЯ UNITY =============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Включение скрипта в инспекторе объектов
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnEnable()
	{
		mSpline = this.target as TTypeSpline;
		if (mSpline.SegmentsPath == null)
		{
			mSpline.SegmentsPath = new List<TMoveSegment>();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование на сцене
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	public void OnSceneGUI()
	{
		Transform spline_transform = mSpline.transform;
		if (mSelectedIndexPoint != -1 && mSelectedIndexPoint < mSpline.CountPoints)
		{
			// Получаем точку кривой в мировых координатах
			Vector3 position_old = mSpline.GetControlPointWorld(mSelectedIndexPoint);

			// Точка перемещения ручки в мировых координатах
			Vector3 position_new = Handles.PositionHandle(position_old, Quaternion.identity);

			// Если они неравны
			if (position_old != position_new)
			{
				switch (mSpline.SnapPlane)
				{
					case TDimensionPlaneSelect.None:
						break;
					case TDimensionPlaneSelect.XZ:
						position_new = new Vector3(position_new.x, position_old.y, position_new.z);
						break;
					case TDimensionPlaneSelect.XY:
						position_new = new Vector3(position_new.x, position_new.y, position_old.z);
						break;
					case TDimensionPlaneSelect.ZY:
						position_new = new Vector3(position_old.x, position_new.y, position_new.z);
						break;
					default:
						break;
				}

				// Перемещаем точку в новую позицию
				MovePoint(position_new);
			}

			Handles.DotHandleCap(-1, position_old, Quaternion.identity, HandleUtility.GetHandleSize(position_old) * .1f, EventType.Repaint);
		}

		Event current = Event.current;
		if (current.type == EventType.MouseDown)
		{
			switch (current.button)
			{
				// Левая кнопка мыши
				case 0:
					{
						OnMouseDownLeft();

						Repaint();
					}
					break;

				// Правая кнопка мыши
				case 1:
					{
						OnMouseDownRight();

						Repaint();
					}
					break;
				default:
					break;
			}
		}

		if (mSelectedIndexPoint == -1 || (mAddPointMode != 0))
		{
			Selection.activeGameObject = mSpline.gameObject;
		}
	}
	#endregion

	#region =============================================== РАБОТА С ТОЧКАМИ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Выбор точки для модификации
	/// </summary>
	/// <param name="screen_point">Точка в экранных координатах</param>
	/// <param name="camera">Камера</param>
	/// <returns>Индекс выбранной точки или -1</returns>
	//-----------------------------------------------------------------------------------------------------------------
	protected Int32 SelectPoint(Vector2 screen_point, Camera camera)
	{
		Single rad_sqr = 120;
		for (Int32 i = 0; i < mSpline.CountPoints; ++i)
		{
			// Получаем вершину и трансформируем в мировое пространство
			Vector3 vertex = mSpline.GetControlPointWorld(i);

			// Проецируем на экран
			Vector2 point = HandleUtility.WorldToGUIPoint(vertex);

			Single delta_x = screen_point.x - point.x;
			Single delta_x_sqr = delta_x * delta_x;
			if (delta_x_sqr > rad_sqr) continue;

			Single delta_y = screen_point.y - point.y;
			Single delta_y_sqr = delta_y * delta_y;
			if (delta_y_sqr > rad_sqr) continue;

			if (delta_x_sqr + delta_y_sqr > rad_sqr)
			{
				continue;
			}
			else
			{
				return (i);
			}
		}
		return (-1);
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Перемещения выбранной точки сплайна
	/// </summary>
	/// <param name="new_position">Новая позиция точки сплайна в мировых координатах</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void MovePoint(Vector3 new_position)
	{
		// Записываем откат
		Undo.RecordObject(mSpline, "Move point");

		// Полностью обновляем кривую
		mSpline.SetControlPointWorld(mSelectedIndexPoint, new_position, true);

		// Сохраняем
		this.serializedObject.Save();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавление точки к сплайну
	/// </summary>
	/// <param name="point">Точка сплайна в мировых координатах</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void AddPoint(Vector3 point)
	{
		// Режим добавления в конце
		if (mAddPointMode == 1)
		{
			// Записываем откат
			Undo.RecordObject(mSpline, "Add point");

			// Полностью обновляем кривую
			mSpline.AddControlPointWorld(point, true);

			// Сохраняем
			this.serializedObject.Save();
		}
		else
		{
			// Записываем откат
			Undo.RecordObject(mSpline, "Add point");

			// Полностью обновляем кривую
			mSpline.InsertControlPointWorld(0, point, true);

			// Сохраняем
			this.serializedObject.Save();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Удаление точки сплайна по индексу
	/// </summary>
	/// <param name="index">Позиция(индекс) удаляемой точки</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void DeletePoint(Int32 index)
	{
		// Записываем откат
		Undo.RecordObject(mSpline, "Delete Point");

		// Удаляем точку
		mSpline.RemoveControlPoint(index, true);

		// Сохраняем
		this.serializedObject.Save();
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавление точки посередине к сплайну перед указанной точки
	/// </summary>
	/// <param name="index">Индекс точки перед которой добавляется новая точка</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void InsertBeforePoint(Int32 index)
	{
		if (index > 0)
		{
			// Записываем откат
			Undo.RecordObject(mSpline, "Insert Point");

			// Считаем точку посередине
			Vector3 p1 = mSpline.GetControlPoint(index);
			Vector3 p2 = mSpline.GetControlPoint(index - 1);

			// Добавляем точку посередине
			mSpline.InsertControlPointWorld(index - 1, (p1 + p2) / 2, true);

			// Сохраняем
			this.serializedObject.Save();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Добавление точки посередине к сплайну после указанной точки
	/// </summary>
	/// <param name="index">Индекс точки после которой добавляется новая точка</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected void InsertAfterPoint(Int32 index)
	{
		if (index < mSpline.CountPoints - 1)
		{
			// Записываем откат
			Undo.RecordObject(mSpline, "Insert Point");

			// Считаем точку посередине
			Vector3 p1 = mSpline.GetControlPoint(index);
			Vector3 p2 = mSpline.GetControlPoint(index + 1);

			// Добавляем точку посередине
			mSpline.InsertControlPointWorld(index + 1, (p1 + p2) / 2, true);

			// Сохраняем
			this.serializedObject.Save();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Получение точки в пространстве
	/// </summary>
	/// <param name="screen_point">Точка в экранных координатах</param>
	/// <param name="camera">Камера</param>
	/// <param name="position">Результирующая точка в пространстве</param>
	/// <returns>Статус успешности получения точки</returns>
	//-----------------------------------------------------------------------------------------------------------------
	protected Boolean GetClickPoint(Vector2 screen_point, Camera camera, out Vector3 position)
	{
		// Получаем луч
		Ray ray = HandleUtility.GUIPointToWorldRay(screen_point);

		// Строим плоскость
		Plane plane = new Plane();
		TDimensionPlaneSelect plane_create = mSpline.SnapPlane;
		switch (plane_create)
		{
			case TDimensionPlaneSelect.XZ:
				{
					plane = new Plane(Vector3.up, Vector3.zero);
				}
				break;
			case TDimensionPlaneSelect.XY:
				{
					plane = new Plane(Vector3.forward, Vector3.zero);
				}
				break;
			case TDimensionPlaneSelect.ZY:
				{
					plane = new Plane(Vector3.right, Vector3.zero);
				}
				break;
			default:
				{

				}
				break;
		}

		// Пускаем луч и смотрим есть ли пересечение
		Single distance;
		if (plane.Raycast(ray, out distance))
		{
			position = ray.GetPoint(distance);
			position = mSpline.transform.InverseTransformPoint(position);
			return (true);
		}

		position = Vector3.zero;
		return (false);
	}
	#endregion

	#region =============================================== ОБЩИЕ МЕТОДЫ ==============================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Нажатие левой кнопки мыши на узел сплайна для редактирования
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void OnMouseDownLeft()
	{

	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Нажатие правой кнопки мыши на узел сплайна для редактирования
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void OnMouseDownRight()
	{

	}
	#endregion

	#region =============================================== МЕТОДЫ РИСОВАНИЯ ==========================================
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров контрольных точек
	/// </summary>
	/// <param name="is_remove">Возможность удаления контрольной точки</param>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void DrawControlPoint(Boolean is_remove)
	{
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(2.0f);
			mSpline.StartPoint = XEditorInspector.PropertyVector3D(nameof(mSpline.StartPoint), mSpline.StartPoint);

			Single h = Mathf.Clamp(XEditorInspector.CONTROL_HEIGHT_SPACE * (mSpline.CountPoints - 1),
				XEditorInspector.CONTROL_HEIGHT_SPACE * 2, XEditorInspector.CONTROL_HEIGHT_SPACE * 8);

			mScrollPosition = GUILayout.BeginScrollView(mScrollPosition, GUILayout.Height(h + 4));
			{
				for (Int32 i = 1; i < mSpline.CountPoints - 1; i++)
				{
					GUILayout.Space(2.0f);
					if (is_remove)
					{
						EditorGUILayout.BeginHorizontal();
						{
							mSpline.ControlPoints[i] = XEditorInspector.PropertyVector3D("Point " + i.ToString(), mSpline.ControlPoints[i]);
							if (GUILayout.Button("X", EditorStyles.miniButtonRight))
							{
								mSpline.RemoveControlPoint(i, true);
								EditorGUILayout.EndHorizontal();
								break;

							}
						}
						EditorGUILayout.EndHorizontal();
					}
					else
					{
						mSpline.ControlPoints[i] = XEditorInspector.PropertyVector3D("Point " + i.ToString(), mSpline.ControlPoints[i]);
					}
				}
			}
			GUILayout.EndScrollView();

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
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}
	}
	
	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров отображения сплайна
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void DrawVisualSpline()
	{
		EditorGUI.BeginChangeCheck();
		{
			GUILayout.Space(4.0f);
			XEditorInspector.DrawGroup(mContentGroupDraw);

			GUILayout.Space(2.0f);
			mSpline.SizeControlHandle = XEditorInspector.PropertyFloatSlider("HandleSize", mSpline.SizeControlHandle, 10, 50);

			GUILayout.Space(2.0f);
			mSpline.ColorControlStart = XEditorInspector.PropertyColor("ControlStart", mSpline.ColorControlStart);

			GUILayout.Space(2.0f);
			mSpline.ColorControl = XEditorInspector.PropertyColor("Control", mSpline.ColorControl);

			GUILayout.Space(2.0f);
			mSpline.ColorControlEnd = XEditorInspector.PropertyColor("ControlEnd", mSpline.ColorControlEnd);

			GUILayout.Space(2.0f);
			mSpline.ColorSpline = XEditorInspector.PropertyColor("Spline", mSpline.ColorSpline);

			GUILayout.Space(2.0f);
			mSpline.ColorSplineAlt = XEditorInspector.PropertyColor("SplineAlt", mSpline.ColorSplineAlt);
		}
		if (EditorGUI.EndChangeCheck())
		{
			this.serializedObject.Save();
		}
	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров редактирования сплайна
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void DrawEditorSpline()
	{

	}

	//-----------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Рисование параметров редактирования контрольной точки
	/// </summary>
	//-----------------------------------------------------------------------------------------------------------------
	protected virtual void DrawEditorPoint()
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