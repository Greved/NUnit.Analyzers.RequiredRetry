﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NUnit.Analyzers.RequiredRetry{
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("NUnit.Analyzers.RequiredRetry.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Test should have Retry attribute.
        /// </summary>
        public static string RequiredRetryAnalyzerDescription {
            get {
                return ResourceManager.GetString("RequiredRetryAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Test &apos;{0}&apos; doesn&apos;t have Retry attribute.
        /// </summary>
        public static string RequiredRetryAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("RequiredRetryAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Test should have Retry attribute.
        /// </summary>
        public static string RequiredRetryAnalyzerTitle {
            get {
                return ResourceManager.GetString("RequiredRetryAnalyzerTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Test should have Retry attribute with greater than &apos;1&apos; value.
        /// </summary>
        public static string RetryCountGreaterThatOneAnalyzerDescription {
            get {
                return ResourceManager.GetString("RetryCountGreaterThatOneAnalyzerDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Test &apos;{0}&apos; has Retry attribute with value below or equal &apos;1&apos;.
        /// </summary>
        public static string RetryCountGreaterThatOneAnalyzerMessageFormat {
            get {
                return ResourceManager.GetString("RetryCountGreaterThatOneAnalyzerMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Test should have Retry attribute with greater than &apos;1&apos; value.
        /// </summary>
        public static string RetryCountGreaterThatOneAnalyzerTitle {
            get {
                return ResourceManager.GetString("RetryCountGreaterThatOneAnalyzerTitle", resourceCulture);
            }
        }
    }
}
