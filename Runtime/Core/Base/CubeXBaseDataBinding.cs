﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseDataBinding.cs
*		Определение интерфейса для нотификации об изменении данных и базового класса для оповещения об изменении своих свойств.
*		Указанный интерфейс является базой для реализации связывания данных. Он обеспечивает концепцию «издатель-подписчик»,
*	в рамках того что если объект является «издателем», т.е. другим объектам нужно знать об изменение его свойства, он
*	обязательно должен правильно и безопасно реализовать указанный интерфейс.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Reflection;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup CoreModuleBase
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение основного интерфейса для нотификации об изменении данных
		/// </summary>
		/// <remarks>
		/// Указанный интерфейс является базой для реализации связывания данных. Он обеспечивает концепцию «издатель-подписчик»,
		/// в рамках того что если объект является «издателем», т.е. другим объектам нужно знать об изменение его свойства,
		/// он обязательно должен правильно и безопасно реализовать указанный интерфейс
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXNotifyPropertyChanged
		{
			/// <summary>
			/// Событие для нотификации об изменении значения свойства. Аргумент - имя свойства и его новое значение
			/// </summary>
			Action<String, System.Object> OnPropertyChanged { get; set; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Основные типы действий по изменению коллекций
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TNotifyCollectionChangedAction
		{
			/// <summary>
			/// Элемент/элементы был добавлен/вставлен в коллекцию
			/// </summary>
			/// <remarks>
			/// Описание аргументов:
			/// - Объект - элемент который добавился, если список элементов по передается как IList
			/// - Индекс элемента - если добавляется то указывается последний индекс, иначе указывается индекс вставки
			/// - Количество элементов - либо 1 либо количество элементов в списке
			/// </remarks>
			Add,

			/// <summary>
			/// Элемент был перемещен в пределах коллекции
			/// </summary>
			/// <remarks>
			/// Описание аргументов:
			/// - Объект - элемент который переместился, может быть null
			/// - Индекс элемента - указывается предыдущаю позиция элемента
			/// - Количество элементов - указывается новая позицяю элемента
			/// </remarks>
			Move,

			/// <summary>
			/// Элемент был удален из коллекции
			/// </summary>
			/// <remarks>
			/// Описание аргументов:
			/// - Объект - элемент который удалили, удаляемый элемент может быть null
			/// - Индекс элемента - указывается индекс элемента которого удалили
			/// - Количество элементов - указывается количество удаляемых элементов
			/// </remarks>
			Remove,

			/// <summary>
			/// Элемент был заменен в коллекции
			/// </summary>
			/// <remarks>
			/// Описание аргументов:
			/// - Объект - новый элемент
			/// - Индекс элемента - указывается индекс элемента которого заменили
			/// - Количество элементов - указывается 1
			/// </remarks>
			Replace,

			/// <summary>
			/// Свойства(данные) элемента в коллекции были изменены
			/// </summary>
			/// <remarks>
			/// Применятся только для элементов которые имеют сложный тип
			/// Описание аргументов:
			/// - Объект - Имя свойства которое изменилось (тип String)
			/// - Индекс элемента - указывается индекс элемента свойства которого изменились
			/// - Количество элементов - указывается 1
			/// </remarks>
			ModifyItem,

			/// <summary>
			/// Содержимое коллекции было переустановлено
			/// </summary>
			/// <remarks>
			/// Описание аргументов:
			/// - Объект - коллекция новых элементов передается как IList
			/// - Индекс элемента - не используется (указывается 0)
			/// - Количество элементов - указывается количество элементов
			/// </remarks>
			Reset,

			/// <summary>
			/// Содержимое коллекции было удалено
			/// </summary>
			/// <remarks>
			/// Описание аргументов:
			/// - Объект - не используется (указывается null)
			/// - Индекс элемента - не используется (указывается 0)
			/// - Количество элементов - не используется (указывается 0)
			/// </remarks>
			Clear
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение основного интерфейса для нотификации изменения коллекции
		/// </summary>
		/// <remarks>
		/// Реализация данного интерфейса любой коллекции определяет так называемую наблюдаемую коллекцию.
		/// Наблюдаемая коллекция - коллекция которая информирует о всех изменения которые происходят с ней
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXNotifyCollectionChanged
		{
			/// <summary>
			/// Событие для нотификации о любом изменении коллекции
			/// </summary>
			/// <remarks>
			/// Аргументы:
			/// - Тип происходящего действия
			/// - Объект - элемент над которым производится действия, либо коллекция
			/// - Индекс элемента
			/// - Количество элементов
			/// Интерпретацию передаваемых аргументов смотреть в комментариях к перечислению <see cref="TNotifyCollectionChangedAction"/>
			/// </remarks>
			Action<TNotifyCollectionChangedAction, System.Object, Int32, Int32> OnCollectionChanged { get; set; }
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для реализации оповещения об изменении своих свойств
		/// </summary>
		/// <remarks>
		/// В качестве нотификации о изменение свойств используются стандартный интерфейс уведомлений <see cref="INotifyPropertyChanged"/>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public class PropertyChangedBase : INotifyPropertyChanged
		{
			#region ======================================= ДАННЫЕ INotifyPropertyChanged =============================
			/// <summary>
			/// Событие срабатывает ПОСЛЕ изменения свойства
			/// </summary>
			public event PropertyChangedEventHandler PropertyChanged;

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(String property_name = "")
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(property_name));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Вспомогательный метод для нотификации изменений свойства
			/// </summary>
			/// <param name="args">Аргументы события</param>
			//---------------------------------------------------------------------------------------------------------
			public void NotifyPropertyChanged(PropertyChangedEventArgs args)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, args);
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