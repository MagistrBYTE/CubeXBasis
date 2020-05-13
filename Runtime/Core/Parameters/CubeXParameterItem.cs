//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема параметрических объектов
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXParameterItem.cs
*		Класс для представления параметра - объекта который содержит данные в формате имя=значения.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleParameters Подсистема параметрических объектов
		//! Подсистема параметрических объектов обеспечивает представление и хранение информации в документоориентированном 
		//! стиле. Основной объект подсистемы — это параметрический объект который хранит список записей в формате
		//! имя=значения. При этом сама запись также может представлена виде параметрического объекта. Это обеспечивает 
		//! представления иерархических структур данных.
		//! \ingroup CoreModule
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение допустимых типов значения для параметра
		/// </summary>
		/// <remarks>
		/// Определение стандартных типов данных значения в контексте использования параметра.
		/// Типы значений спроектированы с учетом поддержки и реализации в современных документоориентированных СУБД.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public enum TParameterValueType
		{
			//
			// ОСНОВНЫЕ ТИПЫ ДАННЫХ
			//
			/// <summary>
			/// Отсутствие определенного значения
			/// </summary>
			Null,

			/// <summary>
			/// Логический тип
			/// </summary>
			Boolean,

			/// <summary>
			/// Целый тип
			/// </summary>
			Integer,

			/// <summary>
			/// Вещественный тип
			/// </summary>
			Real,

			/// <summary>
			/// Тип даты-времени
			/// </summary>
			DateTime,

			/// <summary>
			/// Строковый тип
			/// </summary>
			String,

			/// <summary>
			/// Перечисление
			/// </summary>
			Enum,

			/// <summary>
			/// Массив
			/// </summary>
			Array,

			/// <summary>
			/// Параметрический объект
			/// </summary>
			ParameterObject,

			//
			// ДОПОЛНИТЕЛЬНЫЕ ТИПЫ ДАННЫХ 
			//
			/// <summary>
			/// Цвет
			/// </summary>
			Color,

			/// <summary>
			/// Двухмерный объект данных
			/// </summary>
			Vector2D,

			/// <summary>
			/// Трехмерный объект данных
			/// </summary>
			Vector3D,

			/// <summary>
			/// Четырехмерный объект данных
			/// </summary>
			Vector4D,

			/// <summary>
			/// Прямоугольник
			/// </summary>
			Rect
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Определение интерфейса для представления параметра - объекта который содержит данные в формате имя=значения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface IParameterItem : ICloneable, ICubeXIdentifierID, ICubeXNotCalculation, ICubeXVerified
		{
			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Наименование параметра
			/// </summary>
			/// <remarks>
			/// Имя параметра должно быть уникальных в пределах параметрического объекта
			/// </remarks>
			String Name { get; set; }

			/// <summary>
			/// Тип данных значения
			/// </summary>
			TParameterValueType ValueType { get; }

			/// <summary>
			/// Значение параметра
			/// </summary>
			System.Object IValue { get; set; }

			/// <summary>
			/// Активность параметра
			/// </summary>
			/// <remarks>
			/// Условная активность параметра - на усмотрение пользователя
			/// </remarks>
			Boolean IsActive { get; set; }

			/// <summary>
			/// Видимость параметра
			/// </summary>
			/// <remarks>
			/// Условная видимость параметра - на усмотрение пользователя
			/// </remarks>
			Boolean IsVisible { get; set; }

			/// <summary>
			/// Порядок отображения параметра при его представлении
			/// </summary>
			/// <remarks>
			/// Порядок отображения параметра используется при отображении в инспекторе свойств
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			Int32 OrderVisible { get; set; }

			/// <summary>
			/// Пользовательский тэг данных
			/// </summary>
			/// <remarks>
			/// Условный пользовательский тэг данных - на усмотрение пользователя.
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			Int32 UserTag { get; set; }

			/// <summary>
			/// Пользовательский тип данных
			/// </summary>
			/// <remarks>
			/// Условный пользовательский тип данных - на усмотрение пользователя.
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			Int32 UserData { get; set; }

			/// <summary>
			/// Владелец параметра
			/// </summary>
			ICubeXNotify IOwned { get; set; }
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс для представления параметра - объекта который содержит данные в формате имя=значения
		/// </summary>
		/// <typeparam name="TValue">Тип значения</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public abstract class ParameterItem<TValue> : PropertyChangedBase, IParameterItem, 
			IComparable<ParameterItem<TValue>>, ICubeXDuplicate<ParameterItem<TValue>>
		{
			#region ======================================= СТАТИЧЕСКИЕ ДАННЫЕ ========================================
			//
			// Константы для информирования об изменении свойств
			//
			// Основные параметры
			protected static readonly PropertyChangedEventArgs PropertyArgsName = new PropertyChangedEventArgs(nameof(Name));
			protected static readonly PropertyChangedEventArgs PropertyArgsID = new PropertyChangedEventArgs(nameof(ID));
			protected static readonly PropertyChangedEventArgs PropertyArgsIValue = new PropertyChangedEventArgs(nameof(IValue));
			protected static readonly PropertyChangedEventArgs PropertyArgsValue = new PropertyChangedEventArgs(nameof(Value));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsActive = new PropertyChangedEventArgs(nameof(IsActive));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsVisible = new PropertyChangedEventArgs(nameof(IsVisible));
			protected static readonly PropertyChangedEventArgs PropertyArgsOrderVisible = new PropertyChangedEventArgs(nameof(OrderVisible));
			protected static readonly PropertyChangedEventArgs PropertyArgsUserTag = new PropertyChangedEventArgs(nameof(UserTag));
			protected static readonly PropertyChangedEventArgs PropertyArgsUserData = new PropertyChangedEventArgs(nameof(UserData));

			// Расчеты
			protected static readonly PropertyChangedEventArgs PropertyArgsNotCalculation = new PropertyChangedEventArgs(nameof(NotCalculation));
			protected static readonly PropertyChangedEventArgs PropertyArgsIsVerified = new PropertyChangedEventArgs(nameof(IsVerified));
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			protected internal String mName;
			protected internal TValue mValue;
			protected internal Int32 mID;
			protected internal Int32 mData;

			// Расчеты
			protected internal Boolean mNotCalculation;
			protected internal Boolean mIsVerified;

			// Владелец
			internal protected ICubeXNotify mIOwned;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Наименование параметра
			/// </summary>
			/// <remarks>
			/// Имя параметра должно быть уникальных в пределах параметрического объекта
			/// </remarks>
			[Browsable(false)]
			[XmlAttribute]
			public String Name
			{
				get { return mName; }
				set 
				{
					mName = value;
					NotifyPropertyChanged(PropertyArgsName);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Name));
				}
			}

			/// <summary>
			/// Тип данных значения
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public virtual TParameterValueType ValueType
			{
				get { return TParameterValueType.Null; }
			}

			/// <summary>
			/// Значение параметра
			/// </summary>
			[Browsable(false)]
			[XmlIgnore]
			public System.Object IValue 
			{
				get { return (mValue); }
				set 
				{
					mValue = (TValue)value;
					NotifyPropertyChanged(PropertyArgsIValue);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(IValue));
				}
			}

			/// <summary>
			/// Значение параметра
			/// </summary>
			[Browsable(false)]
			[XmlElement]
			public TValue Value
			{
				get { return (mValue); }
				set
				{
					mValue = (TValue)value;
					NotifyPropertyChanged(PropertyArgsValue);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Value));
				}
			}

			/// <summary>
			/// Уникальный идентификатор параметра
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public Int32 ID
			{
				get { return (mID); }
				set
				{
					mID = value;
					NotifyPropertyChanged(PropertyArgsID);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ID));
				}
			}

			/// <summary>
			/// Активность параметра
			/// </summary>
			/// <remarks>
			/// Условная активность параметра - на усмотрение пользователя
			/// </remarks>
			[Browsable(false)]
			[XmlAttribute]
			public Boolean IsActive
			{
				get { return XPacked.UnpackBoolean(mData, 0); }
				set 
				{ 
					XPacked.PackBoolean(ref mData, 0, value);
					NotifyPropertyChanged(PropertyArgsIsActive);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(IsActive));
				}
			}

			/// <summary>
			/// Видимость параметра
			/// </summary>
			/// <remarks>
			/// Условная видимость параметра - на усмотрение пользователя
			/// </remarks>
			[Browsable(false)]
			[XmlAttribute]
			public Boolean IsVisible
			{
				get { return XPacked.UnpackBoolean(mData, 1); }
				set 
				{
					XPacked.PackBoolean(ref mData, 1, value);
					NotifyPropertyChanged(PropertyArgsIsVisible);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(IsVisible));
				}
			}

			/// <summary>
			/// Порядок отображения параметра при его представлении
			/// </summary>
			/// <remarks>
			/// Порядок отображения параметра используется при отображении в инспекторе свойств
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			[Browsable(false)]
			[XmlAttribute]
			public Int32 OrderVisible
			{
				get { return XPacked.UnpackInteger(mData, 2, 8); }
				set
				{
					XPacked.PackInteger(ref mData, 2, 8, value);
					NotifyPropertyChanged(PropertyArgsOrderVisible);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(OrderVisible));
				}
			}

			/// <summary>
			/// Пользовательский тэг данных
			/// </summary>
			/// <remarks>
			/// Условный пользовательский тэг данных - на усмотрение пользователя.
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			[Browsable(false)]
			[XmlAttribute]
			public Int32 UserTag
			{
				get { return XPacked.UnpackInteger(mData, 10, 8); }
				set 
				{
					XPacked.PackInteger(ref mData, 10, 8, value);
					NotifyPropertyChanged(PropertyArgsUserTag);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(UserTag));
				}
			}

			/// <summary>
			/// Пользовательский тип данных
			/// </summary>
			/// <remarks>
			/// Условный пользовательский тип данных - на усмотрение пользователя.
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			[Browsable(false)]
			[XmlAttribute]
			public Int32 UserData
			{
				get { return XPacked.UnpackInteger(mData, 18, 8); }
				set 
				{ 
					XPacked.PackInteger(ref mData, 18, 8, value);
					NotifyPropertyChanged(PropertyArgsUserData);
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(UserData));
				}
			}

			/// <summary>
			/// Владелец параметра
			/// </summary>
			[Browsable(false)]
			[XmlIgnore]
			public ICubeXNotify IOwned
			{
				get { return mIOwned; }
				set { mIOwned = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXNotCalculation =============================
			/// <summary>
			/// Не учитывать параметр в расчетах
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public Boolean NotCalculation
			{
				get { return (mNotCalculation); }
				set
				{
					mNotCalculation = value;
					NotifyPropertyChanged(PropertyArgsNotCalculation);
					RaiseNotCalculationChanged();
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXVerified ===================================
			/// <summary>
			/// Статус верификации параметра
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public Boolean IsVerified
			{
				get { return (mIsVerified); }
				set
				{
					mIsVerified = value;
					NotifyPropertyChanged(PropertyArgsIsVerified);
					RaiseIsVerifiedChanged();
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected ParameterItem()
			{
				mName = "";
				mData = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="parameter_name">Имя параметра</param>
			//---------------------------------------------------------------------------------------------------------
			protected ParameterItem(String parameter_name)
			{
				mName = parameter_name;
				mData = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="id">Идентификатор параметра</param>
			//---------------------------------------------------------------------------------------------------------
			protected ParameterItem(Int32 id)
			{
				mName = "";
				mID = id;
				mData = 0;
			}
			#endregion
			
			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение параметров для упорядочивания
			/// </summary>
			/// <param name="other">Настройка</param>
			/// <returns>Статус сравнения параметров</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(ParameterItem<TValue> other)
			{
				return (String.CompareOrdinal(Name, other.Name));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода параметра
			/// </summary>
			/// <returns>Хеш-код параметра</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return (this.Name.GetHashCode() ^ mID.GetHashCode());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование параметра
			/// </summary>
			/// <returns>Копия объекта параметра</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Clone()
			{
				return (MemberwiseClone());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дубликата объекта
			/// </summary>
			/// <returns>Дубликат объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public ParameterItem<TValue> Duplicate()
			{
				return (MemberwiseClone() as ParameterItem<TValue>);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление параметра</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String result = String.Format("{0} = {1}", mName, base.ToString());
				return (result);
			}
			#endregion

			#region ======================================= СЛУЖЕБНЫЕ МЕТОДЫ СОБЫТИЙ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса расчёта набора в расчётах.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseNotCalculationChanged()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Изменения статуса верификации объекта.
			/// Метод автоматически вызывается после установки соответствующего свойства
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void RaiseIsVerifiedChanged()
			{

			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================