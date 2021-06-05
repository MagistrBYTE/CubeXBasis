//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема расширения функциональности редактора Unity
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXUnityEditorStyles.cs
*		Визуальные стили для оформления элементов инспектора свойств.
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
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleUnityEditor
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий визуальные стили для оформления элементов инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XEditorStyles
		{
			#region ======================================= ИМЕНА ИКОНОК =============================================
			//
			// РЕСУРСЫ
			//
			/// <summary>
			/// Имя иконки директории
			/// </summary>
			public const String ICON_FOLDER = "Folder Icon";

			/// <summary>
			/// Имя иконки
			/// </summary>
			public const String ICON_DEFAULT_ASSET = "DefaultAsset Icon";

			/// <summary>
			/// Имя иконки пользовательского ресурса
			/// </summary>
			public const String ICON_SCRIPTABLE_OBJECT = "ScriptableObject Icon";

			/// <summary>
			/// Имя иконки материала
			/// </summary>
			public const String ICON_MATERIAL = "Material Icon";

			/// <summary>
			/// Имя иконки текстуры
			/// </summary>
			public const String ICON_TEXTURE_2D = "Texture2D Icon";

			/// <summary>
			/// Имя иконки шрифта
			/// </summary>
			public const String ICON_FONT = "Font Icon";

			/// <summary>
			/// Имя иконки звукового ресурса
			/// </summary>
			public const String ICON_AUDIO_SOURCE = "AudioSource Icon";

			/// <summary>
			/// Имя иконки анимационного клипа
			/// </summary>
			public const String ICON_ANIMATION_CLIP = "AnimationClip Icon";

			/// <summary>
			/// Имя иконки префаба
			/// </summary>
			public const String ICON_PREFAB = "Prefab Icon";

			/// <summary>
			/// Имя иконки префаба модели
			/// </summary>
			public const String ICON_PREFAB_MODEL = "PrefabModel Icon";

			/// <summary>
			/// Имя иконки меша
			/// </summary>
			public const String ICON_MESH = "Mesh Icon";

			/// <summary>
			/// Имя иконки шейдера
			/// </summary>
			public const String ICON_SHADER = "Shader Icon";

			/// <summary>
			/// Имя иконки скина GUI
			/// </summary>
			public const String ICON_GUISKIN = "GUISkin Icon";

			/// <summary>
			/// Имя иконки текстового файла
			/// </summary>
			public const String ICON_TEXT = "TextAsset Icon";

			/// <summary>
			/// Имя иконки XML файла
			/// </summary>
			public const String ICON_XML = "UxmlScript Icon";

			/// <summary>
			/// Имя иконки cs файла
			/// </summary>
			public const String ICON_SCRIPT = "cs Script Icon";

			/// <summary>
			/// Имя иконки мета файла
			/// </summary>
			public const String ICON_META = "MetaFile Icon";

			/// <summary>
			/// Имя иконки сцены
			/// </summary>
			public const String ICON_SCENE = "SceneAsset Icon";

			/// <summary>
			/// Имя иконки видеофайла
			/// </summary>
			public const String ICON_VIDEO_CLIP = "VideoClip Icon";

			/// <summary>
			/// Имя иконки физического материала
			/// </summary>
			public const String ICON_PHYSIC_MATERIAL = "PhysicMaterial Icon";

			/// <summary>
			/// Имя иконки настроек
			/// </summary>
			public const String ICON_GAME_MANAGER = "GameManager Icon";

			/// <summary>
			/// Имя иконки сигнала
			/// </summary>
			public const String ICON_SIGNAL = "animationdopesheetkeyframe";
			#endregion

			#region ======================================= ИМЕНА СТИЛЕЙ ДЛЯ КНОПОК ===================================
			/// <summary>
			/// Имя стиля обычной мини кнопки
			/// </summary>
			public const String MINI_BUTTON = "miniButton";

			/// <summary>
			/// Имя стиля мини кнопки расположенной слева
			/// </summary>
			public const String MINI_BUTTON_L = "miniButtonLeft";

			/// <summary>
			/// Имя стиля мини кнопки расположенной посередине
			/// </summary>
			public const String MINI_BUTTON_M = "miniButtonMid";

			/// <summary>
			/// Имя стиля мини кнопки расположенной справа
			/// </summary>
			public const String MINI_BUTTON_R = "miniButtonRight";

			/// <summary>
			/// Имя стиля обычной кнопки DropDown
			/// </summary>
			public const String BUTTON_DROP_DOWN = "DropDown";

			/// <summary>
			/// Имя стиля обычной кнопки DropDownButton
			/// </summary>
			public const String BUTTON_DROP_DOWN_1 = "DropDownButton";
			#endregion

			#region ======================================= ИМЕНА СТИЛЕЙ ДЛЯ БОКСОВ ===================================
			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_SHURIKEN = "ShurikenEffectBg";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_FLOW_OVERLAY = "flow overlay box";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_FLOW_NODE_0 = "flow node 0";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_FLOW_NODE_0_ON = "flow node 0 on";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_FLOW_NODE_1 = "flow node 1";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_FLOW_NODE_1_ON = "flow node 1 on";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_BLOCK = "CN Box";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_CURVE = "CurveEditorBackground";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_FRAME = "Dopesheetkeyframe";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_SHADOW = "IN ThumbnailShadow";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_OBJECT = "ObjectFieldThumb";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_SELECTION = "SelectionRect";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_NODE = "TE NodeBox";

			/// <summary>
			/// Имя стиля бокса
			/// </summary>
			public const String BOX_NODE_S = "TE NodeBoxSelected";
			#endregion

			#region ======================================= ИМЕНА СТИЛЕЙ ДЛЯ ПОИСКА ===================================
			/// <summary>
			/// Имя стиля текстового поля для строки поиска
			/// </summary>
			public const String SEARCH_FIELD = "toolbarSearchField";

			/// <summary>
			/// Имя стиля кнопки выпадающего меню для строки поиска
			/// </summary>
			public const String SEARCH_FIELD_POPUP = "ToolbarSeachTextFieldPopup";

			/// <summary>
			/// Имя стиля кнопки очистки для строки поиска
			/// </summary>
			public const String SEARCH_FIELD_CANCEL = "ToolbarSeachCancelButton";
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			internal static GUIStyle mDirtyStyle;

			// Стили кнопок
			internal static GUIStyle mButtonMiniDefaultGreenRightStyle;
			internal static GUIStyle mButtonMiniDefaultRedRightStyle;
			internal static GUIStyle mButtonMiniHeaderRedRightStyle;
			internal static GUIStyle mButtonMiniHeaderStyle;
			internal static GUIStyle mButtonMiniHeaderMiddleStyle;
			internal static GUIStyle mButtonPreviewStyle;

			// Стили переключателей
			internal static GUIStyle mToogleLeftStyle;

			// Стили текстового поля
			internal static GUIStyle mTextHeaderHeightStyle;

			// Стили сворачиваемой панели
			internal static GUIStyle mFoldoutBoldStyle;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Рабочий стиль
			/// </summary>
			public static GUIStyle DirtyStyle
			{
				get
				{
					if (mDirtyStyle == null)
						mDirtyStyle = new GUIStyle(GUIStyle.none);

					return (mDirtyStyle);
				}
			}

			//
			// СТИЛИ КНОПОК
			//
			/// <summary>
			/// Стиль для стандартной малой кнопки расположенной справа
			/// </summary>
			public static GUIStyle ButtonMiniDefaultGreenRightStyle
			{
				get
				{
					if (mButtonMiniDefaultGreenRightStyle == null)
					{
#if UNITY_EDITOR
						mButtonMiniDefaultGreenRightStyle = new GUIStyle(UnityEditor.EditorStyles.miniButtonRight);
						mButtonMiniDefaultGreenRightStyle.normal.textColor = ColorGreen;

						if (UnityEditor.EditorGUIUtility.isProSkin == false)
						{
							mButtonMiniDefaultGreenRightStyle.fixedHeight = XEditorInspector.CONTROL_HEIGHT_SPACE;
						}
#else

#endif
					}
					return (mButtonMiniDefaultGreenRightStyle);
				}
			}

			/// <summary>
			/// Стиль для стандартной малой кнопки расположенной справа
			/// </summary>
			public static GUIStyle ButtonMiniDefaultRedRightStyle
			{
				get
				{
					if (mButtonMiniDefaultRedRightStyle == null)
					{
#if UNITY_EDITOR
						mButtonMiniDefaultRedRightStyle = new GUIStyle(UnityEditor.EditorStyles.miniButtonRight);
						mButtonMiniDefaultRedRightStyle.normal.textColor = ColorRed;

						if (UnityEditor.EditorGUIUtility.isProSkin == false)
						{
							mButtonMiniDefaultRedRightStyle.fixedHeight = XEditorInspector.CONTROL_HEIGHT_SPACE;
						}
#else

#endif

					}
					return (mButtonMiniDefaultRedRightStyle);
				}
			}

			/// <summary>
			/// Стиль для стандартной малой(высотой в заголовок) кнопки расположенной справа
			/// </summary>
			public static GUIStyle ButtonMiniHeaderRedRightStyle
			{
				get
				{
					if (mButtonMiniHeaderRedRightStyle == null)
					{
#if UNITY_EDITOR
						mButtonMiniHeaderRedRightStyle = new GUIStyle(UnityEditor.EditorStyles.miniButtonRight);
						mButtonMiniHeaderRedRightStyle.alignment = TextAnchor.MiddleCenter;
						mButtonMiniHeaderRedRightStyle.fixedHeight = XEditorInspector.HEADER_HEIGHT;
						mButtonMiniHeaderRedRightStyle.normal.textColor = ColorRed;
						mButtonMiniHeaderRedRightStyle.border.top = 3;
						mButtonMiniHeaderRedRightStyle.border.left = 3;
						mButtonMiniHeaderRedRightStyle.border.right = 3;
						mButtonMiniHeaderRedRightStyle.border.bottom = 3;
#else

#endif
					}
					return (mButtonMiniHeaderRedRightStyle);
				}
			}

			/// <summary>
			/// Стиль для стандартной малой(высотой в заголовок) кнопки
			/// </summary>
			public static GUIStyle ButtonMiniHeaderStyle
			{
				get
				{
					if (mButtonMiniHeaderStyle == null)
					{
#if UNITY_EDITOR
						mButtonMiniHeaderStyle = new GUIStyle(UnityEditor.EditorStyles.miniButton);
						mButtonMiniHeaderStyle.alignment = TextAnchor.MiddleCenter;
						mButtonMiniHeaderStyle.fixedHeight = XEditorInspector.HEADER_HEIGHT;
						mButtonMiniHeaderStyle.border = new RectOffset(3, 3, 3, 3);
#else

#endif
					}
					return (mButtonMiniHeaderStyle);
				}
			}

			/// <summary>
			/// Стиль для стандартной малой(высотой в заголовок) кнопки расположенной посередине
			/// </summary>
			public static GUIStyle ButtonMiniHeaderMiddleStyle
			{
				get
				{
					if (mButtonMiniHeaderMiddleStyle == null)
					{
#if UNITY_EDITOR
						mButtonMiniHeaderMiddleStyle = new GUIStyle(UnityEditor.EditorStyles.miniButtonMid);
						mButtonMiniHeaderMiddleStyle.alignment = TextAnchor.MiddleCenter;
						mButtonMiniHeaderMiddleStyle.fixedHeight = XEditorInspector.HEADER_HEIGHT;
						mButtonMiniHeaderMiddleStyle.border = new RectOffset(3, 3, 3, 3);
#else

#endif
					}
					return (mButtonMiniHeaderMiddleStyle);
				}
			}

			/// <summary>
			/// Стиль для кнопки предпросмотра
			/// </summary>
			public static GUIStyle ButtonPreviewStyle
			{
				get
				{
					if (mButtonPreviewStyle == null)
					{
#if UNITY_EDITOR
						mButtonPreviewStyle = new GUIStyle(UnityEditor.EditorStyles.largeLabel);
						mButtonPreviewStyle.alignment = UnityEngine.TextAnchor.MiddleCenter;
						mButtonPreviewStyle.fontSize = 10;
#else

#endif
					}
					return (mButtonPreviewStyle);
				}
			}

			//
			// СТИЛИ ПЕРЕКЛЮЧАТЕЛЕЙ
			//
			/// <summary>
			/// Стиль для переключателя расположенного слева
			/// </summary>
			public static GUIStyle ToogleLeftStyle
			{
				get
				{
					if (mToogleLeftStyle == null)
					{
#if UNITY_EDITOR
						mToogleLeftStyle = new GUIStyle(UnityEditor.EditorStyles.toggle);
						mToogleLeftStyle.alignment = TextAnchor.MiddleCenter;
						mToogleLeftStyle.fixedWidth = 0;
#else

#endif
					}
					return (mToogleLeftStyle);
				}
			}

			//
			// СТИЛИ ТЕКСТОВОГО ПОЛЯ
			//
			/// <summary>
			/// Стиль для текстового поля высотой в заголовок
			/// </summary>
			public static GUIStyle TextHeaderHeightStyle
			{
				get
				{
					if (mTextHeaderHeightStyle == null)
					{
#if UNITY_EDITOR
						mTextHeaderHeightStyle = new GUIStyle(UnityEditor.EditorStyles.textField);
						mTextHeaderHeightStyle.alignment = TextAnchor.MiddleLeft;
						mTextHeaderHeightStyle.fixedHeight = XEditorInspector.HEADER_HEIGHT;
#else

#endif
					}
					return (mTextHeaderHeightStyle);
				}
			}

			//
			// СТИЛИ СВОРАЧИВАЕМОЙ ПАНЕЛИ
			//
			/// <summary>
			/// Жирный текст
			/// </summary>
			public static GUIStyle FoldoutBoldStyle
			{
				get
				{
					if (mFoldoutBoldStyle == null)
					{
#if UNITY_EDITOR
						mFoldoutBoldStyle = new GUIStyle(UnityEditor.EditorStyles.foldout);
						mFoldoutBoldStyle.fontStyle = FontStyle.Bold;
#else

#endif
					}
					return (mFoldoutBoldStyle);
				}
			}

			//
			// ЦВЕТА ИНФОРМАЦИОННЫХ ПАНЕЛЕЙ
			//
			/// <summary>
			/// Цвет заголовка секции
			/// </summary>
#if UNITY_EDITOR
			public static readonly Color ColorHeaderSection = UnityEditor.EditorGUIUtility.isProSkin ? new Color(0.2f, 0.7f, 0.2f) : new Color(0.1f, 0.4f, 0.1f);
#else
			public static readonly Color ColorHeaderSection = new Color(0.2f, 0.7f, 0.2f);
#endif


			/// <summary>
			/// Цвет заголовка группы
			/// </summary>
#if UNITY_EDITOR
			public static readonly Color ColorHeaderGroup = UnityEditor.EditorGUIUtility.isProSkin ? new Color(0.65f, 0.85f, 0.1f) : new Color(0.1f, 0.1f, 0.6f);
#else
			public static readonly Color ColorHeaderGroup = new Color(0.65f, 0.85f, 0.1f);
#endif

			/// <summary>
			/// Зеленый цвет
			/// </summary>
#if UNITY_EDITOR
			public static readonly Color ColorGreen = UnityEditor.EditorGUIUtility.isProSkin ? new Color(0f, 0.85f, 0f) : new Color(0.2f, 0.8f, 0.2f);
#else
			public static readonly Color ColorGreen = Color.green;
#endif

			/// <summary>
			/// Желтый цвет
			/// </summary>
#if UNITY_EDITOR
			public static readonly Color ColorYellow = UnityEditor.EditorGUIUtility.isProSkin ? new Color(0.8f, 0.8f, 0f) : new Color(0.5f, 0.5f, 0.0f);
#else
			public static readonly Color ColorYellow = Color.yellow;
#endif

			/// <summary>
			/// Красный цвет
			/// </summary>
#if UNITY_EDITOR
			public static readonly Color ColorRed = UnityEditor.EditorGUIUtility.isProSkin ? new Color(0.85f, 0.0f, 0f) : new Color(0.8f, 0.2f, 0.2f);
#else
			public static readonly Color ColorRed = Color.red;
#endif

			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация визуальных стилей
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public static void InitEditorStyle()
			{
				// Рабочие данные
				mDirtyStyle = new GUIStyle(GUIStyle.none);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================