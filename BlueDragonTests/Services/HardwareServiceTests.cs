using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;

namespace BlueDragon.Services.Tests
{
    public static class MockAsyncQueryProvider
    {
        public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
            { }
            public TestAsyncEnumerable(Expression expression) : base(expression)
            { }
            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }
            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }

        public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;
            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }
            public T Current => _inner.Current;
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;
            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }
        }

        public class TestAsyncQueryProvider<T> : IQueryProvider
        {
            private readonly IQueryProvider _inner;
            public TestAsyncQueryProvider(IQueryable<T> inner)
            {
                _inner = inner.Provider;
            }
            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<T>(expression);
            }
            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }
            public object Execute(Expression expression)
            {
                return _inner.Execute(expression);
            }
            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }
            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            {
                var resultType = typeof(TResult).GetGenericArguments()[0];
                var executionResult = _inner.Execute(expression);
                var result = typeof(Task).GetMethod(nameof(Task.FromResult))
                    .MakeGenericMethod(resultType)
                    .Invoke(null, new[] { executionResult });
                return (TResult)result;
            }
        }

        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<T>(queryable));
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet;
        }
    }

    [TestClass()]
    public class HardwareServiceTests
    {
        [TestMethod()]
        public async Task GetHardwareTest()
        {
            // Arrange
            var hardwareData = new List<Hardware>
            {
                new Hardware { Name = "Hardware1" },
                new Hardware { Name = "Hardware2" }
            };

            var mockDbSet = MockAsyncQueryProvider.GetQueryableMockDbSet(hardwareData);
            var mockContext = new Mock<HccContext>();
            mockContext.Setup(m => m.Hardwares).Returns(mockDbSet.Object);
            var service = new HardwareService(mockContext.Object);

            // Act
            var result = await service.GetHardware();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(List<Hardware>));
            Assert.IsTrue(result.Count >= 0);
        }
    }
}