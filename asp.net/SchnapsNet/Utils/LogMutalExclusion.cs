using System;
using System.Threading;

namespace SchnapsNet.Utils
{

    /// <summary>
    /// static <see cref="Mutex"/> for mutal exclusion, you can only use it once for one mutal exclusion caae, cause it's static 
    /// get <see cref="Mutex"/> by calling <see cref="CreateMutalExlusion(string, bool)"/> 
    /// release <see cref="Mutex"/> by calling <see cref="ReleaseCloseDisposeMutex"/>
    /// </summary>
    internal static class LogMutalExclusion
    {
        private static readonly object _outerLock = new object(), _lock = new object();

        private static Mutex _theMutex = null;

        /// <summary>
        /// Gets the Mutal Exclusion
        /// </summary>
        internal static Mutex TheMutex { get => _theMutex; }

        /// <summary>
        /// static ctor
        /// </summary>
        static LogMutalExclusion()
        {
            _theMutex = null;
        }

        /// <summary>
        /// Gets existing mutex or creates a new <see cref="Mutex"/> 
        /// </summary>
        /// <param name="mutexUniqueName">unique string identifier for the mutal exlusion</param>
        /// <param name="useExistingMutex">if true, existing and valid <see cref="Mutex"/> will be returned, 
        /// otherwise a new <see cref="Mutex"/> will be created; default <see cref="false"/></param>
        /// <returns><see cref="Mutex"/></returns>
        internal static Mutex CreateMutalExlusion(string mutexUniqueName = "MutalExclusion", bool useExistingMutex = false)
        {
            if (useExistingMutex && _theMutex != null && _theMutex.SafeWaitHandle != null &&
                !_theMutex.SafeWaitHandle.IsClosed && !_theMutex.SafeWaitHandle.IsInvalid)
                return _theMutex;

            // Thread.Sleep(16);
            _theMutex = new Mutex(true, mutexUniqueName);

            return _theMutex;
        }

        /// <summary>
        /// Release Mutax exclusion, that not 2 chat programs could be started at same machine
        /// </summary>
        internal static void ReleaseCloseDisposeMutex()
        {
            Exception ex = null;
            Microsoft.Win32.SafeHandles.SafeWaitHandle safeWaitHandle = null;
            IntPtr safeMutextWin32Handle = IntPtr.Zero;

            lock (_outerLock)
            {
                if (_theMutex != null)
                {
                    lock (_lock)
                    {
                        safeWaitHandle = _theMutex.GetSafeWaitHandle();
                        safeMutextWin32Handle = safeWaitHandle.DangerousGetHandle();
                        if (safeWaitHandle != null && !safeWaitHandle.IsClosed)
                        {
                            try
                            {
                                _theMutex.ReleaseMutex();
                                //    safeWaitHandle.DangerousRelease();
                            }
                            catch (Exception exRelease)
                            {
                                ex = new SchnapsException("Releasing Mutex failed", exRelease);
                                SchnapsException.SetLastException(ex);
                            }
                            try
                            {
                                _theMutex.Close();
                                //    safeWaitHandle.Close();
                            }
                            catch (Exception exClose)
                            {
                                ex = new SchnapsException("Closing Mutex failed", exClose);
                                SchnapsException.SetLastException(ex);
                            }
                        }

                        try
                        {
                            _theMutex.Dispose();
                            //    safeWaitHandle.Dispose();
                        }
                        catch (Exception exDispose)
                        {
                            ex = new SchnapsException("Disposing Mutex failed", exDispose);
                            SchnapsException.SetLastException(ex);
                        }
                    }
                }

                try
                {
                    _theMutex = null;
                }
                catch (Exception exNull)
                {
                    ex = new SchnapsException("Setting Mutex to null failed", exNull);
                    SchnapsException.SetLastException(ex);
                }
                finally
                {
                    if (ex != null)
                    {
                        SchnapsException.SetLastException(new SchnapsException("Disposing mutex and safeWaitHandle throwed exception.", ex));
                    }
                }
            }

            return;
        }

    }

}