//
// ExceptionWithContextTests.cs
//
// Author:
//       Martin Gondermann <magicmonty@pagansoft.de>
//
// Copyright (c) 2015 (c) 2015 by Martin Gondermann
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Pagansoft.Functional
{
    [TestFixture]
    public class ExceptionWithContextTests
    {
        private Dictionary<string, object> _context;

        [SetUp]
        public void Setup()
        {
            _context = new Dictionary<string, object> 
            { 
                { "String", "StringValue" },
                { "Int", 42 } 
            };
        }

        [Test]
        public void Should_Initialize_Correctly_With_Ctor_Without_Parameters()
        {
            var ex = new ExceptionWithContext();

            ex.Message.ShouldBeEmpty();
            ex.InnerException.ShouldBe(null);
        }

        [Test]
        public void Should_Initialize_Correctly_With_Ctor_With_Context_As_Parameter()
        {
            var ex = new ExceptionWithContext(_context);

            ex.Message.ShouldBeEmpty();
            ex.InnerException.ShouldBe(null);
        }

        [Test]
        public void Should_Initialize_Correctly_With_Ctor_With_Message_And_Context_As_Parameter()
        {
            var ex = new ExceptionWithContext("ErrorMessage", _context);

            ex.Message.ShouldBe("ErrorMessage");
            ex.InnerException.ShouldBe(null);
        }

        [Test]
        public void Should_Initialize_Correctly_With_Ctor_With_Message_InnerException_And_Context_As_Parameter()
        {
            var inner = new Exception("BOO");
            var ex = new ExceptionWithContext("ErrorMessage", inner, _context);

            ex.Message.ShouldBe("ErrorMessage");
            ex.InnerException.ShouldBeSameAs(inner);
        }

        [Test]
        public void GetContextValue_Returns_Some_Value_If_Key_And_Type_Match()
        {
            var ex = new ExceptionWithContext(_context);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetContextValue<string>("String").ShouldBeSome("StringValue"),
                () => ex.GetContextValue<int>("Int").ShouldBeSome(42));
        }

        [Test]
        public void GetContextValue_Returns_None_If_Key_Is_Not_Found()
        {
            var ex = new ExceptionWithContext(_context);

            ex.GetContextValue<string>("FOO").ShouldBeNone();
        }

        [Test]
        public void GetContextValue_Returns_None_If_Type_Does_Not_Match()
        {
            var ex = new ExceptionWithContext(_context);

            ex.ShouldSatisfyAllConditions(
                () => ex.GetContextValue<int>("String").ShouldBeNone(),
                () => ex.GetContextValue<string>("Int").ShouldBeNone());
        }

        [Test]
        public void Serialization_Works_Correctly()
        {
            var inner = new Exception("BOO");
            var ex = new ExceptionWithContext("ErrorMessage", inner, _context);

            var serialized = Serialize(ex);
            serialized.Length.ShouldBeGreaterThan(0);
            var deserialized = Deserialize<ExceptionWithContext>(serialized);

            deserialized.ShouldSatisfyAllConditions(
                () => deserialized.Message.ShouldBe("ErrorMessage"),
                () => deserialized.InnerException.Message.ShouldBe("BOO"),
                () => deserialized.GetContextValue<string>("String").ShouldBeSome("StringValue"),
                () => deserialized.GetContextValue<int>("Int").ShouldBeSome(42));
        }

        static byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.GetBuffer();
            }
        }

        static T Deserialize<T>(byte[] serialized)
        {
            using (var stream = new MemoryStream())
            {
                stream.Write(serialized, 0, serialized.Length);
                stream.Seek(0, SeekOrigin.Begin);
                var formatter = new BinaryFormatter();

                try
                {
                    return (T)formatter.Deserialize(stream);
                }
                catch
                {
                    return default(T);
                }
            }
        }
    }
}

