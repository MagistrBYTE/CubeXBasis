//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема отображения иерархических данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternTreeViewItem.cs
*		Определение визуальной модели элемента для отображения дерева.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternTreeNode
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение базового интерфейса для отображения узла дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTreeViewItemBase
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Уровень вложенности узла
			/// </summary>
			/// <remarks>
			/// Корневые узлы дерева имеют уровень 0
			/// </remarks>
			Int32 Level { get; }

			/// <summary>
			/// Статус корневого узла
			/// </summary>
			Boolean IsRoot { get; }

			/// <summary>
			/// Статус узла который не имеет дочерних узлов
			/// </summary>
			Boolean IsLeaf { get; }

			/// <summary>
			/// Статус раскрытия узла отображения
			/// </summary>
			Boolean IsExpanded { get; set; }

			/// <summary>
			/// Доступность узла
			/// </summary>
			/// <remarks>
			/// Подразумевается некая логическая доступность узла.
			/// Активировано может быть для нескольких узлов дерева
			/// </remarks>
			Boolean IsEnabled { get; set; }

			/// <summary>
			/// Выбор узла
			/// </summary>
			/// <remarks>
			/// Подразумевает выбор узла пользователем для просмотра свойств.
			/// По умолчанию может быть активировано только для одного узла дерева
			/// </remarks>
			Boolean IsSelected { get; set; }

			/// <summary>
			/// Статус выбор узла отображения
			/// </summary>
			/// <remarks>
			/// Возможность отметить узел флажком
			/// </remarks>
			Boolean? IsChecked { get; set; }

			/// <summary>
			/// Отображение данных узла
			/// </summary>
			/// <remarks>
			/// Подразумевает данных узла
			/// По умолчанию может быть активировано только для одного узла дерева
			/// </remarks>
			Boolean IsPresented { get; set; }

			/// <summary>
			/// Родительский узел отображения
			/// </summary>
			ICubeXTreeViewItemBase IParentNodeView { get; }

			/// <summary>
			/// Список дочерних узлов отображения
			/// </summary>
			IList<ICubeXTreeViewItemBase> IChildrenView { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Раскрытие всего узла отображения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Expanded();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сворачивание всего узла отображения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			void Collapsed();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количество выделенных узлов
			/// </summary>
			/// <returns>Количество выделенных узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			Int32 GetCountCheckedNodes();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для отображения узла дерева c данными
		/// </summary>
		/// <typeparam name="TData">Тип данных узла</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTreeViewItemData<TData> : ICubeXTreeViewItemBase where TData : class
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Данные узла
			/// </summary>
			TData Data { get; set; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для отображения узла дерева с обобщенными данными
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTreeViewItemObject : ICubeXTreeViewItemData<System.Object>
		{

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для отображения узла дерева
		/// </summary>
		/// <typeparam name="TData">Тип данных узла</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class TreeViewItemBase<TData> : PropertyChangedBase, ICubeXTreeViewItemData<TData> where TData : class
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsIsEnabled = new PropertyChangedEventArgs(nameof(IsEnabled));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsSelected = new PropertyChangedEventArgs(nameof(IsSelected));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsChecked = new PropertyChangedEventArgs(nameof(IsChecked));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsPresented = new PropertyChangedEventArgs(nameof(IsPresented));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal TData mData;
			protected internal Boolean mIsExpanded;
			protected internal Boolean mIsEnabled;
			protected internal Boolean mIsSelected;
			protected internal Boolean? mIsChecked = false;
			protected internal Boolean mIsPresented;
			protected internal ICubeXTreeViewItemBase mParent;
			protected internal Int32 mLevel;
			protected internal IList<ICubeXTreeViewItemBase> mChildren;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Данные узла дерева
			/// </summary>
			[Browsable(false)]
			public TData Data
			{
				get { return (mData); }
				set 
				{ 
					mData = value;
					if (mData != null && mData is ICubeXTreeViewItemOwner view_owner)
					{
						view_owner.OwnerView = this;
					}
				}
			}

			/// <summary>
			/// Уровень вложенности узла
			/// </summary>
			/// <remarks>
			/// Корневые узлы дерева имеют уровень 0
			/// </remarks>
			[Browsable(false)]
			public Int32 Level
			{
				get { return (mLevel); }
			}

			/// <summary>
			/// Статус корневого узла
			/// </summary>
			[Browsable(false)]
			public Boolean IsRoot
			{
				get { return (mParent == null); }
			}

			/// <summary>
			/// Статус узла который не имеет дочерних узлов
			/// </summary>
			[Browsable(false)]
			public Boolean IsLeaf
			{
				get { return (mChildren.Count == 0); }
			}

			/// <summary>
			/// Статус раскрытия узла отображения
			/// </summary>
			[Browsable(false)]
			public virtual Boolean IsExpanded
			{
				get { return (mIsExpanded); }
				set
				{
					mIsExpanded = value;
				}
			}

			/// <summary>
			/// Доступность узла
			/// </summary>
			/// <remarks>
			/// Подразумевается некая логическая доступность узла.
			/// Активировано может быть для нескольких узлов дерева
			/// </remarks>
			[Browsable(false)]
			public virtual Boolean IsEnabled
			{
				get { return (mIsEnabled); }
				set
				{
					if (mIsEnabled != value)
					{
						mIsEnabled = value;
						NotifyPropertyChanged(PropertyArgsIsEnabled);
						RaiseIsEnabledChanged();
					}
				}
			}

			/// <summary>
			/// Выбор узла
			/// </summary>
			/// <remarks>
			/// Подразумевает выбор узла пользователем для просмотра свойств.
			/// По умолчанию может быть активировано только для одного узла дерева
			/// </remarks>
			[Browsable(false)]
			public virtual Boolean IsSelected
			{
				get { return (mIsSelected); }
				set
				{
					if (mIsSelected != value)
					{
						mIsSelected = value;
						NotifyPropertyChanged(PropertyArgsIsSelected);
						RaiseIsSelectedChanged();
					}
				}
			}

			/// <summary>
			/// Статус выбор узла отображения
			/// </summary>
			/// <remarks>
			/// Возможность отметить узел флажком
			/// </remarks>
			[Browsable(false)]
			public virtual Boolean? IsChecked
			{
				get { return (mIsChecked); }
				set
				{
					if (mIsChecked != value)
					{
						mIsChecked = value;
						NotifyPropertyChanged(PropertyArgsIsChecked);
						RaiseIsCheckedChanged();
						if (mIsChecked.HasValue)
						{
							if (IChildrenView != null)
							{
								for (Int32 i = 0; i < IChildrenView.Count; i++)
								{
									IChildrenView[i].IsChecked = mIsChecked.Value;
								}
							}
						}
					}
				}
			}

			/// <summary>
			/// Отображение данных узла
			/// </summary>
			/// <remarks>
			/// Подразумевает данных узла
			/// По умолчанию может быть активировано только для одного узла дерева
			/// </remarks>
			[Browsable(false)]
			public virtual Boolean IsPresented
			{
				get { return (mIsPresented); }
				set
				{
					if (mIsPresented != value)
					{
						mIsPresented = value;
						NotifyPropertyChanged(PropertyArgsIsPresented);
						RaiseIsPresentedChanged();
					}
				}
			}

			/// <summary>
			/// Родительский узел отображения
			/// </summary>
			public ICubeXTreeViewItemBase IParentNodeView
			{
				get { return mParent; }
			}

			/// <summary>
			/// Список дочерних узлов отображения
			/// </summary>
			public IList<ICubeXTreeViewItemBase> IChildrenView
			{
				get { return (mChildren); }
			}

			/// <summary>
			/// Количество дочерних узлов
			/// </summary>
			public Int32 CountChild
			{
				get { return mChildren.Count; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные для узла отображения указанными значениями
			/// </summary>
			/// <param name="data">Данные</param>
			//---------------------------------------------------------------------------------------------------------
			public TreeViewItemBase(TData data)
			{
				mData = data;
				mChildren = new List<ICubeXTreeViewItemBase>();
				mLevel = 0;
				if(mData != null && mData is ICubeXTreeViewItemOwner view_owner)
				{
					view_owner.OwnerView = this;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные для узла отображения указанными значениями
			/// </summary>
			/// <param name="data">Данные</param>
			/// <param name="parent">Родительский узел отображения</param>
			//---------------------------------------------------------------------------------------------------------
			public TreeViewItemBase(TData data, ICubeXTreeViewItemBase parent) 
				: this(data)
			{
				mParent = parent;
				mLevel = mParent != null ? mParent.Level + 1 : 0;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление узла</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				if (mData != null)
				{
					return (mData.ToString());
				}
				else
				{
					return ("Now data");
				}
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация дочерних узлов отображения 
			/// </summary>
			/// <param name="index">Индекс узла</param>
			/// <returns>Дочерний узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public ICubeXTreeViewItemBase this[Int32 index]
			{
				get { return mChildren[index]; }
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение доступности элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsEnabledChanged()
			{
				//this.NotifyOwnerUpdated(nameof(IsEnabled));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение выбора элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsSelectedChanged()
			{
				//this.NotifyOwnerUpdated(nameof(IsSelected));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение выбора элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsCheckedChanged()
			{
				//this.NotifyOwnerUpdated(nameof(IsChecked));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение статуса отображения элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsPresentedChanged()
			{
				//if (mIsPresented && mOwner != null)
				//{
				//	mOwner.UnsetAllPresent(this);
				//}

				//this.NotifyOwnerUpdated(nameof(IsPresented));
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление узла
			/// </summary>
			/// <param name="data">Данные узла</param>
			/// <returns>Добавленный узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public ICubeXTreeViewItemBase AddChild(TData data)
			{
				TreeViewItemBase<TData> node = new TreeViewItemBase<TData>(data, this);
				mChildren.Add(node);
				return node;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка существования дочернего узел отображения с указанными данными
			/// </summary>
			/// <param name="data">Данные узла</param>
			/// <returns>Статус вхождения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean HasChild(TData data)
			{
				return FindInChildren(data) != null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск дочернего узла отображения с указанными данными
			/// </summary>
			/// <param name="data">Узел с данными</param>
			/// <returns>Найденный дочерний узел или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public ICubeXTreeViewItemBase FindInChildren(TData data)
			{
				Int32 i = 0, l = CountChild;
				for (; i < l; ++i)
				{
					ICubeXTreeViewItemBase child = mChildren[i];
					if (child is ICubeXTreeViewItemData<TData> child_data)
					{
						if (XObject.ObjectEquals(child_data.Data, data)) return child;
					}	
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление дочернего узла отображения
			/// </summary>
			/// <param name="node">Узел отображения</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean RemoveChild(ICubeXTreeViewItemBase node)
			{
				return mChildren.Remove(node);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка дочерних узлов отображения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mChildren.Clear();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Посещение каждого узла отображения с указанным делегатом
			/// </summary>
			/// <param name="match">Предикат</param>
			//---------------------------------------------------------------------------------------------------------
			public void Visit(Predicate<ICubeXTreeViewItemBase> match)
			{
				if (match(this))
				{
					Int32 i = 0, l = CountChild;
					for (; i < l; ++i)
					{
						((TreeViewItemBase<TData>)mChildren[i]).Visit(match);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Посещение каждого узла с указанным предикатом
			/// </summary>
			/// <param name="match">Предикат</param>
			//---------------------------------------------------------------------------------------------------------
			public void VisitData(Predicate<ICubeXModel> match)
			{
				if (mData != null && mData is ICubeXModel tree_node)
				{
					if (match(tree_node))
					{
						Int32 i = 0, l = CountChild;
						for (; i < l; ++i)
						{
							((TreeViewItemBase<TData>)mChildren[i]).VisitData(match);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Посещение выбранных узлов с указанным предикатом
			/// </summary>
			/// <param name="on_visitor">Предикат</param>
			//---------------------------------------------------------------------------------------------------------
			public void VisitDataChecked(Action<ICubeXModel> on_visitor)
			{
				if(mIsChecked.HasValue && mIsChecked.Value)
				{
					if (mData != null && mData is ICubeXModel tree_node)
					{
						on_visitor(tree_node);
					}
				}

				for (Int32 i = 0; i < CountChild; i++)
				{
					((TreeViewItemBase<TData>)mChildren[i]).VisitDataChecked(on_visitor);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Раскрытие всего узла отображения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Expanded()
			{
				IsExpanded = true;
				for (Int32 i = 0; i < mChildren.Count; i++)
				{
					mChildren[i].Expanded();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сворачивание всего узла отображения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Collapsed()
			{
				IsExpanded = false;
				for (Int32 i = 0; i < mChildren.Count; i++)
				{
					mChildren[i].Collapsed();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количество выделенных узлов
			/// </summary>
			/// <returns>Количество выделенных узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 GetCountCheckedNodes()
			{
				Int32 result = 0;
				if (mIsChecked.HasValue && mIsChecked.Value == true)
				{
					result++;
				}

				if(IChildrenView != null && IChildrenView.Count > 0)
				{
					for (Int32 i = 0; i < IChildrenView.Count; i++)
					{
						result += IChildrenView[i].GetCountCheckedNodes();
					}
				}

				return (result);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для отображения узла дерева с общими данными
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTreeViewItemObject : TreeViewItemBase<System.Object>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные для узла отображения указанными значениями
			/// </summary>
			/// <param name="data">Данные</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewItemObject(System.Object data)
				: base(data)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные для узла отображения указанными значениями
			/// </summary>
			/// <param name="data">Данные</param>
			/// <param name="parent">Родительский узел отображения</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewItemObject(System.Object data, ICubeXTreeViewItemBase parent)
				: base(data, parent)
			{
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================