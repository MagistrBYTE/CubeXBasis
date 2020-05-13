//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ресурсов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXResourcesExtensionMaterial.cs
*		Работа с материалами.
*		Реализация дополнительных методов и константных данных при работе с материалами.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
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
		//! \addtogroup UnityCommonResource
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий дополнительные методы и константные данные при работе с материалами
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XMaterial
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя обычного материала красного цвета
			/// </summary>
			public const String DIFFUSE_RED_NAME = "DiffuseRed";

			/// <summary>
			/// Имя обычного материала зеленого цвета
			/// </summary>
			public const String DIFFUSE_GREEN_NAME = "DiffuseGreen";

			/// <summary>
			/// Имя обычного материала синего цвета
			/// </summary>
			public const String DIFFUSE_BLUE_NAME = "DiffuseBlue";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			private static Material mDefault;
			private static Material mDefaultLine;
			private static Material mDiffuseRed;
			private static Material mDiffuseGreen;
			private static Material mDiffuseBlue;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Материал по умолчанию
			/// </summary>
			public static Material Default
			{
				get
				{
					if (mDefault == null)
					{
						LoadDefault();
					}

					return (mDefault);
				}
			}

			/// <summary>
			/// Материал для рисования линии
			/// </summary>
			public static Material DefaultLine
			{
				get
				{
					if (mDefaultLine == null)
					{
						LoadDefault();
					}

					return (mDefaultLine);
				}
			}

			/// <summary>
			/// Материал красного цвета 
			/// </summary>
			public static Material DiffuseRed
			{
				get
				{
					if (mDiffuseRed == null)
					{
						LoadDefault();
					}

					return (mDiffuseRed);
				}
			}

			/// <summary>
			/// Материал зеленого цвета
			/// </summary>
			public static Material DiffuseGreen
			{
				get
				{
					if (mDiffuseGreen == null)
					{
						LoadDefault();
					}

					return (mDiffuseGreen);
				}
			}

			/// <summary>
			/// Материал синего цвета
			/// </summary>
			public static Material DiffuseBlue
			{
				get
				{
					if (mDiffuseBlue == null)
					{
						LoadDefault();
					}

					return (mDiffuseBlue);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка материалов по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			internal static void LoadDefault()
			{
				Material[] materials = Resources.FindObjectsOfTypeAll<Material>();
				if (materials.Length > 0)
				{
					for (Int32 i = 0; i < materials.Length; i++)
					{
						if (materials[i].name.Equal("Default-Material"))
						{
							mDefault = new Material(materials[i]);
							continue;
						}
						if (materials[i].name.Equal("Default-Line"))
						{
							mDefaultLine = new Material(materials[i]);
							continue;
						}
					}
				}

				mDiffuseRed = new Material(mDefault);
				mDiffuseRed.name = DIFFUSE_RED_NAME;
				mDiffuseRed.color = Color.red;

				mDiffuseGreen = new Material(mDefault);
				mDiffuseGreen.name = DIFFUSE_GREEN_NAME;
				mDiffuseGreen.color = Color.green;

				mDiffuseBlue = new Material(mDefault);
				mDiffuseBlue.name = DIFFUSE_BLUE_NAME;
				mDiffuseBlue.color = Color.blue;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================