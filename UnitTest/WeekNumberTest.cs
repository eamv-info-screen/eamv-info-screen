using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils;

namespace UnitTest {

    [TestClass]
    public class WeekNumberTest {
        private WeekNumber weekNumber = new WeekNumber();

        [TestMethod]
        public void WeekNumber1() {
            DateTime date = new DateTime(2017, 01, 02);
            int result = weekNumber.GetISO8601WeekNumber(date);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void WeekNumber2() {
            DateTime date = new DateTime(2017, 01, 01);
            int result = weekNumber.GetISO8601WeekNumber(date);
            Assert.AreEqual(52, result);
        }

    }
}
