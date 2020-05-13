//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема для работы со сплайнами
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSplineBase.cs
*		Базовый компонент определяющий основное параметры сплайна.
*		Реализация компонента определяющего основное параметры сплайна и его базовую функциональность
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
		/// Базовый компонент определяющий основное параметры сплайна
		/// </summary>
		/// <remarks>
		/// Реализация компонента определяющего основное параметры сплайна и его базовую функциональность
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Math/SplineBezier")]
		public abstract class CubeXSplineBase : MonoBehaviour, ICubeXSpline3D
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
			internal TDimensionPlaneSelect mSnapPlane;
			[SerializeField]
			internal List<Vector3> mDrawingPoints;
			[SerializeField]
			internal Single mLength = 0;
			[SerializeField]
			internal List<TMoveSegment> mSegmentsPath;
			[NonSerialized]
			internal Int32 mCurrentSegmentPath;
			[NonSerialized]
			internal Vector3 mCurrentPosition;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Single mSizeControlHandle = 10;
			[SerializeField]
			internal Color mColorControlStart = Color.cyan;
			[SerializeField]
			internal Color mColorControl = Color.red;
			[SerializeField]
			internal Color mColorControlEnd = Color.magenta;
			[SerializeField]
			internal Color mColorSpline = Color.green;
			[SerializeField]
			internal Color mColorSplineAlt = Color.green;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
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
			/// Данный параметр используется при растеризации сплайна
			/// </remarks>
			public Int32 SegmentsSpline
			{
				get { return mSegmentsSpline; }
				set
				{
					if(mSegmentsSpline != value)
					{
						mSegmentsSpline = value;
						OnUpdateSpline();
					}
				}
			}

			/// <summary>
			/// Список сегментов пути
			/// </summary>
			public List<TMoveSegment> SegmentsPath
			{
				get { return mSegmentsPath; }
				set { mSegmentsPath = value; }
			}

			/// <summary>
			/// Плоскость для ограничение перемещения при модификации точки сплайна
			/// </summary>
			public TDimensionPlaneSelect SnapPlane
			{
				get { return mSnapPlane; }
				set { mSnapPlane = value; }
			}

			//
			// ДАННЫЕ В РЕЖИМЕ РЕДАКТОРА
			//
#if UNITY_EDITOR
			/// <summary>
			/// Размер ручки для редактирования сплайна
			/// </summary>
			public Single SizeControlHandle
			{
				get { return mSizeControlHandle; }
				set { mSizeControlHandle = value; }
			}

			/// <summary>
			/// Цвет начальной точки сплайна
			/// </summary>
			public Color ColorControlStart
			{
				get { return mColorControlStart; }
				set { mColorControlStart = value; }
			}

			/// <summary>
			/// Цвет контрольных точек сплайна
			/// </summary>
			public Color ColorControl
			{
				get { return mColorControl; }
				set { mColorControl = value; }
			}

			/// <summary>
			/// Цвет конечной точки сплайна
			/// </summary>
			public Color ColorControlEnd
			{
				get { return mColorControlEnd; }
				set { mColorControlEnd = value; }
			}

			/// <summary>
			/// Цвет сплайна
			/// </summary>
			public Color ColorSpline
			{
				get { return mColorSpline; }
				set { mColorSpline = value; }
			}

			/// <summary>
			/// Альтернативный цвет сплайна
			/// </summary>
			public Color ColorSplineAlt
			{
				get { return mColorSplineAlt; }
				set { mColorSplineAlt = value; }
			}
#endif
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

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				OnResetSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				OnInitSpline();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисования сплайна в режиме редактора
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawGizmosSpline()
			{
#if UNITY_EDITOR
				// Рисуем саму кривую
				if (mDrawingPoints != null)
				{
					for (Int32 i = 1; i < mDrawingPoints.Count; i++)
					{
						Vector3 from = transform.TransformPoint(mDrawingPoints[i - 1]);
						Vector3 to = transform.TransformPoint(mDrawingPoints[i]);
						if (i % 2 == 0)
						{
							Gizmos.color = mColorSpline;
							Gizmos.DrawLine(from, to);
						}
						else
						{
							Gizmos.color = mColorSplineAlt;
							Gizmos.DrawLine(from, to);
						}
					}
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисования контрольной точки в режиме редактора
			/// </summary>
			/// <param name="index">Индекс контрольной точки</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawGizmosControlPoint(Int32 index)
			{
#if UNITY_EDITOR
				Vector3 point = GetControlPointWorld(index);
				if (index == 0)
				{
					Gizmos.color = mColorControlStart;
				}
				else
				{
					if (index == mControlPoints.Length - 1)
					{
						Gizmos.color = mColorControlEnd;
					}
					else
					{
						Gizmos.color = mColorControl;
					}
				}

				Gizmos.DrawSphere(point, mSizeControlHandle / 100 * UnityEditor.HandleUtility.GetHandleSize(point));
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисования сплайна в режиме игры (только в редакторе)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDrawEditorSpline()
			{
#if UNITY_EDITOR
				// Рисуем саму кривую
				if (mDrawingPoints != null)
				{
					for (Int32 i = 1; i < mDrawingPoints.Count; i++)
					{
						Vector3 from = transform.TransformPoint(mDrawingPoints[i - 1]);
						Vector3 to = transform.TransformPoint(mDrawingPoints[i]);
						if (i % 2 == 0)
						{
							Debug.DrawLine(from, to, mColorSpline);
						}
						else
						{
							Debug.DrawLine(from, to, mColorSplineAlt);
						}
					}
				}
#endif
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
			/// Вычисление первой производной точки на сплайне
			/// </summary>
			/// <remarks>
			/// Первая производная показывает скорость изменения функции в данной точки.
			/// Физическим смысл производной - скорость в данной точке 
			/// </remarks>
			/// <param name="time">Положение точки от 0 до 1</param>
			/// <returns>Скорость точки на сплайне</returns>
			//---------------------------------------------------------------------------------------------------------
			public abstract Vector3 CalculateFirstDerivative(Single time);

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
				CheckCorrectEndPoint();
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

				for (Int32 i = 0; i < mControlPoints.Length; i++)
				{
					Vector3 point = mControlPoints[i];
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
					return transform.TransformPoint(EndPoint);
				}
				if (position <= 0.0f)
				{
					return transform.TransformPoint(StartPoint);
				}

				// Находим в какой отрезок попадает эта позиция
				mCurrentSegmentPath = 0;
				for (Int32 i = 0; i < mSegmentsPath.Count; i++)
				{
					// Если позиция меньше значит она попала в отрезок
					if (position < mSegmentsPath[i].Summa)
					{
						mCurrentSegmentPath = i;
						break;
					}
				}

				// Перемещение относительно данного сегмента
				Single delta = 0;

				if (mCurrentSegmentPath == 0)
				{
					delta = position / mSegmentsPath[mCurrentSegmentPath].Length;
				}
				else
				{
					delta = (position - mSegmentsPath[mCurrentSegmentPath - 1].Summa) / mSegmentsPath[mCurrentSegmentPath].Length;
				}

				// Интерполируем позицию
				mCurrentPosition = transform.TransformPoint(Vector3.Lerp(mDrawingPoints[mCurrentSegmentPath],
					mDrawingPoints[mCurrentSegmentPath + 1], delta));

				return mCurrentPosition;
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
			public virtual Single GetMoveVelocity(Single position)
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
				Int32 index = mCurrentSegmentPath;
				if (!(mCurrentSegmentPath == mSegmentsPath.Count - 1))
				{
					index = mCurrentSegmentPath + 1;
				}


				return transform.TransformPoint(mDrawingPoints[index + 1]);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение положения на сплайне
			/// </summary>
			/// <param name="position">Позиция на сплайне</param>
			/// <param name="epsilon">Погрешность при сравнении</param>
			/// <returns>Положение точки от 0 до 1</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Single GetMoveTime(Vector3 position, Single epsilon = 0.01f)
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
				mDrawingPoints = new List<Vector3>(10);
				mDrawingPoints.Add(Vector3.zero);

				mSegmentsPath = new List<TMoveSegment>();
				mSegmentsSpline = 10;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация параметров сплайна
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnInitSpline()
			{
				if (mDrawingPoints == null)
				{
					mDrawingPoints = new List<Vector3>(10);
					mDrawingPoints.Add(Vector3.zero);
				}

				if (mSegmentsPath == null)
				{
					mSegmentsPath = new List<TMoveSegment>();
				}

				if (mSegmentsSpline < 1)
				{
					mSegmentsSpline = 10;
				}
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
			/// Получение контрольной точки сплайна по индексу в мировых координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <returns>Контрольная точка сплайна в мировых координатах</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Vector3 GetControlPointWorld(Int32 index)
			{
				return transform.TransformPoint(mControlPoints[index]);
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
			/// Установка контрольной точки сплайна по индексу в мировых координатах
			/// </summary>
			/// <param name="index">Позиция(индекс) точки</param>
			/// <param name="point">Контрольная точка сплайна в мировых координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetControlPointWorld(Int32 index, Vector3 point, Boolean update_spline = false)
			{
				mControlPoints[index] = transform.InverseTransformPoint(point);
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
			/// Добавление контрольной точки к сплайну в мировых координатах
			/// </summary>
			/// <param name="point">Точка сплайна в мировых координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddControlPointWorld(Vector3 point, Boolean update_spline = false)
			{
				Array.Resize(ref mControlPoints, mControlPoints.Length + 1);
				mControlPoints[mControlPoints.Length - 1] = transform.InverseTransformPoint(point);
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
			/// Вставка контрольной точки в сплайн в мировых координатах в указанной позиции
			/// </summary>
			/// <param name="index">Позиция(индекс) вставки точки</param>
			/// <param name="point">Точка сплайна в мировых координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void InsertControlPointWorld(Int32 index, Vector3 point, Boolean update_spline = false)
			{
				point = transform.InverseTransformPoint(point);
				mControlPoints = XArray.InsertAt(mControlPoints, point, index);
				if (update_spline)
				{
					this.OnUpdateSpline();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление контрольной точки
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
			/// Удаление контрольной точки в мировых координатах
			/// </summary>
			/// <param name="point">Контрольная точка сплайна в мировых координатах</param>
			/// <param name="update_spline">Статус обновления сплайна</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void RemoveControlPointWorld(Vector3 point, Boolean update_spline = false)
			{
				point = transform.InverseTransformPoint(point);
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