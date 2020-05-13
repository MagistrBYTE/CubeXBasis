//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXMathSplineCommon.cs
*		Алгоритмы представления и визуализации различных сплайнов.
*		Реализация алгоритмов которые представляют различные виды сплайнов, и позволяют производить базовые действия со
*	сплайнами, их аппроксимацию для отображения, перемещение по сплайну.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
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
		//! \defgroup MathSpline Подсистема для работы со сплайнами
		//! Подсистема представления и визуализации различных сплайнов. Реализация алгоритмов которые представляют различные
		//! виды сплайнов и позволяют производить базовые действия со сплайнами, их аппроксимацию для отображения, 
		//! перемещение по сплайну.
		//! \ingroup Math
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Структура для хранения сегмента пути
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public struct TMoveSegment
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Длина сегмента
			/// </summary>
			public Single Length;

			/// <summary>
			/// Совокупная длина с учетом данного сегмента
			/// </summary>
			public Single Summa;
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="length">Длина сегмента</param>
			/// <param name="summa">Совокупная длина с учетом данного сегмента</param>
			//---------------------------------------------------------------------------------------------------------
			public TMoveSegment(Single length, Single summa)
			{
				Length = length;
				Summa = summa;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для представления сплайна в двухмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXSpline2D
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Количество контрольных точек сплайна
			/// </summary>
			Int32 CountPoints { get; }

			/// <summary>
			/// Длина сплайна
			/// </summary>
			Single Length { get; }

			/// <summary>
			/// Контрольные точки сплайна
			/// </summary>
			IList<Vector2> ControlPoints { get; }

			/// <summary>
			/// Список точек сплайна для рисования
			/// </summary>
			IList<Vector2> DrawingPoints { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			Vector2 CalculatePoint(Single time);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Растеризация сплайна - вычисление точек отрезков для рисования сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ComputeDrawingPoints();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление длины сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ComputeLengthSpline();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление выпуклого четырехугольника, заданного опорными точками внутри которого находится сплайн
			/// </summary>
			/// <returns>Прямоугольник</returns>
			//---------------------------------------------------------------------------------------------------------
			Rect GetArea();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для представления сплайна в трехмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXSpline3D
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Количество контрольных точек сплайна
			/// </summary>
			Int32 CountPoints { get; }

			/// <summary>
			/// Длина сплайна
			/// </summary>
			Single Length { get; }

			/// <summary>
			/// Контрольные точки сплайна
			/// </summary>
			IList<Vector3> ControlPoints { get; }

			/// <summary>
			/// Список точек сплайна для рисования
			/// </summary>
			IList<Vector3> DrawingPoints { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			Vector3 CalculatePoint(Single time);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Растеризация сплайна - вычисление точек отрезков для рисования сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ComputeDrawingPoints();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление длины сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void ComputeLengthSpline();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для представления сплайна в двухмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public abstract class CSplineBase2D : ICubeXSpline2D
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Квадрат минимальной длины отрезка для отображения сплайна
			/// </summary>
			/// <remarks>
			/// Если в результате растеризации получится отрезок меньшей длины(квадрата длины) он
			/// не будет принимается в расчет и отображаться
			/// </remarks>
			public static Int32 MinimalSqrLine = 2;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Vector2[] mControlPoints;
			[SerializeField]
			internal Int32 mSegmentsSpline = 10;
			[SerializeField]
			internal List<Vector2> mDrawingPoints;
			[NonSerialized]
			internal Single mLength = 0;
			[SerializeField]
			internal List<TMoveSegment> mSegmentsPath;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Начальная точка
			/// </summary>
			public Vector2 StartPoint
			{
				get { return mControlPoints[0]; }
				set { mControlPoints[0] = value; }
			}

			/// <summary>
			/// Конечная точка
			/// </summary>
			public Vector2 EndPoint
			{
				get { return mControlPoints[mControlPoints.Length - 1]; }
				set { mControlPoints[mControlPoints.Length - 1] = value; }
			}

			/// <summary>
			/// Количество сегментов сплайна
			/// </summary>
			/// <remarks>
			/// Данный параметр используется при растеризации сплайна. Чем больше значение тем выше качество отображения сплайна
			/// </remarks>
			public Int32 SegmentsSpline
			{
				get { return mSegmentsSpline; }
				set
				{
					if (mSegmentsSpline != value)
					{
						mSegmentsSpline = value;
						OnUpdateSpline();
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXSpline2D ===================================
			/// <summary>
			/// Количество контрольных точек сплайна
			/// </summary>
			public Int32 CountPoints
			{
				get { return mControlPoints.Length; }
			}

			/// <summary>
			/// Длина сплайна
			/// </summary>
			public Single Length
			{
				get { return mLength; }
			}

			/// <summary>
			/// Контрольные точки сплайна
			/// </summary>
			public IList<Vector2> ControlPoints
			{
				get { return mControlPoints; }
			}

			/// <summary>
			/// Список точек сплайна для рисования
			/// </summary>
			public IList<Vector2> DrawingPoints
			{
				get { return mDrawingPoints; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			/// <param name="count">Количество контрольных точек</param>
			//---------------------------------------------------------------------------------------------------------
			public CSplineBase2D(Int32 count)
			{
				mControlPoints = new Vector2[count];
				mDrawingPoints = new List<Vector2>();
				mSegmentsPath = new List<TMoveSegment>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start_point">Начальная точка</param>
			/// <param name="end_point">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public CSplineBase2D(Vector2 start_point, Vector2 end_point)
			{
				mControlPoints = new Vector2[3];
				mControlPoints[0] = start_point;
				mControlPoints[1] = (start_point + end_point) / 2;
				mControlPoints[2] = end_point;
				mDrawingPoints = new List<Vector2>();
				mSegmentsPath = new List<TMoveSegment>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="control_points">Набор контрольных точек сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public CSplineBase2D(params Vector2[] control_points)
			{
				mControlPoints = new Vector2[control_points.Length];
				Array.Copy(control_points, mControlPoints, control_points.Length);
				mDrawingPoints = new List<Vector2>();
				mSegmentsPath = new List<TMoveSegment>();
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXSpline2D =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <remarks>
			/// Это основной метод для реализации
			/// </remarks>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public abstract Vector2 CalculatePoint(Single time);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Растеризация сплайна - вычисление точек отрезков для рисования сплайна
			/// </summary>
			/// <remarks>
			/// Качество(степень) растеризации зависит от свойства <see cref="SegmentsSpline"/>
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeDrawingPoints()
			{
				mDrawingPoints.Clear();
				Vector2 prev = mControlPoints[0];
				mDrawingPoints.Add(prev);
				for (Int32 i = 1; i < mSegmentsSpline; i++)
				{
					Single time = (Single)i / mSegmentsSpline;
					Vector2 point = CalculatePoint(time);
					
					// Добавляем если длина больше
					if ((point - prev).sqrMagnitude > MinimalSqrLine)
					{
						mDrawingPoints.Add(point);
						prev = point;
					}
				}

				mDrawingPoints.Add(EndPoint);

				// Проверяем еще раз
				if ((mDrawingPoints[mDrawingPoints.Count - 1] - mDrawingPoints[mDrawingPoints.Count - 2]).sqrMagnitude < MinimalSqrLine)
				{
					mDrawingPoints.RemoveAt(mDrawingPoints.Count - 1);
					mDrawingPoints[mDrawingPoints.Count - 1] = EndPoint;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление длины сплайна
			/// </summary>
			/// <remarks>
			/// Реализация по умолчанию вычисляет длину сплайна по сумме отрезков растеризации
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeLengthSpline()
			{
				mLength = 0;
				mSegmentsPath.Clear();
				for (Int32 i = 1; i < mDrawingPoints.Count; i++)
				{
					Single length = (mDrawingPoints[i] - mDrawingPoints[i - 1]).magnitude;
					mLength += length;
					TMoveSegment segment = new TMoveSegment(length, mLength);
					mSegmentsPath.Add(segment);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление выпуклого четырехугольника, заданного опорными точками внутри которого находится сплайн
			/// </summary>
			/// <remarks>
			/// Реализация по умолчанию вычисляет прямоугольник на основе контрольных точек
			/// </remarks>
			/// <returns>Прямоугольник</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Rect GetArea()
			{
				Single x_min = StartPoint.x, x_max = EndPoint.x, y_min = StartPoint.y, y_max = EndPoint.y;

				for (Int32 i = 0; i < mDrawingPoints.Count; i++)
				{
					Vector2 point = mDrawingPoints[i];
					if (point.x < x_min) x_min = point.x;
					if (point.x > x_max) x_max = point.x;
					if (point.y < y_min) y_min = point.y;
					if (point.y > y_max) y_max = point.y;
				}

				Rect rect_area = new Rect();
				rect_area.x = x_min;
				rect_area.width = x_max - x_min;
				rect_area.y = y_min;
				rect_area.height = y_max - y_min;

				return rect_area;
			}
			#endregion

			#region ======================================= МЕТОДЫ IMove ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции на сплайне
			/// </summary>
			/// <param name="position">Положение точки на сплайне от 0 до длины сплайна</param>
			/// <returns>Позиция на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2 GetMovePosition(Single position)
			{
				// Ограничения
				if (position >= Length)
				{
					return EndPoint;
				}
				if (position <= 0.0f)
				{
					return StartPoint;
				}

				// Находим в какой отрезок попадает эта позиция
				Int32 index = 0;
				for (Int32 i = 0; i < mSegmentsPath.Count; i++)
				{
					// Если позиция меньше значит она попала в отрезок
					if (position < mSegmentsPath[i].Summa)
					{
						index = i;
						break;
					}
				}

				// Перемещение относительно данного сегмента
				Single delta = 0;

				if (index == 0)
				{
					delta = position / mSegmentsPath[index].Length;
				}
				else
				{
					delta = (position - mSegmentsPath[index - 1].Summa) / mSegmentsPath[index].Length;
				}

				// Интерполируем позицию
				return Vector2.Lerp(mDrawingPoints[index], mDrawingPoints[index + 1], delta);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение скорости на сплайне
			/// </summary>
			/// <remarks>
			/// Это коэффициент изменения скорости на сплайне применяется для регулировки скорости
			/// </remarks>
			/// <param name="position">Положение точки на сплайне от 0 до длины сплайна</param>
			/// <returns>Скорость на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMoveVelocity(Single position)
			{
				return 1.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение направления на сплайне
			/// </summary>
			/// <param name="position">Положение точки на сплайне от 0 до длины сплайна</param>
			/// <returns>Направление на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector2 GetMoveDirection(Single position)
			{
				return GetMovePosition(position + 0.01f);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение положения на сплайне
			/// </summary>
			/// <param name="position">Позиция на сплайне</param>
			/// <param name="epsilon">Погрешность при сравнении</param>
			/// <returns>Положение точки от 0 до 1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMoveTime(Vector2 position, Single epsilon = 0.01f)
			{
				// Просматриваем все отрезки и находим нужное положение
				for (Int32 i = 1; i < mDrawingPoints.Count; i++)
				{
					Vector2 p1 = mDrawingPoints[i] - mDrawingPoints[i - 1];
					Vector2 p2 = position - mDrawingPoints[i - 1];

					Single angle = Vector2.Dot(p2, p1) / Vector2.Dot(p2, p1);
					if (angle + epsilon > 0 && angle - epsilon < 1)
					{
						// TODO: Найти точное положение на всей кривой
						return angle;
					}
				}

				return -1;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnResetSpline()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnInitSpline()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление параметров сплайна
			/// </summary>
			/// <remarks>
			/// Реализация по умолчанию обновляет список точек для рисования и длину сплайна
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnUpdateSpline()
			{
				ComputeDrawingPoints();
				ComputeLengthSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование сплайна с помощью GL
			/// </summary>
			/// <param name="material">Материал</param>
			/// <param name="color">Цвет сплайна</param>
			/// <param name="alternative">Альтернативный цвет сплайна на нечетных сегментах</param>
			/// <param name="is_alternative">Статус использования альтернативного цвета</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnDrawSplineGL(Material material, Color color, Color alternative, Boolean is_alternative = true)
			{
				GL.PushMatrix();
				GL.LoadPixelMatrix();
				material.SetPass(0);
				if (is_alternative == false)
				{
					GL.Color(color);
				}
				GL.Begin(GL.LINES);
				{
					for (Int32 i = 1; i < mDrawingPoints.Count; i++)
					{
						Vector2 p1 = new Vector2(mDrawingPoints[i - 1].x, Screen.height - mDrawingPoints[i - 1].y);
						Vector2 p2 = new Vector2(mDrawingPoints[i].x, Screen.height - mDrawingPoints[i].y);

						if (is_alternative)
						{
							if (i % 2 == 0)
							{
								GL.Color(color);
								GL.Vertex(p1);
								GL.Vertex(p2);
							}
							else
							{
								GL.Color(alternative);
								GL.Vertex(p1);
								GL.Vertex(p2);
							}
						}
					}
				}
				GL.End();
				GL.PopMatrix();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка и корректировка расположения первой точки на сплайне.
			/// Применяется когда надо скорректировать отображения сплайна при его замкнутости
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void CheckCorrectStartPoint()
			{
				mDrawingPoints[mDrawingPoints.Count - 1] = StartPoint;
				if ((mDrawingPoints[mDrawingPoints.Count - 1] - mDrawingPoints[mDrawingPoints.Count - 2]).sqrMagnitude < MinimalSqrLine)
				{
					mDrawingPoints.RemoveAt(mDrawingPoints.Count - 1);
					mDrawingPoints[mDrawingPoints.Count - 1] = StartPoint;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка и корректировка расположения последней точки на сплайне
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void CheckCorrectEndPoint()
			{
				mDrawingPoints[mDrawingPoints.Count - 1] = EndPoint;
				if ((mDrawingPoints[mDrawingPoints.Count - 1] - mDrawingPoints[mDrawingPoints.Count - 2]).sqrMagnitude < MinimalSqrLine)
				{
					mDrawingPoints.RemoveAt(mDrawingPoints.Count - 1);
					mDrawingPoints[mDrawingPoints.Count - 1] = EndPoint;
				}
			}
			#endregion

			#region ======================================= РАБОТА С КОНТРОЛЬНЫМИ ТОЧКАМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение контрольной точки сплайна по индексу в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <returns>Контрольная точка сплайна в локальных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Vector2 GetControlPoint(Int32 index)
			{
				return mControlPoints[index];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка контрольной точки сплайна по индексу в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetControlPoint(Int32 index, Vector2 point, Boolean update_spline = false)
			{
				mControlPoints[index] = point;
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление контрольной точки к сплайну в локальных координатах
			/// </summary>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddControlPoint(Vector2 point, Boolean update_spline = false)
			{
				Array.Resize(ref mControlPoints, mControlPoints.Length + 1);
				mControlPoints[mControlPoints.Length - 1] = point;
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка контрольной точки в сплайн в локальных координатах в указанной позиции
			/// </summary>
			/// <param name="index">Позиция(индекс) вставки точки</param>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void InsertControlPoint(Int32 index, Vector2 point, Boolean update_spline = false)
			{
				mControlPoints = XArray.InsertAt(mControlPoints, point, index);
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление контрольной точки в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) удаляемой точки</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RemoveControlPoint(Int32 index, Boolean update_spline = false)
			{
				mControlPoints = XArray.RemoveAt(mControlPoints, index);
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление контрольной точки в локальных координатах
			/// </summary>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RemoveControlPoint(Vector2 point, Boolean update_spline = false)
			{
				for (Int32 i = 0; i < mControlPoints.Length; i++)
				{
					if (mControlPoints[i].Approximately(point))
					{
						mControlPoints = XArray.RemoveAt(mControlPoints, i);
						if (update_spline)
						{
							this.OnUpdateSpline();
						}
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех контрольных точек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ClearControlPoints()
			{
				Array.Clear(mControlPoints, 0, mControlPoints.Length);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для представления сплайна в трехмерном пространстве
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public abstract class CSplineBase3D : ICubeXSpline3D
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Квадрат минимальной длины отрезка для отображения сплайна
			/// </summary>
			/// <remarks>
			/// Если в результате растеризации получится отрезок меньшей длины(квадрата длины) он
			/// не будет принимается в расчет и отображаться
			/// </remarks>
			public static Int32 MinimalSqrLine = 2;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Vector3[] mControlPoints;
			[SerializeField]
			internal Int32 mSegmentsSpline;
			[SerializeField]
			internal List<Vector3> mDrawingPoints;
			[NonSerialized]
			internal Single mLength = 0;
			[SerializeField]
			internal List<TMoveSegment> mSegmentsPath;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Начальная точка
			/// </summary>
			public Vector3 StartPoint
			{
				get { return mControlPoints[0]; }
				set { mControlPoints[0] = value; }
			}

			/// <summary>
			/// Конечная точка
			/// </summary>
			public Vector3 EndPoint
			{
				get { return mControlPoints[mControlPoints.Length - 1]; }
				set { mControlPoints[mControlPoints.Length - 1] = value; }
			}

			/// <summary>
			/// Количество сегментов сплайна
			/// </summary>
			/// <remarks>
			/// Данный параметр используется при растеризации сплайна. Чем больше значение тем выше качество отображения сплайна
			/// </remarks>
			public Int32 SegmentsSpline
			{
				get { return mSegmentsSpline; }
				set
				{
					if (mSegmentsSpline != value)
					{
						mSegmentsSpline = value;
						OnUpdateSpline();
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXSpline3D ===================================
			/// <summary>
			/// Количество контрольных точек сплайна
			/// </summary>
			public Int32 CountPoints
			{
				get { return mControlPoints.Length; }
			}

			/// <summary>
			/// Длина сплайна
			/// </summary>
			public Single Length
			{
				get { return mLength; }
			}

			/// <summary>
			/// Контрольные точки сплайна
			/// </summary>
			public IList<Vector3> ControlPoints
			{
				get { return mControlPoints; }
			}

			/// <summary>
			/// Список точек сплайна для рисования
			/// </summary>
			public IList<Vector3> DrawingPoints
			{
				get { return mDrawingPoints; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			/// <param name="count">Количество контрольных точек</param>
			//---------------------------------------------------------------------------------------------------------
			public CSplineBase3D(Int32 count)
			{
				mControlPoints = new Vector3[count];
				mDrawingPoints = new List<Vector3>();
				mSegmentsPath = new List<TMoveSegment>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="start_point">Начальная точка</param>
			/// <param name="end_point">Конечная точка</param>
			//---------------------------------------------------------------------------------------------------------
			public CSplineBase3D(Vector3 start_point, Vector3 end_point)
			{
				mControlPoints = new Vector3[3];
				mControlPoints[0] = start_point;
				mControlPoints[1] = (start_point + end_point) / 2;
				mControlPoints[2] = end_point;
				mDrawingPoints = new List<Vector3>();
				mSegmentsPath = new List<TMoveSegment>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="control_points">Набор контрольных точек сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public CSplineBase3D(params Vector3[] control_points)
			{
				mControlPoints = new Vector3[control_points.Length];
				Array.Copy(control_points, mControlPoints, control_points.Length);
				mDrawingPoints = new List<Vector3>();
				mSegmentsPath = new List<TMoveSegment>();
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXSpline3D =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление точки на сплайне
			/// </summary>
			/// <remarks>
			/// Это основной метод для реализации
			/// </remarks>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Позиция точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public abstract Vector3 CalculatePoint(Single time);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Растеризация сплайна - вычисление точек отрезков для рисования сплайна
			/// </summary>
			/// <remarks>
			/// Качество(степень) растеризации зависит от свойства <see cref="SegmentsSpline"/>
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeDrawingPoints()
			{
				mDrawingPoints.Clear();
				Vector3 prev = mControlPoints[0];
				mDrawingPoints.Add(prev);
				for (Int32 i = 1; i < mSegmentsSpline; i++)
				{
					Single time = (Single)i / mSegmentsSpline;
					Vector3 point = CalculatePoint(time);

					// Добавляем если длина больше
					if ((point - prev).sqrMagnitude > MinimalSqrLine)
					{
						mDrawingPoints.Add(point);
						prev = point;
					}
				}

				mDrawingPoints.Add(EndPoint);

				// Проверяем еще раз
				if ((mDrawingPoints[mDrawingPoints.Count - 1] - mDrawingPoints[mDrawingPoints.Count - 2]).sqrMagnitude < MinimalSqrLine)
				{
					mDrawingPoints.RemoveAt(mDrawingPoints.Count - 1);
					mDrawingPoints[mDrawingPoints.Count - 1] = EndPoint;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление длины сплайна
			/// </summary>
			/// <remarks>
			/// Реализация по умолчанию вычисляет длину сплайна по сумме отрезков растеризации
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ComputeLengthSpline()
			{
				mLength = 0;
				mSegmentsPath.Clear();
				for (Int32 i = 1; i < mDrawingPoints.Count; i++)
				{
					Single length = (mDrawingPoints[i] - mDrawingPoints[i - 1]).magnitude;
					mLength += length;
					TMoveSegment segment = new TMoveSegment(length, mLength);
					mSegmentsPath.Add(segment);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление выпуклого четырехугольника, заданного опорными точками внутри которого находится сплайн
			/// </summary>
			/// <remarks>
			/// Реализация по умолчанию вычисляет прямоугольник на основе контрольных точек
			/// </remarks>
			/// <returns>Прямоугольник</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Rect GetArea()
			{
				Single x_min = StartPoint.x, x_max = EndPoint.x, y_min = StartPoint.y, y_max = EndPoint.y;

				for (Int32 i = 0; i < mDrawingPoints.Count; i++)
				{
					Vector3 point = mDrawingPoints[i];
					if (point.x < x_min) x_min = point.x;
					if (point.x > x_max) x_max = point.x;
					if (point.y < y_min) y_min = point.y;
					if (point.y > y_max) y_max = point.y;
				}

				Rect rect_area = new Rect();
				rect_area.x = x_min;
				rect_area.width = x_max - x_min;
				rect_area.y = y_min;
				rect_area.height = y_max - y_min;

				return rect_area;
			}
			#endregion

			#region ======================================= МЕТОДЫ IMove ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение позиции на сплайне
			/// </summary>
			/// <param name="position">Положение точки на сплайне от 0 до длины сплайна</param>
			/// <returns>Позиция на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 GetMovePosition(Single position)
			{
				// Ограничения
				if (position >= Length)
				{
					return EndPoint;
				}
				if (position <= 0.0f)
				{
					return StartPoint;
				}

				// Находим в какой отрезок попадает эта позиция
				Int32 index = 0;
				for (Int32 i = 0; i < mSegmentsPath.Count; i++)
				{
					// Если позиция меньше значит она попала в отрезок
					if (position < mSegmentsPath[i].Summa)
					{
						index = i;
						break;
					}
				}

				// Перемещение относительно данного сегмента
				Single delta = 0;

				if (index == 0)
				{
					delta = position / mSegmentsPath[index].Length;
				}
				else
				{
					delta = (position - mSegmentsPath[index - 1].Summa) / mSegmentsPath[index].Length;
				}

				// Интерполируем позицию
				return Vector3.Lerp(mDrawingPoints[index], mDrawingPoints[index + 1], delta);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение скорости на сплайне
			/// </summary>
			/// <remarks>
			/// Это коэффициент изменения скорости на сплайне применяется для регулировки скорости
			/// </remarks>
			/// <param name="position">Положение точки на сплайне от 0 до длины сплайна</param>
			/// <returns>Скорость на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMoveVelocity(Single position)
			{
				return 1.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение направления на сплайне
			/// </summary>
			/// <param name="position">Положение точки на сплайне от 0 до длины сплайна</param>
			/// <returns>Направление на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public Vector3 GetMoveDirection(Single position)
			{
				return GetMovePosition(position + 0.01f);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение положения на сплайне
			/// </summary>
			/// <param name="position">Позиция на сплайне</param>
			/// <param name="epsilon">Погрешность при сравнении</param>
			/// <returns>Положение точки от 0 до 1</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetMoveTime(Vector3 position, Single epsilon = 0.01f)
			{
				// Просматриваем все отрезки и находим нужное положение
				for (Int32 i = 1; i < mDrawingPoints.Count; i++)
				{
					Vector3 p1 = mDrawingPoints[i] - mDrawingPoints[i - 1];
					Vector3 p2 = position - mDrawingPoints[i - 1];

					Single angle = Vector3.Dot(p2, p1) / Vector3.Dot(p2, p1);
					if (angle + epsilon > 0 && angle - epsilon < 1)
					{
						// TODO: Найти точное положение на всей кривой
						return angle;
					}
				}

				return -1;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnResetSpline()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnInitSpline()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление параметров сплайна
			/// </summary>
			/// <remarks>
			/// Реализация по умолчанию обновляет список точек для рисования и длину сплайна
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnUpdateSpline()
			{
				ComputeDrawingPoints();
				ComputeLengthSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка и корректировка расположения первой точки на сплайне.
			/// Применяется когда надо скорректировать отображения сплайна при его замкнутости
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void CheckCorrectStartPoint()
			{
				mDrawingPoints[mDrawingPoints.Count - 1] = StartPoint;
				if ((mDrawingPoints[mDrawingPoints.Count - 1] - mDrawingPoints[mDrawingPoints.Count - 2]).sqrMagnitude < MinimalSqrLine)
				{
					mDrawingPoints.RemoveAt(mDrawingPoints.Count - 1);
					mDrawingPoints[mDrawingPoints.Count - 1] = StartPoint;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка и корректировка расположения последней точки на сплайне
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void CheckCorrectEndPoint()
			{
				mDrawingPoints[mDrawingPoints.Count - 1] = EndPoint;
				if ((mDrawingPoints[mDrawingPoints.Count - 1] - mDrawingPoints[mDrawingPoints.Count - 2]).sqrMagnitude < MinimalSqrLine)
				{
					mDrawingPoints.RemoveAt(mDrawingPoints.Count - 1);
					mDrawingPoints[mDrawingPoints.Count - 1] = EndPoint;
				}
			}
			#endregion

			#region ======================================= РАБОТА С КОНТРОЛЬНЫМИ ТОЧКАМИ =============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение контрольной точки сплайна по индексу в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <returns>Контрольная точка сплайна в локальных координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Vector3 GetControlPoint(Int32 index)
			{
				return mControlPoints[index];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка контрольной точки сплайна по индексу в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetControlPoint(Int32 index, Vector3 point, Boolean update_spline = false)
			{
				mControlPoints[index] = point;
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление контрольной точки к сплайну в локальных координатах
			/// </summary>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddControlPoint(Vector3 point, Boolean update_spline = false)
			{
				Array.Resize(ref mControlPoints, mControlPoints.Length + 1);
				mControlPoints[mControlPoints.Length - 1] = point;
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка контрольной точки в сплайн в локальных координатах в указанной позиции
			/// </summary>
			/// <param name="index">Позиция(индекс) вставки точки</param>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void InsertControlPoint(Int32 index, Vector3 point, Boolean update_spline = false)
			{
				mControlPoints = XArray.InsertAt(mControlPoints, point, index);
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление контрольной точки в локальных координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) удаляемой точки</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RemoveControlPoint(Int32 index, Boolean update_spline = false)
			{
				mControlPoints = XArray.RemoveAt(mControlPoints, index);
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление контрольной точки в локальных координатах
			/// </summary>
			/// <param name="point">Контрольная точка сплайна в локальных координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RemoveControlPoint(Vector3 point, Boolean update_spline = false)
			{
				for (Int32 i = 0; i < mControlPoints.Length; i++)
				{
					if (mControlPoints[i].Approximately(point))
					{
						mControlPoints = XArray.RemoveAt(mControlPoints, i);
						if (update_spline)
						{
							this.OnUpdateSpline();
						}
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех контрольных точек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void ClearControlPoints()
			{
				Array.Clear(mControlPoints, 0, mControlPoints.Length);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================