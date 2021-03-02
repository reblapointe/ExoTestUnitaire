
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuiviVaccinCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuiviVaccinCovid.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void LePlusRecentTest()
        {
            Program p = new Program();
            List<Vaccin> vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "BBBB10101010", Type = "Pfizer", Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = "Moderna", Date = new DateTime(2021,11,2)},
            };
            Vaccin reponse = vaccins[1];
            Assert.AreEqual(reponse, p.LePlusRecent(vaccins));
        }

        [TestMethod()]
        public void LePlusRecentVideTest()
        {
            Program p = new Program();
            List<Vaccin> vaccins = new List<Vaccin>();
            Assert.IsNull(p.LePlusRecent(vaccins));
        }
      
        [TestMethod()]
        public void CreerNouveauVaccinTest()
        {
            DateTime d = new DateTime(2021, 03, 27, 2, 34, 55, 392);

            Mock<Func<DateTime>> mockFournisseurDate = new Mock<Func<DateTime>>();
            mockFournisseurDate.Setup(m => m()).Returns(d);

            Program p = new Program
            {
                FournisseurDeDate = mockFournisseurDate.Object
            };
            Vaccin v = new Vaccin
            {
                NAMPatient = "AAAA99999999",
                Type = "ABC",
                Date = d
            };
            Vaccin cree = p.CreerNouveauVaccin("AAAA99999999", "ABC");
            Assert.AreEqual(v, cree);
        }

        [TestMethod()]
        public void AjouterVaccinTest()
        {
            List<Vaccin> vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "BBBB10101010", Type = "Pfizer", Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = "Moderna", Date = new DateTime(2021,11,2)},
            };


            var data = vaccins.AsQueryable();
            var mockSet = new Mock<DbSet<Vaccin>>();
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContexte = new Mock<VaccinContext>();
            mockContexte.Setup(c => c.Vaccins).Returns(mockSet.Object);


            Program p = new Program
            {
                Contexte = mockContexte.Object
            };

            Vaccin v = new Vaccin
            {
                NAMPatient = "AAAA99999999",
                Type = "ABC",
                Date = new DateTime(2021, 03, 27)
            };
            p.EnregistrerVaccin(v);

            mockContexte.Verify(m => m.Vaccins.Add(v), Times.Once);
            mockContexte.Verify(m => m.SaveChanges());
        }

        [TestMethod()]
        public void AjouterVaccinSuiviTest()
        {
            var vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "BBBB10101010", Type = "Pfizer", Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = "Moderna", Date = new DateTime(2021,11,2)},
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Vaccin>>();
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.Provider).Returns(vaccins.Provider);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.Expression).Returns(vaccins.Expression);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.ElementType).Returns(vaccins.ElementType);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.GetEnumerator()).Returns(vaccins.GetEnumerator());

            var mockContexte = new Mock<VaccinContext>();
            mockContexte.Setup(c => c.Vaccins).Returns(mockSet.Object);

            Program p = new Program
            {
                Contexte = mockContexte.Object
            };

            Vaccin v = new Vaccin
            {
                NAMPatient = "BBBB10101010",
                Type = "Pfizer",
                Date = new DateTime(2021, 03, 27)
            };

            p.EnregistrerVaccin(v);

            mockContexte.Verify(m => m.Vaccins.Add(v), Times.Once);
            mockContexte.Verify(m => m.Vaccins.Add(It.IsAny<Vaccin>()), Times.Once);
            mockContexte.Verify(m => m.SaveChanges());
        }

        [TestMethod()]
        public void AjouterVaccinInvalideTest()
        {

            List<Vaccin> vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = "Moderna", Date = new DateTime(2021,11,2)},
            };


            var data = vaccins.AsQueryable();
            var mockSet = new Mock<DbSet<Vaccin>>();
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<Vaccin>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContexte = new Mock<VaccinContext>();
            mockContexte.Setup(c => c.Vaccins).Returns(mockSet.Object);


            Program p = new Program
            {
                Contexte = mockContexte.Object
            };

            Vaccin dejaDeuxFois = new Vaccin()
            {
                NAMPatient = "AAAA10101010",
                Type = "Pfizer",
                Date = new DateTime(2021, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.EnregistrerVaccin(dejaDeuxFois));

            mockContexte.Verify(m => m.Add(It.IsAny<Vaccin>()), Times.Never);

            Vaccin mauvaisType = new Vaccin()
            {
                NAMPatient = "CCCC10101010",
                Type = "Pfizer",
                Date = new DateTime(2021, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.EnregistrerVaccin(mauvaisType));

            mockContexte.Verify(m => m.Vaccins.Add(It.IsAny<Vaccin>()), Times.Never);
        }
    }
}