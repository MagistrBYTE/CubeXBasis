//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль базового ядра
// Подраздел: Подсистема поддержки инспектора свойств
// Группа: Атрибуты подтверждения правильности (валидации) данных
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXInspectorValidation.cs
*		Атрибуты подтверждения правильности (валидации) данных.
*		Реализация вспомогательных атрибутов которые определят валидность данных(соответствие их определённым требованиям).
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace CubeX
{
	namespace Core
	{
		//-------------------------------------------------------------------------------------------------------------
		//! \defgroup CoreModuleInspectorValidation Атрибуты валидации данных
		//! Атрибуты подтверждения правильности (валидации) данных
		//! \ingroup CoreModuleInspector
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Атрибут подтверждения правильности (валидации) данных
		/// </summary>
		/// <remarks>
		/// Для Unity, в случае не прохождения валидации данных, отображается бокс с информацией
		/// </remarks>
		//-------------------------------------------------------------------------------------------------------------
		[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
		public sealed class CubeXValidationAttribute : CubeXInspectorItemStyledAttribute
		{
			#region ======================================= ДАННЫЕ ====================================================
			internal String mValidationMethodName;
			internal String mMessage = "Data has not been validated";
			internal TLogType mMessageType;
#if UNITY_EDITOR
			private Boolean mIsValidationBox;
			private Single mValidationBoxHeight = XEditorInspector.CONTROL_HEIGHT * 2;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Имя метода который осуществляет проверку на валидацию данных
			/// </summary>
			/// <remarks>
			/// Метод должен иметь один аргумент соответствующего типа и возвращать true, если данные проходят 
			/// валидацию и false в противном случае 
			/// </remarks>
			public String ValidationMethodName
			{
				get { return mValidationMethodName; }
				set { mValidationMethodName = value; }
			}

			/// <summary>
			/// Сообщение которое отображается если данные не прошли валидацию
			/// </summary>
			public String Message
			{
				get { return mMessage; }
				set { mMessage = value; }
			}

			/// <summary>
			/// Тип сообщения
			/// </summary>
			public TLogType MessageType
			{
				get { return mMessageType; }
				set { mMessageType = value; }
			}
			#endregion

			#region ======================================= КОНСТРУКТОРЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="validation_method">Имя метода который осуществляет проверку на валидацию данных</param>
			/// <param name="message_type">Тип сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXValidationAttribute(String validation_method, TLogType message_type = TLogType.Error)
			{
				mValidationMethodName = validation_method;
				mMessageType = message_type;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конструктор инициализирует объект класса указанными параметрами
			/// </summary>
			/// <param name="validation_method">Имя метода который осуществляет проверку на валидацию данных</param>
			/// <param name="message">Сообщение которое отображается если данные не прошли валидацию</param>
			/// <param name="message_type">Тип сообщения</param>
			//---------------------------------------------------------------------------------------------------------
			public CubeXValidationAttribute(String validation_method, String message, TLogType message_type = TLogType.Error)
			{
				mValidationMethodName = validation_method;
				mMessage = message;
				mMessageType = message_type;
			}
			#endregion

			#region ======================================= МЕТОДЫ РЕДАКТОРА UNITY ====================================
#if UNITY_EDITOR
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение высоты элемента необходимого для работы этого атрибута
			/// </summary>
			/// <param name="label">Надпись</param>
			/// <returns>Высота</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Single GetHeight(UnityEngine.GUIContent label)
			{
				mIsValidationBox = false;
				if(mOwned != null)
				{
					// Получаем статус валидации от метода
					Boolean status = (Boolean)mOwned.SerializedProperty.Invoke(ValidationMethodName, mOwned.Value);

					// Если элемент не прошел валидацию то выводим сообщение
					if (status != true)
					{
						mIsValidationBox = true;
						return (GetHeightDefault(label) + mValidationBoxHeight);
					}
				}

				return (GetHeightDefault(label));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Применение действий перед отображением редактора элемента инспектора свойств
			/// </summary>
			/// <remarks>
			/// При необходимости можно менять входные параметры
			/// </remarks>
			/// <param name="position">Прямоугольник для отображения</param>
			/// <param name="label">Надпись</param>
			/// <returns>Следует ли рисовать редактор элемента инспектора свойств</returns>
			//---------------------------------------------------------------------------------------------------------
			public override Boolean BeforeApply(ref UnityEngine.Rect position, ref UnityEngine.GUIContent label)
			{
				if (mIsValidationBox)
				{
					var indent = UnityEditor.EditorGUI.IndentedRect(position);
					UnityEngine.Rect rect_box = indent;
					rect_box.y = indent.yMax - (mValidationBoxHeight - UnityEditor.EditorGUIUtility.standardVerticalSpacing);
					rect_box.height = mValidationBoxHeight;

					UnityEditor.EditorGUI.HelpBox(rect_box, mMessage, ConvertToMessageType(mMessageType));

					// Корректируем размеры для вывода основного свойства
					position.height -= mValidationBoxHeight;
				}

				return (true);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Конвертация из типа сообщения лога в тип сообщения
			/// </summary>
			/// <param name="log_type">Тип сообщения лога</param>
			/// <returns>Тип сообщения</returns>
			//---------------------------------------------------------------------------------------------------------
			public static UnityEditor.MessageType ConvertToMessageType(TLogType log_type)
			{
				switch (log_type)
				{
					case TLogType.Info: return UnityEditor.MessageType.Info;
					case TLogType.Warning: return UnityEditor.MessageType.Warning;
					case TLogType.Error: return UnityEditor.MessageType.Error;
					case TLogType.Exception: return UnityEditor.MessageType.Error;
					default:
						break;
				}

				return (UnityEditor.MessageType.None);
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