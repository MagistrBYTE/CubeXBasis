//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryExcel.cs
*		Репозиторий представляющий собой книгу Excel.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
#if USE_EXCEL
using ExcelDataReader;
using ExcelDataReader.Exceptions;
using ExcelDataReader.Core;
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
		/// Репозиторий представляющий собой книгу Excel
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CRepositoryExcel : RepositoryFile, ICubeXRepositoryMultiple
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal IExcelDataReader mDataReader;
			protected internal DataSet mDataSet;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Читатель данных 
			/// </summary>
			public IExcelDataReader DataReader
			{
				get { return (mDataReader); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXRepository =================================
			/// <summary>
			/// Обобщённый список записей
			/// </summary>
			/// <remarks>
			/// Не поддерживается
			/// </remarks>
			public override IList IRecords
			{
				get { return (null); }
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
			public CRepositoryExcel()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			public CRepositoryExcel(String name)
				: base(name, TRepositoryOrganization.Raw)
			{
				mModifyType = TRepositoryModify.Static;
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
				return (base.Connect(context, is_loaded));
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
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Закрытие репозитория и освобождения всех данных связанных с ним
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void Close()
			{

			}
			#endregion

			#region ======================================= РАБОТА С ЗАПИСЬЮ ICubeXRepository =========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить запись
			/// </summary>
			/// <param name="items">Элементы записи</param>
			//---------------------------------------------------------------------------------------------------------
			public override void AddRecord(params System.Object[] items)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление записи
			/// </summary>
			/// <param name="index">Индекс записи</param>
			/// <param name="items">Элементы записи</param>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdateRecord(Int32 index, params System.Object[] items)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск индекса записи
			/// </summary>
			/// <param name="items">Элементы записи</param>
			/// <returns>Индекс записи</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 FindRecord(params System.Object[] items)
			{
				return (-1);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление записи
			/// </summary>
			/// <param name="index">Индекс записи</param>
			//---------------------------------------------------------------------------------------------------------
			public override void DeleteRecord(Int32 index)
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ ДАННЫХ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из файла на основе типа данных и их организации
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void LoadData(String file_name)
			{
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				Encoding encoding = Encoding.GetEncoding(1252);

				using (var stream = File.Open(file_name, FileMode.Open, FileAccess.Read))
				{
					var reader = ExcelReaderFactory.CreateReader(stream, new ExcelReaderConfiguration()
					{
						// Gets or sets the encoding to use when the input XLS lacks a CodePage
						// record, or when the input CSV lacks a BOM and does not parse as UTF8. 
						// Default: cp1252 (XLS BIFF2-5 and CSV only)
						FallbackEncoding = encoding,

						// Gets or sets the password used to open password protected workbooks.
						//Password = "password",

						// Gets or sets an array of CSV separator candidates. The reader 
						// autodetects which best fits the input data. Default: , ; TAB | # 
						// (CSV only)
						AutodetectSeparators = new char[] { ',', ';', '\t', '|', '#' },

						// Gets or sets a value indicating whether to leave the stream open after
						// the IExcelDataReader object is disposed. Default: false
						LeaveOpen = false,

						// Gets or sets a value indicating the number of rows to analyze for
						// encoding, separator and field count in a CSV. When set, this option
						// causes the IExcelDataReader.RowCount property to throw an exception.
						// Default: 0 - analyzes the entire file (CSV only, has no effect on other
						// formats)
						AnalyzeInitialCsvRows = 0,
					});

					mDataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
					{
						// Gets or sets a value indicating whether to set the DataColumn.DataType 
						// property in a second pass.
						UseColumnDataType = true,

						// Gets or sets a callback to determine whether to include the current sheet
						// in the DataSet. Called once per sheet before ConfigureDataTable.
						FilterSheet = (tableReader, sheetIndex) => true,

						// Gets or sets a callback to obtain configuration options for a DataTable. 
						ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
						{
							// Gets or sets a value indicating the prefix of generated column names.
							EmptyColumnNamePrefix = "Column",

							// Gets or sets a value indicating whether to use a row from the data as column names.
							UseHeaderRow = true,

							// Gets or sets a callback to determine which row is the header row. 
							// Only called when UseHeaderRow = true.
							ReadHeaderRow = (rowReader) => 
							{
								// F.ex skip the first row and use the 2nd row as column headers:
								//rowReader.Read();
							},

							// Gets or sets a callback to determine whether to include the 
							// current row in the DataTable.
							FilterRow = (rowReader) => 
							{
								return true;
							},

							// Gets or sets a callback to determine whether to include the specific
							// column in the DataTable. Called once per column after reading the 
							// headers.
							FilterColumn = (rowReader, columnIndex) => 
							{
								return true;
							}
						}
					});

					XLogger.LogInfo(mDataSet);
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ ДАННЫХ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в файл на основе типа данных и их организации
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void SaveData(String file_name)
			{

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