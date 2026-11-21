using System;
using System.Collections.Generic;
using Xunit;

namespace IndependentWork7.Tests
{
    public class CacheTests
    {
        [Fact]
        public void Add_ValidKeyValue_AddsElementToCache()
        {
            var cache = new Cache<int, string>(3);

            cache.Add(1, "One");

            Assert.True(cache.ContainsKey(1));
            Assert.Equal("One", cache.Get(1));
        }

        [Fact]
        public void Add_ExistingKey_UpdatesValueWithoutIncreasingCount()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "One");
            cache.Add(2, "Two");

            cache.Add(1, "UpdatedOne");

            Assert.Equal(2, cache.Count);
            Assert.Equal("UpdatedOne", cache.Get(1));
            Assert.True(cache.ContainsKey(2));
        }

        [Fact]
        public void Get_ExistingKey_ReturnsValue()
        {
            var cache = new Cache<string, int>(2);
            cache.Add("a", 10);
            cache.Add("b", 20);

            int result = cache.Get("b");

            Assert.Equal(20, result);
        }

        [Fact]
        public void Get_NonExistentKey_ThrowsKeyNotFoundException()
        {
            var cache = new Cache<string, string>(2);
            cache.Add("k1", "v1");

            Assert.Throws<KeyNotFoundException>(() => cache.Get("unknown"));
        }

        [Fact]
        public void Add_ExceedingCapacity_EvictsOldestElement()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(1, "First");
            cache.Add(2, "Second");

            cache.Add(3, "Third");

            Assert.Equal(2, cache.Count);
            Assert.False(cache.ContainsKey(1));
            Assert.True(cache.ContainsKey(2));
            Assert.True(cache.ContainsKey(3));
            Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        }

        [Fact]
        public void ContainsKey_ReturnsTrueForExistingAndFalseForMissing()
        {
            var cache = new Cache<int, string>(2);
            cache.Add(10, "Ten");

            Assert.True(cache.ContainsKey(10));
            Assert.False(cache.ContainsKey(99));
        }

        [Fact]
        public void Count_ReflectsActualItemsCount()
        {
            var cache = new Cache<int, string>(2);
            Assert.Equal(0, cache.Count);

            cache.Add(1, "A");
            Assert.Equal(1, cache.Count);

            cache.Add(2, "B");
            Assert.Equal(2, cache.Count);

            cache.Add(3, "C");
            Assert.Equal(2, cache.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-5)]
        public void Constructor_NonPositiveCapacity_ThrowsArgumentOutOfRangeException(int invalidCapacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(invalidCapacity));
        }
    }
}