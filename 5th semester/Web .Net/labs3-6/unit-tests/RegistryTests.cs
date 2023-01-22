using BLL;
using DAL;
using System;
using Xunit;

namespace unit_tests
{
    public class RegistryTests
    {
        [Fact]
        public void ConstructorTest()
        {
            //ARRANGE
            const string doctorName = "Alex";
            const string patientName = "Max";

            //ACT
            Registry registry = new Registry(patientName, doctorName);
            string registryAccountName = registry.patientName;
            string registryAccountDoctorName = registry.doctorName;
            VisitsRepository repo = registry.db;

            //ASSERT
            Assert.Equal(patientName, registryAccountName);
            Assert.Equal(doctorName, registryAccountDoctorName);
            Assert.NotNull(repo);
        }

        [Fact]
        public void ScheduleTest()
        {
            //ARRANGE
            string doctorName = "Doc" + DateTime.Now.Ticks;
            string patientName = "Man" + DateTime.Now.Ticks;
            DateTime tommorow = DateTime.Now.AddDays(1);

            //ACT
            Registry registry = new Registry(patientName, doctorName);
            string result1 = registry.Schedule();
            registry.SignUp(tommorow.Month + "/" + tommorow.Day + "/" + 
                tommorow.Year, tommorow.Hour + ":" + tommorow.Minute);
            string result2 = registry.Schedule();
            int dif = ((tommorow.Minute / 20) * 20) - tommorow.Minute;
            tommorow = tommorow.AddMinutes(dif);
            tommorow = tommorow.AddMilliseconds(-tommorow.Millisecond);
            tommorow = tommorow.AddSeconds(-tommorow.Second);

            //ASSERT
            Assert.Equal("There is empty here.  .  .\n", result1);
            if (tommorow.Hour < 8 || tommorow.Hour >= 18) Assert.Equal("WRONG: time, " +
                "should be in range between 08:00 and 17:40\n", result2); 
            else Assert.Contains("|| DATE: " + tommorow.ToString() + " || DOCTOR: " +
                        doctorName + "\n", result2);
        }

        [Fact]
        public void GetMedCardTest()
        {
            //ARRANGE
            string doctorName = "Doc" + DateTime.Now.Ticks;
            string patientName = "Man" + DateTime.Now.Ticks;
            DateTime tommorow = DateTime.Now.AddDays(1);
            DateTime yesterday = DateTime.Now.AddDays(-1);

            //ACT
            Registry registry = new Registry(patientName, doctorName);
            string result1 = registry.GetMedCard();
            registry.SignUp(tommorow.Month + "/" + tommorow.Day + "/" + 
                tommorow.Year, tommorow.Hour + ":" + tommorow.Minute);
            registry.SignUp(yesterday.Month + "/" + yesterday.Day + "/" +
                yesterday.Year, yesterday.Hour + ":" + yesterday.Minute);
            string result2 = registry.GetMedCard();
            int dif = ((tommorow.Minute / 20) * 20) - tommorow.Minute;
            tommorow = tommorow.AddMinutes(dif);
            tommorow = tommorow.AddMilliseconds(-tommorow.Millisecond);
            tommorow = tommorow.AddSeconds(-tommorow.Second);

            //ASSERT
            Assert.Equal("There is empty here.  .  .\n", result1);
            if (tommorow.Hour < 8 || tommorow.Hour >= 18) 
                Assert.Equal("WRONG: time, should be in range between 08:00 and 17:40\n", result2);
            else Assert.Contains("|| DATE: " + tommorow.ToString() + " || DOCTOR: " +
                        doctorName + " || DIAGNOSIS: \n", result2);
        }

        [Fact]
        public void ListTest()
        {
            //ARRANGE
            string doctorName = "Doc" + DateTime.Now.Ticks;
            string patientName = "Man" + DateTime.Now.Ticks;
            DateTime date = new DateTime(2022, 12, 2, 13, 20, 0);

            //ACT
            Registry registry = new Registry(patientName, doctorName);
            registry.SignUp(date.Month + "/" + date.Day + "/" +
                date.Year, date.Hour + ":" + date.Minute);
            string result = registry.List();

            //ASSERT
            Assert.Contains("|| DOCTOR: " + doctorName, result);
        }

        [Fact]
        public void SetCurrentPatientAndDoctorTest()
        {
            //ARRANGE
            const string doctorName = "Doc";
            const string patientName = "Man";
            const string newDoctorName = "Maria";
            const string newPatientName = "Paul";

            //ACT
            Registry registry = new Registry(patientName, doctorName);
            string registryStartAccountName = registry.patientName;
            string registryStartAccountDoctorName = registry.doctorName;
            registry.SetCurrentDoctor(newDoctorName);
            registry.SetCurrentPatient(newPatientName);
            string registryEndAccountName = registry.patientName;
            string registryEndAccountDoctorName = registry.doctorName;

            //ASSERT
            Assert.Equal(patientName, registryStartAccountName);
            Assert.Equal(doctorName, registryStartAccountDoctorName);
            Assert.Equal(newPatientName, registryEndAccountName);
            Assert.Equal(newDoctorName, registryEndAccountDoctorName);
        }
    }

}
