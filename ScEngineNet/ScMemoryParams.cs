﻿using ScEngineNet.Native;

namespace ScMemoryNet
{
    /// <summary>
    /// Параметры памяти при инициализации
    /// </summary>
   public class ScMemoryParams
    {
       internal WScMemoryParams scParams;
       private readonly string netExtensionsPath;

       /// <summary>
       /// Возвращает путь к расширениям
       /// </summary>
       /// <value>
       /// Путь к расширениям
       /// </value>
       public string ExtensionsPath
       {
           get { return scParams.ExtensionsPath; }
       }

       /// <summary>
       /// Возвращает путь к конфигурационному файлу
       /// </summary>
       /// <value>
       /// Путь к конфигурационному файлу
       /// </value>
       public string ConfigFile
       {
           get { return scParams.ConfigFile; }
       }

       /// <summary>
       /// Возвращает путь к репозиторию
       /// </summary>
       /// <value>
       /// Путь к репозиторию
       /// </value>
       public string RepoPath
       {
           get { return scParams.RepoPath; }
       }

       /// <summary>
       /// Возвращает путь к расширениям .net
       /// </summary>
       /// <value>
       /// Путь к расширениям .net
       /// </value>
       public string NetExtensionsPath
       {
           get { return netExtensionsPath; }
       }

       /// <summary>
       /// Создает новый экземпляр параметров <see cref="ScMemoryParams"/> class.
       /// </summary>
       /// <param name="clearBeforeInit">Если указано <c>true</c> память очистится перед инициализацией.</param>
       /// <param name="configFile">Путь к конфигурационному файлу</param>
       /// <param name="repoPath">Путь к репозиторию</param>
       /// <param name="extensionsPath">Путь к расширениям</param>
       /// <param name="netExtensionsPath">Путь к расширениям .net</param>
       public ScMemoryParams(bool clearBeforeInit, string configFile, string repoPath, string extensionsPath, string netExtensionsPath)
       {
           this.scParams = new WScMemoryParams() { Clear = clearBeforeInit, ConfigFile = configFile, RepoPath = repoPath, ExtensionsPath = extensionsPath };
           this.netExtensionsPath = netExtensionsPath;
       }
    }
}
