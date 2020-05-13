//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема связывания данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXDataBindingInfo.cs
*		Параметры для связывания данных.
*		Реализация класса для связывания данных одного члена объекта представления с одним членом данных объекта модели.
*	Связываться должны только одинаковые типы. Только к строковому типу представления можно связать любой тип объекта
*	модели так как происходит преобразования в текстовые данные.
*		Определение объекта представления (он же целевой объект) и объекта модели (он же объект источник) зависят только
*	от контекста использования и режима связывания. Это разделение условно, предназначено в первую очередь для того чтобы
*	просто идентифицировать данные в определении класса.
*		Для связывания данных используется стандартная технология рефлексии данных что не очень быстро и эффективно, 
*	но зато универсально, и технология Delegate.CreateDelegate котороя обеспечивает более быстрое обновление свойств и методов
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Reflection;
using System.Collections.Generic;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonDataBinding
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Базовый класс реализующий привязку данных между свойством/методом объекта модели и объекта представления
		/// </summary>
		/// <remarks>
		/// Реализация базового класса для связывания данных одного члена объекта представления с одним членом данных 
		/// объекта модели.
		/// Связываться должны только одинаковые типы. 
		/// Только к строковому типу представления можно связать любой тип объекта модели так как происходит преобразования в текстовые данные.
		/// Определение объекта представления (он же целевой объект) и объекта модели(он же объект источник) зависят только от
		/// контекста использования и режима связывания.
		/// Это разделение условно, предназначено в первую очередь для того что бы просто идентифицировать данные в 
		/// определении класса
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CBindingBase : IComparable<CBindingBase>, IDisposable
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
			protected internal Boolean mIsEnabled = true;

#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal TBindingMode mMode;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal TBindingModeChanged mModeChanged;

			// Объект модели
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal String mModelMemberName;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal TBindingMemberType mModelMemberType;
			protected internal System.Object mModelInstance;
			protected internal ICubeXNotifyPropertyChanged mModelObject;

			// Объект представления
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal String mViewMemberName;
#if (UNITY_2017_1_OR_NEWER)
			[UnityEngine.SerializeField]
#endif
			protected internal TBindingMemberType mViewMemberType;
			protected internal Boolean mIsStringView;
			protected internal System.Object mViewInstance;
			protected internal ICubeXNotifyPropertyChanged mViewObject;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Имя привязки данных
			/// </summary>
			/// <remarks>
			/// Используется для идентификации привязки
			/// </remarks>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Статус включенности привязки данных
			/// </summary>
			/// <remarks>
			/// Применяется для временного отключения/включения связывания данных без удаления самой привязки
			/// </remarks>
			public Boolean IsEnabled
			{
				get { return mIsEnabled; }
				set { mIsEnabled = value; }
			}

			/// <summary>
			/// Режим связывания данных
			/// </summary>
			public TBindingMode Mode
			{
				get { return mMode; }
				set { mMode = value; }
			}

			/// <summary>
			/// Режим изменения свойства объекта представления
			/// </summary>
			/// <remarks>
			/// Запланировано для будущих реализаций
			/// </remarks>
			public TBindingModeChanged ModeChanged
			{
				get { return mModeChanged; }
				set { mModeChanged = value; }
			}

			//
			// ОБЪЕКТ МОДЕЛИ
			//
			/// <summary>
			/// Имя члена объекта привязки со стороны модели
			/// </summary>
			public String ModelMemberName
			{
				get { return mModelMemberName; }
				set { mModelMemberName = value; }
			}

			/// <summary>
			/// Тип члена объекта привязки со стороны модели
			/// </summary>
			public TBindingMemberType ModelMemberType
			{
				get { return mModelMemberType; }
				set { mModelMemberType = value; }
			}

			/// <summary>
			/// Экземпляр модели
			/// </summary>
			/// <remarks>
			/// Экземпляр модели это собственно объект модели, он не обязательно должен поддерживать интерфейс <see cref="ICubeXNotifyPropertyChanged"/>
			/// например если его данными только управляют TBindingMode.DataManager
			/// </remarks>
			public System.Object ModelInstance
			{
				get { return mModelInstance; }
				set { mModelInstance = value; }
			}

			/// <summary>
			/// Интерфейс объекта модели для нотификации об изменении данных
			/// </summary>
			public ICubeXNotifyPropertyChanged Model
			{
				get { return mModelObject; }
			}

			//
			// ОБЪЕКТ ПРЕДСТАВЛЕНИЯ
			//
			/// <summary>
			/// Имя члена объекта привязки со стороны представления
			/// </summary>
			public String ViewMemberName
			{
				get { return mViewMemberName; }
				set { mViewMemberName = value; }
			}

			/// <summary>
			/// Тип члена объекта привязки со стороны представления
			/// </summary>
			public TBindingMemberType ViewMemberType
			{
				get { return mViewMemberType; }
				set { mViewMemberType = value; }
			}

			/// <summary>
			/// Статус строкового отображения 
			/// </summary>
			/// <remarks>
			/// Истинное значение означает что объект представления, как правило, лишь отображает 
			/// данные модели и представление данные модели возможно через стандартный метод <see cref="System.Object.ToString"/>
			/// </remarks>
			public Boolean IsStringView
			{
				get { return mIsStringView; }
			}

			/// <summary>
			/// Экземпляр представления
			/// </summary>
			/// <remarks>
			/// Экземпляр представления это собственно объект представления, он не обязательно должен поддерживать интерфейс <see cref="ICubeXNotifyPropertyChanged"/>
			/// например если только отображает данные TBindingMode.ViewData
			/// </remarks>
			public System.Object ViewInstance
			{
				get { return mViewInstance; }
				set { mViewInstance = value; }
			}

			/// <summary>
			/// Интерфейс объекта представления для нотификации об изменении данных
			/// </summary>
			public ICubeXNotifyPropertyChanged View
			{
				get { return mViewObject; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CBindingBase()
			{
			}
			#endregion

			#region ======================================= СИСТЕМНЫЕ МЕТОДЫ ==========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сравнение объектов для упорядочивания
			/// </summary>
			/// <param name="other">Объект</param>
			/// <returns>Статус сравнения объектов</returns>
			//---------------------------------------------------------------------------------------------------------
			public Int32 CompareTo(CBindingBase other)
			{
				return String.CompareOrdinal(Name, other.Name);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public String ToStringShort()
			{
				String mode = "";
				switch (mMode)
				{
					case TBindingMode.ViewData:
						mode = " <= ";
						break;
					case TBindingMode.DataManager:
						mode = " => ";
						break;
					case TBindingMode.TwoWay:
						mode = " <=> ";
						break;
					default:
						break;
				}

				return ViewMemberName + mode + ModelMemberName;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Преобразование к текстовому представлению
			/// </summary>
			/// <returns>Текстовое представление</returns>
			//---------------------------------------------------------------------------------------------------------
			public override String ToString()
			{
				String mode = "";
				switch (mMode)
				{
					case TBindingMode.ViewData:
						mode = "<=";
						break;
					case TBindingMode.DataManager:
						mode = "=>";
						break;
					case TBindingMode.TwoWay:
						mode = "<=>";
						break;
					default:
						break;
				}

				return "View (" + ViewMemberName + ") " + mode + " Model (" + ModelMemberName + ")";
			}
			#endregion

			#region ======================================= МЕТОДЫ IDisposable ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Освобождение управляемых ресурсов
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Dispose()
			{
				Dispose(true);
				GC.SuppressFinalize(this);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Освобождение управляемых ресурсов
			/// </summary>
			/// <param name="disposing">Статус освобождения</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void Dispose(Boolean disposing)
			{
				// Освобождаем только управляемые ресурсы
				if (disposing)
				{
					if (mModelObject != null)
					{
						mModelObject.OnPropertyChanged -= UpdateModelProperty;
						mModelObject = null;
					}
					if (mViewObject != null)
					{
						mViewObject.OnPropertyChanged -= UpdateViewProperty;
						mViewObject = null;
					}
				}

				// Освобождаем неуправляемые ресурсы
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка типа члена объекта
			/// </summary>
			/// <param name="instance">Экземпляр объекта</param>
			/// <param name="member_name">Имя члена объекта</param>
			/// <param name="member_type">Тип члена объекта</param>
			/// <returns>Член данных</returns>
			//---------------------------------------------------------------------------------------------------------
			protected MemberInfo SetMemberType(System.Object instance, String member_name, ref TBindingMemberType member_type)
			{
				MemberInfo member = instance.GetType().GetMember(member_name)[0];
				if (member != null)
				{
					if (member.MemberType == MemberTypes.Property)
					{
						member_type = TBindingMemberType.Property;
					}
					else
					{
						if (member.MemberType == MemberTypes.Field)
						{
							member_type = TBindingMemberType.Field;
						}
						else
						{
							member_type = TBindingMemberType.Method;
						}
					}
				}

				return member;
			}
			#endregion

			#region ======================================= РАБОТА С ОБЪЕКТОМ МОДЕЛИ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка объекта модели
			/// </summary>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ResetModel(System.Object model_instance)
			{
				if (mModelObject != null)
				{
					mModelObject.OnPropertyChanged -= UpdateModelProperty;
				}

				mModelInstance = model_instance;
				mModelObject = model_instance as ICubeXNotifyPropertyChanged;
				if (mModelObject != null)
				{
					mModelObject.OnPropertyChanged += UpdateModelProperty;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных объекта модели
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			/// <param name="property_value">Значение свойства</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UpdateModelProperty(String property_name, System.Object property_value)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта модели
			/// </summary>
			/// <remarks>
			/// Предполагается что остальные параметры привязки со стороны объекта модели уже корректно настроены
			/// </remarks>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetModel(System.Object model_instance)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта модели
			/// </summary>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			/// <param name="member_name">Имя члена объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetModel(System.Object model_instance, String member_name)
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения привязанного свойства/метода объекта модели
			/// </summary>
			/// <remarks>
			/// Хотя мы всегда должны знать о значении свойства, на которые подписались, однако иногда надо принудительно
			/// его запросить, например, во время присоединения
			/// </remarks>
			/// <returns>Значение привязанного свойства/метода объекта модели</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual System.Object GetModelValue()
			{
				return null;
			}
			#endregion

			#region ======================================= РАБОТА С ОБЪЕКТОМ ПРЕДСТАВЛЕНИЯ ===========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка объекта представления
			/// </summary>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			protected void ResetView(System.Object view_instance)
			{
				if (mViewObject != null)
				{
					mViewObject.OnPropertyChanged -= UpdateViewProperty;
				}

				mViewInstance = view_instance;
				mViewObject = view_instance as ICubeXNotifyPropertyChanged;
				if (mViewObject != null)
				{
					mViewObject.OnPropertyChanged += UpdateViewProperty;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных объекта представления
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			/// <param name="property_value">Значение свойства</param>
			//---------------------------------------------------------------------------------------------------------
			protected virtual void UpdateViewProperty(String property_name, System.Object property_value)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта представления
			/// </summary>
			/// <remarks>
			/// Предполагается что остальные параметры привязки со стороны объекта представления уже корректно настроены
			/// </remarks>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetView(System.Object view_instance)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта представления
			/// </summary>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			/// <param name="member_name">Имя члена объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void SetView(System.Object view_instance, String member_name)
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения привязанного свойства/метода объекта представления
			/// </summary>
			/// <remarks>
			/// Хотя мы всегда должны знать о значении свойства, на которые подписались, однако иногда надо принудительно
			/// его запросить, например, во время присоединения
			/// </remarks>
			/// <returns>Значение привязанного свойства/метода объекта представления</returns>
			//---------------------------------------------------------------------------------------------------------
			public virtual System.Object GetViewValue()
			{
				return null;
			}
			#endregion

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс реализующий привязку данных между свойством/методом объекта модели и объекта представления
		/// </summary>
		/// <remarks>
		/// Реализация класса для связывания данных.
		/// Для связывания параметров используется стандартная рефлексия что является универсальным методом, но не 
		/// достаточно эффективным и быстрым
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CBindingReflection : CBindingBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			protected internal MemberInfo mModelMember;
			protected internal MemberInfo mViewMember;
			protected internal Func<System.Object, System.Object> mOnConvertToModel;
			protected internal Func<System.Object, System.Object> mOnConvertToView;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Член данных для связывания со стороны объекта модели
			/// </summary>
			public MemberInfo ModelMember
			{
				get { return mModelMember; }
			}

			/// <summary>
			/// Член данных для связывания со стороны объекта представления
			/// </summary>
			public MemberInfo ViewMember
			{
				get { return mViewMember; }
			}

			/// <summary>
			/// Делегат для преобразования объекта представления в объект модели
			/// </summary>
			public Func<System.Object, System.Object> OnConvertToModel
			{
				get { return mOnConvertToModel; }
				set { mOnConvertToModel = value; }
			}

			/// <summary>
			/// Делегат для преобразования объекта модели в объект представления
			/// </summary>
			public Func<System.Object, System.Object> OnConvertToView
			{
				get { return mOnConvertToView; }
				set { mOnConvertToView = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CBindingReflection()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			/// <param name="model_member_name">Имя члена объекта модели</param>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			/// <param name="view_member_name">Имя члена объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public CBindingReflection(System.Object model_instance, String model_member_name, System.Object view_instance, 
				String view_member_name)
			{
				SetModel(model_instance, model_member_name);
				SetView(view_instance, view_member_name);
			}
			#endregion

			#region ======================================= РАБОТА С ОБЪЕКТОМ МОДЕЛИ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта модели
			/// </summary>
			/// <remarks>
			/// Предполагается что остальные параметры привязки со стороны объекта модели уже корректно настроены
			/// </remarks>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetModel(System.Object model_instance)
			{
				ResetModel(model_instance);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта модели
			/// </summary>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			/// <param name="member_name">Имя члена объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetModel(System.Object model_instance, String member_name)
			{
				ResetModel(model_instance);

				mModelMember = SetMemberType(model_instance, member_name, ref mModelMemberType);
				if (mModelMember != null)
				{
					mModelMemberName = member_name;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения привязанного свойства/метода объекта модели
			/// </summary>
			/// <remarks>
			/// Хотя мы всегда должны знать о значении свойства, на которые подписались, однако иногда надо принудительно
			/// его запросить, например, во время присоединения
			/// </remarks>
			/// <returns>Значение привязанного свойства/метода объекта модели</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object GetModelValue()
			{
				return mModelMember.GetMemberValue(mModelInstance);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных объекта модели
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			/// <param name="property_value">Значение свойства</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void UpdateModelProperty(String property_name, System.Object property_value)
			{
				if (mIsEnabled)
				{
					if (mModelMemberName == property_name)
					{
						// Если есть конвертер используем его
						if (mOnConvertToView != null)
						{
							mViewMember.SetMemberValue(mViewInstance, mOnConvertToView(property_value));
						}
						else
						{
							if (mIsStringView)
							{
								mViewMember.SetMemberValue(mViewInstance, property_value.ToString());
							}
							else
							{
								mViewMember.SetMemberValue(mViewInstance, property_value);
							}
						}
					}
				}
			}
			#endregion

			#region ======================================= РАБОТА С ОБЪЕКТОМ ПРЕДСТАВЛЕНИЯ ===========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта представления
			/// </summary>
			/// <remarks>
			/// Предполагается что остальные параметры привязки со стороны объекта представления уже корректно настроены
			/// </remarks>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetView(System.Object view_instance)
			{
				ResetView(view_instance);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта представления
			/// </summary>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			/// <param name="member_name">Имя члена типа объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetView(System.Object view_instance, String member_name)
			{
				ResetView(view_instance);
				mViewMember = SetMemberType(view_instance, member_name, ref mViewMemberType);
				if (mViewMember != null)
				{
					mViewMemberName = member_name;
				}
				if(mViewMember.GetMemberType() == typeof(String))
				{
					mIsStringView = true;
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения привязанного свойства/метода объекта представления
			/// </summary>
			/// <remarks>
			/// Хотя мы всегда должны знать о значении свойства, на которые подписались, однако иногда надо принудительно
			/// его запросить, например, во время присоединения
			/// </remarks>
			/// <returns>Значение привязанного свойства/метода объекта представления</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object GetViewValue()
			{
				return mViewMember.GetMemberValue(mViewInstance);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных объекта представления
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			/// <param name="property_value">Значение свойства</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void UpdateViewProperty(String property_name, System.Object property_value)
			{
				if (mIsEnabled)
				{
					if (mViewMemberName == property_name)
					{
						if (mOnConvertToModel != null)
						{
							mModelMember.SetMemberValue(mModelInstance, mOnConvertToModel(property_value));
						}
						else
						{
							mModelMember.SetMemberValue(mModelInstance, property_value);
						}
					}
				}
			}
			#endregion

		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс реализующий привязку данных между свойством/методом объекта модели и объекта представления
		/// </summary>
		/// <remarks>
		/// Реализация класса для связывания данных.
		/// Для связывания параметров используется технология <see cref="Delegate.CreateDelegate(Type, Object, String)"/> 
		/// что обеспечивает более быстрое обновление свойств и полей
		/// </remarks>
		/// <typeparam name="TTypeModel">Тип члена объекта модели</typeparam>
		/// <typeparam name="TTypeView">Тип члена объекта представления</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class BindingDelegate<TTypeModel, TTypeView> : CBindingBase
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			protected internal Action<TTypeModel> mActionModel;
			protected internal Action<TTypeView> mActionView;
			protected internal Func<TTypeView, TTypeModel> mOnConvertToModel;
			protected internal Func<TTypeModel, TTypeView> mOnConvertToView;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Делегат для установки значений объекту модели
			/// </summary>
			public Action<TTypeModel> ActionModel
			{
				get { return mActionModel; }
			}

			/// <summary>
			/// Делегат для установки значений объекту представления
			/// </summary>
			public Action<TTypeView> ActionView
			{
				get { return mActionView; }
			}

			/// <summary>
			/// Делегат для преобразования объекта представления в объект модели
			/// </summary>
			public Func<TTypeView, TTypeModel> OnConvertToModel
			{
				get { return mOnConvertToModel; }
				set { mOnConvertToModel = value; }
			}

			/// <summary>
			/// Делегат для преобразования объекта модели в объект представления
			/// </summary>
			public Func<TTypeModel, TTypeView> OnConvertToView
			{
				get { return mOnConvertToView; }
				set { mOnConvertToView = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public BindingDelegate()
			{
				mIsStringView = (typeof(TTypeView) == typeof(String));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			/// <param name="model_member_name">Имя члена объекта модели</param>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			/// <param name="view_member_name">Имя члена объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public BindingDelegate(System.Object model_instance, String model_member_name, System.Object view_instance, 
				String view_member_name)
			{
				SetModel(model_instance, model_member_name);
				SetView(view_instance, view_member_name);
			}
			#endregion

			#region ======================================= РАБОТА С ОБЪЕКТОМ МОДЕЛИ ==================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта модели
			/// </summary>
			/// <remarks>
			/// Предполагается что остальные параметры привязки со стороны объекта модели уже корректно настроены
			/// </remarks>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetModel(System.Object model_instance)
			{
				ResetModel(model_instance);
				if (mMode != TBindingMode.ViewData)
				{
					String member_name_model = mModelMemberName;
					if (mModelMemberType == TBindingMemberType.Property)
					{
						member_name_model = "set_" + mModelMemberName;
					}
					try
					{
						mActionModel = (Action<TTypeModel>)Delegate.CreateDelegate(typeof(Action<TTypeModel>), model_instance, member_name_model);
					}
					catch (Exception exc)
					{
#if (UNITY_2017_1_OR_NEWER)
						UnityEngine.Debug.LogException(exc);
#endif
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта модели
			/// </summary>
			/// <param name="model_instance">Экземпляр объекта модели</param>
			/// <param name="member_name">Имя члена объекта модели</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetModel(System.Object model_instance, String member_name)
			{
				ResetModel(model_instance);

				if (SetMemberType(model_instance, member_name, ref mModelMemberType) != null)
				{
					mModelMemberName = member_name;
					if (mMode != TBindingMode.ViewData)
					{
						String member_name_model = mModelMemberName;
						if (mModelMemberType == TBindingMemberType.Property)
						{
							member_name_model = "set_" + mModelMemberName;
						}
						try
						{
							mActionModel = (Action<TTypeModel>)Delegate.CreateDelegate(typeof(Action<TTypeModel>), model_instance, member_name_model);
						}
						catch (Exception exc)
						{
#if (UNITY_2017_1_OR_NEWER)
							UnityEngine.Debug.LogException(exc);
#endif
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения привязанного свойства/метода объекта модели
			/// </summary>
			/// <remarks>
			/// Хотя мы всегда должны знать о значении свойства, на которые подписались, однако иногда надо принудительно
			/// его запросить, например, во время присоединения
			/// </remarks>
			/// <returns>Значение привязанного свойства/метода объекта модели</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object GetModelValue()
			{
				// Проверяем сначала свойство 
				if (XReflection.ContainsProperty(mModelInstance, mModelMemberName))
				{
					return (XReflection.GetPropertyValue(mModelInstance, mModelMemberName));
				}
				else
				{
					// Теперь поле
					return (XReflection.GetFieldValue(mModelInstance, mModelMemberName));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных объекта модели
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			/// <param name="property_value">Значение свойства</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void UpdateModelProperty(String property_name, System.Object property_value)
			{
				if (mIsEnabled)
				{
					if (mModelMemberName == property_name)
					{
						if (mOnConvertToView != null)
						{
							mActionView(mOnConvertToView((TTypeModel)property_value));
						}
						else
						{
							if (mIsStringView)
							{
								mActionView((TTypeView)(System.Object)property_value.ToString());
							}
							else
							{
								mActionView((TTypeView)property_value);
							}
						}
					}
				}
			}
			#endregion

			#region ======================================= РАБОТА С ОБЪЕКТОМ ПРЕДСТАВЛЕНИЯ ===========================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта представления
			/// </summary>
			/// <remarks>
			/// Предполагается что остальные параметры привязки со стороны объекта представления уже корректно настроены
			/// </remarks>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetView(System.Object view_instance)
			{
				ResetView(view_instance);
				if (mMode != TBindingMode.DataManager)
				{
					String member_name_view = mViewMemberName;
					if (mViewMemberType == TBindingMemberType.Property)
					{
						member_name_view = "set_" + mViewMemberName;
					}
					try
					{
						mActionView = (Action<TTypeView>)Delegate.CreateDelegate(typeof(Action<TTypeView>), view_instance, member_name_view);
					}
					catch (Exception exc)
					{
#if (UNITY_2017_1_OR_NEWER)
						UnityEngine.Debug.LogException(exc);
#endif
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка объекта представления
			/// </summary>
			/// <param name="view_instance">Экземпляр объекта представления</param>
			/// <param name="member_name">Имя члена объекта представления</param>
			//---------------------------------------------------------------------------------------------------------
			public override void SetView(System.Object view_instance, String member_name)
			{
				mIsStringView = (typeof(TTypeView) == typeof(String));
				ResetView(view_instance);
				if (SetMemberType(view_instance, member_name, ref mViewMemberType) != null)
				{
					mViewMemberName = member_name;
					if (mMode != TBindingMode.DataManager)
					{
						String member_name_view = mViewMemberName;
						if (mViewMemberType == TBindingMemberType.Property)
						{
							member_name_view = "set_" + mViewMemberName;
						}
						try
						{
							mActionView = (Action<TTypeView>)Delegate.CreateDelegate(typeof(Action<TTypeView>), view_instance, member_name_view);
						}
						catch (Exception exc)
						{
#if (UNITY_2017_1_OR_NEWER)
							UnityEngine.Debug.LogException(exc);
#endif
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение значения привязанного свойства/метода объекта представления
			/// </summary>
			/// <remarks>
			/// Хотя мы всегда должны знать о значении свойства, на которые подписались, однако иногда надо принудительно
			/// его запросить, например, во время присоединения
			/// </remarks>
			/// <returns>Значение привязанного свойства/метода объекта представления</returns>
			//---------------------------------------------------------------------------------------------------------
			public override System.Object GetViewValue()
			{
				// Проверяем сначала свойство 
				if (XReflection.ContainsProperty(mViewInstance, mViewMemberName))
				{
					return (XReflection.GetPropertyValue(mViewInstance, mViewMemberName));
				}
				else
				{
					// Теперь поле
					return (XReflection.GetFieldValue(mViewInstance, mViewMemberName));
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление данных объекта представления
			/// </summary>
			/// <param name="property_name">Имя свойства</param>
			/// <param name="property_value">Значение свойства</param>
			//---------------------------------------------------------------------------------------------------------
			protected override void UpdateViewProperty(String property_name, System.Object property_value)
			{
				if (mIsEnabled)
				{
					if (mViewMemberName == property_name)
					{
						if (mOnConvertToModel != null)
						{
							mActionModel(mOnConvertToModel((TTypeView)property_value));
						}
						else
						{
							mActionModel((TTypeModel)property_value);
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