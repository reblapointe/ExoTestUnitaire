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
    }
}