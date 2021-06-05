//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryDatabase.cs
*		Репозиторий представляющий собой базу данных MySql
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//---------------------------------------------------------------------------------------------------------------------
#if USE_MYSQL
using MySql.Data.MySqlClient;
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
		/// Репозиторий представляющий собой базу данных MySql
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CRepositoryDatabaseMySql : RepositoryDatabase
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal MySqlConnection mConnection;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Соединение с базой данных
			/// </summary>
			public MySqlConnection Connection
			{
				get { return (mConnection); }
			}

			/// <summary>
			/// Соединение с базой данных
			/// </summary>
			public override DbConnection IConnection
			{
				get { return (mConnection); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CRepositoryDatabaseMySql()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			public CRepositoryDatabaseMySql(String name)
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
			public override Boolean Connect(System.Object context, Boolean is_loaded)
			{
				String connect_string = null;

				// Проверяем аргументы
				if (context != null && context.GetType() == typeof(String))
				{
					String connect_str = context.ToString();
					if (connect_str.IsExists())
					{
						connect_string = connect_str;

						MySqlConnectionStringBuilder string_builder = new MySqlConnectionStringBuilder(connect_string);
						mSourceName = string_builder.Server;
						mDatabaseName = string_builder.Database;
						mLogin = string_builder.UserID;
					}
				}

				// Если строка запроса пустая то формируем ее
				if (String.IsNullOrEmpty(connect_string))
				{
					connect_string = String.Format("server={0};user={1};database={2};password={3};", mSourceName, mLogin,
						mDatabaseName, mPassword);
				}

				// Соединяем
				try
				{
					mConnection = new MySqlConnection(connect_string);
					mConnection.Open();

					// Создаем набор данных
					mDataSet = new DataSet();

					// Получаем таблицы
					List<String> tables = GetListTable();

					// Заполняем схему
					FillSchemeDataSet(tables);

					// Если надо загружаем еще и данные
					if (is_loaded)
					{
						Restore();
					}

					return (true);

				}
				catch (Exception exc)
				{
					XLogger.LogException(exc);
					return (false);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление репозитория к состоянию последнего сохранения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Restore()
			{
				base.Restore();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение изменения в репозитории
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Save()
			{
				if (IConnection != null && mDataSet != null)
				{
					// Обновляем базу данных
					for (Int32 i = 0; i < mDataSet.Tables.Count; i++)
					{
						String table_name = mDataSet.Tables[i].TableName;

						// Получаем адаптер
						string sql = "SELECT * FROM " + table_name;
						var adapter = new MySqlDataAdapter(sql, mConnection);

						// создаем объект SqlCommandBuilder
						MySqlCommandBuilder commandBuilder = new MySqlCommandBuilder(adapter);
						commandBuilder.ConflictOption = ConflictOption.OverwriteChanges;

						Int32 count = adapter.Update(mDataSet, table_name);
						XLogger.LogInfoFormat("Таблица <{0}>, изменений <{1}>", table_name, count);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Закрытие репозитория и освобождения всех данных связанных с ним
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Close()
			{
				base.Close();
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
			public override DbDataAdapter GetDataAdapter(String query)
			{
				return (new MySqlDataAdapter(query, mConnection));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка таблиц для данной базы данных
			/// </summary>
			/// <returns>Список таблиц</returns>
			//---------------------------------------------------------------------------------------------------------
			public override List<String> GetListTable()
			{
				List<String> tables = new List<String>();

				// Получаем все таблицы
				String query = "SELECT table_name FROM information_schema.tables WHERE table_type = 'BASE TABLE' AND table_schema = '" + mConnection.Database + "'";
				
				MySqlCommand command_get_all_table = new MySqlCommand(query, mConnection);

				using (var reader = command_get_all_table.ExecuteReader())
				{
					while (reader.Read())
					{
						// Имя таблицы
						var table_name = reader.GetString(0);
						tables.Add(table_name);
					}
				}

				return (tables);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение комментариев для таблиц и столбцов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void FillCommentTablesAndColumns()
			{
				for (Int32 it = 0; it < mDataSet.Tables.Count; it++)
				{
					DataTable table = mDataSet.Tables[it];

					String query_table = String.Format("SELECT table_comment FROM information_schema.tables WHERE table_name = '{0}'", table.TableName);
					MySqlCommand command_get_table_comment = new MySqlCommand(query_table, mConnection);
					using (var reader = command_get_table_comment.ExecuteReader())
					{
						while (reader.Read())
						{
							// Комментарий
							table.DisplayExpression = "'" + reader.GetString(0) + "'";
							
						}
					}

					for (Int32 ic = 0; ic < table.Columns.Count; ic++)
					{
						DataColumn column = table.Columns[ic];

						String query_column = String.Format("SELECT column_comment FROM information_schema.columns WHERE table_name = '{0}' AND column_name = '{1}'", table.TableName,
							column.ColumnName);
						MySqlCommand command_get_column_comment = new MySqlCommand(query_column, mConnection);
						using (var reader = command_get_column_comment.ExecuteReader())
						{
							while (reader.Read())
							{
								// Комментарий
								column.Caption = reader.GetString(0);
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Заполнение внешних ключей таблицы
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void FillForeignKeys()
			{
				//for (Int32 i = 0; i < mDataSet.Tables.Count; i++)
				//{
				//	mDataSet.Tables[i].Constraints.Clear();
				//}

				// Получаем таблицу
				DataTable table_foreign_keys = mConnection.GetSchema(SCHEME_FOREIGN_KEYS_COLUMNS_NAME);
				for (Int32 ir = 0; ir < table_foreign_keys.Rows.Count; ir++)
				{
					// Дочерние элементы
					String table_name = table_foreign_keys.Rows[ir]["TABLE_NAME"].ToString();
					String column_name = table_foreign_keys.Rows[ir]["COLUMN_NAME"].ToString();

					// Родительские элементы
					String parent_table_name = table_foreign_keys.Rows[ir]["REFERENCED_TABLE_NAME"].ToString();
					String parent_column_name = table_foreign_keys.Rows[ir]["REFERENCED_COLUMN_NAME"].ToString();

					// Получаем таблицу
					DataTable current_table = mDataSet.Tables[table_name];
					if (current_table != null)
					{
						// Получаем столбец значения которого зависят
						DataColumn child = current_table.Columns[column_name];
						if (child != null)
						{
							// Получаем главную таблицу и ее главный столбец
							DataTable parent_table = mDataSet.Tables[parent_table_name];
							if (parent_table != null)
							{
								DataColumn parent = parent_table.Columns[parent_column_name];
								if (parent != null)
								{
									// Создаем ограничение по внешнему ключу
									ForeignKeyConstraint constraint = new ForeignKeyConstraint(parent, child);

									// Добавлем в таблицу
									current_table.Constraints.Add(constraint);
								}
							}
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
#endif
//=====================================================================================================================