﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема взаимодействия модели с UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternInteractionUIRepository.cs
*		Определение интерфейсов элементов управления взаимодействия с репозиториями данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//---------------------------------------------------------------------------------------------------------------------
#if USE_WINDOWS
using CubeX.Windows;
#endif
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternInteractionUI
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс элемента управления для просмотра и редактирование данных репозитория
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXUIRepositoryDataViewer
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Репозиторий данных для отображения
			/// </summary>
			ICubeXRepository Repository { get; set; }

			/// <summary>
			/// Выражение для фильтра записей
			/// </summary>
			String RowFilter { get; set; }

			/// <summary>
			/// Выбранная запись
			/// </summary>
			System.Object SelectedItem { get; set; }

			/// <summary>
			/// Выбранный индекс записи
			/// </summary>
			Int32 SelectedIndex { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================