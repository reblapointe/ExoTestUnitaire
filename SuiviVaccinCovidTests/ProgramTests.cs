
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
        List<Vaccin> vaccins;
        List<TypeVaccin> types;

        [TestInitialize]
        public void BuildVaccins()
        {
            types = new List<TypeVaccin> {
                new TypeVaccin { Nom = "Pfizer" },
                new TypeVaccin { Nom = "Moderna" },
             };
            vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = types[0], Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "AAAA10101010", Type = types[0], Date = new DateTime(2021,09,21)},
                new Vaccin { NAMPatient = "BBBB10101010", Type = types[0], Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = types[1], Date = new DateTime(2021,11,2)},
            };
        }
        [TestMethod()]
        public void LePlusRecentTest()
        {
            Assert.AreEqual(vaccins[2], Program.LePlusRecent(vaccins.AsQueryable()));
        }

        [TestMethod()]
        public void LePlusRecentVideTest()
        {
            List<Vaccin> vaccins = new();
            Assert.IsNull(Program.LePlusRecent(vaccins.AsQueryable()));
        }
      
        [TestMethod()]
        public void CreerNouveauVaccinTest()
        {
            DateTime d = new (2021, 03, 27, 2, 34, 55, 392);

            Mock<Func<DateTime>> mockFournisseurDate = new();
            mockFournisseurDate.Setup(m => m()).Returns(d);

            Program p = new()
            {
                FournisseurDeDate = mockFournisseurDate.Object
            };
            Vaccin v = new()
            {
                NAMPatient = "AAAA99999999",
                Type = types[0],
                Date = d
            };
            Vaccin cree = p.CreerNouveauVaccin("AAAA99999999", types[0]);
            Assert.AreEqual(v, cree);
        }

        [TestMethod()]
        public void AjouterVaccinTest()
        {
            var mockContexte = new Mock<VaccinContext>();
            mockContexte.Setup(c => c.Vaccins).Returns(MockContextProvider.GetMockSet(vaccins).Object);
            mockContexte.Setup(m => m.SaveChanges());


            Program p = new()
            {
                Contexte = mockContexte.Object
            };

            Vaccin v = new()
            {
                NAMPatient = "AAAA99999999",
                Type = types[0],
                Date = new DateTime(2021, 03, 27)
            };
            p.EnregistrerVaccin(v);

            p.LePlusRecent();

            mockContexte.Verify(m => m.Vaccins.Add(v), Times.Once);
            mockContexte.Verify(m => m.SaveChanges());
        }

        [TestMethod()]
        public void AjouterVaccinSuiviTest()
        {
            var mockContexte = new Mock<VaccinContext>();
            mockContexte.Setup(c => c.Vaccins).Returns(MockContextProvider.GetMockSet(vaccins).Object);

            Program p = new()
            {
                Contexte = mockContexte.Object
            };

            Vaccin v = new()
            {
                NAMPatient = "BBBB10101010",
                Type = types[0],
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
            var mockContexte = new Mock<VaccinContext>();
            mockContexte.Setup(c => c.Vaccins).Returns(MockContextProvider.GetMockSet(vaccins).Object);

            Program p = new()
            {
                Contexte = mockContexte.Object
            };

            Vaccin dejaDeuxFois = new()
            {
                NAMPatient = "AAAA10101010",
                Type = types[0],
                Date = new DateTime(2021, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.EnregistrerVaccin(dejaDeuxFois));

            mockContexte.Verify(m => m.Add(It.IsAny<Vaccin>()), Times.Never);

            Vaccin mauvaisType = new()
            {
                NAMPatient = "CCCC10101010",
                Type = types[0],
                Date = new DateTime(2021, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.EnregistrerVaccin(mauvaisType));

            mockContexte.Verify(m => m.Vaccins.Add(It.IsAny<Vaccin>()), Times.Never);
        }
    }
}