//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема иерархической модели
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternHierarchyModelViewBegin.cs
*		Определение интерфейса модели и шаблонов данных для построения начала иерархических отношений с поддержкой 
*	концепции просмотра и управления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CorePatternHierarchy
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон реализующий начальный механизм иерархических модели
		/// </summary>
		/// <remarks>
		/// Это шаблон специального типа для предоставления корня иерархии
		/// </remarks>
		/// <typeparam name="TModel">Тип модели</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchyViewBegin<TModel> : CollectionModelView<TModel>, ICubeXModelHierarchyView, 
			ICubeXModelHierarchyBegin, ICubeXGroupHierarchy
			where TModel : ICubeXModelHierarchyView
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// Константы для информирования об изменении свойств
			//
			// Основные параметры
			protected static readonly PropertyChangedEventArgs PropertyArgsIsEnabled = new PropertyChangedEventArgs(nameof(IsEnabled));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsSelected = new PropertyChangedEventArgs(nameof(IsSelected));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsChecked = new PropertyChangedEventArgs(nameof(IsChecked));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsPresented = new PropertyChangedEventArgs(nameof(IsPresented));

			// Группирование
			protected static readonly PropertyChangedEventArgs PropertyArgsIsGroupProperty = new PropertyChangedEventArgs(nameof(IsGroupProperty));
			protected static readonly PropertyChangedEventArgs PropertyArgsGroupPropertyName = new PropertyChangedEventArgs(nameof(GroupPropertyName));
			protected static readonly PropertyChangedEventArgs PropertyArgsGroupPropertyValue = new PropertyChangedEventArgs(nameof(GroupPropertyValue));
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			protected internal Boolean mIsEnabled;
			protected internal Boolean mIsSelected;
			protected internal Boolean? mIsChecked;
			protected internal Boolean mIsPresented;

			// Группирование
			protected internal Boolean mIsGroupProperty;
			protected internal String mGroupPropertyName;
			#endregion

			#region ======================================= СВОЙСТВА ICubeXModelOwned =================================
			/// <summary>
			/// Владелец
			/// </summary>
			[Browsable(false)]
			public ICubeXCollectionModel IOwner
			{
				get { return (null); }
				set { }
			}

			/// <summary>
			/// Уникальный идентификатор владельца
			/// </summary>
			[Browsable(false)]
			public Int64 ParentID
			{
				get
				{
					return (-1);
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXModelView ==================================
			/// <summary>
			/// Доступность элемента
			/// </summary>
			[Browsable(false)]
			public Boolean IsEnabled
			{
				get { return (mIsEnabled); }
				set
				{
					if (mIsEnabled != value)
					{
						mIsEnabled = value;
						NotifyPropertyChanged(PropertyArgsIsEnabled);
						RaiseIsEnabledChanged();
					}
				}
			}

			/// <summary>
			/// Выбор элемента
			/// </summary>
			[Browsable(false)]
			public Boolean IsSelected
			{
				get { return (mIsSelected); }
				set
				{
					if (mIsSelected != value)
					{
						mIsSelected = value;

						NotifyPropertyChanged(PropertyArgsIsSelected);
						RaiseIsSelectedChanged();
					}
				}
			}

			/// <summary>
			/// Выбор элемента
			/// </summary>
			[Browsable(false)]
			public Boolean? IsChecked
			{
				get { return (mIsChecked); }
				set
				{
					if (mIsChecked != value)
					{
						mIsChecked = value;
						NotifyPropertyChanged(PropertyArgsIsChecked);
						RaiseIsCheckedChanged();
					}
				}
			}

			/// <summary>
			/// Отображение элемента
			/// </summary>
			[Browsable(false)]
			public Boolean IsPresented
			{
				get { return (mIsPresented); }
				set
				{
					if (mIsPresented != value)
					{
						mIsPresented = value;
						NotifyPropertyChanged(PropertyArgsIsPresented);
						RaiseIsPresentedChanged();
					}
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXGroupHierarchy =============================
			/// <summary>
			///Статус группирования иерархии элементов по указанному свойству
			/// </summary>
			[Browsable(false)]
			public Boolean IsGroupProperty
			{
				get { return (mIsGroupProperty); }
				set
				{
					mIsGroupProperty = value;
					NotifyPropertyChanged(PropertyArgsIsGroupProperty);
					NotifyPropertyChanged(PropertyArgsGroupPropertyName);
					NotifyPropertyChanged(PropertyArgsGroupPropertyValue);
				}
			}

			/// <summary>
			/// Имя свойства по которому осуществляется группирование
			/// </summary>
			[Browsable(false)]
			public String GroupPropertyName
			{
				get { return (mGroupPropertyName); }
				set
				{
					mGroupPropertyName = value;
					NotifyPropertyChanged(PropertyArgsGroupPropertyName);
					NotifyPropertyChanged(PropertyArgsGroupPropertyValue);
				}
			}

			/// <summary>
			/// Значения свойства по которому осуществляется группирование
			/// </summary>
			[Browsable(false)]
			public System.Object GroupPropertyValue
			{
				get
				{
					if (mIsGroupProperty)
					{
						return (GetPropertyValueForGroup());
					}
					else
					{
						return (mName);
					}
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewBegin()
				:this(String.Empty)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя коллекции</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyViewBegin(String name)
				: base(name)
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов для упорядочивания
			/// </summary>
			/// <param name="other">Сравниваемый объект</param>
			/// <returns>Статус сравнения объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(ModelHierarchyViewBegin<TModel> other)
			{
				return (mName.CompareTo(other.Name));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение копии объекта
			/// </summary>
			/// <returns>Копия объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object Clone()
			{
				ModelHierarchyViewBegin<TModel> clone = new ModelHierarchyViewBegin<TModel>(mName);

				for (Int32 i = 0; i < mModels.Count; i++)
				{
					clone.AddExistingEmptyModel((TModel)mModels[i].Clone());
				}

				clone.IsGroupProperty = IsGroupProperty;
				clone.GroupPropertyName = GroupPropertyName;

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

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение доступности элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsEnabledChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение выбора элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsSelectedChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение выбора элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsCheckedChanged()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменение статуса отображения элемента.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsPresentedChanged()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXCollectionModel ==============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление связи с коллекцией для элементов списка
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public override void UpdateOwnerLink()
			{
				for (Int32 i = 0; i < mModels.Count; i++)
				{
					mModels[i].IOwner = this;
					mModels[i].UpdateOwnerLink();
				}
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXGroupHierarchy ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения свойства по которому происходит группирование 
			/// </summary>
			/// <returns>Значение свойства</returns>
			//---------------------------------------------------------------------------------------------------------
			protected virtual System.Object GetPropertyValueForGroup()
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