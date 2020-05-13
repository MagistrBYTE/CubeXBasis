//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Иерархические структуры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternTreeNodeView.cs
*		Определение визуальной модели для отображения дерева.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
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
		/// Определение интерфейса для отображения узла дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTreeNodeView : ICubeXTreeNodeBase
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Данные узла
			/// </summary>
			ICubeXTreeNode Data { get; set; }

			/// <summary>
			/// Статус раскрытия узла отображения
			/// </summary>
			Boolean IsExpanded { get; set; }

			/// <summary>
			/// Статус выбор узла отображения
			/// </summary>
			/// <remarks>
			/// Возможность отметить узел флажком
			/// </remarks>
			Boolean IsChecked { get; set; }

			/// <summary>
			/// Родительский узел отображения
			/// </summary>
			ICubeXTreeNodeView IParentNodeView { get; }

			/// <summary>
			/// Список дочерних узлов отображения
			/// </summary>
			IList<ICubeXTreeNodeView> IChildrenView { get; }
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
		/// Класс для отображения узла дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTreeNodeView : ICubeXTreeNodeView
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="root">Корневой узел</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CTreeNodeView Create(ICubeXTreeNode root)
			{
				CTreeNodeView node_root_view = Create(root, null);
				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="node">Узел дерева</param>
			/// <param name="parent">Родительский узел отображения</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			protected static CTreeNodeView Create(ICubeXTreeNode node, ICubeXTreeNodeView parent)
			{
				CTreeNodeView node_root_view = new CTreeNodeView(node, parent);
				if (parent != null)
				{
					parent.IChildrenView.Add(node_root_view);
				}

				for (Int32 i = 0; i < node.CountChild; i++)
				{
					ICubeXTreeNode node_data = node.IChildren[i];
					Create(node_data, node_root_view);
				}

				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="root">Корневой узел</param>
			/// <param name="match">Предикат для фильтрования узлов</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CTreeNodeView CreateWithFilter(ICubeXTreeNode root, Predicate<ICubeXTreeNode> match)
			{
				CTreeNodeView node_root_view = CreateWithFilter(root, null, match);
				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узла отображения
			/// </summary>
			/// <param name="node">Узел дерева</param>
			/// <param name="parent">Родительский узел отображения</param>
			/// <param name="match">Предикат для фильтрования узлов</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			protected static CTreeNodeView CreateWithFilter(ICubeXTreeNode node, ICubeXTreeNodeView parent,
				Predicate<ICubeXTreeNode> match)
			{
				CTreeNodeView node_root_view = new CTreeNodeView(node, parent);
				if (parent != null)
				{
					if (node.FiltredNode(match))
					{
						parent.IChildrenView.Add(node_root_view);
					}
				}

				for (Int32 i = 0; i < node.CountChild; i++)
				{
					ICubeXTreeNode node_data = node.IChildren[i];
					CreateWithFilter(node_data, node_root_view, match);
				}

				return (node_root_view);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal ICubeXTreeNode mData;
			protected internal Boolean mIsChecked;
			protected internal Boolean mIsExpanded;
			protected internal ICubeXTreeNodeView mParent;
			protected internal Int32 mLevel;
			protected internal IList<ICubeXTreeNodeView> mChildren;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Данные узла дерева
			/// </summary>
			public ICubeXTreeNode Data
			{
				get { return (mData); }
				set { mData = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXTreeNodeView ===============================
			/// <summary>
			/// Статус выбор узла отображения
			/// </summary>
			/// <remarks>
			/// Возможность отметить узел флажком
			/// </remarks>
			public virtual Boolean IsChecked
			{
				get { return (mIsChecked); }
				set
				{
					mIsChecked = value;
					if(mIsChecked)
					{
						if (IChildrenView != null)
						{
							for (Int32 i = 0; i < IChildrenView.Count; i++)
							{
								IChildrenView[i].IsChecked = true;
							}
						}
					}
				}
			}

			/// <summary>
			/// Статус раскрытия узла отображения
			/// </summary>
			public virtual Boolean IsExpanded
			{
				get { return (mIsExpanded); }
				set 
				{
					mIsExpanded = value;
				}
			}

			/// <summary>
			/// Родительский узел отображения
			/// </summary>
			public ICubeXTreeNodeView IParentNodeView
			{
				get { return mParent; }
			}

			/// <summary>
			/// Список дочерних узлов отображения
			/// </summary>
			public IList<ICubeXTreeNodeView> IChildrenView
			{
				get { return (mChildren); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXTreeNodeBase ===============================
			/// <summary>
			/// Уровень вложенности узла
			/// </summary>
			/// <remarks>
			/// Корневые узлы дерева имеют уровень 0
			/// </remarks>
			public Int32 Level
			{
				get { return (mLevel); }
			}

			/// <summary>
			/// Количество дочерних узлов
			/// </summary>
			public Int32 CountChild
			{
				get { return mChildren.Count; }
			}

			/// <summary>
			/// Статус корневого узла
			/// </summary>
			public Boolean IsRoot
			{
				get { return (mParent == null); }
			}

			/// <summary>
			/// Статус узла который не имеет дочерних узлов
			/// </summary>
			public Boolean IsLeaf
			{
				get { return (mChildren.Count == 0); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные для узла отображения указанными значениями
			/// </summary>
			/// <param name="data">Данные</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeNodeView(ICubeXTreeNode data)
			{
				mData = data;
				mChildren = new List<ICubeXTreeNodeView>();
				mLevel = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные для узла отображения указанными значениями
			/// </summary>
			/// <param name="data">Данные</param>
			/// <param name="parent">Родительский узел отображения</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeNodeView(ICubeXTreeNode data, ICubeXTreeNodeView parent) 
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
					return (mData.Name);
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
			public ICubeXTreeNodeView this[Int32 index]
			{
				get { return mChildren[index]; }
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление узла
			/// </summary>
			/// <param name="data">Узел с данными</param>
			/// <returns>Добавленный узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public ICubeXTreeNodeView AddChild(ICubeXTreeNode data)
			{
				CTreeNodeView node = new CTreeNodeView(data, this);
				mChildren.Add(node);
				return node;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка существования дочернего узел отображения с указанными данными
			/// </summary>
			/// <param name="data">Узел с данными</param>
			/// <returns>Статус вхождения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean HasChild(ICubeXTreeNode data)
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
			public ICubeXTreeNodeView FindInChildren(ICubeXTreeNode data)
			{
				Int32 i = 0, l = CountChild;
				for (; i < l; ++i)
				{
					ICubeXTreeNodeView child = mChildren[i];
					if (child.Data.Equals(data)) return child;
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
			public Boolean RemoveChild(ICubeXTreeNodeView node)
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
			/// Посещение каждого узла с указанным предикатом
			/// </summary>
			/// <param name="match">Предикат</param>
			//---------------------------------------------------------------------------------------------------------
			public void VisitData(Predicate<ICubeXTreeNode> match)
			{
				if (match(mData))
				{
					Int32 i = 0, l = CountChild;
					for (; i < l; ++i)
					{
						((CTreeNodeView)mChildren[i]).VisitData(match);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Посещение каждого узла отображения с указанным делегатом
			/// </summary>
			/// <param name="match">Предикат</param>
			//---------------------------------------------------------------------------------------------------------
			public void Visit(Predicate<ICubeXTreeNodeView> match)
			{
				if (match(this))
				{
					Int32 i = 0, l = CountChild;
					for (; i < l; ++i)
					{
						((CTreeNodeView)mChildren[i]).Visit(match);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Посещение каждого узла с указанным предикатом
			/// </summary>
			/// <param name="on_visitor">Предикат</param>
			//---------------------------------------------------------------------------------------------------------
			public void VisitDataSelected(Action<ICubeXTreeNode> on_visitor)
			{
				if(mIsChecked)
				{
					on_visitor(this.Data);
				}

				for (Int32 i = 0; i < CountChild; i++)
				{
					((CTreeNodeView)mChildren[i]).VisitDataSelected(on_visitor);
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
				if (mIsChecked)
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
		/// Базовый класс для отображения дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTreeViewBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal CTreeNodeView mRoot;
			protected internal String mSearchString;
			protected internal TStringSearchOption mSearchOption;
			protected internal CTreeNodeView mSelectedNode;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Корневой узел дерева
			/// </summary>
			public CTreeNodeView Root
			{
				get { return (mRoot); }
			}

			/// <summary>
			/// Строка для фильтрования и поиска узлов по имени
			/// </summary>
			public String SearchString
			{
				get { return (mSearchString); }
				set
				{
					if (mSearchString != value)
					{
						mSearchString = value;
						OnFilterd();
					}
				}
			}

			/// <summary>
			/// Опции поиска узлов по имени
			/// </summary>
			public TStringSearchOption SearchOption
			{
				get { return (mSearchOption); }
				set
				{
					if (mSearchOption != value)
					{
						mSearchOption = value;
						OnFilterd();
					}
				}
			}

			/// <summary>
			/// Текущий выбранный узел отображения
			/// </summary>
			public CTreeNodeView SelectedNode
			{
				get { return (mSelectedNode); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewBase()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="root">Корневой узел отображения</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewBase(CTreeNodeView root)
			{
				mRoot = root;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Построение дерева
			/// </summary>
			/// <param name="root">Данные</param>
			//---------------------------------------------------------------------------------------------------------
			public void Build(CTreeNode root)
			{
				mRoot = CTreeNodeView.Create(root);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Фильтрация данных
			/// </summary>
			/// <remarks>
			/// Метод автоматически вызывается при изменении строки поиска или опции поиска
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OnFilterd()
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