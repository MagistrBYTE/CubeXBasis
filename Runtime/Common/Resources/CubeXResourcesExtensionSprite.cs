//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ресурсов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXResourcesExtensionSprite.cs
*		Работа со спрайтами.
*		Реализация дополнительных методов и константных данных при работе со спрайтами.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
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
		/// Статический класс реализующий дополнительные методы и константные данные при работе со спрайтами
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XSprite
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя спрайта по умолчанию
			/// </summary>
			public const String DEFAULT_NAME = "UISprite";

			/// <summary>
			/// Имя фонового спрайта
			/// </summary>
			public const String BACKGROUND_NAME = "Background";

			/// <summary>
			/// Имя спрайта маски
			/// </summary>
			public const String MASK_NAME = "UIMask";

			/// <summary>
			/// Имя спрайта Knob
			/// </summary>
			public const String KNOB_NAME = "Knob";

			/// <summary>
			/// Имя спрайта Checkmark
			/// </summary>
			public const String CHECKMARK_NAME = "Checkmark";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			private static Sprite mDefault;
			private static Sprite mBackground;
			private static Sprite mMask;
			private static Sprite mKnob;
			private static Sprite mCheckmark;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Спрайт по умолчанию
			/// </summary>
			public static Sprite Default
			{
				get
				{
					if (mDefault == null)
					{
						LoadDefault();
					}

#if UNITY_EDITOR
					if (mDefault == null)
					{
						mDefault = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
					}
#endif

					return (mDefault);
				}
			}

			/// <summary>
			/// Спрайт фонового изображения
			/// </summary>
			public static Sprite Background
			{
				get
				{
					if (mBackground == null)
					{
						LoadDefault();
					}

					return (mBackground);
				}
			}

			/// <summary>
			/// Спрайт маски
			/// </summary>
			public static Sprite Mask
			{
				get
				{
					if (mMask == null)
					{
						LoadDefault();
					}

					return (mMask);
				}
			}

			/// <summary>
			/// Спрайт Knob
			/// </summary>
			public static Sprite Knob
			{
				get
				{
					if (mKnob == null)
					{
						LoadDefault();
					}

					return (mKnob);
				}
			}

			/// <summary>
			/// Спрайт Checkmark
			/// </summary>
			public static Sprite Checkmark
			{
				get
				{
					if (mCheckmark == null)
					{
						LoadDefault();
					}

					return (mCheckmark);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка спрайтов по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			internal static void LoadDefault()
			{
				Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();
				if (sprites.Length > 0)
				{
					for (Int32 i = 0; i < sprites.Length; i++)
					{
						if (DEFAULT_NAME.Equal(sprites[i].name))
						{
							mDefault = sprites[i];
							continue;
						}

						if (BACKGROUND_NAME.Equal(sprites[i].name))
						{
							mBackground = sprites[i];
							continue;
						}

						if (MASK_NAME.Equal(sprites[i].name))
						{
							mMask = sprites[i];
							continue;
						}

						if (KNOB_NAME.Equal(sprites[i].name))
						{
							mKnob = sprites[i];
							continue;
						}

						if (CHECKMARK_NAME.Equal(sprites[i].name))
						{
							mCheckmark = sprites[i];
							continue;
						}
					}
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОИСКА =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса спрайта по имени
			/// </summary>
			/// <param name="name">Имя спрайта</param>
			/// <returns>Найденный ресурс спрайта или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Sprite Find(String name)
			{
				// 1) Ищем среди загруженных ресурсов
				Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();

				for (Int32 i = 0; i < sprites.Length; i++)
				{
					if (sprites[i].name == name)
					{
						return sprites[i];
					}
				}

#if UNITY_EDITOR
				//2) Ищем среди одноименной текстуры
				Texture2D texture_sprite = XResourcesDispatcher.Find<Texture2D>(name);
				if (texture_sprite != null)
				{
					return texture_sprite.GetSpriteInDesign();
				}

				//3) Ищем среди всех текстур
				Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
				if (textures != null && textures.Length > 0)
				{
					for (Int32 it = 0; it < textures.Length; it++)
					{
						UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(
						UnityEditor.AssetDatabase.GetAssetPath(textures[it]));
						if (all_sprites != null && all_sprites.Length > 0)
						{
							for (Int32 i = 0; i < all_sprites.Length; i++)
							{
								if (all_sprites[i].name == name)
								{
									return all_sprites[i] as Sprite;
								}
							}
						}
					}
				}
#endif
				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск ресурса спрайта по вхождению имени
			/// </summary>
			/// <param name="name">Имя спрайта</param>
			/// <returns>Найденный ресурс спрайта или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Sprite FindMatch(String name)
			{
				// 1) Ищем среди загруженных ресурсов
				Sprite[] sprites = Resources.FindObjectsOfTypeAll<Sprite>();

				for (Int32 i = 0; i < sprites.Length; i++)
				{
					if (sprites[i].name.Contains(name))
					{
						return sprites[i];
					}
				}

#if UNITY_EDITOR
				//2) Ищем среди одноименной текстуры
				Texture2D texture_sprite = XResourcesDispatcher.Find<Texture2D>(name);
				if (texture_sprite != null)
				{
					return texture_sprite.GetSpriteInDesign();
				}

				//3) Ищем среди всех текстур
				Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
				if (textures != null && textures.Length > 0)
				{
					for (Int32 it = 0; it < textures.Length; it++)
					{
						UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(
						UnityEditor.AssetDatabase.GetAssetPath(textures[it]));
						if (all_sprites != null && all_sprites.Length > 0)
						{
							for (Int32 i = 0; i < all_sprites.Length; i++)
							{
								if (all_sprites[i].name.Contains(name))
								{
									return all_sprites[i] as Sprite;
								}
							}
						}
					}
				}
#endif
				return null;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================