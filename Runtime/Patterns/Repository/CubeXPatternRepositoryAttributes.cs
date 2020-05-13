//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryAttributes.cs
*		Атрибуты для работы с репозиториями данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
		/// Атрибут для получения значения из репозитория данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public class CubeXRepositoryDataAttribute : Attribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly String mRepositoryName;
			internal readonly String mTableName;
			internal readonly String mColumnName;
			internal readonly String mFilterRecord;
			internal readonly String mFilterProperty;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя репозитория 
			/// </summary>
			public String RepositoryName
			{
				get { return mRepositoryName; }
			}

			/// <summary>
			/// Имя таблицы
			/// </summary>
			/// <remarks>
			/// Может быть null
			/// </remarks>
			public String TableName
			{
				get { return mTableName; }
			}

			/// <summary>
			/// Имя столбца таблицы или репозитория
			/// </summary>
			/// <remarks>
			/// Может быть null
			/// </remarks>
			public String ColumnName
			{
				get { return mColumnName; }
			}

			/// <summary>
			/// Строка фильтра
			/// </summary>
			public String FilterRecord
			{
				get { return mFilterRecord; }
			}

			/// <summary>
			/// Свойство из которого берется значение для строки фильтра
			/// </summary>
			public String FilterProperty
			{
				get { return mFilterRecord; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDataAttribute()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="repository_name">Имя репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDataAttribute(String repository_name)
			{
				mRepositoryName = repository_name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="repository_name">Имя репозитория</param>
			/// <param name="table_name">Имя таблицы</param>
			/// <param name="column_name">Имя столбца таблицы или репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDataAttribute(String repository_name, String table_name, String column_name)
			{
				mRepositoryName = repository_name;
				mTableName = table_name;
				mColumnName = column_name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="repository_name">Имя репозитория</param>
			/// <param name="table_name">Имя таблицы</param>
			/// <param name="column_name">Имя столбца таблицы или репозитория</param>
			/// <param name="filter_record">Строка фильтра</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDataAttribute(String repository_name, String table_name, String column_name,
				String filter_record)
			{
				mRepositoryName = repository_name;
				mTableName = table_name;
				mColumnName = column_name;
				mFilterRecord = filter_record;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="repository_name">Имя репозитория</param>
			/// <param name="table_name">Имя таблицы</param>
			/// <param name="column_name">Имя столбца таблицы или репозитория</param>
			/// <param name="filter_record">Строка фильтра</param>
			/// <param name="filter_property">Свойство из которого берется значение для строки фильтра</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXRepositoryDataAttribute(String repository_name, String table_name, String column_name,
				String filter_record, String filter_property)
			{
				mRepositoryName = repository_name;
				mTableName = table_name;
				mColumnName = column_name;
				mFilterRecord = filter_record;
				mFilterProperty = filter_property;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================