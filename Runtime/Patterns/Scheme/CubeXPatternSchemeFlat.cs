//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема схем данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternSchemeFlat.cs
*		Плоская (линейна) модель организации данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternScheme
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Схема данных представляющая собой плоскую модель
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CSchemeFlatData : IComparable<CSchemeFlatData>, ICloneable, ICubeXSerializeImplementationXML
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя элемента XML для обозначения схемы
			/// </summary>
			public const String XML_ELEMENT = "Scheme";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mName = "";
			protected internal String mDesc = "";
			protected internal ListArray<CSchemeDataProperty> mColumns;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя схемы данных
			/// </summary>
			[Browsable(false)]
			public String Name
			{
				get { return (mName); }
				set
				{
					mName = value;
				}
			}

			/// <summary>
			/// Описание схемы данных
			/// </summary>
			[Browsable(false)]
			public String Desc
			{
				get { return (mDesc); }
				set
				{
					mDesc = value;
				}
			}

			/// <summary>
			/// Список столбцов с данными
			/// </summary>
			[Browsable(false)]
			public ListArray<CSchemeDataProperty> Columns
			{
				get { return (mColumns); }
				set
				{
					mColumns = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeFlatData()
				: this(String.Empty, String.Empty, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя схемы данных</param>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeFlatData(String name)
				: this(name, String.Empty, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя схемы данных</param>
			/// <param name="desc">Описание схемы данных</param>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeFlatData(String name, String desc)
				: this(name, desc, null)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя схемы данных</param>
			/// <param name="items">Список свойств/колонок</param>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeFlatData(String name, params CSchemeDataProperty[] items)
				: this(name, String.Empty, items)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя схемы данных</param>
			/// <param name="desc">Описание схемы данных</param>
			/// <param name="items">Список свойств/колонок</param>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeFlatData(String name, String desc, params CSchemeDataProperty[] items)
			{
				mName = name;
				mDesc = desc;
				if (items != null && items.Length > 0)
				{
					mColumns = new ListArray<CSchemeDataProperty>(items);
				}
				else
				{
					mColumns = new ListArray<CSchemeDataProperty>();
				}
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый объект</param>
			/// <returns>Статус сравнения объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CSchemeFlatData other)
			{
				return (mName.CompareTo(other.Name));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение копии объекта
			/// </summary>
			/// <returns>Копия объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object Clone()
			{
				CSchemeFlatData clone = new CSchemeFlatData(mName);

				for (Int32 i = 0; i < mColumns.Count; i++)
				{
					clone.Columns.Add(mColumns[i].Duplicate());
				}

				return (clone);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Наименование объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (mName);
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXSerializeImplementationXML ===================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись данных объекта в формат данных XML
			/// </summary>
			/// <param name="xml_writer">Средство записи данных в формат XML</param>
			//---------------------------------------------------------------------------------------------------------
			public void WriteToXml(XmlWriter xml_writer)
			{
				xml_writer.WriteStartElement(XML_ELEMENT);
				{
					xml_writer.WriteAttributeString(nameof(Name), mName);
					xml_writer.WriteAttributeString(nameof(Desc), mDesc);

					for (Int32 i = 0; i < mColumns.Count; i++)
					{
						CSchemeDataProperty column = mColumns[i];
						column.WriteToXml(xml_writer);
					}
				}
				xml_writer.WriteEndElement();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных объекта из данных в формате XML
			/// </summary>
			/// <param name="xml_reader">Средство чтения данных формата XML</param>
			//---------------------------------------------------------------------------------------------------------
			public void ReadFromXml(XmlReader xml_reader)
			{
				if (xml_reader.MoveToElement(XML_ELEMENT))
				{
					mName = xml_reader.ReadStringFromAttribute(nameof(Name), mName);
					mDesc = xml_reader.ReadStringFromAttribute(nameof(Desc), mDesc);
					mColumns.Clear();

					XmlReader reader_subtree = xml_reader.ReadSubtree();
					while (reader_subtree.Read())
					{
						if (reader_subtree.NodeType == XmlNodeType.Element && reader_subtree.Name == CSchemeDataProperty.XML_ELEMENT)
						{
							CSchemeDataProperty property = new CSchemeDataProperty();
							property.ReadFromXml(xml_reader);
							mColumns.Add(property);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Чтение данных объекта из данных в формате XML
			/// </summary>
			/// <param name="xml_element">Узел документа XML представляющего элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public void ReadFromXml(XmlElement xml_element)
			{
				if (xml_element.Name == XML_ELEMENT)
				{
					Name = xml_element.GetAttributeValueFromName(nameof(Name), Name);
					Desc = xml_element.GetAttributeValueFromName(nameof(Desc), Desc);

					var childs = xml_element.ChildNodes;
					for (Int32 i = 0; i < childs.Count; i++)
					{
						if(childs[i].NodeType == XmlNodeType.Element && childs[i].Name == CSchemeDataProperty.XML_ELEMENT)
						{
							CSchemeDataProperty property = new CSchemeDataProperty();
							property.ReadFromXml(childs[i] as XmlElement);
							mColumns.Add(property);
						}
					}
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================