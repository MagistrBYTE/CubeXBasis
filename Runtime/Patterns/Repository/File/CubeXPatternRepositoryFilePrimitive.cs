//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Репозиторий данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternRepositoryFilePrimitive.cs
*		Репозиторий представляющий собой отдельный файл примитивных данных.
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
		/// Репозиторий представляющий собой отдельный файл примитивных данных
		/// </summary>
		/// <remarks>
		/// Поддерживается следующие типы файлов:
		/// - Если тип файла бинарный - то данные будут просто прочитаны. При это читается **метка окончание файла**
		/// - Если тип файла текстовый - то данные сначала будут прочитаны в строковый массив и потом будут сконвертированы в указанный тип
		/// - Если тип файла XML - то данные будут прочитаны в соответствии с типовыми правилами
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class RepositoryFilePrimitive<TPrimitive> : RepositoryFile<TPrimitive>
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Код типа объекта
			/// </summary>
			public readonly TypeCode TypeCodeItem = Type.GetTypeCode(typeof(TPrimitive));

			/// <summary>
			/// Столбец для отображения индекса
			/// </summary>
			public static readonly CSchemeDataProperty ColumnIndex = new CSchemeDataProperty("Index", TypeCode.Int32, "№ п/п");

			/// <summary>
			/// Столбец для отображения собственно значения
			/// </summary>
			public static readonly CSchemeDataProperty ColumnValue = new CSchemeDataProperty("Value", TypeCode.String, "Значение");

			/// <summary>
			/// Столбец для описания
			/// </summary>
			public static readonly CSchemeDataProperty ColumnDesc = new CSchemeDataProperty("Desc", TypeCode.String, "Описание");

			/// <summary>
			/// Схема данных используемая при отображении примитивных данных
			/// </summary>
			public static readonly CSchemeFlatData PrimitiveScheme = new CSchemeFlatData("Primitive",
				ColumnIndex,
				ColumnValue,
				ColumnDesc);
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public RepositoryFilePrimitive()
				: base(TRepositoryOrganization.Raw)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя репозитория</param>
			//---------------------------------------------------------------------------------------------------------
			public RepositoryFilePrimitive(String name)
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
				if (items != null && items.Length == 1)
				{
					mRecords.Add(items[0]);
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
				if (items != null && items.Length == 1)
				{
					mRecords[index] = (TPrimitive)items[0];
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
				if (items != null && items.Length == 1)
				{
					return(mRecords.IndexOf(items[0]));
				}

				return (-1);
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
				if (mRecords.Count > 0)
				{
					mRecords.Clear();
				}

				// Открываем файловый поток
				FileStream file_stream = new FileStream(file_name, FileMode.Open, FileAccess.Read);
				BinaryReader reader = new BinaryReader(file_stream);

				// Читаем количество элементов
				Int32 count = reader.ReadInt32();

				if (count > 0)
				{
					// Читаем сами элементы
					TPrimitive[] primitives = reader.ReadPimitives<TPrimitive>(count);

					// Присваиваем
					mRecords.AssignItems(primitives);

					// Читаем метку успешности
					Int32 label = reader.ReadInt32();

					if (label != XExtensionBinaryStream.SUCCESS_LABEL)
					{
						XLogger.LogWarning("Failed to read success label");
					}
				}

				reader.Close();
				file_stream.Close();
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

				// Читаем все строки
				String[] list = File.ReadAllLines(file_name);

				if (list != null && list.Length > 0)
				{
					// Последовательно преобразуем
					for (Int32 i = 0; i < list.Length; i++)
					{
						if (list[i].IsExists())
						{
							TPrimitive primitive = XConverter.ToPrimitive<TPrimitive>(TypeCodeItem, list[i]);
							mRecords.Add(primitive);
						}
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
				XmlNodeList list = document.GetElementsByTagName(CRepositoryFileRecord.XML_ELEMENT_ROOT);
				if (list != null && list.Count > 0)
				{
					XmlElement root = list.Item(0) as XmlElement;

					// Читаем имя
					mName = root.GetAttributeValueFromName(CRepositoryFileRecord.XML_ATTRIBUTE_NAME, mName);

					// Читаем записи согласно правилам
					for (Int32 i = 0; i < root.ChildNodes.Count; i++)
					{
						XmlElement element = root.ChildNodes[i] as XmlElement;

						// Должно быть такое имя элемента
						if (element != null && element.Name == CRepositoryFileRecord.XML_ELEMENT_RECORD)
						{
							String value = element.GetAttribute(CRepositoryFileRecord.XML_ATTRIBUTE_VALUE);
							if (value.IsExists())
							{
								// Преобразуем
								TPrimitive primitive = XConverter.ToPrimitive<TPrimitive>(TypeCodeItem, value);

								mRecords.Add(primitive);
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
				if (mRecords.Count > 0)
				{
					// Открываем файловый поток
					FileStream file_stream = new FileStream(file_name, FileMode.Create, FileAccess.Write);
					BinaryWriter writer = new BinaryWriter(file_stream);

					// Записываем количество элементов
					writer.Write(mRecords.Count);

					// Записываем сами элементы
					writer.Write(mRecords);

					// Записываем метку успешности
					writer.Write(XExtensionBinaryStream.SUCCESS_LABEL);

					// Закрываем потоки
					writer.Close();
					file_stream.Close();
				}
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
						writer.Write(mRecords[i].ToString());
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
				if (mRecords.Count > 0)
				{
					// Создаем поток для записи
					StreamWriter stream_writer = new StreamWriter(file_name);

					// Открываем файл
					XmlWriterSettings xws = new XmlWriterSettings();
					xws.Indent = true;

					XmlWriter writer = XmlWriter.Create(stream_writer, xws);

					// Записываем базовые данные
					writer.WriteStartElement(CRepositoryFileRecord.XML_ELEMENT_ROOT);
					writer.WriteAttributeString(nameof(Name), mName);
					writer.WriteAttributeString(nameof(mRecords.Count), mRecords.Count.ToString());

					for (Int32 i = 0; i < mRecords.Count; i++)
					{
						writer.WriteStartElement(CRepositoryFileRecord.XML_ELEMENT_RECORD);
						writer.WriteAttributeString(CRepositoryFileRecord.XML_ATTRIBUTE_VALUE, mRecords[i].ToString());
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