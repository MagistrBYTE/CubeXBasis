﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Паттерн документ
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternDocumentHierarchy.cs
*		Шаблон документа для иерархических данных с поддержкой концепции просмотра и управления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
//---------------------------------------------------------------------------------------------------------------------
#if USE_WINDOWS
using CubeX.Windows;
#endif
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
		/// Шаблон документа для начального участия в иерархических отношениях с поддержкой концепции просмотра и управления
		/// </summary>
		/// <typeparam name="TModel">Тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class DocumentHierarchyViewBegin<TModel> : ModelHierarchyViewBeginUI<TModel>, ICubeXDocument, 
			ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu
			where TModel : ICubeXModelHierarchyView
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsFileName = new PropertyChangedEventArgs(nameof(FileName));
			protected static readonly PropertyChangedEventArgs PropertyArgsPathFile = new PropertyChangedEventArgs(nameof(PathFile));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mFileName;
			protected internal String mPathFile;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXDocument ===================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя физического файла
			/// </summary>
			[Browsable(false)]
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
			[Browsable(false)]
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

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public DocumentHierarchyViewBegin()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя документа</param>
			//---------------------------------------------------------------------------------------------------------
			public DocumentHierarchyViewBegin(String name)
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
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон документа для участия в иерархических отношениях с поддержкой концепции просмотра и управления
		/// </summary>
		/// <typeparam name="TModel">Соответствующий тип модели</typeparam>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class DocumentHierarchyView<TModel, TCollectionModel> : ModelHierarchyViewUI<TModel, TCollectionModel>, 
			ICubeXDocument, ICubeXUIModelSupportExplore, ICubeXUIModelContextMenu
			where TModel : ICubeXModelHierarchyView
			where TCollectionModel : class, ICubeXCollectionModelView
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsFileName = new PropertyChangedEventArgs(nameof(FileName));
			protected static readonly PropertyChangedEventArgs PropertyArgsPathFile = new PropertyChangedEventArgs(nameof(PathFile));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal String mFileName;
			protected internal String mPathFile;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXDocument ===================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя физического файла
			/// </summary>
			[Browsable(false)]
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
			[Browsable(false)]
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

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public DocumentHierarchyView()
				: this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя документа</param>
			//---------------------------------------------------------------------------------------------------------
			public DocumentHierarchyView(String name)
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
				this.NotifyOwnerUpdated(nameof(FileName));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение пути до файла.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaisePathFileChanged()
			{
				this.NotifyOwnerUpdated(nameof(PathFile));
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

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление документа после загрузки его из файла
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UpdateAfterLoad()
			{

			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================