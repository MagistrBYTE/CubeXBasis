//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseArray.cs
*		Работа с массивами.
*		Реализация дополнительных методов для работы с массивами.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup Basis Базовый набор
		//! Базовый набор содержащий код развивающий в целом платформу NET.
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModule Модуль базового ядра
		//! Модуль, содержащий код, развивающий в целом платформу NET и реализующий дополнительную базовую функциональность
		//! на уровне среды NET. Большинство реализаций модуля кроссплатформенно. Является основой для построения других 
		//! подсистем и модулей.
		//! Применительно к Unity модуль расширяет функциональность базовых типов, при этом в модуле не содержится никаких 
		//! компонентов для Unity, работа осуществляется через центральные диспетчеры и путем непосредственного использования
		//! объектов в коде. Модуль также содержит расширение функциональности редактора Unity.
		//! Модуль это прежде всего кроссплатформенный набор инструментов для решения повседневных задач: большая библиотека
		//! расширений, реализации коллекций на основе массива, гибкая и универсальная подсистема задач, подсистема сообщений
		//! и многое другое.
		//! \ingroup Basis
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleBase Базовая подсистема
		//! Базовая подсистема содержит функциональность, расширяющие возможности базовых типов платформы NET и Unity. 
		//! Большинство типов и реализаций платформенно-независимы.
		//! Сюда входит: работа с битовыми полями, упаковка данных, поддержка загрузки данных различных форматов, 
		//! универсальный тип который может принимать значения разных типов данных, определение класса для представления цвета, 
		//! определение класса для хранения информации в формате имя-значения и т.д.
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий дополнительные методы для работы с массивом
		/// </summary>
		/// <remarks>
		/// Обратите внимание массив(исходный) переданный в качестве аргумента в методы всегда изменяется.
		/// Все методы возвращают результирующий массив
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XArray
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента в конец массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="item">Элемент</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] Add<TType>(TType[] array, TType item)
			{
				Array.Resize(ref array, array.Length + 1);
				array[array.Length - 1] = item;
				return array;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента в указанную позицию массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="item">Элемент</param>
			/// <param name="index">Позиция(индекс) вставки элемента</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] InsertAt<TType>(TType[] array, TType item, Int32 index)
			{
				TType[] temp = array;
				array = new TType[array.Length + 1];
				Array.Copy(temp, 0, array, 0, index);
				array[index] = item;
				Array.Copy(temp, index, array, index + 1, temp.Length - index);

				return array;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элементов в указанную позицию массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="items">Набор элементов</param>
			/// <param name="index">Позиция(индекс) вставки элементов</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] InsertAt<TType>(TType[] array, Int32 index, params TType[] items)
			{
				TType[] temp = array;
				array = new TType[array.Length + items.Length];
				Array.Copy(temp, 0, array, 0, index);
				Array.Copy(items, 0, array, index, items.Length);
				Array.Copy(temp, index, array, index + items.Length, temp.Length - index);

				return array;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента в начало массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="item">Элемент</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] Push<TType>(TType[] array, TType item)
			{
				return InsertAt(array, item, 0);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элементов массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="start">Начальная позиция удаления</param>
			/// <param name="count">Количество элементов для удаления</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveAt<TType>(TType[] array, Int32 start, Int32 count)
			{
				TType[] temp = array;
				array = new TType[array.Length - count >= 0 ? array.Length - count : 0];
				Array.Copy(temp, array, start);
				Int32 index = start + count;
				if (index < temp.Length)
				{
					Array.Copy(temp, index, array, start, temp.Length - index);
				}

				return array;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента массива по индексу
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="index">Индекс удаляемого элемента</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveAt<TType>(TType[] array, Int32 index)
			{
				return RemoveAt(array, index, 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление диапазона массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="start">Начало удаляемого диапазона</param>
			/// <param name="end">Конец удаляемого диапазона</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveRange<TType>(TType[] array, Int32 start, Int32 end)
			{
				return RemoveAt(array, start, end - start + 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление первого элемента массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveFirst<TType>(TType[] array)
			{
				return RemoveAt(array, 0, 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элементов с начала массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="count">Количество удаляемых элементов</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveFirst<TType>(TType[] array, Int32 count)
			{
				return RemoveAt(array, 0, count);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление последнего элемента массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveLast<TType>(TType[] array)
			{
				return RemoveAt(array, array.Length - 1, 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление последних элементов массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="count">Количество удаляемых элементов</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveLast<TType>(TType[] array, Int32 count)
			{
				return RemoveAt(array, array.Length - count, count);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск и удаление первого вхождения элемента
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="item">Удаляемый элемент</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] Remove<TType>(TType[] array, TType item)
			{
				Int32 index = Array.IndexOf(array, item);
				if (index >= 0)
				{
					return RemoveAt(array, index);
				}

				return array;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех вхождений элемента
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="item">Удаляемый элемент</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] RemoveAll<TType>(TType[] array, TType item)
			{
				Int32 index = 0;
				do
				{
					index = Array.IndexOf(array, item);
					if (index >= 0)
					{
						array = RemoveAt(array, index);
					}
				}
				while (index >= 0 && array.Length > 0);
				return array;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение элементов массива
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="start_index">Индекс элемент с которого начитается смещение</param>
			/// <param name="offset">Количество смещения</param>
			/// <param name="count">Количество смещаемых элементов</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] Shift<TType>(TType[] array, Int32 start_index, Int32 offset, Int32 count)
			{
				TType[] result = (TType[])array.Clone();

				start_index = start_index < 0 ? 0 : (start_index >= result.Length ? result.Length - 1 : start_index);
				count = count < 0 ? 0 : (start_index + count >= result.Length ? result.Length - start_index - 1 : count);
				offset = start_index + offset < 0 ? -start_index : (start_index + count + offset >= result.Length ? result.Length - start_index - count : offset);

				Int32 abs_offset = Math.Abs(offset);
				TType[] items = new TType[count]; // What we want to move
				TType[] dec = new TType[abs_offset]; // What is going to replace the thing we move
				Array.Copy(array, start_index, items, 0, count);
				Array.Copy(array, start_index + (offset >= 0 ? count : offset), dec, 0, abs_offset);
				Array.Copy(dec, 0, result, start_index + (offset >= 0 ? 0 : offset + count), abs_offset);
				Array.Copy(items, 0, result, start_index + offset, count);

				return result;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение элементов массива вправо
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="start_index">Индекс элемент с которого начитается смещение</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] ShiftRight<TType>(TType[] array, Int32 start_index)
			{
				return Shift(array, start_index, 1, 1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смещение элементов массива влево
			/// </summary>
			/// <typeparam name="TType">Тип элемента массива</typeparam>
			/// <param name="array">Массив</param>
			/// <param name="start_index">Индекс элемент с которого начитается смещение</param>
			/// <returns>Массив</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TType[] ShiftLeft<TType>(TType[] array, Int32 start_index)
			{
				return Shift(array, start_index, -1, 1);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================