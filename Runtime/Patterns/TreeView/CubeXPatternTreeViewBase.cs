﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема отображения иерархических данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternTreeViewBase.cs
*		Определение общих типов и структур данных подсистемы отображения иерархических данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CorePatternTreeNodeView Подсистема отображения иерархических данных
		//! Подсистема отображения иерархических данных реализует промежуточный слой данных – узлов которые позволяют
		//! управлять видимостью данных, обеспечивают их сортировка, группировку, фильтрацию, позволять выбирать
		//! элементы и производить над ними операции.
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для хранения ссылки на узел отображения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTreeViewItemOwner
		{
			/// <summary>
			/// Узел отображения
			/// </summary>
			ICubeXTreeViewItemBase OwnerView { get; set; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для построение визуальной модели узла дерева
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTreeViewBuilder
		{
			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение количества дочерних узлов
			/// </summary>
			/// <returns>Количество дочерних узлов</returns>
			//---------------------------------------------------------------------------------------------------------
			Int32 GetCountChildrenNode();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дочернего узла по индексу
			/// </summary>
			/// <param name="index">Индекс дочернего узла</param>
			/// <returns>Дочерней узел</returns>
			//---------------------------------------------------------------------------------------------------------
			System.Object GetChildrenNode(Int32 index);
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================