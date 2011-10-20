using FileHelpers;
using NUnit.Framework;

namespace FileHelpersTests.Errors
{
	[TestFixture]
	public class BadFormat
	{
		FileHelperEngine engine;

		[SetUp]
		public void Setup()
		{
			engine = new FileHelperEngine(typeof (SampleType));
		}

		[Test]
		[ExpectedException(typeof (ConvertException))]
		public void BadDate1()
		{
			Common.ReadTest(engine, @"Bad\BadDate1.txt");
		}

		[Test]
		[ExpectedException(typeof (ConvertException))]
		public void BadDate2()
		{
			Common.ReadTest(engine, @"Bad\BadDate2.txt");
		}

		[Test]
		[ExpectedException(typeof (ConvertException))]
		public void BadInt1()
		{
			Common.ReadTest(engine, @"Bad\BadInt1.txt");
		}

		[Test]
		[ExpectedException(typeof (ConvertException))]
		public void BadInt2()
		{
			Common.ReadTest(engine, @"Bad\BadInt2.txt");
		}

		[Test]
		[ExpectedException(typeof (ConvertException))]
		public void BadInt3()
		{
			engine = new FileHelperEngine(typeof (SampleTypeInt));
			Common.ReadTest(engine, @"Bad\BadInt3.txt");
		}

		[Test]
		[ExpectedException(typeof (ConvertException))]
		public void BadInt4()
		{
			engine = new FileHelperEngine(typeof (SampleTypeInt));
			Common.ReadTest(engine, @"Bad\BadInt4.txt");
		}

		[Test]
		//[ExpectedException(typeof (ConvertException))]
		public void NoPendingNullValue()
		{
			engine = new FileHelperEngine(typeof (SampleType));
			Common.ReadTest(engine, @"Bad\NoBadNullValue.txt");
		}

	}
}