﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль алгоритмов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXAlgorithmSettings.cs
*		Настройки модуля алгоритмов применительно к режиму разработки и редактору Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Runtime.CompilerServices;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Algorithm
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup AlgorithmModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для определения настроек модуля алгоритмов применительно к режиму разработки и редактору Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XAlgorithmEditorSettings
		{
			/// <summary>
			/// Относительный путь директории исходного кода модуля алгоритмов
			/// </summary>
			public const String SourcePath = XEditorSettings.SourceBasisPath + "Algorithm/";

			/// <summary>
			/// Путь в размещении меню редактора модуля алгоритмов (для упорядочивания)
			/// </summary>
			public const String MenuPath = XEditorSettings.MenuPlace + "Algorithm/";
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================