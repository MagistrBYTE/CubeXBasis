//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль паттернов
// Подраздел: Подсистема группирования модели
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXPatternGroupingCommon.cs
*		Определение типов и общих структур данных для реализации группирования моделей.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CorePatternGrouping Подсистема группирования модели
		//! Подсистема группирования модели обеспечивает формирование новой иерархии отношений из существующей списка моделей
		//! \ingroup CorePattern
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Интерфейс для определения сгруппированной иерархии элементов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXGroupHierarchy
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Статус группирования иерархии элементов
			/// </summary>
			Boolean IsGroupProperty { get; set; }

			/// <summary>
			/// Имя свойства по которому осуществляется группирование
			/// </summary>
			String GroupPropertyName { get; set; }

			/// <summary>
			/// Значения свойства по которому осуществляется группирование
			/// </summary>
			System.Object GroupPropertyValue { get; }
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Группирование моделей по указанному набору свойств
			/// </summary>
			/// <typeparam name="TSet">Тип набора</typeparam>
			/// <param name="property_names">Набор свойств для группирования</param>
			//---------------------------------------------------------------------------------------------------------
			void GroupingByProperties<TSet>(params String[] property_names) where TSet : class, ICubeXCollectionSupportAdd, ICubeXGroupHierarchy, new();
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Статический класс для реализации дополнительных методов работы с <see cref="ICubeXGroupHierarchy"/> 
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public static class XGroupHierarchyUtilites
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Идентификатор отсутствующего сводного значения
			/// </summary>
			public const String NONE_DATA = "Нет значения";
			#endregion

			#region ======================================= МЕТОДЫ ====================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов поддерживающих интерфейс группирования по возрастанию
			/// </summary>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 ComprareOfAscending(System.Object left, System.Object right)
			{
				if (left == null)
				{
					if (right == null)
					{
						return (0);
					}
					else
					{
						return (1);
					}
				}
				else
				{
					if (right == null)
					{
						return (-1);
					}
					else
					{
						if (left is ICubeXGroupHierarchy && right is ICubeXGroupHierarchy)
						{
							System.Object left_value = (left as ICubeXGroupHierarchy).GroupPropertyValue;
							System.Object right_value = (right as ICubeXGroupHierarchy).GroupPropertyValue;

							if(left_value is IComparable)
							{
								IComparable left_value_comparare = left_value as IComparable;
								return(left_value_comparare.CompareTo(right_value));
							}

							return (0);
						}
						else
						{
							return (0);
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение элементов поддерживающих интерфейс группирования по возрастанию
			/// </summary>
			/// <typeparam name="TItem">Тип элемента</typeparam>
			/// <param name="left">Первый объект</param>
			/// <param name="right">Второй объект</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 ComprareOfAscending<TItem>(TItem left, TItem right)
				where TItem : IComparable
			{
				if (left == null)
				{
					if (right == null)
					{
						return (0);
					}
					else
					{
						return (1);
					}
				}
				else
				{
					if (right == null)
					{
						return (-1);
					}
					else
					{
						if (left is ICubeXGroupHierarchy && right is ICubeXGroupHierarchy)
						{
							System.Object left_value = (left as ICubeXGroupHierarchy).GroupPropertyValue;
							System.Object right_value = (right as ICubeXGroupHierarchy).GroupPropertyValue;

							if (left_value is IComparable)
							{
								IComparable left_value_comparare = left_value as IComparable;
								return (left_value_comparare.CompareTo(right_value));
							}

							return (left.CompareTo(right));
						}
						else
						{
							return (left.CompareTo(right));
						}
					}
				}
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================