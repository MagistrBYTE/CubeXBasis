﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXExtensionDelegate.cs
*		Методы расширения работы с делегатами.
*		Реализация максимально обобщенных расширений направленных на работу с делегатами и событиями.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
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
		/// Статический класс реализующий методы расширений с делегатами
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionDelegate
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на наличие обработчик в списке делегатов
			/// </summary>
			/// <param name="this">Делегат</param>
			/// <param name="delegat">Проверяемый делегат</param>
			/// <returns>Статус наличия</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean HasHandler(this Action @this, Action delegat)
			{
				return @this.GetInvocationList().Contains(delegat);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================