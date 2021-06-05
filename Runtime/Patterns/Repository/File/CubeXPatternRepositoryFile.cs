//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryFile.cs
*		Репозиторий представляющий собой отдельный файл данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;
using System.Data;
using System.Xml;
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
		/// Интерфейс репозитория для работы с отдельным физическим файлом
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXRepositoryFile : ICubeXRepository
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя физического файла
			/// </summary>
			String FileName { get; set; }

			/// <summary>
			/// Путь до файла
			/// </summary>
			String PathFile { get; set; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс репозитория для работы объектом указанного типа отдельного физического файла
		/// </summary>
		/// <typeparam name="TData">Тип объекта</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXRepositoryFile<TData> : ICubeXRepositoryFile, ICubeXRepository<TData>
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс репозитория с отдельным физическим файлом
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public abstract class RepositoryFile : CModel, ICubeXRepositoryFile
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// Константы для информирования об изменении свойств
			//
			protected static readonly PropertyChangedEventArgs PropertyArgsFileName = new PropertyChangedEventArgs(nameof(FileName));
			protected static readonly PropertyChangedEventArgs PropertyArgsSourceName = new PropertyChangedEventArgs(nameof(SourceName));
			protected static readonly PropertyChangedEventArgs PropertyArgsPathFile = new PropertyChangedEventArgs(nameof(PathFile));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mUniqueID;
			protected internal String mFileName;
			protected internal String mPathFile;
			protected internal TRepositoryOrganization mOrganizationType;
			protected internal TRepositoryModify mModifyType;
			protected internal Boolean mIsEditable;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя физического файла
			/// </summary>
			public String FileName
			{
				get { return (mFileName); }
				set
				{
					mFileName = value;
					NotifyPropertyChanged(PropertyArgsFileName);
					NotifyPropertyChanged(PropertyArgsSourceName);
					RaiseFileNameChanged();
				}
			}

			/// <summary>
			/// Путь до файла
			/// </summary>
			public String PathFile
			{
				get { return (mPathFile); }
				set
				{
					mPathFile = value;
					NotifyPropertyChanged(PropertyArgsPathFile);
					RaisePathFileChanged();
				}
			}
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
			/// <remarks>
			/// Совпадает с именем файла
			/// </remarks>
			[Browsable(false)]
			public String SourceName
			{
				get { return (mFileName); }
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
				get { return (mModifyType); }
			}

			/// <summary>
			/// Тип репозитория с точки зрения организации данных
			/// </summary>
			[Browsable(false)]
			public TRepositoryOrganization OrganizationType
			{
				get { return (mOrganizationType); }
			}

			/// <summary>
			/// Обобщённый список записей
			/// </summary>
			[Browsable(false)]
			public abstract IList IRecords { get; }

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

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected RepositoryFile()
				: this(String.Empty, TRepositoryOrganization.Object)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="organization_type">Тип репозитория с точки зрения организации данных</param>
			//---------------------------------------------------------------------------------------------------------
			protected RepositoryFile(TRepositoryOrganization organization_type)
				: this(String.Empty, organization_type)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			/// <param name="organization_type">Тип репозитория с точки зрения организации данных</param>
			//---------------------------------------------------------------------------------------------------------
			protected RepositoryFile(String name, TRepositoryOrganization organization_type = TRepositoryOrganization.Object)
				: base(name)
			{
				mOrganizationType = organization_type;
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение имени файла.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseFileNameChanged()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение пути до файла.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaisePathFileChanged()
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
				String string_connection = "";
				if (context != null && context.GetType() == typeof(String))
				{
					string_connection = context.ToString();
				}

				if (string_connection.IsExists())
				{
					// Проверяем на существующий файл
					if (File.Exists(string_connection))
					{
						// Это полный путь к файлу
						// Загружаем данные
						if (is_loaded)
						{
							LoadData(string_connection);
						}

						// Сохраняем данные
						PathFile = Path.GetDirectoryName(string_connection);
						FileName = Path.GetFileName(string_connection);

						if (String.IsNullOrEmpty(Name))
						{
							Name = Path.GetFileNameWithoutExtension(string_connection);
						}

						return (true);
					}
				}

				// Будем рассматривать как путь
				String file_name = XFileDialog.Open("Открыть файл", string_connection, GetFileExtension());
				if (file_name.IsExists())
				{
					// Загружаем данные
					if (is_loaded)
					{
						LoadData(file_name);
					}

					// Сохраняем данные
					PathFile = Path.GetDirectoryName(file_name);
					FileName = Path.GetFileName(file_name);

					if (String.IsNullOrEmpty(Name))
					{
						Name = Path.GetFileNameWithoutExtension(file_name);
					}

					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление репозитория к состоянию последнего сохранения
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Restore()
			{
				if (IsEditable)
				{
					String file_name = Path.Combine(mPathFile, FileName);
					if (File.Exists(file_name))
					{
						LoadData(file_name);
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
				if (mIsEditable)
				{
					if (mPathFile.IsExists() == false || mFileName.IsExists() == false)
					{
						String file_name = XFileDialog.Save("Сохранить файл", mPathFile,
							mName.IsExists() ? mName : "Репозиторий", GetFileExtension());
						if (file_name.IsExists())
						{
							SaveData(file_name);
							PathFile = Path.GetDirectoryName(file_name);
							FileName = Path.GetFileName(file_name);
							Name = Path.GetFileNameWithoutExtension(file_name);
						}
					}
					else
					{
						String file_name = Path.Combine(mPathFile, mFileName);
						if (File.Exists(file_name))
						{
							SaveData(file_name);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Закрытие репозитория и освобождения всех данных связанных с ним
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void Close()
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
			/// Получение допустимого расширения для открытия/сохранения файла 
			/// </summary>
			/// <returns>Расширение файла без точки</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual String GetFileExtension()
			{
				return (mOrganizationType == TRepositoryOrganization.Object ? XFileExtension.XML : XFileExtension.TXT);
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ ДАННЫХ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из файла на основе типа данных и их организации
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void LoadData(String file_name)
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ ДАННЫХ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в файл на основе типа данных и их организации
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void SaveData(String file_name)
			{

			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Репозиторий реализующий работу с объектом указанного типа отдельного физического файла
		/// </summary>
		/// <remarks>
		/// Поддерживается следующие типы файлов:
		/// - Если тип файла бинарный - пока не поддерживается
		/// - Если тип файла текстовый - пока не поддерживается
		/// - Если тип файла XML - то данные будут прочитаны в соответствии со стандартной моделью сериализации
		/// </remarks>
		/// <typeparam name="TData">Тип объекта</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class RepositoryFile<TData> : RepositoryFile, ICubeXRepositoryFile<TData>
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal ListArray<TData> mRecords;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXRepository =================================
			/// <summary>
			/// Обобщённый список записей
			/// </summary>
			[Browsable(false)]
			public override IList IRecords
			{
				get { return (mRecords); }
			}

			/// <summary>
			/// Список записей
			/// </summary>
			[Browsable(false)]
			public IList<TData> Records
			{
				get { return (mRecords); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public RepositoryFile()
				: this(String.Empty, TRepositoryOrganization.Object)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="organization_type">Тип репозитория с точки зрения организации данных</param>
			//---------------------------------------------------------------------------------------------------------
			public RepositoryFile(TRepositoryOrganization organization_type)
				: this(String.Empty, organization_type)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			/// <param name="organization_type">Тип репозитория с точки зрения организации данных</param>
			//---------------------------------------------------------------------------------------------------------
			public RepositoryFile(String name, TRepositoryOrganization organization_type = TRepositoryOrganization.Object)
				: base(name, organization_type)
			{
				mRecords = new ListArray<TData>();
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
			/// Добавить запись
			/// </summary>
			/// <param name="record">Запись</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddRecord(in TData record)
			{
				mRecords.Add(record);
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
			/// Обновление записи
			/// </summary>
			/// <param name="index">Индекс записи</param>
			/// <param name="record">Запись</param>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateRecord(Int32 index, in TData record)
			{
				mRecords[index] = record;
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
			/// Поиск индекса записи
			/// </summary>
			/// <param name="record">Запись</param>
			/// <returns>Индекс записи</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 FindRecord(in TData record)
			{
				return (mRecords.IndexOf(record));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление записи
			/// </summary>
			/// <param name="index">Индекс записи</param>
			//---------------------------------------------------------------------------------------------------------
			public override void DeleteRecord(Int32 index)
			{
				mRecords.RemoveAt(index);
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ ДАННЫХ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из бинарного файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void LoadFromBinary(String file_name)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из текстового файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void LoadFromTxt(String file_name)
			{
				// Пока не поддерживается
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из файла в формате XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void LoadFromXml(String file_name)
			{
				if (mRecords.Count > 0)
				{
					mRecords.Clear();
				}

				XSerializationDispatcher.UpdateFromXml(mRecords, file_name);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из файла на основе типа данных и их организации
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void LoadData(String file_name)
			{
				if (XFileExtension.IsXmlFileName(file_name))
				{
					LoadFromXml(file_name);
				}
				else
				{
					if (XFileExtension.IsBinaryFileName(file_name))
					{
						LoadFromBinary(file_name);
					}
					else
					{
						LoadFromTxt(file_name);
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ СОХРАНЕНИЯ ДАННЫХ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в бинарный файл
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void SaveToBinary(String file_name)
			{
				// Пока не поддерживается
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в текстовый файл
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void SaveToTxt(String file_name)
			{
				// Пока не поддерживается
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в файл в формате XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void SaveToXml(String file_name)
			{
				if (mRecords.Count > 0)
				{
					XSerializationDispatcher.SaveToXml(file_name, mRecords);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в файл на основе типа данных и их организации
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void SaveData(String file_name)
			{
				// Данные представляют собой объект
				if (XFileExtension.IsXmlFileName(file_name))
				{
					SaveToXml(file_name);
				}
				else
				{
					if (XFileExtension.IsBinaryFileName(file_name))
					{
						SaveToBinary(file_name);
					}
					else
					{
						SaveToTxt(file_name);
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
//=====================================================================================================================