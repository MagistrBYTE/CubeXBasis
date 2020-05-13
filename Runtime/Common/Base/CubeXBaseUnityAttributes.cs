﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Базовая подсистема Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseUnityAttributes.cs
*		Определение общих атрибутов применяемых на уровне Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup UnityCommonBase Базовая подсистема
		//! Базовая подсистема расширяет стандартные возможности и унифицирует работу с общим структурами и данными в Unity.
		//! \ingroup UnityCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения порядка исполнения скрипта
		/// </summary>
		/// <remarks>
		/// Используется что бы автоматически сформировать нужный порядок исполнения скриптов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXExecutionOrderAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Int32 mExecutionOrder = -1;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Порядок исполнения скрипта
			/// </summary>
			public Int32 ExecutionOrder
			{
				get { return mExecutionOrder; }
				set { mExecutionOrder = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="execution_order">Порядок исполнения скрипта</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXExecutionOrderAttribute(Int32 execution_order)
			{
				mExecutionOrder = execution_order;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================