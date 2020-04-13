using Luilliarcec.Pagination;
using Luilliarcec.Pagination.Contracts;
using Luilliarcec.Pagination.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Tests.Unit
{
    [TestClass]
    public class PaginationTest
    {
        public IEnumerable<Employer> Data { get; set; }

        public PaginationTest()
        {
            Data = new List<Employer>()
            {
                new Employer(1, "Luis"),
                new Employer(2, "Carlos"),
                new Employer(3, "Juan"),
                new Employer(4, "Pedro"),
                new Employer(5, "Maria"),
                new Employer(6, "Antonio"),
                new Employer(7, "Jefferson"),
                new Employer(8, "Lissette"),
                new Employer(9, "Andres"),
                new Employer(10, "Fabricio"),
                new Employer(11, "Manuel"),
                new Employer(12, "Ruth"),
                new Employer(13, "Ubaldo"),
                new Employer(14, "Melanni"),
                new Employer(15, "Felix"),
                new Employer(16, "Karla"),
                new Employer(17, "Francisco"),
                new Employer(18, "Kerly"),
                new Employer(19, "Marlene"),
                new Employer(20, "Naomi"),
                new Employer(21, "Emily"),
                new Employer(22, "Jon"),
                new Employer(23, "Snow"),
                new Employer(24, "Ragnark"),
                new Employer(25, "Alberto"),
                new Employer(26, "Mario"),
            };
        }

        [TestMethod]
        public void CheckThatTheTotalRecordsIsCorrect()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "first", limit: 5);
            Assert.AreEqual(Data.Count(), (int)result["total"]);
        }

        [TestMethod]
        public void CheckThatTheLimitOfRecordsPerPageIsCorrect()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "first", limit: 5);
            Assert.AreEqual(5, (int)result["per_page"]);
        }

        [TestMethod]
        public void CheckThatTheCurrentPageIsCorrect()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "next", 2, 5);
            Assert.AreEqual(3, (int)result["current_page"]);
        }

        [TestMethod]
        public void CheckThatTheLastPageIsCorrect()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "next", 2, 5);
            Assert.AreEqual(6, (int)result["last_page"]);
        }

        [TestMethod]
        public void CheckThatRecordsAreGetFromTheFirstPage()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "first", limit: 5);
            var data = (IReadOnlyCollection<IPaginable>)result["data"];

            Assert.AreEqual(5, data.Count());

            int id = 1;
            foreach (Employer item in data)
            {
                Assert.AreEqual(item.Id, id);
                id += 1;
            }
        }

        [TestMethod]
        public void CheckThatRecordsAreGetFromTheNextPage()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "next", 1, 5);
            var data = (IReadOnlyCollection<IPaginable>)result["data"];

            Assert.AreEqual(5, data.Count());
            Assert.AreEqual(2, (int)result["current_page"]);

            int id = 6;
            foreach (Employer item in data)
            {
                Assert.AreEqual(item.Id, id);
                id += 1;
            }
        }

        [TestMethod]
        public void CheckThatRecordsAreGetFromThePreviousPage()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "previous", 5, 5);
            var data = (IReadOnlyCollection<IPaginable>)result["data"];

            Assert.AreEqual(5, data.Count());
            Assert.AreEqual(4, (int)result["current_page"]);

            int id = 16;
            foreach (Employer item in data)
            {
                Assert.AreEqual(item.Id, id);
                id += 1;
            }
        }

        [TestMethod]
        public void CheckThatRecordsAreGetFromTheLastPage()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "last", limit: 5);
            var data = (IReadOnlyCollection<IPaginable>)result["data"];

            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(6, (int)result["current_page"]);

            int id = 26;
            foreach (Employer item in data)
            {
                Assert.AreEqual(item.Id, id);
                id += 1;
            }
        }

        [TestMethod]
        public void CheckThatRecordsAreGetFromTheCurrentPage()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "current", 4, 5);
            var data = (IReadOnlyCollection<IPaginable>)result["data"];

            Assert.AreEqual(5, data.Count());
            Assert.AreEqual(4, (int)result["current_page"]);

            int id = 16;
            foreach (Employer item in data)
            {
                Assert.AreEqual(item.Id, id);
                id += 1;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(PaginationException), "Invalid argument exception, expected between, [next, previous, last, first or current], received foo")]
        public void CheckThatAnErrorIsObtainedWhenSendingAnInvalidArgument()
        {
            var result = Pagination.Paginate(Data.AsQueryable().OrderBy(e => e.Id), "foo", 4, 5);
        }
    }

    public class Employer : IPaginable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Employer() { }

        public Employer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
