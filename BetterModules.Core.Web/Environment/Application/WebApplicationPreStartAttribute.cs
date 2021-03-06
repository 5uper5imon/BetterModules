﻿using System;
using System.Reflection;

namespace BetterModules.Core.Web.Environment.Application
{
    /// <summary>
    /// Application assembly pre-start attribute
    /// Based on: https://github.com/davidebbo/WebActivator
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class WebApplicationPreStartAttribute : BaseActivationMethodAttribute
    {
        public WebApplicationPreStartAttribute(Type type, string methodName)
            : base(type, methodName)
        {
        }

        // Set this to true to have the method run in designer mode (in addition to running at runtime)
        public bool RunInDesigner { get; set; }

        public override bool ShouldRunInDesignerMode()
        {
            return RunInDesigner;
        }
    }

    // Base class of all the activation attributes
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class BaseActivationMethodAttribute : Attribute
    {
        private Type _type;
        private string _methodName;

        public BaseActivationMethodAttribute(Type type, string methodName)
        {
            _type = type;
            _methodName = methodName;
        }

        public Type Type
        {
            get
            {
                return _type;
            }
        }

        public string MethodName
        {
            get
            {
                return _methodName;
            }
        }

        public int Order { get; set; }


        public void InvokeMethod()
        {
            // Get the method
            MethodInfo method = Type.GetMethod(
                MethodName,
                BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

            if (method == null)
            {
                throw new ArgumentException(
                    String.Format("The type {0} doesn't have a static method named {1}",
                        Type, MethodName));
            }

            // Invoke it
            method.Invoke(null, null);
        }

        public virtual bool ShouldRunInDesignerMode()
        {
            return false;
        }
    }
}
