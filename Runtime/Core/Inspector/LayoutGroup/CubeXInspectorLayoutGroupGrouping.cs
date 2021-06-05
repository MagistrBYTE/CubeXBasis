//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты для определения автогруппирования элементов инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorLayoutGroupGrouping.cs
*		Атрибут для определения логической группы элементов инспектора свойств.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleInspectorLayoutGroup Атрибуты автогруппирования
		//! Атрибуты для определения автогруппирования элементов инспектора свойств
		//! \ingroup CoreModuleInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения логической группы элементов инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public class CubeXGroupingAttribute : CubeXInspectorItemStyledAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mGroupName;
			protected internal TColor mHeaderColor;
			protected internal TColor mBackground;
#if UNITY_EDITOR
			protected internal List<CubeXInspectorAttribute> mGroupItems;
			protected internal Single mHeaderHeight = XEditorInspector.HEADER_HEIGHT;
			protected internal UnityEngine.GUIContent mContentItem;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя группы
			/// </summary>
			public String GroupName
			{
				get { return mGroupName; }
				set { mGroupName = value; }
			}

#if UNITY_EDITOR
			/// <summary>
			/// Список элементов группы
			/// </summary>
			public List<CubeXInspectorAttribute> GroupItems
			{
				get { return mGroupItems; }
			}
#endif

			/// <summary>
			/// Цвет надписи
			/// </summary>
			public TColor HeaderColor
			{
				get { return mHeaderColor; }
				set
				{
					mHeaderColor = value;
				}
			}

			/// <summary>
			/// Фоновый цвет
			/// </summary>
			public TColor Background
			{
				get { return mBackground; }
				set
				{
					mBackground = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="group_name">Имя группы</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXGroupingAttribute(String group_name)
			{
				mGroupName = group_name;
				mBackground = TColor.White;
				mHeaderColor = TColor.White;
#if UNITY_EDITOR
				mGroupItems = new List<CubeXInspectorAttribute>();
				mContentItem = new UnityEngine.GUIContent("");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="group_name">Имя группы</param>
			/// <param name="header_color_bgra">Цвет надписи в формате BGRA</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXGroupingAttribute(String group_name, UInt32 header_color_bgra)
			{
				mGroupName = group_name;
				mBackground = TColor.White;
				mHeaderColor = TColor.FromBGRA(header_color_bgra);
#if UNITY_EDITOR
				mGroupItems = new List<CubeXInspectorAttribute>();
				mContentItem = new UnityEngine.GUIContent("");
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="group_name">Имя группы</param>
			/// <param name="header_color_bgra">Цвет надписи в формате BGRA</param>
			/// <param name="background_bgra">Фоновый цвет в формате BGRA</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXGroupingAttribute(String group_name, UInt32 header_color_bgra, UInt32 background_bgra)
			{
				mGroupName = group_name;
				mBackground = TColor.FromBGRA(background_bgra);
				mHeaderColor = TColor.FromBGRA(header_color_bgra);
#if UNITY_EDITOR
				mGroupItems = new List<CubeXInspectorAttribute>();
				mContentItem = new UnityEngine.GUIContent("");
#endif
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение общей высоты всех элементов группы
			/// </summary>
			/// <returns>Общая высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public Single GetHeightItemsGroup()
			{
				Single total_height = 0;
				for (Int32 i = 0; i < mGroupItems.Count; i++)
				{
					UnityEditor.SerializedProperty property_item = mGroupItems[i].SerializedProperty;
					mContentItem.text = property_item.displayName + CubeXInspectorAttribute.SYMBOL_OTHER;

					total_height += UnityEditor.EditorGUI.GetPropertyHeight(property_item, mContentItem, 
						property_item.isExpanded);

					total_height += XEditorInspector.SPACE;
				}

				return (total_height);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта сериализации
			/// </summary>
			/// <remarks>
			/// Метод должен реализовать дополнительную логику по работе с другими свойствами
			/// </remarks>
			/// <param name="serialized_object">Объект сериализации</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetSerializedObject(UnityEditor.SerializedObject serialized_object)
			{
				// Получаем элементы группы
				GroupItems.Clear();
				var other_property = serialized_object.GetIterator();
				var next = other_property.NextVisible(true);
				if (next)
				{
					do
					{
						if (other_property.IsScript()) continue;

						if (other_property.propertyPath.IsExists() && other_property.propertyPath != mOwned.SerializedProperty.propertyPath)
						{
							CubeXInspectorAttribute in_group = new CubeXInspectorAttribute();
							in_group.SetSerializedProperty(other_property.Copy());

							CubeXInGroupAttribute in_group_attribute = XEditorCachedData.GetAttribute<CubeXInGroupAttribute>(other_property);
							if (in_group_attribute != null && in_group_attribute.GroupName == GroupName)
							{
								in_group.GetAttributes();
								GroupItems.Add(in_group);
							}
						}
					}
					while (other_property.NextVisible(false));
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

				// Если группа открыта
				if (mOwned.SerializedProperty.LoadEditorBool("Grouping"))
				{
					// Высота заголовка
					mTotalHeight = mHeaderHeight + XEditorInspector.SPACE;

					// Высота элемента
					mTotalHeight += GetHeightDefault(label) + XEditorInspector.SPACE;

					// Высота всех элементов
					mTotalHeight += GetHeightItemsGroup();
				}
				else
				{
					// Высота заголовка
					mTotalHeight = mHeaderHeight + XEditorInspector.SPACE;
				}

				return (mTotalHeight);
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
				Boolean opened = mOwned.SerializedProperty.LoadEditorBool("Grouping");

				// Общий фоновый бока
				UnityEngine.GUI.backgroundColor = mBackground;
				UnityEngine.GUI.Box(position, UnityEngine.GUIContent.none, mBackgroundStyle);
				UnityEngine.GUI.backgroundColor = UnityEngine.Color.white;

				// Смещаем по ширине
				position.xMin += 1;
				position.xMax -= 1;

				UnityEngine.Rect rect_header = position;
				rect_header.height = mHeaderHeight;

				UnityEngine.GUI.color = mHeaderColor;
				UnityEngine.GUI.Label(rect_header, mGroupName, UnityEditor.EditorStyles.boldLabel);
				UnityEngine.GUI.color = UnityEngine.Color.white;

				UnityEngine.Rect rect_button = position;
				rect_button.x = position.xMax - XEditorInspector.BUTTON_PREVIEW_WIDTH;
				rect_button.width = XEditorInspector.BUTTON_PREVIEW_WIDTH;
				rect_button.height = XEditorInspector.BUTTON_MINI_HEIGHT;

				if (UnityEngine.GUI.Button(rect_button, opened ? XString.TriangleDown : XString.TriangleLeft, XEditorStyles.ButtonPreviewStyle))
				{
					opened = !opened;
					mOwned.SerializedProperty.SaveBoolEditor("Grouping", opened);
				}

				if(opened)
				{
					position.y += mHeaderHeight;
					position.height = ControlHeight;
				}

				return (opened);
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
				if (mOwned.SerializedProperty.LoadEditorBool("Grouping"))
				{
					UnityEngine.Rect rect_item = position;
					rect_item.y += XEditorInspector.SPACE + ControlHeight + mHeaderHeight;

					for (Int32 i = 0; i < GroupItems.Count; i++)
					{
						if (GroupItems[i].CheckVisible())
						{
							mContentItem.text = GroupItems[i].SerializedProperty.displayName + CubeXInspectorAttribute.SYMBOL_OTHER;

							rect_item.height = GroupItems[i].GetMaxHeight(mContentItem);

							UnityEditor.EditorGUI.PropertyField(rect_item, GroupItems[i].SerializedProperty, mContentItem,
								GroupItems[i].SerializedProperty.isExpanded);

							rect_item.y += rect_item.height + XEditorInspector.SPACE;
						}
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