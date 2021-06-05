//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема взаимодействия модели с UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternInteractionUIHierarchy.cs
*		Определение шаблонов иерархической модели данных реализующих взаимодействия с пользовательским интерфейсом 
*	с поддержкой концепции просмотра и управления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
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
		/// с поддержкой концепции просмотра и управления
		/// </summary>
		/// <typeparam name="TModel">Соответствующий тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyViewBeginUI<TModel> : ModelHierarchyViewBegin<TModel>, 
			ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu, ICubeXNotCalculation, ICubeXVerified
			where TModel : ICubeXModelHierarchyView
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// Константы для информирования об изменении свойств
			//
			// Расчеты
			protected static readonly PropertyChangedEventArgs PropertyArgsNotCalculation = new PropertyChangedEventArgs(nameof(NotCalculation));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsVerified = new PropertyChangedEventArgs(nameof(IsVerified));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Контекстное меню
			protected internal CUIContextMenu mUIContextMenu;

			// Расчеты
			protected internal Boolean mNotCalculation;
			protected internal Boolean mIsVerified;
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

			#region ======================================= СВОЙСТВА ICubeXNotCalculation =============================
			/// <summary>
			/// Не учитывать набор в расчетах
			/// </summary>
			[DisplayName("Не учитывать")]
			[Description("Не учитывать набор в расчетах")]
			[Category(XGroupDesc.Calculations)]
			[CubeXPropertyOrder(10)]
			[CubeXCategoryOrder(4)]
			[XmlAttribute]
			public Boolean NotCalculation
			{
				get { return (mNotCalculation); }
				set
				{
					mNotCalculation = value;
					NotifyPropertyChanged(PropertyArgsNotCalculation);
					RaiseNotCalculationChanged();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXVerified ===================================
			/// <summary>
			/// Статус верификации объекта
			/// </summary>
			[DisplayName("Статус верификации")]
			[Description("Статус верификации объекта")]
			[Category(XGroupDesc.Calculations)]
			[CubeXPropertyOrder(20)]
			[XmlAttribute]
			public Boolean IsVerified
			{
				get { return (mIsVerified); }
				set
				{
					mIsVerified = value;
					NotifyPropertyChanged(PropertyArgsIsVerified);
					RaiseIsVerifiedChanged();
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewBeginUI()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewBeginUI(String name)
				: base(name)
			{
				mUIContextMenu = new CUIContextMenu(this);
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса расчёта набора в расчётах.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseNotCalculationChanged()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса верификации объекта.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsVerifiedChanged()
			{

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
		/// с поддержкой концепции просмотра и управления
		/// </summary>
		/// <typeparam name="TModel">Соответствующий тип модели</typeparam>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyViewUI<TModel, TCollectionModel> : ModelHierarchyView<TModel, TCollectionModel>, 
			ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu, ICubeXNotCalculation, ICubeXVerified
			where TModel : ICubeXModelHierarchyView
			where TCollectionModel : class, ICubeXCollectionModelView
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// Константы для информирования об изменении свойств
			//
			// Расчеты
			protected static readonly PropertyChangedEventArgs PropertyArgsNotCalculation = new PropertyChangedEventArgs(nameof(NotCalculation));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsVerified = new PropertyChangedEventArgs(nameof(IsVerified));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Контекстное меню
			protected internal CUIContextMenu mUIContextMenu;

			// Расчеты
			protected internal Boolean mNotCalculation;
			protected internal Boolean mIsVerified;
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

			#region ======================================= СВОЙСТВА ICubeXNotCalculation =============================
			/// <summary>
			/// Не учитывать набор в расчетах
			/// </summary>
			[DisplayName("Не учитывать")]
			[Description("Не учитывать набор в расчетах")]
			[Category(XGroupDesc.Calculations)]
			[CubeXPropertyOrder(10)]
			[CubeXCategoryOrder(4)]
			[XmlAttribute]
			public Boolean NotCalculation
			{
				get { return (mNotCalculation); }
				set
				{
					mNotCalculation = value;
					NotifyPropertyChanged(PropertyArgsNotCalculation);
					RaiseNotCalculationChanged();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXVerified ===================================
			/// <summary>
			/// Статус верификации объекта
			/// </summary>
			[DisplayName("Статус верификации")]
			[Description("Статус верификации объекта")]
			[Category(XGroupDesc.Calculations)]
			[CubeXPropertyOrder(20)]
			[XmlAttribute]
			public Boolean IsVerified
			{
				get { return (mIsVerified); }
				set
				{
					mIsVerified = value;
					NotifyPropertyChanged(PropertyArgsIsVerified);
					RaiseIsVerifiedChanged();
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewUI()
			{
				mUIContextMenu = new CUIContextMenu(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewUI(String name)
				: base(name)
			{
				mUIContextMenu = new CUIContextMenu(this);
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса расчёта набора в расчётах.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseNotCalculationChanged()
			{
				
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса верификации объекта.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsVerifiedChanged()
			{

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
		/// с поддержкой концепции просмотра и управления
		/// </summary>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyViewLastUI<TCollectionModel> : ModelHierarchyViewLast<TCollectionModel>, 
			ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu, ICubeXNotCalculation, ICubeXVerified
			where TCollectionModel : class, ICubeXCollectionModelView
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// Константы для информирования об изменении свойств
			//
			// Расчеты
			protected static readonly PropertyChangedEventArgs PropertyArgsNotCalculation = new PropertyChangedEventArgs(nameof(NotCalculation));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsVerified = new PropertyChangedEventArgs(nameof(IsVerified));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Контекстное меню
			protected internal CUIContextMenu mUIContextMenu;

			// Расчеты
			protected internal Boolean mNotCalculation;
			protected internal Boolean mIsVerified;
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

			#region ======================================= СВОЙСТВА ICubeXNotCalculation =============================
			/// <summary>
			/// Не учитывать набор в расчетах
			/// </summary>
			[DisplayName("Не учитывать")]
			[Description("Не учитывать набор в расчетах")]
			[Category(XGroupDesc.Calculations)]
			[CubeXPropertyOrder(10)]
			[CubeXCategoryOrder(4)]
			[XmlAttribute]
			public Boolean NotCalculation
			{
				get { return (mNotCalculation); }
				set
				{
					mNotCalculation = value;
					NotifyPropertyChanged(PropertyArgsNotCalculation);
					RaiseNotCalculationChanged();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXVerified ===================================
			/// <summary>
			/// Статус верификации объекта
			/// </summary>
			[DisplayName("Статус верификации")]
			[Description("Статус верификации объекта")]
			[Category(XGroupDesc.Calculations)]
			[CubeXPropertyOrder(20)]
			[XmlAttribute]
			public Boolean IsVerified
			{
				get { return (mIsVerified); }
				set
				{
					mIsVerified = value;
					NotifyPropertyChanged(PropertyArgsIsVerified);
					RaiseIsVerifiedChanged();
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewLastUI()
			{
				mUIContextMenu = new CUIContextMenu(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewLastUI(String name)
				: base(name)
			{
				mUIContextMenu = new CUIContextMenu(this);
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса расчёта набора в расчётах.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseNotCalculationChanged()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса верификации объекта.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsVerifiedChanged()
			{

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