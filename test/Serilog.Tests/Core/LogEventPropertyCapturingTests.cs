﻿#if INTERNAL_TESTS

using System;
using System.Linq;
using System.Collections.Generic;

using Serilog.Core;
using Serilog.Events;
using Serilog.Parameters;
using Serilog.Parsing;

using Xunit;

namespace Serilog.Tests.Core
{
    public class LogEventPropertyCapturingTests
    {
        [Fact]
        public void CapturingANamedScalarStringWorks()
        {
            Assert.Equal(
                new[] { new LogEventProperty("who", new ScalarValue("world")) },
                Capture("Hello {who}", "world"),
                new LogEventPropertyStructuralEqualityComparer());
        }

        [Fact]
        public void CapturingAPositionalScalarStringWorks()
        {
            Assert.Equal(
                new[] { new LogEventProperty("0", new ScalarValue("world")) },
                Capture("Hello {0}", "world"),
                new LogEventPropertyStructuralEqualityComparer());
        }

        [Fact]
        public void CapturingMixedPositionalAndNamedScalarsWorksUsingNames()
        {
            Assert.Equal(new[]
                {
                    new LogEventProperty("who", new ScalarValue("worldNamed")),
                    new LogEventProperty("0", new ScalarValue("worldPositional")),
                },
                Capture("Hello {who} {0} {0}", "worldNamed", "worldPositional"),
                new LogEventPropertyStructuralEqualityComparer());
        }

        [Fact(Skip="Proposed behaviour")]
        public void WillCaptureAdditionalPositionalsNotInTemplate()
        {
            Assert.Equal(new[]
                {
                    new LogEventProperty("0", new ScalarValue(0)),
                    new LogEventProperty("1", new ScalarValue(1)),
                    new LogEventProperty("<todo - decide naming strategy>1", new ScalarValue(2)),
                    new LogEventProperty("<todo - decide naming strategy>2", new ScalarValue(3)),
                },
                Capture("Hello {1} {0} 2}", 1, 0, 2, 3),
                new LogEventPropertyStructuralEqualityComparer());
        }

        [Fact(Skip="Proposed behaviour")]
        public void WillCaptureAdditionalNamedPropsNotInTemplate()
        {
            Assert.Equal(new[]
                {
                    new LogEventProperty("who", new ScalarValue("who")),
                    new LogEventProperty("what", new ScalarValue("what")),
                    new LogEventProperty("<todo - decide naming strategy>1", new ScalarValue("where")),
                    new LogEventProperty("<todo - decide naming strategy>2", new ScalarValue("how")),
                },
                Capture("Hello {who} {what} where}", "who", "what", "where", "how"),
                new LogEventPropertyStructuralEqualityComparer());
        }
        
        static IEnumerable<LogEventProperty> Capture(string messageTemplate, params object[] properties)
        {
            var mt = new MessageTemplateParser().Parse(messageTemplate);
            var binder = new PropertyBinder(
                new PropertyValueConverter(10, Enumerable.Empty<Type>(), Enumerable.Empty<IDestructuringPolicy>()));
            return binder.ConstructProperties(mt, properties);
        }

        [Fact]
        public void StructuralEqualityComparerKindaSortaWorksForScalars()
        {
            var scalarStringA = new LogEventProperty("a", new ScalarValue("a"));
            var scalarStringAStructurallyEqual = new LogEventProperty("a", new ScalarValue("a"));

            var scalarStringB = new LogEventProperty("b", new ScalarValue("b"));
            var scalarStringBStructurallyNotEqual = new LogEventProperty("b", new ScalarValue("notEqual"));

            var scalarIntA1 = new LogEventProperty("a", new ScalarValue(1));
            var scalarIntA1StructurallyEqual = new LogEventProperty("a", new ScalarValue(1));
            var scalarIntA1DiffValueSameType = new LogEventProperty("a", new ScalarValue(0));
            var scalarIntB1 = new LogEventProperty("b", new ScalarValue(1));

            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();
            var scalarGuid1 = new LogEventProperty("1", new ScalarValue(guid1));
            var scalarGuid1StructurallyEqual = new LogEventProperty("1", new ScalarValue(guid1));
            var scalarGuid1StructurallyNotEqual = new LogEventProperty("1", new ScalarValue("notEqual"));
            var scalarGuid2 = new LogEventProperty("2", new ScalarValue(guid2));

            var sut = new LogEventPropertyStructuralEqualityComparer();

            Assert.True(sut.Equals(scalarStringA, scalarStringAStructurallyEqual));
            Assert.True(sut.Equals(scalarIntA1, scalarIntA1StructurallyEqual));
            Assert.True(sut.Equals(scalarGuid1, scalarGuid1StructurallyEqual));

            Assert.False(sut.Equals(scalarStringB, scalarStringBStructurallyNotEqual));
            Assert.False(sut.Equals(scalarIntA1, scalarIntB1));
            Assert.False(sut.Equals(scalarIntA1, scalarIntA1DiffValueSameType));

            Assert.False(sut.Equals(scalarGuid1, scalarGuid2));
            Assert.False(sut.Equals(scalarGuid1, scalarGuid1StructurallyNotEqual));
        }

        class LogEventPropertyStructuralEqualityComparer : IEqualityComparer<LogEventProperty>
        {
            readonly IEqualityComparer<LogEventPropertyValue> _valueEqualityComparer;

            public LogEventPropertyStructuralEqualityComparer(
                IEqualityComparer<LogEventPropertyValue> valueEqualityComparer = null)
            {
                this._valueEqualityComparer =
                    valueEqualityComparer ?? new LogEventPropertyValueComparer(EqualityComparer<object>.Default);
            }

            public bool Equals(LogEventProperty x, LogEventProperty y)
            {
                return x.Name == y.Name
                    && _valueEqualityComparer.Equals(x.Value, y.Value);
            }

            public int GetHashCode(LogEventProperty obj)
            {
                return 0;
            }
        }

        class LogEventPropertyValueComparer : IEqualityComparer<LogEventPropertyValue>
        {
            readonly IEqualityComparer<object> _objectEqualityComparer;

            public LogEventPropertyValueComparer(IEqualityComparer<object> objectEqualityComparer = null)
            {
                this._objectEqualityComparer = objectEqualityComparer ?? EqualityComparer<object>.Default;
            }

            public bool Equals(LogEventPropertyValue x, LogEventPropertyValue y)
            {
                var scalarX = x as ScalarValue;
                var scalarY = y as ScalarValue;
                if (scalarX != null && scalarY != null)
                {
                    return _objectEqualityComparer.Equals(scalarX.Value, scalarY.Value);
                }
                else if (x is SequenceValue && y is SequenceValue)
                {
                    throw new NotImplementedException();
                }
                else if (x is StructureValue && y is StructureValue)
                {
                    throw new NotImplementedException();
                }
                else if (x is DictionaryValue && y is DictionaryValue)
                {
                    throw new NotImplementedException();
                }

                throw new NotImplementedException();
            }

            public int GetHashCode(LogEventPropertyValue obj)
            {
                return 0;
            }
        }
    }
}

#endif