﻿using System;

namespace Godot
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExportDirAttribute : ExportAttribute
    {
        public ExportDirAttribute() : base(PropertyHint.Dir)
        {
        }
    }
}
