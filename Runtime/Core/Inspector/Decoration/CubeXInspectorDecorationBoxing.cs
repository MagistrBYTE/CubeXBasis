//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты связанные с оформлением (декорацией) элемента инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorDecorationBoxing.cs
*		Атрибут для определения фонового бокса элемента инспектора свойств.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleInspectorDecoration
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут для определения фонового бокса элемента инспектора свойств
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXBoxingAttribute : CubeXInspectorItemStyledAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal readonly Int32 mOffsetLeft;
			internal readonly Int32 mOffsetTop;
			internal readonly Int32 mOffsetRight;
			internal readonly Int32 mOffsetBottom;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Смещение слева
			/// </summary>
			public Int32 OffsetLeft
			{
				get { return mOffsetLeft; }
			}

			/// <summary>
			/// Смещение сверху
			/// </summary>
			public Int32 OffsetTop
			{
				get { return mOffsetTop; }
			}

			/// <summary>
			/// Смещение справа
			/// </summary>
			public Int32 OffsetRight
			{
				get { return mOffsetRight; }
			}

			/// <summary>
			/// Смещение снизу
			/// </summary>
			public Int32 OffsetBottom
			{
				get { return mOffsetBottom; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="offset_left">Красная компонента цвета</param>
			/// <param name="offset_top">Зеленая компонента цвета</param>
			/// <param name="offset_right">Синяя компонента цвета</param>
			/// <param name="offset_bottom">Альфа компонента цвета</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXBoxingAttribute(Int32 offset_left, Int32 offset_top = 0, Int32 offset_right = 0, Int32 offset_bottom = 0)
			{
				mOffsetLeft = offset_left;
				mOffsetTop = offset_top;
				mOffsetRight = offset_right;
				mOffsetBottom = offset_bottom;
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
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

				var top = UnityEngine.Mathf.Max(0, mOffsetTop);
				var bottom = UnityEngine.Mathf.Max(0, mOffsetBottom);

				return (GetHeightDefault(label) + top + bottom + XEditorInspector.SPACE);
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
				if (mOffsetLeft < 0) indent.xMin += mOffsetLeft;
				if (mOffsetRight < 0) indent.xMax -= mOffsetRight;
				if (mOffsetTop < 0) indent.yMin += mOffsetTop;
				if (mOffsetBottom < 0) indent.yMax -= mOffsetBottom;
				UnityEngine.GUI.Box(indent, UnityEngine.GUIContent.none, mBackgroundStyle);

				// Центрируем для красоты
				position.y += (UnityEngine.Mathf.Abs(mOffsetBottom) + UnityEngine.Mathf.Abs(mOffsetTop)) / 2;
				position.x += 2;
				position.width -= 4;

				return (true);
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