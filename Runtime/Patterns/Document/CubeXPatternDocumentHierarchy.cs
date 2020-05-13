//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Паттерн документ
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternDocumentHierarchy.cs
*		Шаблон документа для иерархических данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternDocument
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон документа для начального участия в иерархических отношениях
		/// </summary>
		/// <typeparam name="TModel">Тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class DocumentHierarchyBegin<TModel> : ModelHierarchyBegin<TModel>, ICubeXDocument, ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu
			where TModel : ICubeXModelHierarchy
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsFileName = new PropertyChangedEventArgs(nameof(FileName));
			protected static readonly PropertyChangedEventArgs PropertyArgsPathFile = new PropertyChangedEventArgs(nameof(PathFile));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mFileName;
			protected internal String mPathFile;
			protected internal CUIContextMenu mUIContextMenu;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXDocument ===================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя физического файла
			/// </summary>
			public String FileName
			{
				get { return (mFileName); }
				set
				{
					mFileName = value;
					NotifyPropertyChanged(PropertyArgsFileName);
					RaiseFileNameChanged();
				}
			}

			/// <summary>
			/// Путь до файла
			/// </summary>
			public String PathFile
			{
				get { return (mPathFile); }
				set
				{
					mPathFile = value;
					NotifyPropertyChanged(PropertyArgsPathFile);
					RaisePathFileChanged();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXUIModelSupportExplore ======================
			/// <summary>
			/// Возможность перемещать модель
			/// </summary>
			[Browsable(false)]
			public virtual Boolean IsDraggableModel
			{
				get { return (false); }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXUIModelContextMenu =========================
			/// <summary>
			/// Контекстное меню для управление моделью
			/// </summary>
			[Browsable(false)]
			public CUIContextMenu UIContextMenu
			{
				get { return (mUIContextMenu); }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public DocumentHierarchyBegin()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя документа</param>
			//---------------------------------------------------------------------------------------------------------
			public DocumentHierarchyBegin(String name)
				: base(name)
			{
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение имени файла.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseFileNameChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение пути до файла.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaisePathFileChanged()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXDocument =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение расширения файла без точки
			/// </summary>
			/// <returns>Расширение файла без точки</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual String GetFileExtension()
			{
				return (XFileExtension.XML);
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXUIModelContextMenu ===========================
#if USE_WINDOWS
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Открытие контекстного меню
			/// </summary>
			/// <param name="context_menu">Контекстное меню</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void OpenContextMenu(System.Windows.Controls.ContextMenu context_menu)
			{
				mUIContextMenu.SetCommandsDefault(context_menu);
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