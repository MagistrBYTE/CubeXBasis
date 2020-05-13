//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Базовая подсистема Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseUnityLayers.cs
*		Общие данные для организации работы с различными слоями в Unity.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс содержащий константы используемых слоев расположения объектов Unity
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XLayer
		{
			//
			// ИМЕНА СЛОЕВ
			//
			/// <summary>
			/// Слой по умолчанию
			/// </summary>
			public const String DEFAULT = "Default";
			
			/// <summary>
			/// Слой для прозрачных объектов
			/// </summary>
			public const String TRANSPARENT_FX = "TransparentFX";
			
			/// <summary>
			/// Слой для объектов которые игнорируют коллизию
			/// </summary>
			public const String IGNORE_RAYCAST = "Ignore Raycast";
			
			/// <summary>
			/// Слой для объектов которые представляют собой воду
			/// </summary>
			public const String WATER = "Water";
			
			/// <summary>
			/// Слой для объектов пользовательского интерфейса
			/// </summary>
			public const String UI = "UI";
			
			//
			// ЧИСЛОВЫЕ КОДЫ СЛОЕВ
			//
			/// <summary>
			/// Слой по умолчанию
			/// </summary>
			public static readonly Int32 Default_ID = LayerMask.NameToLayer(DEFAULT);
			
			/// <summary>
			/// Слой для прозрачных объектов
			/// </summary>
			public static readonly Int32 TransparentFX_ID = LayerMask.NameToLayer(TRANSPARENT_FX);
			
			/// <summary>
			/// Слой для объектов которые игнорируют коллизию
			/// </summary>
			public static readonly Int32 IgnoreRaycast_ID = LayerMask.NameToLayer(IGNORE_RAYCAST);
			
			/// <summary>
			/// Слой для объектов которые представляют собой воду
			/// </summary>
			public static readonly Int32 Water_ID = LayerMask.NameToLayer(WATER);
			
			/// <summary>
			/// Слой для объектов пользовательского интерфейса
			/// </summary>
			public static readonly Int32 UI_ID = LayerMask.NameToLayer(UI);
			
			//
			// МАСКИ СЛОЕВ
			//
			/// <summary>
			/// Маска для слоя по умолчанию
			/// </summary>
			public static readonly Int32 MaskDefault = 1 << Default_ID;
			
			/// <summary>
			/// Маска для слоя прозрачных объектов
			/// </summary>
			public static readonly Int32 MaskTransparentFX = 1 << TransparentFX_ID;
			
			/// <summary>
			/// Маска для слоя объектов которые игнорируют коллизию
			/// </summary>
			public static readonly Int32 MaskIgnoreRaycast = 1 << IgnoreRaycast_ID;
			
			/// <summary>
			/// Маска для слоя объектов которые представляют собой воду
			/// </summary>
			public static readonly Int32 MaskWater = 1 << Water_ID;
			
			/// <summary>
			/// Маска для слоя объектов пользовательского интерфейса
			/// </summary>
			public static readonly Int32 MaskUI = 1 << UI_ID;
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================