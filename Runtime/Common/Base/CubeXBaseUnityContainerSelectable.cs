//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Базовая подсистема Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseUnityContainerSelectable.cs
*		Контейнер для создания, хранение и управление списком дочерних элементов с поддержкой концепции выбора элемента.
*		Контейнер обеспечивает базовую функциональность для создания, хранения и управления списком дочерних элементов 
*	применительно к игровым объектам Unity. 
*		Он обеспечивает синхронизацию между позицией элемента в списке и позицией компонента трасформации применительно
*	к игровому объекту.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Контейнер для создания, хранение и управление списком дочерних элементов с поддержкой концепции выбора элемента
		/// </summary>
		/// <remarks>
		/// <para>
		/// Контейнер обеспечивает базовую функциональность для создания, хранения и управления списком дочерних элементов 
		/// применительно к игровым объектам Unity
		/// </para>
		/// <para>
		/// Он обеспечивает синхронизацию между позицией элемента в списке и позицией компонента трасформации применительно 
		/// к игровому объекту
		/// </para>
		/// </remarks>
		/// <typeparam name="TItem">Тип элемента</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class ContainerSelectableComponent<TItem> : ListSelectedArray<TItem> where TItem : UnityEngine.Component
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal Transform mParent;

			// Механизм создания элементов
			[NonSerialized]
			internal IList mItemsSource;
			[NonSerialized]
			internal Func<System.Object, TItem> mItemConstructor;
			[NonSerialized]
			internal Action<TItem, System.Object> mItemChanged;
			[SerializeField]
			internal TItem mItemPrefab;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Родительский компонент трансформации
			/// </summary>
			public Transform Parent
			{
				get { return mParent; }
				set
				{
					mParent = value;
				}
			}

			//
			// МЕХАНИЗМ СОЗДАНИЯ ЭЛЕМЕНТОВ
			//
			/// <summary>
			/// Источник данных
			/// </summary>
			public IList ItemsSource
			{
				get { return (mItemsSource); }
				set
				{
					if (value is ICubeXNotifyCollectionChanged)
					{
						mItemsSource = value;
						(mItemsSource as ICubeXNotifyCollectionChanged).OnCollectionChanged += OnCollectionChangedHandler;
					}
					else
					{
						if (mItemsSource != null && mItemsSource is ICubeXNotifyCollectionChanged)
						{
							(mItemsSource as ICubeXNotifyCollectionChanged).OnCollectionChanged -= OnCollectionChangedHandler;
						}

						mItemsSource = null;
					}
				}
			}

			/// <summary>
			/// Внешний конструктор для создания элемента
			/// </summary>
			public Func<System.Object, TItem> ItemConstructor
			{
				get { return mItemConstructor; }
				set { mItemConstructor = value; }
			}

			/// <summary>
			/// Делегат для изменения элемента
			/// </summary>
			public Action<TItem, System.Object> ItemChanged
			{
				get { return mItemChanged; }
				set { mItemChanged = value; }
			}

			/// <summary>
			/// Префаб для создания элемента
			/// </summary>
			public TItem ItemPrefab
			{
				get { return mItemPrefab; }
				set { mItemPrefab = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные контейнера предустановленными данными
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ContainerSelectableComponent()
				: base(INIT_MAX_COUNT)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные контейнера указанными данными
			/// </summary>
			/// <param name="max_count">Максимальное количество элементов</param>
			//---------------------------------------------------------------------------------------------------------
			public ContainerSelectableComponent(Int32 max_count)
				: base(max_count)
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление индексов компонентов трансформации
			/// </summary>
			/// <remarks>
			/// Подразумевается что положение элементов в списке изменилось, например в результате сортировки или 
			/// перемещения элемента и теперь необходимо синхронизировать индексы компонента трансформации 
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateIndexTransformSibling()
			{
				for (Int32 i = 0; i < Count; i++)
				{
					mArrayOfItems[i].transform.SetSiblingIndex(i);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление индексов элементов
			/// </summary>
			/// <remarks>
			/// Подразумевается что индексы компонентов трансформации изменились теперь необходимо синхронизировать
			/// с положение данных элементов в списке
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateIndexItems()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ДОБАВЛЕНИЯ ЭЛЕМЕНТОВ ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление существующего элемента в контейнер
			/// </summary>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddExistsItem(TItem item)
			{
				// Присваиваем в иерархию
				if (mParent != null)
				{
					item.transform.SetParent(mParent, false);
				}

				item.name = "Item_" + Count.ToString();

				// Добавляем в список
				Add(item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание из префаба и добавление элемента в контейнер
			/// </summary>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual TItem AddNewItemFromPrefab()
			{
#if UNITY_EDITOR
				if (mItemPrefab == null)
				{
					Debug.LogError("ItemPrefab == NULL");
					return default(TItem);
				}
#endif
				// Создаем элемент и добавляем
				TItem item = GameObject.Instantiate(mItemPrefab);
				item.gameObject.SetActive(true);
				AddExistsItem(item);
				return item;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание элемента из указанного префаба и добавление элемента в контейнер
			/// </summary>
			/// <param name="item_prefab">Префаб элемента</param>
			/// <returns>Созданный элемент</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual TItem AddNewItemFromPrefab(TItem item_prefab)
			{
#if UNITY_EDITOR
				if (item_prefab == null)
				{
					Debug.LogError("item_prefab == NULL");
					return default(TItem);
				}
#endif
				// Создаем элемент и добавляем
				TItem item = GameObject.Instantiate(item_prefab);
				item.gameObject.SetActive(true);
				AddExistsItem(item);
				return item;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Дублирование элемента и добавление элемента в контейнер
			/// </summary>
			/// <param name="index">Индекс дублируемого элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DublicateItem(Int32 index)
			{
				// Создаем элемент
				TItem item = GameObject.Instantiate(this[index]);
				AddExistsItem(item);
			}
			#endregion

			#region ======================================= МЕТОДЫ ВСТАВКИ ЭЛЕМЕНТОВ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента в указанную позицию
			/// </summary>
			/// <param name="index">Позиция вставки</param>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void InsertItem(Int32 index, TItem item)
			{
				// Присваиваем в иерархию
				if (mParent != null)
				{
					item.transform.SetParent(mParent, false);
				}

				item.name = "Item_" + Count.ToString();

				// Вставляем в список
				Insert(index, item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента после указанного элемента
			/// </summary>
			/// <param name="original">Элемент после которого будет произведена вставка</param>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void InsertAfterItem(TItem original, TItem item)
			{
				if (item == null) return;

				Int32 index = IndexOf(original);
				InsertItem(index + 1, item);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вставка элемента перед указанным элементом
			/// </summary>
			/// <param name="original">Элемент перед которым будет произведена вставка</param>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void InsertBeforeItem(TItem original, TItem item)
			{
				if (item == null) return;

				Int32 index = IndexOf(original);
				InsertItem(index, item);
			}
			#endregion

			#region ======================================= МЕТОДЫ УДАЛЕНИЯ ЭЛЕМЕНТОВ =================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента из контейнера по указанному имени игрового объекта
			/// </summary>
			/// <param name="game_object_name">Имя игрового объекта</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean RemoveItem(String game_object_name)
			{
				Boolean status = false;
				for (Int32 i = 0; i < Count; i++)
				{
					if (mArrayOfItems[i].gameObject.name == game_object_name)
					{
						status = RemoveItem(mArrayOfItems[i]);
						break;
					}
				}

				return (status);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента из контейнера по указанному имени игрового объекта вместе с игровым объектом
			/// </summary>
			/// <param name="game_object_name">Имя игрового объекта</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean DeleteItem(String game_object_name)
			{
				Boolean status = false;
				for (Int32 i = 0; i < Count; i++)
				{
					if (mArrayOfItems[i].gameObject.name == game_object_name)
					{
						XGameObjectDispatcher.Destroy(mArrayOfItems[i].gameObject);
						status = RemoveItem(mArrayOfItems[i]);
						break;
					}
				}

				return (status);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента из контейнера по индексу
			/// </summary>
			/// <param name="index">Индекс элемента</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean RemoveItem(Int32 index)
			{
				// Удаляем элемент
				RemoveAt(index);
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента из контейнера по индексу вместе с игровым объектом
			/// </summary>
			/// <param name="index">Индекс элемента</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean DeleteItem(Int32 index)
			{
				// Удаляем элемент
				XGameObjectDispatcher.Destroy(mArrayOfItems[index].gameObject);
				RemoveAt(index);
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента из контейнера
			/// </summary>
			/// <param name="item">Элемент</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean RemoveItem(TItem item)
			{
				if (item == null) return false;

				// 2) Удаляем элемент
				if (Remove(item))
				{
					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление элемента из контейнера вместе с игровым объектом
			/// </summary>
			/// <param name="item">Элемент</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean DeleteItem(TItem item)
			{
				if (item == null) return false;

				// 2) Удаляем элемент
				if (Remove(item))
				{
					XGameObjectDispatcher.Destroy(item.gameObject);
					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех элементов из контейнера
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ClearItems()
			{
				Clear();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление всех элементов из контейнера вместе с игровыми объектами
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DeleteItems()
			{
				for (Int32 i = 0; i < Count; i++)
				{
					XGameObjectDispatcher.Destroy(mArrayOfItems[i].gameObject);
				}

				Clear();
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОИСКА ЭЛЕМЕНТОВ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существовании элемента в контейнере
			/// </summary>
			/// <param name="item">Элемент</param>
			/// <returns>Статус существования элемента</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean ExistsItem(TItem item)
			{
				if (item == null) return (false);

				for (Int32 i = 0; i < Count; i++)
				{
					if (mArrayOfItems[i].Equals(item))
					{
						return (true);
					}
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса указанного элемента
			/// </summary>
			/// <param name="item">Элемент</param>
			/// <returns>Индекс элемента или -1 если модель не найдена</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Int32 IndexOfItem(TItem item)
			{
				return (IndexOf(item));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск элемента по указанному имени игрового объекта
			/// </summary>
			/// <param name="game_object_name">Имя игрового объекта</param>
			/// <returns>Найденный модель или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual TItem GetItemFromName(String game_object_name)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (mArrayOfItems[i].gameObject.name == game_object_name)
					{
						return (mArrayOfItems[i]);
					}
				}

				return (default(TItem));
			}
			#endregion

			#region ======================================= МЕТОДЫ ПЕРЕМЕЩЕНИЯ ЭЛЕМЕНТОВ ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента в новую позицию
			/// </summary>
			/// <param name="old_index">Старая позиция</param>
			/// <param name="new_index">Новая позиция</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void MoveItem(Int32 old_index, Int32 new_index)
			{
				Move(old_index, new_index);

				if (mParent != null)
				{
					Transform transform = mParent.GetChild(old_index);
					if (transform != null)
					{
						transform.SetSiblingIndex(new_index);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вверх по списку
			/// </summary>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void MoveUpItem(TItem item)
			{
				if (item == null) return;

				Int32 index = IndexOf(item);
				if (index > 0)
				{
					MoveUp(index);

					if (mParent != null)
					{
						Transform transform = mParent.GetChild(index);
						if (transform != null)
						{
							transform.SetSiblingIndex(index - 1);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение элемента вниз по списку
			/// </summary>
			/// <param name="item">Элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void MoveDownItem(TItem item)
			{
				if (item == null) return;

				Int32 index = IndexOf(item);
				if (index > -1 && index < Count)
				{
					MoveDown(index);

					if (mParent != null)
					{
						Transform transform = mParent.GetChild(index) as Transform;
						if (transform != null)
						{
							transform.SetSiblingIndex(index + 1);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка элементов по возрастанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SortItemsAscending()
			{
				SortAscending();
				UpdateIndexTransformSibling();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка элементов по убыванию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void SortItemsDescending()
			{
				SortDescending();
				UpdateIndexTransformSibling();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сортировка элементов посредством делегата
			/// </summary>
			/// <param name="comparison">Делегат сравнивающий два объекта одного типа</param>
			//---------------------------------------------------------------------------------------------------------
			public void SortItems(Comparison<TItem> comparison)
			{
				Sort(comparison);
				UpdateIndexTransformSibling();
			}
			#endregion

			#region ======================================= МЕТОДЫ РАБОТЫ С ТЕКУЩИМ ЭЛЕМЕНТОМ =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Дублирование текущего элемента и добавление элемента в контейнер
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void DublicateSelectedItem()
			{
				if (mSelectedIndex > -1)
				{
					DublicateItem(mSelectedIndex);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление текущего элемента из контейнера (удаляется объект)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void DeleteSelectedItem()
			{
				if (mSelectedIndex != -1)
				{
					XGameObjectDispatcher.Destroy(mArrayOfItems[mSelectedIndex].gameObject);
					RemoveAt(mSelectedIndex);
					SelectedIndex = -1;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение текущего элемента назад
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public new void MoveSelectedBackward()
			{
				// Корректируем индекс
				if (SelectedItem != null && mSelectedIndex > 0)
				{
					// Сначала снимаем выделение с текущего
					if (IsSelectableItem)
					{
						ICubeXSelectedItem selected_item = mArrayOfItems[SelectedIndex] as ICubeXSelectedItem;
						selected_item.SetUnselectedItem();
					}

					// Перемещаем вверх по списку
					mArrayOfItems[mSelectedIndex].transform.SetSiblingIndex(mSelectedIndex - 1);
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
			public new void MoveSelectedForward()
			{
				// Корректируем индекс
				if (SelectedItem != null && mSelectedIndex < Count - 1)
				{
					// Сначала снимаем выделение с текущего
					if (IsSelectableItem)
					{
						ICubeXSelectedItem selected_item = mArrayOfItems[SelectedIndex] as ICubeXSelectedItem;
						selected_item.SetUnselectedItem();
					}

					// Перемещаем вниз по списку
					mArrayOfItems[mSelectedIndex].transform.SetSiblingIndex(mSelectedIndex + 1);
					MoveDown(mSelectedIndex);

					// Корректируем индекс
					SetSelectedItem(mSelectedIndex + 1);
				}
			}
			#endregion

			#region ======================================= ОБРАБОТЧИКИ СОБЫТИЙ =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обработчик события изменения данных источника привязки
			/// </summary>
			/// <param name="action">Тип действия</param>
			/// <param name="item">Элемент/список элементов</param>
			/// <param name="index">Индекс элемента</param>
			/// <param name="count">Количество элементов</param>
			//---------------------------------------------------------------------------------------------------------
			private void OnCollectionChangedHandler(TNotifyCollectionChangedAction action, System.Object item, Int32 index, Int32 count)
			{
				switch (action)
				{
					case TNotifyCollectionChangedAction.Add:
						{
							if (item != null && mItemConstructor != null)
							{
								AddExistsItem(mItemConstructor(item));
							}
						}
						break;
					case TNotifyCollectionChangedAction.Move:
						{
							Int32 old_index = index;
							Int32 new_index = count;
							MoveItem(old_index, new_index);
						}
						break;
					case TNotifyCollectionChangedAction.Remove:
						{
							RemoveItem(index);
						}
						break;
					case TNotifyCollectionChangedAction.Replace:
						{
						}
						break;
					case TNotifyCollectionChangedAction.Reset:
						{
							if (count == 0)
							{
								ClearItems();
								IList list = item as IList;
								if (list != null && mItemConstructor != null)
								{
									for (Int32 i = 0; i < count; i++)
									{
										if (list[i] != null)
										{
											AddExistsItem(mItemConstructor(list[i]));
										}
									}
								}
							}
							else
							{
								mItemChanged(mParent.GetChild(index).GetComponent<TItem>(), item);
							}
						}
						break;
					case TNotifyCollectionChangedAction.Clear:
						{
							ClearItems();
						}
						break;
					default:
						break;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Контейнер с поддержкой концепции выбора элемента для компонента <see cref="RectTransform"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CContainerSelectableRectTransform : ContainerSelectableComponent<RectTransform>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные контейнера предустановленными данными
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CContainerSelectableRectTransform()
				: base(INIT_MAX_COUNT)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные контейнера указанными данными
			/// </summary>
			/// <param name="max_count">Максимальное количество элементов</param>
			//---------------------------------------------------------------------------------------------------------
			public CContainerSelectableRectTransform(Int32 max_count)
				: base(max_count)
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОЛЬЗОВАТЕЛЬСКОГО ВВОДА ============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Щелчок по области контейнера
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnPointerClick(PointerEventData event_data)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					RectTransform rect_item = mArrayOfItems[i];

					if (rect_item != null)
					{
						if (RectTransformUtility.RectangleContainsScreenPoint(rect_item, event_data.position,
							event_data.pressEventCamera))
						{
							SelectedIndex = i;
							break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Нажатие на область контейнера
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnPointerDown(PointerEventData event_data)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					RectTransform rect_item = mArrayOfItems[i];

					if (rect_item != null)
					{
						if (RectTransformUtility.RectangleContainsScreenPoint(rect_item, event_data.position,
							event_data.pressEventCamera))
						{
							SelectedIndex = i;
							break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отпускание кнопки мыши/тача
			/// </summary>
			/// <param name="event_data">Параметры события</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnPointerUp(PointerEventData event_data)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					RectTransform rect_item = mArrayOfItems[i];

					if (rect_item != null)
					{
						if (RectTransformUtility.RectangleContainsScreenPoint(rect_item, event_data.position,
							event_data.pressEventCamera))
						{
							SelectedIndex = i;
							break;
						}
					}
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