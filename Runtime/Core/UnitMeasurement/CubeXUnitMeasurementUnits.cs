﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема единиц измерения
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXUnitMeasurementUnits.cs
*		Определение общих классов и структур данных для работы с единицами измерений.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleUnitMeasurement
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений для типов единиц измерения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XUnitType
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение соответствующего типа измерения от указанного единицы измерения
			/// </summary>
			/// <param name="unit_type">Единица измерения</param>
			/// <returns>Тип измерения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static TMeasurementType GetMeasurementType(Enum unit_type)
			{
				Type type = unit_type.GetType();
				TMeasurementType measurement_type = TMeasurementType.Undefined;
				switch (type.Name)
				{
					case nameof(TUnitThing):
						{
							measurement_type = TMeasurementType.Thing;
						}
						break;
					case nameof(TUnitLength):
						{
							measurement_type = TMeasurementType.Length;
						}
						break;
					case nameof(TUnitArea):
						{
							measurement_type = TMeasurementType.Area;
						}
						break;
					default:
						break;
				}

				return (measurement_type);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================