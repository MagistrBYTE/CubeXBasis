﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXCommonSettings.cs
*		Настройки модуля общей функциональности применительно к режиму разработки и редактору Unity
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для определения настроек модуля общей функциональности применительно к режиму разработки и редактору Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XCommonEditorSettings
		{
			/// <summary>
			/// Относительный путь директории исходного кода модуля Common
			/// </summary>
			public const String SourcePath = XEditorSettings.SourceBasisPath + "Common/";

			/// <summary>
			/// Путь в размещении меню редактора общего модуля (для упорядочивания)
			/// </summary>
			public const String MenuPath = XEditorSettings.MenuPlace + "Common/";

			//
			// ПОДСИСТЕМА УТИЛИТ
			//
			/// <summary>
			/// Последовательность в размещении меню редактора подсистемы утилит общего модуля (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderUtility = XEditorSettings.MenuOrderCommon + 50;

			/// <summary>
			/// Путь в размещении меню редактора модуля подсистемы утилит общего модуля (для упорядочивания)
			/// </summary>
			public const String MenuPathUtility = MenuPath + "BaseUtility/";

			//
			// ПОДСИСТЕМА ЛОКАЛИЗАЦИИ
			//
			/// <summary>
			/// Последовательность в размещении меню редактора подсистемы локализации общего модуля (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderLocalization = XEditorSettings.MenuOrderCommon + 100;

			/// <summary>
			/// Путь в размещении меню редактора модуля подсистемы локализации общего модуля (для упорядочивания)
			/// </summary>
			public const String MenuPathLocalization = MenuPath + "Localization/";

			//
			// ПОДСИСТЕМА СЕРИАЛИЗАЦИИ
			//
			/// <summary>
			/// Последовательность в размещении меню редактора подсистемы сериализации общего модуля (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderSerialization = XEditorSettings.MenuOrderCommon + 150;

			/// <summary>
			/// Путь в размещении меню редактора модуля подсистемы сериализации общего модуля (для упорядочивания)
			/// </summary>
			public const String MenuPathSerialization = MenuPath + "Serialization/";

			//
			// ПОДСИСТЕМА АНИМАЦИИ
			//
			/// <summary>
			/// Последовательность в размещении меню редактора подсистемы анимации общего модуля (для упорядочивания)
			/// </summary>
			public const Int32 MenuOrderTween = XEditorSettings.MenuOrderCommon + 200;

			/// <summary>
			/// Путь в размещении меню редактора модуля подсистемы анимации общего модуля (для упорядочивания)
			/// </summary>
			public const String MenuPathTween = MenuPath + "Tween/";

			//
			// ПОСЛЕДНИЕ ПУНКТЫ МЕНЮ
			//
			/// <summary>
			/// Последовательность в размещении меню редактора общего модуля (для упорядочивания в конце)
			/// </summary>
			public const Int32 MenuOrderLast = XEditorSettings.MenuOrderCommon + 250;
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================