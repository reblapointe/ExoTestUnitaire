using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;
using SuiviVaccinationCovid;
using SuiviVaccinationCovid.Modele;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SuiviVaccinationCovidTests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void LePlusRecentTest()
        {
            List<Vaccin> vaccins =
            [
                new Vaccin { Nom = "Pfizer" },
                new Vaccin { Nom = "Moderna" }
            ];

            List<Dose> doses =
            [
                new Dose
                {
                    NAMPatient = "AAAA10101010",
                    Vaccin = vaccins[0],
                    Date = new DateTime(2024,11,21)
                },
                new Dose
                {
                    NAMPatient = "BBBB10101010",
                    Vaccin = vaccins[0],
                    Date = new DateTime(2024,11,30)
                },
                new Dose
                {
                    NAMPatient = "CCCC10101010",
                    Vaccin = vaccins[1],
                    Date = new DateTime(2024,11,2)
                }
            ];

            Dose reponse = new()
            {
                NAMPatient = "BBBB10101010",
                Vaccin = vaccins[0],
                Date = new DateTime(2024, 11, 30)
            };
            Assert.AreEqual(reponse, Program.LePlusRecent(doses.AsQueryable()));

        }

        [TestMethod()]
        public void LePlusRecentVideTest()
        {
            List<Dose> doses = [];
            Assert.ThrowsException<InvalidOperationException>(() =>
                Program.LePlusRecent(doses.AsQueryable<Dose>()));
        }

        [TestMethod()]
        public void CreerNouvelleDoseTest()
        {
            DateTime date = new(2025, 01, 15, 2, 34, 55, 392);
            Vaccin v = new() { Nom = "Pfizer" };

            Mock<Func<DateTime>> mockFournisseurDate = new();
            mockFournisseurDate.Setup(m => m()).Returns(date);

            Program p = new()
            {
                FournisseurDeDate = mockFournisseurDate.Object
            };
            Dose dose = new()
            {
                NAMPatient = "AAAA99999999",
                Vaccin = v,
                Date = date
            };
            Dose cree = p.CreerNouvelleDose("AAAA99999999", v);
            Assert.AreEqual(dose, cree);
        }


        [TestMethod()]
        public void EnregistrerDoseTest()
        {
            List<Vaccin> vaccins =
            [
                new Vaccin { Nom = "Pfizer" },
                new Vaccin { Nom = "Moderna" }
            ];

            var doses = new List<Dose>
            {
                new() { NAMPatient = "AAAA10101010",
                        Vaccin = vaccins[0],
                        Date = new DateTime(2024, 11, 21)},
                new() { NAMPatient = "BBBB10101010",
                        Vaccin = vaccins[0],
                        Date = new DateTime(2024, 11, 30)},
                new() { NAMPatient = "CCCC10101010",
                        Vaccin = vaccins[1],
                        Date = new DateTime(2024, 11, 2)},
            }.AsQueryable();

            var mockContexte = new Mock<VaccinationContext>();
            mockContexte.Setup(c => c.Doses).ReturnsDbSet(doses);

            Program p = new()
            {
                Contexte = mockContexte.Object
            };
            Dose d = new()
            {
                NAMPatient = "DDDD10101010",
                Vaccin = vaccins[0],
                Date = new DateTime(2025, 03, 27)
            };
            p.EnregistrerDose(d);

            mockContexte.Verify(m => m.Doses.Add(d), Times.Once);
            mockContexte.Verify(m => m.Doses.Add(It.IsAny<Dose>()), Times.Once);
            mockContexte.Verify(m => m.SaveChanges());
        }

    }
}