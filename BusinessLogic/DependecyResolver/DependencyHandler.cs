using Autofac;
using BusinessLogic.Library;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.DependecyResolver
{
    public class DependencyHandler : Module
    {
        //this one use for non static projects
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClientHelper>().As<IHttpClientHelper>().SingleInstance();
            builder.RegisterType<RegexHelper>().As<IRegexHelper>();
        }
        //for use in static projects
        public static IContainer Configure()
        {
            var Builder = new ContainerBuilder();
            Builder.RegisterType<HttpClientHelper>().As<IHttpClientHelper>().SingleInstance();
            Builder.RegisterType<RegexHelper>().As<IRegexHelper>();
            return Builder.Build();
        }
    }
}
