﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема консоли
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXConsoleCommon.cs
*		Общие типы и структуры данных подсистема консоли.
*		Подсистема консоли обеспечивает основной механизм отладки и предоставляет возможность просматривать, сортировать
*	и показывать или не показывать все сообщения отладчика, а также используя систему команд и сообщений дополнительно
*	управлять игровым процессом.
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
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup UnityCommonConsole Подсистема консоли
		//! Подсистема консоли для осуществления отладки, отображения сообщений, отправки команд. Подсистема обеспечивает
		//! основной механизм отладки и конфигурирования приложения и предоставляет возможность просматривать, сортировать
		//! и показывать все сообщения приложения, использовать консоль для вызова команд и отправки сообщений для
		//! дополнительного управления игровым процессом.
		//!
		//! ## Возможности/особенности
		//! 1. Показ/скрытие окно консоли, управление списком сообщений (сортировка, очистка)
		//! 2. Исполнение зарегистрированных команд
		//! 3. Отправка сообщений
		//!
		//! ## Использование
		//! 1. Диспетчер консоли можно использовать в ручную(непосредственно вызывать его методы в нужных местах) или 
		//! посредством \ref CubeX.Common.CubeXSystemDispatcher
		//! \ingroup UnityCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения информации о сообщении переданного через консоль
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CMessageConsole : CMessageArgs
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание сообщения из строки ввода
			/// </summary>
			/// <param name="input_string">Строка ввода</param>
			/// <returns>Сформированное сообщение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CMessageConsole CreateFromInputString(String input_string)
			{
				return new CMessageConsole(input_string);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основное параметры
			internal String mArgument;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Аргумент сообщения
			/// </summary>
			public String Argument
			{
				get { return mArgument; }
				set { mArgument = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CMessageConsole()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public CMessageConsole(String name)
				: base(name)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя сообщения</param>
			/// <param name="argument">Аргумент сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public CMessageConsole(String name, String argument)
				: base(name)
			{
				mArgument = argument;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================