﻿using NVMP.Entities;
using System;
using System.Runtime.InteropServices;

namespace NVMP.Marshals
{
    internal class NetPlayerMarshaler : ICustomMarshaler
    {
        public static ICustomMarshaler Instance { get; } = new NetPlayerMarshaler();

        public static ICustomMarshaler GetInstance(string pstrCookie)
            => new NetPlayerMarshaler();

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            throw new NotImplementedException();
        }

        public int GetNativeDataSize()
        {
            return 0;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            var addr = (ManagedObj as NetPlayer).__UnmanagedAddress;
            if (addr == IntPtr.Zero)
                throw new Exception("Marshalling an object with a NULL unmanaged address!");

            return addr;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            if (pNativeData == IntPtr.Zero)
            {
                return null;
            }

            // we want to find the object allocated to the native data. 
            var managedHandle = NetReference.GetManagedHandleFromNativePointer(pNativeData);
            if (managedHandle == IntPtr.Zero)
                throw new Exception("No managed data was associated to native object. All native pointers used in managed environments should have a managed relationship.");

            // resolve the gchandle
            var gchandle = GCHandle.FromIntPtr(managedHandle);
            if (gchandle == null)
                throw new Exception("Marshal failure: managed handle was set, but is invalid!");

            // cast it up
            if (gchandle.Target == null)
                throw new Exception("Marshal failure: target reference is invalid!");

            return gchandle.Target as NetPlayer;
        }
    }
}
