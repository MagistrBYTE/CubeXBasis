//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для определения дополнительных элементов управления
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorControlPreview.cs
*		Атрибут для возможности предпросмотра объекта.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleInspectorControls
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для возможности предпросмотра объекта
		/// </summary>
		/// <remarks>
		/// Только в режиме разработке
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXPreviewAttribute : CubeXInspectorItemStyledAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal Single mPreviewHeight;
#if UNITY_EDITOR
			private UnityEditor.Editor mPreviewEditor;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Высота области предпросмотра
			/// </summary>
			public Single PreviewHeight
			{
				get { return mPreviewHeight; }
				set { mPreviewHeight = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXPreviewAttribute()
			{
				mPreviewHeight = 200;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent(XString.TriangleLeft);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="preview_height">Высота области предпросмотра</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXPreviewAttribute(Single preview_height)
			{
				mPreviewHeight = preview_height;
#if UNITY_EDITOR
				mContent = new UnityEngine.GUIContent(XString.TriangleLeft);
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка возможности отобразить свойство
			/// </summary>
			/// <returns>Статус проверки</returns>
			//---------------------------------------------------------------------------------------------------------
			private Boolean IsSupportPreview()
			{
				if ((mOwned.SerializedProperty.propertyType == UnityEditor.SerializedPropertyType.ObjectReference) &&
					(mOwned.SerializedProperty.objectReferenceValue != null))
				{
					return (true);
				}

				return (false);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка(получение) фонового/рабочего стиля
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void SetBackgroundStyle()
			{
				mBackgroundStyle = FindStyle(mBackgroundStyleName);
				if (mBackgroundStyle == null)
				{
					mBackgroundStyle = UnityEditor.EditorGUIUtility.GetBuiltinSkin(UnityEditor.EditorSkin.Scene).box;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты элемента необходимого для работы этого атрибута
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Single GetHeight(UnityEngine.GUIContent label)
			{
				SetBackgroundStyle();

				Boolean is_preview = mOwned.SerializedProperty.LoadEditorBool("Preview", false);
				if (is_preview && IsSupportPreview())
				{
					return (GetHeightDefault(label) + mPreviewHeight);
				}

				return (GetHeightDefault(label));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение действий перед отображением редактора элемента инспектора свойств
			/// </summary>
			/// <remarks>
			/// При необходимости можно менять входные параметры
			/// </remarks>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			/// <returns>Следует ли рисовать редактор элемента инспектора свойств</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean BeforeApply(ref UnityEngine.Rect position, ref UnityEngine.GUIContent label)
			{
				var indent = UnityEditor.EditorGUI.IndentedRect(position);
				UnityEngine.Rect rect_button = indent;
				rect_button.x = indent.xMax - XEditorInspector.BUTTON_PREVIEW_WIDTH;
				rect_button.height = XEditorInspector.BUTTON_MINI_HEIGHT;
				rect_button.width = XEditorInspector.BUTTON_PREVIEW_WIDTH;

				Boolean opened = mOwned.SerializedProperty.LoadEditorBool("Preview", false);
				mContent.text = opened ? XString.TriangleDown : XString.TriangleLeft;
				if (UnityEngine.GUI.Button(rect_button, mContent, XEditorStyles.ButtonPreviewStyle))
				{
					opened = !opened;
					mOwned.SerializedProperty.SaveBoolEditor("Preview", opened);
				}

				position.width -= XEditorInspector.BUTTON_PREVIEW_WIDTH + 1;
				position.height = mControlHeight;

				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение действий после отображения редактора элемента инспектора свойств
			/// </summary>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			//---------------------------------------------------------------------------------------------------------
			public override void AfterApply(UnityEngine.Rect position, UnityEngine.GUIContent label)
			{
				if (mOwned.SerializedProperty.LoadEditorBool("Preview", false) && IsSupportPreview())
				{
					UnityEngine.Rect rect_preview = position;
					rect_preview.y += XEditorInspector.SPACE * 2 + ControlHeight;
					rect_preview.height = mPreviewHeight - XEditorInspector.SPACE * 2;

					if (UnityEngine.Event.current.type == UnityEngine.EventType.Layout) return;

					if(mPreviewEditor == null)
					{
						mPreviewEditor = XEditorCachedData.GetEditor(mOwned.SerializedProperty);
					}
					if (mPreviewEditor != null)
					{
						mPreviewEditor.DrawPreview(rect_preview);
					}
				}
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================