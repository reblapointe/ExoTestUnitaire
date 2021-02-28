using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuiviVaccinCovid;
using System;
using System.Collections.Generic;
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

            Mock<IFournisseurDeDate> mockFournisseurDate = new Mock<IFournisseurDeDate>();
            mockFournisseurDate.Setup(m => m.Now).Returns(d);

            Program p = new Program();
            Vaccin v = new Vaccin()
            {
                NAMPatient = "AAAA99999999",
                Type = "ABC",
                Date = d
            };
            Vaccin cree = p.CreerNouveauVaccin(mockFournisseurDate.Object, "AAAA99999999", "ABC");
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

            Mock<IDaoVaccin> mockContexte = new Mock<IDaoVaccin>();
            mockContexte.Setup(m => m.AjouterVaccin(It.IsAny<Vaccin>()));
            mockContexte.Setup(m => m.ObtenirVaccins()).Returns(vaccins);
            mockContexte.Setup(m => m.Sauvegarder());

            Program p = new Program();
            Vaccin v = new Vaccin()
            {
                NAMPatient = "AAAA99999999",
                Type = "ABC",
                Date = new DateTime(2021, 03, 27)
            };
            p.AjouterVaccin(mockContexte.Object, v);

            mockContexte.Verify(m => m.AjouterVaccin(v), Times.Once);
            mockContexte.Verify(m => m.Sauvegarder());
        }

        [TestMethod()]
        public void AjouterVaccinSuiviTest()
        {
            List<Vaccin> vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "BBBB10101010", Type = "Pfizer", Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = "Moderna", Date = new DateTime(2021,11,2)},
            };

            Mock<IDaoVaccin> mockContexte = new Mock<IDaoVaccin>();
            mockContexte.Setup(m => m.AjouterVaccin(It.IsAny<Vaccin>()));
            mockContexte.Setup(m => m.ObtenirVaccins()).Returns(vaccins);
            mockContexte.Setup(m => m.Sauvegarder());

            Program p = new Program();
            Vaccin v = new Vaccin
            {
                NAMPatient = "BBBB10101010",
                Type = "Pfizer",
                Date = new DateTime(2021, 03, 27)
            };
            p.AjouterVaccin(mockContexte.Object, v);

            mockContexte.Verify(m => m.AjouterVaccin(v), Times.Once);
            mockContexte.Verify(m => m.AjouterVaccin(It.IsAny<Vaccin>()), Times.Once);
            mockContexte.Verify(m => m.Sauvegarder());
        }

        [TestMethod()]
        public void AjouterVaccinInvalideTest()
        {
            List<Vaccin> vaccins = new List<Vaccin> {
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,21)},
                new Vaccin { NAMPatient = "AAAA10101010", Type = "Pfizer", Date = new DateTime(2021,11,30)},
                new Vaccin { NAMPatient = "CCCC10101010", Type = "Moderna", Date = new DateTime(2021,11,2)},
            };

            Mock<IDaoVaccin> mockContexte = new Mock<IDaoVaccin>();
            mockContexte.Setup(m => m.AjouterVaccin(It.IsAny<Vaccin>()));
            mockContexte.Setup(m => m.ObtenirVaccins()).Returns(vaccins);

            Program p = new Program();
            Vaccin dejaDeuxFois = new Vaccin()
            {
                NAMPatient = "AAAA10101010",
                Type = "Pfizer",
                Date = new DateTime(2021, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.AjouterVaccin(mockContexte.Object, dejaDeuxFois));

            mockContexte.Verify(m => m.AjouterVaccin(It.IsAny<Vaccin>()), Times.Never);

            Vaccin mauvaisType = new Vaccin()
            {
                NAMPatient = "CCCC10101010",
                Type = "Pfizer",
                Date = new DateTime(2021, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.AjouterVaccin(mockContexte.Object, mauvaisType));

            mockContexte.Verify(m => m.AjouterVaccin(It.IsAny<Vaccin>()), Times.Never);
        }
    }
}