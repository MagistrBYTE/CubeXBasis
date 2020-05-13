//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryDatabase.cs
*		Интерфейс репозитория представляющего собой базу данных.
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
using System.ComponentModel;
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
		/// Интерфейс репозитория представляющего собой базу данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface IRepositoryDatabase : ICubeXRepositoryMultiple
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя базы данных
			/// </summary>
			String DatabaseName { get; }

			/// <summary>
			/// Логин для входа
			/// </summary>
			String Login { get; }

			/// <summary>
			/// Пароль для входа
			/// </summary>
			String Password { get; }

			/// <summary>
			/// Соединение с базой данных
			/// </summary>
			DbConnection IConnection { get; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для реализации репозитория представляющего собой базу данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public abstract class RepositoryDatabase : CModel, IRepositoryDatabase
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя схемы для получения информации о таблицах
			/// </summary>
			public const String SCHEME_TABLE_NAME = "Tables";

			/// <summary>
			/// Имя схемы для получения информации о столбцах
			/// </summary>
			public const String SCHEME_COLUMN_NAME = "Columns";

			/// <summary>
			/// Имя схемы для получения информации о внешних ключах
			/// </summary>
			public const String SCHEME_FOREIGN_KEYS_NAME = "Foreign Keys";

			/// <summary>
			/// Имя схемы для получения информации о столбцах связанных с внешними ключами
			/// </summary>
			public const String SCHEME_FOREIGN_KEYS_COLUMNS_NAME = "Foreign Key Columns";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mDatabaseName;
			protected internal String mLogin;
			protected internal String mPassword;
			protected internal String mUniqueID;
			protected internal String mSourceName;
			protected internal Boolean mIsEditable;
			protected internal DataSet mDataSet;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя базы данных
			/// </summary>
			public String DatabaseName
			{
				get { return (mDatabaseName); }
				set
				{ 
					mDatabaseName = value; 
				}
			}

			/// <summary>
			/// Логин для входа
			/// </summary>
			public String Login
			{
				get { return (mLogin); }
				set
				{
					mLogin = value;
				}
			}

			/// <summary>
			/// Пароль для входа
			/// </summary>
			public String Password
			{
				get { return (mPassword); }
				set
				{
					mPassword = value;
				}
			}

			/// <summary>
			/// Соединение с базой данных
			/// </summary>
			public abstract DbConnection IConnection { get; }
			#endregion

			#region ======================================= СВОЙСТВА ICubeXRepository =================================
			/// <summary>
			/// Глобальный строковый идентификатор репозитория
			/// </summary>
			[Browsable(false)]
			public String UniqueID
			{
				get { return (mUniqueID); }
				set
				{
					mUniqueID = value;
				}
			}

			/// <summary>
			/// Имя источника данных
			/// </summary>
			[Browsable(false)]
			public String SourceName
			{
				get { return (mSourceName); }
				set
				{
					mSourceName = value;
				}
			}

			/// <summary>
			/// Схема организации данных
			/// </summary>
			[Browsable(false)]
			public CSchemeFlatData Scheme { get; set; }

			/// <summary>
			/// Тип репозитория с точки зрения модификации данных
			/// </summary>
			[Browsable(false)]
			public TRepositoryModify ModifyType
			{
				get { return (TRepositoryModify.Dynamic); }
			}

			/// <summary>
			/// Тип репозитория с точки зрения организации данных
			/// </summary>
			[Browsable(false)]
			public TRepositoryOrganization OrganizationType
			{
				get { return (TRepositoryOrganization.Raw); }
			}

			/// <summary>
			/// Обобщённый список записей
			/// </summary>
			[Browsable(false)]
			public IList IRecords
			{
				get { return (null); }
			}

			/// <summary>
			/// Статус редактирования записей/данных репозитория 
			/// </summary>
			[Browsable(false)]
			public Boolean IsEditable
			{
				get { return (mIsEditable); }
				set { mIsEditable = true; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXRepositoryMultiple =========================
			/// <summary>
			/// Обобщённый набор данных
			/// </summary>
			public DataSet DataSet
			{
				get { return (mDataSet); }
			}

			/// <summary>
			/// Словарь множественных данных
			/// </summary>
			/// <remarks>
			/// Не поддерживается
			/// </remarks>
			public Dictionary<String, IList> IDictionaryData
			{
				get { return (null); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected RepositoryDatabase()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			protected RepositoryDatabase(String name)
				: base(name)
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXRepository ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Подключение репозитория к существующему набору данных с их загрузкой
			/// </summary>
			/// <param name="context">Контекст данных</param>
			/// <param name="is_loaded">Следует ли загружать все данные</param>
			/// <returns>Статус успешности подключения</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Boolean Connect(System.Object context, Boolean is_loaded)
			{
				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление репозитория к состоянию последнего сохранения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Restore()
			{
				if (IConnection != null && mDataSet != null && mDataSet.Tables != null && mDataSet.Tables.Count > 0)
				{
					mDataSet.EnforceConstraints = false;
					for (Int32 i = 0; i < mDataSet.Tables.Count; i++)
					{
						DataTable table = mDataSet.Tables[i];

						if(table.Rows.Count > 0)
						{
							table.Rows.Clear();
						}

						// Запрос на извлечение всех данных
						String query = "SELECT * FROM " + table.TableName;

						// Получаем адаптер
						var adapter = GetDataAdapter(query);

						// Заполняем набор данных
						adapter.Fill(mDataSet, table.TableName);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение изменения в репозитории
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Save()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Закрытие репозитория и освобождения всех данных связанных с ним
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Close()
			{
				if(IConnection != null && IConnection.State == ConnectionState.Open)
				{
					IConnection.Close();
				}
			}
			#endregion

			#region ======================================= РАБОТА С ЗАПИСЬЮ ICubeXRepository =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить запись
			/// </summary>
			/// <param name="items">Элементы записи</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void AddRecord(params System.Object[] items)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление записи
			/// </summary>
			/// <param name="index">Индекс записи</param>
			/// <param name="items">Элементы записи</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateRecord(Int32 index, params System.Object[] items)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса записи
			/// </summary>
			/// <param name="items">Элементы записи</param>
			/// <returns>Индекс записи</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual Int32 FindRecord(params System.Object[] items)
			{
				return (-1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление записи
			/// </summary>
			/// <param name="index">Индекс записи</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void DeleteRecord(Int32 index)
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение конкретного адаптера данных по указанному запросу
			/// </summary>
			/// <param name="query">Запрос</param>
			/// <returns>Адаптер данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public abstract DbDataAdapter GetDataAdapter(String query);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка таблиц для данной базы данных
			/// </summary>
			/// <returns>Список таблиц</returns>
			//---------------------------------------------------------------------------------------------------------
			public abstract List<String> GetListTable();

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение схемы набора данных по указанному списку таблиц
			/// </summary>
			/// <param name="tables">Список таблиц</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void FillSchemeDataSet(IList<String> tables)
			{
				// Если есть таблицы
				if (tables.Count > 0)
				{
					for (Int32 i = 0; i < tables.Count; i++)
					{
						// Запрос на извлечение данных
						String query = "SELECT * FROM " + tables[i];

						var adapter = GetDataAdapter(query);

						// Заполняем схему
						adapter.FillSchema(mDataSet, SchemaType.Source, tables[i]);
					}

					FillCommentTablesAndColumns();

					FillForeignKeys();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение комментариев для таблиц и столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void FillCommentTablesAndColumns()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение внешних ключей таблицы
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void FillForeignKeys()
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