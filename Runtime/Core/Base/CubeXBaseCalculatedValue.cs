//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseCalculatedValue.cs
*		Калькулированное значение.
*		Класс содержащий значение и дополнительные свойства для его управления.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
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
		/// Класс содержащий целое значение и дополнительные свойства для его управления
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CIntCalculated : PropertyChangedBase, ICloneable, ICubeXNotCalculation
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация объекта из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Объект</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CIntCalculated DeserializeFromString(String data)
			{
				Int32 int_value = XNumbers.ParseInt(data, 0);
				return (new CIntCalculated(int_value));
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal Int32 mValue;
			internal Int32 mSupplement;
			internal Boolean mNotCalculation;
			internal ICubeXNotify mIOwned;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Базовое значение
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public Int32 Value
			{
				get
				{
					return (mValue);
				}
				set
				{
					mValue = value;
					if(mIOwned != null)
					{
						mIOwned.OnNotifyUpdated(this, nameof(Value));
					}

					NotifyPropertyChanged(nameof(Value));
					NotifyPropertyChanged(nameof(CalculatedValue));
				}
			}

			/// <summary>
			/// Смещение базового значения
			/// </summary>
			[Browsable(false)]
			[XmlAttribute]
			public Int32 Supplement
			{
				get
				{
					return (mSupplement);
				}
				set
				{
					mSupplement = value;
					if (mIOwned != null)
					{
						mIOwned.OnNotifyUpdated(this, nameof(Supplement));
					}

					NotifyPropertyChanged(nameof(Supplement));
					NotifyPropertyChanged(nameof(CalculatedValue));
				}
			}

			/// <summary>
			/// Вычисленное значение
			/// </summary>
			[Browsable(false)]
			[XmlIgnore]
			public Int32 CalculatedValue
			{
				get
				{
					if (mNotCalculation)
					{
						return (0);
					}
					else
					{
						return (mValue + mSupplement);
					}
				}
				set
				{
					if (mNotCalculation != true)
					{
						mValue = value - mSupplement;
						if (mIOwned != null)
						{
							mIOwned.OnNotifyUpdated(this, nameof(CalculatedValue));
						}

						NotifyPropertyChanged(nameof(Value));
						NotifyPropertyChanged(nameof(CalculatedValue));
					}
				}
			}

			/// <summary>
			/// Владелец значения
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

					if (mIOwned != null)
					{
						mIOwned.OnNotifyUpdated(this, nameof(NotCalculation));
					}

					NotifyPropertyChanged(nameof(CalculatedValue));
					NotifyPropertyChanged(nameof(NotCalculation));
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="value">Значение</param>
			//---------------------------------------------------------------------------------------------------------
			public CIntCalculated(Int32 value)
			{
				mValue = value;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="value">Значение</param>
			/// <param name="owned">Владелец значения</param>
			//---------------------------------------------------------------------------------------------------------
			public CIntCalculated(Int32 value, ICubeXNotify owned)
			{
				mValue = value;
				mIOwned = owned;
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование объекта
			/// </summary>
			/// <returns>Копия объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление объекта</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				return mValue.ToString();
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================