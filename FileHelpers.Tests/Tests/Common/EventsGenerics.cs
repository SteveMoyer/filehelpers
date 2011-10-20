using System;
using System.IO;
using FileHelpers;
using NUnit.Framework;

namespace FileHelpersTests.CommonTests
{
	[TestFixture]
	public class EventsGenerics:
        TestBaseHelper
	{
		FileHelperEngine<SampleType> engine;

		[Test]
		public void ReadEvents()
		{
			before = 0;
			after = 0;

			engine = new FileHelperEngine<SampleType>();
			engine.BeforeReadRecord += BeforeEvent;
			engine.AfterReadRecord += AfterEvent;

			object[] res = engine.ReadFile(BuildPath("Good", "test1.txt"));

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(4, before);
			Assert.AreEqual(4, after);
		}
		

		[Test]
		public void WriteEvents()
		{
			before = 0;
			after = 0;

            engine = new FileHelperEngine<SampleType>();

            engine.BeforeWriteRecord += engine_BeforeWriteRecord;
			engine.AfterWriteRecord += engine_AfterWriteRecord;

            SampleType[] res = new SampleType[2];

			res[0] = new SampleType();
			res[1] = new SampleType();

			res[0].Field1 = DateTime.Now.AddDays(1);
			res[0].Field2 = "je";
			res[0].Field3 = 0;

			res[1].Field1 = DateTime.Now;
			res[1].Field2 = "ho";
			res[1].Field3 = 2;

			engine.WriteString(res);

			Assert.AreEqual(2, engine.TotalRecords);
			Assert.AreEqual(2, before);
			Assert.AreEqual(2, after);

		}

		
		[Test]
		public void ReadEventsCancelAfter()
		{
			before = 0;
			after = 0;

            engine = new FileHelperEngine<SampleType>();
            engine.AfterReadRecord += AfterEvent2;

            object[] res = engine.ReadFile(BuildPath("Good", "test1.txt"));

			Assert.AreEqual(0, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(0, before);
			Assert.AreEqual(4, after);
		}

		[Test]
		public void ReadEventsCancelBefore()
		{
			before = 0;
			after = 0;

            engine = new FileHelperEngine<SampleType>();
            engine.BeforeReadRecord += BeforeEvent2;

            object[] res = engine.ReadFile(BuildPath("Good", "test1.txt"));

			Assert.AreEqual(0, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(4, before);
			Assert.AreEqual(0, after);
		}
		
		[Test]
		public void ReadEventsCancelAll()
		{
			before = 0;
			after = 0;

            engine = new FileHelperEngine<SampleType>();
            engine.BeforeReadRecord += BeforeEvent2;
			engine.AfterReadRecord += AfterEvent2;

            object[] res = engine.ReadFile(BuildPath("Good", "test1.txt"));

			Assert.AreEqual(0, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(4, before);
			Assert.AreEqual(0, after);
		}
		
		int before = 0;
		int after = 0;

		private void BeforeEvent(EngineBase sender, BeforeReadRecordEventArgs<SampleType> e)
		{
			if (e.RecordLine.StartsWith(" ") || e.RecordLine.StartsWith("-"))
				e.SkipThisRecord = true;

			before++;
		}

        private void AfterEvent(EngineBase sender, AfterReadRecordEventArgs<SampleType> e)
		{
			after++;
		}

        private void engine_BeforeWriteRecord(EngineBase sender, BeforeWriteRecordEventArgs<SampleType> e)
		{
			before++;
		}

        private void engine_AfterWriteRecord(EngineBase sender, AfterWriteRecordEventArgs<SampleType> e)
		{
			after++;
		}

        private void AfterEvent2(EngineBase sender, AfterReadRecordEventArgs<SampleType> e)
		{
			e.SkipThisRecord = true;
			after++;
		}

        private void BeforeEvent2(EngineBase sender, BeforeReadRecordEventArgs<SampleType> e)
		{
			e.SkipThisRecord = true;
			before++;
		}

	}
}