//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема анимации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXTweenSingle.cs
*		Компонент для хранения и управления анимацией значением вещественного типа.
*		Реализация компонента для хранения (используется хранилища анимационных кривых) и управления анимацией
*	значением вещественного типа.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 04.04.2021
//=====================================================================================================================
using System;
using UnityEngine;
//---------------------------------------------------------------------------------------------------------------------
using CubeX.Core;
//=====================================================================================================================
namespace CubeX
{
	namespace Common
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \addtogroup UnityCommonTween
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Компонент для хранения и управления анимацией значением вещественного типа
		/// </summary>
		/// <remarks>
		/// Реализация компонента для хранения (используется хранилища анимационных кривых) и управления анимацией
		/// значением вещественного типа
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		[AddComponentMenu("CubeX/Common/Tween/Animation Single")]
		public class CubeXTweenSingle : MonoBehaviour
		{
			#region ======================================= ДАННЫЕ ====================================================
			// Основные параметры
			[SerializeField]
			[CubeXDisplayName(nameof(TweenSingle))]
			[CubeXSerializeMember(nameof(TweenSingle))]
			internal CTweenSingle mTweenSingle;
			[SerializeField]
			[CubeXDisplayName(nameof(UseTransform))]
			[CubeXSerializeMember(nameof(UseTransform))]
			internal Boolean mUseTransform;
			[SerializeField]
			[CubeXDisplayName(nameof(Parameter))]
			[CubeXSerializeMember(nameof(Parameter))]
			internal TTweenTransformParameterType mParameter;
			[SerializeField]
			[CubeXDisplayName(nameof(ParameterComponent))]
			[CubeXSerializeMember(nameof(ParameterComponent))]
			internal TDimensionComponent mParameterComponent;
			[NonSerialized]
			internal Transform mThisTransform;
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Аниматор значения вещественного типа
			/// </summary>
			public CTweenSingle TweenSingle
			{
				get { return mTweenSingle; }
			}

			/// <summary>
			/// Текущее значение переменной
			/// </summary>
			public Single Value
			{
				get { return mTweenSingle.mValue; }
			}

			/// <summary>
			/// Начальное значение переменной
			/// </summary>
			public Single StartValue
			{
				get { return mTweenSingle.mStartValue; }
				set { mTweenSingle.mStartValue = value; }
			}

			/// <summary>
			/// Целевое значение переменной
			/// </summary>
			public Single TargetValue
			{
				get { return mTweenSingle.mTargetValue; }
				set { mTweenSingle.mTargetValue = value; }
			}

			/// <summary>
			/// Режим проигрывания анимации
			/// </summary>
			public TTweenWrapMode WrapMode
			{
				get { return mTweenSingle.mWrapMode; }
				set { mTweenSingle.mWrapMode = value; }
			}

			/// <summary>
			/// Время в течение которого должно измениться значение переменной от начального к конечной
			/// </summary>
			public Single Duration
			{
				get { return mTweenSingle.mDuration; }
				set { mTweenSingle.mDuration = value; }
			}

			/// <summary>
			/// Нормализованное время прохождение анимации в пределах от 0 до 1
			/// </summary>
			public Single NormalizeTime
			{
				get { return mTweenSingle.mNormalizeTime; }
			}

			/// <summary>
			/// Статус игнорирования масштабирования времени
			/// </summary>
			public Boolean IgnoreTimeScale
			{
				get { return mTweenSingle.IgnoreTimeScale; }
				set { mTweenSingle.IgnoreTimeScale = value; }
			}

			/// <summary>
			/// Количество циклов проигрывания циклических анимаций. 0 - бесконечно
			/// </summary>
			public Int32 CountLoop
			{
				get { return mTweenSingle.mCountLoop; }
				set { mTweenSingle.mCountLoop = value; }
			}

			/// <summary>
			/// Текущие количество проигрывания циклических анимаций
			/// </summary>
			public Int32 CurrentCountLoop
			{
				get { return mTweenSingle.mCurrentCountLoop; }
				set { mTweenSingle.mCurrentCountLoop = value; }
			}

			/// <summary>
			/// Статус применения изменяемого значения к текущему объекту трансформации
			/// </summary>
			public Boolean UseTransform
			{
				get { return mUseTransform; }
				set { mUseTransform = value; }
			}

			/// <summary>
			/// Параметр трансформации к которому применяется изменяемое значение
			/// </summary>
			public TTweenTransformParameterType Parameter
			{
				get { return mParameter; }
				set { mParameter = value; }
			}

			/// <summary>
			/// Компонент трансформации – конкретизация параметра трансформации
			/// </summary>
			public TDimensionComponent ParameterComponent
			{
				get { return mParameterComponent; }
				set { mParameterComponent = value; }
			}

			/// <summary>
			/// Статус анимации
			/// </summary>
			public Boolean IsPlay
			{
				get { return mTweenSingle.mStart; }
			}
			
			/// <summary>
			/// Установка/снятие паузы анимации
			/// </summary>
			public Boolean IsPause
			{
				get { return mTweenSingle.mIsPause; }
				set { mTweenSingle.mIsPause = value; }
			}

			/// <summary>
			/// Статус проигрывания анимации вперед
			/// </summary>
			public Boolean IsForward
			{
				get { return mTweenSingle.mIsForward; }
			}

			/// <summary>
			/// Событие для нотификации о начале анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationStart
			{
				get { return mTweenSingle.mOnAnimationStart; }
				set { mTweenSingle.mOnAnimationStart = value; }
			}

			/// <summary>
			/// Событие для нотификации об окончании анимации. Аргумент - название анимации
			/// </summary>
			public Action<String> OnAnimationCompleted
			{
				get { return mTweenSingle.mOnAnimationCompleted; }
				set { mTweenSingle.mOnAnimationCompleted = value; }
			}
			#endregion

			#region ======================================= СОБЫТИЯ UNITY =============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Инициализация скрипта при присоединении его к объекту(в режиме редактора)
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Reset()
			{
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{
				Init();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{
				mThisTransform = this.transform;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление скрипта каждый кадр
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Update()
			{
				if (mTweenSingle.IsPlay)
				{
					mTweenSingle.UpdateAnimation();
				}
				if (mUseTransform)
				{
					this.UpdateTransform();
				}
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Первичная инициализация данных и параметров компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Init()
			{
				if (mTweenSingle == null)
				{
					mTweenSingle = new CTweenSingle();
				}
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StartAnimation()
			{
				mTweenSingle.StartAnimation();
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Остановка анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void StopAnimation()
			{
				mTweenSingle.StopAnimation();
			}
			
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Обновление трансформации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void UpdateTransform()
			{
				switch (mParameter)
				{
					case TTweenTransformParameterType.Position:
						{
							switch (mParameterComponent)
							{
								case TDimensionComponent.X:
									{
										mThisTransform.position = new Vector3(mTweenSingle.Value, mThisTransform.position.y, mThisTransform.position.z);
									}
									break;
								case TDimensionComponent.Y:
									{
										mThisTransform.position = new Vector3(mThisTransform.position.x, mTweenSingle.Value, mThisTransform.position.z);
									}
									break;
								case TDimensionComponent.Z:
									{
										mThisTransform.position = new Vector3(mThisTransform.position.x, mThisTransform.position.y, mTweenSingle.Value);
									}
									break;
								default:
									break;
							}
						}
						break;
					case TTweenTransformParameterType.LocalPosition:
						{
							switch (mParameterComponent)
							{
								case TDimensionComponent.X:
									{
										mThisTransform.localPosition = new Vector3(mTweenSingle.Value, mThisTransform.localPosition.y, mThisTransform.localPosition.z);
									}
									break;
								case TDimensionComponent.Y:
									{
										mThisTransform.localPosition = new Vector3(mThisTransform.localPosition.x, mTweenSingle.Value, mThisTransform.localPosition.z);
									}
									break;
								case TDimensionComponent.Z:
									{
										mThisTransform.localPosition = new Vector3(mThisTransform.localPosition.x, mThisTransform.localPosition.y, mTweenSingle.Value);
									}
									break;
								default:
									break;
							}
						}
						break;
					case TTweenTransformParameterType.Rotation:
						{
							switch (mParameterComponent)
							{
								case TDimensionComponent.X:
									{
										mThisTransform.eulerAngles = new Vector3(mTweenSingle.Value, mThisTransform.eulerAngles.y, mThisTransform.eulerAngles.z);
									}
									break;
								case TDimensionComponent.Y:
									{
										mThisTransform.eulerAngles = new Vector3(mThisTransform.eulerAngles.x, mTweenSingle.Value, mThisTransform.eulerAngles.z);
									}
									break;
								case TDimensionComponent.Z:
									{
										mThisTransform.eulerAngles = new Vector3(mThisTransform.eulerAngles.x, mThisTransform.eulerAngles.y, mTweenSingle.Value);
									}
									break;
								default:
									break;
							}
						}
						break;
					case TTweenTransformParameterType.LocalRotation:
						{
							switch (mParameterComponent)
							{
								case TDimensionComponent.X:
									{
										mThisTransform.localEulerAngles = new Vector3(mTweenSingle.Value, mThisTransform.localEulerAngles.y, mThisTransform.localEulerAngles.z);
									}
									break;
								case TDimensionComponent.Y:
									{
										mThisTransform.localEulerAngles = new Vector3(mThisTransform.localEulerAngles.x, mTweenSingle.Value, mThisTransform.localEulerAngles.z);
									}
									break;
								case TDimensionComponent.Z:
									{
										mThisTransform.localEulerAngles = new Vector3(mThisTransform.localEulerAngles.x, mThisTransform.localEulerAngles.y, mTweenSingle.Value);
									}
									break;
								default:
									break;
							}
						}
						break;
					case TTweenTransformParameterType.Scale:
						{
							switch (mParameterComponent)
							{
								case TDimensionComponent.X:
									{
										mThisTransform.localScale = new Vector3(mTweenSingle.Value, mThisTransform.localScale.y, mThisTransform.localScale.z);
									}
									break;
								case TDimensionComponent.Y:
									{
										mThisTransform.localScale = new Vector3(mThisTransform.localScale.x, mTweenSingle.Value, mThisTransform.localScale.z);
									}
									break;
								case TDimensionComponent.Z:
									{
										mThisTransform.localScale = new Vector3(mThisTransform.localScale.x, mThisTransform.localScale.y, mTweenSingle.Value);
									}
									break;
								default:
									break;
							}
						}
						break;
					default:
						break;
				}
			}

#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Запуск анимации
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			[ContextMenu("Play")]
			[CubeXMethodCall("Play", 0, TMethodCallMode.OnlyPlay)]
			public void PlayInEditor()
			{
				StartAnimation();
			}
#endif
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================