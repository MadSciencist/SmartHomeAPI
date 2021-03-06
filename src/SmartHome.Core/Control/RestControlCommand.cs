﻿using Autofac;
using SmartHome.Core.Control;
using SmartHome.Core.Entities.Entity;
using SmartHome.Core.RestClient;

namespace SmartHome.Core.Abstractions
{
    public abstract class RestControlCommand : ControlCommandBase
    {
        protected RestControlCommand(ILifetimeScope container, Node node) : base(container, node)
        {
        }

        private PersistentHttpClient _httpClient;
        protected PersistentHttpClient HttpClient =>
            _httpClient ?? (_httpClient = Container.Resolve<PersistentHttpClient>());
    }
}
