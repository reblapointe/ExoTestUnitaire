using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SuiviVaccinationCovid.Modele;
using SuiviVaccinationCovidTests;

namespace SuiviVaccinationCovid.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        List<Dose> doses;
        List<Vaccin> vaccins;

        [TestInitialize]
        public void BuildVaccins()
        {
            vaccins = new List<Vaccin> {
                new Vaccin { Nom = "Pfizer" },
                new Vaccin { Nom = "Moderna" },
            };
            doses = new List<Dose> {
                new Dose { NAMPatient = "AAAA10101010", Vaccin = vaccins[0], Date = new DateTime(2021,11,21)},
                new Dose { NAMPatient = "AAAA10101010", Vaccin = vaccins[0], Date = new DateTime(2021,09,21)},
                new Dose { NAMPatient = "BBBB10101010", Vaccin = vaccins[0], Date = new DateTime(2021,11,30)},
                new Dose { NAMPatient = "CCCC10101010", Vaccin = vaccins[1], Date = new DateTime(2021,11,2)},
            };
        }
        [TestMethod()]
        public void LePlusRecentTest()
        {
            Assert.AreEqual(doses[2], Program.LePlusRecent(doses.AsQueryable()));
        }

        [TestMethod()]
        public void LePlusRecentVideTest()
        {
            List<Dose> doses = new();
            Assert.IsNull(Program.LePlusRecent(doses.AsQueryable()));
        }

        [TestMethod()]
        public void CreerNouvelleDoseTest()
        {
            DateTime date = new(2021, 03, 27, 2, 34, 55, 392);

            Mock<Func<DateTime>> mockFournisseurDate = new();
            mockFournisseurDate.Setup(m => m()).Returns(date);

            Program p = new()
            {
                FournisseurDeDate = mockFournisseurDate.Object
            };
            Dose dose = new()
            {
                NAMPatient = "AAAA99999999",
                Vaccin = vaccins[0],
                Date = date
            };
            Dose cree = p.CreerNouvelleDose("AAAA99999999", vaccins[0]);
            Assert.AreEqual(dose, cree);
        }

        [TestMethod()]
        public void AjouterDoseTest()
        {
            var mockContexte = new Mock<VaccinationContext>();
            mockContexte.Setup(c => c.Doses).Returns(MockContextProvider.GetMockSet(doses).Object);

            Program p = new()
            {
                Contexte = mockContexte.Object
            };

            Dose dose = new()
            {
                NAMPatient = "AAAA99999999",
                Vaccin = vaccins[0],
                Date = new DateTime(2021, 03, 27)
            };
            p.EnregistrerDose(dose);

            p.LePlusRecent();

            mockContexte.Verify(m => m.Doses.Add(dose), Times.Once);
            mockContexte.Verify(m => m.SaveChanges());
        }

        [TestMethod()]
        public void AjouterDoseSuiviTest()
        {
            var mockContexte = new Mock<VaccinationContext>();
            mockContexte.Setup(c => c.Doses).Returns(MockContextProvider.GetMockSet(doses).Object);

            Program p = new()
            {
                Contexte = mockContexte.Object
            };

            Dose d = new()
            {
                NAMPatient = "BBBB10101010",
                Vaccin = vaccins[0],
                Date = new DateTime(2022, 03, 27)
            };

            p.EnregistrerDose(d);

            mockContexte.Verify(m => m.Doses.Add(d), Times.Once);
            mockContexte.Verify(m => m.Doses.Add(It.IsAny<Dose>()), Times.Once);
            mockContexte.Verify(m => m.SaveChanges());
        }

        [TestMethod()]
        public void AjouterDoseInvalideTest()
        {
            var mockContexte = new Mock<VaccinationContext>();
            mockContexte.Setup(c => c.Doses).Returns(MockContextProvider.GetMockSet(doses).Object);

            Program p = new()
            {
                Contexte = mockContexte.Object
            };

            Dose tropRapproché = new()
            {
                NAMPatient = "AAAA10101010",
                Date = new DateTime(2021, 12, 4)
            };
            Assert.ThrowsException<ArgumentException>(() => p.EnregistrerDose(tropRapproché));

            mockContexte.Verify(m => m.Add(It.IsAny<Dose>()), Times.Never);

            Dose mauvaisType = new()
            {
                NAMPatient = "CCCC10101010",
                Vaccin = vaccins[0],
                Date = new DateTime(2022, 03, 27)
            };
            Assert.ThrowsException<ArgumentException>(() => p.EnregistrerDose(mauvaisType));

            mockContexte.Verify(m => m.Doses.Add(It.IsAny<Dose>()), Times.Never);
        }
    }
}