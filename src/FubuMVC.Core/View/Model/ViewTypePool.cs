using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore;
using FubuMVC.Core.Registration;
using FubuMVC.Core.View.Registration;

namespace FubuMVC.Core.View.Model
{
    public class ViewTypePool
    {
        private readonly TypePool _types;

        public ViewTypePool(BehaviorGraph graph)
        {
            _types = graph.Types();
        }


        public Type FindTypeByName(string typeFullName, Action<string> log)
        {
            if (GenericParser.IsGeneric(typeFullName))
            {
                var genericParser = new GenericParser(_types.Assemblies);
                return genericParser.Parse(typeFullName);
            }

            return findClosedTypeByFullName(typeFullName, log);
        }

        private Type findClosedTypeByFullName(string typeFullName, Action<string> log)
        {
            var types = _types.TypesWithFullName(typeFullName);
            var typeCount = types.Count();

            if (typeCount == 1)
            {
                return types.First();
            }

            log("Unable to set view model type : {0}".ToFormat(typeFullName));

            if (typeCount > 1)
            {
                var candidates = types.Select(x => x.AssemblyQualifiedName).Join(", ");
                log("Type ambiguity on: {0}".ToFormat(candidates));
            }

            return null;
        }
    }
}