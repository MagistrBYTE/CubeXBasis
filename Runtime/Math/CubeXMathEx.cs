//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXMathEx.cs
*		Основные математические методы и функции.
*		Реализация базовых и вспомогательных методов для работы с математикой и вещественным типом двойной точности.
*		Необходимость обусловлена применением в ряде будущих проектов вычислений с большими значениями. Также реализованы
*	методы интерполяция промежуточных значений величины и решение различных математических уравнений, систем уравнений,
*	в том числе квадратных и биквадратных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Math Модуль математической системы
		//! Математический модуль реализующий работу с математическими и геометрическими структурами данных
		//! \ingroup Basis
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий математические методы и функции
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XMath
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Значение, для которого все абсолютные значения меньше, чем считаются равными нулю
			/// </summary>
			public const Double ZeroTolerance_d = 0.000000001;

			/// <summary>
			/// Значение, для которого все абсолютные значения меньше, чем считаются равными нулю
			/// </summary>
			public const Single ZeroTolerance_f = 0.000001f;

			/// <summary>
			/// Точность вещественного числа
			/// </summary>
			public const Double Eplsilon_d = 0.00000001;

			/// <summary>
			/// Точность вещественного числа
			/// </summary>
			public const Single Eplsilon_f = 0.0000001f;

			/// <summary>
			/// Точность вещественного числа
			/// </summary>
			public const Double Eplsilon_3d = 0.001;

			/// <summary>
			/// Точность вещественного числа
			/// </summary>
			public const Single Eplsilon_3f = 0.001f;

			/// <summary>
			/// Коэффициент для преобразования радианов в градусы
			/// </summary>
			public const Double RadianToDegree_d = 57.29577951;

			/// <summary>
			/// Коэффициент для преобразования радианов в градусы
			/// </summary>
			public const Single RadianToDegree_f = 57.29577951f;

			/// <summary>
			/// Коэффициент для преобразования градусов в радианы
			/// </summary>
			public const Double DegreeToRadian_d = 0.01745329;

			/// <summary>
			/// Коэффициент для преобразования градусов в радианы
			/// </summary>
			public const Single DegreeToRadian_f = 0.01745329f;

			/// <summary>
			/// Экспонента
			/// </summary>
			public const Double Exponent_d = 2.71828182;

			/// <summary>
			/// Экспонента
			/// </summary>
			public const Single Exponent_f = 2.71828182f;

			/// <summary>
			/// Log2(e)
			/// </summary>
			public const Double Log2E_d = 1.44269504;

			/// <summary>
			/// Log2(e)
			/// </summary>
			public const Single Log2E_f = 1.44269504f;

			/// <summary>
			/// Log10(e)
			/// </summary>
			public const Double Log10E_d = 0.43429448;

			/// <summary>
			/// Log10(e)
			/// </summary>
			public const Single Log10E_f = 0.43429448f;

			/// <summary>
			/// Ln(2)
			/// </summary>
			public const Double Ln2d = 0.69314718;

			/// <summary>
			/// Ln(2)
			/// </summary>
			public const Single Ln2f = 0.69314718f;

			/// <summary>
			/// Ln(10)
			/// </summary>
			public const Double Ln10d = 2.30258509;

			/// <summary>
			/// Ln(10)
			/// </summary>
			public const Single Ln10f = 2.30258509f;

			/// <summary>
			/// Число Pi * 2
			/// </summary>
			public const Double PI2d = 6.283185306;

			/// <summary>
			/// Число Pi * 2
			/// </summary>
			public const Single PI2f = 6.283185306f;

			/// <summary>
			/// Число Pi
			/// </summary>
			public const Double PI_d = 3.141592653;

			/// <summary>
			/// Число Pi
			/// </summary>
			public const Single PI_f = 3.141592653f;

			/// <summary>
			/// Число Pi/2
			/// </summary>
			public const Double PI_2d = 1.570796326;

			/// <summary>
			/// Число Pi/2
			/// </summary>
			public const Single PI_2f = 1.570796326f;

			/// <summary>
			/// Число Pi/3
			/// </summary>
			public const Double PI_3d = 1.047197551;

			/// <summary>
			/// Число Pi/3
			/// </summary>
			public const Single PI_3f = 1.047197551f;

			/// <summary>
			/// Число Pi/4
			/// </summary>
			public const Double PI_4d = 0.785398163;

			/// <summary>
			/// Число Pi/4
			/// </summary>
			public const Single PI_4f = 0.785398163f;

			/// <summary>
			/// Число Pi/6
			/// </summary>
			public const Double PI_6d = 0.523598775598;

			/// <summary>
			/// Число Pi/6
			/// </summary>
			public const Double PI_6f = 0.523598775598;
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на нулевое значение
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsZero(Double value)
			{
				return Math.Abs(value) < ZeroTolerance_d;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на нулевое значение
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsZero(Single value)
			{
				return Math.Abs(value) < ZeroTolerance_f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на единичное значение
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsOne(Double value)
			{
				return IsZero(value - 1.0);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на единичное значение
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsOne(Single value)
			{
				return IsZero(value - 1.0);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение значения в пределах от 0 до 1
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double Clamp01(Double value)
			{
				if (value < 0) value = 0;
				if (value > 1) value = 1;

				return value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение значения в пределах от 0 до 1
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single Clamp01(Single value)
			{
				if (value < 0) value = 0;
				if (value > 1) value = 1;

				return value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение значения в указанных пределах
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="min">Минимальное значение</param>
			/// <param name="max">Максимальное значение</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double Clamp(Double value, Double min, Double max)
			{
				if (value < min) value = min;
				if (value > max) value = max;

				return value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение значения в указанных пределах
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="min">Минимальное значение</param>
			/// <param name="max">Максимальное значение</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single Clamp(Single value, Single min, Single max)
			{
				if (value < min) value = min;
				if (value > max) value = max;

				return value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Ограничение значения в указанных пределах
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="min">Минимальное значение</param>
			/// <param name="max">Максимальное значение</param>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 Clamp(Int32 value, Int32 min, Int32 max)
			{
				if (value < min) value = min;
				if (value > max) value = max;

				return value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений
			/// </summary>
			/// <param name="a">Первое значение</param>
			/// <param name="b">Второе значение</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(Double a, Double b)
			{
				if (Math.Abs(a - b) < Eplsilon_3d)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений
			/// </summary>
			/// <param name="a">Первое значение</param>
			/// <param name="b">Второе значение</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(Single a, Single b)
			{
				if (Math.Abs(a - b) < Eplsilon_3f)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений
			/// </summary>
			/// <param name="a">Первое значение</param>
			/// <param name="b">Второе значение</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(Double a, Double b, Double epsilon = 0.0001)
			{
				if (Math.Abs(a - b) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства значений
			/// </summary>
			/// <param name="a">Первое значение</param>
			/// <param name="b">Второе значение</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(Single a, Single b, Single epsilon = 0.001f)
			{
				if (Math.Abs(a - b) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление квадратного корня
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Квадратный корень</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single Sqrt(Single value)
			{
				return ((Single)Math.Sqrt(value));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление обратного квадратного корня
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Значение обратного квадратного корня</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double InvSqrt(Double value)
			{
				Double result = Math.Sqrt(value);

				if (result > ZeroTolerance_d)
				{
					return 1.0 / result;
				}

				return 1.0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление обратного квадратного корня
			/// </summary>
			/// <param name="value">Значение</param>
			/// <returns>Значение обратного квадратного корня</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single InvSqrt(Single value)
			{
				Single result = (Single)Math.Sqrt(value);

				if (result > ZeroTolerance_f)
				{
					return 1.0f / result;
				}

				return 1.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление синуса
			/// </summary>
			/// <param name="radians">Угол в радианах</param>
			/// <returns>Значение синуса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single Sin(Single radians)
			{
				return ((Single)Math.Sin(radians));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление косинуса
			/// </summary>
			/// <param name="radians">Угол в радианах</param>
			/// <returns>Значение косинуса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single Cos(Single radians)
			{
				return ((Single)Math.Cos(radians));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование интервала одного к другому
			/// </summary>
			/// <remarks>
			/// Под другому это можно интерпретировать как пересечение по Y вертикальной линии отрезка проходящего через точки:
			/// - x1 = dest_start
			/// - x2 = dest_end
			/// - y1 = source_start
			/// - y2 = source_end
			/// - px = value
			/// </remarks>
			/// <param name="dest_start">Начала целевого интервала</param>
			/// <param name="dest_end">Конец целевого интервала</param>
			/// <param name="source_start">Начало исходного интервала</param>
			/// <param name="source_end">Конец исходного интервала</param>
			/// <param name="value">Исходное значение</param>
			/// <returns>Целевое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double ConvertInterval(Double dest_start, Double dest_end, Double source_start,
				Double source_end, Double value)
			{
				Double x1 = dest_start;
				Double x2 = dest_end;
				Double y1 = source_start;
				Double y2 = source_end;

				Double k = (y2 - y1) / (x2 - x1);
				Double b = y1 - (k * x1);

				Double result = (k * value + b);

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование интервала одного к другому
			/// </summary>
			/// <remarks>
			/// Под другому это можно интерпретировать как пересечение по Y вертикальной линии отрезка проходящего через точки:
			/// - x1 = dest_start
			/// - x2 = dest_end
			/// - y1 = source_start
			/// - y2 = source_end
			/// - px = value
			/// </remarks>
			/// <param name="dest_start">Начала целевого интервала</param>
			/// <param name="dest_end">Конец целевого интервала</param>
			/// <param name="source_start">Начало исходного интервала</param>
			/// <param name="source_end">Конец исходного интервала</param>
			/// <param name="value">Исходное значение</param>
			/// <returns>Целевое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single ConvertInterval(Single dest_start, Single dest_end, Single source_start,
				Single source_end, Single value)
			{
				Single x1 = dest_start;
				Single x2 = dest_end;
				Single y1 = source_start;
				Single y2 = source_end;

				Single k = (y2 - y1) / (x2 - x1);
				Single b = y1 - (k * x1);

				Single result = (k * value + b);

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование процента в часть
			/// </summary>
			/// <param name="percent">Процент от 0 до 100</param>
			/// <returns>Часть</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single ToPartFromPercent(Int32 percent)
			{
				Single p = percent;
				if (percent <= 0)
				{
					p = 0;
				}
				if(percent >= 100)
				{
					p = 100;
				}

				return p / 100.0f;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Округление до нужного целого
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="round">Степень округления</param>
			/// <returns>Округленное значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Double RoundToNearest(Double value, Int32 round)
			{
				if (value >= 0)
				{
					Double result = Math.Floor((value + (Double)round / 2) / round) * round;
					return (result);
				}
				else
				{
					Double result = Math.Ceiling((value - (Double)round / 2) / round) * round;
					return (result);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Округление до нужного целого
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="round">Степень округления</param>
			/// <returns>Округленное значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single RoundToNearest(Single value, Int32 round)
			{
				if (value >= 0)
				{
					return (Single)(Math.Floor((value + (Single)round / 2) / round) * round);
				}
				else
				{
					return (Single)(Math.Ceiling((value - (Single)round / 2) / round) * round);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕШЕНИЙ УРАВНЕНИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Решение квадратного уравнения ax2 + bx + c = 0
			/// </summary>
			/// <param name="a">Параметр a</param>
			/// <param name="b">Параметр b</param>
			/// <param name="c">Параметр c</param>
			/// <param name="x1">Корень уравнения x1</param>
			/// <param name="x2">Корень уравнения x2</param>
			/// <returns>
			/// -1 - Корней нет
			/// 0 - Один корень
			/// 1 - Два корня
			/// </returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 SolveQuadraticEquation(Double a, Double b, Double c, out Double x1, out Double x2)
			{
				// Дискриминант
				Double d = b * b - 4 * a * c;

				if (d < 0)
				{
					x1 = 0;
					x2 = 0;
					return -1;
				}
				else
				{
					if (XMath.Approximately(d, 0.0))
					{
						x1 = -b / 2 * a;
						x2 = x1;
						return 0;
					}
					else
					{
						x1 = (-b + d) / 2 * a;
						x2 = (-b - d) / 2 * a;
						return 1;
					}
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================