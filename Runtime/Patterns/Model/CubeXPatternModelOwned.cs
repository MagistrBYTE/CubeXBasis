//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема модели данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternModelOwned.cs
*		Определение интерфейса модели которой владеет коллекция моделей.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		/// Интерфейс для определения модели которой владеет коллекция моделей
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXModelOwned : ICubeXModel
		{
			/// <summary>
			/// Владелец
			/// </summary>
			ICubeXCollectionModel IOwner { get; set; }

			/// <summary>
			/// Уникальный идентификатор владельца
			/// </summary>
			Int64 ParentID { get; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон реализующий минимальный механизм модели которой владеет коллекция моделей
		/// </summary>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		public class ModelOwned<TCollectionModel> : CModel, ICubeXModelOwned
			where TCollectionModel : ICubeXCollectionModel
		{
			#region ======================================= ДАННЫЕ ====================================================
			protected internal TCollectionModel mOwner;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Владелец
			/// </summary>
			[Browsable(false)]
			public TCollectionModel Owner
			{
				get { return (mOwner); }
				set { mOwner = value; }
			}

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
					if(mOwner != null)
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
			public ModelOwned()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelOwned(String name)
				: base(name)
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
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
			/// Изменение имени объекта.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected override void RaiseNameChanged()
			{
				this.NotifyOwnerUpdated(nameof(Name));
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс реализующий минимальный механизм модели которой владеет коллекция
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CModelOwned : ModelOwned<ICubeXCollectionModel>, IComparable<CModelOwned>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CModelOwned()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CModelOwned(String name)
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
			public Int32 CompareTo(CModelOwned other)
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
				CModelOwned clone = new CModelOwned();
				clone.Name = mName;
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
		/// <summary>
		/// Статический класс реализующий методы расширений для интерфейса <see cref="ICubeXModelOwned"/>
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XExtensionModelOwned
		{
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаление модели
			/// </summary>
			/// <param name="model">Модель</param>
			/// <returns>Статус успешности удаления</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean RemoveModel(this ICubeXModelOwned model)
			{
				Boolean status = false;
				if (model != null)
				{
					ICubeXCollectionSupportRemove operation_remove = model.IOwner as ICubeXCollectionSupportRemove;
					if (operation_remove != null)
					{
						status = operation_remove.RemoveModel(model);
					}
				}

				return (status);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Дублирование модели
			/// </summary>
			/// <param name="model">Модель</param>
			/// <returns>Статус успешности дублирования</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Boolean DuplicateModel(this ICubeXModelOwned model)
			{
				Boolean status = false;
				if (model != null)
				{
					ICubeXCollectionSupportInsert operation_insert = model.IOwner as ICubeXCollectionSupportInsert;
					if (operation_insert != null)
					{
						ICubeXModelOwned clone = model.Clone() as ICubeXModelOwned;
						operation_insert.InsertAfterModel(model, clone);
						return (true);
					}
					else
					{
						ICubeXCollectionSupportAdd operation_add = model.IOwner as ICubeXCollectionSupportAdd;
						if (operation_add != null)
						{
							ICubeXModelOwned clone = model.Clone() as ICubeXModelOwned;
							operation_add.AddExistingModel(clone);
							return (true);
						}
					}
				}

				return (status);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение модели вверх
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveUpModel(this ICubeXModelOwned model)
			{
				if (model != null)
				{
					ICubeXCollectionSupportMove operation_move = model.IOwner as ICubeXCollectionSupportMove;
					if (operation_move != null)
					{
						operation_move.MoveUpModel(model);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Перемещение модели вниз
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			public static void MoveDownModel(this ICubeXModelOwned model)
			{
				if (model != null)
				{
					ICubeXCollectionSupportMove operation_move = model.IOwner as ICubeXCollectionSupportMove;
					if (operation_move != null)
					{
						operation_move.MoveDownModel(model);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование владельца об изменении свойства модели
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="property_name">Имя свойства которое изменилось</param>
			//---------------------------------------------------------------------------------------------------------
			public static void NotifyOwnerUpdated(this ICubeXModelOwned model, String property_name)
			{
				if (model.IOwner is ICubeXNotify notify)
				{
					notify.OnNotifyUpdated(model, property_name);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка пар-значения: имя группирования – значение группирования для указанной модели
			/// </summary>
			/// <param name="model">Модель</param>
			/// <returns>Список пар значений</returns>
			//---------------------------------------------------------------------------------------------------------
			public static List<KeyValuePair<String, System.Object>> GetGroupingFromHierarchyOfOwner(this ICubeXModelOwned model)
			{
				List<KeyValuePair<String, System.Object>> result = new List<KeyValuePair<String, System.Object>>();
				if (model.IOwner is ICubeXGroupHierarchy)
				{
					ICubeXGroupHierarchy group_hierarchy = model.IOwner as ICubeXGroupHierarchy;
					if (group_hierarchy.IsGroupProperty)
					{
						result.Add(new KeyValuePair<String, System.Object>(group_hierarchy.GroupPropertyName, group_hierarchy.GroupPropertyValue));
					}
					if(model.IOwner is ICubeXModelOwned)
					{
						ICubeXModelOwned model_owned_owner = model.IOwner as ICubeXModelOwned;
						GetGroupingFromHierarchyOfOwner(model_owned_owner, ref result);
					}
				}

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка пар значения имя группирования – значение группирования для указанной модели
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="result">Список пар значений</param>
			//---------------------------------------------------------------------------------------------------------
			private static void GetGroupingFromHierarchyOfOwner(this ICubeXModelOwned model, ref List<KeyValuePair<String, System.Object>> result)
			{
				if (model.IOwner is ICubeXGroupHierarchy)
				{
					ICubeXGroupHierarchy group_hierarchy = model.IOwner as ICubeXGroupHierarchy;
					if (group_hierarchy.IsGroupProperty)
					{
						result.Add(new KeyValuePair<String, System.Object>(group_hierarchy.GroupPropertyName, group_hierarchy.GroupPropertyValue));
					}
					if (model.IOwner is ICubeXModelOwned)
					{
						ICubeXModelOwned model_owned_owner = model.IOwner as ICubeXModelOwned;
						GetGroupingFromHierarchyOfOwner(model_owned_owner, ref result);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения свойств на основании сгруппированной иерархической модели
			/// </summary>
			/// <param name="model">Модель</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetPropertyValueFromGroupingHierarchy(this ICubeXModelOwned model)
			{
				List<KeyValuePair<String, System.Object>> result = model.GetGroupingFromHierarchyOfOwner();

				for (Int32 i = 0; i < result.Count; i++)
				{
					XReflection.SetPropertyValue(model, result[i].Key, result[i].Value);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения свойств на основании сгруппированной иерархической модели
			/// </summary>
			/// <param name="model">Модель</param>
			/// <param name="properties">Список свойств</param>
			//---------------------------------------------------------------------------------------------------------
			public static void SetPropertyValueFromGroupingHierarchy(this ICubeXModelOwned model, List<KeyValuePair<String, System.Object>> properties)
			{
				for (Int32 i = 0; i < properties.Count; i++)
				{
					if (properties[i].Key.IsExists())
					{
						XReflection.SetPropertyValue(model, properties[i].Key, properties[i].Value);
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