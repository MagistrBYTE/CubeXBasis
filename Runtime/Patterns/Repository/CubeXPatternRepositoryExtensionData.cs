//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryExtensionData.cs
*		Методы расширения наl стандартной моделью управления данными ADO.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Schema;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternRepository
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="ForeignKeyConstraint"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionForeignKeyConstraint
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка указанного столбца на то что он является внешним ключом
			/// </summary>
			/// <param name="this">Внешний ключ</param>
			/// <param name="column">Проверяемый столбец</param> 
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsForeignKey(this ForeignKeyConstraint @this, DataColumn column)
			{
				Boolean has_foreign_key_related = false;

				if (@this.Columns != null && @this.Columns.Length > 0)
				{
					Boolean has_foreign_key = @this.Columns.Contains(column);
					if (has_foreign_key)
					{
						if (@this.RelatedColumns != null && @this.RelatedColumns.Length > 0)
						{
							has_foreign_key_related = true;
						}
					}
				}

				return (has_foreign_key_related);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка указанного имени столбца на то что он является внешним ключом
			/// </summary>
			/// <param name="this">Внешний ключ</param>
			/// <param name="column_name">Имя столбца</param> 
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsForeignKey(this ForeignKeyConstraint @this, String column_name)
			{
				Boolean has_foreign_key_related = false;
				if (@this.Columns != null && @this.Columns.Length > 0)
				{
					for (Int32 i = 0; i < @this.Columns.Length; i++)
					{
						DataColumn column = @this.Columns[i];
						if (column != null && column.ColumnName == column_name)
						{
							if (@this.RelatedColumns != null && @this.RelatedColumns.Length > 0)
							{
								has_foreign_key_related = true;
								break;
							}
						}
					}
				}

				return (has_foreign_key_related);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="DataColumn"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionDataColumn
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка столбца на то что он является внешним ключом
			/// </summary>
			/// <param name="this">Столбец данных</param>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean IsForeignKey(this DataColumn @this)
			{
				DataTable table = @this.Table;
				return (table.CheckForeignKey(@this));
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения для типа <see cref="DataTable"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionDataTable
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка указанного столбца на то что он является внешним ключом
			/// </summary>
			/// <param name="this">Столбец данных</param>
			/// <param name="column">Проверяемый столбец</param> 
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean CheckForeignKey(this DataTable @this, DataColumn column)
			{
				Boolean has_foreign_key = false;
				if (@this != null)
				{
					if (@this.Constraints != null && @this.Constraints.Count > 0)
					{
						for (Int32 i = 0; i < @this.Constraints.Count; i++)
						{
							if (@this.Constraints[i] is ForeignKeyConstraint foreign_key)
							{
								has_foreign_key = foreign_key.IsForeignKey(column);
								if (has_foreign_key)
								{
									break;
								}
							}
						}
					}
				}

				return (has_foreign_key);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка указанного имени столбца на то что он является внешним ключом
			/// </summary>
			/// <param name="this">Столбец данных</param>
			/// <param name="column_name">Имя столбца</param> 
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean CheckForeignKey(this DataTable @this, String column_name)
			{
				Boolean has_foreign_key = false;
				if (@this != null)
				{
					if (@this.Constraints != null && @this.Constraints.Count > 0)
					{
						for (Int32 i = 0; i < @this.Constraints.Count; i++)
						{
							if (@this.Constraints[i] is ForeignKeyConstraint foreign_key)
							{
								has_foreign_key = foreign_key.IsForeignKey(column_name);
								if (has_foreign_key)
								{
									break;
								}
							}
						}
					}
				}

				return (has_foreign_key);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================