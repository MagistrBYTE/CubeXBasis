//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Паттерн документ
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternDocument.cs
*		Интерфейс концепции документа.
*		Под документом понимается объект который связан с отдельным физическим файлом для сохранения/загрузки своих
*	данных, позволяет себя отобразить, экспортировать в доступны форматы, а также отправить на печать.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CorePatternDocument Паттерн документ
		//! Под документом понимается объект который связан с отдельным физическим файлом для сохранения/загрузки своих
		//! данных, позволяет себя отобразить, экспортировать в доступны форматы, а также отправить на печать.
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс концепции документа
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXDocument
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

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение расширения файла без точки
			/// </summary>
			/// <returns>Расширение файла без точки</returns>
			//---------------------------------------------------------------------------------------------------------
			String GetFileExtension();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для реализации функциональности интерфейса <see cref="ICubeXDocument"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionDocument
		{
			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка документа из файла
			/// </summary>
			/// <returns>Документ</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static ICubeXDocument LoadDocument()
			{
				String file_name = XFileDialog.Open("Открыть документ");
				if (file_name.IsExists())
				{
					// Загружаем файл
					ICubeXDocument document = XSerializationDispatcher.LoadFrom(file_name) as ICubeXDocument;
					if (document != null)
					{
						// Получаем путь и имя файла
						document.PathFile = Path.GetDirectoryName(file_name);
						document.FileName = Path.GetFileName(file_name);

						// Если документ поддерживает коллекцию то обновляем связи
						if (document is ICubeXCollectionModel collection)
						{
							// Обновляем связи
							collection.UpdateOwnerLink();
						}

						// Корректируем имя
						if (document is ICubeXModel model)
						{
							if (model.Name.IsExists() == false)
							{
								model.Name = Path.GetFileNameWithoutExtension(file_name);
							}
						}

						// Уведомляем об окончании загрузки
						if (document is ICubeXAfterLoad after_load)
						{
							after_load.OnAfterLoad();
						}
					}

					return (document);
				}

				return (null);
			}

			//-------------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка документа из файла
			/// </summary>
			/// <param name="document">Документ</param>
			/// <returns>Статус загрузки</returns>
			//-------------------------------------------------------------------------------------------------------------
			public static Boolean LoadDocument(this ICubeXDocument document)
			{
				if(document != null)
				{
					String file_name = XFileDialog.Open("Открыть документ", "", document.GetFileExtension());
					if (file_name.IsExists())
					{
						// Уведомляем о начале загрузки
						if (document is ICubeXBeforeLoad before_load)
						{
							before_load.OnBeforeLoad();
						}

						// Получаем путь и имя файла
						document.PathFile = Path.GetDirectoryName(file_name);
						document.FileName = Path.GetFileName(file_name);

						// Если документ поддерживает коллекцию то очищаем её
						if (document is ICubeXCollectionModel collection)
						{
							collection.IModels.Clear();

							// Загружаем данные
							XSerializationDispatcher.UpdateFrom(document, file_name);

							// Обновляем связи
							collection.UpdateOwnerLink();
						}
						else
						{
							XSerializationDispatcher.UpdateFrom(document, file_name);
						}

						// Корректируем имя
						if (document is ICubeXModel)
						{
							ICubeXModel model = document as ICubeXModel;
							if(model.Name.IsExists() == false)
							{
								model.Name = Path.GetFileNameWithoutExtension(file_name);
							}
						}

						// Уведомляем об окончании загрузки
						if (document is ICubeXAfterLoad after_load)
						{
							after_load.OnAfterLoad();
						}

						return (true);
					}
				}
				else
				{

				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Восстановление данных документа к состоянию последнего сохранения
			/// </summary>
			/// <param name="document">Документ</param>
			/// <returns>Статус восстановления</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean RestoreDocument(this ICubeXDocument document)
			{
				if (document != null)
				{
					String file_name = Path.Combine(document.PathFile, document.FileName);
					if (File.Exists(file_name))
					{
						// Уведомляем о начале загрузки
						if (document is ICubeXBeforeLoad before_load)
						{
							before_load.OnBeforeLoad();
						}

						// Если документ поддерживает коллекцию то очищаем её
						if (document is ICubeXCollectionModel collection)
						{
							collection.IModels.Clear();

							// Загружаем данные
							XSerializationDispatcher.UpdateFrom(document, file_name);

							// Обновляем связи
							collection.UpdateOwnerLink();
						}
						else
						{
							XSerializationDispatcher.UpdateFrom(document, file_name);
						}

						// Уведомляем об окончании загрузки
						if (document is ICubeXAfterLoad after_load)
						{
							after_load.OnAfterLoad();
						}

						return (true);
					}
				}
				else
				{

				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение изменения документа в связанный физический файл
			/// </summary>
			/// <param name="document">Документ</param>
			/// <returns>Статус сохранения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean SaveDocument(this ICubeXDocument document)
			{
				if (document != null)
				{
					// Если не существует путь или имя файла
					if (document.PathFile.IsExists() == false || document.FileName.IsExists() == false)
					{
						String doc_name = "Документ";

						// Получаем имя
						if (document is ICubeXModel)
						{
							ICubeXModel model = document as ICubeXModel;
							if (model.Name.IsExists())
							{
								doc_name = model.Name;
							}
						}

						String file_name = XFileDialog.Save("Сохранить документ", document.PathFile, doc_name, document.GetFileExtension());
						if (file_name.IsExists())
						{
							// Уведомляем о начале сохранения 
							if (document is ICubeXBeforeSave before_save)
							{
								before_save.OnBeforeSave();
							}

							// Сохраняем документ
							XSerializationDispatcher.SaveTo(file_name, document);
							document.PathFile = Path.GetDirectoryName(file_name);
							document.FileName = Path.GetFileName(file_name);

							if (document is ICubeXModel)
							{
								ICubeXModel model = document as ICubeXModel;
								if (model.Name.IsExists() == false)
								{
									model.Name = Path.GetFileNameWithoutExtension(file_name);
								}
							}

							// Уведомляем об окончании сохранения 
							if (document is ICubeXAfterSave after_save)
							{
								after_save.OnAfterSave();
							}

							return (true);
						}
					}
					else
					{
						// Проверяем путь
						String file_name = Path.Combine(document.PathFile, document.FileName);
						if (File.Exists(file_name))
						{
							// Уведомляем о начале сохранения 
							if (document is ICubeXBeforeSave before_save)
							{
								before_save.OnBeforeSave();
							}

							XSerializationDispatcher.SaveTo(file_name, document);

							// Уведомляем об окончании сохранения 
							if (document is ICubeXAfterSave after_save)
							{
								after_save.OnAfterSave();
							}

							return (true);
						}
					}
				}
				else
				{

				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение изменения документа в физический файл
			/// </summary>
			/// <param name="document">Документ</param>
			/// <returns>Статус сохранения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean SaveAsDocument(this ICubeXDocument document)
			{
				if (document != null)
				{
					String doc_name = "Документ";

					// Получаем имя
					if (document is ICubeXModel)
					{
						ICubeXModel model = document as ICubeXModel;
						if (model.Name.IsExists())
						{
							doc_name = model.Name;
						}
					}

					String file_name = XFileDialog.Save("Сохранить документ как", document.PathFile, doc_name, document.GetFileExtension());
					if (file_name.IsExists())
					{
						// Уведомляем о начале сохранения 
						if (document is ICubeXBeforeSave before_save)
						{
							before_save.OnBeforeSave();
						}

						// Сохраняем документ
						XSerializationDispatcher.SaveTo(file_name, document);
						document.PathFile = Path.GetDirectoryName(file_name);
						document.FileName = Path.GetFileName(file_name);

						if (document is ICubeXModel)
						{
							ICubeXModel model = document as ICubeXModel;
							if (model.Name.IsExists() == false)
							{
								model.Name = Path.GetFileNameWithoutExtension(file_name);
							}
						}

						// Уведомляем об окончании сохранения 
						if (document is ICubeXAfterSave after_save)
						{
							after_save.OnAfterSave();
						}

						return (true);
					}
				}
				else
				{

				}

				return (false);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================