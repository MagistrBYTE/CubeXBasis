//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryFileRecord.cs
*		Репозиторий представляющий собой отдельный файл в виде строк записей.
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
		/// Репозиторий представляющий собой отдельный файл в виде строк записей
		/// </summary>
		/// <remarks>
		/// Поддерживается следующие типы файлов:
		/// - Если тип файла бинарный - пока не поддерживается
		/// - Если тип файла текстовый - данные читаются на прямую, в качестве разделится используется SeparatorTokens
		/// - Если тип файла XML - то данные будут прочитаны в соответствии с типовыми правилами
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class CRepositoryFileRecord : RepositoryFile<String[]>
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================

			/// <summary>
			/// Имя корневого элемента XML
			/// </summary>
			public const String XML_ELEMENT_ROOT = "Repository";

			/// <summary>
			/// Имя элемента XML содержащего запись
			/// </summary>
			public const String XML_ELEMENT_RECORD = "Record";

			/// <summary>
			/// Имя атрибута XML содержащего наименование репозитория
			/// </summary>
			public const String XML_ATTRIBUTE_NAME = "Name";

			/// <summary>
			/// Имя атрибута XML содержащего значение примитивного типа
			/// </summary>
			public const String XML_ATTRIBUTE_VALUE = "Value";
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			/// <summary>
			/// Разделитель токенов
			/// </summary>
			public static Char[] SeparatorTokens = XChar.SeparatorDotComma;

			/// <summary>
			/// Соединитель токенов
			/// </summary>
			public static String JoinToken = ";";
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CRepositoryFileRecord()
				: base(TRepositoryOrganization.Raw)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			public CRepositoryFileRecord(String name)
				: base(name, TRepositoryOrganization.Raw)
			{
			}
			#endregion

			#region ======================================= РАБОТА С ЗАПИСЬЮ - ICubeXRepository =======================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавить запись
			/// </summary>
			/// <param name="items">Элементы записи</param>
			//---------------------------------------------------------------------------------------------------------
			public override void AddRecord(params System.Object[] items)
			{
				if(items != null && items.Length > 0)
				{
					String[] record = new String[items.Length];
					for (Int32 i = 0; i < items.Length; i++)
					{
						record[i] = items[i].ToString();
						mRecords.Add(record);
					}
				}
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
				if (items != null && items.Length > 0)
				{
					String[] record = mRecords[index];
					for (Int32 i = 0; i < items.Length; i++)
					{
						record[i] = items[i].ToString();
					}
					mRecords[index] = record;
				}
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
				Int32 result = -1;

				if (items != null && items.Length > 0)
				{
					switch (items.Length)
					{
						case 1:
							{
								for (Int32 i = 0; i < mRecords.Count; i++)
								{
									if(mRecords[i][0] == items[0].ToString())
									{
										result = i;
										break;
									}
								}
							}
							break;
						case 2:
							{
								for (Int32 i = 0; i < mRecords.Count; i++)
								{
									if (mRecords[i][0] == items[0].ToString() &&
										mRecords[i][1] == items[1].ToString())
									{
										result = i;
										break;
									}
								}
							}
							break;
						case 3:
							{
								for (Int32 i = 0; i < mRecords.Count; i++)
								{
									if (mRecords[i][0] == items[0].ToString() &&
										mRecords[i][1] == items[1].ToString() &&
										mRecords[i][2] == items[2].ToString())
									{
										result = i;
										break;
									}
								}
							}
							break;
						default:
							break;
					}
				}

				return (result);
			}
			#endregion

			#region ======================================= МЕТОДЫ ЗАГРУЗКИ ДАННЫХ ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из бинарного файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void LoadFromBinary(String file_name)
			{
				// Пока не поддерживается
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из текстового файла
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void LoadFromTxt(String file_name)
			{
				if (mRecords.Count > 0)
				{
					mRecords.Clear();
				}

				String[] list = File.ReadAllLines(file_name);
				if(list != null && list.Length > 0)
				{
					for (Int32 i = 0; i < list.Length; i++)
					{
						mRecords.Add(list[i].Split(SeparatorTokens));
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных из файла в формате XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void LoadFromXml(String file_name)
			{
				if (mRecords.Count > 0)
				{
					mRecords.Clear();
				}

				// Загружаем XML документ
				XmlDocument document = new XmlDocument();
				document.Load(file_name);

				// Находим корневой элемент
				XmlNodeList list = document.GetElementsByTagName(XML_ELEMENT_ROOT);
				if (list != null && list.Count > 0)
				{
					XmlElement root = list.Item(0) as XmlElement;

					// Читаем обязательные атрибуты
					mUniqueID = root.GetAttributeValueFromName(XIdentifier.UniqueID, mUniqueID);
					mName = root.GetAttributeValueFromName(XML_ATTRIBUTE_NAME, mName);
					mID = root.GetAttributeValueFromNameAsInteger(XIdentifier.ID, mID);

					// Читаем записи
					for (Int32 i = 0; i < root.ChildNodes.Count; i++)
					{
						XmlElement element = root.ChildNodes[i] as XmlElement;
						if (element != null && element.Name == XML_ELEMENT_RECORD)
						{
							// Только если есть схема
							if (Scheme != null)
							{
								// Создаем блок данных
								String[] record = new String[Scheme.Columns.Count];

								// Читаем по столбцам
								for (Int32 c = 0; c < Scheme.Columns.Count; c++)
								{
									record[c] = element.GetAttributeValueFromName(Scheme.Columns[c].Name);
								}

								mRecords.Add(record);
							}
						}
						else
						{
							if (element != null && element.Name == CSchemeFlatData.XML_ELEMENT_NAME)
							{
								Scheme = new CSchemeFlatData();
								Scheme.ReadFromXml(element);
							}
						}
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
			protected override void SaveToBinary(String file_name)
			{
				// Пока не поддерживается
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в текстовый файл
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void SaveToTxt(String file_name)
			{
				if (mRecords.Count > 0)
				{
					// Открываем потоки
					FileStream file_stream = new FileStream(file_name, FileMode.Create, FileAccess.Write);
					StreamWriter writer = new StreamWriter(file_stream);

					// Записываем сами элементы
					for (Int32 i = 0; i < mRecords.Count; i++)
					{
						String[] record = mRecords[i];
						String result = String.Join(JoinToken, record);
						writer.WriteLine(result);
					}

					// Закрываем потоки
					writer.Close();
					file_stream.Close();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение данных в файл в формате XML
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void SaveToXml(String file_name)
			{
				// Только если есть схема данных
				if (Scheme != null && mRecords.Count > 0)
				{
					// Создаем поток для записи
					StreamWriter stream_writer = new StreamWriter(file_name);

					// Открываем файл
					XmlWriterSettings xws = new XmlWriterSettings();
					xws.Indent = true;

					XmlWriter writer = XmlWriter.Create(stream_writer, xws);

					// Записываем базовые данные
					writer.WriteStartElement(XML_ELEMENT_ROOT);
					writer.WriteAttributeString(XIdentifier.UniqueID, mUniqueID);
					writer.WriteAttributeString(XML_ATTRIBUTE_NAME, mName);
					writer.WriteAttributeString(XIdentifier.ID, mID.ToString());
					writer.WriteAttributeString(nameof(mRecords.Count), mRecords.Count.ToString());

					for (Int32 i = 0; i < mRecords.Count; i++)
					{
						writer.WriteStartElement(XML_ELEMENT_RECORD);

						// Получаем блок данных
						String[] record = mRecords[i];
						for (Int32 c = 0; c < Scheme.Columns.Count; c++)
						{
							writer.WriteAttributeString(Scheme.Columns[c].Name, record[c]);
						}

						writer.WriteEndElement();
					}

					// Закрываем поток
					writer.WriteEndElement();
					writer.Close();
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