using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PassengerNameListWebApi.BusinessLogic;

namespace PassengerNameListWebApi.Tests
{
    [TestClass]
    public class RecordParserTest
    {

        [TestMethod]
        public void Should_ignore_record_starting_with_dot_R()
        {
            var record = ".R/TKNE";
            var actual = RecordParser.IsIgnoreLine(record);
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void Should_locate_record_index_at_18()
        {
            var record = "1ARNOLD/NIGELMR-B2 .L/LVGVUP";
            var actual = RecordParser.ValidRecordLocator(record, out int foundIndex);
            Assert.AreEqual(true, actual);
            Assert.AreEqual(18, foundIndex);
        }

        [TestMethod]
        public void Should_not_locate_record_when_format_is_wrong()
        {
            var record = "1ARNOLD/NIGELMR-B2.L/LVGVUP";
            var actual = RecordParser.ValidRecordLocator(record, out int foundIndex);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void Should_not_locate_record_when_record_incomplete()
        {
            var record = " .L/LVGVUP";
            var actual = RecordParser.ValidRecordLocator(record, out int foundIndex);
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void Should_parse_correctly_when_correct_input_is_given()
        {
            var record = "1ARNOLD/NIGELMR-B2 .L/LVGVUP";
            var recordParser = new RecordParser();
            recordParser.ParseRecord(record, out string passengerName, out string recordLocator);
            Assert.AreEqual( "ARNOLD/NIGELMR", passengerName);
            Assert.AreEqual("LVGVUP", recordLocator);

        }
        [TestMethod]
        public void Should_require_to_trim_when_name_ends_with_dash_b2()
        {
            var name = "1ARNOLD/NIGELMR-B2";
            var actual = RecordParser.RequireToTrimPassengerName(name);

        }
    }
}
