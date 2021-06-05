//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Древовидные(иерархические) структуры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTreeNodeView.cs
*		Отображения дерева для редактора Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEditor;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Editor
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для отображения дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTreeViewGUI : CTreeViewBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected Rect mControlRect;
			protected Single mStartDrawY;
			protected Single mHeight;
			protected Int32 mControlID;
			protected Boolean mIsCheckableView;
			protected Boolean mIsIconableView;
			protected Func<ICubeXTreeNodeView, Texture> mIconDelegate;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Отображение флажка выбора элемента узла
			/// </summary>
			public Boolean IsCheckableView
			{
				get { return (mIsCheckableView); }
				set
				{
					if (mIsCheckableView != value)
					{
						mIsCheckableView = value;
					}
				}
			}

			/// <summary>
			/// Отображение иконки элемента узла
			/// </summary>
			public Boolean IsIconableView
			{
				get { return (mIsIconableView); }
				set
				{
					if (mIsIconableView != value)
					{
						mIsIconableView = value;
					}
				}
			}

			/// <summary>
			/// Делегат для представления иконки в зависимости от типа узла отображения
			/// </summary>
			public Func<ICubeXTreeNodeView, Texture> IconDelegate
			{
				get { return (mIconDelegate); }
				set
				{
					if (mIconDelegate != value)
					{
						mIconDelegate = value;
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewGUI()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="root">Корневой узел отображения</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewGUI(CTreeNodeView root)
			{
				mRoot = root;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование дерева в автоматическом макете
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void DrawTreeLayout()
			{
				mHeight = 0;
				mStartDrawY = 0;
				mRoot.Visit(OnGetLayoutHeight);

				mControlRect = EditorGUILayout.GetControlRect(false, mHeight);
				mControlID = GUIUtility.GetControlID(FocusType.Passive, mControlRect);
				mRoot.Visit(OnDrawRow);
			}
			#endregion

			#region ======================================= МЕТОДЫ РИСОВАНИЯ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты узла дерева
			/// </summary>
			/// <param name="node_view">Узел дерева</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual Single GetRowHeight(ICubeXTreeNodeView node_view)
			{
				return (EditorGUIUtility.singleLineHeight);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вычисление высоты дерева
			/// </summary>
			/// <param name="node_view">Узел дерева</param>
			/// <returns>Статус продолжения вычисления высоты дерева</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual Boolean OnGetLayoutHeight(ICubeXTreeNodeView node_view)
			{
				if (node_view.Data == null) return true;

				mHeight += GetRowHeight(node_view);
				return (node_view.IsExpanded);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование узла дерева
			/// </summary>
			/// <param name="node_view">Узел дерева</param>
			/// <returns>Статус раскрытия данного узла</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual Boolean OnDrawRow(ICubeXTreeNodeView node_view)
			{
				if (node_view.Data == null) return true;

				Single row_indent = 16 * node_view.Level;
				Single row_height = GetRowHeight(node_view);

				Rect row_rect = new Rect(2, mControlRect.y + mStartDrawY, mControlRect.width, row_height);
				Rect indent_rect = new Rect(row_indent, mControlRect.y + mStartDrawY, mControlRect.width - row_indent, row_height);

				// render
				if (mSelectedNode == node_view)
				{
					GUI.Box(row_rect, GUIContent.none, XEditorStyles.BOX_FLOW_NODE_0_ON);
				}

				OnDrawTreeNode(indent_rect, node_view, mSelectedNode == node_view, false);

				// test for events
				EventType event_type = Event.current.GetTypeForControl(mControlID);
				if (event_type == EventType.MouseUp && row_rect.Contains(Event.current.mousePosition))
				{
					mSelectedNode = node_view as CTreeNodeView;

					GUI.changed = true;
					Event.current.Use();
				}

				mStartDrawY += row_height;
				return node_view.IsExpanded;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование данных узла дерева
			/// </summary>
			/// <param name="rect">Прямоугольник вывода</param>
			/// <param name="node_view">Узел дерева</param>
			/// <param name="selected">Статус выбора узла</param>
			/// <param name="focus">Статус фокуса узла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void OnDrawTreeNode(Rect rect, ICubeXTreeNodeView node_view, Boolean selected, Boolean focus)
			{
				GUIContent label_сontent = new GUIContent(node_view.Data.ToString());

				if(mIsIconableView && mIconDelegate != null)
				{
					label_сontent.image = mIconDelegate(node_view);
				}

				if (!node_view.IsLeaf)
				{
					Rect rect_foldout = new Rect(rect.x, rect.y, 12, rect.height);
					node_view.IsExpanded = EditorGUI.Foldout(rect_foldout, node_view.IsExpanded, GUIContent.none);
				}

				if(mIsCheckableView)
				{
					Rect rect_checked = new Rect(rect.x + 12, rect.y + 1, 12, rect.height);
					node_view.IsChecked = EditorGUI.ToggleLeft(rect_checked, GUIContent.none, node_view.IsChecked);

					Rect rect_value = new Rect(rect.x + 24, rect.y, rect.width - 24, rect.height);
					EditorGUI.LabelField(rect_value, label_сontent, selected ? EditorStyles.whiteBoldLabel : EditorStyles.label);
				}
				else
				{
					Rect rect_value = new Rect(rect.x + 14, rect.y, rect.width - 14, rect.height);
					EditorGUI.LabelField(rect_value, label_сontent, selected ? EditorStyles.whiteBoldLabel : EditorStyles.label);
				}
			}
			#endregion
		}
	}
}
//=====================================================================================================================
#endif
//=====================================================================================================================