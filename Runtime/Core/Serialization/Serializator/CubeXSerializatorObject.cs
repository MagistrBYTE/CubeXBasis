//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializatorObject.cs
*		Сериализация примитивных данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
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
		/// Статический класс реализующий сериализацию стандартных объектов в различные форматы
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSerializatorObject
		{
			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ XML ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись члена объекта (поля/свойства) в формат XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="member">Данные для сериализации члена объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteMemberToXml(XmlWriter writer, System.Object instance, TSerializeDataMember member)
			{
				// Получаем типа члена объекта
				Type member_type = member.GetMemberType();

				switch (member.SerializeType)
				{
					case TSerializeMemberType.Primitive:
						{
							// Получаем значение члена объекта
							System.Object value = member.GetMemberValue(instance);
							XSerializatorPrimitive.WriteToAttribute(writer, member_type, value, member.Name);
						}
						break;
					case TSerializeMemberType.Struct:
						{
							// Получаем значение члена объекта
							System.Object value = member.GetMemberValue(instance);
							WriteInstanceToXml(writer, value, member.Name);
						}
						break;
					case TSerializeMemberType.Class:
						{
							// Получаем значение члена объекта
							System.Object value = member.GetMemberValue(instance);
							WriteInstanceToXml(writer, value, member.Name);
						}
						break;
					case TSerializeMemberType.List:
						{
							//
							// Это коллекция
							//
							// Получаем коллекцию
							IList collection_instance = member.GetMemberValue(instance) as IList;

							// Тип элемента коллекции
							Type element_type = member_type.GetClassicCollectionItemType();

							// Записываем
							XSerializatorCollection.WriteCollectionToXml(writer, element_type, collection_instance, member.Name);
						}
						break;
					case TSerializeMemberType.ListModels:
						{
							//
							// Это коллекция
							//
							// Получаем коллекцию моделей
							ICubeXCollectionModel collection_model = member.GetMemberValue(instance) as ICubeXCollectionModel;

							if(collection_model != null)
							{
								// Записываем
								XSerializatorCollectionModel.WriteCollectionModelToXml(writer, collection_model, member.Name);
							}

						}
						break;
					case TSerializeMemberType.Dictionary:
						{
							//
							// Реализовать
							//
						}
						break;
					case TSerializeMemberType.Reference:
						{
							//
							// Реализовать
							//
						}
						break;
#if UNITY_2017_1_OR_NEWER
					case TSerializeMemberType.UnityComponent:
					case TSerializeMemberType.UnityUserComponent:
						{
							// Записываем начало элемента
							writer.WriteStartElement(member.Name);
							{
								// У нас может быть ситуация когда тип члена объекта это ссылка на базовый класс
								// а реально присвоен объект производного класса

								// Получаем объект члена данных
								System.Object value = member.GetMemberValue(instance);
								if (value != null)
								{
									// У нас реальный объект - получаем его тип
									Type current_type = value.GetType();
									writer.WriteStartElement(current_type.Name);
									{
										XSerializatorUnity.WriteReferenceComponentToXml(writer, value as UnityEngine.Component);
									}
									writer.WriteEndElement();
								}
								else
								{
									// Запишем пустую ссылку по типу члена объекта
									writer.WriteStartElement(member_type.Name);
									writer.WriteEndElement();
								}
							}
							writer.WriteEndElement();
						}
						break;
					case TSerializeMemberType.UnityGameObject:
						{
							// Записываем начало элемента
							writer.WriteStartElement(member.Name);
							{
								writer.WriteStartElement(nameof(UnityEngine.GameObject));
								{
									// Получаем значение члена объекта
									System.Object value = member.GetMemberValue(instance);
									XSerializatorUnity.WriteReferenceGameObjectToXml(writer, value as UnityEngine.GameObject);

								}
								writer.WriteEndElement();
							}
							writer.WriteEndElement();
						}
						break;
					case TSerializeMemberType.UnityResource:
					case TSerializeMemberType.UnityUserResource:
						{
							// Записываем начало элемента
							writer.WriteStartElement(member.Name);
							{
								// У нас может быть ситуация когда тип члена объекта это ссылка на базовый класс
								// а реально присвоен объект производного класса

								// Получаем значение члена объекта
								System.Object value = member.GetMemberValue(instance);
								if (value != null)
								{
									// У нас реальный объект - получаем его тип
									Type current_type = value.GetType();
									writer.WriteStartElement(current_type.Name);
									{
										XSerializatorUnity.WriteReferenceResourceToXml(writer, value as UnityEngine.Object);
									}
									writer.WriteEndElement();
								}
								else
								{
									// Запишем пустую ссылку по типу члена объекта
									writer.WriteStartElement(member_type.Name);
									writer.WriteEndElement();
								}
							}
							writer.WriteEndElement();
						}
						break;
#endif
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись объекта в формат элемента XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="element_name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteInstanceToXml(XmlWriter writer, System.Object instance, String element_name)
			{
				// Получаем реальный тип объекта
				Type object_type = instance.GetType();

				// Получаем данные сериализации по этому типу
				CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(object_type);

				if (serialize_data != null)
				{
					// Записываем начало элемента
					writer.WriteStartElement(String.IsNullOrEmpty(element_name) ? serialize_data.SerializeNameType : element_name);
				}
				else
				{
					// Записываем начало элемента
					writer.WriteStartElement(String.IsNullOrEmpty(element_name) ? object_type.Name : element_name);
				}

				// Если он может сам себя записать
				if (instance is ICubeXSerializeImplementationXML)
				{
					ICubeXSerializeImplementationXML serializable_self = instance as ICubeXSerializeImplementationXML;
					serializable_self.WriteToXml(writer);
				}
				else
				{
					// Смотрим, поддерживает ли объект интерфейс сериализации
					ICubeXSerializableObject serializable = instance as ICubeXSerializableObject;
					if (serializable != null)
					{
						// Если поддерживает то записываем атрибут
						writer.WriteAttributeString(nameof(ICubeXSerializableObject.IDKeySerial), serializable.IDKeySerial.ToString());
					}

					if (serialize_data != null && serialize_data.Members != null)
					{
						for (Int32 i = 0; i < serialize_data.Members.Count; i++)
						{
							WriteMemberToXml(writer, instance, serialize_data.Members[i]);
						}
					}
				}

				// Записываем окончание элемента
				writer.WriteEndElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись объекта в формат элемента XML
			/// </summary>
			/// <remarks>
			/// Оптимизированная версия предназначенная для записи списка объектов
			/// </remarks>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="serialize_data">Данные сериализации</param>
			/// <param name="instance">Экземпляр объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteInstanceToXml(XmlWriter writer, CSerializeData serialize_data, System.Object instance)
			{
				// Получаем реальный тип объекта
				Type object_type = serialize_data.SerializeType;

				// Записываем начало элемента
				writer.WriteStartElement(object_type.Name);

				// Если он может сам себя записать
				if (instance is ICubeXSerializeImplementationXML)
				{
					ICubeXSerializeImplementationXML serializable_self = instance as ICubeXSerializeImplementationXML;
					serializable_self.WriteToXml(writer);
				}
				else
				{
					// Смотрим, поддерживает ли объект интерфейс сериализации
					ICubeXSerializableObject serializable = instance as ICubeXSerializableObject;
					if (serializable != null)
					{
						// Если поддерживает то записываем атрибут
						writer.WriteAttributeString(nameof(ICubeXSerializableObject.IDKeySerial), serializable.IDKeySerial.ToString());
					}

					if (serialize_data != null && serialize_data.Members != null)
					{
						for (Int32 i = 0; i < serialize_data.Members.Count; i++)
						{
							WriteMemberToXml(writer, instance, serialize_data.Members[i]);
						}
					}
				}

				// Записываем окончание элемента
				writer.WriteEndElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение члена данных (поля/свойства) из формата XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="member">Член данных</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ReadMemberFromXml(XmlReader reader, System.Object instance, TSerializeDataMember member)
			{
				// Получаем тип члена данных
				Type member_type = member.GetMemberType();

				switch (member.SerializeType)
				{
					case TSerializeMemberType.Primitive:
						{
							// Читаем
							System.Object child_instance = XSerializatorPrimitive.ReadFromAttribute(reader, member_type, member.Name);

							// Обновляем еще раз
							try
							{
								member.SetMemberValue(instance, child_instance);
							}
							catch (Exception)
							{
#if (UNITY_2017_1_OR_NEWER)
								UnityEngine.Debug.LogErrorFormat("Failed to set property <{0}> of object <{1}> to value <{2}>", member.Name, instance, child_instance);
#else
								XLogger.LogErrorFormatModule(nameof(XSerializationDispatcher), "Failed to set property <{0}> of object <{1}> to value <{2}>", member.Name, instance, child_instance);
#endif
							}
						}
						break;
					case TSerializeMemberType.Struct:
						{
							// Получаем данные сериализации для этого типа
							CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(member_type);

							if (serialize_data == null)
							{
#if (UNITY_2017_1_OR_NEWER)
								UnityEngine.Debug.LogErrorFormat("There is no specified type: <{0}>", member_type.Name);
#else
								XLogger.LogErrorFormatModule(nameof(XSerializationDispatcher), "There is no specified type: <{0}>", member_type.Name);
#endif
								return;
							}

							// Читаем
							System.Object child_instance = ReadInstanceFromXml(reader, serialize_data, null, member.Name);

							// Обновляем еще раз
							try
							{
								member.SetMemberValue(instance, child_instance);
							}
							catch (Exception)
							{
#if (UNITY_2017_1_OR_NEWER)
								UnityEngine.Debug.LogErrorFormat("Failed to set property <{0}> of object <{1}> to value <{2}>", member.Name, instance, child_instance);
#else
								XLogger.LogErrorFormatModule(nameof(XSerializationDispatcher), "Failed to set property <{0}> of object <{1}> to value <{2}>", member.Name, instance, child_instance);
#endif
							}
						}
						break;
					case TSerializeMemberType.Class:
						{
							// Получаем данные сериализации для этого типа
							CSerializeData serialize_data = XSerializationDispatcher.GetSerializeData(member_type);

							if (serialize_data == null)
							{
#if (UNITY_2017_1_OR_NEWER)
								UnityEngine.Debug.LogErrorFormat("There is no specified type: <{0}>", member_type.Name);
#else
								XLogger.LogErrorFormatModule(nameof(XSerializationDispatcher), "There is no specified type: <{0}>", member_type.Name);
#endif
								return;
							}

							// Читаем
							System.Object child_instance = ReadInstanceFromXml(reader, serialize_data, null, member.Name);

							// Обновляем еще раз
							try
							{
								member.SetMemberValue(instance, child_instance);
							}
							catch (Exception)
							{
#if (UNITY_2017_1_OR_NEWER)
								UnityEngine.Debug.LogErrorFormat("Failed to set property <{0}> of object <{1}> to value <{2}>", member.Name, instance, child_instance);
#else
								XLogger.LogErrorFormatModule(nameof(XSerializationDispatcher), "Failed to set property <{0}> of object <{1}> to value <{2}>", member.Name, instance, child_instance);
#endif
							}
						}
						break;
					case TSerializeMemberType.List:
						{
							//
							// Это коллекция
							//
							// Перемещаемся к элементу
							reader.MoveToElement(member.Name);

							// Читаем количество элементов
							Int32 count = reader.ReadIntegerFromAttribute(nameof(IList.Count));

							// Получаем коллекцию
							IList collection = member.GetMemberValue(instance) as IList;

							// Если ее нет то создаем и устанавливаем
							if (collection == null)
							{
								// Если это стандартный список или массив то используем конструктор с аргументом
								if (member_type.IsArray ||
									(member_type.IsGenericType && member_type.Name == XExtensionReflectionType.LIST_1) ||
									(member_type.IsGenericType && member_type.Name == XExtensionReflectionType.LIST_ARRAY_1))
								{
									collection = XReflection.CreateInstance(member_type, count > 0 ? count : 10) as IList;
								}
								else
								{
									// Используем конструктор без параметров
									collection = XReflection.CreateInstance(member_type) as IList;
								}
								member.SetMemberValue(instance, collection);
							}

							XSerializatorCollection.ReadCollectionFromXml(reader, collection, member.Name, count);
						}
						break;
					case TSerializeMemberType.ListModels:
						{
							//
							// Это коллекция
							//
							// Перемещаемся к элементу
							reader.MoveToElement(member.Name);

							// Читаем количество элементов
							Int32 count = reader.ReadIntegerFromAttribute(nameof(IList.Count));

							// Получаем коллекцию моделей
							ICubeXCollectionModel collection_model = member.GetMemberValue(instance) as ICubeXCollectionModel;

							// Если ее нет то пробуем ее создать
							if (collection_model == null)
							{
								collection_model = XReflection.CreateInstance(member_type) as ICubeXCollectionModel;
								member.SetMemberValue(instance, collection_model);
							}

							// Читаем стандартную коллекцию
							XSerializatorCollectionModel.ReadCollectionModelFromXml(reader, collection_model, member.Name, count);

							// Обновляем связи
							collection_model.UpdateOwnerLink();
						}
						break;
					case TSerializeMemberType.Dictionary:
						{
							//
							// Реализовать
							//
						}
						break;
					case TSerializeMemberType.Reference:
						{
							//
							// Реализовать
							//
						}
						break;
#if UNITY_2017_1_OR_NEWER
					case TSerializeMemberType.UnityComponent:
					case TSerializeMemberType.UnityUserComponent:
						{
							XSerializatorUnity.ReadReferenceComponentFromXml(reader, instance, member);
						}
						break;
					case TSerializeMemberType.UnityGameObject:
						{
							XSerializatorUnity.ReadReferenceGameObjectFromXml(reader, instance, member);
						}
						break;
					case TSerializeMemberType.UnityResource:
					case TSerializeMemberType.UnityUserResource:
						{
							XSerializatorUnity.ReadReferenceResourceFromXml(reader, instance, member);
						}
						break;
#endif
					default:
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение объекта из формата элемента XML
			/// </summary>
			/// <param name="reader">Средство чтения данных формата XML</param>
			/// <param name="serialize_data">Данные сериализации</param>
			/// <param name="element_name">Имя элемента</param>
			/// <param name="instance">Экземпляр объекта</param>
			/// <returns>Экземпляр объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public static System.Object ReadInstanceFromXml(XmlReader reader, CSerializeData serialize_data, System.Object instance, String element_name)
			{
				// Перемещаемся к элементу
				reader.MoveToElement(element_name);

				Boolean is_support_serializable = false;
				Int64 id_key_serial = -1;
				Boolean is_existing_object = false;

				// Получаем тип объекта
				Type object_type = serialize_data.SerializeType;

				// Смотрим, поддерживает ли тип интерфейс сериализации
				if (instance == null)
				{
					if (object_type.IsSupportInterface<ICubeXSerializableObject>())
					{
						is_support_serializable = true;

						// Читаем код сериализации
						id_key_serial = reader.ReadLongFromAttribute(nameof(ICubeXSerializableObject.IDKeySerial), -1);
						if (id_key_serial != -1)
						{
							// Пробуем найти в словаре объектов
							if (XSerializationDispatcher.SerializableObjects.ContainsKey(id_key_serial))
							{
								// Теперь мы просто обновляем данные объекта
								instance = XSerializationDispatcher.SerializableObjects[id_key_serial];
								is_existing_object = true;
							}
						}
					}
				}

				// Объект мы не нашли - значит создаем с помощью конструктора
				if (instance == null)
				{
					if (XSerializationDispatcher.Constructor == null)
					{
						instance = XReflection.CreateInstance(object_type);
					}
					else
					{
						instance = XSerializationDispatcher.Constructor(object_type.Name);

						// Конструктор почему то не создал объект указанного типа
						if (instance == null)
						{
							instance = XReflection.CreateInstance(object_type);
						}
					}
				}

				// Если объект может сам себя прочитать
				if (instance is ICubeXSerializeImplementationXML)
				{
					ICubeXSerializeImplementationXML serializable_self = instance as ICubeXSerializeImplementationXML;
					serializable_self.ReadFromXml(reader);

					// Этот объект тоже может поддерживать сериализацию
					if (instance is ICubeXSerializableObject)
					{
						is_support_serializable = true;
					}
				}
				else
				{
					// Последовательно читаем данные 
					if (serialize_data.Members != null)
					{
						for (Int32 i = 0; i < serialize_data.Members.Count; i++)
						{
							ReadMemberFromXml(reader, instance, serialize_data.Members[i]);
						}
					}
				}

				// Если объект поддерживает сериализацию и является созданным
				if (is_support_serializable && is_existing_object == false)
				{
					ICubeXSerializableObject serializable = instance as ICubeXSerializableObject;
					XSerializationDispatcher.SerializableObjects.Add(serializable.IDKeySerial, serializable);
				}

				return instance;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================