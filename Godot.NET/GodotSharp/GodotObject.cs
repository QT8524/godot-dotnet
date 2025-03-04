﻿using System;
using System.Runtime.InteropServices;

namespace Godot
{
    /// <summary>Base class for all other classes in the engine.</summary>
    /// <seealso href="https://docs.godotengine.org/en/latest/classes/class_object.html"/>
    public partial class GodotObject : IDisposable, IEquatable<GodotObject>
    {
        public struct PropertyInfo
        {
            public string Name;
            public VariantType Type;
            public PropertyHint Hint;
            public string HintString;
        }

        internal static StringName GetClass(nint native)
            => StringDB.SearchOrCreate(MethodHelper.CallMethodBind<nint>(native, __vtable[0]));
        public static bool IsInstanceValid(GodotObject obj)
            => obj?._handle != nint.Zero;

        private GCHandle _bind;
        private nint _handle;
        private bool _disposed;
        private bool _ref;

        internal nint Binding { [MImpl(MImplOpts.AggressiveInlining)] get => (nint)_bind; }
        public nint Handle { [MImpl(MImplOpts.AggressiveInlining)] get => _handle; }
        public bool IsNull { [MImpl(MImplOpts.AggressiveInlining)] get => _bind.IsAllocated; }

        /// <inheritdoc cref="Dispose()"/>
        /// <param name="gc"><see langword="true"/> when called automatically by garbage collection, otherwise <see langword="false"/>.</param>
        protected virtual void Dispose(bool gc) { }

        private void InternalFree(bool gc)
        {
            _disposed = true;

            try
            {
                Dispose(gc);
            } catch (Exception e)
            {
                GD.PrintException(e);
            }

            if (_ref)
            {
                RefCounted r = this as RefCounted;
                r?.Unreference();
            }

            _bind.Free();

            _handle = nint.Zero;
        }

        /// <summary>Frees the managed instance of the <see cref="GodotObject"/> and related resources.</summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            GC.SuppressFinalize(this);
            InternalFree(false);
        }

        /// <inheritdoc cref="Dispose()"/>
        ~GodotObject()
        {
            if (!_disposed)
                Dispose(true);
        }

        internal void Initialize(GCHandle bind, nint native, bool counted)
        {
            _bind = bind;
            _handle = native;
            _ref = counted;

            if (_ref)
            {
                RefCounted r = this as RefCounted;
                r.Reference();
            }
        }

        public static explicit operator nint(GodotObject obj)
            => obj?._handle ?? nint.Zero;

        public static bool operator ==(GodotObject a, GodotObject b)
            => a?._handle == b?._handle;
        public static bool operator !=(GodotObject a, GodotObject b)
            => a?._handle != b?._handle;
        public bool Equals(GodotObject other)
            => _handle == other?._handle;
        public override bool Equals(object obj)
            => obj is GodotObject && Equals((GodotObject)obj);
        public override int GetHashCode()
            => _handle.GetHashCode();

        internal GodotObject(bool refcounted)
        {
            Type type = GetType();
            GCHandle bind = GCHandle.Alloc(this, refcounted ? GCHandleType.Weak : GCHandleType.Normal);
            Initialize(bind, ClassDB.MakeHandleForManaged(type, bind), refcounted);
        }

        public GodotObject() : this(false)
        {
        }

        // Ideally we wouldn't have these here, but reflection is also a pain...
        [MImpl(MImplOpts.AggressiveInlining)]
        internal nuint __gdext_PropertyCount()
            => _PropertyCount();
        [MImpl(MImplOpts.AggressiveInlining)]
        internal void __gdext_GetPropertyList(Span<PropertyInfo> info)
            => _GetPropertyList(info);
        [MImpl(MImplOpts.AggressiveInlining)]
        internal void __gdext_Call(StringName method, ref Variant arg0, ref Variant ret)
            => _Call(method, ref arg0, ref ret);
        [MImpl(MImplOpts.AggressiveInlining)]
        internal bool __gdext_Set(StringName property, Variant value)
            => _Set(property, value);
        [MImpl(MImplOpts.AggressiveInlining)]
        internal bool __gdext_Get(StringName property, ref Variant value)
            => _Get(property, ref value);
        [MImpl(MImplOpts.AggressiveInlining)]
        internal bool __gdext_ValidateProperty(ref PropertyInfo info)
            => _ValidateProperty(ref info);
        [MImpl(MImplOpts.AggressiveInlining)]
        internal bool __gdext_PropertyCanRevert(StringName property)
            => _PropertyCanRevert(property);
        [MImpl(MImplOpts.AggressiveInlining)]
        internal bool __gdext_PropertyGetRevert(StringName property, ref Variant ret)
            => _PropertyGetRevert(property, ref ret);

        protected virtual nuint _PropertyCount()
        {
            return 0;
        }
        protected virtual void _GetPropertyList(Span<PropertyInfo> info)
        {
        }
        protected virtual void _Call(StringName method, ref Variant arg0, ref Variant ret)
        {
        }
        protected virtual bool _Set(StringName property, Variant value)
        {
            return false;
        }
        protected virtual bool _Get(StringName property, ref Variant value)
        {
            return false;
        }
        protected virtual bool _ValidateProperty(ref PropertyInfo info)
        {
            return false;
        }
        protected virtual bool _PropertyCanRevert(StringName property)
        {
            return false;
        }
        protected virtual bool _PropertyGetRevert(StringName property, ref Variant ret)
        {
            return false;
        }
        public virtual bool _Notification(int what, bool reversed)
        {
            return false;
        }
    }
}
