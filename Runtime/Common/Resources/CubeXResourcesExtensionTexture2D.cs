//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ресурсов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXResourcesExtensionTexture2D.cs
*		Работа с двухмерными текстурами.
*		Реализация дополнительных методов, константных данных, а также методов расширения функциональности ресурса Texture2D.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
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
		/// Статический класс реализующий дополнительные методы и константные данные при работе с двухмерными текстурами
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XTexture2D
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			private static Texture2D mWhiteAlpha25;
			private static Texture2D mWhiteAlpha50;
			private static Texture2D mWhiteAlpha75;
			private static Texture2D mWhite3х3Alpha10;
			private static Texture2D mWhite3х3Alpha25;
			private static Texture2D mBlackAlpha25;
			private static Texture2D mBlackAlpha50;
			private static Texture2D mBlackAlpha75;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Белая текстура
			/// </summary>
			public static Texture2D White
			{
				get
				{
					return Texture2D.whiteTexture;
				}
			}

			/// <summary>
			/// Белая текстура с прозрачностью 25
			/// </summary>
			public static Texture2D WhiteAlpha25
			{
				get
				{
					if (mWhiteAlpha25 == null)
					{
						mWhiteAlpha25 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
						mWhiteAlpha25.SetPixel(0, 0, new Color(1, 1, 1, 0.25f));
						mWhiteAlpha25.Apply();
					}
					return mWhiteAlpha25;
				}
			}

			/// <summary>
			/// Белая текстура с прозрачностью 50
			/// </summary>
			public static Texture2D WhiteAlpha50
			{
				get
				{
					if (mWhiteAlpha50 == null)
					{
						mWhiteAlpha50 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
						mWhiteAlpha50.SetPixel(0, 0, new Color(1, 1, 1, 0.5f));
						mWhiteAlpha50.Apply();
					}
					return mWhiteAlpha50;
				}
			}

			/// <summary>
			/// Белая текстура с прозрачностью 75
			/// </summary>
			public static Texture2D WhiteAlpha75
			{
				get
				{
					if (mWhiteAlpha75 == null)
					{
						mWhiteAlpha75 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
						mWhiteAlpha75.SetPixel(0, 0, new Color(1, 1, 1, 0.75f));
						mWhiteAlpha75.Apply();
					}
					return mWhiteAlpha75;
				}
			}

			/// <summary>
			/// Белая текстура размером 3х3 с прозрачностью на краях 10
			/// </summary>
			/// <remarks>
			/// Заполнение текстуры (степень прозрачности):
			/// (0.1) (0.1) (0.1)
			/// (1.0) (1.0) (1.0)
			/// (0.1) (0.1) (0.1)
			/// </remarks>
			public static Texture2D White3х3Alpha10
			{
				get
				{
					if (mWhite3х3Alpha10 == null)
					{
						Single opacity = 0.1f;
						mWhite3х3Alpha10 = new Texture2D(3, 3, TextureFormat.ARGB32, false);
						mWhite3х3Alpha10.SetPixel(0, 0, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha10.SetPixel(0, 1, Color.white);
						mWhite3х3Alpha10.SetPixel(0, 2, new Color(1, 1, 1, opacity));

						mWhite3х3Alpha10.SetPixel(1, 0, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha10.SetPixel(1, 1, Color.white);
						mWhite3х3Alpha10.SetPixel(1, 2, new Color(1, 1, 1, opacity));

						mWhite3х3Alpha10.SetPixel(2, 0, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha10.SetPixel(2, 1, Color.white);
						mWhite3х3Alpha10.SetPixel(2, 2, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha10.Apply();
					}
					return mWhite3х3Alpha10;
				}
			}

			/// <summary>
			/// Белая текстура размером 3х3 с прозрачностью на краях 25
			/// </summary>
			/// <remarks>
			/// Заполнение текстуры (степень прозрачности):
			/// (0.25) (0.25) (0.25)
			/// (1.0) (1.0) (1.0)
			/// (0.25) (0.25) (0.25)
			/// </remarks>
			public static Texture2D White3х3Alpha25
			{
				get
				{
					if (mWhite3х3Alpha25 == null)
					{
						Single opacity = 0.25f;
						mWhite3х3Alpha25 = new Texture2D(3, 3, TextureFormat.ARGB32, false);
						mWhite3х3Alpha25.SetPixel(0, 0, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha25.SetPixel(0, 1, Color.white);
						mWhite3х3Alpha25.SetPixel(0, 2, new Color(1, 1, 1, opacity));

						mWhite3х3Alpha25.SetPixel(1, 0, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha25.SetPixel(1, 1, Color.white);
						mWhite3х3Alpha25.SetPixel(1, 2, new Color(1, 1, 1, opacity));

						mWhite3х3Alpha25.SetPixel(2, 0, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha25.SetPixel(2, 1, Color.white);
						mWhite3х3Alpha25.SetPixel(2, 2, new Color(1, 1, 1, opacity));
						mWhite3х3Alpha25.Apply();
					}
					return mWhite3х3Alpha25;
				}
			}

			/// <summary>
			/// Черная текстура
			/// </summary>
			public static Texture2D Black
			{
				get
				{
					return Texture2D.blackTexture;
				}
			}

			/// <summary>
			/// Черная текстура с прозрачностью 25
			/// </summary>
			public static Texture2D BlackAlpha25
			{
				get
				{
					if (mBlackAlpha25 == null)
					{
						mBlackAlpha25 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
						mBlackAlpha25.SetPixel(0, 0, new Color(0, 0, 0, 0.25f));
						mBlackAlpha25.Apply();
					}
					return mBlackAlpha25;
				}
			}

			/// <summary>
			/// Черная текстура с прозрачностью 50
			/// </summary>
			public static Texture2D BlackAlpha50
			{
				get
				{
					if (mBlackAlpha50 == null)
					{
						mBlackAlpha50 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
						mBlackAlpha50.SetPixel(0, 0, new Color(0, 0, 0, 0.5f));
						mBlackAlpha50.Apply();
					}
					return mBlackAlpha50;
				}
			}

			/// <summary>
			/// Черная текстура с прозрачностью 75
			/// </summary>
			public static Texture2D BlackAlpha75
			{
				get
				{
					if (mBlackAlpha75 == null)
					{
						mBlackAlpha75 = new Texture2D(1, 1, TextureFormat.ARGB32, false);
						mBlackAlpha75.SetPixel(0, 0, new Color(0, 0, 0, 0.75f));
						mBlackAlpha75.Apply();
					}
					return mBlackAlpha75;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений ресурса Texture2D
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionTexture2D
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение основного спрайта текстуры в режиме редактора
			/// </summary>
			/// <param name="this">Текстура</param>
			/// <returns>Спрайт</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Sprite GetSpriteInDesign(this Texture2D @this)
			{
#if UNITY_EDITOR
				UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(
					UnityEditor.AssetDatabase.GetAssetPath(@this));
				if (all_sprites.Length > 0)
				{
					return all_sprites[0] as Sprite;
				}
				else
				{
					return null;
				}
#else
				return(null);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение массива спрайтов текстуры в режиме редактора
			/// </summary>
			/// <param name="this">Текстура</param>
			/// <returns>Массив спрайтов</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Sprite[] GetSpritesInDesign(this Texture2D @this)
			{
#if UNITY_EDITOR

				UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(
					UnityEditor.AssetDatabase.GetAssetPath(@this));
				if (all_sprites.Length > 0)
				{
					Sprite[] sprites = new Sprite[all_sprites.Length];

					for (Int32 i = 0; i < all_sprites.Length; i++)
					{
						sprites[i] = all_sprites[i] as Sprite;
					}

					return (sprites);
				}
				else
				{
					return null;
				}
#else
				return(null);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение бордюров основного спрайта текстуры в режиме редактора
			/// </summary>
			/// <param name="this">Текстура</param>
			/// <returns>Бордюры</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Vector4 GetSpriteBorderInDesign(this Texture2D @this)
			{
#if UNITY_EDITOR
				UnityEngine.Object[] all_sprites = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(
					UnityEditor.AssetDatabase.GetAssetPath(@this));
				if (all_sprites.Length > 0)
				{
					return (all_sprites[0] as Sprite).border;
				}
				else
				{
					return Vector4.zero;
				}
#else
				return(Vector4.zero);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение контраста текстуры в режиме редактора
			/// </summary>
			/// <param name="this">Текстура</param>
			/// <param name="value">Коэффициент изменения контраста от 0 до 1</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ChangeContrastInDesign(this Texture2D @this, Single value)
			{
				// Получаем пиксели
				Color[] pixels = @this.GetPixels();

				// 1) Вначале мы вычисляем среднее значение яркости изображения
				Double brightness = 0;
				for (Int32 y = 0; y < @this.height; y++)
				{
					for (Int32 x = 0; x < @this.width; x++)
					{
						Color pixel = pixels[x + y * @this.width];
						brightness += pixel.r * 0.299 + pixel.g * 0.587 + pixel.b * 0.114;
					}
				}

				// 2) Находим среднюю яркость
				Single ab = (Single)(brightness / (@this.height * @this.width));

				// Затем для каждого пикселя изображения для каждой из цветовых компонент R, G, и B мы должны найти 
				// отклонение от среднего значения яркости delta, и это отклонение умножить на коэффициент усиления(ослабления)
				// контраста value
				for (Int32 y = 0; y < @this.height; y++)
				{
					for (Int32 x = 0; x < @this.width; x++)
					{
						Color pixel = pixels[x + y * @this.width];
						pixel.r = pixel.r + (pixel.r - ab) * value;
						pixel.g = pixel.g + (pixel.g - ab) * value;
						pixel.b = pixel.b + (pixel.b - ab) * value;
						pixels[x + y * @this.width] = pixel;
					}
				}

				@this.SetPixels(pixels);
				@this.Apply();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение яркости текстуры в режиме редактора
			/// </summary>
			/// <param name="this">Текстура</param>
			/// <param name="value">Коэффициент изменения яркости от 0 до 1</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ChangeBrightnessInDesign(this Texture2D @this, Single value)
			{
				// Получаем пиксели
				Color[] pixels = @this.GetPixels();

				for (Int32 y = 0; y < @this.height; y++)
				{
					for (Int32 x = 0; x < @this.width; x++)
					{
						Color pixel = pixels[x + y * @this.width];

						// Цветовой тон
						Single hue;

						// Насыщенность
						// Чем больше этот параметр, тем «чище» цвет, поэтому этот параметр иногда называют чистотой цвета.
						// А чем ближе этот параметр к нулю, тем ближе цвет к нейтральному серому.
						Single saturation;

						// Яркость
						Single brightness;

						// Переводим в цветовую модель HSV
						Color.RGBToHSV(pixel, out hue, out saturation, out brightness);

						// Увеличиваем
						brightness += value;

						// Переводим назад
						pixels[x + y * @this.width] = Color.HSVToRGB(hue, saturation, brightness);
					}
				}

				@this.SetPixels(pixels);
				@this.Apply();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение насыщенности текстуры в режиме редактора
			/// </summary>
			/// <param name="this">Текстура</param>
			/// <param name="value">Коэффициент насыщенности яркости от 0 до 1</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ChangeSaturationInDesign(this Texture2D @this, Single value)
			{
				// Получаем пиксели
				Color[] pixels = @this.GetPixels();

				for (Int32 y = 0; y < @this.height; y++)
				{
					for (Int32 x = 0; x < @this.width; x++)
					{
						Color pixel = pixels[x + y * @this.width];

						// Цветовой тон
						Single hue;

						// Насыщенность
						// Чем больше этот параметр, тем «чище» цвет, поэтому этот параметр иногда называют чистотой цвета.
						// А чем ближе этот параметр к нулю, тем ближе цвет к нейтральному серому.
						Single saturation;

						// Яркость
						Single brightness;

						// Переводим в цветовую модель HSV
						Color.RGBToHSV(pixel, out hue, out saturation, out brightness);

						// Увеличиваем
						saturation += value;

						// Переводим назад
						pixels[x + y * @this.width] = Color.HSVToRGB(hue, saturation, brightness);
					}
				}

				@this.SetPixels(pixels);
				@this.Apply();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================