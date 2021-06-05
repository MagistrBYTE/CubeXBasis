//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorUnityAttributesHeaders.cs
*		Атрибуты декоративной отрисовки заголовков секций и групп свойств/полей компонентов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using UnityEngine;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут декоративной отрисовки заголовка секции
		/// </summary>
		/// <remarks>
		/// Реализация декоративной атрибута отрисовки заголовка секции c возможностью задать выравнивания и цвет текста заголовка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXHeaderSectionAttribute : PropertyAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mName;
			internal Color mTextColor = XUnityColor.Zero;
			internal TextAnchor mTextAlignment = TextAnchor.MiddleCenter;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя заголовка
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Цвет текста заголовка
			/// </summary>
			public Color TextColor
			{
				get { return mTextColor; }
				set { mTextColor = value; }
			}

			/// <summary>
			/// Выравнивание текста заголовка
			/// </summary>
			public TextAnchor TextAlignment
			{
				get { return mTextAlignment; }
				set { mTextAlignment = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionAttribute()
			{
				mName = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionAttribute(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionAttribute(String name, UInt32 color, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="ord">Порядок отображения свойства</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionAttribute(String name, UInt32 color, Int32 ord, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
				order = ord;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут декоративной отрисовки заголовка секции в боксе
		/// </summary>
		/// <remarks>
		/// Реализация атрибута декоративной отрисовки заголовка секции в боксе c возможностью задать выравнивания и цвет текста заголовка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXHeaderSectionBoxAttribute : PropertyAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mName;
			internal Color mTextColor = XUnityColor.Zero;
			internal TextAnchor mTextAlignment = TextAnchor.MiddleCenter;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя заголовка
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Цвет текста заголовка
			/// </summary>
			public Color TextColor
			{
				get { return mTextColor; }
				set { mTextColor = value; }
			}

			/// <summary>
			/// Выравнивание текста заголовка
			/// </summary>
			public TextAnchor TextAlignment
			{
				get { return mTextAlignment; }
				set { mTextAlignment = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionBoxAttribute()
			{
				mName = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionBoxAttribute(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionBoxAttribute(String name, UInt32 color, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="ord">Порядок отображения свойства</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderSectionBoxAttribute(String name, UInt32 color, Int32 ord, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
				order = ord;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут декоративной отрисовки заголовка группы
		/// </summary>
		/// <remarks>
		/// Реализация декоративной атрибута отрисовки заголовка группы c возможностью задать выравнивания и цвет текста заголовка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXHeaderGroupAttribute : PropertyAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mName;
			internal Color mTextColor = XUnityColor.Zero;
			internal TextAnchor mTextAlignment = TextAnchor.MiddleLeft;
			internal Int32 mIndent;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя заголовка
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Цвет текста заголовка
			/// </summary>
			public Color TextColor
			{
				get { return mTextColor; }
				set { mTextColor = value; }
			}

			/// <summary>
			/// Выравнивание текста заголовка
			/// </summary>
			public TextAnchor TextAlignment
			{
				get { return mTextAlignment; }
				set { mTextAlignment = value; }
			}

			/// <summary>
			/// Уровень смещения
			/// </summary>
			public Int32 Indent
			{
				get { return mIndent; }
				set { mIndent = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupAttribute()
			{
				mName = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupAttribute(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="indent">Уровень смещения</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupAttribute(String name, Int32 indent)
			{
				mName = name;
				mIndent = indent;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupAttribute(String name, UInt32 color, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="ord">Порядок отображения свойства</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupAttribute(String name, UInt32 color, Int32 ord, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
				order = ord;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут декоративной отрисовки заголовка группы в боксе
		/// </summary>
		/// <remarks>
		/// Реализация атрибута декоративной отрисовки заголовка группы в боксе c возможностью задать выравнивания и цвет текста заголовка
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXHeaderGroupBoxAttribute : PropertyAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mName;
			internal Color mTextColor = XUnityColor.Zero;
			internal TextAnchor mTextAlignment = TextAnchor.MiddleLeft;
			internal Int32 mIndent;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя заголовка
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Цвет текста заголовка
			/// </summary>
			public Color TextColor
			{
				get { return mTextColor; }
				set { mTextColor = value; }
			}

			/// <summary>
			/// Выравнивание текста заголовка
			/// </summary>
			public TextAnchor TextAlignment
			{
				get { return mTextAlignment; }
				set { mTextAlignment = value; }
			}

			/// <summary>
			/// Уровень смещения
			/// </summary>
			public Int32 Indent
			{
				get { return mIndent; }
				set { mIndent = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupBoxAttribute()
			{
				mName = "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupBoxAttribute(String name)
			{
				mName = name;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="indent">Уровень смещения</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupBoxAttribute(String name, Int32 indent)
			{
				mName = name;
				mIndent = indent;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupBoxAttribute(String name, UInt32 color, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя заголовка</param>
			/// <param name="color">Цвет текста заголовка</param>
			/// <param name="ord">Порядок отображения свойства</param>
			/// <param name="text_alignment">Выравнивание текста заголовка</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXHeaderGroupBoxAttribute(String name, UInt32 color, Int32 ord, TextAnchor text_alignment = TextAnchor.MiddleLeft)
			{
				mName = name;
				mTextColor = color.ToColor();
				mTextAlignment = text_alignment;
				order = ord;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================