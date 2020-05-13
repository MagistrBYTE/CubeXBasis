//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Методы расширений
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXExtensionUnityXml.cs
*		Методы расширения для сериализации базовых классов и структурных типов Unity в XML формат.
*		Реализация методов расширений потоков чтения и записи XML данных, а также методов работы с объектной моделью 
*	документа XML для сериализации базовых классов и структурных типов Unity в XML формат.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleExtension
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширения потоков чтения и записи XML данных для сериализации базовых классов и структурных типов Unity в XML формат
		/// </summary>
		/// <remarks>
		/// Реализация методов расширений потоков чтения и записи XML данных, а также методов работы с объектной моделью 
		/// документа XML для сериализации базовых классов и структурных типов Unity в XML формат
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionXmlStreamUnity
		{
			#region ======================================= ДАННЫЕ ====================================================
			/// <summary>
			/// Имя идентификатора игрового объекта
			/// </summary>
			public const String ID = "ID";

			/// <summary>
			/// Имя игрового объекта
			/// </summary>
			public const String NAME = "Name";

			/// <summary>
			/// Полный путь игрового объекта
			/// </summary>
			public const String PATH = "Path";

			/// <summary>
			/// Имя тэга игрового объекта
			/// </summary>
			public const String TAG = "Tag";

			/// <summary>
			/// Имя типа компонента или ресурса
			/// </summary>
			public const String TYPE = "Type";
			#endregion

			#region ======================================= ЗАПИСЬ POD ДАННЫХ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных двухмерного вектора в формат атрибутов
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="vector">Двухмерный вектор</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteVector2DToAttribute(this XmlWriter xml_writer, String name, Vector2 vector)
			{
				xml_writer.WriteAttributeString(name, vector.SerializeToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных трехмерного вектора в формат атрибутов
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="vector">Трехмерный вектор</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteVector3DToAttribute(this XmlWriter xml_writer, String name, Vector3 vector)
			{
				xml_writer.WriteAttributeString(name, vector.SerializeToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных четырехмерного вектора в формат атрибутов
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="vector">Четырехмерный вектор</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteVector4DToAttribute(this XmlWriter xml_writer, String name, Vector4 vector)
			{
				xml_writer.WriteAttributeString(name, vector.SerializeToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных кватерниона в формат атрибутов
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="quaternion">Кватернион</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteQuaternionToAttribute(this XmlWriter xml_writer, String name, Quaternion quaternion)
			{
				xml_writer.WriteAttributeString(name, quaternion.SerializeToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных цветового значения в формат атрибутов
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="color">Цветовое значение</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteColorToAttribute(this XmlWriter xml_writer, String name, Color color)
			{
				xml_writer.WriteAttributeString(name, color.SerializeToString());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных прямоугольника в формат атрибутов
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="rect">Прямоугольник</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteRectToAttribute(this XmlWriter xml_writer, String name, Rect rect)
			{
				xml_writer.WriteAttributeString(name, rect.SerializeToString());
			}
			#endregion

			#region ======================================= ЗАПИСЬ ССЫЛОК =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных ссылки на компонент в формат атрибутов
			/// </summary>
			/// <remarks>
			/// Внимание!!!
			/// Запишется 5 атрибутов: идентификатор, имя, путь тэг и тип компонента
			/// </remarks>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="component">Компонент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteComponentToAttribute(this XmlWriter xml_writer, String name, UnityEngine.Component component)
			{
				if (component != null)
				{
					xml_writer.WriteAttributeString(name + ID, component.gameObject.GetInstanceID().ToString());
					xml_writer.WriteAttributeString(name + NAME, component.gameObject.name);
					xml_writer.WriteAttributeString(name + PATH, XSerializatorUnity.GetPathScene(component));
					xml_writer.WriteAttributeString(name + TAG, component.gameObject.tag);
					xml_writer.WriteAttributeString(name + TYPE, component.GetType().Name);
				}
				else
				{
					xml_writer.WriteAttributeString(name + ID, "-1");
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных ссылки на игровой объект в формат атрибутов
			/// </summary>
			/// <remarks>
			/// Внимание!!!
			/// Запишется 4 атрибута: идентификатор, имя, путь и тэг
			/// </remarks>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="game_object">Компонент</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteGameObjectToAttribute(this XmlWriter xml_writer, String name, GameObject game_object)
			{
				if (game_object != null)
				{
					xml_writer.WriteAttributeString(name + ID, game_object.GetInstanceID().ToString());
					xml_writer.WriteAttributeString(name + NAME, game_object.name);
					xml_writer.WriteAttributeString(name + PATH, XSerializatorUnity.GetPathScene(game_object));
					xml_writer.WriteAttributeString(name + TAG, game_object.tag);
				}
				else
				{
					xml_writer.WriteAttributeString(name + ID, "-1");
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных ссылки на ресурс в формат атрибутов
			/// </summary>
			/// <remarks>
			/// Внимание!!!
			/// Запишется 3 атрибута: идентификатор, имя и тип ресурса
			/// </remarks>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="resource">Ресурс</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteResourceToAttribute(this XmlWriter xml_writer, String name, UnityEngine.Object resource)
			{
				if (resource != null)
				{
					xml_writer.WriteAttributeString(name + ID, resource.GetInstanceID().ToString());
					xml_writer.WriteAttributeString(name + NAME, resource.name);
					xml_writer.WriteAttributeString(name + TYPE, resource.GetType().Name);
				}
				else
				{
					xml_writer.WriteAttributeString(name + ID, "-1");
				}
			}
			#endregion

			#region ======================================= ЧТЕНИЕ POD ДАННЫХ =========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных двухмерного вектора в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <returns>Двухмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 ReadUnityVector2DFromAttribute(this XmlReader xml_reader, String name)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityVector2.DeserializeFromString(value);
				}
				return Vector2.zero;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных двухмерного вектора в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Двухмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector2 ReadUnityVector2DFromAttribute(this XmlReader xml_reader, String name, Vector2 default_value)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityVector2.DeserializeFromString(value);
				}
				return default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных трехмерного вектора в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <returns>Трехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 ReadUnityVector3DFromAttribute(this XmlReader xml_reader, String name)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityVector3.DeserializeFromString(value);
				}
				return Vector3.zero;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных трехмерного вектора в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Трехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector3 ReadUnityVector3DFromAttribute(this XmlReader xml_reader, String name, Vector3 default_value)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityVector3.DeserializeFromString(value);
				}
				return default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных четырехмерного вектора в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <returns>Четырехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector4 ReadUnityVector4DFromAttribute(this XmlReader xml_reader, String name)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityVector4.DeserializeFromString(value);
				}
				return Vector4.zero;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных четырехмерного вектора в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Четырехмерный вектор</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector4 ReadUnityVector4DFromAttribute(this XmlReader xml_reader, String name, Vector4 default_value)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityVector4.DeserializeFromString(value);
				}
				return default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных кватерниона в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <returns>Кватернион</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Quaternion ReadUnityQuaternionFromAttribute(this XmlReader xml_reader, String name)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityQuaternion.DeserializeFromString(value);
				}
				return Quaternion.identity;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных кватерниона в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Кватернион</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Quaternion ReadUnityQuaternionFromAttribute(this XmlReader xml_reader, String name, Quaternion default_value)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityQuaternion.DeserializeFromString(value);
				}
				return default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных цветового значения в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <returns>Цветовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector4 ReadUnityColorFromAttribute(this XmlReader xml_reader, String name)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityColor.DeserializeFromString(value);
				}
				return Color.black;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных цветового значения в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Цветовое значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector4 ReadUnityColorFromAttribute(this XmlReader xml_reader, String name, Color default_value)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityColor.DeserializeFromString(value);
				}
				return default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных прямоугольника в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <returns>Прямоугольник</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect ReadUnityRectFromAttribute(this XmlReader xml_reader, String name)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityRect.DeserializeFromString(value);
				}
				return Rect.zero;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных прямоугольника в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Прямоугольник</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Rect ReadUnityRectFromAttribute(this XmlReader xml_reader, String name, Rect default_value)
			{
				String value;
				if ((value = xml_reader.GetAttribute(name)) != null)
				{
					return XUnityRect.DeserializeFromString(value);
				}
				return default_value;
			}
			#endregion

			#region ======================================= ЧТЕНИЕ ССЫЛОК =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных ссылки на компонент в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Компонент Unity</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TComponent ReadUnityComponentFromAttribute<TComponent>(this XmlReader xml_reader, String name, 
				TComponent default_value) where TComponent : UnityEngine.Component
			{
				TComponent component = null;
				GameObject game_object = null;

				// Должно быть записано идентификатор, имя, путь тэг и тип компонента
				String value_id = xml_reader.GetAttribute(name + ID);
				String value_name = xml_reader.GetAttribute(name + NAME);
				String value_path = xml_reader.GetAttribute(name + PATH);
				String value_tag = xml_reader.GetAttribute(name + TAG);
				String value_type = xml_reader.GetAttribute(name + TYPE);

				// Если имени нет значит однозначно выходим
				if (String.IsNullOrEmpty(value_name))
				{
					// Пробуем такой вариант
					value_name = xml_reader.GetAttribute(name);
					if (String.IsNullOrEmpty(value_name))
					{
						return default_value;
					}
				}

				// Проверка на значение по умолчанию
				if (default_value != null && default_value.name == value_name) return default_value;

				// Получаем идентификатор
				Int32 id = -1;
				if (value_id.IsExists())
				{
					Int32.TryParse(value_id, out id);
				}

				// Объекта нет возвращаем значение по умолчанию
				if (id == -1)
				{
					return (default_value);
				}

				// Ищем
				game_object = XSerializatorUnity.FindGameObject(id, value_path, value_name, value_tag);

				if (game_object != null)
				{
					component = XSerializatorUnity.EnsureComponent(game_object, typeof(TComponent)) as TComponent;
					return (component);
				}
				else
				{
					return (default_value);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных ссылки на игровой объект в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Игровой объект Unity</returns>
			//---------------------------------------------------------------------------------------------------------
			public static GameObject ReadUnityGameObjectFromAttribute(this XmlReader xml_reader, String name, GameObject default_value)
			{
				GameObject game_object = null;

				// Должно быть записано идентификатор, имя, путь тэг и тип компонента
				String value_id = xml_reader.GetAttribute(name + ID);
				String value_name = xml_reader.GetAttribute(name + NAME);
				String value_path = xml_reader.GetAttribute(name + PATH);
				String value_tag = xml_reader.GetAttribute(name + TAG);

				// Если имени нет значит однозначно выходим
				if (String.IsNullOrEmpty(value_name))
				{
					// Пробуем такой вариант
					value_name = xml_reader.GetAttribute(name);
					if (String.IsNullOrEmpty(value_name))
					{
						return default_value;
					}
				}

				// Проверка на значение по умолчанию
				if (default_value != null && default_value.name == value_name) return default_value;

				// Получаем идентификатор
				Int32 id = -1;
				if (value_id.IsExists())
				{
					Int32.TryParse(value_id, out id);
				}

				// Объекта нет возвращаем значение по умолчанию
				if (id == -1)
				{
					return (default_value);
				}

				// Ищем
				game_object = XSerializatorUnity.FindGameObject(id, value_path, value_name, value_tag);

				if (game_object != null)
				{
					return (game_object);
				}
				else
				{
					return (default_value);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных ссылки на ресурс в формате атрибутов
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			/// <param name="name">Имя атрибута</param>
			/// <param name="default_value">Значение по умолчанию в случает отсутствия атрибута</param>
			/// <returns>Ресурс Unity</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TResource ReadUnityResourceFromAttribute<TResource>(this XmlReader xml_reader, String name,
				TResource default_value) where TResource : UnityEngine.Object
			{
				TResource resource = null;

				// Должно быть записано идентификатор, имя и тип ресурса
				String value_id = xml_reader.GetAttribute(name + ID);
				String value_name = xml_reader.GetAttribute(name + NAME);
				String value_type = xml_reader.GetAttribute(name + TYPE);

				// Если имени нет значит однозначно выходим
				if (String.IsNullOrEmpty(value_name))
				{
					// Пробуем такой вариант
					value_name = xml_reader.GetAttribute(name);
					if (String.IsNullOrEmpty(value_name))
					{
						return default_value;
					}
				}

				// Проверка на значение по умолчанию
				if(default_value != null && default_value.name == value_name) return default_value;

				// Получаем идентификатор
				Int32 id = -1;
				if (value_id.IsExists())
				{
					Int32.TryParse(value_id, out id);
				}

				// Объекта нет возвращаем значение по умолчанию
				if (id == -1)
				{
					return (default_value);
				}

				// Ищем
				resource = XSerializatorUnity.FindResource(id, value_name, typeof(TResource))as TResource;
				if(resource != null)
				{
					return (resource);
				}
				else
				{
					return (default_value);
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