﻿using System;
using System.Reflection;

namespace FubuMVC.StructureMap.Diagnostics
{
    public class SearchOption
    {
        public SearchOption()
        {
        }

        public SearchOption(Assembly assembly)
        {
            value = display = assembly.GetName().Name;
            type = "Assembly";
        }

        public static SearchOption ForNamespace(string ns)
        {
            return new SearchOption
            {
                type = "Namespace",
                value = ns,
                display = ns
            };
        }

        public static SearchOption ForReturnedType(Type returnedType)
        {
            return new SearchOption
            {
                type = "Returned-Type",
                value = returnedType.FullName,
                display = returnedType.Name
            };
        }

        public static SearchOption ForPluginType(Type pluginType)
        {
            return new SearchOption
            {
                type = "Plugin-Type",
                value = pluginType.FullName,
                display = pluginType.Name
            };
        }

        public string display;
        public string value;
        public string type;
    }
}