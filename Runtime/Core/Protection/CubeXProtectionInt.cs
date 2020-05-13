﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема защиты
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXProtectionInt.cs
*		Защита целого числа.
*		Реализация механизма защиты (шифрования/декодирование) целого числа.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Runtime.InteropServices;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleProtection Подсистема защиты
		//! Механизм защиты (шифрования/декодирование) для примитивных типов данных.
		//! Используются простые методы - манипулирование и операции с битами и циклические сдвиги.
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Структура-оболочка для защиты целого числа
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[StructLayout(LayoutKind.Explicit)]
		public struct TProtectionInt
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Маска для шифрования/декодирование
			/// </summary>
			public const UInt32 XOR_MASK = 0XAAAAAAAA;
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			[FieldOffset(0)]
			private Int32 mEncryptValue;

			[FieldOffset(0)]
			private UInt32 mConvertValue;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Зашифрованное значение
			/// </summary>
			public Int32 EncryptedValue
			{
				get
				{
					// Обходное решение для конструктора структуры по умолчанию
					if (mConvertValue == 0 && mEncryptValue == 0)
					{
						mConvertValue = XOR_MASK;
					}

					return mEncryptValue;
				}
			}
			#endregion

			#region ======================================= ОПЕРАТОРЫ ПРЕОБРАЗОВАНИЯ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в обычное целое число
			/// </summary>
			/// <param name="value">Структура-оболочка для защиты целого числа</param>
			/// <returns>Целое число</returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator Int32(TProtectionInt value)
			{
				value.mConvertValue ^= XOR_MASK;
				var original = value.mEncryptValue;
				value.mConvertValue ^= XOR_MASK;
				return original;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Неявное преобразование в объект типа структуры-оболочки для защиты целого числа
			/// </summary>
			/// <param name="value">Целое число</param>
			/// <returns>Структура-оболочка для защиты целого числа</returns>
			//---------------------------------------------------------------------------------------------------------
			public static implicit operator TProtectionInt(Int32 value)
			{
				var protection = new TProtectionInt();
				protection.mEncryptValue = value;
				protection.mConvertValue ^= XOR_MASK;
				return protection;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================