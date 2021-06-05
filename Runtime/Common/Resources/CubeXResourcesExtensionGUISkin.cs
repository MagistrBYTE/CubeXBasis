//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема ресурсов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXResourcesExtensionGUISkin.cs
*		Работа со стандартным ресурсом GUISkin
*		Реализация дополнительных методов, константных данных, а также методов расширения функциональности ресурса GUISkin.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
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
		/// Статический класс реализующий дополнительные методы и константные данные при работе с GUISkin
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XGUISkin
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Имя основного скина GUI
			/// </summary>
			public const String DEFAULT_SKIN_NAME = "DarkKit";

			/// <summary>
			/// Путь для ресурса GUI скина
			/// </summary>
			/// <remarks>
			/// Указанный ресурс является основным визуальном стилем для отображения элементов
			/// </remarks>
#if UNITY_EDITOR
			public const String DEFAULT_SKIN_PATH = XEditorSettings.ResourcesUIPath + "DarkKit/DarkKit.guiskin";
#else
			public const String DEFAULT_SKIN_PATH = XEditorSettings.ResourcesUIPath + DEFAULT_SKIN_NAME;
#endif

			//
			// ТИПОВЫЕ ИМЕНА СТИЛЕЙ
			//
			//
			// ПРОСТОЙ ТЕКСТ
			//
			/// <summary>
			/// Имя визуального стиля для рисования простого текста
			/// </summary>
			public const String TEXT_STYLE_NAME = "Text";

			/// <summary>
			/// Имя визуального стиля для рисования моноширинного текста
			/// </summary>
			public const String TEXT_MONO_STYLE_NAME = "TextMono";

			/// <summary>
			/// Имя визуального стиля для рисования текста заголовка
			/// </summary>
			public const String TEXT_HEADER_STYLE_NAME = "TextHeader";

			/// <summary>
			/// Имя визуального стиля для рисования текста значения
			/// </summary>
			public const String TEXT_VALUE_STYLE_NAME = "TextValue";

			/// <summary>
			/// Имя визуального стиля для рисования текста столбца
			/// </summary>
			public const String TEXT_COLUMN_STYLE_NAME = "TextColumn";

			/// <summary>
			/// Имя визуального стиля для рисования текста меньшим шрифтом
			/// </summary>
			public const String TEXT_SMALL_STYLE_NAME = "TextSmall";

			//
			// НАДПИСИ
			//
			/// <summary>
			/// Имя визуального стиля для рисования общей надписи
			/// </summary>
			public const String LABEL_STYLE_NAME = "label";

			/// <summary>
			/// Имя визуального стиля для рисования моноширинной надписи
			/// </summary>
			public const String LABEL_MONO_STYLE_NAME = "LabelMono";

			/// <summary>
			/// Имя визуального стиля для рисования надписи заголовка
			/// </summary>
			public const String LABEL_HEADER_STYLE_NAME = "LabelHeader";

			/// <summary>
			/// Имя визуального стиля для рисования надписи значения
			/// </summary>
			public const String LABEL_VALUE_STYLE_NAME = "LabelValue";

			/// <summary>
			/// Имя визуального стиля для рисования надписи столбца
			/// </summary>
			public const String LABEL_COLUMN_STYLE_NAME = "LabelColumn";

			/// <summary>
			/// Имя визуального стиля для рисования надписи меньшим шрифтом
			/// </summary>
			public const String LABEL_SMALL_STYLE_NAME = "LabelSmall";

			//
			// ПАНЕЛИ
			//
			/// <summary>
			/// Имя визуального стиля для рисования общей панели
			/// </summary>
			public const String PANEL_STYLE_NAME = "box";

			/// <summary>
			/// Имя визуального стиля для рисования панели группирования
			/// </summary>
			public const String PANEL_GROUP_STYLE_NAME = "PanelGroup";

			/// <summary>
			/// Имя визуального стиля для рисования панели с заголовком
			/// </summary>
			public const String PANEL_HEADER_STYLE_NAME = "PanelHeader";

			/// <summary>
			/// Имя визуального стиля для рисования панели значения
			/// </summary>
			public const String PANEL_VALUE_STYLE_NAME = "PanelValue";

			/// <summary>
			/// Имя визуального стиля для рисования панели столбца
			/// </summary>
			public const String PANEL_COLUMN_STYLE_NAME = "PanelColumn";

			//
			// КНОПКИ
			//
			/// <summary>
			/// Имя визуального стиля для рисования некоторых управляющих кнопок
			/// </summary>
			public const String BUTTON_MINI_STYLE_NAME = "ButtonMini";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			internal static GUISkin mDefaultSkin;
			internal static GUIStyle mTextStyle;
			internal static GUIStyle mTextMonoStyle;
			internal static GUIStyle mTextHeaderStyle;
			internal static GUIStyle mTextValueStyle;
			internal static GUIStyle mTextColumnStyle;
			internal static GUIStyle mTextSmallStyle;

			internal static GUIStyle mLabelStyle;
			internal static GUIStyle mLabelMonoStyle;
			internal static GUIStyle mLabelHeaderStyle;
			internal static GUIStyle mLabelValueStyle;
			internal static GUIStyle mLabelColumnStyle;
			internal static GUIStyle mLabelSmallStyle;
		
			internal static GUIStyle mPanelStyle;
			internal static GUIStyle mPanelGroupStyle;
			internal static GUIStyle mPanelHeaderStyle;
			internal static GUIStyle mPanelValueStyle;
			internal static GUIStyle mPanelColumnStyle;

			internal static GUIStyle mButtonMiniStyle;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Проверка на существование визуального скина по умолчанию
			/// </summary>
			public static Boolean IsDefaultSkinNull
			{
				get
				{
					return (mDefaultSkin == null);
				}
			}

			/// <summary>
			/// Текущий визуальный скин для рисования
			/// </summary>
			public static GUISkin DefaultSkin
			{
				get
				{
					if(mDefaultSkin == null)
					{
						LoadDefault();
					}
					return mDefaultSkin;
				}
				set
				{
					mDefaultSkin = value;
				}
			}

			//
			// ПРОСТОЙ ТЕКСТ
			//
			/// <summary>
			/// Визуальный стиль для рисования простого текста
			/// </summary>
			public static GUIStyle TextStyle
			{
				get
				{
					if (mTextStyle == null)
					{
						LoadDefault();
					}

					return (mTextStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования для рисования моноширинного текста
			/// </summary>
			public static GUIStyle TextMonoStyle
			{
				get
				{
					if (mTextMonoStyle == null)
					{
						LoadDefault();
					}

					return (mTextMonoStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста заголовка
			/// </summary>
			public static GUIStyle TextHeaderStyle
			{
				get
				{
					if (mTextHeaderStyle == null)
					{
						LoadDefault();
					}

					return (mTextHeaderStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста значения
			/// </summary>
			public static GUIStyle TextValueStyle
			{
				get
				{
					if (mTextValueStyle == null)
					{
						LoadDefault();
					}

					return (mTextValueStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста столбца
			/// </summary>
			public static GUIStyle TextColumnStyle
			{
				get
				{
					if (mTextColumnStyle == null)
					{
						LoadDefault();
					}

					return (mTextColumnStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования текста меньшим шрифтом
			/// </summary>
			public static GUIStyle TextSmallStyle
			{
				get
				{
					if (mTextSmallStyle == null)
					{
						LoadDefault();
					}

					return (mTextSmallStyle);
				}
			}

			//
			// НАДПИСИ
			//
			/// <summary>
			/// Визуальный стиль для рисования простой надписи
			/// </summary>
			public static GUIStyle LabelStyle
			{
				get
				{
					if (mLabelStyle == null)
					{
						LoadDefault();
					}

					return (mLabelStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования для рисования моноширинной надписи
			/// </summary>
			public static GUIStyle LabelMonoStyle
			{
				get
				{
					if (mLabelMonoStyle == null)
					{
						LoadDefault();
					}

					return (mLabelMonoStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи заголовка
			/// </summary>
			public static GUIStyle LabelHeaderStyle
			{
				get
				{
					if (mLabelHeaderStyle == null)
					{
						LoadDefault();
					}

					return (mLabelHeaderStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи значения
			/// </summary>
			public static GUIStyle LabelValueStyle
			{
				get
				{
					if (mLabelValueStyle == null)
					{
						LoadDefault();
					}

					return (mLabelValueStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи столбца
			/// </summary>
			public static GUIStyle LabelColumnStyle
			{
				get
				{
					if (mLabelColumnStyle == null)
					{
						LoadDefault();
					}

					return (mLabelColumnStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования надписи меньшим шрифтом
			/// </summary>
			public static GUIStyle LabelSmallStyle
			{
				get
				{
					if (mLabelSmallStyle == null)
					{
						LoadDefault();
					}

					return (mLabelSmallStyle);
				}
			}

			//
			// ПАНЕЛИ
			//
			/// <summary>
			/// Визуальный стиль для рисования общей панели
			/// </summary>
			public static GUIStyle PanelStyle
			{
				get
				{
					if (mPanelStyle == null)
					{
						LoadDefault();
					}

					return (mPanelStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели группирования
			/// </summary>
			public static GUIStyle PanelGroupStyle
			{
				get
				{
					if (mPanelGroupStyle == null)
					{
						LoadDefault();
					}

					return (mPanelGroupStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели с заголовком
			/// </summary>
			public static GUIStyle PanelHeaderStyle
			{
				get
				{
					if (mPanelHeaderStyle == null)
					{
						LoadDefault();
					}

					return (mPanelHeaderStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели значения
			/// </summary>
			public static GUIStyle PanelValueStyle
			{
				get
				{
					if (mPanelValueStyle == null)
					{
						LoadDefault();
					}

					return (mPanelValueStyle);
				}
			}

			/// <summary>
			/// Визуальный стиль для рисования панели столбца
			/// </summary>
			public static GUIStyle PanelColumnStyle
			{
				get
				{
					if (mPanelColumnStyle == null)
					{
						LoadDefault();
					}

					return (mPanelColumnStyle);
				}
			}

			//
			// КНОПКИ
			//
			/// <summary>
			/// Визуальный стиль для рисования некоторых управляющих кнопок
			/// </summary>
			public static GUIStyle ButtonMiniStyle
			{
				get
				{
					if (mButtonMiniStyle == null)
					{
						LoadDefault();
					}

					return mButtonMiniStyle;
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка визуального скина по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			internal static void LoadDefault()
			{
				if (mDefaultSkin == null)
				{
					GUISkin[] skins = Resources.FindObjectsOfTypeAll<GUISkin>();

					for (Int32 i = 0; i < skins.Length; i++)
					{
						if (DEFAULT_SKIN_NAME.Equal(skins[i].name))
						{
							mDefaultSkin = skins[i];
							break;
						}
					}

#if UNITY_EDITOR
					if (mDefaultSkin == null)
					{
						mDefaultSkin = UnityEditor.AssetDatabase.LoadAssetAtPath<GUISkin>(DEFAULT_SKIN_PATH);
					}
#else
					if (mDefaultSkin == null)
					{
						mDefaultSkin = Resources.Load<GUISkin>(DEFAULT_SKIN_PATH);
					}
#endif
					if (mDefaultSkin == null && skins.Length > 0)
					{
						mDefaultSkin = skins[0];
					}
				}

				if(mDefaultSkin != null)
				{
					LoadDefaultStyles(mDefaultSkin);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка стандартных визуальных стилей 
			/// </summary>
			/// <param name="skin">Визуальный скин</param>
			//---------------------------------------------------------------------------------------------------------
			private static void LoadDefaultStyles(GUISkin skin)
			{
				//
				// ПРОСТОЙ ТЕКСТ
				//
				if (mTextStyle == null) mTextStyle = skin.FindStyle(TEXT_STYLE_NAME);
				if (mTextStyle == null)
				{
					mTextStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", TEXT_STYLE_NAME, "label");
				}

				if (mTextMonoStyle == null) mTextMonoStyle = skin.FindStyle(TEXT_MONO_STYLE_NAME);
				if (mTextMonoStyle == null)
				{
					mTextMonoStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", TEXT_MONO_STYLE_NAME, "label");
				}

				if (mTextHeaderStyle == null) mTextHeaderStyle = skin.FindStyle(TEXT_HEADER_STYLE_NAME);
				if (mTextHeaderStyle == null)
				{
					mTextHeaderStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", TEXT_HEADER_STYLE_NAME, "label");
				}

				if (mTextValueStyle == null) mTextValueStyle = skin.FindStyle(TEXT_VALUE_STYLE_NAME);
				if (mTextValueStyle == null)
				{
					mTextValueStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", TEXT_VALUE_STYLE_NAME, "label");
				}

				if (mTextColumnStyle == null) mTextColumnStyle = skin.FindStyle(TEXT_COLUMN_STYLE_NAME);
				if (mTextColumnStyle == null)
				{
					mTextColumnStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", TEXT_COLUMN_STYLE_NAME, "label");
				}

				if (mTextSmallStyle == null) mTextSmallStyle = skin.FindStyle(TEXT_SMALL_STYLE_NAME);
				if (mTextSmallStyle == null)
				{
					mTextSmallStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", TEXT_SMALL_STYLE_NAME, "label");
				}

				//
				// НАДПИСИ
				//
				if (mLabelStyle == null) mLabelStyle = skin.label;

				if (mLabelMonoStyle == null) mLabelMonoStyle = skin.FindStyle(LABEL_MONO_STYLE_NAME);
				if (mLabelMonoStyle == null)
				{
					mLabelMonoStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", LABEL_MONO_STYLE_NAME, "label");
				}

				if (mLabelHeaderStyle == null) mLabelHeaderStyle = skin.FindStyle(LABEL_HEADER_STYLE_NAME);
				if (mLabelHeaderStyle == null)
				{
					mLabelHeaderStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", LABEL_HEADER_STYLE_NAME, "label");
				}

				if (mLabelValueStyle == null) mLabelValueStyle = skin.FindStyle(LABEL_VALUE_STYLE_NAME);
				if (mLabelValueStyle == null)
				{
					mLabelValueStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", LABEL_VALUE_STYLE_NAME, "label");
				}

				if (mLabelColumnStyle == null) mLabelColumnStyle = skin.FindStyle(LABEL_COLUMN_STYLE_NAME);
				if (mLabelColumnStyle == null)
				{
					mLabelColumnStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", LABEL_COLUMN_STYLE_NAME, "label");
				}

				if (mLabelSmallStyle == null) mLabelSmallStyle = skin.FindStyle(LABEL_SMALL_STYLE_NAME);
				if (mLabelSmallStyle == null)
				{
					mLabelSmallStyle = skin.label;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", LABEL_SMALL_STYLE_NAME, "label");
				}

				//
				// ПАНЕЛИ
				//
				if (mPanelStyle == null) mPanelStyle = skin.box;

				if (mPanelGroupStyle == null) mPanelGroupStyle = skin.FindStyle(PANEL_GROUP_STYLE_NAME);
				if (mPanelGroupStyle == null)
				{
					mPanelGroupStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", PANEL_GROUP_STYLE_NAME, "box");
				}

				if (mPanelHeaderStyle == null) mPanelHeaderStyle = skin.FindStyle(PANEL_HEADER_STYLE_NAME);
				if (mPanelHeaderStyle == null)
				{
					mPanelHeaderStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", PANEL_HEADER_STYLE_NAME, "box");
				}

				if (mPanelValueStyle == null) mPanelValueStyle = skin.FindStyle(PANEL_VALUE_STYLE_NAME);
				if (mPanelValueStyle == null)
				{
					mPanelValueStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", PANEL_VALUE_STYLE_NAME, "box");
				}

				if (mPanelColumnStyle == null) mPanelColumnStyle = skin.FindStyle(PANEL_COLUMN_STYLE_NAME);
				if (mPanelColumnStyle == null)
				{
					mPanelColumnStyle = skin.box;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", PANEL_COLUMN_STYLE_NAME, "box");
				}

				if (mButtonMiniStyle == null) mButtonMiniStyle = skin.FindStyle(BUTTON_MINI_STYLE_NAME);
				if (mButtonMiniStyle == null)
				{
					mButtonMiniStyle = new GUIStyle(skin.button);
					mButtonMiniStyle.border.left = 1;
					mButtonMiniStyle.border.right = 1;
					mButtonMiniStyle.border.top = 1;
					mButtonMiniStyle.border.bottom = 1;
					mButtonMiniStyle.fontSize = 9;
					Debug.LogWarningFormat("Visual style <{0}> not found, default style loaded <{1}>", BUTTON_MINI_STYLE_NAME, "button");
				}

			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений ресурса GUISkin
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionGUISkin
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление произвольного стиля в GUI скин
			/// </summary>
			/// <param name="this">GUI скин</param>
			/// <param name="style">Добавляемый стиль</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddCustomStyle(this GUISkin @this, GUIStyle style)
			{
				GUIStyle[] styles = @this.customStyles;
				Array.Resize(ref styles, @this.customStyles.Length + 1);
				styles[styles.Length - 1] = style;
				@this.customStyles = styles;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(@this);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление произвольного стиля в GUI скин
			/// </summary>
			/// <param name="this">GUI скин</param>
			/// <param name="style_name">Имя стиля</param>
			//---------------------------------------------------------------------------------------------------------
			public static void AddCustomStyle(this GUISkin @this, String style_name)
			{
				GUIStyle style = new GUIStyle(GUIStyle.none);
				style.name = style_name;

				GUIStyle[] styles = @this.customStyles;
				Array.Resize(ref styles, @this.customStyles.Length + 1);
				styles[styles.Length - 1] = style;
				@this.customStyles = styles;
#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(@this);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление произвольного стиля из GUI скин
			/// </summary>
			/// <param name="this">GUI скин</param>
			/// <param name="style">Удаляемый стиль</param>
			//---------------------------------------------------------------------------------------------------------
			public static void RemoveCustomStyle(this GUISkin @this, GUIStyle style)
			{
				GUIStyle[] styles = @this.customStyles;
				Int32 index = Array.IndexOf(styles, style);
				if(index != -1)
				{
					List<GUIStyle> list_style = new List<GUIStyle>(styles);
					list_style.Remove(style);
					styles = list_style.ToArray();
				}
				
				@this.customStyles = styles;

#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(@this);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Дубликат произвольного стиля в GUI скин
			/// </summary>
			/// <param name="this">GUI скин</param>
			/// <param name="style">Дублируемый стиль</param>
			//---------------------------------------------------------------------------------------------------------
			public static void DuplicateCustomStyle(this GUISkin @this, GUIStyle style)
			{
				GUIStyle[] styles = @this.customStyles;
				Int32 index = Array.IndexOf(styles, style);
				if (index != -1)
				{
					List<GUIStyle> list_style = new List<GUIStyle>(styles);

					GUIStyle duplicate = new GUIStyle(style);
					duplicate.name += "(Duplicate)";
					list_style.Insert(index, duplicate);

					styles = list_style.ToArray();
					@this.customStyles = styles;
				}
				else
				{
					// Добавляем в конец
					GUIStyle duplicate = new GUIStyle(style);
					duplicate.name += "(Duplicate)";
					@this.AddCustomStyle(duplicate);
				}

#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(@this);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение вверх по списку произвольного стиля в GUI скин
			/// </summary>
			/// <param name="this">GUI скин</param>
			/// <param name="style">Перемещаемый стиль</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveCustomStyleUp(this GUISkin @this, GUIStyle style)
			{
				GUIStyle[] styles = @this.customStyles;
				Int32 index = Array.IndexOf(styles, style);
				if (index != -1)
				{
					List<GUIStyle> list_style = new List<GUIStyle>(styles);

					list_style.MoveElementUp(index);

					styles = list_style.ToArray();
				}

				@this.customStyles = styles;

#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(@this);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение вниз по списку произвольного стиля в GUI скин
			/// </summary>
			/// <param name="this">GUI скин</param>
			/// <param name="style">Перемещаемый стиль</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveCustomStyleDown(this GUISkin @this, GUIStyle style)
			{
				GUIStyle[] styles = @this.customStyles;
				Int32 index = Array.IndexOf(styles, style);
				if (index != -1)
				{
					List<GUIStyle> list_style = new List<GUIStyle>(styles);

					list_style.MoveElementDown(index);

					styles = list_style.ToArray();
				}

				@this.customStyles = styles;

#if UNITY_EDITOR
				UnityEditor.EditorUtility.SetDirty(@this);
#endif
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================