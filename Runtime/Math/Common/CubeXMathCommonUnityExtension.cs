//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Общая математическая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXMathCommonUnityExtension.cs
*		Методы (математические) расширения к типам Unity.
*		Реализация математических методов расширения к типам Unity для обеспечения удобства работы.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup MathCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс расширяющий методы стандартных математических типов и типов Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XMathExtension
		{
			#region ======================================= МЕТОДЫ Move ===============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт перемещению компонента трансформации к указанной позиции
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveTo(this MonoBehaviour @this, Single duration, Vector3 target_position, TEasingType easing_type)
			{
				MoveTo(@this, @this.transform, duration, target_position, easing_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт перемещению компонента трансформации к указанной позиции
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveTo(this MonoBehaviour @this, Single duration, Vector3 target_position, TEasingType easing_type, Action on_completed)
			{
				MoveTo(@this, @this.transform, duration, target_position, easing_type, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт перемещению компонента трансформации к указанной позиции
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveTo(this MonoBehaviour @this, Transform transform, Single duration, Vector3 target_position, TEasingType easing_type)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(MoveToLinearIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(MoveToQuadInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(MoveToQuadOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(MoveToQuadInOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(MoveToCubeInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(MoveToCubeOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(MoveToCubeInOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(MoveToBackInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(MoveToBackOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(MoveToBackInOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(MoveToExpoInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(MoveToExpoOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(MoveToExpoInOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(MoveToSineInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(MoveToSineOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(MoveToSineInOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(MoveToElasticInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(MoveToElasticOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(MoveToElasticInOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(MoveToBounceInIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(MoveToBounceOutIteration(transform, duration, target_position));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(MoveToBounceInOutIteration(transform, duration, target_position));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт перемещению компонента трансформации к указанной позиции
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveTo(this MonoBehaviour @this, Transform transform, Single duration, Vector3 target_position, TEasingType easing_type, Action on_completed)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(MoveToLinearIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(MoveToQuadInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(MoveToQuadOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(MoveToQuadInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(MoveToCubeInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(MoveToCubeOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(MoveToCubeInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(MoveToBackInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(MoveToBackOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(MoveToBackInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(MoveToExpoInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(MoveToExpoOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(MoveToExpoInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(MoveToSineInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(MoveToSineOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(MoveToSineInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(MoveToElasticInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(MoveToElasticOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(MoveToElasticInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(MoveToBounceInIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(MoveToBounceOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(MoveToBounceInOutIteration(transform, duration, target_position, on_completed));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLinearIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.Linear(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToLinearIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.Linear(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToQuadInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.QuadIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToQuadInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.QuadIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToQuadOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.QuadOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToQuadOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.QuadOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToQuadInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.QuadInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToQuadInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.QuadInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToCubeInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.CubeIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToCubeInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.CubeIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToCubeOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.CubeOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToCubeOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.CubeOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToCubeInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.CubeInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToCubeInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.CubeInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBackInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BackIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBackInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BackIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBackOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BackOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBackOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BackOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBackInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BackInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBackInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BackInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToExpoInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ExpoIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToExpoInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ExpoIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToExpoOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ExpoOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToExpoOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ExpoOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToExpoInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ExpoInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToExpoInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ExpoInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToSineInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.SineIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToSineInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.SineIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToSineOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.SineOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToSineOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.SineOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToSineInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.SineInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToSineInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.SineInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToElasticInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ElasticIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToElasticInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ElasticIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToElasticOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ElasticOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToElasticOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ElasticOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToElasticInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ElasticInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToElasticInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.ElasticInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBounceInIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BounceIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBounceInIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BounceIn(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBounceOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BounceOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBounceOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BounceOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBounceInOutIteration(this Transform @this, Single duration, Vector3 target_position)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BounceInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма перемещения компонента трансформации
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время перемещения</param>
			/// <param name="target_position">Целевая позиция</param>
			/// <param name="on_completed">Обработчик события окончания перемещения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator MoveToBounceInOutIteration(this Transform @this, Single duration, Vector3 target_position, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Vector3 start_position = @this.position;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.position = XMathEasing.BounceInOut(ref start_position, ref target_position, time);
					yield return null;
				}

				@this.position = target_position;

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ Rotation ===========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationTo(this MonoBehaviour @this, Single duration, Quaternion target, TEasingType easing_type)
			{
				RotationTo(@this, @this.transform, duration, target, easing_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationTo(this MonoBehaviour @this, Single duration, Quaternion target, TEasingType easing_type, Action on_completed)
			{
				RotationTo(@this, @this.transform, duration, target, easing_type, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationTo(this MonoBehaviour @this, Transform transform, Single duration, Quaternion target, TEasingType easing_type)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToLinearIteration(transform, duration, target));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToQuadInIteration(transform, duration, target));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToQuadOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToQuadInOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToCubeInIteration(transform, duration, target));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToCubeOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToCubeInOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToBackInIteration(transform, duration, target));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToBackOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToBackInOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToExpoInIteration(transform, duration, target));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToExpoOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToExpoInOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToSineInIteration(transform, duration, target));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToSineOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToSineInOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToElasticInIteration(transform, duration, target));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToElasticOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToElasticInOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToBounceInIteration(transform, duration, target));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToBounceOutIteration(transform, duration, target));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToBounceInOutIteration(transform, duration, target));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationTo(this MonoBehaviour @this, Transform transform, Single duration, Quaternion target, TEasingType easing_type, Action on_completed)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToLinearIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToQuadInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToQuadOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToQuadInOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToCubeInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToCubeOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToCubeInOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToBackInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToBackOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToBackInOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToExpoInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToExpoOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToExpoInOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToSineInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToSineOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToSineInOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToElasticInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToElasticOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToElasticInOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToBounceInIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToBounceOutIteration(transform, duration, target, on_completed));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToBounceInOutIteration(transform, duration, target, on_completed));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToLinearIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, time);
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToLinearIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, time);
					yield return null;
				}

				@this.rotation = target;
				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToQuadInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, time * time);
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToQuadInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, time * time);
					yield return null;
				}

				@this.rotation = target;
				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToQuadOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.QuadOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToQuadOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.QuadOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToQuadInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.QuadInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToQuadInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.QuadInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToCubeInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.CubeIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToCubeInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.CubeIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToCubeOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.CubeOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToCubeOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.CubeOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToCubeInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.CubeInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToCubeInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.CubeInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBackInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BackIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBackInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BackIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBackOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BackOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBackOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BackOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBackInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BackInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBackInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BackInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToExpoInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ExpoIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToExpoInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ExpoIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToExpoOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ExpoOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToExpoOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ExpoOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToExpoInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ExpoInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Кватернион вращения</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToExpoInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ExpoInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToSineInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.SineIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToSineInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.SineIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToSineOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.SineOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToSineOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.SineOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToSineInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.SineInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToSineInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.SineInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToElasticInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ElasticIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToElasticInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ElasticIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToElasticOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ElasticOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToElasticOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ElasticOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToElasticInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ElasticInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToElasticInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.ElasticInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBounceInIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BounceIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBounceInIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BounceIn(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBounceOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BounceOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBounceOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BounceOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBounceInOutIteration(this Transform @this, Single duration, Quaternion target)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BounceInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации к указанному кватерниону
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target">Целевой кватернион</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToBounceInOutIteration(this Transform @this, Single duration, Quaternion target, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Quaternion start_rotation = @this.rotation;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.rotation = Quaternion.Lerp(start_rotation, target, XMathEasing.BounceInOut(0, 1, time));
					yield return null;
				}

				@this.rotation = target;

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ RotationX ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси X
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToX(this MonoBehaviour @this, Single duration, Single target_angle, TEasingType easing_type)
			{
				RotationToX(@this, @this.transform, duration, target_angle, easing_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси X
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToX(this MonoBehaviour @this, Single duration, Single target_angle, TEasingType easing_type, Action on_completed)
			{
				RotationToX(@this, @this.transform, duration, target_angle, easing_type, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси X
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToX(this MonoBehaviour @this, Transform transform, Single duration, Single target_angle, TEasingType easing_type)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToXLinearIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToXQuadInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToXQuadOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToXQuadInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToXCubeInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToXCubeOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToXCubeInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToXBackInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToXBackOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToXBackInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToXExpoInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToXExpoOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToXExpoInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToXSineInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToXSineOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToXSineInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToXElasticInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToXElasticOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToXElasticInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToXBounceInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToXBounceOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToXBounceInOutIteration(transform, duration, target_angle));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси X
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToX(this MonoBehaviour @this, Transform transform, Single duration, Single target_angle, TEasingType easing_type, Action on_completed)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToXLinearIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToXQuadInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToXQuadOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToXQuadInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToXCubeInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToXCubeOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToXCubeInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToXBackInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToXBackOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToXBackInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToXExpoInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToXExpoOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToXExpoInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToXSineInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToXSineOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToXSineInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToXElasticInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToXElasticOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToXElasticInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToXBounceInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToXBounceOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToXBounceInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXLinearIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.Linear(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXLinearIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.Linear(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXQuadInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.QuadIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXQuadInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.QuadIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXQuadOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.QuadOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXQuadOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.QuadOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXQuadInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.QuadInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXQuadInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.QuadInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXCubeInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.CubeIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXCubeInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.CubeIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXCubeOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.CubeOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXCubeOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.CubeOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXCubeInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.CubeInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXCubeInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.CubeInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBackInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BackIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBackInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BackIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBackOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BackOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBackOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BackOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBackInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BackInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBackInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BackInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXExpoInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ExpoIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXExpoInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ExpoIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXExpoOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ExpoOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXExpoOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ExpoOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXExpoInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ExpoInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXExpoInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ExpoInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXSineInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.SineIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXSineInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.SineIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXSineOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.SineOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXSineOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.SineOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXSineInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.SineInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXSineInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.SineInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXElasticInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ElasticIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXElasticInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ElasticIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXElasticOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ElasticOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXElasticOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ElasticOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXElasticInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ElasticInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXElasticInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.ElasticInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBounceInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BounceIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBounceInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BounceIn(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBounceOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BounceOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBounceOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BounceOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBounceInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BounceInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси X
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToXBounceInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.x;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(XMathEasing.BounceInOut(start_angle, target_angle, time), @this.eulerAngles.y, @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(target_angle, @this.eulerAngles.y, @this.eulerAngles.z);

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ RotationY ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Y
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToY(this MonoBehaviour @this, Single duration, Single target_angle, TEasingType easing_type)
			{
				RotationToY(@this, @this.transform, duration, target_angle, easing_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Y
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToY(this MonoBehaviour @this, Single duration, Single target_angle, TEasingType easing_type, Action on_completed)
			{
				RotationToY(@this, @this.transform, duration, target_angle, easing_type, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Y
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToY(this MonoBehaviour @this, Transform transform, Single duration, Single target_angle, TEasingType easing_type)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToYLinearIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToYQuadInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToYQuadOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToYQuadInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToYCubeInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToYCubeOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToYCubeInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToYBackInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToYBackOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToYBackInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToYExpoInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToYExpoOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToYExpoInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToYSineInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToYSineOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToYSineInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToYElasticInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToYElasticOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToYElasticInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToYBounceInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToYBounceOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToYBounceInOutIteration(transform, duration, target_angle));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Y
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToY(this MonoBehaviour @this, Transform transform, Single duration, Single target_angle, TEasingType easing_type, Action on_completed)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToYLinearIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToYQuadInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToYQuadOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToYQuadInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToYCubeInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToYCubeOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToYCubeInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToYBackInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToYBackOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToYBackInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToYExpoInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToYExpoOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToYExpoInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToYSineInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToYSineOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToYSineInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToYElasticInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToYElasticOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToYElasticInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToYBounceInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToYBounceOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToYBounceInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYLinearIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.Linear(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYLinearIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.Linear(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYQuadInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.QuadIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYQuadInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.QuadIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYQuadOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.QuadOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYQuadOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.QuadOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYQuadInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.QuadInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYQuadInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.QuadInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYCubeInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.CubeIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYCubeInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.CubeIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYCubeOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.CubeOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYCubeOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.CubeOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYCubeInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.CubeInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYCubeInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.CubeInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBackInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BackIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBackInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BackIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBackOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BackOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBackOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BackOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBackInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BackInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBackInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BackInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYExpoInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ExpoIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYExpoInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ExpoIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYExpoOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ExpoOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYExpoOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ExpoOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYExpoInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ExpoInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYExpoInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ExpoInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYSineInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.SineIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYSineInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.SineIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYSineOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.SineOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYSineOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.SineOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYSineInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.SineInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYSineInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.SineInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYElasticInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ElasticIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYElasticInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ElasticIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYElasticOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ElasticOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYElasticOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ElasticOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYElasticInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ElasticInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYElasticInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.ElasticInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBounceInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BounceIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBounceInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BounceIn(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBounceOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BounceOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBounceOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BounceOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBounceInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BounceInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Y
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToYBounceInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.y;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, XMathEasing.BounceInOut(start_angle, target_angle, time), @this.eulerAngles.z);
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, target_angle, @this.eulerAngles.z);

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ RotationZ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Z
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToZ(this MonoBehaviour @this, Single duration, Single target_angle, TEasingType easing_type)
			{
				RotationToZ(@this, @this.transform, duration, target_angle, easing_type);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Z
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToZ(this MonoBehaviour @this, Single duration, Single target_angle, TEasingType easing_type, Action on_completed)
			{
				RotationToZ(@this, @this.transform, duration, target_angle, easing_type, on_completed);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Z
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToZ(this MonoBehaviour @this, Transform transform, Single duration, Single target_angle, TEasingType easing_type)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToZLinearIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToZQuadInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToZQuadOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToZQuadInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToZCubeInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToZCubeOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToZCubeInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToZBackInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToZBackOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToZBackInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToZExpoInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToZExpoOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToZExpoInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToZSineInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToZSineOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToZSineInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToZElasticInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToZElasticOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToZElasticInOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToZBounceInIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToZBounceOutIteration(transform, duration, target_angle));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToZBounceInOutIteration(transform, duration, target_angle));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт вращению компонента трансформации к указанному углу по оси Z
			/// </summary>
			/// <param name="this">Пользовательский компонент</param>
			/// <param name="transform">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="easing_type">Тип функции скорости</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RotationToZ(this MonoBehaviour @this, Transform transform, Single duration, Single target_angle, TEasingType easing_type, Action on_completed)
			{
				switch (easing_type)
				{
					case TEasingType.Linear:
						{
							@this.StartCoroutine(RotationToZLinearIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadIn:
						{
							@this.StartCoroutine(RotationToZQuadInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadOut:
						{
							@this.StartCoroutine(RotationToZQuadOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.QuadInOut:
						{
							@this.StartCoroutine(RotationToZQuadInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeIn:
						{
							@this.StartCoroutine(RotationToZCubeInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeOut:
						{
							@this.StartCoroutine(RotationToZCubeOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.CubeInOut:
						{
							@this.StartCoroutine(RotationToZCubeInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackIn:
						{
							@this.StartCoroutine(RotationToZBackInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackOut:
						{
							@this.StartCoroutine(RotationToZBackOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BackInOut:
						{
							@this.StartCoroutine(RotationToZBackInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoIn:
						{
							@this.StartCoroutine(RotationToZExpoInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoOut:
						{
							@this.StartCoroutine(RotationToZExpoOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ExpoInOut:
						{
							@this.StartCoroutine(RotationToZExpoInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineIn:
						{
							@this.StartCoroutine(RotationToZSineInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineOut:
						{
							@this.StartCoroutine(RotationToZSineOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.SineInOut:
						{
							@this.StartCoroutine(RotationToZSineInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticIn:
						{
							@this.StartCoroutine(RotationToZElasticInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticOut:
						{
							@this.StartCoroutine(RotationToZElasticOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.ElasticInOut:
						{
							@this.StartCoroutine(RotationToZElasticInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceIn:
						{
							@this.StartCoroutine(RotationToZBounceInIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceOut:
						{
							@this.StartCoroutine(RotationToZBounceOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					case TEasingType.BounceInOut:
						{
							@this.StartCoroutine(RotationToZBounceInOutIteration(transform, duration, target_angle, on_completed));
						}
						break;
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZLinearIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.Linear(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZLinearIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.Linear(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZQuadInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.QuadIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZQuadInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.QuadIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZQuadOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.QuadOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZQuadOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.QuadOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZQuadInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.QuadInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZQuadInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.QuadInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZCubeInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.CubeIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZCubeInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.CubeIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZCubeOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.CubeOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZCubeOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.CubeOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZCubeInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.CubeInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZCubeInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.CubeInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBackInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BackIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBackInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BackIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBackOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BackOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBackOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BackOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBackInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BackInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBackInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BackInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZExpoInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ExpoIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZExpoInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ExpoIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZExpoOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ExpoOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZExpoOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ExpoOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZExpoInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ExpoInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZExpoInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ExpoInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZSineInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.SineIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZSineInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.SineIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZSineOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.SineOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZSineOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.SineOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZSineInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.SineInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZSineInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.SineInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZElasticInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ElasticIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZElasticInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ElasticIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZElasticOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ElasticOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZElasticOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ElasticOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZElasticInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ElasticInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZElasticInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.ElasticInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBounceInIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BounceIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBounceInIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BounceIn(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBounceOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BounceOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBounceOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BounceOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBounceInOutIteration(this Transform @this, Single duration, Single target_angle)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BounceInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подпрограмма вращения компонента трансформации по оси Z
			/// </summary>
			/// <param name="this">Компонент трансформации</param>
			/// <param name="duration">Время вращения</param>
			/// <param name="target_angle">Целевой угол</param>
			/// <param name="on_completed">Обработчик события окончания вращения</param>
			/// <returns>Перечислитель</returns>
			//---------------------------------------------------------------------------------------------------------
			public static IEnumerator RotationToZBounceInOutIteration(this Transform @this, Single duration, Single target_angle, Action on_completed)
			{
				Single time = 0;
				Single start_time = 0;
				Single start_angle = @this.eulerAngles.z;
				while (time < 1)
				{
					start_time += Time.unscaledDeltaTime;
					time = start_time / duration;
					@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, XMathEasing.BounceInOut(start_angle, target_angle, time));
					yield return null;
				}

				@this.eulerAngles = new Vector3(@this.eulerAngles.x, @this.eulerAngles.y, target_angle);

				on_completed();
			}
			#endregion

			#region ======================================= МЕТОДЫ ПРЕОБРАЗОВАНИЯ =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в вектор Vector2Df
			/// </summary>
			/// <param name="@this">Вектор</param>
			/// <returns>Вектор Vector2Df</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2Df ToVector2Df(this Vector2 @this)
			{
				return (new Vector2Df(@this.x, @this.y));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в вектор Vector3Df
			/// </summary>
			/// <param name="@this">Вектор</param>
			/// <returns>Вектор Vector3Df</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3Df ToVector3Df(this Vector3 @this)
			{
				return (new Vector3Df(@this.x, @this.y, @this.z));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование в кватернион Quaternion3Df
			/// </summary>
			/// <param name="@this">Кватернион</param>
			/// <returns>Кватернион Quaternion3Df</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Quaternion3Df ToQuaternion3Df(this Quaternion @this)
			{
				return (new Quaternion3Df(@this.x, @this.y, @this.z, @this.w));
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================