﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема взаимодействия модели с UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternInteractionUIExploreModel.cs
*		Определение интерфейсов элементов управления и моделей для взаимодействия.
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
		//! \defgroup CorePatternInteractionUI Подсистема взаимодействия модели с UI
		//! Подсистема взаимодействия модели с UI обеспечивает связь и возможность двустороннего управления между моделью 
		//! и соответствующим элементом интерфейса.
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс модели для взаимодействия с элементом управления который управляет данной моделью
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXUIModelSupportExplore
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Возможность перемещать модель
			/// </summary>
			Boolean IsDraggableModel { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================		
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс элемента управления для обозрения и управления моделью
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXUIExploreModel
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Выбранная коллекция модели
			/// </summary>
			ICubeXCollectionModel SelectedCollection { get; }

			/// <summary>
			/// Отображаемая коллекция модели которая поддерживает концепцию просмотра и управления
			/// </summary>
			ICubeXCollectionModelView PresentedCollection { get; }

			/// <summary>
			/// Выбранная модель
			/// </summary>
			ICubeXModelOwned SelectedModel { get; }
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