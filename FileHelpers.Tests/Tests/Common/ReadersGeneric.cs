#if NET_2_0

using System;
using System.Collections;
using System.Data;
using System.IO;
using FileHelpers;
using NUnit.Framework;

namespace FileHelpersTests.CommonTests
{
	[TestFixture]
	public class ReadersGenerics
	{

		[Test]
		public void ReadFile()
		{
			FileHelperEngine<SampleType> engine = new FileHelperEngine<SampleType>();

			SampleType[] res;
			res = engine.ReadFile(Common.TestPath(@"Good\test1.txt"));

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(0, engine.ErrorManager.ErrorCount);

			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
			Assert.AreEqual("901", res[0].Field2);
			Assert.AreEqual(234, res[0].Field3);

			Assert.AreEqual(new DateTime(1314, 11, 10), res[1].Field1);
			Assert.AreEqual("012", res[1].Field2);
			Assert.AreEqual(345, res[1].Field3);

		}


		[Test]
		public void ReadFileStatic()
		{
			SampleType[] res;
            res = CommonEngine.ReadFile <SampleType>(Common.TestPath(@"Good\test1.txt"));

			Assert.AreEqual(4, res.Length);

			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
			Assert.AreEqual("901", res[0].Field2);
			Assert.AreEqual(234, res[0].Field3);

			Assert.AreEqual(new DateTime(1314, 11, 10), res[1].Field1);
			Assert.AreEqual("012", res[1].Field2);
			Assert.AreEqual(345, res[1].Field3);

		}


		[Test]
		public void AsyncRead()
		{

            FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>();

			SampleType rec1, rec2;
            asyncEngine.BeginReadFile(Common.TestPath(@"Good\test1.txt"));

			rec1 = asyncEngine.ReadNext();
			Assert.IsNotNull(rec1);
			rec2 = asyncEngine.ReadNext();
			Assert.IsNotNull(rec1);

			Assert.IsTrue(rec1 != rec2);

			rec1 = asyncEngine.ReadNext();
			Assert.IsNotNull(rec2);
			rec1 = asyncEngine.ReadNext();
			Assert.IsNotNull(rec2);

			Assert.IsTrue(rec1 != rec2);

			Assert.AreEqual(0, asyncEngine.ErrorManager.ErrorCount);

            asyncEngine.Close();

		}

		[Test]
		public void AsyncReadMoreAndMore()
		{
            FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>();
            SampleType rec1;
            asyncEngine.BeginReadFile(Common.TestPath(@"Good\test1.txt"));

			rec1 = asyncEngine.ReadNext();
			rec1 = asyncEngine.ReadNext();
			rec1 = asyncEngine.ReadNext();
			rec1 = asyncEngine.ReadNext();
			rec1 = asyncEngine.ReadNext();

			Assert.IsTrue(rec1 == null);

			rec1 = (SampleType) asyncEngine.ReadNext();
			Assert.AreEqual(0, asyncEngine.ErrorManager.ErrorCount);

            asyncEngine.Close();
		}


		[Test]
		public void AsyncRead2()
		{
			SampleType rec1;

            FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>();
            asyncEngine.BeginReadFile(Common.TestPath(@"Good\test1.txt"));

            int lineAnt = asyncEngine.LineNumber;
			while (asyncEngine.ReadNext() != null)
			{
				rec1 = asyncEngine.LastRecord;
				Assert.IsNotNull(rec1);
				Assert.AreEqual(lineAnt + 1, asyncEngine.LineNumber);
				lineAnt = asyncEngine.LineNumber;
			}

			Assert.AreEqual(4, asyncEngine.TotalRecords);
			Assert.AreEqual(0, asyncEngine.ErrorManager.ErrorCount);
            asyncEngine.Close();
		}

		[Test]
		public void AsyncReadEnumerable()
		{
            FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>();
            asyncEngine.BeginReadFile(Common.TestPath(@"Good\test1.txt"));

			int lineAnt = asyncEngine.LineNumber;
			
			foreach (SampleType rec1 in asyncEngine)
			{
				Assert.IsNotNull(rec1);
				Assert.AreEqual(lineAnt + 1, asyncEngine.LineNumber);
				lineAnt = asyncEngine.LineNumber;
			}

			Assert.AreEqual(4, asyncEngine.TotalRecords);
			Assert.AreEqual(0, asyncEngine.ErrorManager.ErrorCount);

            asyncEngine.Close();

		}

		[Test]
		[ExpectedException(typeof(FileHelpersException))]
		public void AsyncReadEnumerableBad()
		{
            FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>();

			foreach (SampleType rec1 in asyncEngine)
			{
				rec1.ToString();
			}
		}

		[Test]
		public void AsyncReadEnumerable2()
		{
            using (FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>())
			{
                asyncEngine.BeginReadFile(Common.TestPath(@"Good\test1.txt"));

                int lineAnt = asyncEngine.LineNumber;
			
				foreach (SampleType rec1 in asyncEngine)
				{
					Assert.IsNotNull(rec1);
					Assert.AreEqual(lineAnt + 1, asyncEngine.LineNumber);
					lineAnt = asyncEngine.LineNumber;
				}

                Assert.AreEqual(4, asyncEngine.TotalRecords);
                Assert.AreEqual(0, asyncEngine.ErrorManager.ErrorCount);
            }

		}

		[Test]
		public void AsyncReadEnumerableAutoDispose()
		{

            FileHelperAsyncEngine<SampleType> asyncEngine = new FileHelperAsyncEngine<SampleType>();
            asyncEngine.BeginReadFile(Common.TestPath(@"Good\test1.txt"));
			
			asyncEngine.ReadNext();
			asyncEngine.ReadNext();
		}
		
		[Test]
		public void ReadStream()
		{
			string data = "11121314901234" + Environment.NewLine +
				"10111314012345" + Environment.NewLine +
				"11101314123456" + Environment.NewLine +
				"10101314234567" + Environment.NewLine;

            FileHelperEngine<SampleType> engine = new FileHelperEngine<SampleType>();

			SampleType[] res;
			res = engine.ReadStream(new StringReader(data));

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(0, engine.ErrorManager.ErrorCount);

			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
			Assert.AreEqual("901", res[0].Field2);
			Assert.AreEqual(234, res[0].Field3);

			Assert.AreEqual(new DateTime(1314, 11, 10), res[1].Field1);
			Assert.AreEqual("012", res[1].Field2);
			Assert.AreEqual(345, res[1].Field3);

		}

		[Test]
		public void ReadString()
		{
			string data = "11121314901234" + Environment.NewLine +
				"10111314012345" + Environment.NewLine +
				"11101314123456" + Environment.NewLine +
				"10101314234567" + Environment.NewLine;

            FileHelperEngine<SampleType> engine = new FileHelperEngine<SampleType>();

            SampleType[] res;
            res = engine.ReadFile(Common.TestPath(@"Good\test1.txt"));

			res =engine.ReadString(data);

			Assert.AreEqual(4, res.Length);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(0, engine.ErrorManager.ErrorCount);

			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
			Assert.AreEqual("901", res[0].Field2);
			Assert.AreEqual(234, res[0].Field3);

			Assert.AreEqual(new DateTime(1314, 11, 10), res[1].Field1);
			Assert.AreEqual("012", res[1].Field2);
			Assert.AreEqual(345, res[1].Field3);

		}


		[Test]
		public void ReadStringStatic()
		{
			string data = "11121314901234" + Environment.NewLine +
				"10111314012345" + Environment.NewLine +
				"11101314123456" + Environment.NewLine +
				"10101314234567" + Environment.NewLine;

			SampleType[] res;
			res = CommonEngine.ReadString<SampleType>(data);

			Assert.AreEqual(4, res.Length);

			Assert.AreEqual(new DateTime(1314, 12, 11), res[0].Field1);
			Assert.AreEqual("901", res[0].Field2);
			Assert.AreEqual(234, res[0].Field3);

			Assert.AreEqual(new DateTime(1314, 11, 10), res[1].Field1);
			Assert.AreEqual("012", res[1].Field2);
			Assert.AreEqual(345, res[1].Field3);

		}


		[Test]
		public void ReadEmpty()
		{
			string data = "";

            FileHelperEngine<SampleType> engine = new FileHelperEngine<SampleType>();

			SampleType[] res;
			res = engine.ReadStream(new StringReader(data));

			Assert.AreEqual(0, res.Length);
			Assert.AreEqual(0, engine.TotalRecords);
			Assert.AreEqual(0, engine.ErrorManager.ErrorCount);

		}

		[Test]
		public void ReadEmptyStream()
		{
            FileHelperEngine<SampleType> engine = new FileHelperEngine<SampleType>();

            SampleType[] res;
            res = engine.ReadFile(Common.TestPath(@"Good\testempty.txt"));

			Assert.AreEqual(0, res.Length);
			Assert.AreEqual(0, engine.TotalRecords);
			Assert.AreEqual(0, engine.ErrorManager.ErrorCount);

		}

		
		[Test]
		public void ReadFileAsDataTable()
		{
            FileHelperEngine<SampleType> engine = new FileHelperEngine<SampleType>();

			DataTable res;
			res = engine.ReadFileAsDT(Common.TestPath(@"Good\test1.txt"));

			Assert.AreEqual(4, res.Rows.Count);
			Assert.AreEqual(4, engine.TotalRecords);
			Assert.AreEqual(0, engine.ErrorManager.ErrorCount);

			Assert.AreEqual(new DateTime(1314, 12, 11), res.Rows[0]["Field1"]);
			Assert.AreEqual("901", res.Rows[0]["Field2"]);
			Assert.AreEqual(234, res.Rows[0]["Field3"]);

			Assert.AreEqual(new DateTime(1314, 11, 10), res.Rows[1]["Field1"]);
			Assert.AreEqual("012", res.Rows[1]["Field2"]);
			Assert.AreEqual(345, res.Rows[1]["Field3"]);

		}

	}
}

#endif