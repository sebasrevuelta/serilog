﻿// Copyright 2014 Serilog Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Reflection;
using Serilog.Core;
using Serilog.Policies;

namespace Serilog.Configuration
{
    /// <summary>
    /// Controls template parameter destructuring configuration.
    /// </summary>
    public class LoggerDestructuringConfiguration
    {
        readonly LoggerConfiguration _loggerConfiguration;
        readonly Action<Type> _addScalar;
        readonly Action<IDestructuringPolicy> _addPolicy;
        readonly Action<int> _setMaximumDepth;

        internal LoggerDestructuringConfiguration(
            LoggerConfiguration loggerConfiguration,
            Action<Type> addScalar,
            Action<IDestructuringPolicy> addPolicy,
            Action<int> setMaximumDepth)
        {
            if (loggerConfiguration == null) throw new ArgumentNullException("loggerConfiguration");
            if (addScalar == null) throw new ArgumentNullException("addScalar");
            if (addPolicy == null) throw new ArgumentNullException("addPolicy");
            _loggerConfiguration = loggerConfiguration;
            _addScalar = addScalar;
            _addPolicy = addPolicy;
            _setMaximumDepth = setMaximumDepth;
        }

        /// <summary>
        /// Treat objects of the specified type as scalar values, i.e., don't break
        /// them down into properties event when destructuring complex types.
        /// </summary>
        /// <param name="scalarType">Type to treat as scalar.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public LoggerConfiguration AsScalar(Type scalarType)
        {
            if (scalarType == null) throw new ArgumentNullException("scalarType");
            _addScalar(scalarType);
            return _loggerConfiguration;
        }

        /// <summary>
        /// Treat objects of the specified type as scalar values, i.e., don't break
        /// them down into properties event when destructuring complex types.
        /// </summary>
        /// <typeparam name="TScalar">Type to treat as scalar.</typeparam>
        /// <returns>Configuration object allowing method chaining.</returns>
        public LoggerConfiguration AsScalar<TScalar>()
        {
            return AsScalar(typeof(TScalar));
        }

        /// <summary>
        /// When destructuring objects, transform instances with the provided policies.
        /// </summary>
        /// <param name="destructuringPolicies">Policies to apply when destructuring.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public LoggerConfiguration With(params IDestructuringPolicy[] destructuringPolicies)
        {
            if (destructuringPolicies == null) throw new ArgumentNullException("destructuringPolicies");
            foreach (var destructuringPolicy in destructuringPolicies)
            {
                if (destructuringPolicy == null)
                    throw new ArgumentException("Null policy is not allowed.");
                _addPolicy(destructuringPolicy);
            }
            return _loggerConfiguration;
        }

        /// <summary>
        /// When destructuring objects, transform instances with the provided policy.
        /// </summary>
        /// <typeparam name="TDestructuringPolicy">Policy to apply when destructuring.</typeparam>
        /// <returns>Configuration object allowing method chaining.</returns>
        public LoggerConfiguration With<TDestructuringPolicy>()
            where TDestructuringPolicy : IDestructuringPolicy, new()
        {
            return With(new TDestructuringPolicy());
        }

        /// <summary>
        /// When destructuring objects, transform instances of the specified type with
        /// the provided function.
        /// </summary>
        /// <param name="transformation">Function mapping instances of <typeparamref name="TValue"/>
        /// to an alternative representation.</param>
        /// <typeparam name="TValue">Type of values to transform.</typeparam>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public LoggerConfiguration ByTransforming<TValue>(Func<TValue, object> transformation)
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            var policy = new ProjectedDestructuringPolicy(t => t == typeof(TValue),
                                                          o => transformation((TValue)o));
            return With(policy);
        }

        /// <summary>
        /// When destructuring objects, transform instances of the specified type (or derived from the
        /// specified type) with the provided function.
        /// </summary>
        /// <param name="transformation">Function mapping instances of <typeparamref name="TValue"/>
        /// to an alternative representation.</param>
        /// <typeparam name="TValue">Type of values to transform.</typeparam>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public LoggerConfiguration ByTransformingFrom<TValue>(Func<TValue, object> transformation)
        {
            if (transformation == null) throw new ArgumentNullException("transformation");
            var policy = new ProjectedDestructuringPolicy(t => typeof(TValue).GetTypeInfo().IsAssignableFrom(t.GetTypeInfo()),
                                                          o => transformation((TValue)o));
            return With(policy);
        }

        /// <summary>
        /// When destructuring objects, depth will be limited to 5 property traversals deep to
        /// guard against ballooning space when recursive/cyclic structures are accidentally passed. To
        /// increase this limit pass a higher value.
        /// </summary>
        /// <param name="maximumDestructuringDepth">The maximum depth to use.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public LoggerConfiguration ToMaximumDepth(int maximumDestructuringDepth)
        {
            if (maximumDestructuringDepth < 0) throw new ArgumentOutOfRangeException("maximumDestructuringDepth");
            _setMaximumDepth(maximumDestructuringDepth);
            return _loggerConfiguration;
        }
    }
}

