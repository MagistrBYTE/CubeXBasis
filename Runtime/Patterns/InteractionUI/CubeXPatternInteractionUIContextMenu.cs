//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема взаимодействия модели с UI
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternInteractionUIContextMenu.cs
*		Определение интерфейса модели, управление которой осуществляется посредством контекстного меню.
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
		/// Интерфейс модели, управление которой осуществляется посредством контекстного меню
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXUIModelContextMenu
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Контекстное меню для управление моделью
			/// </summary>
			CUIContextMenu UIContextMenu { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Открытие контекстного меню
			/// </summary>
			/// <param name="context_menu">Контекстное меню</param>
			//---------------------------------------------------------------------------------------------------------
			void OpenContextMenu(System.Windows.Controls.ContextMenu context_menu);
#endif
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс инкапсулирующий элемент контекстного меню для управления моделью
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CUIContextMenuItem : ICubeXDuplicate<CUIContextMenuItem>
		{
			#region ======================================= ДАННЫЕ ====================================================
			public ICubeXModel Model;
			public Action<ICubeXModel> OnAction;
			public Action<ICubeXModel> OnAfterAction;
#if USE_WINDOWS
			public System.Windows.Controls.MenuItem MenuItem;
#endif
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem(ICubeXModel model)
				: this(model, String.Empty, null, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="name">Имя элемента меню</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem(ICubeXModel model, String name)
				: this(model, name, null, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик событие основного действия</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem(String name, Action<ICubeXModel> on_action)
				: this(null, name, on_action, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик событие основного действия</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem(ICubeXModel model, String name, Action<ICubeXModel> on_action)
				: this(model, name, on_action, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик событие основного действия</param>
			/// <param name="on_after_action">Дополнительный обработчик события после основного действия</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem(ICubeXModel model, String name, Action<ICubeXModel> on_action, 
				Action<ICubeXModel> on_after_action)
			{
				Model = model;
				OnAction = on_action;
				OnAfterAction = on_after_action;

#if USE_WINDOWS
				CreateMenuItem(name, null);
#endif
			}

#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик событие основного действия</param>
			/// <param name="icon">Графическая иконка</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem(String name, Action<ICubeXModel> on_action, System.Drawing.Bitmap icon)
			{
				OnAction = on_action;
				CreateMenuItem(name, icon);
			}
#endif
			#endregion

			#region ======================================= МЕТОДЫ ICubeXDuplicate ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дубликата объекта
			/// </summary>
			/// <returns>Дубликат объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenuItem Duplicate()
			{
				CUIContextMenuItem item = new CUIContextMenuItem();
				item.Model = Model;
				item.OnAction = OnAction;
				item.OnAfterAction = OnAfterAction;
#if USE_WINDOWS
				item.CreateMenuItem(MenuItem);
#endif
				return (item);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента меню
			/// </summary>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="icon">Графическая иконка</param>
			//---------------------------------------------------------------------------------------------------------
			public void CreateMenuItem(String name, System.Drawing.Bitmap icon)
			{
				if (MenuItem == null)
				{
					MenuItem = new System.Windows.Controls.MenuItem();
					MenuItem.Header = name;
					MenuItem.Click += OnItemClick;
					if (icon != null)
					{
						MenuItem.Icon = new System.Windows.Controls.Image
						{
							Source = icon.ToBitmapSource(),
							Width = 16,
							Height = 16
						};
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента меню
			/// </summary>
			/// <param name="menu_item">Элемента меню</param>
			//---------------------------------------------------------------------------------------------------------
			public void CreateMenuItem(System.Windows.Controls.MenuItem menu_item)
			{
				if (MenuItem == null)
				{
					MenuItem = new System.Windows.Controls.MenuItem();
					MenuItem.Header = menu_item.Header;
					MenuItem.Icon = menu_item.Icon;
					MenuItem.Click += OnItemClick;
				}
				else
				{
					MenuItem.Header = menu_item.Header;
					MenuItem.Icon = menu_item.Icon;
				}
			}
#endif
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ =======================================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события щелчка на элементе меню
			/// </summary>
			/// <param name="sender">Источник события</param>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnItemClick(Object sender, System.Windows.RoutedEventArgs args)
			{
				if(OnAction != null)
				{
					OnAction(Model);
				}
				if (OnAfterAction != null)
				{
					OnAfterAction(Model);
				}
			}
#endif
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс инкапсулирующий контекстное меню для управления моделью
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CUIContextMenu
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
#if USE_WINDOWS
			/// <summary>
			/// Элемент меню - загрузить модель из файла
			/// </summary>
			public readonly static CUIContextMenuItem Load = new CUIContextMenuItem("Загрузить...", 
				OnLoadItemClick, XResources.Oxygen_document_open_32);

			/// <summary>
			/// Элемент меню - сохранить модель в файл
			/// </summary>
			public readonly static CUIContextMenuItem Save = new CUIContextMenuItem("Сохранить...", 
				OnSaveItemClick, XResources.Oxygen_document_save_32);

			/// <summary>
			/// Элемент меню - удалить модель
			/// </summary>
			public readonly static CUIContextMenuItem Remove = new CUIContextMenuItem("Удалить", 
				OnRemoveItemClick, XResources.Oxygen_list_remove_32);

			/// <summary>
			/// Элемент меню - дублировать модель
			/// </summary>
			public readonly static CUIContextMenuItem Duplicate = new CUIContextMenuItem("Дублировать", 
				OnDuplicateItemClick, XResources.Oxygen_tab_duplicate_32);

			/// <summary>
			/// Элемент меню - переместить модель вверх
			/// </summary>
			public readonly static CUIContextMenuItem MoveUp = new CUIContextMenuItem("Переместить вверх", 
				OnMoveUpItemClick, XResources.Oxygen_arrow_up_22);

			/// <summary>
			/// Элемент меню - переместить модель вниз
			/// </summary>
			public readonly static CUIContextMenuItem MoveDown = new CUIContextMenuItem("Переместить вниз", 
				OnMoveDownItemClick, XResources.Oxygen_arrow_down_22);

			/// <summary>
			/// Элемент меню - переместить модель вниз
			/// </summary>
			public readonly static CUIContextMenuItem NotCalculation = new CUIContextMenuItem("Не учитывать в расчетах",
				OnNotCalculationItemClick, XResources.Oxygen_user_busy_32);
#else
			/// <summary>
			/// Элемент меню - загрузить модель из файла
			/// </summary>
			public readonly static CUIContextMenuItem Load = new CUIContextMenuItem("Загрузить...", OnLoadItemClick);

			/// <summary>
			/// Элемент меню - сохранить модель в файл
			/// </summary>
			public readonly static CUIContextMenuItem Save = new CUIContextMenuItem("Сохранить...", OnSaveItemClick);

			/// <summary>
			/// Элемент меню - удалить модель
			/// </summary>
			public readonly static CUIContextMenuItem Remove = new CUIContextMenuItem("Удалить", OnRemoveItemClick);

			/// <summary>
			/// Элемент меню - дублировать модель
			/// </summary>
			public readonly static CUIContextMenuItem Duplicate = new CUIContextMenuItem("Дублировать", OnDuplicateItemClick);

			/// <summary>
			/// Элемент меню - переместить модель вверх
			/// </summary>
			public readonly static CUIContextMenuItem MoveUp = new CUIContextMenuItem("Переместить вверх", OnMoveUpItemClick);

			/// <summary>
			/// Элемент меню - переместить модель вниз
			/// </summary>
			public readonly static CUIContextMenuItem MoveDown = new CUIContextMenuItem("Переместить вниз", OnMoveDownItemClick);

			/// <summary>
			/// Элемент меню - переместить модель вниз
			/// </summary>
			public readonly static CUIContextMenuItem NotCalculation = new CUIContextMenuItem("Не учитывать в расчетах", OnNotCalculationItemClick);
#endif
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события загрузка модели из файла
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnLoadItemClick(ICubeXModel model)
			{
				if (model != null)
				{
					// Если модель поддерживает интерфейс документа
					if (model is ICubeXDocument document)
					{
						document.LoadDocument();
					}
					else
					{
						String file_name = XFileDialog.Open();
						if (file_name.IsExists())
						{
							// Уведомляем о начале загрузки
							if (model is ICubeXBeforeLoad before_load)
							{
								before_load.OnBeforeLoad();
							}

							XSerializationDispatcher.UpdateFrom(model, file_name);

							if (model is ICubeXCollectionModel collection_model)
							{
								collection_model.UpdateOwnerLink();
							}

							// Уведомляем об окончании загрузки
							if (model is ICubeXAfterLoad after_load)
							{
								after_load.OnAfterLoad();
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события сохранения модели в файл
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnSaveItemClick(ICubeXModel model)
			{
				if (model != null)
				{
					// Если модель поддерживает интерфейс документа
					if (model is ICubeXDocument document)
					{
						document.SaveDocument();
					}
					else
					{
						String file_name = XFileDialog.Save();
						if (file_name.IsExists())
						{
							// Уведомляем о начале сохранения 
							if (model is ICubeXBeforeSave before_save)
							{
								before_save.OnBeforeSave();
							}

							XSerializationDispatcher.SaveTo(file_name, model);

							// Уведомляем об окончании сохранения 
							if (model is ICubeXAfterSave after_save)
							{
								after_save.OnAfterSave();
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события удаления модели
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnRemoveItemClick(ICubeXModel model)
			{
				if(model is ICubeXModelOwned owned)
				{
					owned.RemoveModel();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события дублирование модели
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnDuplicateItemClick(ICubeXModel model)
			{
				if (model is ICubeXModelOwned owned)
				{
					owned.DuplicateModel();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события перемещение модели вверх
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnMoveUpItemClick(ICubeXModel model)
			{
				if (model is ICubeXModelOwned owned)
				{
					owned.MoveUpModel();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события перемещение модели вниз
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnMoveDownItemClick(ICubeXModel model)
			{
				if (model is ICubeXModelOwned owned)
				{
					owned.MoveDownModel();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработка события смены статуса модели для учитывания в расчетах
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			private static void OnNotCalculationItemClick(ICubeXModel model)
			{
				if (model is ICubeXNotCalculation calculation)
				{
					calculation.NotCalculation = !calculation.NotCalculation;
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			public ICubeXModel Model;
			public Boolean IsCreatedItems;
			public List<CUIContextMenuItem> Items;
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenu()
				: this(null, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenu(ICubeXModel model)
				: this(model, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="items">Набор элементов меню</param>
			//---------------------------------------------------------------------------------------------------------
			public CUIContextMenu(ICubeXModel model, params CUIContextMenuItem[] items)
			{
				Model = model;
				if (items != null && items.Length > 0)
				{
					Items = new List<CUIContextMenuItem>(items);
				}
				else
				{
					Items = new List<CUIContextMenuItem>();
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента меню
			/// </summary>
			/// <param name="menu_item">Элемент меню</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(CUIContextMenuItem menu_item)
			{
				Items.Add(menu_item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элементов меню
			/// </summary>
			/// <param name="items">Набор элементов меню</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(params CUIContextMenuItem[] items)
			{
				Items.AddRange(items);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента меню
			/// </summary>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик событие основного действия</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(String name, Action<ICubeXModel> on_action)
			{
				Items.Add(new CUIContextMenuItem(name, on_action));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента меню
			/// </summary>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик событие основного действия</param>
			/// <param name="on_after_action">Дополнительный обработчик события после основного действия</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(String name, Action<ICubeXModel> on_action, Action<ICubeXModel> on_after_action)
			{
				Items.Add(new CUIContextMenuItem(null, name, on_action, on_after_action));
			}

#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление элемента меню
			/// </summary>
			/// <param name="name">Имя элемента меню</param>
			/// <param name="on_action">Обработчик события элемента меню</param>
			/// <param name="icon">Иконка элемента меню</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddItem(String name, Action<ICubeXModel> on_action, System.Drawing.Bitmap icon)
			{
				Items.Add(new CUIContextMenuItem(name, on_action, icon));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка команд для контекстного меню по умолчанию
			/// </summary>
			/// <param name="context_menu">Контекстное меню</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetCommandsDefault(System.Windows.Controls.ContextMenu context_menu)
			{
				if (Model == null) return;

				// Устанавливаем/обновляем модель
				if (IsCreatedItems == false)
				{
					for (Int32 i = 0; i < Items.Count; i++)
					{
						Items[i].Model = Model;
						context_menu.Items.Add(Items[i].MenuItem);
					}

					IsCreatedItems = true;
				}
				else
				{
					// Устанавливаем/обновляем модель
					for (Int32 i = 0; i < Items.Count; i++)
					{
						Items[i].Model = Model;
					}
				}
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