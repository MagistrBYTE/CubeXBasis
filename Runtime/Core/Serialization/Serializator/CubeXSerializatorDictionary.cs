﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема сериализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXSerializatorDictionary.cs
*		Сериализация словаря.
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
		/// Статический класс реализующий сериализацию словарей в различные форматы
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSerializatorDictionary
		{
			#region ======================================= МЕТОДЫ ЧТЕНИЯ/ЗАПИСИ XML ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запись словаря в формат элемента XML
			/// </summary>
			/// <param name="writer">Средство записи данных в формат XML</param>
			/// <param name="dictionary">Обобщённый словарь</param>
			/// <param name="element_name">Имя элемента</param>
			//---------------------------------------------------------------------------------------------------------
			public static void WriteDictionaryToXml(XmlWriter writer, IDictionary dictionary, String element_name)
			{

			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================