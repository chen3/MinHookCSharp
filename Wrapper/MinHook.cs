using ExtraConstraints;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace QiDiTu.Wrapper.MinHook
{
    public static class MinHook
    {
        /// <summary>
        /// MinHook Error Codes.
        /// </summary>
        public enum MH_STATUS
        {
            /// <summary>
            /// Unknown error. Should not be returned.
            /// </summary>
            MH_UNKNOWN = -1,

            /// <summary>
            /// Successful.
            /// </summary>
            MH_OK = 0,

            /// <summary>
            /// MinHook is already initialized.
            /// </summary>
            MH_ERROR_ALREADY_INITIALIZED,

            /// <summary>
            /// MinHook is not initialized yet, or already uninitialized.
            /// </summary>
            MH_ERROR_NOT_INITIALIZED,

            /// <summary>
            /// The hook for the specified target function is already created.
            /// </summary>
            MH_ERROR_ALREADY_CREATED,

            /// <summary>
            /// The hook for the specified target function is not created yet.
            /// </summary>
            MH_ERROR_NOT_CREATED,

            /// <summary>
            /// The hook for the specified target function is already enabled.
            /// </summary>
            MH_ERROR_ENABLED,

            /// <summary>
            /// The hook for the specified target function is not enabled yet, or already
            /// disabled.
            /// </summary>
            MH_ERROR_DISABLED,

            /// <summary>
            /// The specified pointer is invalid. It points the address of non-allocated
            /// and/or non-executable region.
            /// </summary>
            MH_ERROR_NOT_EXECUTABLE,

            /// <summary>
            /// The specified target function cannot be hooked.
            /// </summary>
            MH_ERROR_UNSUPPORTED_FUNCTION,

            /// <summary>
            /// Failed to allocate memory.
            /// </summary>
            MH_ERROR_MEMORY_ALLOC,

            /// <summary>
            /// Failed to change the memory protection.
            /// </summary>
            MH_ERROR_MEMORY_PROTECT,

            /// <summary>
            /// The specified module is not loaded.
            /// </summary>
            MH_ERROR_MODULE_NOT_FOUND,

            /// <summary>
            /// The specified function is not found.
            /// </summary>
            MH_ERROR_FUNCTION_NOT_FOUND
        };

        /// <summary>
        /// Initialize the MinHook library. You must call this function EXACTLY ONCE
        /// at the beginning of your program.
        /// </summary>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_Initialize")]
        public static extern MH_STATUS MH_Initialize();

        /// <summary>
        /// Uninitialize the MinHook library. You must call this function EXACTLY
        /// ONCE at the end of your program.
        /// </summary>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_Uninitialize")]
        public static extern MH_STATUS MH_Uninitialize();

        /// <summary>
        /// Enables an already created hook.
        /// </summary>
        /// <param name="target">Target function pointer. If this parameter is
        /// <code>IntPtr.Zero</code>, all created hooks are enabled in one go.
        /// </param>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_EnableHook")]
        public static extern MH_STATUS MH_EnableHook(IntPtr target);

        /// <summary>
        /// Disables an already created hook.
        /// </summary>
        /// <param name="target">Target function pointer.If this parameter is
        /// IntPtr.Zero, all created hooks are disabled in one go.
        /// </param>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_DisableHook")]
        public static extern MH_STATUS MH_DisableHook(IntPtr target);

        /// <summary>
        /// Queues to enable an already created hook.
        /// </summary>
        /// <param name="target">Target function pointer. If this parameter is
        /// <code>IntPtr.Zero</code>, all created hooks are queued to be enabled.
        /// </param>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_QueueEnableHook")]
        public static extern MH_STATUS MH_QueueEnableHook(IntPtr target);

        /// <summary>
        /// Queues to disable an already created hook.
        /// </summary>
        /// <param name="target">Target function pointer. If this parameter is
        /// <code>IntPtr.Zero</code>, all created hooks are queued to be disabled.
        /// </param>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_QueueDisableHook")]
        public static extern MH_STATUS MH_QueueDisableHook(IntPtr target);

        /// <summary>
        /// Applies all queued changes in one go.
        /// </summary>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_ApplyQueued")]
        public static extern MH_STATUS MH_ApplyQueued();

        /// <summary>
        /// Creates a Hook for the specified target function, in disabled state.
        /// </summary>
        /// <param name="target">Target function pointer, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="pOriginal">A trampoline function variant pointer, which will be
        /// set the function variant to the original target function.
        /// This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <see cref="MH_CreateHook(IntPtr, IntPtr, out IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHook")]
        public static extern MH_STATUS MH_CreateHook(IntPtr target, IntPtr detour,
                                                    IntPtr pOriginal);

        /// <summary>
        /// Creates a Hook for the specified target function, in disabled state.
        /// </summary>
        /// <param name="target">Target function pointer, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline function pointer, which will be
        /// used to call the original target function.
        /// </param>
        /// <see cref="MH_CreateHook(IntPtr, IntPtr, IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHook")]
        public static extern MH_STATUS MH_CreateHook(IntPtr target, IntPtr detour,
                                                    out IntPtr original);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="pOriginal">A trampoline function variant pointer, which will be
        /// set the function variant to the original target function. 
        /// This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <see cref="MH_CreateHookApi(string, string, IntPtr, out IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHookApi")]
        public static extern MH_STATUS
        MH_CreateHookApi([MarshalAs(UnmanagedType.LPWStr)]string moduleName,
                        [MarshalAs(UnmanagedType.LPStr)]string procName,
                        IntPtr detour, IntPtr pOriginal);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline function pointer, which will be
        /// used to call the original target function.
        /// </param>
        /// <see cref="MH_CreateHookApi(string, string, IntPtr, IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHookApi")]
        public static extern MH_STATUS
        MH_CreateHookApi([MarshalAs(UnmanagedType.LPWStr)]string moduleName,
                        [MarshalAs(UnmanagedType.LPStr)]string procName,
                        IntPtr detour, out IntPtr original);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline function pointer, which will be
        /// used to call the original target function. This parameter can be null.
        /// </param>
        /// <param name="pTarget">A target function variant pointer, which will be 
        /// set the function variant to the other functions. 
        /// This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <see cref="MH_CreateHookApi(string, string, IntPtr, out IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, IntPtr, out IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, IntPtr, IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, out IntPtr, out IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHookApiEx")]
        public static extern MH_STATUS
        MH_CreateHookApiEx([MarshalAs(UnmanagedType.LPWStr)]string moduleName,
                        [MarshalAs(UnmanagedType.LPStr)]string procName,
                        IntPtr detour, out IntPtr original, IntPtr pTarget);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="pOriginal">A trampoline function variant pointer, which will be
        /// set the function variant to the original target function.
        /// This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <param name="pTarget">A target function variant pointer, which will be 
        /// set the function variant to the other functions. 
        /// This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <see cref="MH_CreateHookApi(string, string, IntPtr, IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, IntPtr, out IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, out IntPtr, IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, out IntPtr, out IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHookApiEx")]
        public static extern MH_STATUS
        MH_CreateHookApiEx([MarshalAs(UnmanagedType.LPWStr)]string moduleName,
                        [MarshalAs(UnmanagedType.LPStr)]string procName,
                        IntPtr detour, IntPtr pOriginal, IntPtr pTarget);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="pOriginal">A trampoline function variant pointer, which will be
        /// set the function variant to the original target function.
        /// This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <param name="target">Target function pointer, which will be used with other
        /// functions. This parameter shoule be <code>IntPtr.Zero</code>.
        /// </param>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, IntPtr, IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, out IntPtr, IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, out IntPtr, out IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHookApiEx")]
        public static extern MH_STATUS
        MH_CreateHookApiEx([MarshalAs(UnmanagedType.LPWStr)]string moduleName,
                        [MarshalAs(UnmanagedType.LPStr)]string procName,
                        IntPtr detour, IntPtr pOriginal, out IntPtr target);

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour function pointer, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline function pointer, which will be
        /// used to call the original target function.
        /// </param>
        /// <param name="target">Target function pointer, which will be used
        /// with other functions.
        /// </param>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, IntPtr, IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, IntPtr, out IntPtr)"/>
        /// <see cref="MH_CreateHookApiEx(string, string, IntPtr, out IntPtr, IntPtr)"/>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_CreateHookApiEx")]
        public static extern MH_STATUS
        MH_CreateHookApiEx([MarshalAs(UnmanagedType.LPWStr)]string moduleName,
                        [MarshalAs(UnmanagedType.LPStr)]string procName,
                        IntPtr detour, out IntPtr original, out IntPtr target);

        /// <summary>
        /// Removes an already created hook.
        /// </summary>
        /// <param name="target">A pointer to the target function.</param>
        /// <returns>status</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_RemoveHook")]
        public static extern MH_STATUS MH_RemoveHook(IntPtr target);

        /// <summary>
        /// Translates the MH_STATUS to its name as a string.
        /// </summary>
        /// <param name="status">status</param>
        /// <returns>status name</returns>
        [DllImport("MinHook.dll", EntryPoint = "MH_StatusToString")]
        [return: MarshalAs(UnmanagedType.LPStr)]
        public static extern string MH_StatusToString(MH_STATUS status);

    }

    public class MinHookException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the QiDiTu.Wrapper.MinHookException class.
        /// </summary>
        /// <param name="status">exception statu</param>
        public MinHookException(MinHook.MH_STATUS status = MinHook.MH_STATUS.MH_UNKNOWN)
        {
            Status = status;
        }

        /// <summary>
        /// Initializes a new instance of the QiDiTu.Wrapper.MinHookException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MinHookException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the QiDiTu.Wrapper.MinHookException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public MinHookException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public virtual MinHook.MH_STATUS Status
        {
            get;
            private set;
        }

    };

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_ALREADY_INITIALIZED"/>
    public class AlreadyInitializedException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.AlreadyInitializedException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AlreadyInitializedException(string message =
                                    "MinHook is already initialized.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.AlreadyInitializedException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public AlreadyInitializedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_ALREADY_INITIALIZED;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_NOT_INITIALIZED"/>
    public class NotInitializedException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.NotInitializedException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotInitializedException(string message =
                    "MinHook is not initialized yet, or already uninitialized.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.NotInitializedException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public NotInitializedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_NOT_INITIALIZED;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_ALREADY_CREATED"/>
    public class AleradyCreatedException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.AleradyCreatedException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public AleradyCreatedException(string message =
                    "The hook for the specified target function is already created.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.AleradyCreatedException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public AleradyCreatedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_ALREADY_CREATED;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_NOT_CREATED"/>
    public class TargetNotCreatedException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.TargetNotCreatedException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TargetNotCreatedException(string message =
                    "The hook for the specified target function is not created yet.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.TargetNotCreatedException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public TargetNotCreatedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_NOT_CREATED;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_ENABLED"/>
    public class EnabledException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.EnabledException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public EnabledException(string message =
                    "The hook for the specified target function is already enabled.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.EnabledException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public EnabledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_ENABLED;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_DISABLED"/>
    public class DisabledException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.DisabledException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DisabledException(string message =
                            "The hook for the specified target function is not" +
                            " enabled yet, or already disabled.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.DisabledException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public DisabledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_DISABLED;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_NOT_EXECUTABLE"/>
    public class NotExecutableException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.NotExecutableException class with 
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public NotExecutableException(string message =
                    "The specified pointer is invalid. It points the address" +
                    " of non-allocated  and/or non-executable region.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.NotExecutableException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public NotExecutableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_NOT_EXECUTABLE;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_UNSUPPORTED_FUNCTION"/>
    public class UnsupportedFunctionException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.UnsupportedFunctionException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public UnsupportedFunctionException(string message =
                                "The specified target function cannot be hooked.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.UnsupportedFunctionException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public UnsupportedFunctionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_UNSUPPORTED_FUNCTION;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_MEMORY_ALLOC"/>
    public class MemoryAllocException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.MemoryAllocException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MemoryAllocException(string message = "Failed to allocate memory.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.MemoryAllocException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public MemoryAllocException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_MEMORY_ALLOC;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_MEMORY_PROTECT"/>
    public class MemoryProtectException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.MemoryProtectException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MemoryProtectException(string message =
                            "Failed to change the memory protection.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.MemoryProtectException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public MemoryProtectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_MEMORY_PROTECT;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_MODULE_NOT_FOUND"/>
    public class ModuleNotFoundException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.ModuleNotFoundException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ModuleNotFoundException(string message =
                                "The specified module is not loaded.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.ModuleNotFoundException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public ModuleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_MODULE_NOT_FOUND;
            }
        }
    }

    /// <see cref="MinHook.MH_STATUS.MH_ERROR_FUNCTION_NOT_FOUND"/>
    public class FunctionNotFoundException : MinHookException
    {
        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.FunctionNotFoundException class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public FunctionNotFoundException(string message =
                                "The specified function is not found.")
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the 
        /// QiDiTu.Wrapper.FunctionNotFoundException class with
        /// a specified error message and a reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the
        /// exception.
        /// </param>
        /// <param name="innerException">The exception that is the cause of the current
        /// exception, or a null reference (Nothing in Visual Basic) if no inner
        /// exception is specified.
        /// </param>
        public FunctionNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Get the status for the exception
        /// </summary>
        public override MinHook.MH_STATUS Status
        {
            get
            {
                return MinHook.MH_STATUS.MH_ERROR_FUNCTION_NOT_FOUND;
            }
        }
    }

    public static class Helper
    {
        /// <summary>
        /// Retrieves the address of an exported function or variable from the specified
        /// dynamic-link library (DLL).
        /// </summary>
        /// <param name="hModule">A handle to the DLL module that contains the function
        /// or variable.
        /// </param>
        /// <param name="procName">The function or variable name, or the function's
        /// ordinal value. If this parameter is an ordinal value, it must be in the
        /// low-order word; the high-order word must be zero.
        /// </param>
        /// <returns>If the function succeeds, the return value is the address of the
        /// exported function or variable. If the function fails, the return value is
        /// IntPtr.Zero. To get extended error information,
        /// call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true,
                    SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        /// <summary>
        /// Initialize the MinHook library. You must call this function EXACTLY ONCE
        /// at the beginning of your program.
        /// </summary>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_Initialize"/>
        public static void initialize()
        {
            MinHook.MH_STATUS status = MinHook.MH_Initialize();
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Uninitialize the MinHook library. You must call this function EXACTLY
        /// ONCE at the end of your program.
        /// </summary>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_Uninitialize"/>
        public static void uninitialize()
        {
            MinHook.MH_STATUS status = MinHook.MH_Uninitialize();
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Enables an already created hook.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name</param>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_EnableHook(IntPtr)"/>
        public static void enableHook(string moduleName, string procName)
        {
            IntPtr address = getProcAddress(moduleName, procName);
            MinHook.MH_STATUS status = MinHook.MH_EnableHook(address);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Enable all created hooks in one go.
        /// </summary>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_EnableHook(IntPtr)"/>
        public static void enableAllHook()
        {
            MinHook.MH_STATUS status = MinHook.MH_EnableHook(IntPtr.Zero);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Disables an already created hook.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name</param>
        /// <returns>status</returns>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_DisableHook(IntPtr)"/>
        public static void disableHook(string moduleName, string procName)
        {
            IntPtr address = getProcAddress(moduleName, procName);
            MinHook.MH_STATUS status = MinHook.MH_DisableHook(address);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Disable all created hooks in one go.
        /// </summary>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_DisableHook(IntPtr)"/>
        public static void disnableAllHook()
        {
            MinHook.MH_STATUS status = MinHook.MH_DisableHook(IntPtr.Zero);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Creates a Hook for the specified target function, in disabled state.
        /// </summary>
        /// <param name="target">Target function, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour delegate, which will override
        /// the target function.
        /// </param>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void createHook<[DelegateConstraint] T>(IntPtr pTarget, T detour)
        {
            IntPtr ptr = Marshal.GetFunctionPointerForDelegate(detour);
            MinHook.MH_STATUS status = MinHook.MH_CreateHook(pTarget, ptr, IntPtr.Zero);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Creates a Hook for the specified target function, in disabled state.
        /// </summary>
        /// <param name="target">Target function, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour delegate, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline delegate, which will be
        /// used to call the original target function.
        /// </param>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void createHook<[DelegateConstraint] T>(IntPtr pTarget, T detour,
                                                            out T original)
        {
            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = Marshal.GetFunctionPointerForDelegate(detour);
            MinHook.MH_STATUS status = MinHook.MH_CreateHook(pTarget, ptr2, out ptr1);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
            original = Marshal.GetDelegateForFunctionPointer<T>(ptr1);
        }

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour delegate, which will override
        /// the target function.
        /// </param>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void createHookApi<[DelegateConstraint] T>(string moduleName,
                                                        string procName, T detour)
        {
            IntPtr ptr = Marshal.GetFunctionPointerForDelegate(detour);
            MinHook.MH_STATUS status = MinHook.MH_CreateHookApi(moduleName, procName,
                                                                ptr, IntPtr.Zero);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour delegate, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline delegate, which will be
        /// used to call the original target function.
        /// </param>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void createHookApi<[DelegateConstraint] T>(string moduleName,
                                string procName, T detour, out T original)
        {
            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = Marshal.GetFunctionPointerForDelegate(detour);
            MinHook.MH_STATUS status = MinHook.MH_CreateHookApi(moduleName, procName,
                                                                ptr2, out ptr1);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
            original = Marshal.GetDelegateForFunctionPointer<T>(ptr1);
        }

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour delegate, which will override
        /// the target function.
        /// </param>
        /// <param name="target">Target delegate, which will be used
        /// with other functions.
        /// </param>
        /// <see cref="MH_CreateHookApiEx{T}(string, string, T, ref T, ref T)"/>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void createHookApiEx<[DelegateConstraint] T>(string moduleName,
                                            string procName, T detour, out T target)
        {
            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = Marshal.GetFunctionPointerForDelegate(detour);
            MinHook.MH_STATUS status = MinHook.MH_CreateHookApiEx(moduleName, procName,
                                                            ptr2, IntPtr.Zero, out ptr1);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
            target = ptr1 == IntPtr.Zero ? default(T) :
                                    Marshal.GetDelegateForFunctionPointer<T>(ptr1);
        }

        /// <summary>
        /// Creates a Hook for the specified API function, in disabled state.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name, which will be
        /// overridden by the detour function.
        /// </param>
        /// <param name="detour">Detour delegate, which will override
        /// the target function.
        /// </param>
        /// <param name="original">Trampoline delegate, which will be
        /// used to call the original target function.
        /// </param>
        /// <param name="target">Target delegate, which will be used
        /// with other functions.
        /// </param>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void createHookApiEx<[DelegateConstraint] T>(string moduleName,
                                string procName, T detour, out T original, out T target)
        {
            IntPtr ptr1 = IntPtr.Zero;
            IntPtr ptr2 = IntPtr.Zero;
            IntPtr ptr3 = Marshal.GetFunctionPointerForDelegate(detour);
            MinHook.MH_STATUS status = MinHook.MH_CreateHookApiEx(moduleName, procName,
                                                            ptr3, out ptr1, out ptr2);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
            original = ptr1 == IntPtr.Zero ? default(T) :
                                    Marshal.GetDelegateForFunctionPointer<T>(ptr1);
            target = ptr2 == IntPtr.Zero ? default(T) :
                                    Marshal.GetDelegateForFunctionPointer<T>(ptr2);
        }

        /// <summary>
        /// Queues to enable an already created hook.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name</param>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_QueueEnableHook(IntPtr)"/>
        public static void queueEnableHook(string moduleName, string procName)
        {
            IntPtr address = getProcAddress(moduleName, procName);
            MinHook.MH_STATUS status = MinHook.MH_QueueEnableHook(address);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Queues to enable an already all created hook.
        /// </summary>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_QueueEnableHook(IntPtr)"/>
        public static void queueEnableAllHook()
        {
            MinHook.MH_STATUS status = MinHook.MH_QueueEnableHook(IntPtr.Zero);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Queues to disable an already created hook.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name</param>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_QueueDisableHook(IntPtr)"/>
        public static void queueDisableHook(string moduleName, string procName)
        {
            IntPtr address = getProcAddress(moduleName, procName);
            MinHook.MH_STATUS status = MinHook.MH_QueueDisableHook(address);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Queues to disable an already all created hook.
        /// </summary>
        /// <exception cref="MinHookException">include all subclass</exception>
        /// <see cref="MinHook.MH_QueueDisableHook(IntPtr)"/>
        public static void queueDisableAllHook()
        {
            MinHook.MH_STATUS status = MinHook.MH_QueueDisableHook(IntPtr.Zero);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Removes an already created hook.
        /// </summary>
        /// <param name="moduleName">Loaded module name which contains the
        /// target function.
        /// </param>
        /// <param name="procName">Target function name</param>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <exception cref="MinHookException">include all subclass</exception>
        public static void removeHook(string moduleName, string procName)
        {
            IntPtr address = getProcAddress(moduleName, procName);
            MinHook.MH_STATUS status = MinHook.MH_RemoveHook(address);
            if (status != MinHook.MH_STATUS.MH_OK)
            {
                throw getExceptionByStatu(status);
            }
        }

        /// <summary>
        /// Get module IntPtr by name
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <returns>module base address</returns>
        private static IntPtr getMoudle(string moduleName)
        {
            string name = moduleName.ToLower();
            if (!name.EndsWith(".dll"))
            {
                name += ".dll";
            }
            foreach (ProcessModule module in Process.GetCurrentProcess().Modules)
            {
                if (module.ModuleName.ToLower() == name)
                {
                    return module.BaseAddress;
                }
            }
            throw new ModuleNotFoundException();
        }

        /// <summary>
        /// Get function IntPtr by name
        /// </summary>
        /// <param name="module">moudle pointer</param>
        /// <param name="procName">function name</param>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <returns>function pointer</returns>
        private static IntPtr getProcAddress(IntPtr module, string procName)
        {
            IntPtr ptr = GetProcAddress(module, procName);
            if (ptr == IntPtr.Zero)
            {
                throw new FunctionNotFoundException();
            }
            return ptr;
        }

        /// <summary>
        /// Get function IntPtr by name
        /// </summary>
        /// <param name="moduleName">module name</param>
        /// <param name="procName">function name</param>
        /// <exception cref="FunctionNotFoundException"></exception>
        /// <exception cref="ModuleNotFoundException"></exception>
        /// <returns>function pointer</returns>
        private static IntPtr getProcAddress(string moduleName, string procName)
        {
            return getProcAddress(getMoudle(moduleName), procName);
        }

        /// <summary>
        /// convert minhook status to exception
        /// </summary>
        /// <param name="statu">MinHook exception statu</param>
        /// <returns></returns>
        private static MinHookException getExceptionByStatu(MinHook.MH_STATUS statu)
        {
            switch (statu)
            {
                case MinHook.MH_STATUS.MH_ERROR_ALREADY_CREATED:
                {
                    return new AleradyCreatedException();
                }
                case MinHook.MH_STATUS.MH_ERROR_ALREADY_INITIALIZED:
                {
                    return new AlreadyInitializedException();
                }
                case MinHook.MH_STATUS.MH_ERROR_DISABLED:
                {
                    return new DisabledException();
                }
                case MinHook.MH_STATUS.MH_ERROR_ENABLED:
                {
                    return new EnabledException();
                }
                case MinHook.MH_STATUS.MH_ERROR_FUNCTION_NOT_FOUND:
                {
                    return new FunctionNotFoundException();
                }
                case MinHook.MH_STATUS.MH_ERROR_MEMORY_ALLOC:
                {
                    return new MemoryAllocException();
                }
                case MinHook.MH_STATUS.MH_ERROR_MEMORY_PROTECT:
                {
                    return new MemoryProtectException();
                }
                case MinHook.MH_STATUS.MH_ERROR_MODULE_NOT_FOUND:
                {
                    return new ModuleNotFoundException();
                }
                case MinHook.MH_STATUS.MH_ERROR_NOT_CREATED:
                {
                    return new TargetNotCreatedException();
                }
                case MinHook.MH_STATUS.MH_ERROR_NOT_EXECUTABLE:
                {
                    return new NotExecutableException();
                }
                case MinHook.MH_STATUS.MH_ERROR_NOT_INITIALIZED:
                {
                    return new NotInitializedException();
                }
                case MinHook.MH_STATUS.MH_ERROR_UNSUPPORTED_FUNCTION:
                {
                    return new UnsupportedFunctionException();
                }
                default:
                {
                    return new MinHookException();
                }
            }
        }

    }

}
