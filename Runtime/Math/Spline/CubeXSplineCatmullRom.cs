//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineCatmullRom.cs
*		Компонент представляющий сплайн CatmullRom.
*		Реализация компонента представляющего собой сплайн CatmullRom
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
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
		/// Компонент представляющий сплайн CatmullRom
		/// </summary>
		/// <remarks>
		/// Реализация компонента представляющего собой сплайн CatmullRom
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Math/Spline/CatmullRom Spline")]
		public class CubeXSplineCatmullRom : CubeXSplineBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Boolean mIsClosed;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус замкнутости сплайна
			/// </summary>
			public Boolean IsClosed
			{
				get { return mIsClosed; }
				set
				{
					if(mIsClosed != value)
					{
						mIsClosed = value;
						OnUpdateSpline();
					}
				}
			}

			/// <summary>
			/// Количество кривых в пути
			/// </summary>
			public Int32 CurveCount
			{
				get { return mControlPoints.Length - 1; }
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
			/// Рисование сплайна CatmullRom в сцене в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawGizmos()
			{
#if UNITY_EDITOR
				// Рисуем опорные точки
				for (Int32 i = 0; i < mControlPoints.Length; i++)
				{
					OnDrawGizmosControlPoint(i);
				}

				// Кривая
				OnDrawGizmosSpline();
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
					index_curve = mControlPoints.Length - 1;
				}
				else
				{
					time = Mathf.Clamp01(time) * CurveCount;
					index_curve = (Int32)time;
					time -= index_curve;
					index_curve *= 1;
				}

				if(mIsClosed)
				{
					// Получаем индексы точек
					Int32 ip_0 = ClampPosition(index_curve - 1);
					Int32 ip_1 = ClampPosition(index_curve);
					Int32 ip_2 = ClampPosition(index_curve + 1);
					Int32 ip_3 = ClampPosition(index_curve + 2);

					Vector3 point = CCatmullRomSpline3D.CalculatePoint(time, 
						ref mControlPoints[ip_0],
						ref mControlPoints[ip_1],
						ref mControlPoints[ip_2], 
						ref mControlPoints[ip_3]);

					return point;
				}
				else
				{
					// Получаем индексы точек
					Int32 ip_0 = index_curve - 1 < 0 ? 0 : index_curve - 1;
					Int32 ip_1 = index_curve;
					Int32 ip_2 = index_curve + 1 > mControlPoints.Length - 1 ? mControlPoints.Length - 1 : index_curve + 1;
					Int32 ip_3 = index_curve + 2 > mControlPoints.Length - 1 ? mControlPoints.Length - 1 : index_curve + 2;

					Vector3 point = CCatmullRomSpline3D.CalculatePoint(time, 
						ref mControlPoints[ip_0],
						ref mControlPoints[ip_1],
						ref mControlPoints[ip_2], 
						ref mControlPoints[ip_3]);

					return point;
				}
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
					index_curve = mControlPoints.Length - 1;
				}
				else
				{
					time = Mathf.Clamp01(time) * CurveCount;
					index_curve = (Int32)time;
					time -= index_curve;
					index_curve *= 1;
				}

				if (mIsClosed)
				{
					// Получаем индексы точек
					Int32 ip_0 = ClampPosition(index_curve - 1);
					Int32 ip_1 = ClampPosition(index_curve);
					Int32 ip_2 = ClampPosition(index_curve + 1);
					Int32 ip_3 = ClampPosition(index_curve + 2);

					Vector3 velocity = CCatmullRomSpline3D.CalculateFirstDerivative(time, 
						ref mControlPoints[ip_0],
						ref mControlPoints[ip_1], 
						ref mControlPoints[ip_2], 
						ref mControlPoints[ip_3]);

					return velocity;
				}
				else
				{
					// Получаем индексы точек
					Int32 ip_0 = index_curve - 1 < 0 ? 0 : index_curve - 1;
					Int32 ip_1 = index_curve;
					Int32 ip_2 = index_curve + 1 > mControlPoints.Length - 1 ? mControlPoints.Length - 1 : index_curve + 1;
					Int32 ip_3 = index_curve + 2 > mControlPoints.Length - 1 ? mControlPoints.Length - 1 : index_curve + 2;

					Vector3 velocity = CCatmullRomSpline3D.CalculateFirstDerivative(time,
						ref mControlPoints[ip_0],
						ref mControlPoints[ip_1], 
						ref mControlPoints[ip_2], 
						ref mControlPoints[ip_3]);

					return velocity;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Растеризация сплайна - вычисление точек отрезков для рисования сплайна
			/// </summary>
			/// <remarks>
			/// Качество(степень) растеризации зависит от свойства <see cref="SegmentsCurve"/>
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void ComputeDrawingPoints()
			{
				mDrawingPoints.Clear();

				if (mIsClosed)
				{
					for (Int32 ip = 0; ip < mControlPoints.Length; ip++)
					{
						// Получаем индексы точек
						Int32 ip_0 = ClampPosition(ip - 1);
						Int32 ip_1 = ClampPosition(ip);
						Int32 ip_2 = ClampPosition(ip + 1);
						Int32 ip_3 = ClampPosition(ip + 2);

						Vector3 prev = CalculatePoint(0, ip_0, ip_1, ip_2, ip_3);
						mDrawingPoints.Add(prev);
						for (Int32 i = 1; i < mSegmentsSpline; i++)
						{
							Single time = (Single)i / mSegmentsSpline;
							Vector3 point = CalculatePoint(time, ip_0, ip_1, ip_2, ip_3);

							// Добавляем если длина больше 1,4
							if ((point - prev).sqrMagnitude > 2)
							{
								mDrawingPoints.Add(point);
								prev = point;
							}
						}
					}

					CheckCorrectStartPoint();
				}
				else
				{
					for (Int32 ip = 0; ip < mControlPoints.Length - 1; ip++)
					{
						// Получаем индексы точек
						Int32 ip_0 = ip - 1 < 0 ? 0 : ip - 1;
						Int32 ip_1 = ip;
						Int32 ip_2 = ip + 1 > mControlPoints.Length - 1 ? mControlPoints.Length - 1 : ip + 1;
						Int32 ip_3 = ip + 2 > mControlPoints.Length - 1 ? mControlPoints.Length - 1 : ip + 2;

						Vector3 prev = CalculatePoint(0, ip_0, ip_1, ip_2, ip_3);
						mDrawingPoints.Add(prev);
						for (Int32 i = 1; i < mSegmentsSpline; i++)
						{
							Single time = (Single)i / mSegmentsSpline;
							Vector3 point = CalculatePoint(time, ip_0, ip_1, ip_2, ip_3);

							// Добавляем если длина больше 1,4
							if ((point - prev).sqrMagnitude > 2)
							{
								mDrawingPoints.Add(point);
								prev = point;
							}
						}
					}

					CheckCorrectEndPoint();
				}
			}
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

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение точек для организации замкнутой кривой
			/// </summary>
			/// <param name="pos">Позиция</param>
			/// <returns>Скорректированная позиция</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Int32 ClampPosition(Int32 pos)
			{
				if (pos < 0)
				{
					pos = mControlPoints.Length - 1;
				}

				if (pos > mControlPoints.Length)
				{
					pos = 1;
				}
				else if (pos > mControlPoints.Length - 1)
				{
					pos = 0;
				}

				return pos;
			}
			#endregion

			#region ======================================= РАБОТА С КОНТРОЛЬНЫМИ ТОЧКАМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне CatmullRom представленной с помощью четырех контрольных точек
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1, где 0 соответствует крайней "левой" точки, 1 соответствует крайне
			/// "правой" конечной точки кривой</param>
			/// <param name="index_p0">Индекс первой точки</param>
			/// <param name="index_p1">Индекс второй точки</param>
			/// <param name="index_p2">Индекс третьей точки</param>
			/// <param name="index_p3">Индекс четвертой точки</param>
			/// <returns>Позиция точки на сплайне CatmullRom</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 CalculatePoint(Single time, Int32 index_p0, Int32 index_p1, Int32 index_p2, Int32 index_p3)
			{
				Vector3 a = 2f * mControlPoints[index_p1];
				Vector3 b = mControlPoints[index_p2] - mControlPoints[index_p0];
				Vector3 c = 2f * mControlPoints[index_p0] - 5f * mControlPoints[index_p1] + 4f * mControlPoints[index_p2] - mControlPoints[index_p3];
				Vector3 d = -mControlPoints[index_p0] + 3f * mControlPoints[index_p1] - 3f * mControlPoints[index_p2] + mControlPoints[index_p3];

				//The cubic polynomial: a + b * t + c * t^2 + d * t^3
				Vector3 pos = 0.5f * (a + b * time + c * time * time + d * time * time * time);

				return pos;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================