
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuiviVaccinCovid.Tests
{
    public class MockContextProvider
    {
        public static Mock<DbSet<T>> GetMockSet<T>(List<T> list) where T : class
        {
            var queryable = list.AsQueryable();
            var mockList = new Mock<DbSet<T>>();

            mockList.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockList.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockList.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockList.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            
            return mockList;
        }
    }
}