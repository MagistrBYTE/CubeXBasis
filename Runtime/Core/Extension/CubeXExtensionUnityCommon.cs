//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXExtensionUnityCommon.cs
*		Методы расширения функциональности базовых классов и структурных типов Unity.
*		Реализация методов расширения функциональности для структурных типов Unity и базовых классов которые
*	будут приспособлены к взаимодействию для работы с типами Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleExtension
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Color"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityColor
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация цветового значения в строку
			/// </summary>
			/// <param name="this">Цветовое значение</param>
			/// <returns>Строка</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Color @this)
			{
				return String.Format("{0},{1},{2},{3}", (Int32)(@this.r * 255.0f), (Int32)(@this.g * 255.0f),
					(Int32)(@this.b * 255.0f), (Int32)(@this.a * 255.0f));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к цвету в формате RGBA
			/// </summary>
			/// <param name="this">Цветовое значение</param>
			/// <returns>Цвет в формате RGBA</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 ToRGBA(this Color @this)
			{
				Int32 red = @this.r >= 1.0f ? 0xff :
							   @this.r <= 0.0f ? 0x00 : (Int32)(@this.r * 255.0f + 0.5f);
				Int32 green = @this.g >= 1.0f ? 0xff :
							   @this.g <= 0.0f ? 0x00 : (Int32)(@this.g * 255.0f + 0.5f);
				Int32 blue = @this.b >= 1.0f ? 0xff :
							   @this.b <= 0.0f ? 0x00 : (Int32)(@this.b * 255.0f + 0.5f);
				Int32 alpha = @this.a >= 1.0f ? 0xff :
							   @this.a <= 0.0f ? 0x00 : (Int32)(@this.a * 255.0f + 0.5f);

				return (Int32)((red << 24) | (green << 16) | (blue << 8) | alpha);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к 32-битному цветовому значению
			/// </summary>
			/// <param name="this">Цветовое значение</param>
			/// <returns>32-битное цветовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color32 ToColor32(this Color @this)
			{
				const Single factor = 255.0f;
				Color32 color = new Color32();
				color.r = (Byte)(@this.r * factor);
				color.g = (Byte)(@this.g * factor);
				color.b = (Byte)(@this.b * factor);
				color.a = (Byte)(@this.a * factor);

				return color;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к 32-битному цветовому значению
			/// </summary>
			/// <param name="this">Цветовое значение</param>
			/// <returns>32-битное цветовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TColor ToTColor(this Color @this)
			{
				const Single factor = 255.0f;
				TColor color = new TColor();
				color.R = (Byte)(@this.r * factor);
				color.G = (Byte)(@this.g * factor);
				color.B = (Byte)(@this.b * factor);
				color.A = (Byte)(@this.a * factor);

				return color;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства цветовых значений
			/// </summary>
			/// <param name="this">Первое значение</param>
			/// <param name="color">Второе значение</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Approximately(this Color @this, Color color, Single epsilon)
			{
				if (Mathf.Abs(@this.r - color.r) < epsilon &&
					Mathf.Abs(@this.g - color.g) < epsilon &&
					Mathf.Abs(@this.b - color.b) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Аппроксимация равенства цветовых значений с учетом альфа канала
			/// </summary>
			/// <param name="this">Первое значение</param>
			/// <param name="color">Второе значение</param>
			/// <param name="epsilon">Погрешность</param>
			/// <returns>Статус равенства значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean ApproximatelyWithAlpha(this Color @this, Color color, Single epsilon)
			{
				if (Mathf.Abs(@this.r - color.r) < epsilon &&
					Mathf.Abs(@this.g - color.g) < epsilon &&
					Mathf.Abs(@this.b - color.b) < epsilon &&
					Mathf.Abs(@this.a - color.a) < epsilon)
				{
					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение инвертированного цвета без учета альфа канала
			/// </summary>
			/// <param name="this">Цвет</param>
			/// <returns>Инвертированный цвет</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color Inverted(this Color @this)
			{
				var result = Color.white - @this;
				result.a = @this.a;
				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение цвети с измененной компонентой красного канала
			/// </summary>
			/// <param name="this">Цвет</param>
			/// <param name="red">Новое значение красной компоненты</param>
			/// <returns>Модифицированный цвет</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color WithRed(this Color @this, Single red)
			{
				return new Color(red, @this.g, @this.b, @this.a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение цвети с измененной компонентой зеленого канала
			/// </summary>
			/// <param name="this">Цвет</param>
			/// <param name="green">Новое значение зеленой компоненты</param>
			/// <returns>Модифицированный цвет</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color WithGreen(this Color @this, Single green)
			{
				return new Color(@this.r, green, @this.b, @this.a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение цвети с измененной компонентой синего канала
			/// </summary>
			/// <param name="this">Цвет</param>
			/// <param name="blue">Новое значение синий компоненты</param>
			/// <returns>Модифицированный цвет</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color WithBlue(this Color @this, Single blue)
			{
				return new Color(@this.r, @this.g, blue, @this.a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение цвети с измененной компонентой альфа канала
			/// </summary>
			/// <param name="this">Цвет</param>
			/// <param name="alpha">Новое значение альфа компоненты</param>
			/// <returns>Модифицированный цвет</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color WithAlpha(this Color @this, Single alpha)
			{
				return new Color(@this.r, @this.g, @this.b, alpha);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Color32"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityColor32
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к цвету в формате RGBA
			/// </summary>
			/// <param name="this">32-битное цветовое значение</param>
			/// <returns>Цвет в формате RGBA</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 ToRGBA(this Color32 @this)
			{
				return (Int32)((@this.r << 24) | (@this.g << 16) | (@this.b << 8) | @this.a);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к цветовому значению
			/// </summary>
			/// <param name="this">32-битное цветовое значение</param>
			/// <returns>Цветовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Color ToColor(this Color32 @this)
			{
				const Single factor = 1.0f / 255.0f;
				Color color = new Color();
				color.r = (Single)@this.r * factor;
				color.g = (Single)@this.g * factor;
				color.b = (Single)@this.b * factor;
				color.a = (Single)@this.a * factor;

				return color;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация цветового значения в строку
			/// </summary>
			/// <param name="this">Цветовое значение</param>
			/// <returns>Строка</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Color32 @this)
			{
				return String.Format("{0},{1},{2},{3}", @this.r, @this.g, @this.b, @this.a);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Rect"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityRect
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация прямоугольника в строку
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Rect @this)
			{
				return String.Format("{0};{1};{2};{3}", @this.x, @this.y, @this.width, @this.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника полученного в результате пересечения
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="rect_other">Прямоугольник</param>
			/// <returns>Прямоугольник полученный в результате пересечения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect Intersect(this Rect @this, Rect rect_other)
			{
				return XUnityRect.IntersectRect(rect_other, @this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника полученного в результате объединения
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="rect_other">Прямоугольник</param>
			/// <returns>Прямоугольник полученный в результате объединения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect Union(this Rect @this, Rect rect_other)
			{
				return XUnityRect.UnionRect(rect_other, @this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Увеличение/уменьшение размеров прямоугольника из центра на заданные величины
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <returns>Прямоугольник полученный в результате увеличения/уменьшения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect Inflate(this Rect @this, Single width, Single height)
			{
				return new Rect(@this.x - width, @this.y - height, @this.width + 2 * width, @this.height + 2 * height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов и возвращение прямоугольника столбца по индексу
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="column_count">Количество столбцов</param>
			/// <param name="column_index">Индекс столбца</param>
			/// <returns>Прямоугольник столбца по индексу</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetColumnFromIndex(this Rect @this, Int32 column_count, Int32 column_index)
			{
				Rect rect = new Rect(@this.x + column_index * @this.width / column_count,
					@this.y, @this.width / column_count, @this.height);

				return rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_left">Дополнительное абсолютное смещение слева</param>
			/// <param name="offset_from_right">Дополнительное абсолютное смещение справа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumns(this Rect @this, Single percent_one, ref Rect rect_one, 
				Single percent_two, ref Rect rect_two, Single offset_from_left, Single offset_from_right)
			{
				Single width = @this.width;

				rect_one.x = @this.x;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов со смещением
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_left">Дополнительное абсолютное смещение слева</param>
			/// <param name="offset_from_right">Дополнительное абсолютное смещение справа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumnsWithOffset(this Rect @this, Single percent_one, ref Rect rect_one,
				Single percent_two, ref Rect rect_two, Single offset_from_left, Single offset_from_right)
			{
				Single width = @this.width - (offset_from_left + offset_from_right);

				rect_one.x = @this.x + offset_from_left;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов со смещением справа
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_right">Дополнительное абсолютное смещение справа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumnsWithOffsetRight(this Rect @this, Single percent_one, ref Rect rect_one,
				Single percent_two, ref Rect rect_two, Single offset_from_right)
			{
				Single width = @this.width - (offset_from_right);

				rect_one.x = @this.x;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов со смещением слева
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_left">Дополнительное абсолютное смещение слева</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumnsWithOffsetLeft(this Rect @this, Single percent_one, ref Rect rect_one,
				Single percent_two, ref Rect rect_two, Single offset_from_left)
			{
				Single width = @this.width - (offset_from_left);

				rect_one.x = @this.x + offset_from_left;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество строк и возвращение прямоугольника строки по индексу
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="row_count">Количество строк</param>
			/// <param name="row_index">Индекс строки</param>
			/// <returns>Прямоугольник строки по индексу</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetRowFromIndex(this Rect @this, Int32 row_count, Int32 row_index)
			{
				Rect rect = new Rect(@this.x, @this.y + row_index * @this.height / row_count, 
					@this.width, @this.height / row_count);

				return rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника смещенного по вертикали и горизонтали
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="offset_x">Смещение по горизонтали</param>
			/// <param name="offset_y">Смещение по вертикали</param>
			/// <returns>Прямоугольник полученный в результате смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetFromOffset(this Rect @this, Single offset_x, Single offset_y)
			{
				return new Rect(@this.x + offset_x, @this.y + offset_y, @this.width, @this.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника смещенного по вертикали
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="offset_y">Смещение по вертикали</param>
			/// <returns>Прямоугольник полученный в результате смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetFromOffsetY(this Rect @this, Single offset_y)
			{
				return new Rect(@this.x, @this.y + offset_y, @this.width, @this.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника смещенного по горизонтали
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="offset_x">Смещение по горизонтали</param>
			/// <returns>Прямоугольник полученный в результате смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect GetFromOffsetX(this Rect @this, Single offset_x)
			{
				return new Rect(@this.x + offset_x, @this.y, @this.width, @this.height);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.RectInt"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityRectInt
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация прямоугольника в строку
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this RectInt @this)
			{
				return String.Format("{0};{1};{2};{3}", @this.x, @this.y, @this.width, @this.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника полученного в результате пересечения
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="rect_other">Прямоугольник</param>
			/// <returns>Прямоугольник полученный в результате пересечения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt Intersect(this RectInt @this, RectInt rect_other)
			{
				return XUnityRectInt.IntersectRect(rect_other, @this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника полученного в результате объединения
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="rect_other">Прямоугольник</param>
			/// <returns>Прямоугольник полученный в результате объединения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt Union(this RectInt @this, RectInt rect_other)
			{
				return XUnityRectInt.UnionRect(rect_other, @this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Увеличение/уменьшение размеров прямоугольника из центра на заданные величины
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="width">Ширина</param>
			/// <param name="height">Высота</param>
			/// <returns>Прямоугольник полученный в результате увеличения/уменьшения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt Inflate(this RectInt @this, Int32 width, Int32 height)
			{
				return new RectInt(@this.x - width, @this.y - height, @this.width + 2 * width, @this.height + 2 * height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов и возвращение прямоугольника столбца по индексу
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="column_count">Количество столбцов</param>
			/// <param name="column_index">Индекс столбца</param>
			/// <returns>Прямоугольник столбца по индексу</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt GetColumnFromIndex(this RectInt @this, Int32 column_count, Int32 column_index)
			{
				RectInt rect = new RectInt(@this.x + column_index * @this.width / column_count,
					@this.y, @this.width / column_count, @this.height);

				return rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_left">Дополнительное абсолютное смещение слева</param>
			/// <param name="offset_from_right">Дополнительное абсолютное смещение справа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumns(this RectInt @this, Int32 percent_one, ref RectInt rect_one,
				Int32 percent_two, ref RectInt rect_two, Int32 offset_from_left, Int32 offset_from_right)
			{
				Int32 width = @this.width;

				rect_one.x = @this.x;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов со смещением
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_left">Дополнительное абсолютное смещение слева</param>
			/// <param name="offset_from_right">Дополнительное абсолютное смещение справа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumnsWithOffset(this RectInt @this, Int32 percent_one, ref RectInt rect_one,
				Int32 percent_two, ref RectInt rect_two, Int32 offset_from_left, Int32 offset_from_right)
			{
				Int32 width = @this.width - (offset_from_left + offset_from_right);

				rect_one.x = @this.x + offset_from_left;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов со смещением справа
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_right">Дополнительное абсолютное смещение справа</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumnsWithOffsetRight(this RectInt @this, Int32 percent_one, ref RectInt rect_one,
				Int32 percent_two, ref RectInt rect_two, Int32 offset_from_right)
			{
				Int32 width = @this.width - (offset_from_right);

				rect_one.x = @this.x;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество столбцов со смещением слева
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="percent_one">Процент ширины первого прямоугольника</param>
			/// <param name="rect_one">Прямоугольник для сохранения результата</param>
			/// <param name="percent_two">Процент ширины второго прямоугольника</param>
			/// <param name="rect_two">Прямоугольник для сохранения результата</param>
			/// <param name="offset_from_left">Дополнительное абсолютное смещение слева</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetColumnsWithOffsetLeft(this RectInt @this, Int32 percent_one, ref RectInt rect_one,
				Int32 percent_two, ref RectInt rect_two, Int32 offset_from_left)
			{
				Int32 width = @this.width - (offset_from_left);

				rect_one.x = @this.x + offset_from_left;
				rect_one.y = @this.y;
				rect_one.width = width * percent_one;
				rect_one.height = @this.height;

				rect_two.x = rect_one.xMax;
				rect_two.y = @this.y;
				rect_two.width = width * percent_two;
				rect_two.height = @this.height;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Деление прямоугольника на указанное количество строк и возвращение прямоугольника строки по индексу
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="row_count">Количество строк</param>
			/// <param name="row_index">Индекс строки</param>
			/// <returns>Прямоугольник строки по индексу</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt GetRowFromIndex(this RectInt @this, Int32 row_count, Int32 row_index)
			{
				RectInt rect = new RectInt(@this.x, @this.y + row_index * @this.height / row_count,
					@this.width, @this.height / row_count);

				return rect;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника смещенного по вертикали и горизонтали
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="offset_x">Смещение по горизонтали</param>
			/// <param name="offset_y">Смещение по вертикали</param>
			/// <returns>Прямоугольник полученный в результате смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt GetFromOffset(this RectInt @this, Int32 offset_x, Int32 offset_y)
			{
				return new RectInt(@this.x + offset_x, @this.y + offset_y, @this.width, @this.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника смещенного по вертикали
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="offset_y">Смещение по вертикали</param>
			/// <returns>Прямоугольник полученный в результате смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt GetFromOffsetY(this RectInt @this, Int32 offset_y)
			{
				return new RectInt(@this.x, @this.y + offset_y, @this.width, @this.height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение прямоугольника смещенного по горизонтали
			/// </summary>
			/// <param name="this">Прямоугольник</param>
			/// <param name="offset_x">Смещение по горизонтали</param>
			/// <returns>Прямоугольник полученный в результате смещения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static RectInt GetFromOffsetX(this RectInt @this, Int32 offset_x)
			{
				return new RectInt(@this.x + offset_x, @this.y, @this.width, @this.height);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.RectOffset"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityRectOffset
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация прямоугольника смещения в строку
			/// </summary>
			/// <param name="this">Прямоугольник смещения</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this RectOffset @this)
			{
				return String.Format("{0};{1};{2};{3}", @this.left, @this.top, @this.right, @this.bottom);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка границ по границам спрайта
			/// </summary>
			/// <param name="this">Прямоугольник смещения</param>
			/// <param name="border">Границы спрайта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetFromBorderSprite(this RectOffset @this, Vector4 border)
			{
				@this.left = (Int32)border.x;
				@this.top = (Int32)border.w;
				@this.right = (Int32)border.z;
				@this.bottom = (Int32)border.y;
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.Bounds"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityBounds
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация прямоугольника AABB в строку
			/// </summary>
			/// <param name="this">Прямоугольник AABB</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this Bounds @this)
			{
				return String.Format("{0};{1};{2};{3};{4};{5}", @this.center.x, @this.center.y, @this.center.z,
					@this.extents.x, @this.extents.y, @this.extents.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение максимальной длины стороны ограничивающего объема
			/// </summary>
			/// <param name="this">Ограничивающий объем</param>
			/// <returns>Максимальная длина стороны</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetMaxExtents(this Bounds @this)
			{
				if (@this.extents.x >= @this.extents.y && @this.extents.x >= @this.extents.z)
				{
					return @this.extents.x;
				}
				else
				{
					if (@this.extents.y >= @this.extents.x && @this.extents.y >= @this.extents.z)
					{
						return @this.extents.y;
					}
					else
					{
						return @this.extents.z;
					}
				}
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.BoundsInt"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityBoundsInt
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация прямоугольника AABB в строку
			/// </summary>
			/// <param name="this">Прямоугольник AABB</param>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String SerializeToString(this BoundsInt @this)
			{
				return String.Format("{0};{1};{2};{3};{4};{5}", @this.x, @this.y, @this.z,
					@this.size.x, @this.size.y, @this.size.z);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение максимальной длины стороны ограничивающего объема
			/// </summary>
			/// <param name="this">Ограничивающий объем</param>
			/// <returns>Максимальная длина стороны</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Single GetMaxExtents(this BoundsInt @this)
			{
				if (@this.max.x >= @this.max.y && @this.max.x >= @this.max.z)
				{
					return @this.max.x;
				}
				else
				{
					if (@this.max.y >= @this.max.x && @this.max.y >= @this.max.z)
					{
						return @this.max.y;
					}
					else
					{
						return @this.max.z;
					}
				}
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="UnityEngine.LayerMask"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionUnityLayerMask
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обращение маски слоев
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <returns>Маска слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LayerMask Inverse(this LayerMask @this)
			{
				return ~@this;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление к маски слоев указанный слоев
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <param name="layer_names">Список имен слоев</param>
			/// <returns>Маска слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LayerMask AddToMask(this LayerMask @this, String[] layer_names)
			{
				return @this | XUnityLayerMask.NamesToMask(layer_names);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление из маски слоев указанный слоев
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <param name="layer_names">Список имен слоев</param>
			/// <returns>Маска слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LayerMask RemoveFromMask(this LayerMask @this, String[] layer_names)
			{
				LayerMask inverted_original = ~@this;
				return ~(inverted_original | XUnityLayerMask.NamesToMask(layer_names));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление из маски слоев указанный слоев
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <param name="layers">Список индексов слоев</param>
			/// <returns>Маска слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static LayerMask RemoveFromMask(this LayerMask @this, Int32[] layers)
			{
				Int32 len = layers.Length;
				var names = new String[len];
				for (Int32 i = 0; i < len; i++)
				{
					names[i] = LayerMask.LayerToName(layers[i]);
				}

				return RemoveFromMask(@this, names);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на вхождение указанного слоя
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <param name="other">Маска слоев</param>
			/// <returns>Статус вхождения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean Contains(this LayerMask @this, LayerMask other)
			{
				// Convert the object's layer to a bitfield for comparison
				Int32 bit_mask = 1 << other.value;
				return (@this.value & bit_mask) > 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование маски слоев к массиву имен слоев
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <returns>Список имен слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String[] MaskToNames(this LayerMask @this)
			{
				var output = new List<String>();

				for (Int32 i = 0; i < 32; ++i)
				{
					Int32 shifted = 1 << i;
					if ((@this & shifted) == shifted)
					{
						String layerName = LayerMask.LayerToName(i);
						if (layerName.IsExists())
						{
							output.Add(layerName);
						}
					}
				}
				return output.ToArray();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование маски слоев к текстовому представлению
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <returns>Строка с именами слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String MaskToString(this LayerMask @this)
			{
				return MaskToString(@this, ", ");
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование маски слоев к текстовому представлению
			/// </summary>
			/// <param name="this">Маска слоев</param>
			/// <param name="delimiter">Разделитель</param>
			/// <returns>Строка с именами слоев</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String MaskToString(this LayerMask @this, String delimiter)
			{
				return String.Join(delimiter, MaskToNames(@this));
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================