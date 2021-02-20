﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Longship.Plugins;
using UnityEngine;

namespace Longship.Managers
{
    public class PluginManager : Manager
    {
        private readonly string _pluginsPath;
        private readonly Dictionary<Type, LoadedPlugin> _plugins = new Dictionary<Type, LoadedPlugin>();

        public PluginManager(string pluginsPath)
        {
            _pluginsPath = pluginsPath;
        }

        public override void Init()
        {
            _loadPlugins();
            _enablePlugins();
        }
        
        public T GetPlugin<T>() where T : IPlugin
        {
            if (_plugins.TryGetValue(typeof(T), out var value))
            {
                return (T) value.Plugin;
            }

            return null;
        }

        public bool DisablePlugin<T>() where T : IPlugin
        {
            if (_plugins.TryGetValue(typeof(T), out var value))
            {
                Longship.Log($"Disabling {value.Name}...");
                try
                {
                    Longship.Instance.ClearEventListeners(value.Plugin);
                    Longship.Instance.CommandsManager.ClearListeners(value.Plugin);
                    value.Plugin.OnDisable();
                }
                catch (Exception e)
                {
                    Longship.LogError($"Error while disabling plugin {value.Name}");
                    Longship.LogException(e);
                }
                Longship.Log($"{value.Name} disabled.");
            }

            return false;
        }

        private void _enablePlugins()
        {
            foreach (var entry in _plugins)
            {
                Longship.Log($"Enabling plugin {entry.Value.Name}...");
                try
                {
                    entry.Value.Plugin.OnEnable();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Can't enable plugin {entry.Value.Name}.");
                    Debug.LogException(e);
                }
            }
        }

        private void _loadPlugins()
        {
            var pluginsDirectory = new DirectoryInfo(_pluginsPath);

            if (!pluginsDirectory.Exists)
            {
                pluginsDirectory.Create();
            }
            
            foreach (var file in pluginsDirectory.GetFiles("*.dll"))
            {
                var assembly = Assembly.LoadFrom(file.FullName);
                foreach (var type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(typeof(IPlugin)) || type.IsAbstract) continue;
                    Longship.Log($"Loading plugin {file.Name}...");
                    try
                    {
                        _plugins[type] = new LoadedPlugin()
                        {
                            Plugin = type.InvokeMember(
                                null,
                                BindingFlags.CreateInstance,
                                null,
                                null,
                                null) as IPlugin,
                            Name = file.Name
                        };
                    }
                    catch (Exception e)
                    {
                        Longship.LogError($"Can't load plugin {file.Name}.");
                        Longship.LogException(e);
                    }
                }
            }
        }

        private struct LoadedPlugin
        {
            public IPlugin Plugin;
            public string Name;
        }
    }
}