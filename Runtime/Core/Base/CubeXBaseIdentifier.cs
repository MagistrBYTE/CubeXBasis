﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseIdentifier.cs
*		Работа с идентификаторами.
*		Определение типовых правил для работы с идентификаторами в части их функционального использования и наименования.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий работу по генерации уникальных значений
		/// </summary>
		/// <remarks>
		/// Также класс определяет типовые идентификаторы и правила их использования, а также наиболее распространённые
		/// строковые литералы 
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XIdentifier
		{
			#region ======================================= ЧИСЛОВЫЕ ИДЕНТИФИКАТОРЫ ===================================
			/// <summary>
			/// Числовой индекс
			/// </summary>
			/// <remarks>
			/// Обозначает последовательный номер элемент в коллекции/списке и т.п.
			/// Должен быть однозначно сопоставим с <see cref="ICubeXIndexable"/>
			/// Должен быть уникальным в пределах коллекции
			/// </remarks>
			public const String Index = nameof(Index);

			/// <summary>
			/// Числовой идентификатор (или код)
			/// </summary>
			/// <remarks>
			/// <para>
			/// Обозначает условный числовой идентификатор или индекс (не обязательно последовательный).
			/// Должен быть уникальным в пределах локального контекста
			/// </para>
			/// <para>
			/// Для таблиц базы данных это как правило основной первичный ключ
			/// </para>
			/// </remarks>
			public const String ID = nameof(ID);

			/// <summary>
			/// Глобальный уникальный числовой идентификатор
			/// </summary>
			/// <remarks>
			/// Должен быть уникальным в пределах глобального контекста
			/// </remarks>
			public const String UID = nameof(UID);
			#endregion

			#region ======================================= СТРОКОВЫЕ ИДЕНТИФИКАТОРЫ ==================================
			/// <summary>
			/// Строковый идентификатор
			/// </summary>
			/// <remarks>
			/// Как правило должен быть на английском языке для возможности применения в коде.
			/// Должен быть уникальным в пределах локального контекста
			/// </remarks>
			public const String KeyID = nameof(KeyID);

			/// <summary>
			/// Глобальный строковый идентификатор
			/// </summary>
			/// <remarks>
			/// Как правило должен быть на английском языке для возможности применения в коде.
			/// Должен быть уникальным в пределах глобального контекста
			/// </remarks>
			public const String UniqueID = nameof(UniqueID);

			/// <summary>
			/// Строковый идентификатор (код)
			/// </summary>
			/// <remarks>
			/// Может быть на любом языке, должен иметь возможность внешней верификации
			/// </remarks>
			public const String CodeID = nameof(CodeID);

			/// <summary>
			/// Строковый идентификатор (обозначение)
			/// </summary>
			/// <remarks>
			/// Может быть на любом языке, должен иметь возможность внешней верификации
			/// </remarks>
			public const String SymbolID = nameof(SymbolID);
			#endregion

			#region ======================================= ТИПОВЫЕ СТРОКОВЫЕ ЛИТЕРАЛЫ ================================
			/// <summary>
			/// Строковый идентификатор - Имя
			/// </summary>
			public const String Name = "Name";

			/// <summary>
			/// Строковый идентификатор - Значение
			/// </summary>
			public const String Value = "Value";
			#endregion

			#region ======================================= МЕТОДЫ ГЕНЕРАЦИИ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Генерация числового идентификатора указанного объекта
			/// </summary>
			/// <param name="obj">Объект</param>
			/// <returns>Числовой идентификатор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 GenerateID(System.Object obj)
			{
				if(obj == null)
				{
					return (-1);
				}
				else
				{
					return (obj.GetHashCode());
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