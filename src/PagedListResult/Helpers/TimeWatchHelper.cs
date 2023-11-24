// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.PagedListResult
//  Author           : RzR
//  Created On       : 2023-11-10 13:39
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-13 16:44
// ***********************************************************************
//  <copyright file="TimeWatchHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Diagnostics;

#endregion

namespace PagedListResult.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>A time watch helper.</summary>
    /// <remarks>RzR, 13-Nov-23.</remarks>
    /// =================================================================================================
    internal class TimeWatchHelper
    {
        /// <summary>(Immutable) the instance.</summary>
        private static readonly TimeWatchHelper _instance;

        /// <summary>(Immutable) the current watch.</summary>
        private readonly Stopwatch _currentWatch;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Static constructor.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// =================================================================================================
        static TimeWatchHelper() => _instance = new TimeWatchHelper();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Constructor that prevents a default instance of this class from being created.
        /// </summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// =================================================================================================
        private TimeWatchHelper() => _currentWatch = new Stopwatch();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets the instance.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <returns>A TimeWatchHelper.</returns>
        /// =================================================================================================
        public static TimeWatchHelper Instance() => _instance;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Starts a new.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// =================================================================================================
        internal void StartNew()
        {
            _currentWatch.Reset();
            _currentWatch.Start();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Starts new and watch.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <returns>A Stopwatch.</returns>
        /// =================================================================================================
        internal Stopwatch StartNewAndWatch()
        {
            _currentWatch.Reset();
            _currentWatch.Start();

            return _currentWatch;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>Gets the stop.</summary>
        /// <remarks>RzR, 13-Nov-23.</remarks>
        /// <returns>A long.</returns>
        /// =================================================================================================
        internal long Stop()
        {
            _currentWatch.Stop();

            return _currentWatch.ElapsedMilliseconds;
        }
    }
}