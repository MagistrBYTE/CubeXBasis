﻿//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Базовая подсистема
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXBaseVariant.cs
*		Универсальный тип данных.
*		Реализация универсального типа данных с поддержкой сериализации на уровне Unity. Данный тип хранит один из возможных
*	предопределённых типов данных.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
//---------------------------------------------------------------------------------------------------------------------
#if !(UNITY_2017_1_OR_NEWER)
using CubeX.Maths;
#endif
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
		/// Определение допустимых типов значений
		/// </summary>
		/// <remarks>
		/// Определение стандартных типов данных значения в контексте использования универсального типа.
		/// Указанные типы данных имеют прямую поддержку в подсистеме сообщений.
		/// Также они охватываю более 90% функциональности которая требуется в проектах с поддержкой универсального типа.
		/// Для Unity определенна поддержка игровых объектов и наиболее распространённых ресурсов
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		public enum TValueType
		{
			/// <summary>
			/// Отсутствие определенного значения
			/// </summary>
			Void,

			/// <summary>
			/// Логический тип
			/// </summary>
			Boolean,

			/// <summary>
			/// Целый тип
			/// </summary>
			Integer,

			/// <summary>
			/// Перечисление
			/// </summary>
			Enum,

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
			Rect,

#if (UNITY_2017_1_OR_NEWER)
			/// <summary>
			/// Игровой объект
			/// </summary>
			GameObject,

			/// <summary>
			/// Ресурс Unity - двухмерная текстура
			/// </summary>
			Texture2D,

			/// <summary>
			/// Ресурс Unity - спрайт
			/// </summary>
			Sprite,

			/// <summary>
			/// Ресурс Unity - трехмерная модель
			/// </summary>
			Model,

			/// <summary>
			/// Ресурс Unity - текстовые или бинарные данные
			/// </summary>
			TextAsset,
#endif
			/// <summary>
			/// Базовый объект
			/// </summary>
			SysObject
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Универсальный тип данных
		/// </summary>
		/// <remarks>
		/// Реализация универсального типа данных с поддержкой сериализации на уровне Unity.
		/// Данный тип хранит один из возможных предопределённых типов данных.
		/// Для однозначной идентификации объектов которые не могут быть сведены к стандартному типу в поле <see cref="CVariant.StringValue"/>
		/// храниться контекстная информация об объекте (его тип, ссылка и т.д.).
		/// При установке владельца интерфейса для уведомлений, объект уведомляет об изменении своих свойств.
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CVariant : IComparable<CVariant>, ICloneable
		{
			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Десереализация универсального типа данных из строки
			/// </summary>
			/// <param name="data">Строка данных</param>
			/// <returns>Универсальный тип данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public static CVariant DeserializeFromString(String data)
			{
				//
				// TODO
				// 
				CVariant variant = new CVariant();
				return variant;
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal protected TValueType mValueType;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal protected String mStringData;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
			internal protected UnityEngine.Vector4 mNumberData;
#else
			internal Maths.Vector4D mNumberData;
#endif
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			internal protected System.Object mReferenceData;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
			internal protected UnityEngine.Object mUnityData;
#endif

			[NonSerialized]
			internal protected ICubeXNotify mIOwned;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Тип данных значения
			/// </summary>
			public TValueType ValueType
			{
				get { return mValueType; }
				set 
				{ 
					mValueType = value;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ValueType));
				}
			}

			/// <summary>
			/// Логический тип
			/// </summary>
			public Boolean BooleanValue
			{
#if (UNITY_2017_1_OR_NEWER)
				get { return mNumberData.x == 1; }
				set
				{
					if (value)
					{
						mNumberData.x = 1;
					}
					else
					{
						mNumberData.x = 0;
					}

					mValueType = TValueType.Boolean;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(BooleanValue));
				}
#else
				get { return (mNumberData.X == 1); }
				set
				{
					if (value)
					{
						mNumberData.X = 1;
					}
					else
					{
						mNumberData.X = 0;
					}

					mValueType = TValueType.Boolean;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(BooleanValue));
				}
#endif
			}

			/// <summary>
			/// Целый тип
			/// </summary>
			public Int32 IntegerValue
			{
#if (UNITY_2017_1_OR_NEWER)
				get { return (Int32)mNumberData.x; }
				set
				{
					mNumberData.x = value;
					mValueType = TValueType.Integer;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(IntegerValue));
				}
#else
				get { return ((Int32)mNumberData.X); }
				set
				{
					mNumberData.X = value;
					mValueType = TValueType.Integer;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(IntegerValue));
				}
#endif
			}

			/// <summary>
			/// Перечисление
			/// </summary>
			/// <remarks>
			/// Имя реального типа сохраняется в поле <see cref="mStringData"/>
			/// </remarks>
			public Enum EnumValue
			{
				get
				{
					if (mReferenceData != null && mReferenceData.GetType().IsEnum)
					{
						return ((Enum)mReferenceData);
					}
					else
					{
						return (null);
					}
				}
				set
				{
					mReferenceData = value;
					mValueType = TValueType.Enum;
					if(mReferenceData != null)
					{
						mStringData = mReferenceData.GetType().Name;
					}
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(EnumValue));
				}
			}

			/// <summary>
			/// Вещественный тип
			/// </summary>
			public Single RealValue
			{
#if (UNITY_2017_1_OR_NEWER)
				get { return mNumberData.x; }
				set
				{
					mNumberData.x = value;
					mValueType = TValueType.Real;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(RealValue));
				}
#else
				get { return ((Single)mNumberData.X); }
				set
				{
					mNumberData.X = value;
					mValueType = TValueType.Real;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(RealValue));
				}
#endif
			}

			/// <summary>
			/// Тип даты-времени
			/// </summary>
			public DateTime DateTimeValue
			{
				get { return DateTime.Parse(mStringData); }
				set
				{
					mStringData = value.ToString();
					mValueType = TValueType.DateTime;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(DateTimeValue));
				}
			}

			/// <summary>
			/// Строковый тип
			/// </summary>
			public String StringValue
			{
				get { return mStringData; }
				set
				{
					mStringData = value;
					mValueType = TValueType.String;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(StringValue));
				}
			}

#if (UNITY_2017_1_OR_NEWER)
			/// <summary>
			/// Двухмерный объект данных
			/// </summary>
			public UnityEngine.Vector2 Vector2DValue
			{
				get { return mNumberData; }
				set
				{
					mNumberData = value;
					mValueType = TValueType.Vector2D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector2DValue));
				}
			}

			/// <summary>
			/// Трехмерный объект данных
			/// </summary>
			public UnityEngine.Vector3 Vector3DValue
			{
				get { return mNumberData; }
				set
				{
					mNumberData = value;
					mValueType = TValueType.Vector3D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector3DValue));
				}
			}

			/// <summary>
			/// Четырехмерный объект данных
			/// </summary>
			public UnityEngine.Vector4 Vector4DValue
			{
				get { return mNumberData; }
				set
				{
					mNumberData = value;
					mValueType = TValueType.Vector4D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector4DValue));
				}
			}

			/// <summary>
			/// Цвет
			/// </summary>
			public UnityEngine.Color ColorValue
			{
				get { return new UnityEngine.Color(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w); }
				set
				{
					mNumberData.x = value.r;
					mNumberData.y = value.g;
					mNumberData.z = value.b;
					mNumberData.w = value.a;
					mValueType = TValueType.Color;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ColorValue));
				}
			}

			/// <summary>
			/// Прямоугольник
			/// </summary>
			public UnityEngine.Rect RectValue
			{
				get { return new UnityEngine.Rect(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w); }
				set
				{
					mNumberData.x = value.x;
					mNumberData.y = value.y;
					mNumberData.z = value.width;
					mNumberData.w = value.height;
					mValueType = TValueType.Rect;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(RectValue));
				}
			}
#else
			/// <summary>
			/// Двухмерный объект данных
			/// </summary>
			public Maths.Vector2D Vector2DValue
			{
				get { return (new Maths.Vector2D(mNumberData.X, mNumberData.Y)); }
				set
				{
					mNumberData.X = value.X;
					mNumberData.Y = value.Y;
					mValueType = TValueType.Vector2D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector2DValue));
				}
			}

			/// <summary>
			/// Трехмерный объект данных
			/// </summary>
			public Maths.Vector3D Vector3DValue
			{
				get { return (new Maths.Vector3D(mNumberData.X, mNumberData.Y, mNumberData.Z)); }
				set
				{
					mNumberData.X = value.X;
					mNumberData.Y = value.Y;
					mNumberData.Z = value.Z;
					mValueType = TValueType.Vector3D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector3DValue));
				}
			}

			/// <summary>
			/// Четырехмерный объект данных
			/// </summary>
			public Maths.Vector4D Vector4DValue
			{
				get { return (mNumberData); }
				set
				{
					mNumberData = value;
					mValueType = TValueType.Vector4D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector4DValue));
				}
			}

			/// <summary>
			/// Цвет
			/// </summary>
			public TColor ColorValue
			{
				get { return (new TColor((Byte)mNumberData.X, (Byte)mNumberData.Y, (Byte)mNumberData.Z, (Byte)mNumberData.W)); }
				set
				{
					mNumberData.X = value.R;
					mNumberData.Y = value.G;
					mNumberData.Z = value.B;
					mNumberData.W = value.A;
					mValueType = TValueType.Color;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ColorValue));
				}
			}

			/// <summary>
			/// Прямоугольник
			/// </summary>
			public Maths.Rect2D RectValue
			{
				get { return (new Maths.Rect2D(mNumberData.X, mNumberData.Y, mNumberData.Z, mNumberData.W)); }
				set
				{
					mNumberData.X = value.X;
					mNumberData.Y = value.Y;
					mNumberData.Z = value.Width;
					mNumberData.W = value.Height;
					mValueType = TValueType.Rect;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(RectValue));
				}
			}
#endif
			/// <summary>
			/// Базовый объект
			/// </summary>
			/// <remarks>
			/// Имя реального типа объекта сохраняется в поле <see cref="mStringData"/>
			/// </remarks>
			public System.Object SysObject
			{
				get { return mReferenceData; }
				set
				{
					mReferenceData = value;
					mValueType = TValueType.SysObject;
					if (mReferenceData != null)
					{
						mStringData = mReferenceData.GetType().Name;
					}

					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(SysObject));
				}
			}

#if (UNITY_2017_1_OR_NEWER)
			/// <summary>
			/// Игровой объект
			/// </summary>
			public UnityEngine.GameObject GameObjectValue
			{
				get 
				{ 
					return mUnityData as UnityEngine.GameObject; 
				}
				set
				{
					mUnityData = value;
					mValueType = TValueType.GameObject;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(GameObjectValue));
				}
			}

			/// <summary>
			/// Ресурс Unity -двухмерная текстура
			/// </summary>
			public UnityEngine.Texture2D Texture2DValue
			{
				get
				{
					return mUnityData as UnityEngine.Texture2D;
				}
				set
				{
					mUnityData = value;
					mValueType = TValueType.Texture2D;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Texture2DValue));
				}
			}

			/// <summary>
			/// Ресурс Unity - спрайт
			/// </summary>
			public UnityEngine.Sprite SpriteValue
			{
				get
				{
					return mUnityData as UnityEngine.Sprite;
				}
				set
				{
					mUnityData = value;
					mValueType = TValueType.Sprite;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(SpriteValue));
				}
			}

			/// <summary>
			/// Ресурс Unity - трехмерная модель
			/// </summary>
			public UnityEngine.GameObject ModelValue
			{
				get
				{
					return mUnityData as UnityEngine.GameObject;
				}
				set
				{
					mUnityData = value;
					mValueType = TValueType.Model;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ModelValue));
				}
			}

			/// <summary>
			/// Ресурс Unity - текстовые или бинарные данные
			/// </summary>
			public UnityEngine.TextAsset TextAssetValue
			{
				get
				{
					return mUnityData as UnityEngine.TextAsset;
				}
				set
				{
					mUnityData = value;
					mValueType = TValueType.TextAsset;
					if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(TextAssetValue));
				}
			}
#endif
			/// <summary>
			/// Владелец
			/// </summary>
			public ICubeXNotify IOwned
			{
				get { return mIOwned; }
				set 
				{ 
					mIOwned = value; 
				}
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные универсального типа нулевыми значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CVariant()
			{
				mValueType = TValueType.Void;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует данные универсального типа указанным объектом
			/// </summary>
			/// <remarks>
			/// Тип объекта выводится автоматически
			/// </remarks>
			/// <param name="obj">Объект</param>
			//---------------------------------------------------------------------------------------------------------
			public CVariant(System.Object obj)
			{
				Set(obj);
			}
			#endregion
		
			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение универсальных типов данных для упорядочивания
			/// </summary>
			/// <param name="other">Объект универсального типа данных</param>
			/// <returns>Статус сравнения</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CVariant other)
			{
				Int32 result = 0;
 				switch (mValueType)
				{
					case TValueType.Void:
						break;
					case TValueType.Boolean:
						break;
					case TValueType.Integer:
						{
							result = Comparer<Int32>.Default.Compare(IntegerValue, other.IntegerValue);
						}
						break;
					case TValueType.Enum:
						break;
					case TValueType.Real:
						{
							result = Comparer<Single>.Default.Compare(RealValue, other.RealValue);
						}
						break;
					case TValueType.DateTime:
						{
							result = Comparer<DateTime>.Default.Compare(DateTimeValue, other.DateTimeValue);
						}
						break;
					case TValueType.String:
						{
							result = String.CompareOrdinal(StringValue, other.StringValue);
						}
						break;
					case TValueType.Color:
						{
#if (UNITY_2017_1_OR_NEWER)
							result = Comparer<UnityEngine.Color>.Default.Compare(ColorValue, other.ColorValue);
#else
							result = Comparer<TColor>.Default.Compare(ColorValue, other.ColorValue);
#endif
						}
						break;
					case TValueType.Vector2D:
						break;
					case TValueType.Vector3D:
						break;
					case TValueType.Vector4D:
						break;
					case TValueType.Rect:
						break;
#if (UNITY_2017_1_OR_NEWER)
					case TValueType.GameObject:
					case TValueType.Texture2D:
					case TValueType.Sprite:
					case TValueType.Model:
					case TValueType.TextAsset:
						{
							// Сравниваем по имени
							UnityEngine.Object unity_object_this = mUnityData;
							UnityEngine.Object unity_object_other = other.mUnityData;
							if(unity_object_this != null && unity_object_other != null)
							{
								result = String.CompareOrdinal(unity_object_this.name, unity_object_other.name);
							}
						}
						break;
#endif
					case TValueType.SysObject:
						break;

					default:
						break;
				}

				return(result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение хеш-кода объекта универсального типа данных
			/// </summary>
			/// <returns>Хеш-код</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Int32 GetHashCode()
			{
				return (this.GetHashCode() ^ mNumberData.GetHashCode() ^ mStringData.GetHashCode());
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Полное копирование объекта универсального типа данных
			/// </summary>
			/// <returns>Копия объекта универсального типа данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object Clone()
			{
				return MemberwiseClone();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String result = "";
				switch (mValueType)
				{
					case TValueType.Void:
						{
							result = nameof(TValueType.Void);
						}
						break;
					case TValueType.Boolean:
						{
							result = BooleanValue.ToString();
						}
						break;
					case TValueType.Integer:
						{
							result = IntegerValue.ToString();
						}
						break;
					case TValueType.Enum:
						break;
					case TValueType.Real:
						{
							result = RealValue.ToString();
						}
						break;
					case TValueType.DateTime:
						break;
					case TValueType.String:
						{
							result = StringValue;
						}
						break;
					case TValueType.Color:
						{
							result = ColorValue.ToString();
						}
						break;
					case TValueType.Vector2D:
						{
							result = Vector2DValue.ToString();
						}
						break;
					case TValueType.Vector3D:
						{
							result = Vector3DValue.ToString();
						}
						break;
					case TValueType.Vector4D:
						{
							result = Vector4DValue.ToString();
						}
						break;
					case TValueType.Rect:
						{
							result = RectValue.ToString();
						}
						break;
#if (UNITY_2017_1_OR_NEWER)
					case TValueType.GameObject:
					case TValueType.Texture2D:
					case TValueType.Sprite:
					case TValueType.Model:
					case TValueType.TextAsset:
						{
							result = mUnityData?.ToString();
						}
						break;
#endif
					case TValueType.SysObject:
						{
							result = mReferenceData.ToString();
						}
						break;

					default:
						break;
				}

				return (result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <param name="format">Формат отображения</param>
			/// <returns>Текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToString(String format)
			{
				String result = "";
				switch (mValueType)
				{
					case TValueType.Void:
						{
							result = nameof(TValueType.Void);
						}
						break;
					case TValueType.Boolean:
						{
							result = BooleanValue.ToString();
						}
						break;
					case TValueType.Integer:
						{
							result = IntegerValue.ToString(format);
						}
						break;
					case TValueType.Enum:
						break;
					case TValueType.Real:
						{
							result = RealValue.ToString(format);
						}
						break;
					case TValueType.DateTime:
						break;
					case TValueType.String:
						{
							result = StringValue;
						}
						break;
					case TValueType.Color:
						{
							result = ColorValue.ToString();
						}
						break;
					case TValueType.Vector2D:
						{
							result = Vector2DValue.ToString(format);
						}
						break;
					case TValueType.Vector3D:
						{
							result = Vector3DValue.ToString(format);
						}
						break;
					case TValueType.Vector4D:
						{
							result = Vector4DValue.ToString(format);
						}
						break;
					case TValueType.Rect:
						{
							result = RectValue.ToString(format);
						}
						break;
#if (UNITY_2017_1_OR_NEWER)
					case TValueType.GameObject:
						{
							result = GameObjectValue.ToString();
						}
						break;
					case TValueType.Texture2D:
						{
							result = Texture2DValue.ToString();
						}
						break;
					case TValueType.Sprite:
						{
							result = SpriteValue.ToString();
						}
						break;
					case TValueType.Model:
						{
							result = ModelValue.ToString();
						}
						break;
					case TValueType.TextAsset:
						{
							result = TextAssetValue.ToString();
						}
						break;
#endif
					case TValueType.SysObject:
						{
							result = mReferenceData.ToString();
						}
						break;
					default:
						break;
				}

				return (result);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка значения универсального типа данных
			/// </summary>
			/// <param name="value">Значение</param>
			//---------------------------------------------------------------------------------------------------------
			public void Set(System.Object value)
			{
				if (value == null)
				{
					mValueType = TValueType.Void;
					return;
				}

				// Получаем тип
				Type type = value.GetType();
				switch (type.Name)
				{
					case nameof(Boolean):
						{
							Boolean v = (Boolean)value;
#if (UNITY_2017_1_OR_NEWER)
							if (v)
							{
								mNumberData.x = 1;
							}
							else
							{
								mNumberData.x = 0;
							}
#else

							if (v)
							{
								mNumberData.X = 1;
							}
							else
							{
								mNumberData.X = 0;
							}
#endif
							mValueType = TValueType.Boolean;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(BooleanValue));
						}
						break;
					case nameof(Int32):
						{
#if (UNITY_2017_1_OR_NEWER)
							mNumberData.x = (Int32)value;
#else
							mNumberData.X = (Int32)value;
#endif
							mValueType = TValueType.Integer;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(IntegerValue));
						}
						break;
					case nameof(Single):
						{
#if (UNITY_2017_1_OR_NEWER)
							mNumberData.x = (Single)value;
#else
							mNumberData.X = (Single)value;
#endif
							mValueType = TValueType.Real;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(RealValue));
						}
						break;
					case nameof(DateTime):
						{
							DateTime v = (DateTime)value;
							mStringData = v.ToString();
							mValueType = TValueType.DateTime;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(DateTimeValue));
						}
						break;
					case nameof(String):
						{
							mStringData = value.ToString();
							mValueType = TValueType.String;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(StringValue));
						}
						break;
#if (UNITY_2017_1_OR_NEWER)
					case nameof(UnityEngine.Color):
						{
							UnityEngine.Color v = (UnityEngine.Color)value;
							mNumberData.x = v.r;
							mNumberData.y = v.g;
							mNumberData.z = v.b;
							mNumberData.w = v.a;
							mValueType = TValueType.Color;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ColorValue));
						}
						break;
					case nameof(UnityEngine.Vector2):
						{
							UnityEngine.Vector2 v = (UnityEngine.Vector2)value;
							mNumberData = v;
							mValueType = TValueType.Vector2D;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector2DValue));
						}
						break;
					case nameof(UnityEngine.Vector3):
						{
							UnityEngine.Vector3 v = (UnityEngine.Vector3)value;
							mNumberData = v;
							mValueType = TValueType.Vector3D;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector3DValue));
						}
						break;
					case nameof(UnityEngine.Vector4):
						{
							UnityEngine.Vector4 v = (UnityEngine.Vector4)value;
							mNumberData = v;
							mValueType = TValueType.Vector4D;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Vector4DValue));
						}
						break;
					case nameof(UnityEngine.Rect):
						{
							UnityEngine.Rect v = (UnityEngine.Rect)value;
							mNumberData.x = v.x;
							mNumberData.y = v.y;
							mNumberData.z = v.width;
							mNumberData.w = v.height;
							mValueType = TValueType.Rect;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(RectValue));
						}
						break;
#endif
					default:
						{
							// Проверка на перечисление
							if (type.IsEnum)
							{
								mReferenceData = value;
								mStringData = type.Name;
								if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(EnumValue));
								break;
							}

#if (UNITY_2017_1_OR_NEWER)
							// Проверка на тип Unity
							if (type == typeof(UnityEngine.GameObject))
							{
								UnityEngine.GameObject game_object = value as UnityEngine.GameObject;
								mUnityData = value as UnityEngine.GameObject;
								if (game_object.scene.name == null)
								{
									mValueType = TValueType.Model;
									if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ModelValue));
								}
								else
								{
									mValueType = TValueType.GameObject;
									if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(GameObjectValue));
								}
								break;
							}

							// Проверка на тип Texture2D
							if (type == typeof(UnityEngine.Texture2D))
							{
								mUnityData = value as UnityEngine.Texture2D;
								mValueType = TValueType.Texture2D;
								if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(Texture2DValue));
								break;
							}

							// Проверка на тип Sprite
							if (type == typeof(UnityEngine.Sprite))
							{
								mUnityData = value as UnityEngine.Sprite;
								mValueType = TValueType.Sprite;
								if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(SpriteValue));
								break;
							}

							// Проверка на тип TextAsset
							if (type == typeof(UnityEngine.TextAsset))
							{
								mUnityData = value as UnityEngine.TextAsset;
								mValueType = TValueType.TextAsset;
								if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(TextAssetValue));
								break;
							}
#endif
							mReferenceData = value;
							mValueType = TValueType.SysObject;
							mStringData = type.Name;
							if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(SysObject));

						}
						break;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка типа универсального типа данных
			/// </summary>
			/// <param name="type">Тип</param>
			//---------------------------------------------------------------------------------------------------------
			public void Set(Type type)
			{
				switch (type.Name)
				{
					case nameof(Boolean):
						{
							mValueType = TValueType.Boolean;
						}
						break;
					case nameof(Int32):
						{
							mValueType = TValueType.Integer;
						}
						break;
					case nameof(Single):
						{
							mValueType = TValueType.Real;
						}
						break;
					case nameof(DateTime):
						{
							mValueType = TValueType.DateTime;
						}
						break;
					case nameof(String):
						{
							mValueType = TValueType.String;
						}
						break;
#if (UNITY_2017_1_OR_NEWER)
					case nameof(UnityEngine.Color):
						{
							mValueType = TValueType.Color;
						}
						break;
					case nameof(UnityEngine.Vector2):
						{
							mValueType = TValueType.Vector2D;
						}
						break;
					case nameof(UnityEngine.Vector3):
						{
							mValueType = TValueType.Vector3D;
						}
						break;
					case nameof(UnityEngine.Vector4):
						{
							mValueType = TValueType.Vector4D;
						}
						break;
					case nameof(UnityEngine.Rect):
						{
							mValueType = TValueType.Rect;
						}
						break;
#endif
					default:
						{
							// Проверка на перечисление
							if (type.IsEnum)
							{
								mValueType = TValueType.Enum;
								break;
							}

#if (UNITY_2017_1_OR_NEWER)

							// Проверка на тип Unity
							if (type == typeof(UnityEngine.GameObject))
							{
								mValueType = TValueType.GameObject;
								break;
							}

							// Проверка на тип Texture2D
							if (type == typeof(UnityEngine.Texture2D))
							{
								mValueType = TValueType.Texture2D;
								break;
							}

							// Проверка на тип Sprite
							if (type == typeof(UnityEngine.Sprite))
							{
								mValueType = TValueType.Sprite;
								break;
							}

							// Проверка на тип TextAsset
							if (type == typeof(UnityEngine.TextAsset))
							{
								mValueType = TValueType.TextAsset;
								break;
							}
#endif
							mValueType = TValueType.SysObject;
							mStringData = type.Name;

						}
						break;
				}

				if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ValueType));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значение соответствующего типа
			/// </summary>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public System.Object Get()
			{
				switch (mValueType)
				{
					case TValueType.Void:
						{
							return null;
						}
					case TValueType.Boolean:
						{
#if (UNITY_2017_1_OR_NEWER)
							return mNumberData.x == 1;
#else
							return (mNumberData.X == 1);
#endif
						}
					case TValueType.Integer:
						{
#if (UNITY_2017_1_OR_NEWER)
							return (Int32)mNumberData.x;
#else
							return ((Int32)mNumberData.X);
#endif
						}
					case TValueType.Enum:
						{
							return mReferenceData;
						}
					case TValueType.Real:
						{
#if (UNITY_2017_1_OR_NEWER)
							return mNumberData.x;
#else
							return (mNumberData.X);
#endif
						}
					case TValueType.DateTime:
						{
							return DateTime.Parse(mStringData);
						}
					case TValueType.String:
						{
							return mStringData;
						}
#if (UNITY_2017_1_OR_NEWER)
					case TValueType.Color:
						{
							return new UnityEngine.Color(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w);
						}
					case TValueType.Vector2D:
						{
							return new UnityEngine.Vector2(mNumberData.x, mNumberData.y);
						}
					case TValueType.Vector3D:
						{
							return new UnityEngine.Vector3(mNumberData.x, mNumberData.y, mNumberData.z);
						}
					case TValueType.Vector4D:
						{
							return new UnityEngine.Vector4(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w);
						}
					case TValueType.Rect:
						{
							return new UnityEngine.Rect(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w);
						}
					case TValueType.GameObject:
						{
							return mUnityData;
						}
					case TValueType.Texture2D:
						{
							return mUnityData;
						}
					case TValueType.Sprite:
						{
							return mUnityData;
						}
					case TValueType.Model:
						{
							return mUnityData;
						}
					case TValueType.TextAsset:
						{
							return mUnityData;
						}
#endif
					case TValueType.SysObject:
						break;
					default:
						{
						}
						break;
				}

				return mReferenceData;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значение как строкового типа
			/// </summary>
			/// <returns>Значение</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetAsString()
			{
				switch (mValueType)
				{
					case TValueType.Void:
						{
							return (nameof(TValueType.Void));
						}
					case TValueType.Boolean:
						{
#if (UNITY_2017_1_OR_NEWER)
							return(mNumberData.x == 1).ToString();
#else
							return(mNumberData.X == 1).ToString();
#endif
						}
					case TValueType.Integer:
						{
#if (UNITY_2017_1_OR_NEWER)
							return ((Int32)mNumberData.x).ToString();
#else
							return ((Int32)mNumberData.X).ToString();
#endif
						}
					case TValueType.Enum:
						{
							if(mReferenceData != null)
							{
								return ("Null");
							}
							else
							{
								return (mReferenceData.ToString());
							}
						}
					case TValueType.Real:
						{
#if (UNITY_2017_1_OR_NEWER)
							return (mNumberData.x).ToString();
#else
							return (mNumberData.X).ToString();
#endif
						}
					case TValueType.DateTime:
						{
							return (mStringData);
						}
					case TValueType.String:
						{
							return (mStringData);
						}
#if (UNITY_2017_1_OR_NEWER)
					case TValueType.Color:
						{
							return (new UnityEngine.Color(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w).ToString());
						}
					case TValueType.Vector2D:
						{
							return (new UnityEngine.Vector2(mNumberData.x, mNumberData.y).ToString());
						}
					case TValueType.Vector3D:
						{
							return (new UnityEngine.Vector3(mNumberData.x, mNumberData.y, mNumberData.z).ToString());
						}
					case TValueType.Vector4D:
						{
							return (new UnityEngine.Vector4(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w).ToString());
						}
					case TValueType.Rect:
						{
							return (new UnityEngine.Rect(mNumberData.x, mNumberData.y, mNumberData.z, mNumberData.w).ToString());
						}
					case TValueType.GameObject:
						{
							return (((UnityEngine.GameObject)mUnityData)?.name);
						}
					case TValueType.Texture2D:
						{
							return (((UnityEngine.Texture2D)mUnityData)?.name);
						}
					case TValueType.Sprite:
						{
							return (((UnityEngine.Sprite)mUnityData)?.name);
						}
					case TValueType.Model:
						{
							return (((UnityEngine.GameObject)mUnityData)?.name);
						}
					case TValueType.TextAsset:
						{
							return (((UnityEngine.TextAsset)mUnityData)?.name);
						}
#endif
					case TValueType.SysObject:
						break;

					default:
						{
						}
						break;
				}

				if (mReferenceData != null)
				{
					return ("Null");
				}
				else
				{
					return (mReferenceData.ToString());
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка данных
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Clear()
			{
				mValueType = TValueType.Void;
				if (mIOwned != null) mIOwned.OnNotifyUpdated(this, nameof(ValueType));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сериализация типа данных в строку
			/// </summary>
			/// <returns>Строка данных</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual String SerializeToString()
			{
				return String.Format("[{0}]", mValueType);
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================