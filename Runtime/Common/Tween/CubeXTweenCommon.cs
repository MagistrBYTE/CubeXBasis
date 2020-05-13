//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenCommon.cs
*		Общие типы и структуры данных подсистема анимации.
*		Общие типы и структуры данных подсистемы анимации применяемые в целом к игровому процессу и в других подсистемах.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.ComponentModel;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup UnityCommonTween Подсистема анимации
		//! Подсистема предназначена для анимации различных параметров посредством анимационных кривых или функций плавности.
		//! Данная подсистема специально предназначена для типовой анимации различных параметров (трансформации,
		//! цвета, спрайта) с поддержкой различных режимов посредством анимационных кривых.
		//!
		//! ## Возможности/особенности
		//! 1. Полноценное управления различными режимами воспроизведения анимации
		//! 2. Информирование путем событий о начале и окончании анимации
		//! 3. Возможность использования как готовых компонент, так и независимых классов в пользовательских скриптах
		//!
		//! ## Описание
		//! Подсистема анимации обеспечивает плавное изменение величины строго определенного типа по определенному правилу.
		//! В большинстве случаев в качестве правила (источника) выступает анимационная кривая. 
		//! Использование в качестве правила (источника) функции запланировано на будущие выпуски.
		//!
		//! ## Использование хранилища кривых 
		//! Для централизованного доступа, хранения и представления анимационных кривых используется системный 
		//! ресурс \ref CubeX.Common.CubeXTweenCurveStorage.
		//! Данный ресурс представляет методы добавления, удаления анимационных кривых, доступ к кривой по индексу, 
		//! вычисление значения кривой по ключу времени, а также методы по сохранению данных всех анимационных кривых в 
		//! файл и последующей загрузки.
		//!
		//! ## Использование хранилища анимации
		//! Для централизованного доступа, хранения и представления анимации спрайтов используется системный 
		//! ресурс \ref CubeX.Common.CubeXTweenSpriteStorage.
		//! Данный ресурс представляет методы добавления, удаления, получения анимационных спрайтов(последовательности спрайтов
		//! одной анимационной цепочки), а также заполнение анимационных цепочек на основе текстуры спрайта.
		//!
		//! ## Использование
		//! 1. Путем создания переменных класса. В качестве типа переменных могут выступать типы: \ref CubeX.Common.CTweenInteger, 
		//! \ref CubeX.Common.CTweenSingle и другие.
		//! 2. В методе Update или LateUpdate должен выполнятся метод UpdateAnimation
		//! 3. В качестве отдельных компонент \ref CubeX.Common.CubeXTweenSingle, \ref CubeX.Common.CubeXTweenColor 
		//! 4. Для работы подсистемы анимации на сцене должен присутствовать центральный диспетчер анимации \ref CubeX.Common.CubeXTweenDispatcher
		//! \ingroup UnityCommon
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Источник данных по проигрыванию анимации
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTweenAnimationSource
		{
			/// <summary>
			/// Функция плавности
			/// </summary>
			FunctionEasing,

			/// <summary>
			/// Анимационная кривая
			/// </summary>
			AnimationCurve
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Режим проигрывания анимации
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTweenWrapMode
		{
			/// <summary>
			/// Однократное проигрывание анимации
			/// </summary>
			Once,

			/// <summary>
			/// Однократное проигрывание анимации (назад)
			/// </summary>
			OnceBackward,

			/// <summary>
			/// Циклическое проигрывание анимации
			/// </summary>
			Loop,

			/// <summary>
			/// Однократное проигрывание анимации вперед и назад
			/// </summary>
			PingPong,

			/// <summary>
			/// Циклическое проигрывание анимации вперед и назад
			/// </summary>
			LoopPingPong
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Параметр трансформации (для связывания параметров)
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public enum TTweenTransformParameterType
		{
			/// <summary>
			/// Позиция
			/// </summary>
			Position,

			/// <summary>
			/// Локальная позиция
			/// </summary>
			LocalPosition,

			/// <summary>
			/// Вращение
			/// </summary>
			Rotation,

			/// <summary>
			/// Локальное вращение
			/// </summary>
			LocalRotation,

			/// <summary>
			/// Масштабирование
			/// </summary>
			Scale
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Обобщенный интерфейс для анимации значения
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		public interface ICubeXTweenValue
		{
			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Игнорирование масштабирование времени
			/// </summary>
			Boolean IgnoreTimeScale { get; }

			/// <summary>
			/// Продолжительность анимации
			/// </summary>
			Single Duration { get; }
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение текущего значения к анимации
			/// </summary>
			/// <param name="percentage">Процент в частях</param>
			//---------------------------------------------------------------------------------------------------------
			void TweenValue(Single percentage);
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления анимации обобщенным типом
		/// </summary>
		/// <typeparam name="TAnimatableType">Тип анимационного значения</typeparam>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class TweenGenericObject<TAnimatableType> : ICubeXTweenValue
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXSerializeMember(nameof(Name))]
			internal String mName;
			[SerializeField]
			[CubeXSerializeMember(nameof(WrapMode))]
			internal TTweenWrapMode mWrapMode;
			[NonSerialized]
			internal TAnimatableType mValue;
			[SerializeField]
			[CubeXSerializeMember(nameof(StartValue))]
			internal TAnimatableType mStartValue;
			[SerializeField]
			[CubeXSerializeMember(nameof(TargetValue))]
			internal TAnimatableType mTargetValue;
			[SerializeField]
			[CubeXSerializeMember(nameof(IgnoreTimeScale))]
			internal Boolean mIgnoreTimeScale;
			[NonSerialized]
			internal TAnimatableType mDistance;
			[SerializeField]
			[CubeXSerializeMember(nameof(Duration))]
			internal Single mDuration = 0.3f;
			[NonSerialized]
			internal Single mCorrectTime;
			[NonSerialized]
			internal Single mCurrentTime;
			[NonSerialized]
			internal Single mDeltaTime;
			[NonSerialized]
			internal Single mNormalizeTime;
			[NonSerialized]
			internal Boolean mPreEnd;
			[NonSerialized]
			internal Boolean mStart;
			[NonSerialized]
			internal Int32 mCurrentCountLoop;
			[SerializeField]
			[CubeXSerializeMember(nameof(CountLoop))]
			internal Int32 mCountLoop;
			[NonSerialized]
			internal Boolean mIsPause;
			[NonSerialized]
			internal Boolean mIsForward;
			[SerializeField]
			[CubeXSerializeMember(nameof(CurveIndexForward))]
			internal Int32 mCurveIndexForward;
			[SerializeField]
			[CubeXSerializeMember(nameof(CurveIndexBackward))]
			internal Int32 mCurveIndexBackward;
			[NonSerialized]
			internal Int32 mCurrentIndexForward = -1;

			// События
			[NonSerialized]
			internal Action<String> mOnAnimationStart;
			[NonSerialized]
			internal Action<String> mOnAnimationCompleted;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Название анимации
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Текущее значение переменной
			/// </summary>
			public TAnimatableType Value
			{
				get { return mValue; }
			}

			/// <summary>
			/// Начальное значение переменной
			/// </summary>
			public TAnimatableType StartValue
			{
				get { return mStartValue; }
				set { mStartValue = value; }
			}

			/// <summary>
			/// Целевое значение переменной
			/// </summary>
			public TAnimatableType TargetValue
			{
				get { return mTargetValue; }
				set { mTargetValue = value; }
			}

			/// <summary>
			/// Режим проигрывания анимации
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mWrapMode; }
				set { mWrapMode = value; }
			}

			/// <summary>
			/// Время(по умолчанию) в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single Duration
			{
				get { return mDuration; }
				set { mDuration = value; }
			}

			/// <summary>
			/// Нормализованное время прохождение анимации в пределах от 0 до 1
			/// </summary>
			[Description("Нормализованное время прохождение анимации в пределах от 0 до 1")]
			public Single NormalizeTime
			{
				get { return mNormalizeTime; }
			}

			/// <summary>
			/// Статус игнорирования масштабирования времени
			/// </summary>
			public Boolean IgnoreTimeScale
			{
				get { return mIgnoreTimeScale; }
				set { mIgnoreTimeScale = value; }
			}

			/// <summary>
			/// Статус активной анимации
			/// </summary>
			/// <remarks>
			/// Означает что нужно вызвать каждый кадр метод UpdateAnimation в производных классах
			/// </remarks>
			public Boolean IsPlay
			{
				get { return mStart; }
			}

			/// <summary>
			/// Установка/снятие паузы анимации
			/// </summary>
			public Boolean IsPause
			{
				get { return mIsPause; }
				set { mIsPause = value; }
			}

			/// <summary>
			/// Статус проигрывания анимации вперед
			/// </summary>
			public Boolean IsForward
			{
				get { return mIsForward; }
			}

			/// <summary>
			/// Имя анимационной кривой
			/// </summary>
			public String CurveName
			{
				get 
				{
					return CubeXTweenDispatcher.CurveStorage.Curves[mCurveIndexForward].Name;
				}
			}

			/// <summary>
			/// Количество циклов проигрывания циклических анимаций. 0 - бесконечно
			/// </summary>
			[Description("Количество циклов проигрывания циклических анимаций. 0 - бесконечно")]
			public Int32 CountLoop
			{
				get { return mCountLoop; }
				set { mCountLoop = value; }
			}

			/// <summary>
			/// Текущие количество проигрывания циклических анимаций
			/// </summary>
			public Int32 CurrentCountLoop
			{
				get { return mCurrentCountLoop; }
				set { mCurrentCountLoop = value; }
			}

			/// <summary>
			/// Индекс кривой для анимации вперед
			/// </summary>
			public Int32 CurveIndexForward
			{
				get { return mCurveIndexForward; }
			}

			/// <summary>
			/// Индекс кривой для анимации назад
			/// </summary>
			public Int32 CurveIndexBackward
			{
				get { return mCurveIndexBackward; }
			}

			/// <summary>
			/// Текущий индекс кривой для анимации вперед
			/// </summary>
			/// <remarks>
			/// По умолчанию используется индекс кривой <see cref="CurveIndexForward"/>, однако 
			/// если используется только однократное проигрывание и программное управление, 
			/// то можно использовать в качестве индекса кривой индекс кривой <see cref="CurveIndexBackward"/>
			/// </remarks>
			public Int32 CurrentIndexForward
			{
				get { return mCurrentIndexForward; }
				set { mCurrentIndexForward = value; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о начале анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationStart
			{
				get { return mOnAnimationStart; }
				set { mOnAnimationStart = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationCompleted
			{
				get { return mOnAnimationCompleted; }
				set { mOnAnimationCompleted = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию инициализирует объект класса предустановленными значениями
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public TweenGenericObject()
			{
				mName = "";
				mCorrectTime = 1;
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сброс анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void ResetAnimation()
			{
				if (mCurrentIndexForward == -1) mCurrentIndexForward = mCurveIndexForward;

				mCorrectTime = mDuration / CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurrentIndexForward);
				mCurrentTime = 0;
				mValue = mStartValue;
				mIsForward = true;
				mCurrentCountLoop = 0;
				mPreEnd = false;
				mDeltaTime = 0;

				if(mWrapMode == TTweenWrapMode.OnceBackward)
				{
					mIsForward = false;
					mValue = mTargetValue;
					mCorrectTime = mDuration / CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurveIndexBackward);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление времени анимации и определение статуса продолжения
			/// </summary>
			/// <returns>Статус продолжения анимации</returns>
			//---------------------------------------------------------------------------------------------------------
			protected Boolean UpdateTimeAnimation()
			{
				// Выход из последнего прохода
				if(mPreEnd)
				{
					mStart = false;
					mPreEnd = false;

					if (mOnAnimationCompleted != null)
					{
						mOnAnimationCompleted(mName);
					}

					return false;
				}

				if (mStart && mIsPause == false)
				{
					// Прошла часть времени - прибавляем
					mCurrentTime += mIgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

					if (mIsForward)
					{
						// Получаем значение на кривой по соответствующему нормализованному времени
						mNormalizeTime = mCurrentTime / mCorrectTime;
						mDeltaTime = CubeXTweenDispatcher.CurveStorage.Evaluate(mCurrentIndexForward, mNormalizeTime);
					}
					else
					{
						// Получаем значение на кривой по соответствующему нормализованному времени
						Single back_time = CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurveIndexBackward);
						mNormalizeTime = back_time - mCurrentTime / mCorrectTime;
						mDeltaTime = CubeXTweenDispatcher.CurveStorage.Evaluate(mCurveIndexBackward, mNormalizeTime);
					}

					return true;
				}

				return false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Проверка статуса анимации и вызов событий
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			protected void CheckAnimationStatus()
			{
				// Время закончилось 
				if (mCurrentTime >= mDuration)
				{
					switch (mWrapMode)
					{
						case TTweenWrapMode.Once:
							{
								mPreEnd = true;
								mValue = mTargetValue;
							}
							break;
						case TTweenWrapMode.OnceBackward:
							{
								mPreEnd = true;
								mValue = mStartValue;
							}
							break;
						case TTweenWrapMode.Loop:
							{
								mCurrentTime = 0;
								mValue = mStartValue;

								if(mCountLoop != 0)
								{
									if(mCurrentCountLoop == mCountLoop)
									{
										mPreEnd = true;
									}
								}

								mCurrentCountLoop++;
							}
							break;
						case TTweenWrapMode.PingPong:
							{
								if (mIsForward)
								{
									mIsForward = false;
									mValue = mTargetValue;
									mCurrentTime = 0;
									mCorrectTime = mDuration / CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurveIndexForward);
								}
								else
								{
									mPreEnd = true;
									mValue = mStartValue;
									mCorrectTime = mDuration / CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurveIndexBackward);
								}
							}
							break;
						case TTweenWrapMode.LoopPingPong:
							{
								if (mIsForward)
								{
									mIsForward = false;
									mValue = mTargetValue;
									mCurrentTime = 0;
									mCorrectTime = mDuration / CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurveIndexForward);
								}
								else
								{
									mIsForward = true;
									mValue = mStartValue;
									mCurrentTime = 0;

									if (mCountLoop != 0)
									{
										if (mCurrentCountLoop == mCountLoop)
										{
											mPreEnd = true;
										}
									}

									mCurrentCountLoop++;

									mCorrectTime = mDuration / CubeXTweenDispatcher.CurveStorage.GetTimeCurve(mCurveIndexBackward);
								}
							}
							break;
						default:
							break;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Остановка анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopAnimation()
			{
				ResetAnimation();
				mStart = false;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка индекса кривой анимации вперед по имени анимационной кривой
			/// </summary>
			/// <param name="name">Имя анимационной кривой</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetCurveForwardIndex(String name)
			{
				mCurveIndexForward = CubeXTweenDispatcher.CurveStorage.GetIndexCurve(name);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Установка индекса кривой анимации назад по имени анимационной кривой
			/// </summary>
			/// <param name="name">Имя анимационной кривой</param>
			//---------------------------------------------------------------------------------------------------------
			public void SetCurveBackwardIndex(String name)
			{
				mCurveIndexBackward = CubeXTweenDispatcher.CurveStorage.GetIndexCurve(name);
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение текущего значения к анимации
			/// </summary>
			/// <param name="percentage">Процент в частях</param>
			//---------------------------------------------------------------------------------------------------------
			public virtual void TweenValue(Single percentage)
			{
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления анимации целым значением
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenInteger : TweenGenericObject<Int32>, ICubeXTaskInfo
		{
			#region ======================================= СВОЙСТВА ICubeXTaskInfo ===================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public Boolean IsTaskCompleted
			{
				get { return !mStart; }
			}

			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			public Single TaskPercentCompletion
			{
				get { return mNormalizeTime; }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTaskInfo =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = (Int32)(mStartValue + mDeltaTime * mDistance);

					this.CheckAnimationStatus();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				StopAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				StopAnimation();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				this.ResetAnimation();

				mDistance = mTargetValue - mStartValue;

				if (mOnAnimationStart != null)
				{
					mOnAnimationStart(mName);
				}

				mStart = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				if(this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = (Int32)(mStartValue + mDeltaTime * mDistance);

					this.CheckAnimationStatus();
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления анимации вещественным значением
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenSingle : TweenGenericObject<Single>, ICubeXTaskInfo
		{
			#region ======================================= СВОЙСТВА ICubeXTaskInfo ===================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public Boolean IsTaskCompleted
			{
				get { return !mStart; }
			}

			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			public Single TaskPercentCompletion
			{
				get { return mNormalizeTime; }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTaskInfo =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				StopAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				StopAnimation();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				this.ResetAnimation();

				mDistance = mTargetValue - mStartValue;

				if (mOnAnimationStart != null)
				{
					mOnAnimationStart(mName);
				}

				mStart = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления анимации цветовым значением
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenColor : TweenGenericObject<Color>, ICubeXTaskInfo
		{
			#region ======================================= СВОЙСТВА ICubeXTaskInfo ===================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public Boolean IsTaskCompleted
			{
				get { return !mStart; }
			}

			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			public Single TaskPercentCompletion
			{
				get { return mNormalizeTime; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор по умолчанию
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public CTweenColor()
			{
				mValue = Color.white;
				mTargetValue = Color.red;
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTaskInfo =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				StopAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				StopAnimation();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				this.ResetAnimation();

				mDistance = mTargetValue - mStartValue;

				if (mOnAnimationStart != null)
				{
					mOnAnimationStart(mName);
				}

				mStart = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления анимации 2D вектором
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenVector2D : TweenGenericObject<Vector2>, ICubeXTaskInfo
		{
			#region ======================================= СВОЙСТВА ICubeXTaskInfo ===================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public Boolean IsTaskCompleted
			{
				get { return !mStart; }
			}

			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			public Single TaskPercentCompletion
			{
				get { return mNormalizeTime; }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTaskInfo =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				StopAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				StopAnimation();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				this.ResetAnimation();

				mDistance = mTargetValue - mStartValue;

				if (mOnAnimationStart != null)
				{
					mOnAnimationStart(mName);
				}

				mStart = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Класс для хранения и управления анимации 3D вектором
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenVector3D : TweenGenericObject<Vector3>, ICubeXTaskInfo
		{
			#region ======================================= СВОЙСТВА ICubeXTaskInfo ===================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public Boolean IsTaskCompleted
			{
				get { return !mStart; }
			}

			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			public Single TaskPercentCompletion
			{
				get { return mNormalizeTime; }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTaskInfo =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				StopAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				StopAnimation();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				this.ResetAnimation();

				mDistance = mTargetValue - mStartValue;

				if (mOnAnimationStart != null)
				{
					mOnAnimationStart(mName);
				}

				mStart = true;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				if (this.UpdateTimeAnimation())
				{
					// Изменяем значение переменной
					mValue = mStartValue + mDeltaTime * mDistance;

					this.CheckAnimationStatus();
				}
			}
			#endregion
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Специальный класс для хранения и управления анимации спрайтами
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CTweenSprite : ICubeXTaskInfo
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			internal String mName;
			[SerializeField]
			internal Int32 mGroupIndex;
			[SerializeField]
			internal Int32 mGroupSpriteIndex;
			[SerializeField]
			internal Int32 mStorageSpriteIndex;
			[SerializeField]
			internal TTweenWrapMode mWrapMode;
			[SerializeField]
			internal Int32 mStartFrame;
			[SerializeField]
			internal Int32 mTargetFrame;
			[SerializeField]
			internal Single mDuration = 0.3f;
			[NonSerialized]
			internal Single mNormalizeTime;
			[NonSerialized]
			internal Single mCurrentTime;
			[NonSerialized]
			internal Int32 mFrameIndex;
			[NonSerialized]
			internal Int32 mFrameCount;
			[NonSerialized]
			internal Boolean mIsPause;
			[NonSerialized]
			internal Boolean mStart;
			[NonSerialized]
			internal Boolean mIsForward;

			// События
			[NonSerialized]
			internal Action<String> mOnAnimationStart;
			[NonSerialized]
			internal Action<String> mOnAnimationCompleted;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Название анимации
			/// </summary>
			public String Name
			{
				get { return mName; }
				set { mName = value; }
			}

			/// <summary>
			/// Индекс группы анимационных цепочек
			/// </summary>
			public Int32 GroupIndex
			{
				get { return mGroupIndex; }
				set
				{
					mGroupIndex = value;
					mStorageSpriteIndex = CubeXTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Имя группы анимационных цепочек
			/// </summary>
			public String GroupName
			{
				get
				{
					if (mGroupIndex == -1)
					{
						return (String.Empty);
					}
					else
					{
						return CubeXTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Name;
					}
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки в группе
			/// </summary>
			public Int32 GroupSpriteIndex
			{
				get { return mGroupSpriteIndex; }
				set
				{
					if (value < 0) value = 0;
					if(value > CubeXTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Count - 1)
					{
						value = CubeXTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex].Count - 1;
					}
					mGroupSpriteIndex = value;
					mStorageSpriteIndex = CubeXTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Имя анимационной цепочки из группы
			/// </summary>
			public String GroupSpriteName
			{
				get
				{
					if (mGroupSpriteIndex == -1)
					{
						return String.Empty;
					}
					else
					{
						return (CubeXTweenDispatcher.SpriteStorage.GroupSprites[mGroupIndex][mGroupSpriteIndex].Name);
					}
				}
			}

			/// <summary>
			/// Индекс анимационной цепочки из хранилища анимаций спрайтов
			/// </summary>
			public Int32 StorageSpriteIndex
			{
				get { return mStorageSpriteIndex; }
				set
				{
					mStorageSpriteIndex = value;
					CubeXTweenDispatcher.SpriteStorage.GetGroupIndexAndSpriteIndex(mStorageSpriteIndex, ref mGroupIndex, ref mGroupSpriteIndex);
				}
			}

			/// <summary>
			/// Режим проигрывания анимации
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mWrapMode; }
				set { mWrapMode = value; }
			}

			/// <summary>
			/// Начальный кадр
			/// </summary>
			public Int32 StartFrame
			{
				get { return mStartFrame; }
				set { mStartFrame = value; }
			}

			/// <summary>
			/// Целевой кадр
			/// </summary>
			public Int32 TargetFrame
			{
				get { return mTargetFrame; }
				set { mTargetFrame = value; }
			}

			/// <summary>
			/// Время в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single Duration
			{
				get { return mDuration; }
				set { mDuration = value; }
			}

			/// <summary>
			/// Нормализованное время прохождение анимации в пределах от 0 до 1
			/// </summary>
			public Single NormalizeTime
			{
				get { return mNormalizeTime; }
			}

			/// <summary>
			/// Номер текущего кадра
			/// </summary>
			public Int32 FrameIndex
			{
				get { return mFrameIndex; }
			}

			/// <summary>
			/// Текущий проигрываемый спрайт
			/// </summary>
			public Sprite FrameSprite
			{
				get
				{
					if (mStorageSpriteIndex == -1)
					{
						return null;
					}
					else
					{
						return (CubeXTweenDispatcher.SpriteStorage.GetFrameSprite(mGroupIndex, mGroupSpriteIndex, mFrameIndex));
					}
				}
			}

			/// <summary>
			/// Статус проигрывания анимации
			/// </summary>
			/// <remarks>
			/// Означает что нужно вызвать каждый кадр метод UpdateAnimation в производных классах
			/// </remarks>
			public Boolean IsPlay
			{
				get { return mStart; }
			}

			/// <summary>
			/// Установка/снятие паузы анимации
			/// </summary>
			public Boolean IsPause
			{
				get { return mIsPause; }
				set { mIsPause = value; }
			}

			/// <summary>
			/// Статус проигрывания анимации вперед
			/// </summary>
			public Boolean IsForward
			{
				get { return mIsForward; }
			}

			//
			// СОБЫТИЯ
			//
			/// <summary>
			/// Событие для нотификации о начале анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationStart
			{
				get { return mOnAnimationStart; }
				set { mOnAnimationStart = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationCompleted
			{
				get { return mOnAnimationCompleted; }
				set { mOnAnimationCompleted = value; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXTaskInfo ===================================
			/// <summary>
			/// Статус завершение задачи
			/// </summary>
			public Boolean IsTaskCompleted
			{
				get
				{
					if (mWrapMode != TTweenWrapMode.Loop && mWrapMode != TTweenWrapMode.LoopPingPong)
					{
						return !mStart;
					}
					else
					{
#if UNITY_EDITOR
						Debug.LogWarningFormat("Task sprite <{0}> WrapMode = {1}", mName, mWrapMode);
#endif
						return (true);
					}
				}
			}

			/// <summary>
			/// Процент выполнения задачи
			/// </summary>
			public Single TaskPercentCompletion
			{
				get { return mNormalizeTime; }
			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXTaskInfo =====================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск выполнение задачи
			/// </summary>
			/// <remarks>
			/// Здесь должна выполняться первоначальная работа по подготовки к выполнению задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void RunTask()
			{
				StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Выполнение задачи
			/// </summary>
			/// <remarks>
			/// Непосредственное выполнение задачи. Метод будет вызываться каждый раз в зависимости от способа
			/// и режима выполнения задачи
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ExecuteTask()
			{
				UpdateAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Принудительная остановка выполнения задачи
			/// </summary>
			/// <remarks>
			/// Если задачи будет принудительно остановлена то здесь можно/нужно реализовать необходимые действия
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void StopTask()
			{
				StopAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Переустановка данных задачи
			/// </summary>
			/// <remarks>
			/// Здесь может быть выполняться работа по подготовки к выполнению задачи, но без запуска на выполнение
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			public void ResetTask()
			{
				StopAnimation();
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			/// <param name="animation_name">Имя проигрываемой анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation(String animation_name)
			{
				// Только если анимация не запущена
				if (mStart == false)
				{
					for (Int32 ig = 0; ig < CubeXTweenDispatcher.SpriteStorage.GroupCount; ig++)
					{
						CTweenSpriteGroup sprite_group = CubeXTweenDispatcher.SpriteStorage.GroupSprites[ig];
						if(sprite_group != null)
						{
							for (Int32 i = 0; i < sprite_group.Count; i++)
							{
								CTweenSpriteData sprite_data = sprite_group[i];
								if(sprite_data != null && sprite_data.Name == animation_name)
								{
									mGroupIndex = ig;
									mGroupSpriteIndex = i;
									mStorageSpriteIndex = CubeXTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(ig, i);
									mCurrentTime = 0;
									mFrameCount = sprite_data.Count;
									mStart = true;
									mIsForward = true;

									if (mOnAnimationStart != null)
									{
										mOnAnimationStart(animation_name);
									}

									if (mWrapMode == TTweenWrapMode.OnceBackward)
									{
										mIsForward = false;
									}
								}
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			/// <param name="index">Индекс проигрываемой анимации</param>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation(Int32 index)
			{
				// Только если анимация не запущена
				if (mStart == false)
				{
					mGroupSpriteIndex = index;
					mStorageSpriteIndex = CubeXTweenDispatcher.SpriteStorage.GetStorageSpriteIndex(mGroupIndex, index);
					mStart = true;
					mIsForward = true;
					mCurrentTime = 0;
					mFrameCount = CubeXTweenDispatcher.SpriteStorage.GroupSprites[GroupIndex][index].Count;

					if (mOnAnimationStart != null)
					{
						mOnAnimationStart(this.GroupSpriteName);
					}

					if (mWrapMode == TTweenWrapMode.OnceBackward)
					{
						mIsForward = false;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				// Только если анимация не запущена
				if (mStart == false)
				{
					mStart = true;
					mIsForward = true;
					mCurrentTime = 0;
					CubeXTweenDispatcher.SpriteStorage.GetGroupIndexAndSpriteIndex(mStorageSpriteIndex, ref mGroupIndex, ref mGroupSpriteIndex);
					mFrameCount = CubeXTweenDispatcher.SpriteStorage.GroupSprites[GroupIndex][mGroupSpriteIndex].Count;

					if (mOnAnimationStart != null)
					{
						mOnAnimationStart(this.GroupSpriteName);
					}

					if (mWrapMode == TTweenWrapMode.OnceBackward)
					{
						mIsForward = false;
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateAnimation()
			{
				if (mStart && mIsPause == false)
				{
					// Прошла часть времени - прибавляем
					mCurrentTime += Time.deltaTime;

					if (mIsForward)
					{
						// Получаем значение на кривой по соответствующему нормализованному времени
						mNormalizeTime = mCurrentTime / mDuration;
					}
					else
					{
						// Получаем значение на кривой по соответствующему нормализованному времени
						mNormalizeTime = 1.0f - mCurrentTime / mDuration;
					}

					mFrameIndex = mStartFrame + (Int32)(mNormalizeTime * (mTargetFrame - mStartFrame) + 0.5f);
					mFrameIndex = Mathf.Clamp(mFrameIndex, 0, mFrameCount - 1);

					if (mCurrentTime >= mDuration)
					{
						switch (mWrapMode)
						{
							case TTweenWrapMode.Once:
								{
									mStart = false;
									mFrameIndex = mTargetFrame;

									if (mOnAnimationCompleted != null)
									{
										mOnAnimationCompleted(this.GroupSpriteName);
									}
								}
								break;
							case TTweenWrapMode.OnceBackward:
								{
									mStart = false;
									mFrameIndex = mStartFrame;

									if (mOnAnimationCompleted != null)
									{
										mOnAnimationCompleted(this.GroupSpriteName);
									}
								}
								break;
							case TTweenWrapMode.Loop:
								{
									mCurrentTime = 0;
									mFrameIndex = mStartFrame;
								}
								break;
							case TTweenWrapMode.PingPong:
								{
									if (mIsForward)
									{
										mIsForward = false;
										mFrameIndex = mTargetFrame;
										mCurrentTime = 0;
									}
									else
									{
										mStart = false;
										mFrameIndex = mStartFrame;

										if (mOnAnimationCompleted != null)
										{
											mOnAnimationCompleted(this.GroupSpriteName);
										}
									}
								}
								break;
							case TTweenWrapMode.LoopPingPong:
								{
									if (mIsForward)
									{
										mIsForward = false;
										mFrameIndex = mTargetFrame;
										mCurrentTime = 0;

									}
									else
									{
										mIsForward = true;
										mFrameIndex = mStartFrame;
										mCurrentTime = 0;
									}
								}
								break;
							default:
								break;
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Остановка анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopAnimation()
			{
				mCurrentTime = 0;
				mFrameIndex = mStartFrame;
				mStart = false;
				mIsForward = true;
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================