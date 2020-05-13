//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема схем данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternSchemeCommon.cs
*		Общие типы и структуры данных подсистемы схем данных.
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
		//! \defgroup CorePatternScheme Подсистема схем данных
		//! Подсистема схем данных определяет как организованы данные в различных источника и как их отображать.
		//! Помимо этого, подсистема определяет также некоторые аспекты отображения в соответствующих элементах управления.
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Тип организации данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TSchemeDataType
		{
			/// <summary>
			/// Плоская модель
			/// </summary>
			/// <remarks>
			/// Плоская модель данных подразумевает хранение данных обычных POD типов, как правило объект представляет 
			/// собой отдельную запись в таблице базы данных(листе Excel) а столбцы соответствуют полям/свойствам объекта
			/// </remarks>
			Flat
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Описание свойства/поля данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public class CSchemeDataProperty : IComparable<CSchemeDataProperty>, ICubeXDuplicate<CSchemeDataProperty>, 
			ICloneable, ICubeXSerializeImplementationXML
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя элемента XML для обозначения свойства
			/// </summary>
			public const String XML_ELEMENT = "Column";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mName = "";
			protected internal TypeCode mDataTypeCode;
			protected internal Int32 mColumnIndex;
			protected internal String mCaption;
			protected internal Boolean mIsHide;
			protected internal Boolean mIsMainColumn;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя колонки (свойства/поля)
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
			/// Тип данных колонки (свойства/поля)
			/// </summary>
			[Browsable(false)]
			public Type DataType
			{
				get
				{ 
					return (mDataTypeCode.ToType()); 
				}
			}

			/// <summary>
			/// Код типа данных колонки (свойства/поля)
			/// </summary>
			[Browsable(false)]
			public TypeCode DataTypeCode
			{
				get { return (mDataTypeCode); }
				set
				{
					mDataTypeCode = value;
				}
			}

			/// <summary>
			/// Индекс колонки
			/// </summary>
			[Browsable(false)]
			public Int32 ColumnIndex
			{
				get { return (mColumnIndex); }
				set
				{
					mColumnIndex = value;
				}
			}

			/// <summary>
			/// Наименование заголовка для элемента управления
			/// </summary>
			[Browsable(false)]
			public String Caption
			{
				get { return (mCaption); }
				set
				{
					mCaption = value;
				}
			}

			/// <summary>
			/// Статус скрытия колонки в элементе управления
			/// </summary>
			[Browsable(false)]
			public Boolean IsHide
			{
				get { return (mIsHide); }
				set
				{
					mIsHide = value;
				}
			}

			/// <summary>
			/// Статус основной колонки
			/// </summary>
			[Browsable(false)]
			public  Boolean IsMainColumn
			{
				get { return (mIsMainColumn); }
				set
				{
					mIsMainColumn = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeDataProperty()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя колонки (свойства/поля)</param>
			/// <param name="type_code">Тип данных колонки (свойства/поля)</param>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeDataProperty(String name, TypeCode type_code)
			{
				mName = name;
				mDataTypeCode = type_code;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя колонки (свойства/поля)</param>
			/// <param name="type_code">Тип данных колонки (свойства/поля)</param>
			/// <param name="caption">Наименование заголовка для элемента управления</param>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeDataProperty(String name, TypeCode type_code, String caption)
			{
				mName = name;
				mDataTypeCode = type_code;
				mCaption = caption;
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
			public Int32 CompareTo(CSchemeDataProperty other)
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
				CSchemeDataProperty clone = (CSchemeDataProperty)this.MemberwiseClone();
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
				return (mName + "[" + DataTypeCode.ToString() + "]");
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXDuplicate ====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дубликата объекта
			/// </summary>
			/// <returns>Дубликат объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSchemeDataProperty Duplicate()
			{
				CSchemeDataProperty clone = (CSchemeDataProperty)this.MemberwiseClone();
				return (clone);
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
					xml_writer.WriteAttributeString(nameof(Name), Name);
					xml_writer.WriteEnumToAttribute(nameof(DataTypeCode), DataTypeCode);
					xml_writer.WriteIntegerToAttribute(nameof(ColumnIndex), ColumnIndex);
					xml_writer.WriteAttributeString(nameof(Caption), Caption);
					xml_writer.WriteBooleanToAttribute(nameof(IsHide), IsHide);
					xml_writer.WriteBooleanToAttribute(nameof(IsMainColumn), IsMainColumn);
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
					Name = xml_reader.ReadStringFromAttribute(nameof(Name), Name);
					mDataTypeCode = xml_reader.ReadEnumFromAttribute(nameof(DataTypeCode), DataTypeCode);
					ColumnIndex = xml_reader.ReadIntegerFromAttribute(nameof(ColumnIndex), ColumnIndex);
					Caption = xml_reader.ReadStringFromAttribute(nameof(Caption), Caption);
					IsHide = xml_reader.ReadBooleanFromAttribute(nameof(IsHide), IsHide);
					IsMainColumn = xml_reader.ReadBooleanFromAttribute(nameof(IsMainColumn), IsMainColumn);
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
					mDataTypeCode = xml_element.GetAttributeValueFromNameAsEnum(nameof(DataTypeCode), DataTypeCode);
					ColumnIndex = xml_element.GetAttributeValueFromNameAsInteger(nameof(ColumnIndex), ColumnIndex);
					Caption = xml_element.GetAttributeValueFromName(nameof(Caption), Caption);
					IsHide = xml_element.GetAttributeValueFromNameAsBoolean(nameof(IsHide), IsHide);
					IsMainColumn = xml_element.GetAttributeValueFromNameAsBoolean(nameof(IsMainColumn), IsMainColumn);
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