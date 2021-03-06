﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема иерархической модели
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternHierarchyModelLast.cs
*		Определение интерфейса модели и шаблонов данных для модели которая является последней в иерархии (замыкает иерархию).
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
		/// Интерфейс для определения модели которая поддерживает иерархические отношения 
		/// и служит для обозначения окончания иерархии
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXModelHierarchyLast
		{
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Шаблон реализующий механизм модели который участвует в иерархических отношениях, но не содержит дочерних данных
		/// </summary>
		/// <typeparam name="TCollectionModel">Соответствующий тип коллекции</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class ModelHierarchyLast<TCollectionModel> : ModelOwned<TCollectionModel>, ICubeXModelHierarchy, ICubeXModelHierarchyLast
			where TCollectionModel : ICubeXCollectionModel
		{
			#region ======================================= СВОЙСТВА ICubeXCollectionModel ============================
			/// <summary>
			/// Список моделей коллекции
			/// </summary>
			[Browsable(false)]
			public IList IModels
			{
				get
				{
					return (null);
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyLast()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public ModelHierarchyLast(String name)
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
				ModelHierarchyLast<TCollectionModel> clone = new ModelHierarchyLast<TCollectionModel>(mName);

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
			public void UpdateOwnerLink()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на поддержку модели
			/// </summary>
			/// <remarks>
			/// Поддержка модели подразумевает возможность добавить ее в список элементов по различным критериям 
			/// </remarks>
			/// <param name="model">Модель</param>
			/// <returns>Статус поддержки модели</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean IsSupportModel(ICubeXModelOwned model)
			{
				return (false);
			}

			//--------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Фильтрация дочерних элементов по предикату
			/// </summary>
			/// <param name="match">Предикат</param>
			/// <returns>Истина если объект или его дочерний элемент проходят условия проверки предикатом</returns>
			//--------------------------------------------------------------------------------------------------------
			public virtual Boolean FiltredModel(Predicate<ICubeXModel> match)
			{
				if (match != null)
				{
					return (match(this));
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
			public virtual void VisitModel(Action<ICubeXModel> on_visitor)
			{
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для объекта модели который участвует в иерархических отношениях, но не содержит дочерних данных
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CModelHierarchyLast : ModelHierarchyLast<ICubeXCollectionModel>, IComparable<CModelHierarchyLast>
		{
			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CModelHierarchyLast()
				: this(String.Empty)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя объекта</param>
			//---------------------------------------------------------------------------------------------------------
			public CModelHierarchyLast(String name)
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
			public Int32 CompareTo(CModelHierarchyLast other)
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
				CModelHierarchyLast clone = new CModelHierarchyLast(mName);
				
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