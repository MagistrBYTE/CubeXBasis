//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseSettings.cs
*		Класс для хранения информации в формате имя-значения.
*		Реализация основного класса(настройки) для хранения информации в формате имя-значения, а также концепции настроек
*	которая представляет из себя список настроек, доступ которым возможно по индексу и по имени.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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
		/// Класс для хранения информации в формате имя-значения
		/// </summary>
		/// <remarks>
		/// <para>
		/// Концепция настроек представляет собой удобной, простой и мощный механизм хранения пользовательских данных 
		/// в формате имя - значения. Данная подсистема полностью динамична, манипулирования настройками может происходить 
		/// как в режиме редактора, так и в режиме работы приложения, при необходимости настройки можно сохранить 
		/// в нужном формате (бинарный поток, формат данных XML) во внешнем хранилище
		/// </para>
		/// <para>
		/// Настройка обеспечивает сериализуемость данных на уровне Unity
		/// </para>
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CSettingItem : CVariant, IComparable<CSettingItem>, ICloneable, ICubeXDuplicate<CSettingItem>
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация настройки из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Настройка</returns>
			//---------------------------------------------------------------------------------------------------------
			public static new CSettingItem DeserializeFromString(String data)
			{
				//
				// TODO
				// 
				CSettingItem setting = new CSettingItem();
				return setting;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal String mName;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal Int32 mData;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal Int32 mID;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Наименование настройки
			/// </summary>
			/// <remarks>
			/// Имя настройки должно быть уникальных в пределах списка настройки
			/// </remarks>
			public String Name
			{
				get { return mName; }
				set 
				{
					mName = value;
					if (IOwned != null) IOwned.OnNotifyUpdated(this, nameof(Name));
				}
			}

			/// <summary>
			/// Активность настройки
			/// </summary>
			/// <remarks>
			/// Условная активность настройки - на усмотрение пользователя
			/// </remarks>
			public Boolean IsActive
			{
				get { return XPacked.UnpackBoolean(mData, 0); }
				set 
				{ 
					XPacked.PackBoolean(ref mData, 0, value);
					if (IOwned != null) IOwned.OnNotifyUpdated(this, nameof(IsActive));
				}
			}

			/// <summary>
			/// Видимость настройки
			/// </summary>
			/// <remarks>
			/// Условная видимость настройки - на усмотрение пользователя
			/// </remarks>
			public Boolean IsVisible
			{
				get { return XPacked.UnpackBoolean(mData, 1); }
				set 
				{
					XPacked.PackBoolean(ref mData, 1, value);
					if (IOwned != null) IOwned.OnNotifyUpdated(this, nameof(IsVisible));
				}
			}

			/// <summary>
			/// Пользовательский тэг данных
			/// </summary>
			/// <remarks>
			/// Условный пользовательский тэг данных - на усмотрение пользователя.
			/// Сохраняется последние 8 бит, диапазон возможных значений: 0-255
			/// </remarks>
			public Int32 UserTag
			{
				get { return XPacked.UnpackInteger(mData, 2, 8); }
				set 
				{
					XPacked.PackInteger(ref mData, 2, 8, value);
					if (IOwned != null) IOwned.OnNotifyUpdated(this, nameof(UserTag));
				}
			}

			/// <summary>
			/// Пользовательский тип данных
			/// </summary>
			/// <remarks>
			/// Условный пользовательский тип данных - на усмотрение пользователя.
			/// Сохраняется последние 16 бит, диапазон возможных значений: 0-65536
			/// </remarks>
			public Int32 UserData
			{
				get { return XPacked.UnpackInteger(mData, 10, 16); }
				set 
				{ 
					XPacked.PackInteger(ref mData, 10, 16, value);
					if (IOwned != null) IOwned.OnNotifyUpdated(this, nameof(UserData));
				}
			}

			/// <summary>
			/// Идентификатор настройки
			/// </summary>
			public Int32 ID
			{
				get { return mID; }
				set 
				{
					if (mID != value)
					{
						mID = value;
						if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ID));
					}
				}
			}

			/// <summary>
			/// Владелец настройки
			/// </summary>
			public CSettings Owned
			{
				get { return mIOwned as CSettings; }
				set { mIOwned = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem()
			{
				mName = "";
				mData = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem(String setting_name)
			{
				mName = setting_name;
				mData = 0;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="id">Идентификатор настройки</param>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem(Int32 id)
			{
				mName = "";
				mID = id;
				mData = 0;
			}
			#endregion
			
			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение настроек для упорядочивания
			/// </summary>
			/// <param name="other">Настройка</param>
			/// <returns>Статус сравнения настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CSettingItem other)
			{
				return (String.CompareOrdinal(Name, other.Name));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода настройки
			/// </summary>
			/// <returns>Хеш-код настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return (this.Name.GetHashCode() ^ mNumberData.GetHashCode() ^ mStringData.GetHashCode());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование настройки
			/// </summary>
			/// <returns>Копия объекта настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public new Object Clone()
			{
				return (MemberwiseClone());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дубликата объекта
			/// </summary>
			/// <returns>Дубликат объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem Duplicate()
			{
				return (MemberwiseClone() as CSettingItem);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String result = String.Format("{0} = {1}", mName, base.ToString());
				return (result);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация настройки в строку
			/// </summary>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String SerializeToString()
			{
				return String.Format("[{0}]", mValueType);
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Настройки
		/// </summary>
		/// <remarks>
		/// Настройки – представляют собой список настроек доступ которым возможно по индексу и по имени.
		/// Поиск соответствующей настройки - линейный.
		/// Дублирование имени настройки не допускается
		/// Список настроек обеспечивает сериализуемость данных на уровне Unity.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CSettings : IComparable<CSettings>, ICloneable, ICubeXNotify, ICubeXDuplicate<CSettings>,
			ICubeXNotifyCollectionChanged
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal String mName;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal List<CSettingItem> mSettings;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal Boolean mIsAutoCreate;

			[NonSerialized]
			internal ICubeXNotify mIOwned;

			// События
			[NonSerialized]
			protected internal Action<TNotifyCollectionChangedAction, Object, Int32, Int32> mOnCollectionChanged;

			// Служебные данные
#if (UNITY_EDITOR)
			[UnityEngine.SerializeField]
			public Boolean mExpanded;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя списка настроек
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Количество настроек
			/// </summary>
			public Int32 Count
			{
				get { return mSettings.Count; }
			}

			/// <summary>
			/// Список настроек
			/// </summary>
			public List<CSettingItem> Settings
			{
				get { return mSettings; }
			}

			/// <summary>
			/// Автоматическое создание настройки
			/// </summary>
			/// <remarks>
			/// Применяется при индексации на основе имени настройки.
			/// При отсутствии настройки с указанным имением настройка будет создана автоматически с параметрами по умолчанию
			/// </remarks>
			public Boolean IsAutoCreate
			{
				get { return mIsAutoCreate; }
				set { mIsAutoCreate = value; }
			}

			/// <summary>
			/// Владелец списка настроек
			/// </summary>
			public ICubeXNotify IOwned
			{
				get { return mIOwned; }
				set
				{
					mIOwned = value;
				}
			}

			/// <summary>
			/// Владелец списка настроек
			/// </summary>
			public CSettingObject Owned
			{
				get { return mIOwned as CSettingObject; }
				set
				{
					mIOwned = value;
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о любом изменении коллекции.
			/// Аргументы - Тип происходящего действия, элемент над которым производится действия, индекс элемента,
			/// количество элементов
			/// </summary>
			public Action<TNotifyCollectionChangedAction, Object, Int32, Int32> OnCollectionChanged
			{
				get { return mOnCollectionChanged; }
				set { mOnCollectionChanged = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CSettings()
			{
				mSettings = new List<CSettingItem>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="capacity">Начальная максимальная емкость списка</param>
			//---------------------------------------------------------------------------------------------------------
			public CSettings(Int32 capacity)
			{
				mSettings = new List<CSettingItem>(capacity);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="name">Имя списка настроек</param>
			//---------------------------------------------------------------------------------------------------------
			public CSettings(String name)
			{
				mName = name;
				mSettings = new List<CSettingItem>();
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение списка настроек для упорядочивания
			/// </summary>
			/// <param name="other">Список настроек</param>
			/// <returns>Статус сравнения списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CSettings other)
			{
				return (String.CompareOrdinal(Name, other.Name));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода списка настроек
			/// </summary>
			/// <returns>Хеш-код списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return (this.Name.GetHashCode());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование списка настроек
			/// </summary>
			/// <returns>Копия списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Object Clone()
			{
				CSettings settings = new CSettings();
				settings.Name = mName;
				settings.IsAutoCreate = mIsAutoCreate;

				for (Int32 i = 0; i < mSettings.Count; i++)
				{
					CSettingItem setting_item = mSettings[i].Duplicate();
					setting_item.IOwned = settings;
					settings.mSettings.Add(setting_item);
				}

				return (settings);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение дубликата объекта
			/// </summary>
			/// <returns>Дубликат объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettings Duplicate()
			{
				return (MemberwiseClone() as CSettings);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return (mName);
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация настроек на основе индекса
			/// </summary>
			/// <param name="index">Индекс настройки</param>
			/// <returns>Настройка</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem this[Int32 index]
			{
				get { return mSettings[index]; }
				set 
				{
					mSettings[index] = value;
					if (mOnCollectionChanged != null)
					{
						mOnCollectionChanged(TNotifyCollectionChangedAction.Replace, value, index, 1);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация настроек на основе имени
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <returns>Настройка</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem this[String setting_name]
			{
				get { return Get(setting_name); }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXNotify =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта о начале изменения данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого будут меняться</param>
			/// <param name="data_name">Имя данных</param>
			/// <returns>Статус разрешения/согласования изменения данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean OnNotifyUpdating(System.Object source, String data_name)
			{
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта об окончании изменении данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого изменились</param>
			/// <param name="data_name">Имя данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnNotifyUpdated(System.Object source, String data_name)
			{
				if (mIOwned != null) mIOwned.OnNotifyUpdated(source, data_name);

				if (mOnCollectionChanged != null)
				{
					Int32 index = mSettings.IndexOf(source as CSettingItem);
					mOnCollectionChanged(TNotifyCollectionChangedAction.ModifyItem, data_name, index, 1);
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование настройки с указанным именем
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <returns>Статус наличия настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Exists(String setting_name)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование настройки с указанным идентификатором настройки
			/// </summary>
			/// <param name="id">Идентификатор настройки</param>
			/// <returns>Статус наличия настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Exists(Int32 id)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].ID == id)
					{
						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование настройки с указанным именем и идентификатором настройки
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="id">Идентификатор настройки</param>
			/// <returns>Статус наличия настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Exists(String setting_name, Int32 id)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name && this[i].ID == id)
					{
						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индекс настройки с указанным именем
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <returns>Индекс настройки с указанными именем или -1 если настройка с таким именем не найдена</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 IndexOf(String setting_name)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return i;
					}
				}

				return -1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления настройки с указанным именем и значением
			/// </summary>
			/// <remarks>
			/// Если настройка с указанным именем существуют до данные не добавятся!!!
			/// </remarks>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="value">Значение настройки</param>
			/// <returns>Статус успешности добавления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Add(String setting_name, System.Object value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return false;
					}
				}

				CSettingItem setting = new CSettingItem(setting_name);
				setting.Set(value);

				mSettings.Add(setting);

				if(mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Add, setting, mSettings.Count - 1, 1);
				}

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления настройки с указанным именем и типом значением
			/// </summary>
			/// <remarks>
			/// Если настройка с указанным именем существуют до данные не добавятся!!!
			/// </remarks>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="value_type">Тип значения настройки</param>
			/// <returns>Статус успешности добавления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean AddFromValueType(String setting_name, TValueType value_type)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return false;
					}
				}

				CSettingItem setting = new CSettingItem(setting_name);
				setting.ValueType = value_type;

				mSettings.Add(setting);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Add, setting, mSettings.Count - 1, 1);
				}

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления настройки с указанным именем, идентификатором настройки и значением
			/// </summary>
			/// <remarks>
			/// Если настройка с указанным именем и идентификатором настройки существуют до данные не добавятся!!!
			/// </remarks>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="id">Идентификатор настройки</param>
			/// <param name="value">Значение настройки</param>
			/// <returns>Статус успешности добавления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Add(String setting_name, Int32 id, System.Object value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].ID == id && this[i].Name == setting_name)
					{
						return false;
					}
				}

				CSettingItem setting = new CSettingItem(setting_name);
				setting.ID = id;
				setting.Set(value);

				mSettings.Add(setting);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Add, setting, mSettings.Count - 1, 1);
				}

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров настройки по указанному имени
			/// </summary>
			/// <remarks>
			/// Если настройка с указанным именем не существуют до параметры не установятся
			/// </remarks>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="value">Значение настройки</param>
			/// <returns>Статус успешности установки параметров настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Set(String setting_name, System.Object value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						this[i].Set(value);
						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка параметров настройки по указанному имени и идентификатором настройки
			/// </summary>
			/// <remarks>
			/// Если настройка с указанным именем не существуют до параметры не установятся
			/// </remarks>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="id">Идентификатор настройки</param>
			/// <param name="value">Значение настройки</param>
			/// <returns>Статус успешности установки параметров настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Set(String setting_name, Int32 id, System.Object value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].ID == id && this[i].Name == setting_name)
					{
						this[i].Set(value);
						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления/обновление настройки с указанным именем
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="value">Значение настройки</param>
			/// <returns>Статус успешности добавление или обновления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Update(String setting_name, System.Object value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						this[i].Set(value);
						return true;
					}
				}

				CSettingItem setting = new CSettingItem(setting_name);
				setting.Set(value);

				mSettings.Add(setting);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Add, setting, mSettings.Count - 1, 1);
				}

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления/обновление настройки с указанным именем и идентификатором настройки
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="id">Идентификатор настройки</param>
			/// <param name="value">Значение настройки</param>
			/// <returns>Статус успешности добавление или обновления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Update(String setting_name, Int32 id, System.Object value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].ID == id && this[i].Name == setting_name)
					{
						this[i].Set(value);
						return true;
					}
				}

				CSettingItem setting = new CSettingItem(setting_name);
				setting.ID = id;
				setting.Set(value);

				mSettings.Add(setting);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Add, setting, mSettings.Count - 1, 1);
				}

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение настройки по указанному имени
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <returns>Найденная настройка или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem Get(String setting_name)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return this[i];
					}
				}

				if(IsAutoCreate)
				{
					CSettingItem setting = new CSettingItem(setting_name);

					mSettings.Add(setting);
					return setting;
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение настройки по указанному имени и идентификатору настройки
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="id">Идентификатор настройки</param>
			/// <returns>Найденная настройка или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettingItem Get(String setting_name, Int32 id)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].ID == id && this[i].Name == setting_name)
					{
						return this[i];
					}
				}

				if (IsAutoCreate)
				{
					CSettingItem setting = new CSettingItem(setting_name);
					setting.ID = id;

					mSettings.Add(setting);
					return setting;
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения настройки по указанному имени
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <returns>Найденное значение или значение по умолчанию</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object GetValue(String setting_name)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return (this[i].Get());
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения настройки по указанному имени
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="default_value">Значение по умолчанию</param>
			/// <returns>Найденное значение или значение по умолчанию</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object GetValue(String setting_name, System.Object default_value)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						return (this[i].Get());
					}
				}

				return default_value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения настройки по указанному имени и идентификатору настройки
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <param name="id">Идентификатор настройки</param>
			/// <returns>Найденное значение или значение по умолчанию</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object GetValueFromID(String setting_name, Int32 id)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].ID == id && this[i].Name == setting_name)
					{
						return (this[i].Get());
					}
				}

				return (null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления настройки с указанным именем
			/// </summary>
			/// <param name="setting_name">Имя настройки</param>
			/// <returns>Статус успешности удаления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Remove(String setting_name)
			{
				for (Int32 i = 0; i < Count; i++)
				{
					if (this[i].Name == setting_name)
					{
						mSettings.RemoveAt(i);

						if (mOnCollectionChanged != null)
						{
							mOnCollectionChanged(TNotifyCollectionChangedAction.Remove, null, i, 1);
						}

						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления настройки по указанному индексу
			/// </summary>
			/// <param name="index">Индекс настройки</param>
			/// <returns>Статус успешности удаления настройки</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean RemoveAt(Int32 index)
			{
				mSettings.RemoveAt(index);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Remove, null, index, 1);
				}

				return true;
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс представляющий собой объект который обеспечивает хранение и управления списками настроек
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CSettingObject : ICubeXNotify, ICubeXNotifyCollectionChanged
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal List<CSettings> mListSettings;

			// События
			[NonSerialized]
			protected internal Action<TNotifyCollectionChangedAction, Object, Int32, Int32> mOnCollectionChanged;

			// Служебные данные
#if (UNITY_EDITOR)
			[UnityEngine.SerializeField]
			public Boolean mExpanded;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Количество списков настроек
			/// </summary>
			public Int32 Count
			{
				get { return mListSettings.Count; }
			}

			/// <summary>
			/// Список настроек
			/// </summary>
			public List<CSettings> Settings
			{
				get { return mListSettings; }
			}

			/// <summary>
			/// Группа настроек по умолчанию
			/// </summary>
			public CSettings Default
			{
				get
				{
					if(mListSettings.Count == 0)
					{
						mListSettings.Add(new CSettings(nameof(Default)));
					}

					return mListSettings[0];
				}
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о любом изменении коллекции.
			/// Аргументы - Тип происходящего действия, элемент над которым производится действия, индекс элемента,
			/// количество элементов
			/// </summary>
			public Action<TNotifyCollectionChangedAction, Object, Int32, Int32> OnCollectionChanged
			{
				get { return mOnCollectionChanged; }
				set { mOnCollectionChanged = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CSettingObject()
			{
				mListSettings = new List<CSettings>();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="capacity">Начальная максимальная емкость списка</param>
			//---------------------------------------------------------------------------------------------------------
			public CSettingObject(Int32 capacity)
			{
				mListSettings = new List<CSettings>(capacity);
			}
			#endregion

			#region ======================================= ИНДЕКСАТОР ================================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация списка настроек на основе индекса
			/// </summary>
			/// <param name="index">Индекс списка настроек</param>
			/// <returns>Список настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettings this[Int32 index]
			{
				get { return mListSettings[index]; }
				set { mListSettings[index] = value; }
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индексация списка настроек на основе имени
			/// </summary>
			/// <param name="settings_name">Имя списка настроек</param>
			/// <returns>Список настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettings this[String settings_name]
			{
				get { return Get(settings_name); }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXNotify =======================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта о начале изменения данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого будут меняться</param>
			/// <param name="data_name">Имя данных</param>
			/// <returns>Статус разрешения/согласования изменения данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean OnNotifyUpdating(System.Object source, String data_name)
			{
				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Информирование данного объекта об окончании изменении данных указанного объекта
			/// </summary>
			/// <param name="source">Объект данные которого изменились</param>
			/// <param name="data_name">Имя данных</param>
			//---------------------------------------------------------------------------------------------------------
			public void OnNotifyUpdated(System.Object source, String data_name)
			{
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавления списка настроек с указанным именем
			/// </summary>
			/// <param name="settings_name">Имя списка настроек</param>
			//---------------------------------------------------------------------------------------------------------
			public void Add(String settings_name)
			{
				CSettings settings = new CSettings(settings_name);
				mListSettings.Add(settings);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Add, settings, mListSettings.Count - 1, 1);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка на существование списка настроек с указанным именем
			/// </summary>
			/// <param name="settings_name">Имя списка настроек</param>
			/// <returns>Статус наличия списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Exists(String settings_name)
			{
				for (Int32 i = 0; i < mListSettings.Count; i++)
				{
					if (mListSettings[i].Name == settings_name)
					{
						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Индекс списка настроек с указанным именем
			/// </summary>
			/// <param name="settings_name">Имя списка настроек</param>
			/// <returns>Индекс списка настроек с указанными именем или -1 если настройка с таким именем не найдена</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 IndexOf(String settings_name)
			{
				for (Int32 i = 0; i < mListSettings.Count; i++)
				{
					if (mListSettings[i].Name == settings_name)
					{
						return i;
					}
				}

				return -1;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение списка настроек по указанному имени
			/// </summary>
			/// <param name="settings_name">Имя списка настроек</param>
			/// <returns>Найденный список настроек или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public CSettings Get(String settings_name)
			{
				for (Int32 i = 0; i < mListSettings.Count; i++)
				{
					if (mListSettings[i].Name == settings_name)
					{
						return mListSettings[i];
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления списка настроек с указанным именем
			/// </summary>
			/// <param name="settings_name">Имя списка настроек</param>
			/// <returns>Статус успешности удаления списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Remove(String settings_name)
			{
				for (Int32 i = 0; i < mListSettings.Count; i++)
				{
					if (mListSettings[i].Name == settings_name)
					{
						mListSettings.RemoveAt(i);

						if (mOnCollectionChanged != null)
						{
							mOnCollectionChanged(TNotifyCollectionChangedAction.Remove, null, i, 1);
						}

						return true;
					}
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Удаления списка настроек по указанному индексу
			/// </summary>
			/// <param name="index">Индекс списка настроек</param>
			/// <returns>Статус успешности удаления списка настроек</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean Remove(Int32 index)
			{
				mListSettings.RemoveAt(index);

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Remove, null, index, 1);
				}

				return true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка всех списков настроек
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mListSettings.Clear();

				if (mOnCollectionChanged != null)
				{
					mOnCollectionChanged(TNotifyCollectionChangedAction.Clear, null, 0, 0);
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