//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializatorCollectionModel.cs
*		Сериализация коллекции моделей.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleSerialization
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий сериализацию коллекции моделей в различные форматы
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSerializatorCollectionModel
		{
			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ XML ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись коллекции моделей в формат элемента XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="collection">Обобщённая коллекция</param>
			/// <param name="element_name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteCollectionModelToXml(XmlWriter writer, ICubeXCollectionModel collection, String element_name)
			{
				if (collection != null)
				{
					IList list = collection.IModels;

					if (list != null)
					{
						// Тип элемента коллекции
						Type element_type = list.GetType().GetClassicCollectionItemType();

						// Записываем начало элемента
						writer.WriteStartElement(element_name);

						// Количество элементов коллекции
						writer.WriteAttributeString(nameof(IList.Count), list.Count.ToString());

						TSerializeDataType serialize_data_type = CSerializeData.ComputeSerializeDataType(element_type);

						// Проверяем на интерфейс
						if (element_type.IsInterface && serialize_data_type == TSerializeDataType.Primitive)
						{
							// Принудительно меняем тип
							serialize_data_type = TSerializeDataType.Class;
						}

						switch (serialize_data_type)
						{
							case TSerializeDataType.Primitive:
								{
									for (Int32 i = 0; i < list.Count; i++)
									{
										writer.WriteStartElement(element_type.Name);
										XSerializatorPrimitive.WriteToAttribute(writer, element_type, list[i], XSerializationDispatcher.XML_NAME_ATTRIBUTE_VALUE);
										writer.WriteEndElement();
									}
								}
								break;
							case TSerializeDataType.Struct:
								{
									//
									// Тип коллекции фиксирован
									//
									// Получаем данные сериализации для этого типа
									CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(element_type);

									if (serialize_data == null)
									{
#if (UNITY_2017_1_OR_NEWER)
										UnityEngine.Debug.LogErrorFormat("There is no specified type: <{0}>", element_type.Name);
#else
										XLogger.LogErrorFormatModule(XSerializationDispatcher.MODULE_NAME, "There is no specified type: <{0}>", element_type.Name);
#endif
									}
									else
									{
										for (Int32 i = 0; i < list.Count; i++)
										{
											XSerializatorObject.WriteInstanceToXml(writer, serialize_data, list[i]);
										}
									}
								}
								break;
							case TSerializeDataType.Class:
								{
									//
									// Тип коллекции может быть разным
									//
									for (Int32 i = 0; i < list.Count; i++)
									{
										XSerializatorObject.WriteInstanceToXml(writer, list[i], null);
									}
								}
								break;
#if UNITY_2017_1_OR_NEWER
							case TSerializeDataType.UnityComponent:
							case TSerializeDataType.UnityUserComponent:
								{
									//
									// Записываем ссылку
									//
									for (Int32 i = 0; i < list.Count; i++)
									{
										writer.WriteStartElement(element_type.Name);
										XSerializatorUnity.WriteReferenceComponentToXml(writer, list[i] as UnityEngine.Component);
										writer.WriteEndElement();
									}
								}
								break;
							case TSerializeDataType.UnityGameObject:
								{
									//
									// Записываем ссылку
									//
									for (Int32 i = 0; i < list.Count; i++)
									{
										writer.WriteStartElement(element_type.Name);
										XSerializatorUnity.WriteReferenceGameObjectToXml(writer, list[i] as UnityEngine.GameObject);
										writer.WriteEndElement();
									}
								}
								break;
							case TSerializeDataType.UnityResource:
							case TSerializeDataType.UnityUserResource:
								{
									//
									// Записываем ссылку
									//
									for (Int32 i = 0; i < list.Count; i++)
									{
										writer.WriteStartElement(element_type.Name);
										XSerializatorUnity.WriteReferenceResourceToXml(writer, list[i] as UnityEngine.Object);
										writer.WriteEndElement();
									}
								}
								break;
#endif
							default:
								break;
						}

						writer.WriteEndElement();
					}
					else
					{
						//
						// Всегда записываем для сохранения топологии данных
						//
						// Записываем начало элемента
						writer.WriteStartElement(element_name);

						// Количество элементов коллекции
						writer.WriteAttributeString(nameof(IList.Count), "0");

						writer.WriteEndElement();
					}
				}
				else
				{
					//
					// Всегда записываем для сохранения топологии данных
					//
					// Записываем начало элемента
					writer.WriteStartElement(element_name);

					// Количество элементов коллекции
					writer.WriteAttributeString(nameof(IList.Count), "0");

					writer.WriteEndElement();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение коллекции моделей из формата XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="collection">Коллекции</param>
			/// <param name="element_name">Имя элемента</param>
			/// <param name="count">Количество элементов</param>
			/// <returns>Коллекция</returns>
			//---------------------------------------------------------------------------------------------------------
			public static ICubeXCollectionModel ReadCollectionModelFromXml(XmlReader reader, ICubeXCollectionModel collection, String element_name, Int32 count)
			{
				if (collection == null)
				{
					return null;
				}

				IList list = collection.IModels;

				Boolean is_new_collection = false;

				// Если коллекция нет то создаем новую
				if (list == null)
				{
					is_new_collection = true;
					list = new ArrayList();
				}

				// Получем базовый тип элемента коллекции
				Type element_type = null;
				if (is_new_collection)
				{
					element_type = typeof(System.Object);
				}
				else
				{
					element_type = list.GetType().GetClassicCollectionItemType();
				}

				// Если у нас коллекция существующая то получаем данные сериализации для её элемента
				CSerializeData serialize_data = null;
				if (is_new_collection)
				{
					// Мы читаем просто коллекцию элементов, пока мы не знаем какой конкретно тип её элементов
				}
				else
				{
					serialize_data = XSerializationDispatcher.GetSerializeData(element_type);
				}

				Int32 index = 0;
				if (reader.NodeType != XmlNodeType.None)
				{
					XmlReader reader_subtree = reader.ReadSubtree();
					while (reader_subtree.Read())
					{
						// Если элемент не равен
						if (reader_subtree.NodeType == XmlNodeType.Element && reader_subtree.Name != element_name)
						{
							// Проверяем тип
							// Если имя узла не равно имени нашего типа значит это новый тип
							if (reader_subtree.Name != element_type.Name)
							{
								// Получаем данные сериализации по этому типу
								serialize_data = XSerializationDispatcher.GetSerializeData(reader_subtree.Name);
								if (serialize_data == null)
								{
#if (UNITY_2017_1_OR_NEWER)
									UnityEngine.Debug.LogErrorFormat("There is no specified type: <{0}>", reader_subtree.Name);
#else
									XLogger.LogErrorFormatModule(XSerializationDispatcher.MODULE_NAME, "There is no specified type: <{0}>", reader_subtree.Name);
#endif
									continue;
								}
								else
								{
									element_type = serialize_data.SerializeType;
								}
							}

							// Коллекция фиксированного размера - массив
							if (list.IsFixedSize)
							{
								if (index < list.Count)
								{
									list.SetAt(index, XSerializationDispatcher.ReadDataFromXml(reader_subtree, null, index));
									index++;
								}
							}
							else
							{
								list.SetAt(index, XSerializationDispatcher.ReadDataFromXml(reader_subtree, null, index));
								index++;
							}
						}
					}

					if (index < count)
					{
#if (UNITY_2017_1_OR_NEWER)
						UnityEngine.Debug.LogErrorFormat("Elements read less <{0}> than required <{1}>", index, count);
#else
						XLogger.LogErrorFormatModule(XSerializationDispatcher.MODULE_NAME, "Elements read less <{0}> than required <{1}>", index, count);
#endif
					}

					reader_subtree.Close();
				}

				return (collection);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================