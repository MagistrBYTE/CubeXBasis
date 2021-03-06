﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль математической системы
// Подраздел: Подсистема генерации псевдослучайных значений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXMathRandomCommon.cs
*		Общие типы и структуры данных подсистемы получения псевдослучайных значений.
*		Определение типов и общего интерфейса для реализации генератора псевдослучайных значений.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace CubeX
{
	namespace Maths
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup MathRandom Подсистема Random
		//! Подсистема генерации псевдослучайных значений. Подсистема обеспечивает генерацию псевдослучайных значений 
		//! по различным алгоритмам.
		//! \ingroup Math
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение общего интерфейса генератора для получения псевдослучайных значений
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXRandom
		{
			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получиться следующие псевдослучайное число в диапазоне [0 - 1]
			/// </summary>
			/// <returns>Псевдослучайное число</returns>
			//---------------------------------------------------------------------------------------------------------
			Single NextSingle();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получиться следующие псевдослучайное число в диапазоне [0 - max]
			/// </summary>
			/// <param name="max">Максимальное число</param>
			/// <returns>Псевдослучайное число</returns>
			//---------------------------------------------------------------------------------------------------------
			Single NextSingle(Single max);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получиться следующие псевдослучайное число в диапазоне [min - max]
			/// </summary>
			/// <param name="min">Минимальное число</param>
			/// <param name="max">Максимальное число</param>
			/// <returns>Псевдослучайное число</returns>
			//---------------------------------------------------------------------------------------------------------
			Single NextSingle(Single min, Single max);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получиться следующие псевдослучайное число в диапазоне [0 - 4294967295]
			/// </summary>
			/// <returns>Псевдослучайное число</returns>
			//---------------------------------------------------------------------------------------------------------
			UInt32 NextInteger();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получиться следующие псевдослучайное число в диапазоне [0 - max]
			/// </summary>
			/// <param name="max">Максимальное число</param>
			/// <returns>Псевдослучайное число</returns>
			//---------------------------------------------------------------------------------------------------------
			UInt32 NextInteger(UInt32 max);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получиться следующие псевдослучайное число в диапазоне [min - max]
			/// </summary>
			/// <param name="min">Минимальное число</param>
			/// <param name="max">Максимальное число</param>
			/// <returns>Псевдослучайное число</returns>
			//---------------------------------------------------------------------------------------------------------
			UInt32 NextInteger(UInt32 min, UInt32 max);
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================