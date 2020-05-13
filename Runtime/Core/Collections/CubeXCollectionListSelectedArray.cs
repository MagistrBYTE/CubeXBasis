//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема коллекций
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCollectionListSelectedArray.cs
*		Список с поддержкой концепции выбора элемента на основе массива.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleCollections
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для элемента списка который может быть выбран
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXSelectedItem
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка элемента списка как выбранного
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void SetSelectedItem();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Снятия выбора элемента списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void SetUnselectedItem();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Возможность выбора элемента списка
			/// </summary>
			/// <remarks>
			/// Имеется виду возможность выбора элемента списка в данный момент
			/// </remarks>
			/// <returns>Возможность выбора</returns>
			//---------------------------------------------------------------------------------------------------------
			Boolean CanSelectedItem();
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для коллекции поддерживающий концепцию выбора элемента
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXListSelected
		{
			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Количество элементов списка
			/// </summary>
			Int32 Count { get; }

			/// <summary>
			/// Выбранный индекс элемента списка, -1 выбора нет
			/// </summary>
			/// <remarks>
			/// При множественном выборе индекс последнего выбранного элемента
			/// </remarks>
			Int32 SelectedIndex { get; set; }

			/// <summary>
			/// Предпоследний выбранный индекс элемента списка, -1 выбора нет
			/// </summary>
			Int32 PrevSelectedIndex { get; }

			//
			// МНОЖЕСТВЕННЫЙ ВЫБОР
			//
			/// <summary>
			/// Возможность выбора нескольких элементов
			/// </summary>
			Boolean IsMultiSelected { get; set; }

			/// <summary>
			/// Режим выбора нескольких элементов (первый раз выделение, второй раз снятие выделения)
			/// </summary>
			Boolean ModeSelectAddRemove { get; set; }

			/// <summary>
			/// При множественном выборе всегда должен быть выбран хотя бы один элемент
			/// </summary>
			Boolean AlwaysSelectedItem { get; set; }

			/// <summary>
			/// Режим включения отмены выделения элемента
			/// </summary>
			/// <remarks>
			/// При его включение будет вызваться метод элемента <see cref="ICubeXSelectedItem.SetUnselectedItem"/>.
			/// Это может не понадобиться если, например, режим визуального реагирования как у кнопки
			/// </remarks>
			Boolean IsEnabledUnselectingItem { get; set; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение выбранного элемента
			/// </summary>
			/// <typeparam name="TItem">Тип элемента</typeparam>
			/// <returns>Выбранный элемента списка или значение по умолчанию если никакой элемент не выбран</returns>
			//---------------------------------------------------------------------------------------------------------
			TItem GetSelectedItem<TItem>() where TItem : ICubeXSelectedItem;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка, является указанный элемент выбранным в списке
			/// </summary>
			/// <typeparam name="TItem">Тип элемента</typeparam>
			/// <param name="item">Элемент списка</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			Boolean CheckItemIsSelected<TItem>(TItem item) where TItem : ICubeXSelectedItem;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса расположения в контейнере
			/// </summary>
			/// <typeparam name="TItem">Тип элемента</typeparam>
			/// <param name="item">Элемент списка</param>
			/// <returns>Индекс расположения элемент контейнере или -1 если его там нет</returns>
			//---------------------------------------------------------------------------------------------------------
			Int32 GetIndexItem<TItem>(TItem item) where TItem : ICubeXSelectedItem;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация элемента
			/// </summary>
			/// <remarks>
			/// Под активацией элемента понимается внешнее воздействие на элемент - щелчок или нажатие на нем
			/// </remarks>
			/// <typeparam name="TItem">Тип элемента</typeparam>
			/// <param name="item">Элемент списка</param>
			//---------------------------------------------------------------------------------------------------------
			void ActivatedItem<TItem>(TItem item) where TItem : ICubeXSelectedItem;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сброс выделения списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Unselect();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Список с поддержкой концепции выбора элемента на основе массива
		/// </summary>
		/// <remarks>
		/// Реализация cписка с поддержкой концепции выбора элемента на основе массива, с полной поддержкой 
		/// функциональности <see cref="ListArray{TItem}"/> с учетом особенности концепции выбора элемента
		/// </remarks>
		/// <typeparam name="TItem">Тип элемента списка</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class ListSelectedArray<TItem> : ListArray<TItem>, ICubeXListSelected
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Статус поддержки типом интерфейса <see cref="ICubeXSelectedItem"/>
			/// </summary>
			public readonly Boolean IsSelectableItem = typeof(TItem).IsSupportInterface<ICubeXSelectedItem>();
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Int32 mSelectedIndex = -1;
			internal Int32 mPrevSelectedIndex = -1;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Boolean mIsEnabledUnselectingItem;

			// Множественный выбор
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Boolean mIsMultiSelected = false;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Boolean mModeSelectAddRemove = false;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Boolean mAlwaysSelectedItem = false;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal List<TItem> mSelectedItems;

			// События
			internal Action mOnCurrentItemChanged;
			internal Action<Int32> mOnSelectedIndexChanged;
			internal Action<Int32> mOnSelectionAddItem;
			internal Action<Int32> mOnSelectionRemovedItem;
			internal Action<TItem> mOnSelectedItem;
			internal Action<TItem> mOnActivatedItem;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Выбранный индекс элемента списка, -1 выбора нет
			/// </summary>
			/// <remarks>
			/// При множественном выборе индекс последнего выбранного элемента
			/// </remarks>
			public Int32 SelectedIndex
			{
				get { return (mSelectedIndex); }
				set
				{
					this.SetSelectedItem(value);
				}
			}

			/// <summary>
			/// Предпоследний выбранный индекс элемента списка, -1 выбора нет
			/// </summary>
			public Int32 PrevSelectedIndex
			{
				get { return (mPrevSelectedIndex); }
			}

			/// <summary>
			/// Выбранный элемент списка
			/// </summary>
			public TItem SelectedItem
			{
				get
				{
					if (mSelectedIndex > -1)
					{
						return mArrayOfItems[mSelectedIndex];
					}
					else
					{
						return default(TItem);
					}
				}
			}

			/// <summary>
			/// Предпоследний выбранный элемент списка
			/// </summary>
			public TItem PrevSelectedItem
			{
				get
				{
					if (mPrevSelectedIndex > -1)
					{
						return mArrayOfItems[mPrevSelectedIndex];
					}
					else
					{
						return default(TItem);
					}
				}
			}

			/// <summary>
			/// Режим включения отмены выделения элемента
			/// </summary>
			/// <remarks>
			/// При его включение будет вызваться метод элемента <see cref="ICubeXSelectedItem.SetUnselectedItem"/>.
			/// Это может не понадобиться если, например, режим визуального реагирования как у кнопки
			/// </remarks>
			public Boolean IsEnabledUnselectingItem
			{
				get { return (mIsEnabledUnselectingItem);}
				set { mIsEnabledUnselectingItem = value; }
			}

			//
			// МНОЖЕСТВЕННЫЙ ВЫБОР
			//
			/// <summary>
			/// Возможность выбора нескольких элементов
			/// </summary>
			public Boolean IsMultiSelected
			{
				get { return mIsMultiSelected; }
				set
				{
					mIsMultiSelected = value;
				}
			}

			/// <summary>
			/// Режим выбора нескольких элементов (первый раз выделение, второй раз снятие выделения)
			/// </summary>
			public Boolean ModeSelectAddRemove
			{
				get { return mModeSelectAddRemove; }
				set
				{
					mModeSelectAddRemove = value;
				}
			}

			/// <summary>
			/// При множественном выборе всегда должен быть выбран хотя бы один элемент
			/// </summary>
			public Boolean AlwaysSelectedItem
			{
				get { return mAlwaysSelectedItem; }
				set
				{
					mAlwaysSelectedItem = value;
				}
			}

			/// <summary>
			/// Список выбранных элементов
			/// </summary>
			public List<TItem> SelectedItems
			{
				get { return mSelectedItems; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации об изменение текущего выбранного элемента
			/// </summary>
			public Action OnCurrentItemChanged
			{
				get { return mOnCurrentItemChanged; }
				set { mOnCurrentItemChanged = value; }
			}

			/// <summary>
			/// Событие для нотификации об изменение индекса выбранного элемента. Аргумент - индекс выбранного элемента
			/// </summary>
			public Action<Int32> OnSelectedIndexChanged
			{
				get { return mOnSelectedIndexChanged; }
				set { mOnSelectedIndexChanged = value; }
			}

			/// <summary>
			/// Событие для нотификации о добавлении элемента к списку выделенных(после добавления). Аргумент - индекс (позиция) добавляемого элемента
			/// </summary>
			public Action<Int32> OnSelectionAddItem
			{
				get { return mOnSelectionAddItem; }
				set { mOnSelectionAddItem = value; }
			}

			/// <summary>
			/// Событие для нотификации о удалении элемента из списка выделенных(после удаления). Аргумент - индекс (позиция) удаляемого элемента
			/// </summary>
			public Action<Int32> OnSelectionRemovedItem
			{
				get { return mOnSelectionRemovedItem; }
				set { mOnSelectionRemovedItem = value; }
			}

			/// <summary>
			/// Событие для нотификации о выборе элемента
			/// </summary>
			/// <remarks>
			/// В основном применяется(должно применяется) для служебных целей
			/// </remarks>
			public Action<TItem> OnSelectedItem
			{
				get { return mOnSelectedItem; }
				set { mOnSelectedItem = value; }
			}

			/// <summary>
			/// Событие для нотификации о активации элемента
			/// </summary>
			/// <remarks>
			/// В основном применяется(должно применяется) для служебных целей
			/// </remarks>
			public Action<TItem> OnActivatedItem
			{
				get { return mOnActivatedItem; }
				set { mOnActivatedItem = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка предустановленными данными
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ListSelectedArray()
				: base(INIT_MAX_COUNT)
			{
				mSelectedItems = new List<TItem>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные списка указанными данными
			/// </summary>
			/// <param name="max_count">Максимальное количество элементов</param>
			//---------------------------------------------------------------------------------------------------------
			public ListSelectedArray(Int32 max_count)
				: base(max_count)
			{
				mSelectedItems = new List<TItem>();
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ СО СПИСКОМ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение выбранного элемента
			/// </summary>
			/// <typeparam name="TItemSelected">Тип элемента</typeparam>
			/// <returns>Выбранный элемента списка или значение по умолчанию если никакой элемент не выбран</returns>
			//---------------------------------------------------------------------------------------------------------
			public TItemSelected GetSelectedItem<TItemSelected>() where TItemSelected : ICubeXSelectedItem
			{
				return (TItemSelected)(SelectedItem as ICubeXSelectedItem);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка, является указанный элемент выбранным в списке
			/// </summary>
			/// <typeparam name="TItemSelected">Тип элемента</typeparam>
			/// <param name="item">Элемент списка</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean CheckItemIsSelected<TItemSelected>(TItemSelected item) where TItemSelected : ICubeXSelectedItem
			{
				if(mIsMultiSelected)
				{
					return mSelectedItems.Contains((TItem)(item as ICubeXSelectedItem));
				}
				else
				{
					if(mSelectedIndex > -1 && mSelectedIndex < Count)
					{
						return mArrayOfItems[mSelectedIndex].Equals(item);
					}
					else
					{
						return false;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение индекса расположения в контейнере
			/// </summary>
			/// <typeparam name="TItem">Тип элемента</typeparam>
			/// <param name="item">Элемент списка</param>
			/// <returns>Индекс расположения элемент контейнере или -1 если его там нет</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetIndexItem<TItemSelected>(TItemSelected item) where TItemSelected : ICubeXSelectedItem
			{
				return (Array.IndexOf(mArrayOfItems, item));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация элемента списка
			/// </summary>
			/// <typeparam name="TItemSelected">Тип элемента</typeparam>
			/// <param name="item">Элемент списка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ActivatedItem<TItemSelected>(TItemSelected item) where TItemSelected : ICubeXSelectedItem
			{
				ActivatedItemDirect(item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сброс выделения списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Unselect()
			{
				if (mSelectedIndex != -1 && mSelectedIndex < Count)
				{
					if(mIsMultiSelected == false && mIsEnabledUnselectingItem)
					{
						if (IsSelectableItem)
						{
							ICubeXSelectedItem selected_item = mArrayOfItems[mSelectedIndex] as ICubeXSelectedItem;
							selected_item.SetUnselectedItem();
						}
					}

					mPrevSelectedIndex = mSelectedIndex;
					mSelectedIndex = -1;

					// Информируем о смене выбранного элемента
					if (mOnSelectedIndexChanged != null) mOnSelectedIndexChanged(mSelectedIndex);
					if (mOnCurrentItemChanged != null) mOnCurrentItemChanged();

					if (mIsMultiSelected && mIsEnabledUnselectingItem)
					{
						for (Int32 i = 0; i < mSelectedItems.Count; i++)
						{
						}

						mSelectedItems.Clear();
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ТЕКУЩИМ ЭЛЕМЕНТОМ =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Активация элемента списка
			/// </summary>
			/// <param name="item">Элемент списка</param>
			//---------------------------------------------------------------------------------------------------------
			internal void ActivatedItemDirect(ICubeXSelectedItem item)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					// 1) Смотрим на совпадение
					if (mArrayOfItems[i].Equals(item))
					{
						SetSelectedItem(i);
						if (mOnActivatedItem != null) mOnActivatedItem((TItem)item);
						break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка выбранного элемента
			/// </summary>
			/// <param name="index">Индекс выбранного элемента</param>
			//---------------------------------------------------------------------------------------------------------
			internal void SetSelectedItem(Int32 index)
			{
				if (index > -1 && index < Count)
				{
					// Выключенный элемент выбрать нельзя
					if (IsSelectableItem)
					{
						ICubeXSelectedItem selected_item = mArrayOfItems[index] as ICubeXSelectedItem;
						if (selected_item.CanSelectedItem() == false) return;
					}

					Int32 old_index = mSelectedIndex;

					// Если выбран другой элемент
					if (old_index != index)
					{
						// Сохраняем старый выбор
						mPrevSelectedIndex = mSelectedIndex;
						mSelectedIndex = index;

						// Если нет режима мульти выбора
						if (!mIsMultiSelected)
						{
							// Обновляем статус
							if (IsSelectableItem)
							{
								ICubeXSelectedItem selected_item = mArrayOfItems[mSelectedIndex] as ICubeXSelectedItem;
								selected_item.SetSelectedItem();
							}

							// Если предыдущий элемент был выбран, то снимаем выбор
							if (mPrevSelectedIndex != -1 && mPrevSelectedIndex < Count)
							{
								// Если нет мульти выбора
								if (IsSelectableItem && mIsEnabledUnselectingItem)
								{
									ICubeXSelectedItem prev_selected_item = mArrayOfItems[mPrevSelectedIndex] as ICubeXSelectedItem;
									prev_selected_item.SetUnselectedItem();
								}
							}
						}

						// Информируем о смене выбранного элемента
						if (mOnSelectedIndexChanged != null) mOnSelectedIndexChanged(mSelectedIndex);
						if (mOnCurrentItemChanged != null) mOnCurrentItemChanged();
					}

					// Пользователь выбрал тот же элемент  - Только если включен мультирежим 
					if (mIsMultiSelected)
					{
						// Смотрим, есть ли у нас элемент в выделенных
						if (mSelectedItems.Contains(mArrayOfItems[index]))
						{
							// Есть
							// Режим снятие/выделения
							if (mModeSelectAddRemove)
							{
								// Только если мы можем оставлять элементе на невыбранными
								if (mAlwaysSelectedItem == false ||
								   mAlwaysSelectedItem && mSelectedItems.Count > 1)
								{
									// Убираем выделение
									if (IsSelectableItem && mIsEnabledUnselectingItem)
									{
										ICubeXSelectedItem selected_item = mArrayOfItems[index] as ICubeXSelectedItem;
										selected_item.SetUnselectedItem();
									}

									// Удаляем
									mSelectedItems.Remove(mArrayOfItems[index]);

									// Информируем - вызываем событие
									if (mOnSelectionRemovedItem != null) mOnSelectionRemovedItem(index);
								}
								else
								{
#if UNITY_EDITOR
									UnityEngine.Debug.Log("At least one element must be selected");
#else
									XLogger.LogInfo("At least one element must be selected");
#endif
								}
							}
						}
						else
						{
							// Нету - добавляем
							mSelectedItems.Add(mArrayOfItems[index]);

							// Выделяем элемент
							if (IsSelectableItem)
							{
								ICubeXSelectedItem selected_item = mArrayOfItems[index] as ICubeXSelectedItem;
								selected_item.SetSelectedItem();
							}

							// Информируем - вызываем событие
							if (mOnSelectionAddItem != null) mOnSelectionAddItem(index);
						}
					}
				}
				else
				{
					if (index < 0)
					{
						this.Unselect();
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Дублирование текущего элемента и добавление элемента в список элементов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DublicateSelectedItem()
			{
				if (mSelectedIndex != -1)
				{
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление текущего элемента из списка (удаляется объект)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DeleteSelectedItem()
			{
				if (mSelectedIndex != -1)
				{
					RemoveAt(mSelectedIndex);
					SelectedIndex = -1;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение текущего элемента назад
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void MoveSelectedBackward()
			{
				// Корректируем индекс
				if (SelectedItem != null && mSelectedIndex > 0)
				{
					MoveUp(mSelectedIndex);

					// Корректируем индекс
					SetSelectedItem(mSelectedIndex - 1);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение текущего элемента вперед
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void MoveSelectedForward()
			{
				// Корректируем индекс
				if (SelectedItem != null && mSelectedIndex < Count - 1)
				{
					MoveDown(mSelectedIndex);

					// Корректируем индекс
					SelectedIndex++;
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ С МНОЖЕСТВЕННЫМ ВЫБОРОМ ============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Служебный метод
			/// </summary>
			/// <returns>Список выделенных индексов</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetSelectedIndexes()
			{
				String result = "{" + mSelectedItems.Count.ToString() + "} ";
				for (Int32 i = 0; i < mSelectedItems.Count; i++)
				{
					if (mSelectedItems[i] != null)
					{
						result += mSelectedItems[i].ToString() + "; ";
					}
				}

				return result;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================