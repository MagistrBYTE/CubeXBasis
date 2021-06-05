//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineBezierPath.cs
*		Компонент представляющий путь из кубических кривых Безье.
*		Реализация компонента представляющего путь из кубических кривых Безье
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup MathSpline
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент представляющий путь из кубических кривых Безье
		/// </summary>
		/// <remarks>
		/// Реализация компонента представляющего путь из кубических кривых Безье
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Math/Spline/BezierPath Spline")]
		public class CubeXSplineBezierPath : CubeXSplineBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsClosed;
			[SerializeField]
			internal TBezierHandleMode[] mHandleModes;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус замкнутости сплайна
			/// </summary>
			public Boolean IsClosed
			{
				get { return mIsClosed; }
				set
				{
					if (mIsClosed != value)
					{
						mIsClosed = value;

						if (mIsClosed == true)
						{
							mHandleModes[mHandleModes.Length - 1] = mHandleModes[0];
							SetControlPoint(0, mControlPoints[0], true);
						}

						OnUpdateSpline();
					}
				}
			}

			/// <summary>
			/// Количество кривых в пути
			/// </summary>
			public Int32 CurveCount
			{
				get { return (mControlPoints.Length - 1) / 3; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				LineRenderer line_render = this.GetComponent<LineRenderer>();
				if (line_render != null)
				{
					line_render.positionCount = mDrawingPoints.Count;
					for (Int32 i = 0; i < mDrawingPoints.Count; i++)
					{
						line_render.SetPosition(i, this.transform.TransformPoint(mDrawingPoints[i]));
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование кривой Безье в сцене в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawGizmos()
			{
#if UNITY_EDITOR
				for (Int32 i = 0; i < mControlPoints.Length; i++)
				{
					if (IsHandlePoint(i))
					{
						OnDrawGizmosHandlePoint(i);
					}
					else
					{
						OnDrawGizmosControlPoint(i);
					}


					// Управляющий вектор к начальной точки
					if (i == 1)
					{
						Vector3 p1 = GetControlPointWorld(1);
						Vector3 p2 = GetControlPointWorld(0);
						Gizmos.color = Color.blue;
						Gizmos.DrawLine(p1, p2);
						continue;
					}

					// Управляющий вектор к конечной точки
					if (i == mControlPoints.Length - 2)
					{
						Vector3 p1 = GetControlPointWorld(mControlPoints.Length - 2);
						Vector3 p2 = GetControlPointWorld(mControlPoints.Length - 1);

						Gizmos.color = Color.blue;
						Gizmos.DrawLine(p1, p2);
						continue;
					}

					if (i > 1 && i < mControlPoints.Length - 2 && i % 3 == 0)
					{
						Vector3 pc = GetControlPointWorld(i);
						Vector3 p1 = GetControlPointWorld(i - 1);
						Vector3 p2 = GetControlPointWorld(i + 1);
						Gizmos.color = Color.blue;
						Gizmos.DrawLine(p1, pc);
						Gizmos.DrawLine(p2, pc);
					}
				}

				OnDrawGizmosSpline();
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисования управляющей точки в режиме редактора
			/// </summary>
			/// <param name="index">Индекс контрольной точки</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawGizmosHandlePoint(Int32 index)
			{
#if UNITY_EDITOR
				Vector3 point = GetControlPointWorld(index);
				Single size = mSizeControlHandle / 100 * UnityEditor.HandleUtility.GetHandleSize(point);
				Gizmos.color = Color.blue;
				Gizmos.DrawCube(point, new Vector3(size, size, size));
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXSpline3D =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Vector3 CalculatePoint(Single time)
			{
				Int32 index_curve;
				if (time >= 1f)
				{
					time = 1f;
					index_curve = mControlPoints.Length - 4;
				}
				else
				{
					time = Mathf.Clamp01(time) * CurveCount;
					index_curve = (Int32)time;
					time -= index_curve;
					index_curve *= 3;
				}

				Vector3 point = CBezierCubic3D.CalculatePoint(time, 
					ref mControlPoints[index_curve],
					ref mControlPoints[index_curve + 1],
					ref mControlPoints[index_curve + 2], 
					ref mControlPoints[index_curve + 3]);

				return point;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление первой производной точки на сплайне
			/// </summary>
			/// <remarks>
			/// Первая производная показывает скорость изменения функции в данной точки.
			/// Физическим смысл производной - скорость в данной точке 
			/// </remarks>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Скорость точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Vector3 CalculateFirstDerivative(Single time)
			{
				Int32 index_curve;
				if (time >= 1f)
				{
					time = 1f;
					index_curve = mControlPoints.Length - 4;
				}
				else
				{
					time = Mathf.Clamp01(time) * CurveCount;
					index_curve = (Int32)time;
					time -= index_curve;
					index_curve *= 3;
				}

				Vector3 velocity = CBezierCubic3D.CalculateFirstDerivative(time, 
					ref mControlPoints[index_curve],
					ref mControlPoints[index_curve + 1],
					ref mControlPoints[index_curve + 2], 
					ref mControlPoints[index_curve + 3]);

				return velocity;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Растеризация сплайна - вычисление точек отрезков для рисования сплайна
			/// </summary>
			/// <remarks>
			/// Качество(степень) растеризации зависит от свойства <see cref="SegmentsSpline"/>
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeDrawingPoints()
			{
				mDrawingPoints.Clear();

				for (Int32 i = 0; i < CurveCount; i++)
				{
					Vector3 prev = CalculateCurvePoint(i, 0);
					mDrawingPoints.Add(prev);
					for (Int32 ip = 1; ip < SegmentsSpline; ip++)
					{
						Single time = (Single)ip / SegmentsSpline;
						Vector3 point = CalculateCurvePoint(i, time);

						// Добавляем если длина больше 1,4
						if ((point - prev).sqrMagnitude > MinimalSqrLine)
						{
							 mDrawingPoints.Add(point);
							prev = point;
						}
					}
				}

				if (mIsClosed)
				{
					CheckCorrectStartPoint();
				}
				else
				{
					CheckCorrectEndPoint();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ IMove ==============================================
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnResetSpline()
			{
				base.OnResetSpline();

				mControlPoints = new Vector3[]
				{
					Vector3.zero,
					Vector3.forward * 100 + Vector3.right * 100,
					Vector3.forward * 100 + Vector3.right * 200,
					Vector3.right * 300
				};

				mHandleModes = new TBezierHandleMode[]
				{
					TBezierHandleMode.Free,
					TBezierHandleMode.Free
				};

				OnUpdateSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnInitSpline()
			{
				base.OnInitSpline();

				if (mControlPoints == null)
				{
					mControlPoints = new Vector3[]
					{
						Vector3.zero,
						Vector3.forward * 100 + Vector3.right * 100,
						Vector3.forward * 100 + Vector3.right * 200,
						Vector3.right * 300
					};
				}

				mHandleModes = new TBezierHandleMode[]
				{
					TBezierHandleMode.Free,
					TBezierHandleMode.Free
				};

				OnUpdateSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void OnUpdateSpline()
			{
				ComputeDrawingPoints();
				ComputeLengthSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка режима редактирования управляющей точки
			/// </summary>
			/// <param name="index">Позиция(индекс) контрольной точки</param>
			//---------------------------------------------------------------------------------------------------------
			private void SetHandleMode(Int32 index)
			{
				Int32 mode_index = (index + 1) / 3;
				TBezierHandleMode mode = mHandleModes[mode_index];
				if (mode == TBezierHandleMode.Free || !mIsClosed && (mode_index == 0 || mode_index == mHandleModes.Length - 1))
				{
					return;
				}

				Int32 middle_index = mode_index * 3;
				Int32 fixed_index, enforced_index;
				if (index <= middle_index)
				{
					fixed_index = middle_index - 1;
					if (fixed_index < 0)
					{
						fixed_index = mControlPoints.Length - 2;
					}
					enforced_index = middle_index + 1;
					if (enforced_index >= mControlPoints.Length)
					{
						enforced_index = 1;
					}
				}
				else
				{
					fixed_index = middle_index + 1;
					if (fixed_index >= mControlPoints.Length)
					{
						fixed_index = 1;
					}
					enforced_index = middle_index - 1;
					if (enforced_index < 0)
					{
						enforced_index = mControlPoints.Length - 2;
					}
				}

				Vector3 middle = mControlPoints[middle_index];
				Vector3 enforced_tangent = middle - mControlPoints[fixed_index];
				if (mode == TBezierHandleMode.Aligned)
				{
					enforced_tangent = enforced_tangent.normalized * Vector3.Distance(middle, mControlPoints[enforced_index]);
				}

				mControlPoints[enforced_index] = middle + enforced_tangent;

				OnUpdateSpline();
			}
			#endregion

			#region ======================================= РАБОТА С КОНТРОЛЬНЫМИ ТОЧКАМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 CalculatePointWorld(Single time)
			{
				return transform.TransformPoint(CalculatePoint(time));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка контрольной точки сплайна по индексу в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetControlPoint(Int32 index, Vector3 point, Boolean update_spline = false)
			{
				if (index % 3 == 0)
				{
					Vector3 delta = point - mControlPoints[index];
					if (mIsClosed)
					{
						if (index == 0)
						{
							mControlPoints[1] += delta;
							mControlPoints[mControlPoints.Length - 2] += delta;
							mControlPoints[mControlPoints.Length - 1] = point;
						}
						else if (index == mControlPoints.Length - 1)
						{
							mControlPoints[0] = point;
							mControlPoints[1] += delta;
							mControlPoints[index - 1] += delta;
						}
						else
						{
							mControlPoints[index - 1] += delta;
							mControlPoints[index + 1] += delta;
						}
					}
					else
					{
						if (index > 0)
						{
							mControlPoints[index - 1] += delta;
						}
						if (index + 1 < mControlPoints.Length)
						{
							mControlPoints[index + 1] += delta;
						}
					}
				}
				mControlPoints[index] = point;
				SetHandleMode(index);

				if (update_spline)
				{
					OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка контрольной точки сплайна по индексу в мировых координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <param name="point">Контрольная точка сплайна в мировых координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetControlPointWorld(Int32 index, Vector3 point, Boolean update_spline = false)
			{
				point = transform.InverseTransformPoint(point);
				SetControlPoint(index, point, update_spline);
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ОТДЕЛЬНЫМИ КРИВЫМИ ========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить кривую
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void AddCurve()
			{
				Vector3 point = mControlPoints[mControlPoints.Length - 1];
				Array.Resize(ref mControlPoints, mControlPoints.Length + 3);
				point.x += 100f;
				mControlPoints[mControlPoints.Length - 3] = point;
				point.x += 100f;
				mControlPoints[mControlPoints.Length - 2] = point;
				point.x += 100f;
				mControlPoints[mControlPoints.Length - 1] = point;

				Array.Resize(ref mHandleModes, mHandleModes.Length + 1);
				mHandleModes[mHandleModes.Length - 1] = mHandleModes[mHandleModes.Length - 2];
				SetHandleMode(mControlPoints.Length - 4);

				if (mIsClosed)
				{
					mControlPoints[mControlPoints.Length - 1] = mControlPoints[0];
					mHandleModes[mHandleModes.Length - 1] = mHandleModes[0];
					SetHandleMode(0);
				}

				OnUpdateSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление последней кривой
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void RemoveCurve()
			{
				if (CurveCount > 1)
				{
					Array.Resize(ref mControlPoints, mControlPoints.Length - 3);
					Array.Resize(ref mHandleModes, mHandleModes.Length - 1);
					SetHandleMode(mControlPoints.Length - 2, TBezierHandleMode.Free);
					if (mIsClosed)
					{
						mControlPoints[mControlPoints.Length - 1] = mControlPoints[0];
						mHandleModes[mHandleModes.Length - 1] = mHandleModes[0];
						SetHandleMode(0);
					}

					OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на отдельной кривой Безье в локальных координатах
			/// </summary>
			/// <param name="curve_index">Индекс кривой</param>
			/// <param name="time">Время</param>
			/// <returns>Точка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 CalculateCurvePoint(Int32 curve_index, Single time)
			{
				Int32 node_index = curve_index * 3;

				Vector3 p0 = mControlPoints[node_index];
				Vector3 p1 = mControlPoints[node_index + 1];
				Vector3 p2 = mControlPoints[node_index + 2];
				Vector3 p3 = mControlPoints[node_index + 3];

				return CBezierCubic3D.CalculatePoint(time, ref p0, ref p1, ref p2, ref p3);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на отдельной кривой Безье в мировых координатах
			/// </summary>
			/// <param name="curve_index">Индекс кривой</param>
			/// <param name="time">Время</param>
			/// <returns>Точка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 CalculateCurvePointWorld(Int32 curve_index, Single time)
			{
				Int32 node_index = curve_index * 3;

				Vector3 p0 = mControlPoints[node_index];
				Vector3 p1 = mControlPoints[node_index + 1];
				Vector3 p2 = mControlPoints[node_index + 2];
				Vector3 p3 = mControlPoints[node_index + 3];

				return transform.TransformPoint(CBezierCubic3D.CalculatePoint(time, ref p0, ref p1, ref p2, ref p3));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение контрольной точки на отдельной кривой Безье в локальных координатах
			/// </summary>
			/// <param name="curve_index">Индекс кривой</param>
			/// <param name="point_index">Индекс контрольной точки</param>
			/// <returns>Контрольная точка</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 GetCurveControlPoint(Int32 curve_index, Int32 point_index)
			{
				curve_index = curve_index * 3;
				return mControlPoints[curve_index + point_index];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка позиции контрольной точки на отдельной кривой Безье в локальных координатах
			/// </summary>
			/// <param name="curve_index">Индекс кривой</param>
			/// <param name="point_index">Индекс точки</param>
			/// <param name="position">Позиция контрольной точки</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetCurveControlPoint(Int32 curve_index, Int32 point_index, Vector3 position)
			{
				curve_index = curve_index * 3;
				mControlPoints[curve_index + point_index] = position;
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С УПРАВЛЯЮЩИМИ ТОЧКАМИ ======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на управляющую точку
			/// </summary>
			/// <param name="index">Позиция(индекс) контрольной точки</param>
			/// <returns>Статус управляющей точки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsHandlePoint(Int32 index)
			{
				return index == 1 ||
				       index == 2 ||
				       index % 3 == 1 ||
				       index % 3 == 2;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получения режима редактирования управляющей точки
			/// </summary>
			/// <param name="index">Позиция(индекс) контрольной точки</param>
			/// <returns>Режим редактирования управляющей точки</returns>
			//---------------------------------------------------------------------------------------------------------
			public TBezierHandleMode GetHandleMode(Int32 index)
			{
				return mHandleModes[(index + 1) / 3];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка режима редактирования управляющей точки
			/// </summary>
			/// <param name="index">Позиция(индекс) контрольной точки</param>
			/// <param name="mode">Режим редактирования управляющей точки</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetHandleMode(Int32 index, TBezierHandleMode mode)
			{
				Int32 mode_index = (index + 1) / 3;
				mHandleModes[mode_index] = mode;

				if (mIsClosed)
				{
					if (mode_index == 0)
					{
						mHandleModes[mHandleModes.Length - 1] = mode;
					}
					else if (mode_index == mHandleModes.Length - 1)
					{
						mHandleModes[0] = mode;
					}
				}

				SetHandleMode(index);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================