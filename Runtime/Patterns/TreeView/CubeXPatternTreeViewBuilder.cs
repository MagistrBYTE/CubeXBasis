//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема отображения иерархических данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternTreeViewBuilder.cs
*		Определение статического класса для посторенние визуальной модели дерева.
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
		/// Статический класс для построение визуальной модели дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XTreeViewBuilder
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание узла отображения с общими данными
			/// </summary>
			/// <param name="data">Данные</param>
			/// <param name="parent">Родительский узел отображения</param>
			//---------------------------------------------------------------------------------------------------------
			public static CTreeViewItemObject CreateViewItemObject(System.Object data, ICubeXTreeViewItemBase parent)
			{
				return (new CTreeViewItemObject(data, parent));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="root">Данные корневого узла</param>
			/// <param name="creator">Конструктор для создания узлов</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CTreeViewItemObject Build(System.Object root)
			{
				CTreeViewItemObject node_root_view = Build<System.Object, CTreeViewItemObject>(root, null, 
					CreateViewItemObject);
				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="root">Данные корневого узла</param>
			/// <param name="creator">Конструктор для создания узлов</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TViewItem Build<TData, TViewItem>(TData root, Func<TData, ICubeXTreeViewItemBase, 
				TViewItem> creator) where TData : class
									where TViewItem : TreeViewItemBase <TData>
			{
				TViewItem node_root_view = Build<TData, TViewItem>(root, null, creator);
				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="data">Данные узла дерева</param>
			/// <param name="parent">Родительский узел отображения</param>
			/// <param name="creator">Конструктор для создания узлов</param>
			/// <returns>Узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TViewItem Build<TData, TViewItem>(TData data, ICubeXTreeViewItemBase parent,
				Func<TData, ICubeXTreeViewItemBase,	TViewItem> creator) where TData : class
																		where TViewItem : TreeViewItemBase<TData>
			{
				TViewItem node_root_view = creator(data, parent);
				if (parent != null)
				{
					parent.IChildrenView.Add(node_root_view);
					if(data is ICubeXTreeViewItemOwner view_owner)
					{
						view_owner.OwnerView = node_root_view;
					}
				}

				// 1) Проверяем в порядке приоритета
				// Если есть поддержка интерфеса для построения используем его
				if(data is ICubeXTreeViewBuilder view_builder)
				{
					Int32 count_child = view_builder.GetCountChildrenNode();
					for (Int32 i = 0; i < count_child; i++)
					{
						TData node_data = (TData)view_builder.GetChildrenNode(i);
						if (node_data != null)
						{
							Build<TData, TViewItem>(node_data, node_root_view, creator);
						}
					}
				}
				else
				{
					if (data is ICubeXModelHierarchyBegin hierarchy_begin)
					{
						Int32 count_child = hierarchy_begin.IModels.Count;
						for (Int32 i = 0; i < count_child; i++)
						{
							TData node_data = (TData)hierarchy_begin.IModels[i];
							if (node_data != null)
							{
								Build<TData, TViewItem>(node_data, node_root_view, creator);
							}
						}
					}
					else
					{
						if (data is ICubeXModelHierarchy hierarchy)
						{
							if (hierarchy.IModels != null)
							{
								Int32 count_child = hierarchy.IModels.Count;
								for (Int32 i = 0; i < count_child; i++)
								{
									TData node_data = (TData)hierarchy.IModels[i];
									if (node_data != null)
									{
										Build<TData, TViewItem>(node_data, node_root_view, creator);
									}
								}
							}
						}
					}
				}

				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="root">Данные корневого узла</param>
			/// <param name="match">Предикат для фильтрования узлов</param>
			/// <param name="creator">Конструктор для создания узлов</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CTreeViewItemObject BuildWithFilter(System.Object root, Predicate<ICubeXModel> match)
			{
				CTreeViewItemObject node_root_view = BuildWithFilter<System.Object, 
					CTreeViewItemObject>(root, null, match, CreateViewItemObject);
				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узлов отображения
			/// </summary>
			/// <param name="root">Данные корневого узла</param>
			/// <param name="match">Предикат для фильтрования узлов</param>
			/// <param name="creator">Конструктор для создания узлов</param>
			/// <returns>Корневой узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TViewItem BuildWithFilter<TData, TViewItem>(TData root, Predicate<ICubeXModel> match,
				Func<TData, ICubeXTreeViewItemBase, TViewItem> creator) where TData : class
																		where TViewItem : TreeViewItemBase<TData>
			{
				TViewItem node_root_view = BuildWithFilter<TData, TViewItem>(root, null, match, creator);
				return (node_root_view);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рекурсивное создание узла отображения
			/// </summary>
			/// <param name="data">Данные узла дерева</param>
			/// <param name="parent">Родительский узел отображения</param>
			/// <param name="match">Предикат для фильтрования узлов</param>
			/// <param name="creator">Конструктор для создания узлов</param>
			/// <returns>Узел отображения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TViewItem BuildWithFilter<TData, TViewItem>(TData data, ICubeXTreeViewItemBase parent,
				Predicate<ICubeXModel> match, Func<TData, ICubeXTreeViewItemBase, TViewItem> creator)
				where TData : class
				where TViewItem : TreeViewItemBase<TData>
			{
				TViewItem node_root_view = creator(data, parent);
				if (data is ICubeXCollectionModel collection_model)
				{
					if (parent != null)
					{
						if (collection_model.FiltredModel(match))
						{
							parent.IChildrenView.Add(node_root_view);
							if (data is ICubeXTreeViewItemOwner view_owner)
							{
								view_owner.OwnerView = node_root_view;
							}
						}
					}

					if (collection_model.IModels != null && collection_model.IModels.Count > 0)
					{
						for (Int32 i = 0; i < collection_model.IModels.Count; i++)
						{
							TData node_data = (TData)collection_model.IModels[i];
							if (node_data != null)
							{
								BuildWithFilter<TData, TViewItem>(node_data, node_root_view, match, creator);
							}
						}
					}
				}

				return (node_root_view);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================