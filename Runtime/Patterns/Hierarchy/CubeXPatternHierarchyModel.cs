//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема иерархической модели
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternHierarchyModel.cs
*		Определение интерфейса модели и шаблонов данных для построения иерархических отношений.
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
		//! \defgroup CorePatternHierarchy Подсистема иерархической модели
		//! Подсистема иерархической модели обеспечивает взаимосвязанную иерархическую модель отношений
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения модели которая поддерживает иерархические отношения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXModelHierarchy : ICubeXModelOwned, ICubeXCollectionModel
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон реализующий механизм модели поддерживающей иерархические отношения которая при этом полноценно управляет 
		/// своими элементами(моделями)
		/// </summary>
		/// <typeparam name="TModel">Соответствующий тип модели</typeparam>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelHierarchy<TModel, TCollectionModel> : CollectionModel<TModel>, ICubeXModelHierarchy
			where TCollectionModel : class, ICubeXCollectionModel
			where TModel : ICubeXModelHierarchy
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal TCollectionModel mOwner;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Владелец
			/// </summary>
			[Browsable(false)]
			public TCollectionModel Owner
			{
				get { return (mOwner); }
				set { mOwner = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXModelOwned =================================
			/// <summary>
			/// Владелец
			/// </summary>
			[Browsable(false)]
			public ICubeXCollectionModel IOwner
			{
				get { return (mOwner); }
				set { mOwner = (TCollectionModel)value; }
			}

			/// <summary>
			/// Уникальный идентификатор владельца
			/// </summary>
			[Browsable(false)]
			public Int64 ParentID
			{
				get
				{
					if (mOwner != null)
					{
						return (mOwner.ID);
					}
					return (-1);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchy()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя модели</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchy(String name)
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
				ModelHierarchy<TModel, TCollectionModel> clone = new ModelHierarchy<TModel, TCollectionModel>();
				clone.Name = mName;

				for (Int32 i = 0; i < mModels.Count; i++)
				{
					clone.AddExistingEmptyModel((TModel)mModels[i].Clone());
				}

				if (clone is ICubeXGroupHierarchy clone_group && this is ICubeXGroupHierarchy group)
				{
					clone_group.IsGroupProperty = group.IsGroupProperty;
					clone_group.GroupPropertyName = group.GroupPropertyName;
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

			//--------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Фильтрация дочерних элементов по предикату
			/// </summary>
			/// <param name="match">Предикат</param>
			/// <returns>Истина если объект или его дочерний элемент проходят условия проверки предикатом</returns>
			//--------------------------------------------------------------------------------------------------------
			public override Boolean FiltredModel(Predicate<ICubeXModel> match)
			{
				if (match != null)
				{
					// Проверяем даный узел
					Boolean result = match(this);

					// Проверяем дочерние узлы
					if (mModels != null && mModels.Count > 0)
					{
						for (Int32 i = 0; i < mModels.Count; i++)
						{
							result = mModels[i].FiltredModel(match);
							if (result)
							{
								result = true;
								break;
							}
						}
					}

					return (result);
				}
				else
				{
					return (true);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Посещение элементов и дочерних элементов указанным посетителем
			/// </summary>
			/// <param name="on_visitor">Делегат посетителя</param>
			//---------------------------------------------------------------------------------------------------------
			public override void VisitModel(Action<ICubeXModel> on_visitor)
			{
				if (mModels != null && mModels.Count > 0 && on_visitor != null)
				{
					for (Int32 i = 0; i < mModels.Count; i++)
					{
						on_visitor(mModels[i]);
						if(mModels[i] != null)
						{
							mModels[i].VisitModel(on_visitor);
						}
					}
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс реализующий методы расширений для интерфейса <see cref="ICubeXModelHierarchy"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionModelHierarchy
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование иерархической модели в плоский список моделей
			/// </summary>
			/// <typeparam name="TItem">Тип модели списка</typeparam>
			/// <param name="model_hierarchy">Модель которая поддерживает иерархические отношения</param>
			/// <param name="collection">Список который будет заполнен</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ToFlatCollection<TItem>(this ICubeXModelHierarchy model_hierarchy, ref ListArray<TItem> collection)
				where TItem : ICubeXModelOwned
			{
				if (model_hierarchy != null)
				{
					for (Int32 i = 0; i < model_hierarchy.IModels.Count; i++)
					{
						if (model_hierarchy.IModels[i] is TItem item)
						{
							collection.Add(item);
						}
						else
						{
							if (model_hierarchy.IModels[i] is ICubeXModelHierarchy model_hierarchy_inner)
							{
								model_hierarchy_inner.ToFlatCollection(ref collection);
							}
						}
					}
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================