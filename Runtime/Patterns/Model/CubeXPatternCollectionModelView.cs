//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема модели данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternCollectionModelView.cs
*		Тип коллекции для хранения модели которые которая поддерживает концепцию просмотра и управления
*		Определение интерфейса для коллекции модели которой поддерживают концепцию просмотра и управления, а также типа
*	коллекции реализующего минимальный механизм коллекции для моделей которые поддерживают концепцию просмотра и управления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternModel
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения коллекции для модели которая поддерживает концепцию просмотра и управления
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXCollectionModelView : ICubeXCollectionModel
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Текущая выбранная модель
			/// </summary>
			ICubeXModelView ISelectedModel { get; set; }

			/// <summary>
			/// Текущая модель для отображения
			/// </summary>
			ICubeXModelView IPresentedModel { get; set; }

			/// <summary>
			/// Возможность выбора нескольких моделей
			/// </summary>
			Boolean IsMultiSelected { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выключение выбора всех моделей кроме исключаемой
			/// </summary>
			/// <param name="exclude">Исключаемая модель</param>
			//---------------------------------------------------------------------------------------------------------
			void UnsetAllSelected(ICubeXModelView exclude);

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выключение презентации всех моделей кроме исключаемой
			/// </summary>
			/// <param name="exclude">Исключаемая модель</param>
			//---------------------------------------------------------------------------------------------------------
			void UnsetAllPresent(ICubeXModelView exclude);
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон коллекции для модели которая поддерживает концепцию просмотра и управления с полноценной 
		/// поддержкой всех операций
		/// </summary>
		/// <typeparam name="TModel">Тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class CollectionModelView<TModel, TList> : CollectionModel<TModel, TList>, ICubeXCollectionModelView
			where TModel : ICubeXModelView
			where TList : ListArray<TModel>, new()
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			protected static readonly PropertyChangedEventArgs PropertyArgsSelectedModel = new PropertyChangedEventArgs(nameof(SelectedModel));
			protected static readonly PropertyChangedEventArgs PropertyArgsPresentedModel = new PropertyChangedEventArgs(nameof(PresentedModel));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			protected internal TModel mSelectedModel;
			protected internal TModel mPresentedModel;
			protected internal Boolean mIsMultiSelected;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Текущая выбранная модель
			/// </summary>
			[Browsable(false)]
			public ICubeXModelView ISelectedModel
			{
				get { return (mSelectedModel); }
				set
				{
					SelectedModel = (TModel)value;
				}
			}

			/// <summary>
			/// Текущая выбранная модель
			/// </summary>
			[Browsable(false)]
			public TModel SelectedModel
			{
				get
				{
					return (mSelectedModel);
				}
				set
				{
					if (System.Object.ReferenceEquals(mSelectedModel, value) == false)
					{
						mSelectedModel = value;
						NotifyPropertyChanged(PropertyArgsSelectedModel);
					}
				}
			}

			/// <summary>
			/// Текущая модель для отображения
			/// </summary>
			[Browsable(false)]
			public ICubeXModelView IPresentedModel
			{
				get { return (mPresentedModel); }
				set
				{
					PresentedModel = (TModel)value;
				}
			}

			/// <summary>
			/// Текущая модель для отображения
			/// </summary>
			[Browsable(false)]
			public TModel PresentedModel
			{
				get
				{
					return (mPresentedModel);
				}
				set
				{
					if (System.Object.ReferenceEquals(mPresentedModel, value) == false)
					{
						mPresentedModel = value;
						NotifyPropertyChanged(PropertyArgsPresentedModel);
					}
				}
			}

			/// <summary>
			/// Возможность выбора нескольких элементов
			/// </summary>
			[Browsable(false)]
			public Boolean IsMultiSelected
			{
				get
				{
					return (mIsMultiSelected);
				}
				set
				{
					mIsMultiSelected = value;
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CollectionModelView()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя коллекции</param>
			//---------------------------------------------------------------------------------------------------------
			public CollectionModelView(String name)
				: base(name)
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение копии объекта
			/// </summary>
			/// <returns>Копия объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object Clone()
			{
				CollectionModelView<TModel, TList> clone = new CollectionModelView<TModel, TList>();
				clone.Name = mName;

				for (Int32 i = 0; i < mModels.Count; i++)
				{
					clone.AddExistingModel((TModel)mModels[i].Clone());
				}

				return (clone);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Имя объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (mName);
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXCollectionModelView ==========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выключение выбор всех элементов кроме исключаемого
			/// </summary>
			/// <param name="exclude">Исключаемый элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetAllSelected(ICubeXModelView exclude)
			{
				if (exclude != null)
				{
					for (Int32 i = 0; i < mModels.Count; i++)
					{
						if (Object.ReferenceEquals(mModels[i], exclude) == false)
						{
							mModels[i].IsSelected = false;
						}
					}

					SelectedModel = (TModel)exclude;
				}
				else
				{
					for (Int32 i = 0; i < mModels.Count; i++)
					{
						mModels[i].IsSelected = false;
					}

					SelectedModel = default;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выключение презентации всех элементов кроме исключаемого
			/// </summary>
			/// <param name="exclude">Исключаемый элемент</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void UnsetAllPresent(ICubeXModelView exclude)
			{
				if (exclude != null)
				{
					for (Int32 i = 0; i < mModels.Count; i++)
					{
						if (Object.ReferenceEquals(mModels[i], exclude) == false)
						{
							mModels[i].IsPresented = false;
						}
					}

					PresentedModel = (TModel)exclude;
				}
				else
				{
					// Выключаем все модели
					for (Int32 i = 0; i < mModels.Count; i++)
					{
						mModels[i].IsPresented = false;
					}

					PresentedModel = default;
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон коллекции для модели которая поддерживает концепцию просмотра и управления с полноценной поддержкой всех операций
		/// </summary>
		/// <typeparam name="TModel">Тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class CollectionModelView<TModel> : CollectionModelView<TModel, ListArray<TModel>> 
			where TModel : ICubeXModelView
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CollectionModelView()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя коллекции</param>
			//---------------------------------------------------------------------------------------------------------
			public CollectionModelView(String name)
				: base(name)
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение копии объекта
			/// </summary>
			/// <returns>Копия объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object Clone()
			{
				CollectionModelView<TModel> clone = new CollectionModelView<TModel>();
				clone.Name = mName;

				for (Int32 i = 0; i < mModels.Count; i++)
				{
					clone.AddExistingModel((TModel)mModels[i].Clone());
				}

				return (clone);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Имя объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (mName);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================