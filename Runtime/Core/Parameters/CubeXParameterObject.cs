﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема параметрических объектов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXParameterObject.cs
*		Определение класса для представления параметра значения которого представляет список параметров.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleParameters
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для представления параметра значения которого представляет список параметров
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CParameterObject : ParameterItem<ListArray<IParameterItem>>, ICubeXNotify
		{
			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Тип данных значения
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public override TParameterValueType ValueType
			{
				get { return TParameterValueType.ParameterObject; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CParameterObject()
			{
				mValue = new ListArray<IParameterItem>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="parameter_name">Имя параметра</param>
			/// <param name="parameters">Список параметров</param>
			//---------------------------------------------------------------------------------------------------------
			public CParameterObject(String parameter_name, params IParameterItem[] parameters)
				: base(parameter_name)
			{
				if(parameters != null && parameters.Length > 0)
				{
					for (Int32 i = 0; i < parameters.Length; i++)
					{
						if(parameters[i] != null)
						{
							parameters[i].IOwned = this;
						}
					}
				}

				mValue = new ListArray<IParameterItem>(parameters);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="id">Идентификатор параметра</param>
			/// <param name="parameters">Список параметров</param>
			//---------------------------------------------------------------------------------------------------------
			public CParameterObject(Int32 id, params IParameterItem[] parameters)
				: base(id)
			{
				if (parameters != null && parameters.Length > 0)
				{
					for (Int32 i = 0; i < parameters.Length; i++)
					{
						if (parameters[i] != null)
						{
							parameters[i].IOwned = this;
						}
					}
				}

				mValue = new ListArray<IParameterItem>(parameters);
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXNotify =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта о начале изменения данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого будут меняться</param>
			/// <param name="data_name">Имя данных</param>
			/// <returns>Статус разрешения/согласования изменения данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean OnNotifyUpdating(System.Object source, String data_name)
			{
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта об окончании изменении данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого изменились</param>
			/// <param name="data_name">Имя данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnNotifyUpdated(System.Object source, String data_name)
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