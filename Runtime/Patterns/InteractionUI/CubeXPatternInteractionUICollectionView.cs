﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема взаимодействия модели с UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternInteractionUICollectionView.cs
*		Определение шаблонов коллекции моделей, реализующих взаимодействия с пользовательским интерфейсом с поддержкой
*	концепции просмотра и управления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		/// Шаблон коллекции моделей реализующей взаимодействия с пользовательским интерфейсом с поддержкой 
		/// концепции просмотра и управления
		/// </summary>
		/// <typeparam name="TModel">Тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class CollectionModelViewUI<TModel> : CollectionModelView<TModel>, ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu 
			where TModel : ICubeXModelView
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal CUIContextMenu mUIContextMenu;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXUIModelSupportExplore ======================
			/// <summary>
			/// Возможность перемещать модель
			/// </summary>
			[Browsable(false)]
			public virtual Boolean IsDraggableModel
			{
				get { return (false); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXUIModelContextMenu =========================
			/// <summary>
			/// Контекстное меню для управление моделью
			/// </summary>
			[Browsable(false)]
			public CUIContextMenu UIContextMenu
			{
				get { return (mUIContextMenu); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CollectionModelViewUI()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя коллекции</param>
			//---------------------------------------------------------------------------------------------------------
			public CollectionModelViewUI(String name)
				: base(name)
			{
				mUIContextMenu = new CUIContextMenu(this);
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXUIModelContextMenu ===========================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Открытие контекстного меню
			/// </summary>
			/// <param name="context_menu">Контекстное меню</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OpenContextMenu(System.Windows.Controls.ContextMenu context_menu)
			{
				mUIContextMenu.SetCommandsDefault(context_menu);
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================