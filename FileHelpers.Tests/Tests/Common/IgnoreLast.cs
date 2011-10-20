using System;
using System.IO;
using FileHelpers;
using NUnit.Framework;
using System.Collections;

namespace FileHelpersTests.CommonTests
{
	[TestFixture]
	public class IgnoreLasts
	{
		FileHelperEngine engine;
		FileHelperAsyncEngine asyncEngine;

		[Test]
		public void DiscardLast1()
		{
			engine = new FileHelperEngine(typeof (DiscardLastType0));

			DiscardLastType0[] res = (DiscardLastType0[]) Common.ReadTest(engine, @"Good\DiscardLast0.txt");

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
		}

		[Test]
		public void DiscardLast2()
		{
			engine = new FileHelperEngine(typeof (DiscardLastType1));

			DiscardLastType1[] res = (DiscardLastType1[]) Common.ReadTest(engine, @"Good\DiscardLast1.txt");

            Assert.AreEqual(engine.TotalRecords, res.Length);

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
		}

        [Test]
        public void DiscardLast2bis()
        {
            engine = new FileHelperEngine(typeof(DiscardLastType1bis));

            DiscardLastType1bis[] res = (DiscardLastType1bis[])Common.ReadTest(engine, @"Good\DiscardLast1.txt");

            Assert.AreEqual(engine.TotalRecords, res.Length);

            Assert.AreEqual(3, res.Length);
            Assert.AreEqual(new DateTime(1314, 11, 10), res[0].Field1);
        }

        [Test]
		public void DiscardLast3()
		{
			engine = new FileHelperEngine(typeof (DiscardLastType11));

			DiscardLastType11[] res = (DiscardLastType11[]) Common.ReadTest(engine, @"Good\DiscardLast1.txt");

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
		}

		[Test]
		public void DiscardLast4()
		{
			engine = new FileHelperEngine(typeof (DiscardLastType2));

			DiscardLastType2[] res = (DiscardLastType2[]) Common.ReadTest(engine, @"Good\DiscardLast2.txt");

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
			Assert.AreEqual("you can get this lines\r\nwith the FileHelperEngine.FooterText property\r\n", engine.FooterText);
		}

		[Test]
		public void DiscardLast5()
		{
			engine = new FileHelperEngine(typeof (DiscardLastType2));

			DiscardLastType2[] res = (DiscardLastType2[]) Common.ReadTest(engine, @"Good\DiscardLast3.txt");

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
		}

		[Test]
		public void DiscardLast6()
		{
			asyncEngine = new FileHelperAsyncEngine(typeof (DiscardLastType2));

			Common.BeginReadTest(asyncEngine, @"Good\DiscardLast2.txt");

            ArrayList arr = new ArrayList();
            while (asyncEngine.ReadNext() != null)
            {
                arr.Add(asyncEngine.LastRecord);
            }

            Assert.AreEqual(4, asyncEngine.TotalRecords);
            
			Assert.AreEqual("you can get this lines\r\nwith the FileHelperEngine.FooterText property\r\n", asyncEngine.FooterText);

            Assert.AreEqual(new DateTime(1314, 12, 11), ((DiscardLastType2)arr[0]).Field1);

		}


		[Test]
		public void DiscardWriteRead()
		{
			engine = new FileHelperEngine(typeof (DiscardLastType1));

			DiscardLastType1[] res = (DiscardLastType1[]) Common.ReadTest(engine, @"Good\DiscardLast1.txt");
			engine.FooterText = "This is a new Footer....\r\n";

			engine.WriteFile("tempo.txt", res);
			
            engine.FooterText = "none none";

			DiscardLastType1[] res2 = (DiscardLastType1[]) engine.ReadFile(@"tempo.txt");

			Assert.AreEqual(res.Length, res2.Length);
			Assert.AreEqual("This is a new Footer....\r\n", engine.FooterText);

			if (File.Exists("tempo.txt")) File.Delete("tempo.txt");

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
		}

        [Test]
        public void DiscardWriteRead2()
        {
            engine = new FileHelperEngine(typeof(DiscardLastType1));

            DiscardLastType1[] res = (DiscardLastType1[])Common.ReadTest(engine, @"Good\DiscardLast1.txt");


            asyncEngine = new FileHelperAsyncEngine(typeof(DiscardLastType1));

            asyncEngine.FooterText = "This is a new Footer....";

            asyncEngine.BeginWriteFile("temp.txt");
            asyncEngine.WriteNexts(res);
            asyncEngine.Close();

            asyncEngine.FooterText = "none none";

            asyncEngine.BeginReadFile("temp.txt");

            while (asyncEngine.ReadNext() != null)
            {}

            Assert.AreEqual(res.Length, asyncEngine.TotalRecords);
            Assert.AreEqual("This is a new Footer....\r\n", asyncEngine.FooterText);

            asyncEngine.Close();

            Assert.AreEqual(res.Length, asyncEngine.TotalRecords);
            Assert.AreEqual("This is a new Footer....\r\n", asyncEngine.FooterText);

            if (File.Exists("tempo.txt")) File.Delete("tempo.txt");
        }



	}

	[FixedLengthRecord]
	[IgnoreLast(0)]
    [IgnoreFirst(0)]
    public class DiscardLastType0
	{
		[FieldFixedLength(8)]
		[FieldConverter(ConverterKind.Date, "ddMMyyyy")] public DateTime Field1;

		[FieldFixedLength(3)] public string Field2;

		[FieldFixedLength(3)]
		[FieldConverter(ConverterKind.Int32)] public int Field3;

	}

	[FixedLengthRecord]
    [IgnoreLast()]
	public class DiscardLastType1
	{
		[FieldFixedLength(8)]
		[FieldConverter(ConverterKind.Date, "ddMMyyyy")] public DateTime Field1;

		[FieldFixedLength(3)] public string Field2;

		[FieldFixedLength(3)]
		[FieldConverter(ConverterKind.Int32)] public int Field3;

	}

    [FixedLengthRecord]
    [IgnoreFirst()]
    [IgnoreLast()]
    public class DiscardLastType1bis
    {
        [FieldFixedLength(8)]
        [FieldConverter(ConverterKind.Date, "ddMMyyyy")]
        public DateTime Field1;

        [FieldFixedLength(3)]
        public string Field2;

        [FieldFixedLength(3)]
        [FieldConverter(ConverterKind.Int32)]
        public int Field3;

    }

	[FixedLengthRecord]
	[IgnoreLast(1)]
	public class DiscardLastType11
	{
		[FieldFixedLength(8)]
		[FieldConverter(ConverterKind.Date, "ddMMyyyy")] public DateTime Field1;

		[FieldFixedLength(3)] public string Field2;

		[FieldFixedLength(3)]
		[FieldConverter(ConverterKind.Int32)] public int Field3;

	}

	[FixedLengthRecord]
	[IgnoreLast(2)]
	public class DiscardLastType2
	{
		[FieldFixedLength(8)]
		[FieldConverter(ConverterKind.Date, "ddMMyyyy")] public DateTime Field1;

		[FieldFixedLength(3)] public string Field2;

		[FieldFixedLength(3)]
		[FieldConverter(ConverterKind.Int32)] public int Field3;

	}

}