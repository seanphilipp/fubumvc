﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bottles.Diagnostics;
using FubuCore;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Runtime.Files;

namespace FubuMVC.Core.View.Model
{
    public abstract class ViewFacility<T> : IViewFacility, IFubuRegistryExtension where T : class, ITemplateFile
    {
        private List<T> _views = new List<T>();
        private ViewCollection<T> _bottle;

        public abstract Func<IFubuFile, T> CreateBuilder(SettingsCollection settings);

        public abstract FileSet FindMatching(SettingsCollection settings);

        public void Configure(FubuRegistry registry)
        {
            registry.AlterSettings<ViewEngineSettings>(x => x.AddFacility(this));
            registry.Services(registerServices);

            registry.AlterSettings<CommonViewNamespaces>(addNamespacesForViews);

            registry.AlterSettings<AssetSettings>(registerWatchSettings);
        }

        protected abstract void registerWatchSettings(AssetSettings settings);

        protected abstract void addNamespacesForViews(CommonViewNamespaces namespaces);

        protected abstract void registerServices(ServiceRegistry services);

        public virtual void Fill(ViewEngineSettings settings, BehaviorGraph graph, IPerfTimer timer)
        {
            var builder = CreateBuilder(graph.Settings);
            var match = FindMatching(graph.Settings);

            // HAS TO BE SHALLOW
            match.DeepSearch = false;

            _bottle = new ViewCollection<T>(this, graph.Files, builder, settings, match);


            LayoutAttachment = timer.RecordTask("Attaching Layouts for " + GetType().Name,
                () => AttachLayouts(settings));

            _views = _bottle.AllViews().ToList();
        }

        protected virtual void precompile(BehaviorGraph graph)
        {
            // do nothing
        }

        public Task LayoutAttachment { get; private set; }

        public void AttachViewModels(ViewTypePool types, ITemplateLogger logger)
        {
            _bottle.AttachViewModels(types, logger);
        }

        public abstract void ReadSharedNamespaces(CommonViewNamespaces namespaces);

        public void AttachLayouts(ViewEngineSettings settings)
        {
            _bottle.AttachLayouts(settings.ApplicationLayoutName);
        }

        public ViewEngineSettings Settings { get; set; }

        public Type TemplateType
        {
            get { return typeof (T); }
        }

        public IEnumerable<IViewToken> AllViews()
        {
            return _views.OfType<IViewToken>();
        }

        public IEnumerable<T> AllTemplates()
        {
            return _views;
        }

        public ITemplateFile FindInShared(string viewName)
        {
            return _bottle.FindInShared(viewName);
        }

        public T FindPartial(T template, string name)
        {
            var partialName = "_" + name;

            return AllViews().FirstOrDefault(x => x.Name() == partialName) as T;
        }

        public IEnumerable<string> SharedPaths()
        {
            return _bottle.SharedPaths();
        }
    }
}