//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryDispatcher.cs
*		Диспетчер для управления всеми доступными репозиториями данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
		/// Диспетчер для управления всеми доступными репозиториями данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CRepositoryDispatcher
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected Dictionary<String, ICubeXRepository> mRepositories;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Словарь репозиторий по ключу идентификатора
			/// </summary>
			public Dictionary<String, ICubeXRepository> Repositories
			{
				get
				{
					return (mRepositories);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CRepositoryDispatcher()
			{
				mRepositories = new Dictionary<String, ICubeXRepository>();
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация репозиториев на основе идентификатора
			/// </summary>
			/// <param name="identifier">Строковый идентификатор репозитория</param>
			/// <returns>Репозиторий</returns>
			//---------------------------------------------------------------------------------------------------------
			public ICubeXRepository this[String identifier]
			{
				get
				{
					return (mRepositories[identifier]);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка всех репозиториев из указанной директории
			/// </summary>
			/// <param name="directory">Директория</param>
			/// <param name="is_loaded">Статус загрузки данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void LoadAllRepositories(String directory, Boolean is_loaded)
			{
				String[] files = Directory.GetFiles(directory);
				for (Int32 i = 0; i < files.Length; i++)
				{
					// Получаем расширение
					String ext = Path.GetExtension(files[i]);
					if(ext == XFileExtension.XML_D)
					{
						// Загружаем файл
						XmlDocument document = new XmlDocument();
						document.Load(files[i]);

						if(document.DocumentElement != null && 
							document.DocumentElement.Name == CRepositoryFileRecord.XML_ELEMENT_ROOT)
						{
							CRepositoryFileRecord repository_records = new CRepositoryFileRecord();
							repository_records.Connect(files[i], is_loaded);

							if (repository_records.UniqueID.IsExists())
							{
								this.Repositories.Add(repository_records.UniqueID, repository_records);
							}
							else
							{
								String file_name = Path.GetFileNameWithoutExtension(files[i]);
								this.Repositories.Add(file_name, repository_records);
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
//=====================================================================================================================