//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема хранения состояний
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternMemento.cs
*		Интерфейс для представления состояния с возможностью отмены/повторения действия.
*		Определение концепции интерфейса для возможности не нарушая инкапсуляцию, зафиксировать и сохранить внутреннее
*	состояние объекта так, чтобы позднее восстановить его в это состояние с возможностью отмены/повторения действия.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CorePatternMemento Подсистема хранения состояний
		//! Подсистема хранения состояний обеспечивает сохранения состояния объекта/системы и возможность его последующего
		//! восстановления. Является основой для шаблона проектирования повторении и отмены действия.
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для представления состояния с возможностью отмены/повторения действия
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXStateMemento : ICubeXState
		{
			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Undo();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Повторение последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Redo();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс менеджера для отмены/повторения действия
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXMementoManager
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Возможность отмены действия
			/// </summary>
			Boolean CanUndo { get; }

			/// <summary>
			/// Возможность повтора действия
			/// </summary>
			Boolean CanRedo { get; }

			/// <summary>
			/// Доступность менеджера
			/// </summary>
			Boolean IsEnabled { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Undo();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Повторение последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Redo();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление нового состояния элемента в историю действий
			/// </summary>
			/// <remarks>
			/// Вызывается клиентом после выполнения некоторых действий
			/// </remarks>
			/// <param name="state">Состояние объекта</param>
			//---------------------------------------------------------------------------------------------------------
			void AddStateToHistory(ICubeXStateMemento state);
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Менеджер для отмены/повторения действия
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CMementoManager : ICubeXMementoManager, INotifyPropertyChanged
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsCanUndo = new PropertyChangedEventArgs(nameof(CanUndo));
			protected static readonly PropertyChangedEventArgs PropertyArgsCanRedo = new PropertyChangedEventArgs(nameof(CanRedo));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsEnabled = new PropertyChangedEventArgs(nameof(IsEnabled));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			internal ObservableCollection<ICubeXStateMemento> mHistoryStates;
			internal Int32 mNextUndo;
			internal Boolean mIsEnabled;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// История состояния объектов
			/// </summary>
			public ObservableCollection<ICubeXStateMemento> HistoryStates { get; }

			/// <summary>
			/// Возможность отмены действия
			/// </summary>
			public Boolean CanUndo
			{
				get
				{
					// If the NextUndo pointer is -1, no commands to undo
					if (mNextUndo < 0 || mNextUndo > mHistoryStates.Count - 1) // precaution
					{
						return (false);
					}

					return (true);
				}
			}

			/// <summary>
			/// Возможность повтора действия
			/// </summary>
			public Boolean CanRedo
			{
				get
				{
					// If the NextUndo pointer points to the last item, no commands to redo
					if (mNextUndo == mHistoryStates.Count - 1)
					{
						return (false);
					}

					return (true);
				}
			}

			/// <summary>
			/// Возможность повтора действия
			/// </summary>
			public Boolean IsEnabled
			{
				get
				{
					return (mIsEnabled);
				}
				set
				{
					mIsEnabled = value;
					NotifyPropertyChanged(PropertyArgsIsEnabled);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CMementoManager()
			{
				mHistoryStates = new ObservableCollection<ICubeXStateMemento>();
				mNextUndo = -1;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка списка истории действий
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ClearHistory()
			{
				mHistoryStates.Clear();
				mNextUndo = -1;
				NotifyPropertyChanged(PropertyArgsCanUndo);
				NotifyPropertyChanged(PropertyArgsCanRedo);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление нового состояния элемента в историю действий
			/// </summary>
			/// <remarks>
			/// Вызывается клиентом после выполнения некоторых действий
			/// </remarks>
			/// <param name="state">Состояние объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddStateToHistory(ICubeXStateMemento state)
			{
				// Purge history list
				TrimHistoryList();

				// Add command and increment undo counter
				mHistoryStates.Add(state);

				mNextUndo++;

				NotifyPropertyChanged(PropertyArgsCanUndo);
				NotifyPropertyChanged(PropertyArgsCanRedo);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отмена последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Undo()
			{
				if (!CanUndo)
				{
					return;
				}

				// Get the Command object to be undone
				ICubeXStateMemento state = mHistoryStates[mNextUndo];

				// Execute the Command object's undo method
				state.Undo();

				// Move the pointer up one item
				mNextUndo--;

				//XManager.Presenter.Update();

				// Обновляем свойства
				NotifyPropertyChanged(PropertyArgsCanUndo);
				NotifyPropertyChanged(PropertyArgsCanRedo);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Повторение последнего действия
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Redo()
			{
				if (!CanRedo)
				{
					return;
				}

				// Get the Command object to redo
				Int32 item_to_redo = mNextUndo + 1;
				ICubeXStateMemento state = mHistoryStates[item_to_redo];

				// Execute the Command object
				state.Redo();

				// Move the undo pointer down one item
				mNextUndo++;

				//XManager.Presenter.Update();

				// Обновляем свойства
				NotifyPropertyChanged(PropertyArgsCanUndo);
				NotifyPropertyChanged(PropertyArgsCanRedo);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка служебного списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			private void TrimHistoryList()
			{
				// We can redo any undone command until we execute a new 
				// command. The new command takes us off in a new direction,
				// which means we can no longer redo previously undone actions. 
				// So, we purge all undone commands from the history list.*/

				// Exit if no items in History list
				if (mHistoryStates.Count == 0)
				{
					return;
				}

				// Exit if NextUndo points to last item on the list
				if (mNextUndo == mHistoryStates.Count - 1)
				{
					return;
				}

				// Purge all items below the NextUndo pointer
				for (Int32 i = mHistoryStates.Count - 1; i > mNextUndo; i--)
				{
					mHistoryStates.RemoveAt(i);
				}
			}
			#endregion

			#region ======================================= ДАННЫЕ INotifyPropertyChanged =============================
			/// <summary>
			/// Событие срабатывает ПОСЛЕ изменения свойства
			/// </summary>
			public event PropertyChangedEventHandler PropertyChanged;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства.
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(String property_name = "")
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(property_name));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства.
			/// </summary>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(PropertyChangedEventArgs args)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, args);
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