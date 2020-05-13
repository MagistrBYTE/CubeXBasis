//=====================================================================================================================
// Проект: CubeXPlatform
// Раздел: Модуль общей функциональности
// Подраздел: Подсистема локализации
// Автор: MagistrBYTE aka DanielDem <dementevds@gmail.com>
//---------------------------------------------------------------------------------------------------------------------
/** \file CubeXLocalizationDispatcher.cs
*		Центральный диспетчер для локализации ресурсов.
*/
//---------------------------------------------------------------------------------------------------------------------
// Версия: 1.0.0.0
// Последнее изменение от 23.02.2020
//=====================================================================================================================
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
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
		//! \addtogroup UnityCommonLocalization
		/*@{*/
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Центральный диспетчер для локализации ресурсов
		/// </summary>
		//-------------------------------------------------------------------------------------------------------------
		[Serializable]
		public class CubeXLocalizationDispatcher : CubeXSystemSingleton<CubeXLocalizationDispatcher>, ISerializationCallbackReceiver,
			ICubeXSingleton, ICubeXMessageHandler
		{
			#region ======================================= КОНСТАНТНЫЕ ДАННЫЕ ========================================
			/// <summary>
			/// Начальный символ обрамления токена ключа
			/// </summary>
			public const Char SYMBOL_KEY_START = '[';

			/// <summary>
			/// Основной символ разделителя
			/// </summary>
			public const Char SYMBOL_KEY_DELIMETR = '#';

			/// <summary>
			/// Конечный символ обрамления токена ключа
			/// </summary>
			public const Char SYMBOL_KEY_END = ']';
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ СВОЙСТВА ======================================
			/// <summary>
			/// Глобальное событие для нотификации о смене языка. Аргумент - новый язык
			/// </summary>
			public static event Action<CLanguageInfo> OnLanguageChanged = delegate { };
			#endregion

			#region ======================================= СТАТИЧЕСКИЕ МЕТОДЫ ========================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смена языка
			/// </summary>
			/// <param name="language_name">Имя языка</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ChangedLanguage(String language_name)
			{
				Instance.ChangedLang(language_name);

				// Вызываем событие
				OnLanguageChanged(null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смена языка
			/// </summary>
			/// <param name="language_index">Индекс языка в списке</param>
			//---------------------------------------------------------------------------------------------------------
			public static void ChangedLanguage(Int32 language_index)
			{
				Instance.ChangedLang(mInstance.mLanguages[language_index].Name);

				// Вызываем событие
				OnLanguageChanged(null);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текстового ресурса по умолчанию по ключу локализации
			/// </summary>
			/// <param name="id">Ключ локализации текстового ресурса</param>
			/// <returns>Текстовый ресурс по умолчанию</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetTextDefaultFromID(Int64 id)
			{
				String result = "";
				if(Instance.mLocalizeDictionaryDefault.TryGetValue(id, out result))
				{
					return result;
				}

				return "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текстового ресурса по умолчанию по номеру (индексу)
			/// </summary>
			/// <param name="number">Последовательный номер текстового ресурса</param>
			/// <returns>Текстовый ресурс по умолчанию</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetTextDefaultFromNumber(Int32 number)
			{
				return Instance.mLocalizeListDefault[number];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение ключа локализации текстового ресурса по умолчанию
			/// </summary>
			/// <param name="text_default">Текстовый ресурс по умолчанию</param>
			/// <returns>Ключ локализации текстового ресурса или ноль если его нет</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int64 GetIDFromTextDefault(String text_default)
			{
				return Instance.mLocalizeDictionaryDefault.FirstOrDefault(pair => pair.Value == text_default).Key;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение последовательного номера (индекса) текстового ресурса по умолчанию
			/// </summary>
			/// <param name="text_default">Текстовый ресурс по умолчанию</param>
			/// <returns>Последовательный номер (индекс) текстового ресурса</returns>
			//---------------------------------------------------------------------------------------------------------
			public static Int32 GetNumberFromTextDefault(String text_default)
			{
				return Instance.mLocalizeListDefault.IndexOf(text_default);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текущего текстового ресурса по ключу локализации
			/// </summary>
			/// <param name="id">Ключ локализации текстового ресурса</param>
			/// <returns>Текущий текстовый ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetTextCurrentFromID(Int64 id)
			{
				String result;
				if (Instance.mLocalizeDictionaryCurrent.TryGetValue(id, out result))
				{
					return result;
				}

				return "";
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текущего текстового ресурса по ключу локализации
			/// </summary>
			/// <param name="id">Ключ локализации текстового ресурса</param>
			/// <param name="result">Текущий текстовый ресурс</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetTextCurrentFromID(Int64 id, out String result)
			{
				Instance.mLocalizeDictionaryCurrent.TryGetValue(id, out result);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текущего текстового ресурса по номеру (индексу)
			/// </summary>
			/// <param name="number">Последовательный номер текстового ресурса</param>
			/// <returns>Текущий текстовый ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetTextCurrentFromNumber(Int32 number)
			{
				return Instance.mLocalizeListCurrent[number];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текущего текстового ресурса по номеру (индексу)
			/// </summary>
			/// <param name="number">Последовательный номер текстового ресурса</param>
			/// <param name="result">Текущий текстовый ресурс</param>
			//---------------------------------------------------------------------------------------------------------
			public static void GetTextCurrentFromNumber(Int32 number, out String result)
			{
				result = Instance.mLocalizeListCurrent[number];
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текущего текстового ресурса по текстовому ресурсу по умолчанию
			/// </summary>
			/// <param name="text_default">Текстовый ресурс по умолчанию</param>
			/// <returns>Текущий текстовый ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public static String GetTextCurrentFromTextDefault(String text_default)
			{
				return Instance.GetLocalizeResource(text_default);
			}
			#endregion

			#region ======================================= ДАННЫЕ ====================================================
			// Системные параметры
			[SerializeField]
			internal Boolean mIsMainInstance;
			[SerializeField]
			internal TSingletonDestroyMode mDestroyMode;
			[SerializeField]
			internal Boolean mIsDontDestroy;
			[SerializeField]
			internal String mGroupMessage = "Localization";

			// Основные параметры
			[SerializeField]
			internal List<CLanguageInfo> mLanguages;
			[NonSerialized]
			internal Int32 mCurrentIndexLanguage;
			[NonSerialized]
			internal Dictionary<Int64, String> mLocalizeDictionaryDefault;
			[NonSerialized]
			internal Dictionary<Int64, String> mLocalizeDictionaryCurrent;
			[NonSerialized]
			internal List<String> mLocalizeListDefault;
			[NonSerialized]
			internal List<String> mLocalizeListCurrent;

			// Данные в режиме редактора
#if UNITY_EDITOR
			[SerializeField]
			internal Boolean mExpandedLanguages;
			[SerializeField]
			internal Boolean mExpandedTestTranslator;
			[SerializeField]
			internal Boolean mExpandedTranslation;
			[SerializeField]
			internal List<CTextTranslate> mListTranslate;
#endif
			#endregion

			#region ======================================= СВОЙСТВА ==================================================
			/// <summary>
			/// Язык текста по умолчанию
			/// </summary>
			public CLanguageInfo DefaultLanguage
			{
				get { return mLanguages[0]; }
			}

			/// <summary>
			/// Текущий язык
			/// </summary>
			public CLanguageInfo CurrentLanguage
			{
				get { return mLanguages[mCurrentIndexLanguage]; }
			}

			/// <summary>
			/// Индекс текущего языка
			/// </summary>
			public Int32 CurrentLanguageIndex
			{
				get { return mCurrentIndexLanguage; }
			}

			/// <summary>
			/// Список поддерживаемых языков
			/// </summary>
			public List<CLanguageInfo> Languages
			{
				get { return mLanguages; }
			}

			/// <summary>
			/// Словарь текстовых данных языка по умолчанию
			/// </summary>
			public Dictionary<Int64, String> LocalizeDictionaryDefault
			{
				get { return mLocalizeDictionaryDefault; }
			}

			/// <summary>
			/// Словарь текстовых данных текущего языка
			/// </summary>
			public Dictionary<Int64, String> LocalizeDictionaryCurrent
			{
				get { return mLocalizeDictionaryCurrent; }
			}

			/// <summary>
			/// Список текстовых данных языка по умолчанию
			/// </summary>
			public List<String> LocalizeListDefault
			{
				get { return mLocalizeListDefault; }
			}

			/// <summary>
			/// Список текстовых данных текущего языка
			/// </summary>
			public List<String> LocalizeListCurrent
			{
				get { return mLocalizeListCurrent; }
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXSingleton ==================================
			//
			// ОСНОВНЫЕ ПАРАМЕТРЫ
			//
			/// <summary>
			/// Статус основного экземпляра
			/// </summary>
			public Boolean IsMainInstance
			{
				get
				{
					return mIsMainInstance;
				}
				set
				{
					mIsMainInstance = value;
				}
			}

			/// <summary>
			/// Статус удаления игрового объекта
			/// </summary>
			/// <remarks>
			/// При дублировании будет удалятся либо непосредственного игровой объект либо только компонент
			/// </remarks>
			public TSingletonDestroyMode DestroyMode
			{
				get
				{
					return mDestroyMode;
				}
				set
				{
					mDestroyMode = value;
				}
			}

			/// <summary>
			/// Не удалять объект когда загружается новая сцена
			/// </summary>
			public Boolean IsDontDestroy
			{
				get
				{
					return mIsDontDestroy;
				}
				set
				{
					mIsDontDestroy = value;
				}
			}
			#endregion

			#region ======================================= СВОЙСТВА ICubeXMessageHandler =============================
			/// <summary>
			/// Группа которой принадлежит данный обработчик событий
			/// </summary>
			public String MessageHandlerGroup
			{
				get
				{
					return (mGroupMessage);
				}
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
				if (mLanguages == null)
				{
					mLanguages = new List<CLanguageInfo>
					{
						new CLanguageInfo(CLanguageInfo.LanguageNames[0], CLanguageInfo.LanguageAbbrs[0]),
						new CLanguageInfo(CLanguageInfo.LanguageNames[1], CLanguageInfo.LanguageAbbrs[1])
					};
				}

				if (mLocalizeListDefault == null)
				{
					// Создаем список
					mLocalizeListDefault = new List<String>();

					// Создаем словарь
					mLocalizeDictionaryDefault = new Dictionary<Int64, String>();

					// Загружаем данные
					if (DefaultLanguage.FileData == null)
					{
						Debug.LogErrorFormat("Default Language <{0}> not data", DefaultLanguage.Name);
						return;
					}

					// Загружаем данные
					List<KeyValuePair<String, String>> data = LoadLocalizeDataFromFile(DefaultLanguage.FileData);

					// Загружаем данные в список и словарь
					LoadData(data, ref mLocalizeDictionaryDefault, ref mLocalizeListDefault);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Псевдоконструктор скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Awake()
			{
				if (mLanguages == null)
				{
					mLanguages = new List<CLanguageInfo>
					{
						new CLanguageInfo(CLanguageInfo.LanguageNames[0], CLanguageInfo.LanguageAbbrs[0]),
						new CLanguageInfo(CLanguageInfo.LanguageNames[1], CLanguageInfo.LanguageAbbrs[1])
					};
				}

				if (mLocalizeListDefault == null)
				{
					// Создаем список
					mLocalizeListDefault = new List<String>();

					// Создаем словарь
					mLocalizeDictionaryDefault = new Dictionary<Int64, String>();

					// Загружаем данные
					if (DefaultLanguage.FileData == null)
					{
						Debug.LogErrorFormat("Default Language <{0}> not data", DefaultLanguage.Name);
						return;
					}

					// Загружаем данные
					List<KeyValuePair<String, String>> data = LoadLocalizeDataFromFile(DefaultLanguage.FileData);

					// Загружаем данные в список и словарь
					LoadData(data, ref mLocalizeDictionaryDefault, ref mLocalizeListDefault);
				}

				// Инициализация данных
				if (!CheckDublicate())
				{
	
				}

				if (mIsDontDestroy)
				{
					GameObject.DontDestroyOnLoad(this.gameObject);
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Включение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnEnable()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Старт скрипта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void Start()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Рисование UnityGUI
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnGUI()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Отключение компонента
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDisable()
			{
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Игровой объект уничтожился
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnDestroy()
			{
			}
			#endregion

			#region ======================================= МЕТОДЫ ISerializationCallbackReceiver =====================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс перед сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnBeforeSerialize()
			{

			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Процесс после сериализацией объекта
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void OnAfterDeserialize()
			{

			}
			#endregion

			#region ======================================= МЕТОДЫ ICubeXMessageHandler ===============================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Основной метод для обработки сообщения
			/// </summary>
			/// <param name="args">Аргументы сообщения</param>
			/// <returns>Статус успешности обработки сообщений</returns>
			//---------------------------------------------------------------------------------------------------------
			public Boolean OnMessageHandler(CMessageArgs args)
			{
				Boolean status = true;
				return (status);
			}
			#endregion

			#region ======================================= ОБЩИЕ МЕТОДЫ ==============================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение текущего текстового ресурса по текстовому ресурсу по умолчанию
			/// </summary>
			/// <param name="text">Текстовый ресурс по умолчанию</param>
			/// <returns>Текущий текстовый ресурс</returns>
			//---------------------------------------------------------------------------------------------------------
			public String GetLocalizeResource(String text)
			{
				if (mLocalizeListCurrent != null && mLocalizeListDefault != null)
				{
					// Ищем совпадение в предыдущем списке
					for (Int32 i = 0; i < mLocalizeListDefault.Count; i++)
					{
						String text_default = mLocalizeListDefault[i];

						// Символ перевода каретки убираем
						if(text.IndexOf(XChar.CarriageReturn) > -1)
						{
							text = text.Replace("\r", "");
						}

						if (String.CompareOrdinal(text_default, text) == 0)
						{
							return mLocalizeListCurrent[i];
						}
					}
				}

				return text;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Смена языка
			/// </summary>
			/// <param name="language_name">Имя языка</param>
			//---------------------------------------------------------------------------------------------------------
			public void ChangedLang(String language_name)
			{
				// Смена языка идет первый раз
				if (mLocalizeListCurrent == null)
				{
					mLocalizeListCurrent = new List<String>(mLocalizeListDefault.Count);
				}
				else
				{
					// Копируем с предыдущего
					mLocalizeListDefault.Clear();
					mLocalizeListDefault.AddRange(mLocalizeListCurrent);
					mLocalizeListCurrent.Clear();
				}

				if(mLocalizeDictionaryCurrent == null)
				{
					mLocalizeDictionaryCurrent = new Dictionary<Int64, String>();
				}
				else
				{
					// Копируем с предыдущего
					mLocalizeDictionaryDefault.Clear();

					foreach (Int32 key in mLocalizeDictionaryCurrent.Keys)
					{
						mLocalizeDictionaryDefault.Add(key, mLocalizeDictionaryCurrent[key]);
					}

					mLocalizeDictionaryCurrent.Clear();
				}

				// Ищем язык
				CLanguageInfo lang = FindLang(language_name);
				if (lang == null)
				{
					Debug.LogErrorFormat("Language <{0}> not found", language_name);
					return;
				}

				// Индекс языка
				mCurrentIndexLanguage = mLanguages.IndexOf(lang);

				// Загружаем данные
				if (lang.FileData == null)
				{
					Debug.LogErrorFormat("Language <{0}> not data", language_name);
					return;
				}

				List<KeyValuePair<String, String>>  data = LoadLocalizeDataFromFile(lang.FileData);

				if (data.Count != mLocalizeListDefault.Count + mLocalizeDictionaryDefault.Count)
				{
					Debug.LogErrorFormat("Language <{0}> data do not converge", language_name);
					return;
				}

				LoadData(data, ref mLocalizeDictionaryCurrent, ref mLocalizeListCurrent);
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Поиск языка для локализации
			/// </summary>
			/// <param name="language_name">Имя языка</param>
			/// <returns>Найденный язык или null</returns>
			//---------------------------------------------------------------------------------------------------------
			public CLanguageInfo FindLang(String language_name)
			{
				for (Int32 i = 0; i < mLanguages.Count; i++)
				{
					if(mLanguages[i].Name == language_name)
					{
						return mLanguages[i];
					}
				}

				return null;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Добавление языка для локализации
			/// </summary>
			/// <param name="language_name">Имя языка</param>
			//---------------------------------------------------------------------------------------------------------
			public void AddLang(String language_name)
			{
				mLanguages.Add(new CLanguageInfo(language_name, language_name.Substring(0, 2).ToLower()));
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Очистка списка языков
			/// </summary>
			//---------------------------------------------------------------------------------------------------------
			public void ClearLangs()
			{
				CLanguageInfo lang_default = DefaultLanguage;
				mLanguages.Clear();
				mLanguages.Add(lang_default);
			}
			#endregion

			#region ======================================= МЕТОДЫ ПОЛУЧЕНИЯ ДАННЫХ ===================================
			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение всех данных полей которые поддерживают локализацию
			/// </summary>
			/// <param name="fields">Массив полей</param>
			/// <param name="obj">Экземпляр типа</param>
			//---------------------------------------------------------------------------------------------------------
			internal void GetAllLocalizeDataFromFields(System.Reflection.FieldInfo[] fields, System.Object obj)
			{
				for (Int32 i = 0; i < fields.Length; i++)
				{
					// Если наш тип строковый и поддерживает локализацию
					if (fields[i].FieldType == typeof(String) && fields[i].HasAttribute<CubeXLocalizationAttribute>())
					{
						//1) Получаем текст
						String value = (String)fields[i].GetValue(obj);

						//2) Добавляем в базу
						mLocalizeListDefault.Add(value);
					}

					// Если наш тип прямо поддерживает локализацию
					if (fields[i].FieldType == typeof(TLocalizableText))
					{
						//1) Получаем данные
						TLocalizableText str_loc = (TLocalizableText)fields[i].GetValue(obj);

						//2) Получаем ключ по хэш коду
						Int32 key = str_loc.IDKeyLocalize;
						if (key != 0 && key != -1)
						{
							//3) Добавляем в базу
							if (!mLocalizeDictionaryDefault.ContainsKey(key))
							{
								mLocalizeDictionaryDefault.Add(key, str_loc.Text);
							}
						}
						else
						{
							// Только если объект участвует в локализации
							if (key != -1)
							{
								// Добавляем в список
								mLocalizeListDefault.Add(str_loc.Text);
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение всех данных свойств которые поддерживают локализацию
			/// </summary>
			/// <param name="properties">Массив полей</param>
			/// <param name="obj">Экземпляр типа</param>
			//---------------------------------------------------------------------------------------------------------
			internal void GetAllLocalizeDataFromProperties(System.Reflection.PropertyInfo[] properties, System.Object obj)
			{
				for (Int32 i = 0; i < properties.Length; i++)
				{
					// Если наш тип строковый и поддерживает локализацию
					if (properties[i].PropertyType == typeof(String) && properties[i].HasAttribute<CubeXLocalizationAttribute>())
					{
						//1) Получаем текст
						String value = (String)properties[i].GetValue(obj, null);

						//2) Добавляем в базу
						mLocalizeListDefault.Add(value);
					}

					// Если наш тип прямо поддерживает локализацию
					if (properties[i].PropertyType == typeof(TLocalizableText))
					{
						//1) Получаем данные
						TLocalizableText str_loc = (TLocalizableText)properties[i].GetValue(obj, null);

						//2) Получаем ключ по хэш коду
						Int32 key = str_loc.IDKeyLocalize;
						if (key != 0 && key != -1)
						{
							//3) Добавляем в базу
							if (!mLocalizeDictionaryDefault.ContainsKey(key))
							{
								mLocalizeDictionaryDefault.Add(key, str_loc.Text);
							}
						}
						else
						{
							// Только если объект участвует в локализации
							if (key != -1)
							{
								// Добавляем в список
								mLocalizeListDefault.Add(str_loc.Text);
							}
						}
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Получение всех данных которые поддерживают локализацию
			/// </summary>
			/// <remarks>
			/// Метод берет список всех компонентов производных от MonoBehaviour и:
			/// 1) Если компонент реализует интерфейс ICubeXLocalizable то добавляется в базу локализации
			/// 2) Если поле/свойство имеет строковый тип и атрибут CubeXLocalizationAttribute то добавляется в базу локализации
			/// 3) Если поле/свойство имеет тип TStringLocalizable то добавляется в базу локализации
			/// 4) Если поле/свойство имеет тип класса или структуры и его поля/свойства поддерживают п.2 или 3 то добавляется в базу локализации
			/// </remarks>
			//---------------------------------------------------------------------------------------------------------
			internal void GetAllLocalizeData()
			{
#if UNITY_EDITOR
				// Подготавливаем базу
				if (mLocalizeListDefault == null)
				{
					mLocalizeListDefault = new List<String>();
				}
				if (mLocalizeListDefault.Count > 0)
				{
					mLocalizeListDefault.Clear();
				}
				if (mLocalizeDictionaryDefault == null)
				{
					mLocalizeDictionaryDefault = new Dictionary<Int64, String>();
				}
				if (mLocalizeDictionaryDefault.Count > 0)
				{
					mLocalizeDictionaryDefault.Clear();
				}

				// Добавляем все элементы (в том числе и неактивные)
				MonoBehaviour[] elements = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
				for (Int32 i = 0; i < elements.Length; i++)
				{
					if (elements[i] != null)
					{
						// Отдельно обрабатываем интерфейс
						ICubeXLocalizable localizable = elements[i] as ICubeXLocalizable;
						if (localizable != null)
						{
							//1) Получаем текст
							String value = localizable.TextLocalize;

							//2) Получаем ключ по хэш коду
							Int32 key = localizable.IDKeyLocalize;
							if (key != 0 && key != -1)
							{
								//3) Добавляем в базу
								if (!mLocalizeDictionaryDefault.ContainsKey(key))
								{
									mLocalizeDictionaryDefault.Add(key, value);
								}
							}
							else
							{
								// Только если объект участвует в локализации
								if (key != -1)
								{
									// Добавляем в список
									mLocalizeListDefault.Add(value);
								}
							}
						}

						// Обрабатываем поля первого уровня
						System.Reflection.FieldInfo[] fields = null;// XReflection.GetSerializeData(elements[i].GetType()).Fields;
						GetAllLocalizeDataFromFields(fields, elements[i]);

						// Обрабатываем поля второго уровня
						for (Int32 f = 0; f < fields.Length; f++)
						{
							try
							{
								// Ссылки не обрабатываем
								if (fields[f].FieldType.IsSubclassOf(typeof(UnityEngine.Object)))
									continue;

								// Если наш тип сложный
								if ((fields[f].FieldType.IsClass || fields[f].FieldType.IsValueType)
									&& fields[f].FieldType.IsPrimitive == false
									&& fields[f].FieldType.IsEnum == false)
								{
									// 1) Объект родитель
									System.Object obj_parent = fields[f].GetValue(elements[i]);

									if (obj_parent != null)
									{
										System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

										// 2) Получаем поля
										System.Reflection.FieldInfo[] fields_two = fields[f].FieldType.GetFields(flags);

										// 3) Обрабатываем поля
										GetAllLocalizeDataFromFields(fields_two, obj_parent);

										// 4) Получаем свойства
										System.Reflection.PropertyInfo[] properties_two = fields[f].FieldType.GetProperties(flags);

										// 5) Обрабатываем свойства
										GetAllLocalizeDataFromProperties(properties_two, obj_parent);
									}
								}
							}
							catch (Exception exc)
							{
								Debug.LogException(exc);
							}
						}

						// Обрабатываем свойства первого уровня
						System.Reflection.PropertyInfo[] properties = null;// XReflection.GetSerializeData(elements[i].GetType()).Properties;
						GetAllLocalizeDataFromProperties(properties, elements[i]);

						// Обрабатываем свойства второго уровня
						for (Int32 p = 0; p < properties.Length; p++)
						{
							try
							{
								// Ссылки не обрабатываем
								if (properties[p].PropertyType.IsSubclassOf(typeof(UnityEngine.Object)))
									continue;

								// Если наш тип сложный
								if ((properties[p].PropertyType.IsClass || properties[p].PropertyType.IsValueType)
									&& properties[p].PropertyType.IsPrimitive == false
									&& properties[p].PropertyType.IsEnum == false)
								{
									// 1) Объект родитель
									System.Object obj_parent = properties[p].GetValue(elements[i], null);

									if (obj_parent != null)
									{
										System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

										// 2) Получаем поля
										System.Reflection.FieldInfo[] fields_two = properties[p].PropertyType.GetFields(flags);

										// 3) Обрабатываем поля
										GetAllLocalizeDataFromFields(fields_two, obj_parent);

										// 4) Получаем свойства
										System.Reflection.PropertyInfo[] properties_two = properties[p].PropertyType.GetProperties(flags);

										// 5) Обрабатываем свойства
										GetAllLocalizeDataFromProperties(properties_two, obj_parent);
									}
								}
							}
							catch (Exception exc)
							{
								Debug.LogException(exc);
							}
						}
					}
				}

				// Заполняем данные для оперативного перевода
				if (mListTranslate == null) mListTranslate = new List<CTextTranslate>();
				mListTranslate.Clear();

				if (mLocalizeDictionaryDefault != null)
				{
					// Отображаем данные сначала с ключами
					foreach (var item in mLocalizeDictionaryDefault.Keys)
					{
						CTextTranslate tt = new CTextTranslate();
						tt.IDKeyLocalize = (Int32)item;
						tt.Default = mLocalizeDictionaryDefault[item];
						mListTranslate.Add(tt);
					}
				}

				if (mLocalizeListDefault != null)
				{
					// Отображаем просто данные
					for (Int32 i = 0; i < mLocalizeListDefault.Count; i++)
					{
						CTextTranslate tt = new CTextTranslate();
						tt.Default = mLocalizeListDefault[i];
						mListTranslate.Add(tt);
					}
				}
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Создание файла локализации по умолчанию
			/// </summary>
			/// <param name="file_name">Имя файла локализации по умолчанию</param>
			//---------------------------------------------------------------------------------------------------------
			internal void CreateLocalizeFileDefault(String file_name)
			{
#if UNITY_EDITOR
				// Подготавливаем базу
				GetAllLocalizeData();

				// Создаем файл локализации по умолчанию
				StreamWriter writer = new StreamWriter(file_name);

				// 1) Сначала записываем текстовые данные с ключом локализации в формате ##[key]
				writer.WriteLine("///////////////////////////////////");
				writer.WriteLine("// Data from key");
				writer.WriteLine("///////////////////////////////////");
				foreach (var item in mLocalizeDictionaryDefault)
				{
					String text = item.Value;

					// Записываем данные через разделитель
					writer.Write(XString.SeparatorFileData);
					writer.Write(SYMBOL_KEY_START);
					writer.Write(item.Key.ToString());
					writer.WriteLine(SYMBOL_KEY_END);

					// Сами данные
					writer.WriteLine(text);
				}

				// 2) Потом записываем данные просто данные
				writer.WriteLine("///////////////////////////////////");
				writer.WriteLine("// Data no key");
				writer.WriteLine("///////////////////////////////////");
				for (Int32 i = 0; i < mLocalizeListDefault.Count; i++)
				{
					String text = mLocalizeListDefault[i];

					// Записываем данные через разделитель в формате
					writer.WriteLine(XString.SeparatorFileData);

					// Сами данные
					writer.WriteLine(text);
				}

				writer.Close();

				// Обновляем в редакторе
				UnityEditor.AssetDatabase.Refresh(UnityEditor.ImportAssetOptions.Default);
				UnityEditor.EditorUtility.DisplayDialog(XFileDialog.FILE_SAVE_SUCCESSFULLY, "Path\n" + file_name, "OK");

				DefaultLanguage.FileData = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(file_name);
#endif
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных локализации в указанный словарь и список
			/// </summary>
			/// <param name="data">Данные</param>
			/// <param name="dic">Словарь</param>
			/// <param name="list">Список</param>
			//---------------------------------------------------------------------------------------------------------
			internal void LoadData(List<KeyValuePair<String, String>> data, ref Dictionary<Int64, String> dic, ref List<String> list)
			{
				// Загружаем данные
				for (Int32 i = 0; i < data.Count; i++)
				{
					// Если это ключ - т.е. обрамлен символами []
					if (data[i].Key.IndexOf(SYMBOL_KEY_START) >= 0 && data[i].Key.IndexOf(SYMBOL_KEY_END) > 1)
					{
						// Читаем ключ
						String key_text = data[i].Key.Trim(SYMBOL_KEY_START, SYMBOL_KEY_END, SYMBOL_KEY_DELIMETR,
							XChar.NewLine, XChar.CarriageReturn, XChar.Space);
						Int64 key = Int64.Parse(key_text);

						// Добавляем в словарь
						dic.Add(key, data[i].Value);
					}
					else
					{
						list.Add(data[i].Value);
					}
				}
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Загрузка данных локализации
			/// </summary>
			/// <param name="text_assest">Текстовый файл локализации</param>
			/// <returns>Данные локализации</returns>
			//---------------------------------------------------------------------------------------------------------
			internal List<KeyValuePair<String, String>> LoadLocalizeDataFromFile(TextAsset text_assest)
			{
				// Разбиваем на токены
				String[] lines = text_assest.text.Split(XChar.NewLine);

				// Разбиваем на токены
				List<KeyValuePair<String, String>> data = XString.ConvertLinesToGroupLinesIgnoringComments(lines,
					XString.SeparatorFileData);

				return data;
			}

			//---------------------------------------------------------------------------------------------------------
			/// <summary>
			/// Сохранение переведённых данных в файл
			/// </summary>
			/// <param name="file_name">Имя файла</param>
			//---------------------------------------------------------------------------------------------------------
			internal void SaveTranslationData(String file_name)
			{
#if UNITY_EDITOR

#endif
			}
			#endregion
		}
		//-------------------------------------------------------------------------------------------------------------
		/*@}*/
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================