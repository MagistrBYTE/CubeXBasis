//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineBezierCubic.cs
*		Компонент представляющий кубическую кривую Безье.
*		Реализация компонента представляющего собой кубическую кривую Безье
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		/// Компонент представляющий кубическую кривую Безье
		/// </summary>
		/// <remarks>
		/// Реализация компонента представляющего собой кубическую кривую Безье
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Math/Spline/BezierCubic Spline")]
		public class CubeXSplineBezierCubic : CubeXSplineBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Первая управляющая точка
			/// </summary>
			public Vector3 HandlePoint1
			{
				get { return mControlPoints[1]; }
				set { mControlPoints[1] = value; }
			}

			/// <summary>
			/// Вторая управляющая точка
			/// </summary>
			public Vector3 HandlePoint2
			{
				get { return mControlPoints[2]; }
				set { mControlPoints[2] = value; }
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
				// Рисуем опорные и управляющие точки
				OnDrawGizmosControlPoint(0);
				OnDrawGizmosHandlePoint(1);
				OnDrawGizmosHandlePoint(2);
				OnDrawGizmosControlPoint(3);

				// Рисуем соединение
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(GetControlPointWorld(0), GetControlPointWorld(1));
				Gizmos.DrawLine(GetControlPointWorld(2), GetControlPointWorld(3));

				// Кривая
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
				return CBezierCubic3D.CalculatePoint(time, 
					ref mControlPoints[0], 
					ref mControlPoints[1],
					ref mControlPoints[2],
					ref mControlPoints[3]);
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
				return CBezierCubic3D.CalculateFirstDerivative(time,
					ref mControlPoints[0],
					ref mControlPoints[1],
					ref mControlPoints[2],
					ref mControlPoints[3]);
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
						Vector3.forward * 200 + Vector3.right * 200,
						Vector3.right * 300
					};
				}

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
			#endregion

			#region ======================================= РАБОТА С КОНТРОЛЬНЫМИ ТОЧКАМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на кривой Безье представленной с помощью четырех контрольных точек
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
			/// "правой" конечной точки кривой</param>
			/// <returns>Позиция точки на кривой Безье</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 CalculatePointToWorld(Single time)
			{
				return transform.TransformPoint(CalculatePoint(time));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание кубической кривой Безье проходящий через заданные точки на равномерно заданном времени
			/// </summary>
			/// <param name="start">Начальная точка</param>
			/// <param name="point1">Первая точка</param>
			/// <param name="point2">Вторая точка</param>
			/// <param name="end">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public void CreateFromPassesPoint(Vector3 start, Vector3 point1, Vector3 point2, Vector3 end)
			{
				mControlPoints[0] = start;
				mControlPoints[1].x = (-5 * start.x + 18 * point1.x - 9 * point2.x + 2 * end.x) / 6;
				mControlPoints[1].y = (-5 * start.y + 18 * point1.y - 9 * point2.y + 2 * end.y) / 6;
				mControlPoints[2].x = (2 * start.x - 9 * point1.x + 18 * point2.x - 5 * end.x) / 6;
				mControlPoints[2].y = (2 * start.y - 9 * point1.y + 18 * point2.y - 5 * end.y) / 6;
				mControlPoints[3] = end;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на управляющую точку
			/// </summary>
			/// <param name="index">Позиция(индекс) контрольной точки</param>
			/// <returns>Статус управляющей точки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsHandlePoint(Int32 index)
			{
				return index == 1 || index == 2;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================