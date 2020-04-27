﻿using System;

namespace SUCC
{
    /// <summary>
    /// Private fields and properties with this attribute WILL be saved and loaded by SUCC.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class DoSaveAttribute : Attribute
    {
        public string Name { get; }

        public DoSaveAttribute()
        {
        }

        public DoSaveAttribute(string name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
