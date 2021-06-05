//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Древовидные(иерархические) структуры данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTreeNodeViewFileSystem.cs
*		Визуальная модель для отображения дерева представляющего собой элементы файловой системы.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
using System;
using System.IO;
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
		/// Класс для отображения элементы файловой системы
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CTreeViewFileSystem : CTreeViewGUI
		{
			#region ======================================= ДАННЫЕ ====================================================
			private CDirectoryNode mBaseDirectory;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Базовая директория обозревателя
			/// </summary>
			public CDirectoryNode BaseDirectory
			{
				get { return (mBaseDirectory); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewFileSystem()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="base_path">Базовая директория</param>
			//---------------------------------------------------------------------------------------------------------
			public CTreeViewFileSystem(String base_path = XEditorSettings.ASSETS_PATH)
			{
				mBaseDirectory = new CDirectoryNode(new DirectoryInfo(base_path));
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перестроение всего дерева
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ReBuild()
			{
				Build(mBaseDirectory);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Фильтрация данных
			/// </summary>
			/// <remarks>
			/// Метод автоматически вызывается при изменении строки поиска или опции поиска
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public override void OnFilterd()
			{
				if(String.IsNullOrEmpty(mSearchString))
				{
					mRoot = CTreeNodeView.Create(mBaseDirectory);
				}
				else
				{
					mRoot = CTreeNodeView.CreateWithFilter(mBaseDirectory, (ICubeXTreeNode node)=>
					{
						return (node.Name.Contains(mSearchString));
					});

					mRoot.Expanded();
				}
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
			protected override Single GetRowHeight(ICubeXTreeNodeView node_view)
			{
				if (node_view.Data is CDirectoryNode)
				{
					return (EditorGUIUtility.singleLineHeight + 2);
				}
				else
				{
					return (EditorGUIUtility.singleLineHeight + 2);
				}
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
			protected override void OnDrawTreeNode(Rect rect, ICubeXTreeNodeView node_view, Boolean selected, Boolean focus)
			{
				GUIContent label_сontent = new GUIContent(node_view.Data.ToString());
				if(node_view.Data is CDirectoryNode)
				{
					label_сontent.image = EditorGUIUtility.IconContent(XEditorStyles.ICON_FOLDER).image;
				}
				else
				{
					if (node_view.Data is CFileNode)
					{
						CFileNode file_node = node_view.Data as CFileNode;
						label_сontent.image = XEditorInspector.GetIconForFile(file_node.Info.Extension);
					}
				}

				if (!node_view.IsLeaf)
				{
					Rect rect_foldout = new Rect(rect.x, rect.y, 12, rect.height);
					node_view.IsExpanded = EditorGUI.Foldout(rect_foldout, node_view.IsExpanded, GUIContent.none);
				}

				Rect rect_checked = new Rect(rect.x + 12, rect.y + 1, 12, rect.height);
				node_view.IsChecked = EditorGUI.ToggleLeft(rect_checked, GUIContent.none, node_view.IsChecked);

				Rect rect_value = new Rect(rect.x + 24, rect.y, rect.width - 24, rect.height);
				EditorGUI.LabelField(rect_value, label_сontent, selected ? EditorStyles.whiteBoldLabel : EditorStyles.label);
			}
			#endregion
		}
	}
}
//=====================================================================================================================
#endif
//=====================================================================================================================