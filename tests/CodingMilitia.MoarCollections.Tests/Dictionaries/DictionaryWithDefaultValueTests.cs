using CodingMilitia.MoarCollections.Dictionaries;
using System;
using System.Collections.Generic;
using Xunit;

namespace CodingMilitia.MoarCollections.Tests.Dictionaries
{
    public class DictionaryWithDefaultValueTests
    {
        private const int DefaultValue = 666;
        private static readonly Func<int, int> DefaultValueFunc = (key) => DefaultValue;
        private static IDictionary<int, int> SampleDictionary = new Dictionary<int, int>
        {
            [0] = 0,
            [1] = 111
        };

        [Fact]
        public void WhenKeyIsInDictionaryItIsReturnedRegardlessOfDefaultValue()
        {
            var dictionary = new DictionaryWithDefaultValue<int, int>(SampleDictionary, DefaultValue);
            Assert.Equal(111, dictionary[1]);
            Assert.NotEqual(DefaultValue, dictionary[1]);
        }

        [Fact]
        public void WhenKeyIsInDictionaryItIsReturnedRegardlessOfDefaultValueFunc()
        {
            var dictionary = new DictionaryWithDefaultValue<int, int>(SampleDictionary, DefaultValueFunc);

            Assert.Equal(111, dictionary[1]);
            Assert.NotEqual(DefaultValue, dictionary[1]);
        }

        [Fact]
        public void WhenKeyIsNotInDictionaryItReturnsTheDefaultValue()
        {
            var dictionary = new DictionaryWithDefaultValue<int, int>(SampleDictionary, DefaultValue);
            Assert.Equal(DefaultValue, dictionary[6]);
        }

        [Fact]
        public void WhenKeyIsNotInDictionaryItReturnsTheDefaultValueFuncResult()
        {
            var dictionary = new DictionaryWithDefaultValue<int, int>(SampleDictionary, DefaultValueFunc);
            Assert.Equal(DefaultValue, dictionary[6]);
        }

        [Fact]
        public void WhenKeyIsNotInDictionaryAndDefaultValueFuncThrowsItIsPropagated()
        {
            var dictionary = new DictionaryWithDefaultValue<int, int>(SampleDictionary, (key) => throw new InvalidOperationException("Test"));
            Assert.Throws<InvalidOperationException>(() => dictionary[6]);
        }

        [Fact]
        public void WhenNullSourceDictionaryIsProvidedAnArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>("sourceDictionary", () => new DictionaryWithDefaultValue<int, int>(null, 0));
            Assert.Throws<ArgumentNullException>("sourceDictionary", () => new DictionaryWithDefaultValue<int, int>(null, (key) => 0));
        }

        [Fact]
        public void WhenNullDefaultValueFuncIsProvidedAnArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>("defaultValueFunc", () => new DictionaryWithDefaultValue<int, int>((Func<int, int>)null));
            Assert.Throws<ArgumentNullException>("defaultValueFunc", () => new DictionaryWithDefaultValue<int, int>(SampleDictionary, (Func<int, int>)null));
        }
    }
}