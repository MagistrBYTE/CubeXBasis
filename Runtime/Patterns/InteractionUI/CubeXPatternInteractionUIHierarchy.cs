//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема взаимодействия модели с UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternInteractionUIHierarchy.cs
*		Определение шаблонов иерархической модели данных реализующих взаимодействия с пользовательским интерфейсом.
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
		/// Шаблон реализующий начальный механизм иерархической модели для взаимодействия с пользовательским интерфейсом
		/// </summary>
		/// <typeparam name="TModel">Соответствующий тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyBeginUI<TModel> : ModelHierarchyBegin<TModel>, ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu
			where TModel : ICubeXModelHierarchy
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
			public ModelHierarchyBeginUI()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyBeginUI(String name)
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
		/// <summary>
		/// Шаблон реализующий механизм иерархической модели для взаимодействия с пользовательским интерфейсом
		/// </summary>
		/// <typeparam name="TModel">Соответствующий тип модели</typeparam>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyUI<TModel, TCollectionModel> : ModelHierarchy<TModel, TCollectionModel>, ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu
			where TCollectionModel : class, ICubeXCollectionModel
			where TModel : ICubeXModelHierarchy
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
			public ModelHierarchyUI()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyUI(String name)
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
		/// <summary>
		/// Шаблон реализующий конечный механизм иерархической модели для взаимодействия с пользовательским интерфейсом
		/// </summary>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyLastUI<TCollectionModel> : ModelHierarchyLast<TCollectionModel>, ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu
			where TCollectionModel : class, ICubeXCollectionModel
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
			public ModelHierarchyLastUI()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyLastUI(String name)
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