﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема текстовых данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTextLine.cs
*		Строка текстовых данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleText
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Строка текстовых данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTextLine : CTextStr, IComparable<CTextLine>, ICubeXIndexable, ICubeXDuplicate<CTextLine>
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mLabel;
			protected internal Int32 mIndex;
			protected internal CTextList mOwned;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Метка строки
			/// </summary>
			/// <remarks>
			/// Метка служит для дополнительной идентификации строки
			/// </remarks>
			public String Label
			{
				get { return (mLabel); }
				set
				{
					mLabel = value;
				}
			}

			/// <summary>
			/// Индекс строки в списке строк
			/// </summary>
			public Int32 Index
			{
				get { return (mIndex); }
				set
				{
					mIndex = value;
				}
			}

			/// <summary>
			/// Список строк текстовых данных - Владелец данной строки
			/// </summary>
			public CTextList Owned
			{
				get { return (mOwned); }
				set
				{
					mOwned = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTextLine()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="str">Строка</param>
			//---------------------------------------------------------------------------------------------------------
			public CTextLine(String str)
			{
				mRawString = str;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="prefix">Начальный префикс строки</param>
			/// <param name="symbol">Символ для заполнения</param>
			/// <param name="total_length">Общая требуемая длина строки</param>
			//---------------------------------------------------------------------------------------------------------
			public CTextLine(String prefix, Char symbol, Int32 total_length)
			{
				mRawString = prefix + new String(symbol, total_length - prefix.Length);
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение строк текстовых данных для упорядочивания
			/// </summary>
			/// <param name="other">Строка</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CTextLine other)
			{
				return (mRawString.CompareTo(other.mRawString));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дубликата объекта
			/// </summary>
			/// <returns>Дубликат объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public CTextLine Duplicate()
			{
				return (new CTextLine(this.RawString));
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ =================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сложение строк
			/// </summary>
			/// <param name="left">Первая строка текстовых данных</param>
			/// <param name="right">Вторая строка текстовых данных</param>
			/// <returns>Объединённая строка текстовых данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CTextLine operator +(CTextLine left, CTextLine right)
			{
				return new CTextLine(left.mRawString + right.mRawString);
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ ПРЕОБРАЗОВАНИЯ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа String
			/// </summary>
			/// <param name="text_line">Строка текстовых данных</param>
			/// <returns>Строка</returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator String(CTextLine text_line)
			{
				return (text_line.RawString);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа <see cref="CTextLine"/>
			/// </summary>
			/// <param name="str">Строка</param>
			/// <returns>Строка текстовых данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator CTextLine(String str)
			{
				return (new CTextLine(str));
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================